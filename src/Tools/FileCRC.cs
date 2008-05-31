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
            if (args.Length == 0 || args.Length > 2)
            {
                Console.WriteLine("Usage: WofCRC.exe inputEncodedFilename [-b]");
                Console.WriteLine("-b suppresses C#'s array formatted byte output");
                Console.WriteLine("Prints file CRC");
                return;
            }
            string filename = args[0];

            if (!File.Exists(filename))
            {
                MessageBox.Show("File '" + filename + "' does not exist");
                return;
            }
            byte[] crc = SHA1_Hash.DigestEncodedFile(filename);
            if (args.Length == 2 && args[1].Equals("-b"))
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