using CDNHw;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace CDNCache
{
    public partial class Form1 : Form
    {
        public string serverIP = "127.0.0.1";
        public int serverPort = 8090;

        TCPHelper tcp = new TCPHelper();
        MD5 md5 = MD5.Create();

        Dictionary<string,List<string>> FileCache= new Dictionary<string,List<string>>();
        Dictionary<string,string> FileCacheLog=new Dictionary<string, string> ();
        Dictionary<string, byte[]> FragmentCache = new Dictionary<string, byte[]>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tcp.InitListener();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                //?
                tcp.AcceptLoop(ProcessRequest);
                backgroundWorker1.ReportProgress(0);//refresh the listbox after an acceptance
            }
        }
        //?
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            RefreshData();

        }

        public void RefreshData()
        {
            lstCache.Items.Clear();
            lstFiles.Items.Clear();

            foreach (string fileName in FileCache.Keys)
            {
                lstFiles.Items.Add(fileName);
            }
            foreach (string digest in FragmentCache.Keys)
            {
                lstCache.Items.Add(digest);
            }
        }

        private string ProcessRequest(string request)
        {
            if (request == "LS")
            {
                return tcp.SendRequest(request, serverIP, serverPort); //list the files
            }
            if (request.StartsWith("DL:"))
            {
                // 
                string fileName = request.Substring(3);
                if (!FileCache.ContainsKey(fileName))
                {
                    string fileStructRaw = tcp.SendRequest("ST:"+fileName,serverIP, serverPort);
                    FileCache[fileName] = new List<string>(fileStructRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
                } //get the file structure in md5 lines
                int fragmentIndex = 0;
                int cacheMatchCount = 0;

                List<byte> buffer = new List<byte>();

                foreach(string digest in FileCache[fileName])
                {
                    if (!FragmentCache.ContainsKey(digest))
                    {
                        string fragmentB64 = tcp.SendRequest("FR:" + fileName + ":" + digest, serverIP, serverPort); //get the fragment data from the server
                        FragmentCache[digest] = Convert.FromBase64String(fragmentB64);
                    }
                    else
                    {
                        cacheMatchCount++; //the fragment's md5 is in the cache, increase the hit count
                    }
                    buffer.AddRange(FragmentCache[digest]);
                    fragmentIndex++;
                }
                if (!FileCacheLog.ContainsKey(fileName))
                {
                    FileCacheLog[fileName] = "";
                }
                FileCacheLog[fileName] += string.Format(@"user request: file {0} at {1:yyyy-MM-dd HH:mm:ss}
response: {2:0.0}% of file {0} was constructed with the cached data" + Environment.NewLine, fileName, DateTime.Now, cacheMatchCount * 100.0 / fragmentIndex);

                return Convert.ToBase64String(buffer.ToArray());//make the byte data into base64 string so as to send it back to the client
            }
            return "Hello!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileCache.Clear();
            FragmentCache.Clear();
            FileCacheLog.Clear();
            RefreshData();
        }

        

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //show the log
            if (lstFiles.SelectedIndex >= 0)
            {
                string fileName = lstFiles.SelectedItem.ToString();
                if (FileCacheLog.ContainsKey(fileName))
                {
                    txtLog.Text = FileCacheLog[fileName];
                }

            }
        }
    }
}