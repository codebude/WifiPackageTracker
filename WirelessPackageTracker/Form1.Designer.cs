namespace WirelessPackageTracker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageService = new System.Windows.Forms.TabPage();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBoxSettingsCard = new System.Windows.Forms.GroupBox();
            this.textBoxSettingsIP = new System.Windows.Forms.TextBox();
            this.labelSettingsSDIP = new System.Windows.Forms.Label();
            this.labelSettingsSDPath = new System.Windows.Forms.Label();
            this.textBoxSettingsSDPath = new System.Windows.Forms.TextBox();
            this.buttonSettingsSave = new System.Windows.Forms.Button();
            this.groupBoxServiceStatus = new System.Windows.Forms.GroupBox();
            this.labelServiceStatus = new System.Windows.Forms.Label();
            this.buttonServiceStatusStart = new System.Windows.Forms.Button();
            this.buttonServiceStatusStop = new System.Windows.Forms.Button();
            this.groupBoxServiceLog = new System.Windows.Forms.GroupBox();
            this.richTextBoxServiceLog = new System.Windows.Forms.RichTextBox();
            this.groupBoxSettingsService = new System.Windows.Forms.GroupBox();
            this.labelSettingsServicePath = new System.Windows.Forms.Label();
            this.textBoxSettingsServicePath = new System.Windows.Forms.TextBox();
            this.checkBoxSettingsServiceAutostart = new System.Windows.Forms.CheckBox();
            this.buttonSettingsServicePath = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageService.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBoxSettingsCard.SuspendLayout();
            this.groupBoxServiceStatus.SuspendLayout();
            this.groupBoxServiceLog.SuspendLayout();
            this.groupBoxSettingsService.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageService);
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(653, 463);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageService
            // 
            this.tabPageService.Controls.Add(this.groupBoxServiceLog);
            this.tabPageService.Controls.Add(this.groupBoxServiceStatus);
            this.tabPageService.Location = new System.Drawing.Point(4, 22);
            this.tabPageService.Name = "tabPageService";
            this.tabPageService.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageService.Size = new System.Drawing.Size(645, 437);
            this.tabPageService.TabIndex = 0;
            this.tabPageService.Text = "Service";
            this.tabPageService.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBoxSettingsService);
            this.tabPageSettings.Controls.Add(this.buttonSettingsSave);
            this.tabPageSettings.Controls.Add(this.groupBoxSettingsCard);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(645, 437);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Einstellungen";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBoxSettingsCard
            // 
            this.groupBoxSettingsCard.Controls.Add(this.labelSettingsSDPath);
            this.groupBoxSettingsCard.Controls.Add(this.textBoxSettingsSDPath);
            this.groupBoxSettingsCard.Controls.Add(this.labelSettingsSDIP);
            this.groupBoxSettingsCard.Controls.Add(this.textBoxSettingsIP);
            this.groupBoxSettingsCard.Location = new System.Drawing.Point(8, 6);
            this.groupBoxSettingsCard.Name = "groupBoxSettingsCard";
            this.groupBoxSettingsCard.Size = new System.Drawing.Size(303, 136);
            this.groupBoxSettingsCard.TabIndex = 0;
            this.groupBoxSettingsCard.TabStop = false;
            this.groupBoxSettingsCard.Text = "SD-Karten Einstellungen";
            // 
            // textBoxSettingsIP
            // 
            this.textBoxSettingsIP.Location = new System.Drawing.Point(16, 42);
            this.textBoxSettingsIP.Name = "textBoxSettingsIP";
            this.textBoxSettingsIP.Size = new System.Drawing.Size(268, 20);
            this.textBoxSettingsIP.TabIndex = 0;
            // 
            // labelSettingsSDIP
            // 
            this.labelSettingsSDIP.AutoSize = true;
            this.labelSettingsSDIP.Location = new System.Drawing.Point(13, 26);
            this.labelSettingsSDIP.Name = "labelSettingsSDIP";
            this.labelSettingsSDIP.Size = new System.Drawing.Size(67, 13);
            this.labelSettingsSDIP.TabIndex = 1;
            this.labelSettingsSDIP.Text = "IP-Addresse:";
            // 
            // labelSettingsSDPath
            // 
            this.labelSettingsSDPath.AutoSize = true;
            this.labelSettingsSDPath.Location = new System.Drawing.Point(13, 77);
            this.labelSettingsSDPath.Name = "labelSettingsSDPath";
            this.labelSettingsSDPath.Size = new System.Drawing.Size(184, 13);
            this.labelSettingsSDPath.TabIndex = 3;
            this.labelSettingsSDPath.Text = "Pfad zu den Bildern auf der SD-Karte:";
            // 
            // textBoxSettingsSDPath
            // 
            this.textBoxSettingsSDPath.Location = new System.Drawing.Point(16, 93);
            this.textBoxSettingsSDPath.Name = "textBoxSettingsSDPath";
            this.textBoxSettingsSDPath.Size = new System.Drawing.Size(268, 20);
            this.textBoxSettingsSDPath.TabIndex = 2;
            // 
            // buttonSettingsSave
            // 
            this.buttonSettingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettingsSave.Location = new System.Drawing.Point(562, 406);
            this.buttonSettingsSave.Name = "buttonSettingsSave";
            this.buttonSettingsSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSettingsSave.TabIndex = 1;
            this.buttonSettingsSave.Text = "Speichern";
            this.buttonSettingsSave.UseVisualStyleBackColor = true;
            this.buttonSettingsSave.Click += new System.EventHandler(this.buttonSettingsSave_Click);
            // 
            // groupBoxServiceStatus
            // 
            this.groupBoxServiceStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxServiceStatus.Controls.Add(this.buttonServiceStatusStop);
            this.groupBoxServiceStatus.Controls.Add(this.buttonServiceStatusStart);
            this.groupBoxServiceStatus.Controls.Add(this.labelServiceStatus);
            this.groupBoxServiceStatus.Location = new System.Drawing.Point(8, 6);
            this.groupBoxServiceStatus.Name = "groupBoxServiceStatus";
            this.groupBoxServiceStatus.Size = new System.Drawing.Size(629, 51);
            this.groupBoxServiceStatus.TabIndex = 0;
            this.groupBoxServiceStatus.TabStop = false;
            this.groupBoxServiceStatus.Text = "Status";
            // 
            // labelServiceStatus
            // 
            this.labelServiceStatus.AutoSize = true;
            this.labelServiceStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceStatus.ForeColor = System.Drawing.Color.Red;
            this.labelServiceStatus.Location = new System.Drawing.Point(16, 19);
            this.labelServiceStatus.Name = "labelServiceStatus";
            this.labelServiceStatus.Size = new System.Drawing.Size(71, 16);
            this.labelServiceStatus.TabIndex = 0;
            this.labelServiceStatus.Text = "Gestoppt";
            // 
            // buttonServiceStatusStart
            // 
            this.buttonServiceStatusStart.Location = new System.Drawing.Point(93, 16);
            this.buttonServiceStatusStart.Name = "buttonServiceStatusStart";
            this.buttonServiceStatusStart.Size = new System.Drawing.Size(75, 23);
            this.buttonServiceStatusStart.TabIndex = 1;
            this.buttonServiceStatusStart.Text = "Starten";
            this.buttonServiceStatusStart.UseVisualStyleBackColor = true;
            this.buttonServiceStatusStart.Click += new System.EventHandler(this.buttonServiceStatusStart_Click);
            // 
            // buttonServiceStatusStop
            // 
            this.buttonServiceStatusStop.Location = new System.Drawing.Point(174, 16);
            this.buttonServiceStatusStop.Name = "buttonServiceStatusStop";
            this.buttonServiceStatusStop.Size = new System.Drawing.Size(75, 23);
            this.buttonServiceStatusStop.TabIndex = 2;
            this.buttonServiceStatusStop.Text = "Stoppen";
            this.buttonServiceStatusStop.UseVisualStyleBackColor = true;
            this.buttonServiceStatusStop.Click += new System.EventHandler(this.buttonServiceStatusStop_Click);
            // 
            // groupBoxServiceLog
            // 
            this.groupBoxServiceLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxServiceLog.Controls.Add(this.richTextBoxServiceLog);
            this.groupBoxServiceLog.Location = new System.Drawing.Point(8, 63);
            this.groupBoxServiceLog.Name = "groupBoxServiceLog";
            this.groupBoxServiceLog.Size = new System.Drawing.Size(629, 366);
            this.groupBoxServiceLog.TabIndex = 1;
            this.groupBoxServiceLog.TabStop = false;
            this.groupBoxServiceLog.Text = "Log";
            // 
            // richTextBoxServiceLog
            // 
            this.richTextBoxServiceLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxServiceLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxServiceLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxServiceLog.ForeColor = System.Drawing.Color.Lime;
            this.richTextBoxServiceLog.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxServiceLog.Name = "richTextBoxServiceLog";
            this.richTextBoxServiceLog.Size = new System.Drawing.Size(623, 347);
            this.richTextBoxServiceLog.TabIndex = 0;
            this.richTextBoxServiceLog.Text = "";
            // 
            // groupBoxSettingsService
            // 
            this.groupBoxSettingsService.Controls.Add(this.buttonSettingsServicePath);
            this.groupBoxSettingsService.Controls.Add(this.checkBoxSettingsServiceAutostart);
            this.groupBoxSettingsService.Controls.Add(this.labelSettingsServicePath);
            this.groupBoxSettingsService.Controls.Add(this.textBoxSettingsServicePath);
            this.groupBoxSettingsService.Location = new System.Drawing.Point(8, 148);
            this.groupBoxSettingsService.Name = "groupBoxSettingsService";
            this.groupBoxSettingsService.Size = new System.Drawing.Size(303, 113);
            this.groupBoxSettingsService.TabIndex = 2;
            this.groupBoxSettingsService.TabStop = false;
            this.groupBoxSettingsService.Text = "Service Einstellungen";
            // 
            // labelSettingsServicePath
            // 
            this.labelSettingsServicePath.AutoSize = true;
            this.labelSettingsServicePath.Location = new System.Drawing.Point(13, 60);
            this.labelSettingsServicePath.Name = "labelSettingsServicePath";
            this.labelSettingsServicePath.Size = new System.Drawing.Size(147, 13);
            this.labelSettingsServicePath.TabIndex = 3;
            this.labelSettingsServicePath.Text = "Pfad zu Bildarchiv auf Server:";
            // 
            // textBoxSettingsServicePath
            // 
            this.textBoxSettingsServicePath.Location = new System.Drawing.Point(16, 76);
            this.textBoxSettingsServicePath.Name = "textBoxSettingsServicePath";
            this.textBoxSettingsServicePath.Size = new System.Drawing.Size(196, 20);
            this.textBoxSettingsServicePath.TabIndex = 2;
            // 
            // checkBoxSettingsServiceAutostart
            // 
            this.checkBoxSettingsServiceAutostart.AutoSize = true;
            this.checkBoxSettingsServiceAutostart.Location = new System.Drawing.Point(16, 29);
            this.checkBoxSettingsServiceAutostart.Name = "checkBoxSettingsServiceAutostart";
            this.checkBoxSettingsServiceAutostart.Size = new System.Drawing.Size(268, 17);
            this.checkBoxSettingsServiceAutostart.TabIndex = 4;
            this.checkBoxSettingsServiceAutostart.Text = "Service (wenn möglich) bei Applikationsstart starten";
            this.checkBoxSettingsServiceAutostart.UseVisualStyleBackColor = true;
            // 
            // buttonSettingsServicePath
            // 
            this.buttonSettingsServicePath.Location = new System.Drawing.Point(218, 76);
            this.buttonSettingsServicePath.Name = "buttonSettingsServicePath";
            this.buttonSettingsServicePath.Size = new System.Drawing.Size(66, 20);
            this.buttonSettingsServicePath.TabIndex = 5;
            this.buttonSettingsServicePath.Text = "wählen";
            this.buttonSettingsServicePath.UseVisualStyleBackColor = true;
            this.buttonSettingsServicePath.Click += new System.EventHandler(this.buttonSettingsServicePath_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 463);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "WirelessPackageTracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageService.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupBoxSettingsCard.ResumeLayout(false);
            this.groupBoxSettingsCard.PerformLayout();
            this.groupBoxServiceStatus.ResumeLayout(false);
            this.groupBoxServiceStatus.PerformLayout();
            this.groupBoxServiceLog.ResumeLayout(false);
            this.groupBoxSettingsService.ResumeLayout(false);
            this.groupBoxSettingsService.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageService;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.Button buttonSettingsSave;
        private System.Windows.Forms.GroupBox groupBoxSettingsCard;
        private System.Windows.Forms.Label labelSettingsSDPath;
        private System.Windows.Forms.TextBox textBoxSettingsSDPath;
        private System.Windows.Forms.Label labelSettingsSDIP;
        private System.Windows.Forms.TextBox textBoxSettingsIP;
        private System.Windows.Forms.GroupBox groupBoxServiceStatus;
        private System.Windows.Forms.Button buttonServiceStatusStop;
        private System.Windows.Forms.Button buttonServiceStatusStart;
        private System.Windows.Forms.Label labelServiceStatus;
        private System.Windows.Forms.GroupBox groupBoxServiceLog;
        private System.Windows.Forms.GroupBox groupBoxSettingsService;
        private System.Windows.Forms.Label labelSettingsServicePath;
        private System.Windows.Forms.TextBox textBoxSettingsServicePath;
        private System.Windows.Forms.CheckBox checkBoxSettingsServiceAutostart;
        private System.Windows.Forms.Button buttonSettingsServicePath;
        public System.Windows.Forms.RichTextBox richTextBoxServiceLog;
    }
}

