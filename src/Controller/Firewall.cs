/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;
using NATUPNPLib;
using NETCONLib;
using NetFwTypeLib;
using System.Windows.Forms;

namespace Wof.Controller
{
	
	/// <summary>
	/// Description of Firewall.
	/// </summary>
	public class Firewall
	{
		private  const string CLSID_FIREWALL_MANAGER = 
	      "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
		private static NetFwTypeLib.INetFwMgr GetFirewallManager()
		{
		    Type objectType = Type.GetTypeFromCLSID(
		          new Guid(CLSID_FIREWALL_MANAGER));
		    return Activator.CreateInstance(objectType) 
		          as NetFwTypeLib.INetFwMgr;
		}
	
	
			// ProgID for the AuthorizedApplication object
		private const string PROGID_AUTHORIZED_APPLICATION =  "HNetCfg.FwAuthorizedApplication";
		
		public static bool AuthorizeApplication(string title, string applicationPath,
		    NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipVersion )
		{   
		  // Create the type from prog id
		  Type type = Type.GetTypeFromProgID(PROGID_AUTHORIZED_APPLICATION);
		  INetFwAuthorizedApplication auth = Activator.CreateInstance(type)
		      as INetFwAuthorizedApplication;
		  auth.Name  = title;
		  auth.ProcessImageFileName = applicationPath;
		  auth.Scope = scope;
		  auth.IpVersion = ipVersion;
		  auth.Enabled = true;
		
		
		  INetFwMgr manager = GetFirewallManager();
		  try
		  {
		    manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
		  }
		  catch (Exception ex)
		  {
		    return false;
		  }
		  return true;
		}
		public static void AddException()
		{
			
			AuthorizeApplication (Game.Name + " " + EngineConfig.C_WOF_VERSION, Application.ExecutablePath,  NET_FW_SCOPE_.NET_FW_SCOPE_ALL,
                NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
		}
	}
}
