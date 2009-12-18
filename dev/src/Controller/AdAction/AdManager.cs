using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AdManaged;
using Mogre;
using Timer=Mogre.Timer;
using System.Threading;
using Wof.Model.Level.Common;

namespace Wof.Controller.AdAction
{
    

    public class AdManager
    {
        public const bool C_DEBUG_MODE = true;
        public const string C_AD_KEY = "bea3f0f70c9ae51eba5cf04184d96b99";
   
        public const int C_AD_DOWNLOAD_TIMEOUT = 50000;
        public const int C_CONNECT_TIMEOUT = 2000;
        public const string C_ADS_DIR = "..\\..\\media\\materials\\textures\\ads/";
        /// <summary>
        /// Czas jaki musi uplynac miedzy reklamami zeby byly pokazane
        /// </summary>
        public const int C_MIN_AD_INTERVAL = 5000; 
        
        

        private List<Ad> ads = new List<Ad>();
        private Dictionary<int, bool> downloadingAds = new Dictionary<int, bool>();
        private Ad currentAd = null;

        private CommercialAdAction adAction;
        private static readonly AdManager singleton = new AdManager();

		private uint lastRegisterImpression = 0;

        
        private Timer timer = new Timer();
           
    
        public enum AdStatus
        {
            NO_ADS, TIMEOUT, OK, DOWNLOAD_FAILED
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

       
       



        public void Work()
        {
        	lock(this)
        	{
            	adAction.Work();
        	}
        }

        private AdManager()
        {
            timer.Reset();
            adAction = new CommercialAdAction();
            int result = AdAction.Init(C_AD_KEY, C_ADS_DIR, C_CONNECT_TIMEOUT);
            if(result == 0)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Error connecting adserver");
            }
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
                TextureManager.Singleton.Load(path, "Ads");
                return path;
            }
            catch (Exception ex)
            {
                // nie można zdekodować obrazka? pobieranie jednak sie nie udalo?
                if (OgreException.IsThrown)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch
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
            int id1 = 0;
            
            try
            {              
                id1 = adAction.Get_Ad_For_Zone(zone, ratio);      
                Console.WriteLine(id1);
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
                    Work();
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
	                Work();
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
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="downloadMsTimeout"></param>
        /// <param name="allowedSizes">Jesli null to nie ma ograniczenia wielkosci</param>
        /// <param name="adDownloadedCallback"></param>
        /// <returns></returns>
        public AdStatus GetAdAsync(string zone, float ratio, out int id)
        {
            id = 0;
            try
            {              
                id = adAction.Get_Ad_For_Zone(zone, ratio);      
                Console.WriteLine(id);
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
		                    Work();
		                    System.Threading.Thread.Sleep(100);		                   
		                }
		
		                }
		               ).Start(id);
    
                return AdStatus.OK;
            }
            return AdStatus.NO_ADS;
        }
        
        

        
        public void CloseAd(Ad ad)
        {
        	if(ads.Contains(ad)) ads.Remove(ad);
            adAction.Close_Ad(ad.id);
        }
    }
}
