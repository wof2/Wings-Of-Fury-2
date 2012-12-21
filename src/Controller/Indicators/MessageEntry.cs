using System;
using System.Globalization;
using Mogre;
using Wof.Languages;
using Wof.Misc;

namespace Wof.Controller.Indicators
{
    /// <summary>
    /// Klasa przechowuj¹ca informacje o pojedynczej wiadomoœci. 
    /// x, y - wspó³rzêdne w zakresie [0-1], licz¹c od lewego górnego rogu ekranu (nale¿y pamiêtaæ o tym, ¿e istnieje niewielki margines)
    /// time - czas przez jaki pokazywana jest wiadomoœæ.
    /// </summary>
    public class MessageEntry
    {
        protected bool noBackground = false;

        private bool blinking;

        public bool Blinking
        {
            get { return blinking; }
        }


        private bool permanent;

        public bool Permanent
        {
            get { return permanent; }
        }

        protected float x;

        public float X
        {
            get { return x; }
        }

        public void IncreaseX(float x)
        {
            this.x += x;
        }

        protected float y;

        public float Y
        {
            get { return y; }
        }

        public void IncreaseY(float y)
        {
            this.y += y;
        }


        private String message;

        public String Message
        {
            get { return message; }
             //set {  message = value; }
        }

        private uint time;

        public uint Time
        {
            get { return time; }
        }

        private float charHeight;

        public float CharHeight
        {
            get { return charHeight; }
        }

        public String getCharHeightString()
        {
            return StringConverter.ToString(charHeight);
            //return String.Format(EngineConfig.Nfi, "{0:f}", charHeight);
        }

        private ColourValue colourTop;
        public ColourValue ColourTop
        {
            get { return colourTop; }
        }

        public String ColourTopString
        {

            get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(colourTop.r), StringConverter.ToString(colourTop.g), StringConverter.ToString(colourTop.b)); }
        }

        private ColourValue colourBottom;

        public ColourValue ColourBottom
        {
            get { return colourBottom; }
        }
        public String ColourBottomString
        {
            get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(colourBottom.r), StringConverter.ToString(colourBottom.g), StringConverter.ToString(colourBottom.b)); }
        }

        static MessageEntry()
        {
            RestoreDefaultColours();
        }

        private static ColourValue defaultColourTop;

        public static ColourValue DefaultColourTop
        {
            get { return defaultColourTop; }
        }

        public static String DefaultColourTopString
        {
            get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(DefaultColourTop.r), StringConverter.ToString(DefaultColourTop.g), StringConverter.ToString(DefaultColourTop.b)); }
        }


       private static ColourValue defaultColourBottom;

    

        public static ColourValue DefaultColourBottom
        {
            get { return defaultColourBottom; }
        }

        public static String DefaultColourBottomString
       {
           get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(DefaultColourBottom.r), StringConverter.ToString(DefaultColourBottom.g), StringConverter.ToString(DefaultColourBottom.b)); }
       }

        public static void RestoreDefaultColours()
        {
            defaultColourBottom  = new ColourValue(0.1f, 0.1f, 0.1f);
            defaultColourTop = new ColourValue(0.3f, 0.3f, 0.3f);

        }
        public static void SetDefaultColours(ColourValue top, ColourValue bottom)
        {
            defaultColourBottom = bottom;
            defaultColourTop = top;
        }

        public int GetCharsPerLine(float containerWidth, float screenHeight)
        {
            return ViewHelper.GetMaximumCharsPerLine(Languages.FontManager.CurrentFont, Message, containerWidth, screenHeight*charHeight);
        }

        public String GetMessageAdjustedByContainerWidth(float containerWidth,  float screenHeight)
        {
            return LanguageResources.SplitInsertingNewLinesByLength(Message, GetCharsPerLine(containerWidth, screenHeight));
        }

       
        public bool NoBackground
        {
            get { return noBackground; }
        }

        protected virtual void Init()
        {
            
        }

        public MessageEntry(float x, float y, uint time, String message, float charHeight, ColourValue colourTop,
                            ColourValue colourBottom, bool blinking, bool permanent, bool noBackground)
        {
            this.x = x;
            this.y = y;
            this.time = time;
            this.message = message;
            this.charHeight = charHeight;
            this.colourTop = colourTop;
            this.colourBottom = colourBottom;
            this.blinking = blinking;
            this.permanent = permanent;
            this.noBackground = noBackground;
            Init();
                    
        }

        public MessageEntry(uint time, String message, float charHeight, ColourValue colourTop, ColourValue colourBottom)
            : this(0, 0, time, message, charHeight, colourTop, colourBottom, false, false, false)

        {
        }

      

        public MessageEntry(uint time, String message)
            : this(0, 0, time, message, EngineConfig.CurrentFontSize, DefaultColourTop, DefaultColourBottom, false, false, false)
        {
        }

    

        public MessageEntry(float x, float y, String message, bool blinking, bool permanent)
            : this(x, y, 3000, message, EngineConfig.CurrentFontSize, DefaultColourTop, DefaultColourBottom, blinking, permanent, false)
        {
        }

        public MessageEntry(float x, float y, String message, bool blinking, bool permanent, bool noBackground)
            : this(x, y, 3000, message, EngineConfig.CurrentFontSize, DefaultColourTop, DefaultColourBottom, blinking, permanent, false)
        {
        }

        public MessageEntry(float x, float y, String message)
            : this(x, y, message, false, false)
        {
        }

        public MessageEntry(float x, float y, String message, uint time)
            : this(x, y, time, message, EngineConfig.CurrentFontSize, DefaultColourTop, DefaultColourBottom, false, false, false)
        {
        }

        public MessageEntry(String message) : this(4000, message)
        {
        }
    }
}