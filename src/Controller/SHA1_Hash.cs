using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Mogre;
using Wof.Controller.Screens;
using Wof.View;

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
                new byte[] {161,32,231,121,165,245,87,75,212,193,218,109,29,156,164,39,103,129,127,242},
                new byte[] {3,73,56,92,80,16,151,217,104,36,43,102,62,48,60,205,87,8,180,69},
                new byte[] {165,54,22,72,6,219,83,189,180,129,202,50,15,241,98,101,142,179,89,101},
                new byte[] {242,219,47,217,144,125,160,241,199,85,255,97,218,157,62,246,143,164,137,128},
                new byte[] {238,153,81,53,117,92,73,118,201,140,156,2,134,254,240,138,134,246,197,103},
                new byte[] {172,75,19,92,219,135,24,135,52,119,16,59,205,8,27,81,167,18,105,48},
                new byte[] {39,77,69,137,233,11,16,222,13,64,101,13,31,125,34,13,172,156,101,232},
                new byte[] {157,75,83,175,27,56,102,103,233,232,226,245,80,64,132,7,224,252,8,73},
                new byte[] {157,75,83,175,27,56,102,103,233,232,226,245,80,64,132,7,224,252,8,73}
            };

        private static readonly Dictionary<string, byte[]> hashOfImage = new Dictionary<string, byte[]>();

        private static readonly SHA1 sha = new SHA1Managed();
        private static readonly ASCIIEncoding ae = new ASCIIEncoding();


        private static void InitHashOfImage()
        {
            if(hashOfImage.Count > 0) return;
            hashOfImage[GameScreen.C_DEFAULT_AD_IMAGE_NAME] = new byte[] { 59, 65, 55, 101, 126, 22, 65, 85, 243, 224, 243, 18, 147, 13, 35, 102 };
            
        }
        public static byte[] ComputeMD5(string path)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(File.ReadAllBytes(path));
        }

        public static bool ValidateImage(string imageName)
        {
            string path;
            try
            {
                FileInfo_NativePtr info;
                info = ResourceGroupManager.Singleton.FindResourceFileInfo(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, imageName)[0];
                path = info.archive.Name + "/" + imageName;

            }
            catch (Exception)
            {
                return false;
            }
            
            

            MD5 md5 = MD5.Create();
            InitHashOfImage();
            if (!hashOfImage.ContainsKey(imageName))
            {
                return true;
            }

            byte[] hash = md5.ComputeHash(File.ReadAllBytes(path));
             
            //Wiêc trzeba rêcznie sprawdziæ wszystkie pary czy s¹ równe
            for (int i = 0; i < hashOfImage.Count; i++)
            {
                if (hash[i] != hashOfImage[imageName][i])
                     return false;
            }

            return true;
        }

        public static bool ValidateLevel(int levelNumber, string levelContent)
        {
            return true;
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