using OnBarcode.Barcode.BarcodeScanner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WirelessPackageTracker
{
    public partial class Form1 : Form
    {
        private Settings settingsObj;
        private Worker workerObj;
        private Webserver ws;
        private BackgroundWorker bgwImageStream;
        private TcpClient clientSocket;
        private string tempFolder;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tempFolder = Application.StartupPath + "\\temp";
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
            CleanTempFolder();

            this.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            LoadSettings();
            if (settingsObj.ServiceAutostart)
                buttonServiceStatusStart_Click(buttonServiceStatusStart, new EventArgs());
        }

        private void CleanTempFolder()
        {
            try
            {
                foreach (string file in Directory.GetFiles(tempFolder))
                    File.Delete(file);
            }
            catch { }
        }

        private void LoadSettings()
        {
            string[] settingKeys = new string[] { "sd_ip", "sd_path", "service_auto", "service_path" };
            settingsObj = new Settings();
            bool allSet = true;
            foreach (string key in settingKeys)
            {
                string val = SettingsHelper.GetAppSetting(key);
                if (string.IsNullOrEmpty(val))
                    allSet = false;               
            }

           

            textBoxSettingsIP.Text = settingsObj.SdIp = SettingsHelper.GetAppSetting("sd_ip");                
            textBoxSettingsSDPath.Text = settingsObj.SdPath = SettingsHelper.GetAppSetting("sd_path");
            checkBoxSettingsServiceAutostart.Checked = settingsObj.ServiceAutostart = Convert.ToBoolean(SettingsHelper.GetAppSetting("service_auto"));
            textBoxSettingsServicePath.Text = settingsObj.ServicePath = SettingsHelper.GetAppSetting("service_path");

            if (!allSet)
            {
                tabControlMain.TabPages.Remove(tabPageService);
                MessageBox.Show("Bitte setzen Sie zuerst alle Einstellungen, bevor Sie den Service nutzen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (tabControlMain.TabPages.Count == 1)
                    tabControlMain.TabPages.Insert(0,tabPageService);
            }
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBoxSettingsServicePath.Text))
            {
                MessageBox.Show("Ungültiges Serververzeichnis. Einstellungen werden nicht gespeichert!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Directory.CreateDirectory(textBoxSettingsServicePath.Text.TrimEnd('\\') + "\\test_dir");
                Directory.Delete(textBoxSettingsServicePath.Text.TrimEnd('\\') + "\\test_dir");
            }
            catch (Exception ee)
            {
                MessageBox.Show("Keine ausreichenden Schreibrechte im Serververzeichnis. Einstellungen werden nicht gespeichert!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SettingsHelper.SetAppSetting("sd_ip", textBoxSettingsIP.Text);
            SettingsHelper.SetAppSetting("sd_path", textBoxSettingsSDPath.Text.TrimStart('/'));
            SettingsHelper.SetAppSetting("service_auto", checkBoxSettingsServiceAutostart.Checked.ToString());
            SettingsHelper.SetAppSetting("service_path", textBoxSettingsServicePath.Text.TrimEnd('\\'));
            MessageBox.Show("Die Einstellungen wurden gespeichert.", "Erfolgreich abgeschlossen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadSettings();
        }

        private void buttonSettingsServicePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBoxSettingsServicePath.Text = fbd.SelectedPath;
        }

        private void WebserverStop()
        {
            try
            {
                if (ws != null)
                    ws.Stop();
            }
            catch { }
            finally
            {
                PrintLog("Entferne Rechte für Webserver.");
                PrintLog(RunCmdAsAdmin("netsh http delete urlacl url=http://" + settingsObj.ServiceIp + ":8080/"));
            }
        }

        private void WebserverStart()
        {
            IPHostEntry host;
            settingsObj.ServiceIp = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    settingsObj.ServiceIp = ip.ToString();
                }
            }

            if (settingsObj.ServiceIp == "?")
            {
                MessageBox.Show("Konnte Webserver nicht starten, da keine lokale IP ermittelt werden konnte.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PrintLog("Hole Rechte für Webserver.");
                PrintLog(RunCmdAsAdmin("netsh http add urlacl url=http://" + settingsObj.ServiceIp + ":8080/ user=Jeder"));

                ws = new Webserver(SendResponse, "http://" + settingsObj.ServiceIp + ":8080/");
                ws.Run();
                PrintLog("Webserver läuft auf: " + settingsObj.ServiceIp +":8080");
            }
        }

        private bool CheckIfSdIsUp()
        {
            bool res = false;
            using (Ping sender = new Ping())
            {
                try
                {
                    PingReply pingResult = sender.Send(settingsObj.SdIp);
                    res = (pingResult.Status == IPStatus.Success);
                }
                catch (Exception ee)
                {
                    PrintLog(ee.Message);
                    res = false;
                }                
            }
            return res;
        }

        public string SendResponse(HttpListenerRequest request)
        {
            string reqPath = (request.RawUrl.Contains('?') ? request.RawUrl.Split('?')[0] : request.RawUrl).TrimEnd('/');
            string resp = "ERROR";
            if (reqPath == "/set/start")
            {
                CleanTempFolder();
                workerObj = new Worker() { takeFnames = new List<TakeFile>(), takeLastAmount = 0 };
                workerObj.Status = ProcessStep.Initial;

                if (CheckIfSdIsUp())
                {
                    StartSdStreamClient();
                    workerObj.Status = ProcessStep.BcodeGet;
                    resp = "OK";
                }                    
                else
                    PrintLog("SD-Karten Ping fehlgeschlagen.");

                
            }
            else if (reqPath == "/get/code")
            {               
                if (!string.IsNullOrEmpty(workerObj.LastPicBcodeName))
                {                   
                    BackgroundWorker bgwBcode = new BackgroundWorker();
                    bgwBcode.DoWork += bgw_DoWork;                   
                    bgwBcode.RunWorkerAsync("bCode_" + Guid.NewGuid().ToString());
                    resp = "ANALYZE";
                }
            }
            else if (reqPath == "/get/code/detail")
            {
                if (!string.IsNullOrEmpty(workerObj.respCode))
                    resp = workerObj.respCode;
            }
            else if (reqPath == "/set/code/reset")
            {
                workerObj.respCode = string.Empty;
                workerObj.Status = ProcessStep.BcodeGet;
            }
            else if (reqPath == "/set/take")
            {
                workerObj.Status = ProcessStep.Initial;               
                StartSdStreamClient();
                workerObj.takeFolder = CreateTakePath(request.QueryString["take_id"]);
                workerObj.Status = ProcessStep.TakePictures;
                resp = "OK";
            }
            else if (reqPath == "/get/takevalid")
            {
                if (!Directory.Exists(CreateTakePath(request.QueryString["take_id"])))
                    resp = "OK";
            }
            else if (reqPath == "/get/takelist")
            {
                if (workerObj.takeLastAmount < workerObj.takeFnames.Count() && workerObj.takeFnames.Where((x, i) => i >= workerObj.takeLastAmount && x.Valid).Count() > 0)
                {                    
                    resp = "OK";
                    foreach (var tFile in workerObj.takeFnames.Where((x, i) => i >= workerObj.takeLastAmount))
                    {
                        resp += "http://" + settingsObj.ServiceIp + ":8080/get/take?img=" + tFile.FileName + ".jpg;" + tFile.FileName + "|";
                    }
                    workerObj.takeLastAmount = workerObj.takeFnames.Count();
                    resp = resp.TrimEnd('|');
                }
            }
            else if (reqPath == "/get/take")
            {
                resp = "getimg;" + tempFolder + "\\" + request.QueryString["img"];
            }
            else if (reqPath == "/set/take/delete")
            {
                List<TakeFile> tmpLst = new List<TakeFile>();
                foreach (var file in workerObj.takeFnames)
                {
                    if (file.FileName == request.QueryString["take_id"])
                        tmpLst.Add(new TakeFile() { FileName = file.FileName, Valid = false });
                    else
                        tmpLst.Add(file);
                }
                workerObj.takeFnames = tmpLst;
                resp = "OK";
            }
            else if (reqPath == "/set/finalize")
            {
                workerObj.Status = ProcessStep.Finalize;
                try
                {
                    Directory.CreateDirectory(workerObj.takeFolder);
                    foreach (var file in workerObj.takeFnames.Where((x, i) => x.Valid))
                    {
                        var tFileName = file.FileName + ".jpg";
                        File.Move(tempFolder + "\\" + tFileName, workerObj.takeFolder + "\\" + tFileName);
                    }
                    resp = "OK";
                }         
                catch (Exception ee)
                {
                    PrintLog(ee.Message);
                }
            }
            
            return resp;
        }


        private string CreateTakePath(string takeid)
        {
            return settingsObj.ServicePath + "\\" + CleanFileFolderPath(takeid);
        }

        private string CleanFileFolderPath(string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "");
            }
            return path;
        }

        private void StartSdStreamClient()
        {
            if (bgwImageStream == null || !bgwImageStream.IsBusy || !clientSocket.Connected)
            {
                bgwImageStream = new BackgroundWorker();
                bgwImageStream.WorkerSupportsCancellation = true;
                bgwImageStream.DoWork += bgwImageStream_DoWork;
                bgwImageStream.RunWorkerAsync();
            }
        }

        void bgwImageStream_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                clientSocket = new TcpClient();                
                NetworkStream serverStream = default(NetworkStream);
                clientSocket.Connect(settingsObj.SdIp, 5566);
                PrintLog("Connected to SD-ImageStream server.");
                var bgw = sender as BackgroundWorker;    
                while (!bgw.CancellationPending)
                {
                    serverStream = clientSocket.GetStream();
                    int buffSize = 0;
                    byte[] inStream = new byte[65536];
                    buffSize = clientSocket.ReceiveBufferSize;
                       
                    serverStream.Read(inStream, 0, buffSize);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream).Trim('\0');
                    PrintLog("Foto erkannt: " + returndata + "\r\n\tFreigegeben für: " + workerObj.Status.ToString());

                    string imageName = string.Empty;
                    if (returndata.Contains(settingsObj.SdPath))
                        imageName = returndata.Substring(returndata.IndexOf(settingsObj.SdPath) + settingsObj.SdPath.Length).Trim();

                    if (imageName.Length > 0 && workerObj.Status == ProcessStep.BcodeGet)
                    {
                        workerObj.LastPicBcodeName = imageName;
                        workerObj.Status = ProcessStep.BcodeAnalyze;                        
                    }
                    else if (imageName.Length > 0 && workerObj.Status == ProcessStep.TakePictures)
                    {
                        BackgroundWorker bgwBcode = new BackgroundWorker();
                        bgwBcode.DoWork += bgw_DoWork;
                        bgwBcode.RunWorkerCompleted +=bgwBcode_RunWorkerCompleted;
                        var fName = Guid.NewGuid().ToString();
                        bgwBcode.RunWorkerAsync(fName + ";" + imageName);
                    }
                }
                serverStream.Close();
                clientSocket.Close();
                
                PrintLog("Disconnected from SD-ImageStream server.");
            }
            catch (Exception ee)
            {
                if ((sender as BackgroundWorker).CancellationPending)
                    PrintLog("Disconnected from SD-ImageStream server.");
                else
                    PrintLog(ee.Message);
            }            
        }

        void bgwBcode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workerObj.Status == ProcessStep.TakePictures)
                workerObj.takeFnames.Add(new TakeFile(){ FileName = e.Result.ToString(), Valid = true });
        }


        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            string arg = (e.Argument as string);
            string tFname = tempFolder + "\\" + (workerObj.Status == ProcessStep.TakePictures ? arg.Split(';')[0] : arg) + ".jpg";

            try
            {
                using (SpecialWebClient wc = new SpecialWebClient())
                { 
                    wc.DownloadFile("http://" + settingsObj.SdIp + "/sd/" + settingsObj.SdPath.Trim('/') + "/" + (workerObj.Status == ProcessStep.TakePictures ? arg.Split(';')[1] : workerObj.LastPicBcodeName), tFname);

                    if (workerObj.Status == ProcessStep.BcodeAnalyze)
                    {
                        string[] barcodes = BarcodeScanner.Scan(tFname, BarcodeType.All);
                        if (barcodes.Length >= 1)
                        {
                            workerObj.respCode = barcodes[0] + ";" + "http://" + settingsObj.SdIp + "/cgi-bin/thumbNail?fn=/www/sd/" + settingsObj.SdPath.Trim('/') + "/" + workerObj.LastPicBcodeName;
                        }
                        else
                        {
                            workerObj.respCode = "FAIL";
                        }
                    }
                    else if (workerObj.Status == ProcessStep.TakePictures)
                        e.Result = arg.Split(';')[0];
                }
            }
            catch (Exception ee)
            {
                if (workerObj.Status == ProcessStep.BcodeAnalyze)
                    workerObj.respCode = "FAIL";                
            }

            if (workerObj.Status == ProcessStep.BcodeAnalyze)
                workerObj.LastPicBcodeName = string.Empty;            
        }

        private class SpecialWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 15 * 1000;
                if (w is HttpWebRequest)
                {
                    ((HttpWebRequest)w).KeepAlive = false;
                }
                return w;
            }
        }

        private List<string> SdFileList()
        {
            string fListUrl = "http://"+settingsObj.SdIp+"/cgi-bin/tslist?PATH=%2Fwww%2Fsd%2F" + HttpUtility.UrlEncode(settingsObj.SdPath.Trim('/'));
            SetAllowUnsafeHeaderParsing200();
            string fList = string.Empty;
            List<string> res = new List<string>();
            using (SpecialWebClient wc = new SpecialWebClient())
            { 
                try
                {
                    fList = wc.DownloadString(fListUrl);
                    Regex re = new Regex(@"FileName[0-9]+=(?<img>.+?)&");
                    MatchCollection mc = re.Matches(fList.Split('\n')[2].Trim());
                    res = new List<string>(mc.Cast<Match>().Select(m => m.Groups["img"].Value).ToArray());
                }
                catch (Exception ee)
                {
                    PrintLog(ee.Message);
                    res = new List<string>() { "FAIL" };
                }
            }
            return res;
        }

        public static bool SetAllowUnsafeHeaderParsing200()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WebserverStop();
        }

        private void RenderStatusInterface(int state)
        {
            switch (state)
            {
                case 0: 
                    buttonServiceStatusStart.Enabled = false;
                    buttonServiceStatusStop.Enabled = false;
                    labelServiceStatus.ForeColor = Color.Orange;
                    break;
                case 1:
                    buttonServiceStatusStart.Enabled = false;
                    buttonServiceStatusStop.Enabled = true;
                    labelServiceStatus.Text = "Gestartet";
                    labelServiceStatus.ForeColor = Color.Green;
                    break;
                case 2:
                    buttonServiceStatusStart.Enabled = true;
                    buttonServiceStatusStop.Enabled = false;
                    labelServiceStatus.Text = "Gestoppt";
                    labelServiceStatus.ForeColor = Color.Red;
                    break;
            } 
        }

        private void buttonServiceStatusStart_Click(object sender, EventArgs e)
        {
            RenderStatusInterface(0);
            WebserverStart();
            RenderStatusInterface(1);
        }

        private void buttonServiceStatusStop_Click(object sender, EventArgs e)
        {
            RenderStatusInterface(0);
            WebserverStop();
            if (bgwImageStream != null)
                bgwImageStream.CancelAsync();
            if (clientSocket != null)
                clientSocket.Close();
            RenderStatusInterface(2);
        }

        private void PrintLog(string msg)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                this.richTextBoxServiceLog.Text = DateTime.Now.ToUniversalTime() + ": " + msg + "\r\n" + this.richTextBoxServiceLog.Text;
            }));        
        }

        public static string RunCmdAsAdmin(string cmd)
        {

            ProcessStartInfo procStartInfo = new ProcessStartInfo()
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.GetEncoding(850),
                StandardErrorEncoding = System.Text.Encoding.GetEncoding(850),
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;               
                proc.Start();
               
                string output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                    output = proc.StandardError.ReadToEnd();
                output = output.Replace("\r\n\r\n", "").Trim('\r').Trim('\n').Trim('\r');
                return output;
            }
        }

    }

    struct Settings
    {
        public string SdIp { get; set; }
        public string SdPath { get; set; }
        public bool ServiceAutostart { get; set; }
        public string ServicePath { get; set; }
        public string ServiceIp { get; set; }
    }

    struct Worker
    {        
        public string LastPicBcodeName { get; set; }
        public string respCode { get; set; }
        public string takeFolder { get; set; }
        public int takeLastAmount { get; set; }
        public List<TakeFile> takeFnames { get; set; }
        public ProcessStep Status { get; set; }
    }

    struct TakeFile
    {        
        public string FileName { get; set; }
        public bool Valid { get; set; }
    }

    enum ProcessStep
    {
        Initial,
	    BcodeGet,
	    BcodeAnalyze,
        TakePictures,
        Finalize
    };
}
