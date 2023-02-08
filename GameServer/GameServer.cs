using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    public partial class GameServer : Form
    {
        private MainServer m_mainServer;

        // Client 접속을 관리할 스레드
        private Thread m_mainServerThread;

        // 게임 업데이트 타이머
        private const int UpdateTime = 100; // 0.1초

        // DB 업데이트 타이머
        private const int DBUpdateTime = 10000; // 10초

        public GameServer()
        {
            InitializeComponent();

            m_mainServer = new MainServer();

            DBManager.Instance.DbUpdate(dbListBox);

            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);

            updateTimer.Interval = UpdateTime;
            updateTimer.Enabled = true;

            dbUpdateTimer.Interval = DBUpdateTime;
            dbUpdateTimer.Enabled = true;
        }

        private void serverOpenBtn_Click(object sender, EventArgs e)
        {
            if (portTextBox.Text == "")
            {
                WriteToRichTextBox("포트번호를 입력해주세요.");
            }
            else
            {
                if (m_mainServer.ServerNet == null)
                {
                    m_mainServerThread = new Thread(new ThreadStart(() => m_mainServer.Init(Int32.Parse(portTextBox.Text), 30)));
                    m_mainServerThread.IsBackground = true;
                    m_mainServerThread.Start();

                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    string localIP = string.Empty;

                    for (int i = 0; i < host.AddressList.Length; i++)
                    {
                        if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIP = host.AddressList[i].ToString();
                            break;
                        }
                    }

                    IPTextBox.Text = localIP;

                    WriteToRichTextBox("서버가 오픈되었습니다.");
                }
                else
                {
                    WriteToRichTextBox("서버가 이미 오픈되어있습니다.");
                }
            }
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void portTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        public void WriteToRichTextBox(string text)
        {
            serverLogRTB.AppendText(text);
            serverLogRTB.AppendText("\n");
            serverLogRTB.ScrollToCaret();
        }

        private void GameServer_Load(object sender, EventArgs e)
        {

        }

        private void GameServerClosed(object sender, FormClosedEventArgs e)
        {
            updateTimer.Stop();
            if (m_mainServerThread != null)
            {
                m_mainServerThread.Abort();
            }
        }

        private void NetworkMessageCheck()
        {
            if (m_mainServer.ServerNet.networkMessage.Count != 0)
            {
                WriteToRichTextBox(m_mainServer.ServerNet.networkMessage.Dequeue());
            }
        }

        // 지정된 시간마다 업데이트 돌 부분
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (m_mainServer.ServerNet != null)
            {
                NetworkMessageCheck();
                m_mainServer.ReceivePacket();
            }
        }

        private void dbBtn_Click(object sender, EventArgs e)
        {
            if (DBManager.Instance != null)
            {
                DBManager.Instance.SearchDB(dbListBox, tableListBox);
            }
        }

        private void tableBtn_Click(object sender, EventArgs e)
        {
            if (DBManager.Instance != null)
            {
                DBManager.Instance.SearchTable(tableListBox, tableDataGridView);
            }
        }

        // 지정된 시간마다 DB 업데이트 돌릴 부분
        private void dbUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (DBManager.Instance != null)
            {
                DBManager.Instance.DbUpdate(dbListBox);
            }
        }
    }
}
