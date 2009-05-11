using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wof.Misc
{
    public class IniFile
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            string lpReturnString,
            int nSize,
            string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFilename);
    }

    /// <summary>
	/// Base class for using the ini file as a configuration source
	/// </summary>
	public abstract class IniFileConfiguration<IniSection>
	{
	    private string category;
	    
	    private string path;

		#region Öffentliche Konstruktoren

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pValueKey">The section key.</param>
        protected IniFileConfiguration(string pValueKey)
		{
			category = pValueKey;
			path = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + pValueKey ;
		}

	    #endregion

		#region Geschützte Methoden

        /// <summary>
        /// Returning a string value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetString(string key)
        {
            string returnString = new string(' ', 1000);
      
            IniFile.GetPrivateProfileString(
                category,
                key, "",
                returnString, 999,
                path + ".ini");

            if (returnString.IndexOf('\0') >= 0)
                returnString = returnString.Substring(0, returnString.IndexOf('\0'));			
            return returnString.Trim();
        }

        /// <summary>
        /// Returning an integer value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected int GetInteger(string name, int defaultValue)
		{
            try
            {
                return int.Parse(GetString(name));
            }
            catch (Exception)
            {
                return defaultValue;
            }
		}

        /// <summary>
        /// Returning a boolean value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
	    protected bool GetBoolean(string name, bool defaultValue)
	    {
            try
            {
                return bool.Parse(GetString(name));
            }
            catch (Exception)
            {
                return defaultValue;
            }
	    }

        /// <summary>
        /// Set an integer value to the ini file
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="intValue">The int value.</param>
        protected void SetInteger(string name, int intValue)
		{
            WriteString(name, intValue.ToString());
		}

        /// <summary>
        /// Set a boolean value to the ini file
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
	    protected void SetBoolean(string name, bool boolValue)
	    {
	        WriteString(name, boolValue.ToString());    
	    }

        /// <summary>
        /// Set a string value to the ini file
        /// </summary>
        /// <param name="key">Schlüsselwert</param>
        /// <param name="keyValue">Wert der gesetzt werden soll</param>
        /// <returns></returns>
        protected void WriteString(string key, string keyValue)
        {
         
            IniFile.WritePrivateProfileString(
                category,
                key, keyValue,
                path + ".ini");
        }

		#endregion

        #region Members to overwrite

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>The section.</value>
        public abstract IniSection Value
        {
            get ;
            set ;
        }

        #endregion
    }
}
