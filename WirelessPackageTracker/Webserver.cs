using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WirelessPackageTracker
{
    public class Webserver
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public Webserver(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                PrintLog("Betriebssystem zu alt. Benötigt: Windows XP SP2, Server 2003 oder neuer.");
                throw new NotSupportedException("Betriebssystem zu alt. Benötigt: Windows XP SP2, Server 2003 oder neuer.");
            }

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            
            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public Webserver(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }
        
        private void PrintLog(string msg)
        {
            Form1 f1 = (Form1)Application.OpenForms[0];
            f1.Invoke(new MethodInvoker(delegate() {
                f1.richTextBoxServiceLog.Text = DateTime.Now.ToUniversalTime() + ": " + msg + "\r\n" + f1.richTextBoxServiceLog.Text;
            }));
           
        }
        

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                PrintLog("Webserver läuft...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                                string rstr = _responderMethod(ctx.Request);
                                if (rstr.StartsWith("getimg"))
                                {
                                    var fName = rstr.Split(';')[1];
                                    FileInfo fInfo = new FileInfo(fName);
                                    long numBytes = fInfo.Length;

                                    using (FileStream fStream = new FileStream(fName, FileMode.Open, FileAccess.Read))
                                    {
                                        using (BinaryReader br = new BinaryReader(fStream))
                                        {
                                            byte[] bOutput = br.ReadBytes((int)numBytes);

                                            br.Close();

                                            fStream.Close();

                                            ctx.Response.ContentType = "image/jpg";
                                            ctx.Response.ContentLength64 = bOutput.Length;

                                            ctx.Response.OutputStream.Write(bOutput, 0, bOutput.Length);                                            
                                        }
                                       
                                    }                                    
                                }
                                else
                                {
                                    byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                }
                            }
                            catch (Exception ee)
                            {
                                PrintLog(ee.Message);
                            } 
                            finally
                            {                               
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } 
            });
        }

        public void Stop()
        {
            PrintLog("Webserver wird gestoppt.");
            _listener.Stop();
            _listener.Close();
            PrintLog("Webserver ist gestoppt.");
        }
    }
}
