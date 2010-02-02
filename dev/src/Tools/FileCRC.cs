using System;
using System.IO;
using System.Windows.Forms;
using Wof.Controller;

namespace Wof.Tools
{
    /// <summary>
    /// Tool s³u¿¹cy do sprawdzania sumy kontrolnej plików
    /// </summary>
    public class FileCRC
    {
        private static void Main(string[] args)
        {
            // string outputFile;
            if (args.Length == 0 || args.Length > 3)
            {
                Console.WriteLine("Usage: WofCRC.exe inputEncodedFilename [-b] [-m]");
                Console.WriteLine("-b suppresses C#'s array formatted byte output");
                Console.WriteLine("-m calculates MD5 instead of RSA");
                Console.WriteLine("Prints file CRC");
                return;
            }
            string filename = args[0];

            if (!File.Exists(filename))
            {
                MessageBox.Show("File '" + filename + "' does not exist");
                return;
            }

            byte[] crc;
            bool md5 = false;
            try
            {
                 md5 = "-m".Equals(args[1]) || "-m".Equals(args[2]) || "-m".Equals(args[3]);
            }
            catch (Exception)
            {
               
            }

            bool format = false;
            try
            {
                 format = "-b".Equals(args[1]) || "-b".Equals(args[2]) || "-b".Equals(args[3]);
            }
            catch (Exception)
            {
                
            }
           
            if (!md5)
            {
                crc = SHA1_Hash.DigestEncodedFile(filename);
            }
            else
            {
                crc = SHA1_Hash.ComputeMD5(filename);
            }


            if (format)
            {
                Console.Write("new byte[] {");
                for (int i = 0; i < crc.Length; i++)
                {
                    Console.Write(crc[i]);
                    if (i < crc.Length - 1) Console.Write(",");
                }
                Console.Write("},");
            }
            else
            {
                foreach (byte b in crc)
                {
                    Console.Write(b);
                }
            }
        }
    }
}