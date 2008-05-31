using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Wof.Controller
{
    public class SHA1_Hash
    {
        /// <summary>
        /// Obs³uga sum kontrolnych zawartoœci niezaszyfrowanych level'i 
        /// <author>Kamil S³awiñski</author>
        /// </summary>
        private static readonly byte[][] hashOfLevel = new byte[][]
            {
                new byte[] {44,12,23,192,228,15,115,236,9,66,24,5,174,53,4,146,106,237,140,48},
                new byte[] {21,82,15,133,91,75,12,101,142,172,59,174,193,194,96,173,225,136,79,138} // demo Level 2
               // - > original Level 2:  new byte[] {3,73,56,92,80,16,151,217,104,36,43,102,62,48,60,205,87,8,180,69},
              
                
                /*new byte[] {165, 54, 22, 72, 6, 219, 83, 189, 180, 129, 202, 50, 15, 241, 98, 101, 142, 179, 89, 101},
                new byte[]
                    {134, 14, 150, 155, 85, 109, 201, 227, 209, 252, 136, 49, 170, 215, 172, 128, 143, 196, 209, 16},
                new byte[] {140, 97, 83, 118, 165, 162, 138, 140, 98, 228, 99, 34, 192, 193, 98, 170, 205, 242, 192, 63},
                new byte[] {137, 63, 93, 101, 121, 157, 111, 29, 69, 72, 69, 215, 44, 178, 88, 164, 221, 159, 155, 156}*/
            };

        private static readonly SHA1 sha = new SHA1Managed();
        private static readonly ASCIIEncoding ae = new ASCIIEncoding();

        public static bool ValidateLevel(int levelNumber, string levelContent)
        {
           // return true;
            if (levelNumber > hashOfLevel.Length) return false;

            byte[] hash = DigestMessage(levelContent);

            //Array1.Equals(Array2) - porównuje instancje a nie wartoœci!!!
            //Wiêc trzeba rêcznie sprawdziæ wszystkie pary czy s¹ równe
            for (int i = 0; i < hashOfLevel.Length; i++)
            {
                if (hash[i] != hashOfLevel[levelNumber - 1][i])
                    return false;
            }

            return true;
        }

        public static byte[] DigestMessage(string message)
        {
            byte[] data = ae.GetBytes(message);
            return sha.ComputeHash(data);
        }

        public static byte[] DigestEncodedFile(string filePath)
        {
            return DigestMessage(RijndaelSimple.Decrypt(File.ReadAllText(filePath)));
        }
    }
}