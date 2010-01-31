using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace Wof.Controller
{

    public class Licensing
    {

       public static readonly string C_LICENSE_FILE = "enhanced.dat";
       private static readonly string C_ENHANCED_VERSION_LICENSE = "asZc2czA4l6a12sd";
       private static string hash;

        public static string Hash
        {
            get { 
                if(hash == null) BuildHash();
                
                return hash; 
            }
        }

       public static bool BuildLicenseFile()
       {
           try
           {
               BuildHash();
               
               string encrypted = RijndaelSimple.Encrypt(C_ENHANCED_VERSION_LICENSE, Hash, RijndaelSimple.saltValue,
                                  RijndaelSimple.hashAlgorithm, RijndaelSimple.passwordIterations,
                                  RijndaelSimple.initVector, RijndaelSimple.keySize);

               File.WriteAllText(C_LICENSE_FILE, encrypted);

               return true;
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex);
               return false;
           }
          

       }

       public static bool IsEhnancedVersion()
       {
           BuildHash();
           if (!File.Exists(C_LICENSE_FILE))
           {
               return false;
           }
           string contents = File.ReadAllText(C_LICENSE_FILE);
           string plain = DecryptLicense(contents);

           if (plain.Equals(C_ENHANCED_VERSION_LICENSE))
           {
               return true;
           }

           return false;
       }
       

        private static string DecryptLicense(string licenseContents)
       {
           return RijndaelSimple.Decrypt(licenseContents, Hash, RijndaelSimple.saltValue,
                                  RijndaelSimple.hashAlgorithm, RijndaelSimple.passwordIterations,
                                  RijndaelSimple.initVector, RijndaelSimple.keySize);

            
       }

       private static void BuildHash()
       {
         
           string ret = "";
           byte[] arr = SHA1_Hash.DigestMessage(GetId());
           foreach (byte b in arr)
           {
               ret += b.ToString()+ ";";
           }

           hash = ret;
       }

       public static string GetId()
       {
          
           ManagementObjectSearcher searcher;
           string[] keys = new string[] { "Win32_baseboard", "Win32_Processor" };
           string ret = "";

           searcher = new ManagementObjectSearcher("select * from " + keys[0]);
           var mobos = searcher.Get();

           foreach (var m in mobos)
           {
               foreach (PropertyData PC in m.Properties)
               {
                   if (PC.Name.Equals("SerialNumber") || PC.Name.Equals("Product"))
                   {
                       ret += PC.Value;
                   }
                  
               }
           }


           searcher = new ManagementObjectSearcher("select * from " + keys[1]);
           mobos = searcher.Get();

           foreach (var m in mobos)
           {
               foreach (PropertyData PC in m.Properties)
               {
                   if (PC.Name.Equals("ProcessorId"))
                   {
                       ret += PC.Value;
                   }
               }
           }


           return ret;

       }
    }
}
