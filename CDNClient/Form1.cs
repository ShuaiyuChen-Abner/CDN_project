using System.Net.Sockets;
using System.Text;
using CDNHw;

namespace CDNClient
{
    public partial class Form1 : Form
    {
        public string serverIP = "127.0.0.1";
        public int serverPort = 8091;

        TCPHelper tcp = new TCPHelper();




        public Form1()
        {

            InitializeComponent();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string resp = tcp.SendRequest("LS", serverIP, serverPort);
            if (resp != null)
            {
                string[] files = resp.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                listBox1.Items.Clear();
                foreach (string file in files)
                {
                    listBox1.Items.Add(file);
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string fileName = listBox1.SelectedItem.ToString();
                string fileB64 = tcp.SendRequest("DL:" + fileName, serverIP, serverPort);
                if (fileB64 != null)
                {
                    pictureBox1.Image = null;
                   
                    byte[] buff = Convert.FromBase64String(fileB64);
                    MemoryStream ms = new MemoryStream(buff);
                    Bitmap bm = new Bitmap(ms);
                    pictureBox1.Image = bm;

                }
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}