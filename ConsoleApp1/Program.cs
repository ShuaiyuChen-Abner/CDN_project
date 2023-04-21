using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {

        static void Main()
        {
            string input = "Hello, World!";
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 只使用前8个字节
                long number = BitConverter.ToInt64(hashBytes, 0);
                Console.WriteLine($"Long value from first 8 bytes of the MD5 hash: {number}");
                Console.WriteLine(number%2048);
            }
        }

    }
}