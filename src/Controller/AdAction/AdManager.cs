using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AdManaged;
using Mogre;
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
     

        private Dictionary<string, List<Ad>> ads = new Dictionary<string, List<Ad>>();
        private bool downloadingAds = false;
        private Ad currentAd = null;

        private CommercialAdAction adAction;
        private static readonly AdManager singleton = new AdManager();


        private string currentZone;

        public enum AdStatus
        {
            NO_ADS, TIMEOUT, OK, DOWNLOAD_FAILED
        } ;

      
      

        public class Ad
        {
            public int id;
            public string path;
            public bool animated;

            public string zone;

            public Ad(int id, string path, bool animated, string zone)
            {
                this.id = id;
                this.path = path;
                this.animated = animated;
                this.zone = zone;
            }

            public bool Equals(Ad obj)
            {
                if (obj.GetType() != typeof(Ad)) return false;
                return Equals((Ad)obj);
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

       
       

        public bool HasCurrentAd
        {
            get { return currentAd != null; }
        }

        public bool DownloadingAds
        {
            get { return downloadingAds; }
        }

        public void Work()
        {
            adAction.Work();
        }

        private AdManager()
        {
          
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

            Ad ad = new Ad(id, path, animated, currentZone);

            List<Ad> list;
            if (ads.ContainsKey(currentZone))
            {
                list = ads[currentZone];
                if (!list.Contains(ad))
                {
                    list.Add(ad);
                }
            }
            else
            {
                list = new List<Ad>();
                list.Add(ad);
                ads.Add(currentZone, list);
            }
            
           


            downloadingAds = false;
        }

       


        public void RegisterImpression(Ad ad)
        {
            adAction.Add(ad.id);
        }
/*
        /// <summary>
        /// Zlicza wyświetlenie dla bieżącej reklamy
        /// </summary>
        public void RegisterImpression()
        {
            RegisterImpression(currentAd.id);
        }

        public string LoadAdTexture()
        {
            return LoadAdTexture(currentAd);
        }
*/
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


        public AdStatus GetAd(string zone, out Ad outAd)
        {
            return GetAd(zone, C_AD_DOWNLOAD_TIMEOUT, AdDownloaded, out outAd);
        }

        public AdStatus GetAd(string zone, int downloadMsTimeout, AdDownloaded adDownloadedCallback, out Ad outAd)
        {
            return GetAd(zone, C_AD_DOWNLOAD_TIMEOUT, null, AdDownloaded, out outAd);
        }
        public AdStatus GetAd(string zone, int downloadMsTimeout, IAdSize[] allowedSizes, out Ad outAd)
        {
            return GetAd(zone, C_AD_DOWNLOAD_TIMEOUT, allowedSizes, AdDownloaded, out outAd);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="downloadMsTimeout"></param>
        /// <param name="allowedSizes">Jesli null to nie ma ograniczenia wielkosci</param>
        /// <param name="adDownloadedCallback"></param>
        /// <returns></returns>
        public AdStatus GetAd(string zone, int downloadMsTimeout, IAdSize[] allowedSizes, AdManaged.AdDownloaded adDownloadedCallback, out Ad outAd)
        {
            outAd = null;
            AdList[] adl;
            try
            {
              //  adAction.Get_Ad_For_Zone()
                adl = adAction.Get_Ad_Format_List_For_Zone(zone);
            }
            catch (Exception ex)
            {
                return AdStatus.DOWNLOAD_FAILED;
            }
            
           
            int id1 = 0;
            List<AdList> staticAds = new List<AdList>();
            for (int i = 0; i < adl.Length; i++)
            {
                bool existed = false;
                if(!ads.ContainsKey(zone))
                {
                    existed = false;
                }
                else
                {
                    Ad existing = ads[zone].Find(delegate(Ad ad)
                    {
                        return ad.id.Equals(adl[i].ad_id);
                    });
                    if (existing != null)
                    {
                        existed = true;
                    }
                }
                if (!existed) // nie mozna pobrac 2 razy tej samej reklamy
                {
                    if (allowedSizes == null)
                    {

                        staticAds.Add(adl[i]);

                    }
                    else
                    {
                        AdSize currentSize = new AdSize(adl[i]);
                       
                        if(Array.Find(allowedSizes, delegate(IAdSize size)
                           {
                               PointD s1 = size.getSize();
                               PointD s2 = currentSize.getSize();
                               return s1.X == s2.X && s1.Y == s2.Y;
                           }) != null  /*  && !adl[i].animated */)
                        {
                            staticAds.Add(adl[i]);
                        }
                        
                    }
                }
                //    id1 = adl[i].ad_id; // pobierz ostatnią
            }
            if(staticAds.Count > 0)
            {
                id1 = staticAds[staticAds.Count - 1].ad_id;
            }
            


            Timer timer = new Timer();
            timer.Reset();
            uint start, end;
            start = timer.Milliseconds;


            if (id1 != 0)
            {
                // jesli są reklamy
                downloadingAds = true;
                currentZone = zone;

                try
                {
                   
                    if (!adAction.Download_Ad(id1, adDownloadedCallback))
                    {
                        currentZone = null;
                        return AdStatus.DOWNLOAD_FAILED;
                    }
                }
                catch (Exception ex)
                {
                    return AdStatus.DOWNLOAD_FAILED;
                }
                

                //  System.Console.Write("Downloading " + id1);
                for (; DownloadingAds; )
                {
                    // System.Console.Write(".");
                    adAction.Work();
                    System.Threading.Thread.Sleep(100);
                    end = timer.Milliseconds;
                    if (end - start > downloadMsTimeout)
                    {
                        currentZone = null;
                        return AdStatus.TIMEOUT;
                    }
                }

                
                // przypisz dopiero co sciagnieta reklame
                currentAd = ads[zone].Find(delegate(Ad ad)
                                               {
                                                   return ad.id.Equals(id1);
                                               });
                outAd = currentAd;

                return AdStatus.OK;
            }
            return AdStatus.NO_ADS;
        }

        /*
        public void CloseAd()
        {
            if(HasCurrentAd)
            {
                adAction.Close_Ad(currentAd.id);
                ClearCurrentAd();
            }
            
        }*/

        public void CloseAd(Ad ad)
        {
            adAction.Close_Ad(ad.id);
        }
    }
}
