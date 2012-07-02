using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using AdManaged;
using Mogre;
using Wof.Model.Level.Common;
using Timer = Mogre.Timer;
using Wof.View.Effects;

namespace Wof.Controller.AdAction
{
    

    public class AdManager
    {
        public const bool C_DEBUG_MODE = true;
        public const string C_AD_KEY = "bea3f0f70c9ae51eba5cf04184d96b99";
   
        public const int C_AD_DOWNLOAD_TIMEOUT = 5000;
        public const int C_CONNECT_TIMEOUT = 2000;
        public const string C_ADS_DIR = "..\\..\\media\\materials\\textures\\ads/";
        /// <summary>
        /// Czas jaki musi uplynac miedzy reklamami zeby byly pokazane
        /// </summary>
        public const int C_MIN_AD_INTERVAL = 5000; 
        
        

        private List<Ad> ads = new List<Ad>();
        private Dictionary<int, bool> downloadingAds = new Dictionary<int, bool>();
     
        private CommercialAdAction adAction;
        private AdHelper3D adHelper3D;
        
        private static readonly AdManager singleton = new AdManager();

		private uint lastRegisterImpression = 0;

        
        private Timer timer = new Timer();

        public delegate void AdDownloadedAsync(Ad ad);
           
    
        public enum AdStatus
        {
            NO_ADS, TIMEOUT, OK, DOWNLOAD_FAILED, ADS_DISABLED
        } ;

      
      

        public class Ad
        {
            public int id;
            public string path;
            public bool animated;

        
            public Ad(int id, string path, bool animated)
            {
                this.id = id;
                this.path = path;
                this.animated = animated;
            
            }

            public bool Equals(Ad obj)
            {
                if (obj.GetType() != typeof(Ad)) return false;
                return ((Ad)obj).id.Equals(id);
            }

            public override int GetHashCode()
            {
                return id;
            }


        }
      

        public static AdManager Singleton
        {
            get { return singleton; }
        }

        public CommercialAdAction AdAction
        {
            get { return adAction; }
        }
        
          public AdHelper3D AdHelper3D
        {
            get { return adHelper3D; }
        }

        public bool ConnectionErrorOccured
        {
            get { return connectionErrorOccured; }
        }


        public void Work(Camera c)
        {
        	lock(this)
        	{
        		try
        		{
        			
        			adAction.Work();
            		adHelper3D.Work(adAction);
            		if(c != null) UpdateCamera(c);
        		}
        		catch(Exception)
        		{
        			
        		}
            	
        	}
        }


        private bool connectionErrorOccured = false;

        public bool TestConnection()
        {
            return true;
            try
            {
                connectionErrorOccured = false;
                WebRequest request = WebRequest.Create("http://openx.adveraction.pl/www/delivery/Mobile/standalone.php");
                using (WebResponse response = request.GetResponse())
                {
                   
                   //response.Headers[HttpResponseHeader.]
                }
            }
            catch (Exception ex)
            {
                if (LogManager.Singleton != null)
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Error connecting adserver");
                }
                connectionErrorOccured = true;
            }

            
         

            return !connectionErrorOccured;

        }

        ~AdManager()
        {
            if(adHelper3D != null)
            {
                //adHelper3D.Clear();
                //adHelper3D.Dispose();
                //adHelper3D = null;
            }
            if(adAction != null)
            {
                //adAction.CleanUp();
               // adAction = null;
            }
            
        }

        private AdManager()
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
            timer.Reset();
            adAction = new CommercialAdAction();
            adHelper3D = new AdHelper3D(0.02f, 80, 2000, 2);
           // int result = AdAction.Init(C_AD_KEY, C_ADS_DIR, C_CONNECT_TIMEOUT);
           // if (result == 0)
           // {


           // }
           
        }
        public void AdDownloaded(Ad ad)
        {
            AdDownloaded(ad.id, ad.path, ad.animated);
        }

        private void AdDownloaded(int id, string path, bool animated)
        {
            // System.Console.WriteLine("AD downloaded, path=" + path + ", anim=" + animated);

            Ad ad = new Ad(id, path, animated);
            ads.Add(ad);            
            downloadingAds[id] = false;
        }

       
        public bool CanShowAd()
        {
        	return (timer.Milliseconds - lastRegisterImpression) > C_MIN_AD_INTERVAL;
        }
        

        public void RegisterImpression(Ad ad)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
            lastRegisterImpression = timer.Milliseconds;
        	
            adAction.Add(ad.id);
        }

        /// <summary>
        /// Wczytuje daną reklamę z dysku i ładuje do TextureManagera. W przypadku błędu zwraca null
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public string LoadAdTexture(Ad ad)
        {
            if(ad == null) return null;

            string path = ad.path;
            try
            {
           //     Image s = new Image();

           //       Image k = s.Load("7178C1654343ECCF358E8EEA0271100B.gif", "General");
           //     Image k = s.Load("1.gif", "General");

             //   TextureManager.Singleton.Load("../../media/FE55EF898AF9D8FE122A83882B98C32D.gif", "Ads");
               if(path.EndsWith(".gif"))
                {
                 //   throw new Exception("nie ma obslugi gif");
                }
             
                TextureManager.Singleton.Load(path, "Ads");
                return path;
            }
           
            catch (SEHException ex)
            {
              
                // nie można zdekodować obrazka? pobieranie jednak sie nie udalo?
                if (OgreException.IsThrown)
                {
                    try
                    {
                        OgreException.ClearLastException();
                        File.Delete(path);
                        
                    
                    }
                    catch(Exception ex2)
                    {
                    }
                }

                return null;
            }
        }


        public AdStatus GetAd(string zone, float ratio, out Ad outAd)
        {
            return GetAd(zone, C_AD_DOWNLOAD_TIMEOUT, ratio, AdDownloaded, out outAd);
        }

        
       

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="downloadMsTimeout"></param>
        /// <param name="allowedSizes">Jesli null to nie ma ograniczenia wielkosci</param>
        /// <param name="adDownloadedCallback"></param>
        /// <returns></returns>
        public AdStatus GetAd(string zone, int downloadMsTimeout, float ratio, AdManaged.AdDownloaded adDownloadedCallback, out Ad outAd)
        {
            outAd = null;   
            if (EngineConfig.IsEnhancedVersion)
            {
                return AdStatus.ADS_DISABLED;
            }
                    
            int id1 = 0;
            
            try
            {
                id1 = adAction.Get_Ad_For_Zone(zone, ratio);      
              //  Console.WriteLine(id1);
            }
            catch (Exception ex)
            {
                return AdStatus.DOWNLOAD_FAILED;
            }
        


            Timer timer = new Timer();
            timer.Reset();
            uint start, end;
            start = timer.Milliseconds;


            if (id1 != 0)
            {
                // jesli są reklamy
                downloadingAds[id1] = true;
               
                try
                {
                   
                    if (!adAction.Download_Ad(id1, adDownloadedCallback))
                    {                      
                        return AdStatus.DOWNLOAD_FAILED;
                    }
                }
                catch (Exception ex)
                {
                    return AdStatus.DOWNLOAD_FAILED;
                }
                

                //  System.Console.Write("Downloading " + id1);
                for (; downloadingAds[id1]; )
                {
                    // System.Console.Write(".");
                    Work(null);
                    System.Threading.Thread.Sleep(100);
                    end = timer.Milliseconds;
                    if (end - start > downloadMsTimeout)
                    {
                        return AdStatus.TIMEOUT;
                    }
                }

                
                // przypisz dopiero co sciagnieta reklame
                outAd = ads.Find(delegate(Ad ad)
                                               {
                                                   return ad.id.Equals(id1);
                                               });
                

                return AdStatus.OK;
            }
            return AdStatus.NO_ADS;
        }
        
        
        public AdStatus GatherAsyncResult(int id, int downloadMsTimeout, out Ad outAd)
        {
            outAd = null;
            if (EngineConfig.IsEnhancedVersion)
            {
                return AdStatus.ADS_DISABLED;
            }
                  
        	
        	if(downloadingAds.ContainsKey(id)) 
        	{
        	   	// jeszcze nie skonczono
        	   	
        	   	Timer timer = new Timer();
	            timer.Reset();
	            uint start, end;
	            start = timer.Milliseconds;
	        	   	
	        	for (; downloadingAds[id]; )
	            {
	                // System.Console.Write(".");
	                Work(null);
	                System.Threading.Thread.Sleep(100);
	                end = timer.Milliseconds;
	                if (end - start > downloadMsTimeout)
	                {
	                   
	                    return AdStatus.TIMEOUT;
	                }
	            }
        		
        	} else 
        	{
        		return AdStatus.DOWNLOAD_FAILED;
        		
        	}
            // przypisz dopiero co sciagnieta reklame
	        outAd = ads.Find(delegate(Ad ad)
	                                       {
	                                           return ad.id.Equals(id);
	                                       });
            return AdStatus.OK;
    	   
        	
        }
        
        public void ClearDynamicAds()
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
        	AdHelper3D.Stop_Time();
        	AdHelper3D.Clear();
        }
        
      
        public void RemoveDynamicAd(AdQuadrangle3D quadrangle3D)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
            adHelper3D.Remove_Ad(quadrangle3D.GetBillboardId());
            
        }
        public AdQuadrangle3D AddDynamicAd(SceneManager sceneMgr, int id, Vector3 origin, Vector2 size, bool isPersistent)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return null;
            }
            Ad outAd = ads.Find(delegate(Ad ad)
	                                       {
	                                           return ad.id.Equals(id);
	                                       });
            
            LoadAdTexture(outAd);
            Quadrangle q = new Quadrangle(new PointD(0, 0), size.x, size.y);
            AdQuadrangle3D q3d = new AdQuadrangle3D(sceneMgr, outAd, isPersistent);
        	q3d.SetCorners3D(q, origin, outAd.path);
        	
        	float[][] corners = q3d.GetCorners3DArray();
            int billboardId = AdHelper3D.Add_Ad(id, 
        	                  corners[0][0], corners[0][1], corners[0][2],
        	                  corners[1][0], corners[1][1], corners[1][2],
        	                  corners[2][0], corners[2][1], corners[2][2]
        	                 );
        	                  
        	q3d.SetBillboardId(billboardId);       
        	AdHelper3D.Start_Time();
        
        	return q3d;
        }
        
        public void UpdateCamera(Camera c)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
            //Camera c = new Camera();
            Matrix4 proj = c.ProjectionMatrix;
            Matrix4 view = c.ViewMatrix;

        
            adHelper3D.Camera(new float[]{  
                              	proj.m00, proj.m01, proj.m02, proj.m03,
                              	proj.m10, proj.m11, proj.m12, proj.m13,
                              	proj.m20, proj.m21, proj.m22, proj.m23, 
                              	proj.m30, proj.m31, proj.m32, proj.m33                              
                              }, 
                              new float[]{  
                              	view.m00, view.m01, view.m02, view.m03,
                              	view.m10, view.m11, view.m12, view.m13,
                              	view.m20, view.m21, view.m22, view.m23, 
                              	view.m30, view.m31, view.m32, view.m33                              
                              }	);

           
                              
        	
        }
        /// <summary>
        /// Czy reklama dynamiczna jest widoczna z punktu widzenia API ? (czy spelnia wymagania)
        /// </summary>
        /// <param name="quadrangle3D"></param>
        /// <returns></returns>
        public bool IsDynamicAdVisible(AdQuadrangle3D quadrangle3D)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return false;
            }
            int corners;
            float angle, area, timer;
            bool visible = AdHelper3D.Get_Ad_State(quadrangle3D.GetBillboardId(), out corners, out angle, out area, out timer);

          //  Console.WriteLine(area + " " + timer + " " + angle + " " + corners);
            return visible;
        }

        public AdStatus GetAdAsync(string zone, float ratio, out int id)
        {
            return GetAdAsync(zone, ratio, out id, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="downloadMsTimeout"></param>
        /// <param name="allowedSizes">Jesli null to nie ma ograniczenia wielkosci</param>
        /// <param name="adDownloadedAsyncCallback"></param>
        /// <returns></returns>
        public AdStatus GetAdAsync(string zone, float ratio, out int id, AdDownloadedAsync adDownloadedAsyncCallback)
        {
            id = 0;
            if (EngineConfig.IsEnhancedVersion)
            {
                return AdStatus.ADS_DISABLED;
            }
            try
            {              
                id = adAction.Get_Ad_For_Zone(zone, ratio);      
               // Console.WriteLine(id);
            }
            catch (Exception ex)
            {
                return AdStatus.DOWNLOAD_FAILED;
            }
      
            if (id != 0)
            {
                try
                {   
                	downloadingAds[id] = true;
                    if (!adAction.Download_Ad(id, AdDownloaded))
                    {       
                    	downloadingAds.Remove(id);
                        return AdStatus.DOWNLOAD_FAILED;
                    }
                }
                catch (Exception ex)
                {
                	downloadingAds.Remove(id);
                    return AdStatus.DOWNLOAD_FAILED;
                }
               
                
                new Thread(delegate (object data) {
                        int adId = (int)data;
		                //System.Console.Write("Downloading " + adId);
		                for (; downloadingAds[adId]; )
		                {
		                   // System.Console.Write(".");
		                    Work(null);
		                    System.Threading.Thread.Sleep(100);		                   
		                }
                        if(adDownloadedAsyncCallback != null)
                        {
                            Ad adOut= ads.Find(delegate(Ad ad)
	                                       {
	                                           return ad.id.Equals(adId);
	                                       });
                            adDownloadedAsyncCallback(adOut);
                        }
		
		                }
		               ).Start(id);
    
                return AdStatus.OK;
            }
            return AdStatus.NO_ADS;
        }


       
        
        

        
        public void CloseAd(Ad ad)
        {
            if (EngineConfig.IsEnhancedVersion)
            {
                return;
            }
        	if(ads.Contains(ad))
        	{
        	    ads.Remove(ad);
        	}
            string path = ad.path;
            try
            {
                TextureManager.Singleton.Unload(path);
                TextureManager.Singleton.Remove(path);
                adAction.Close_Ad(ad.id);

            }
            catch (Exception)
            {
            }
            
           // AdHelper3D h = new AdHelper3D();
           // h.Add_Ad(
          
        }
    }
}
