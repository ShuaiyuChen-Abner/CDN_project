using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

using CDNHw;
namespace CDNServer
{
    public partial class Form1 : Form
    {
        TCPHelper tcp = new TCPHelper();
        string baseDir = "";
        MD5 md5 = MD5.Create();
        Dictionary<string, List<string>> FileCache = new Dictionary<string, List<string>>();
        Dictionary<string, byte[]> FragmentCache = new Dictionary<string, byte[]>();


        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles");
            }
            baseDir = AppDomain.CurrentDomain.BaseDirectory + "/serverFiles";
            listBox1.Items.Clear();
            foreach (FileInfo fileInfo in GetAllServerFileNames())
            {
                listBox1.Items.Add(fileInfo.Name);
            }
            // make md5 digest for per 2KB data
            foreach (FileInfo fileInfo in GetAllServerFileNames())
            {

                List<string> fileStruct = new List<string>();
                byte[] data = File.ReadAllBytes(fileInfo.FullName);

                MD5 md5 = MD5.Create();
                int window = 16;
                byte[] buffer = new byte[window];
                int frangment_start = 0;
                byte[] md5_hash = new byte[16];

                // split file into fragments by rabin function
                for (int i = window - 1; i < data.Length; i++)
                {
                    Array.Copy(data, i + 1 - window, buffer, 0, window);
                    md5_hash = md5.ComputeHash(buffer);
                    //ȡǰ��λת��Ϊʮ����
                    long md5DecimalValue = BitConverter.ToInt64(md5_hash, 0);

                    if ((Math.Abs(md5DecimalValue) % 2048 == 369))
                    {

                        //fragmen_start �� i�ֿ�ɹ���Ӧ�ü���md5���ŵ�fileStruct�С�
                        byte[] fragment = new byte[i - frangment_start + 1];
                        Array.Copy(data, frangment_start, fragment, 0, i - frangment_start + 1);
                        string digest = MD5Str.md5(fragment);
                        fileStruct.Add(digest);
                        if (!FragmentCache.ContainsKey(digest))
                        {
                            FragmentCache.Add(digest, fragment);
                        }
                        frangment_start = i + 1;
                        if (i + window > data.Length)
                        {
                            fragment = new byte[data.Length - frangment_start];
                            Array.Copy(data, frangment_start, fragment, 0, data.Length - frangment_start);

                            digest = MD5Str.md5(fragment);
                            fileStruct.Add(digest);
                            if (!FragmentCache.ContainsKey(digest))
                            {
                                FragmentCache.Add(digest, fragment);
                            }
                        }
                        else
                        {
                            i += window;
                        }
                    }
                    else
                    {
                        if (i + 1 == data.Length)
                        {
                            //˵����ʱ��һ��͵��ļ����һ���ֽڣ���ֱ�Ӵ����һ��װ��fragment����start
                            byte[] fragment = new byte[data.Length - frangment_start];
                            Array.Copy(data, frangment_start, fragment, 0, data.Length - frangment_start);

                            string digest = MD5Str.md5(fragment);
                            fileStruct.Add(digest);
                            if (!FragmentCache.ContainsKey(digest))
                            {
                                FragmentCache.Add(digest, fragment);
                            }
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                FileCache.Add(fileInfo.Name, fileStruct);

                /*
                                for (int i = 0; i < data.Length; i += 2048)
                                {
                                    byte[] buffer = new byte[data.Length - i > 2048 ? 2048 : data.Length - i];
                                    Array.Copy(data, i, buffer, 0, buffer.Length);
                                    string digest = MD5Str.md5(buffer);
                                    fileStruct.Add(digest);
                                    // store file fragment in memory
                                    if (!FragmentCache.ContainsKey(digest))
                                    {
                                        FragmentCache.Add(digest, buffer);
                                    }

                                }
                                FileCache.Add(fileInfo.Name, fileStruct);*/
            }



            tcp.InitListener();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                tcp.AcceptLoop(ProcessRequest);

            }
        }

        private string ProcessRequest(string request)
        {
            if (request == "LS")
            {
                List<string> files = new List<string>();
                foreach (FileInfo file in GetAllServerFileNames())
                {
                    files.Add(file.Name);
                }
                return string.Join(Environment.NewLine, files.ToArray());//return file list
            }
            if (request.StartsWith("ST:"))
            {
                string fileName = request.Substring(3);
                FileInfo fileInfo = GetFileInfoByFileName(fileName);
                if (fileInfo != null)
                {
                    return string.Join(Environment.NewLine, FileCache[fileName]); //return digests
                }
            }
            if (request.StartsWith("FR:"))
            {
                string[] fileNameParts = request.Substring(3).Split(':');
                string fileName = fileNameParts[0];
                string digest = fileNameParts[1];
                FileInfo fileInfo = GetFileInfoByFileName(fileName);
                if (fileInfo != null)
                {
                    //return file fragment by fragment md5

                    byte[] Fragment = FragmentCache[digest];

                    return Convert.ToBase64String(Fragment);
                }
            }


            return "Error!";
        }
        /// <summary>
        /// get file info
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileInfo GetFileInfoByFileName(string fileName)
        {
            var fileInfoLst = GetAllServerFileNames();
            foreach (var fileInfo in fileInfoLst)
            {
                if (fileInfo.Name == fileName)
                    return fileInfo;
            }
            return null;

        }
        /// <summary>
        /// get all files' info from the server dir
        /// </summary>
        /// <returns></returns>
        public List<FileInfo> GetAllServerFileNames()
        {
            List<FileInfo> result = new List<FileInfo>();
            string[] files = Directory.GetFiles(baseDir);
            foreach (string file in files)
            {
                result.Add(new FileInfo(file));
            }
            return result;
        }




        public class MD5Str
        {
            public static string md5(byte[] buffer)
            {

                try
                {

                    var check = MD5.Create();
                    byte[] somme = check.ComputeHash(buffer);
                    string ret = "";
                    foreach (byte a in somme)
                    {
                        if (a < 16)
                            ret += "0" + a.ToString("X");
                        else
                            ret += a.ToString("X");
                    }
                    return ret.ToUpper();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// allow administrator to choose file and place it into server dir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            File.Copy(openFileDialog1.FileName, baseDir + "/" + new FileInfo(openFileDialog1.FileName).Name);
            listBox1.Items.Clear();
            foreach (FileInfo fileInfo in GetAllServerFileNames())
            {
                listBox1.Items.Add(fileInfo.Name);
            }
        }

    }
}