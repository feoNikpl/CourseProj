namespace ChatClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.DownloadFile = new System.Windows.Forms.Button();
            this.ClientName = new System.Windows.Forms.TextBox();
            this.SerwerConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.UsersFiles = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ClientID = new System.Windows.Forms.TextBox();
            this.UdpConnect = new System.Windows.Forms.Button();
            this.ClientFiles = new System.Windows.Forms.ListBox();
            this.AddFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DeleteFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // DownloadFile
            // 
            this.DownloadFile.Enabled = false;
            this.DownloadFile.Location = new System.Drawing.Point(440, 210);
            this.DownloadFile.Name = "DownloadFile";
            this.DownloadFile.Size = new System.Drawing.Size(121, 23);
            this.DownloadFile.TabIndex = 6;
            this.DownloadFile.Text = "Скачать";
            this.DownloadFile.UseVisualStyleBackColor = true;
            this.DownloadFile.Click += new System.EventHandler(this.DownloadFile_Click);
            // 
            // ClientName
            // 
            this.ClientName.Location = new System.Drawing.Point(12, 27);
            this.ClientName.Name = "ClientName";
            this.ClientName.Size = new System.Drawing.Size(100, 20);
            this.ClientName.TabIndex = 7;
            // 
            // SerwerConnect
            // 
            this.SerwerConnect.Location = new System.Drawing.Point(132, 81);
            this.SerwerConnect.Name = "SerwerConnect";
            this.SerwerConnect.Size = new System.Drawing.Size(151, 20);
            this.SerwerConnect.TabIndex = 8;
            this.SerwerConnect.Text = "Соедениться с сервером";
            this.SerwerConnect.UseVisualStyleBackColor = true;
            this.SerwerConnect.Visible = false;
            this.SerwerConnect.Click += new System.EventHandler(this.SerwerConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "введите ваше имя";
            // 
            // UsersFiles
            // 
            this.UsersFiles.FormattingEnabled = true;
            this.UsersFiles.Location = new System.Drawing.Point(12, 151);
            this.UsersFiles.Name = "UsersFiles";
            this.UsersFiles.Size = new System.Drawing.Size(388, 251);
            this.UsersFiles.TabIndex = 12;
            this.UsersFiles.SelectedIndexChanged += new System.EventHandler(this.UsersFiles_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "ваш ID";
            this.label3.Visible = false;
            // 
            // ClientID
            // 
            this.ClientID.Location = new System.Drawing.Point(12, 82);
            this.ClientID.Name = "ClientID";
            this.ClientID.Size = new System.Drawing.Size(100, 20);
            this.ClientID.TabIndex = 14;
            this.ClientID.Visible = false;
            // 
            // UdpConnect
            // 
            this.UdpConnect.Location = new System.Drawing.Point(132, 27);
            this.UdpConnect.Name = "UdpConnect";
            this.UdpConnect.Size = new System.Drawing.Size(151, 20);
            this.UdpConnect.TabIndex = 16;
            this.UdpConnect.Text = "найти сервер";
            this.UdpConnect.UseVisualStyleBackColor = true;
            this.UdpConnect.Click += new System.EventHandler(this.UdpConnect_Click);
            // 
            // ClientFiles
            // 
            this.ClientFiles.FormattingEnabled = true;
            this.ClientFiles.Location = new System.Drawing.Point(12, 455);
            this.ClientFiles.Name = "ClientFiles";
            this.ClientFiles.Size = new System.Drawing.Size(388, 173);
            this.ClientFiles.TabIndex = 17;
            this.ClientFiles.SelectedIndexChanged += new System.EventHandler(this.ClientFiles_SelectedIndexChanged);
            // 
            // AddFile
            // 
            this.AddFile.Enabled = false;
            this.AddFile.Location = new System.Drawing.Point(440, 497);
            this.AddFile.Name = "AddFile";
            this.AddFile.Size = new System.Drawing.Size(121, 23);
            this.AddFile.TabIndex = 18;
            this.AddFile.Text = "Добавить";
            this.AddFile.UseVisualStyleBackColor = true;
            this.AddFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Файлы доступные для загрузки";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 430);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Мои файлы";
            // 
            // DeleteFile
            // 
            this.DeleteFile.Enabled = false;
            this.DeleteFile.Location = new System.Drawing.Point(440, 536);
            this.DeleteFile.Name = "DeleteFile";
            this.DeleteFile.Size = new System.Drawing.Size(121, 23);
            this.DeleteFile.TabIndex = 21;
            this.DeleteFile.Text = "Добавить";
            this.DeleteFile.UseVisualStyleBackColor = true;
            this.DeleteFile.Click += new System.EventHandler(this.DeleteFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 640);
            this.Controls.Add(this.DeleteFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AddFile);
            this.Controls.Add(this.ClientFiles);
            this.Controls.Add(this.UdpConnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ClientID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsersFiles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SerwerConnect);
            this.Controls.Add(this.ClientName);
            this.Controls.Add(this.DownloadFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button DownloadFile;
        private System.Windows.Forms.TextBox ClientName;
        private System.Windows.Forms.Button SerwerConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox UsersFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ClientID;
        private System.Windows.Forms.Button UdpConnect;
        private System.Windows.Forms.ListBox ClientFiles;
        private System.Windows.Forms.Button AddFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DeleteFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

