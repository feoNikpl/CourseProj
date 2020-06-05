using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CSfunction;
using Message = CSfunction.Message;

namespace ChatClient
{
    
    public partial class Form1 : Form
    {
        private Client client;
        private int SelectIndex  = -1;
        private string FileName = "";
        private const bool SET = true;
        private const bool DELETE = false;
        private const string UploadDirr = "C:\\Users\\feopl\\Desktop\\загрузить\\";
        public Form1()
        {
            InitializeComponent();
            client = new Client();
            client.ReceiveMessageEvent += ShowReceivedMessage;
        }

        public void ShowReceivedMessage(Message message)
        {
            if (message is ServerAnswerRequest)
            {
                AnswerRequestManager(message);
            }
            if (message is ClientIDMessage)
                IDManager(message);
            if (message is FileListMessage)
            {
                RefreshFiles();
            }
        }
        
        public void RefreshFiles()
        {
            Action action = delegate
            {
                UsersFiles.Items.Clear();
                ClientFiles.Items.Clear();
                foreach (FileSet fileSet in client.FileSets)
                {
                    if (fileSet.HostID != client.ClientID)
                        UsersFiles.Items.Add(fileSet.FileName);
                    else
                        ClientFiles.Items.Add(fileSet.FileName);
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }

        }

        private void SerwerConnect_Click(object sender, EventArgs e)
        {
            if (ClientID.Text != "")
            {
                if (client.TCPConnectServer(ClientName.Text,Convert.ToInt32(ClientID.Text)))
                {
                    SerwerConnect.Enabled = false;
                    DownloadFile.Enabled = true;
                    AddFile.Enabled = true;
                    DeleteFile.Enabled = true;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client.TcpSocket != null)
                client.Disconnect();

        }


        private void AnswerRequestManager(Message message)
        {
            Action action = delegate
            {
                ServerAnswerRequest mes = (ServerAnswerRequest)message;
                if (mes.Existance == true)
                {
                    ClientID.Visible = true;
                    label3.Visible = true;
                    SerwerConnect.Visible = true;
                }
                else
                {
                    client.TCPConnectServer(ClientName.Text, 0);
                    DownloadFile.Enabled = true;
                    AddFile.Enabled = true;
                    DeleteFile.Enabled = true;
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }

        }

        private void IDManager(Message message)
        {
            Action action = delegate
            {
                ClientIDMessage mes = (ClientIDMessage)message;
                ClientID.Visible = true;
                label3.Visible = true;
                ClientID.Enabled = false;
                ClientID.Text = mes.Id.ToString();
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }

        }

        private void UsersFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DownloadFile.Enabled = true;
            SelectIndex = UsersFiles.SelectedIndex;
            FileName = UsersFiles.Items[SelectIndex].ToString();
        }

        private void ClientFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DownloadFile.Enabled = false;
            SelectIndex = ClientFiles.SelectedIndex;
            FileName = ClientFiles.Items[SelectIndex].ToString();
        }

        private void UdpConnect_Click(object sender, EventArgs e)
        {
            client.UDPConnectServer(ClientName.Text);
            ClientName.Enabled = false;
            UdpConnect.Enabled = false;
        }

        private void DownloadFile_Click(object sender, EventArgs e)
        {
            if (FileName != "")
                client.DownloadFileRequest(FileName);
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filepath = openFileDialog1.FileName;
            string filename = Path.GetFileName(openFileDialog1.FileName);
            string extention = Path.GetExtension(filepath);
            File.Copy(filepath, UploadDirr + filename);
            if (extention == ".jpeg" || extention == ".jpg")
            {
                RLE RLE = new RLE();
                RLE.FileCompression(UploadDirr + filename);
            }
            client.SendMessage(filename, SET);
        }

        private void DeleteFile_Click(object sender, EventArgs e)
        {
            File.Delete(UploadDirr + FileName);
            client.SendMessage(FileName, DELETE);
        }

    }
}

