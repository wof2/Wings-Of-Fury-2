using System;
using System.Globalization;
using Mogre;

namespace Wof.Controller.Indicators
{
    /// <summary>
    /// Klasa przechowuj¹ca informacje o pojedynczej wiadomoœci. 
    /// x, y - wspó³rzêdne w zakresie [0-1], licz¹c od lewego górnego rogu ekranu (nale¿y pamiêtaæ o tym, ¿e istnieje niewielki margines)
    /// time - czas przez jaki pokazywana jest wiadomoœæ.
    /// </summary>
    public class MessageEntry
    {
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

        private float x;

        public float X
        {
            get { return x; }
        }

        public void IncreaseX(float x)
        {
            this.x += x;
        }

        private float y;

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

        public String ColourTop
        {

            get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(colourTop.r), StringConverter.ToString(colourTop.g), StringConverter.ToString(colourTop.b)); }
        }

        private ColourValue colourBottom;

        public String ColourBottom
        {
            get { return String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(colourBottom.r), StringConverter.ToString(colourBottom.g), StringConverter.ToString(colourBottom.b)); }
        }


        private static ColourValue defaultColourTop = new ColourValue(0.8f, 0.34f, 0.34f);

        public static ColourValue DefaultColourTop
        {
            get { return defaultColourTop; }
        }

        private static ColourValue defaultColourBottom = new ColourValue(0.8f, 0.1f, 0.1f);

        public static ColourValue DefaultColourBottom
        {
            get { return defaultColourBottom; }
        }


        public MessageEntry(float x, float y, uint time, String message, float charHeight, ColourValue colourTop,
                            ColourValue colourBottom, bool blinking, bool permanent)
        {
            this.x = x;
            this.y = y;
            this.time = time;
            this.message = message;
            this.charHeight = 0.03f;
            this.colourTop = colourTop;
            this.colourBottom = colourBottom;
            this.blinking = blinking;
            this.permanent = permanent;
                    
        }

        public MessageEntry(uint time, String message, float charHeight, ColourValue colourTop, ColourValue colourBottom)
            : this(0, 0, time, message, charHeight, colourTop, colourBottom, false, false)

        {
        }

        public MessageEntry(uint time, String message)
            : this(0, 0, time, message, 0.03f, DefaultColourTop, DefaultColourBottom, false, false)
        {
        }

        public MessageEntry(float x, float y, String message, bool blinking, bool permanent)
            : this(x, y, 3000, message, 0.03f, DefaultColourTop, DefaultColourBottom, blinking, permanent)
        {
        }

        public MessageEntry(float x, float y, String message)
            : this(x, y, message, false, false)
        {
        }

        public MessageEntry(String message) : this(4000, message)
        {
        }
    }
}