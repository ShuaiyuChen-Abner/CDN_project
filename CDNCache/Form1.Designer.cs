namespace CDNCache
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstFiles = new ListBox();
            txtLog = new TextBox();
            lstCache = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            button1 = new Button();
            SuspendLayout();
            // 
            // lstFiles
            // 
            lstFiles.FormattingEnabled = true;
            lstFiles.ItemHeight = 17;
            lstFiles.Location = new Point(316, 36);
            lstFiles.Margin = new Padding(2, 3, 2, 3);
            lstFiles.Name = "lstFiles";
            lstFiles.Size = new Size(275, 565);
            lstFiles.TabIndex = 1;
            lstFiles.MouseDoubleClick += lstFiles_MouseDoubleClick;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(609, 37);
            txtLog.Margin = new Padding(2, 3, 2, 3);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(237, 564);
            txtLog.TabIndex = 2;
            // 
            // lstCache
            // 
            lstCache.FormattingEnabled = true;
            lstCache.ItemHeight = 17;
            lstCache.Location = new Point(9, 36);
            lstCache.Margin = new Padding(2, 3, 2, 3);
            lstCache.Name = "lstCache";
            lstCache.Size = new Size(303, 565);
            lstCache.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 16);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(43, 17);
            label1.TabIndex = 4;
            label1.Text = "Cache";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(316, 14);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(27, 17);
            label2.TabIndex = 5;
            label2.Text = "File";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(609, 14);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(73, 17);
            label3.TabIndex = 6;
            label3.Text = "Log Viewer";
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            // 
            // button1
            // 
            button1.Location = new Point(180, 6);
            button1.Margin = new Padding(2, 3, 2, 3);
            button1.Name = "button1";
            button1.Size = new Size(96, 25);
            button1.TabIndex = 7;
            button1.Text = "CacheClear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(893, 608);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lstCache);
            Controls.Add(txtLog);
            Controls.Add(lstFiles);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Form1";
            Text = "CDNCache";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ListBox lstFiles;
        private TextBox txtLog;
        private ListBox lstCache;
        private Label label1;
        private Label label2;
        private Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button button1;
    }
}