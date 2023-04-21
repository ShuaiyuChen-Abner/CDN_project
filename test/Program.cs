using System.Numerics;
using System.Security.Cryptography;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string baseDir = "";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles");
            }
            baseDir = AppDomain.CurrentDomain.BaseDirectory + "/serverFiles";
            
            FileInfo fileInfo = GetFileInfoByFileName("a.PNG");
            byte[] data = File.ReadAllBytes(fileInfo.FullName);

            Dictionary<string, List<string>> FileCache = new Dictionary<string, List<string>>();
            Dictionary<string, byte[]> FragmentCache = new Dictionary<string, byte[]>();

            List<int> fragment_len = new List<int>();


            List<string> fileStruct = new List<string>();
            MD5 md5 = MD5.Create();
            int window = 16;
            byte[] buffer = new byte[window];
            int frangment_start = 0;
            string diget = "";
            byte[] md5_hash = new byte[16];

            List<byte> result = new List<byte>();

            // split file into fragments by rabin function
            for (int i = window - 1; i < data.Length; i++)
            {
                Array.Copy(data, i + 1 - window, buffer, 0, window);
                md5_hash = md5.ComputeHash(buffer);
                //取前八位转换为十进制
                long md5DecimalValue = BitConverter.ToInt64(md5_hash, 0);

                if ((Math.Abs(md5DecimalValue) % 2048 == 369))
                {

                    //fragmen_start 到 i分块成功，应该计算md5，放到fileStruct中。
                    byte[] fragment = new byte[i - frangment_start + 1];
                    Array.Copy(data, frangment_start, fragment, 0, i - frangment_start + 1);
                    foreach(byte j in fragment)
                    {
                        result.Add(j);
                    }
                    string digest = MD5Str.md5(fragment);
                    fileStruct.Add(digest);
                    if (!FragmentCache.ContainsKey(digest))
                    {
                        FragmentCache.Add(digest, buffer);
                    }
                    frangment_start = i + 1;
                    if (i + window > data.Length)
                    {
                        fragment = new byte[data.Length - frangment_start];
                        Array.Copy(data, frangment_start, fragment, 0, data.Length - frangment_start);
                        foreach (byte j in fragment)
                        {
                            result.Add(j);
                        }
                        digest = MD5Str.md5(fragment);
                        fileStruct.Add(digest);
                        if (!FragmentCache.ContainsKey(digest))
                        {
                            FragmentCache.Add(digest, buffer);
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
                        //说明此时下一块就到文件最后一个字节，则直接从最后一个装到fragment——start
                        byte[] fragment = new byte[data.Length - frangment_start];
                        Array.Copy(data, frangment_start, fragment, 0, data.Length - frangment_start);
                        foreach (byte j in fragment)
                        {
                            result.Add(j);
                        }
                        string digest = MD5Str.md5(fragment);
                        fileStruct.Add(digest);
                        if (!FragmentCache.ContainsKey(digest))
                        {
                            FragmentCache.Add(digest, buffer);
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

 


            byte[] result1 = result.ToArray();
            Console.WriteLine( result1.SequenceEqual(data));
            //Console.WriteLine("一共多少块：" + fragment_len.Count);
            //Console.WriteLine("最大值为：" + fragment_len.Max());
            //Console.WriteLine("最小值为：" + fragment_len.Min());
            //Console.WriteLine("平均长度为："  + fragment_len.Average());
        }
        public static List<FileInfo> GetAllServerFileNames()
        {
            string baseDir = "";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/serverFiles");
            }
            baseDir = AppDomain.CurrentDomain.BaseDirectory + "/serverFiles";
            List<FileInfo> result = new List<FileInfo>();
            string[] files = Directory.GetFiles(baseDir);
            foreach (string file in files)
            {
                result.Add(new FileInfo(file));
            }
            return result;
        }

        public static FileInfo GetFileInfoByFileName(string fileName)
        {
            var fileInfoLst = GetAllServerFileNames();
            foreach (var fileInfo in fileInfoLst)
            {
                if (fileInfo.Name == fileName)
                    return fileInfo;
            }
            return null;

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
    }
}