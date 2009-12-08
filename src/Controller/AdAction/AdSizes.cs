using System;
using AdManaged;
using Mogre;
using Wof.Model.Level.Common;

namespace Wof.Controller.AdAction
{

    public class AdSizeUtils
    {
        

        public static PointD ScaleAdToDisplay(Pair<uint, uint> textureDimensions, PointD targetSize, bool recalcRatio)
        {
           
            float scale;
            // reklamy musza zmiescic siê na targetSize
            if (textureDimensions.first > targetSize.X)
            {
                scale = 1.0f * targetSize.X / textureDimensions.first; // jesli mialoby wyjsc za ekran
            }
            else
            {
                scale = 1.0f * textureDimensions.first / targetSize.X; 
            }

            if(textureDimensions.second * scale > targetSize.Y)
            {
                // jesli mimo skalowania w osi X dalej nie miescimy siê w osi Y nalezy przeprowadzic skalowanie w Y (wtedy zmiescimy sie w obu osiach)
                if (textureDimensions.second > targetSize.Y)
                {
                    scale = 1.0f * targetSize.Y / textureDimensions.second; // jesli mialoby wyjsc za ekran
                }
              //  return new PointD(scale * prop, scale);
            }
            else
            {
              
            }
            // proporcja ma takze by zachowana
            float prop;
            if (recalcRatio)
            {
                prop = 1.0f / ((1.0f * textureDimensions.first / textureDimensions.second) / (1.0f * targetSize.X / targetSize.Y));
 
            }
            else
            {
                prop = 1.0f;
            }
            

            return new PointD(scale, scale * prop);
          
            
        }


      


        /// <summary>
        /// Nale¿y wkleiæ wszystkie klasy które implementuj¹ IAdSize z wyj¹tkiem AdSize (ktory reprezentuej customowy rozmiar)
        /// </summary>
        /// <returns></returns>
        public static IAdSize[] GetAllSizes()
        {
            return new IAdSize[] { new Billboard_1024x1024(), new Billboard_512x1024(), new Billboard_1024x512(), new Billboard_1024x256(), new Billboard_1024x128()  };
        }

        public static IAdSize[] GetSizesGreaterEqual(int width, int height)
        {
            return GetSizesGreaterEqual(new AdSize(width, height));
        }

        public static IAdSize[] GetSizesSmallerEqual(int width, int height)
        {
            return GetSizesSmallerEqual(new AdSize(width, height));
        }


        public static IAdSize[] GetSizesGreaterEqual(AdSize adSize)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().X >= adSize.getSize().X && size.getSize().Y >= adSize.getSize().Y; });
        }

        public static IAdSize[] GetSizesSmallerEqual(AdSize adSize)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().X <= adSize.getSize().X && size.getSize().Y <= adSize.getSize().Y; });
        }


        public static IAdSize[] GetSizesOfHeight(int height)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().Y == height; });
        }

        public static IAdSize[] GetSizesOfWidth(int width)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().X == width; });
        }

        public static IAdSize[] GetSizesOfHeightSmallerEqual(int height)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().Y <= height; });
        }

        public static IAdSize[] GetSizesOfHeightGreaterEqual(int height)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().Y >= height; });
        }


        public static IAdSize[] GetSizesOfWidthSmallerEqual(int width)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().X <= width; });
        }

        public static IAdSize[] GetSizesOfWidthGreaterEqual(int width)
        {
            return Array.FindAll(GetAllSizes(), delegate(IAdSize size) { return size.getSize().X >= width; });
        }
    }


    public class AdSize : IAdSize
    {
        private readonly PointD size;
        #region Implementation of IAdSize


    
       public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if(obj is IAdSize)
            {
                return  (obj as IAdSize).getSize().X == this.getSize().X && (obj as IAdSize).getSize().Y == this.getSize().Y;
            }
          
            return base.Equals(obj);
            
        }

// override object.GetHashCode
        public override int GetHashCode()
        {
            return size.GetHashCode();
        }
        public AdSize(AdList ad)
        {
            size = new PointD(ad.width, ad.height);
        }

        public AdSize(int width, int height)
        {
            size = new PointD(width, height);
        }
        public AdSize(PointD size)
        {
            this.size = size;
        }

        public PointD getSize()
        {
            return size;
        }

        #endregion
    }

    public interface IAdSize
    {
        PointD getSize();
    };

    #region AdSizes 
    class Billboard_1024x1024 : IAdSize
    {
        public PointD getSize()
        {
            return new PointD(1024, 1024);
        }
    };
   
    class Billboard_512x1024 : IAdSize
    {
        public PointD getSize()
        {
            return new PointD(512, 1024);
        }
    };

    class Billboard_1024x512 : IAdSize
    {
        public PointD getSize()
        {
            return new PointD(1024, 512);
        }
    };

    class Billboard_1024x256 : IAdSize
    {
        public PointD getSize()
        {
            return new PointD(1024, 256);
        }
    };

    class Billboard_1024x128 : IAdSize
    {
        public PointD getSize()
        {
            return new PointD(1024, 128);
        }
    };

    #endregion
          
}