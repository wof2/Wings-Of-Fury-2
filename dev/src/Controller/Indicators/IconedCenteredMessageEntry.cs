using System;
using Mogre;
using Wof.Misc;

namespace Wof.Controller.Indicators
{
    public class IconedMessageEntry : MessageEntry
    {
        private string icon = "";

        private Vector2 customIconDimensions = Vector2.ZERO;
        /*
        private Vector2 customIconPosition = Vector2.NEGATIVE_UNIT_X;

        public bool HasCustomIconPosition()
        {
            return customIconPosition != Vector2.NEGATIVE_UNIT_X;
        }
        */
        public IconedMessageEntry(MessageEntry entry, String icon) : this(entry.X, entry.Y, entry.Time, entry.Message, entry.CharHeight, entry.ColourTop, entry.ColourBottom, entry.Blinking, entry.Permanent, entry.NoBackground)
        {
            this.icon = icon;
        }

        public IconedMessageEntry(float x, float y, uint time, string message, float charHeight, ColourValue colourTop, ColourValue colourBottom, bool blinking, bool permanent, bool noBackground)
            : base(x, y, time, message, charHeight, colourTop, colourBottom, blinking, permanent, noBackground)
        {

        }
        public IconedMessageEntry(float x, float y, uint time, string message, float charHeight, ColourValue colourTop, ColourValue colourBottom, bool blinking, bool permanent) : base(x, y, time, message, charHeight, colourTop, colourBottom, blinking, permanent, false)
        {
     
        }

        public IconedMessageEntry(uint time, string message, float charHeight, ColourValue colourTop, ColourValue colourBottom) : base(time, message, charHeight, colourTop, colourBottom)
        {
        }

        public IconedMessageEntry(uint time, string message) : base(time, message)
        {
        }

        public IconedMessageEntry(float x, float y, string message, bool blinking, bool permanent) : base(x, y, message, blinking, permanent)
        {
        }

        public IconedMessageEntry(float x, float y, string message) : base(x, y, message)
        {
        }

        public IconedMessageEntry(float x, float y, string message, uint time) : base(x, y, message, time)
        {
        }

        public IconedMessageEntry(string message) : base(message)
        {
        }
        public void CenterIconOnScreen(Viewport viewport)
        {


            x =  ((1 - customIconDimensions.x) * 0.5f) ;
            y = ((1 - customIconDimensions.y)*0.5f);
        }


        public void UseAutoDectetedIconDimesions(Viewport viewport)
        {
            try
            {
                TexturePtr t = TextureManager.Singleton.Load(icon, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                if (t != null && t.SrcWidth > 0 && t.SrcHeight > 0)
                {

                    customIconDimensions = new Vector2(1.0f * t.SrcWidth / viewport.ActualWidth, 1.0f * t.SrcHeight / viewport.ActualHeight);
                }
                
            }
            catch(Exception)
            {
               
                
            }
           
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public Vector2 CustomIconDimensions
        {
            get { return customIconDimensions; }
            set { customIconDimensions = value; }
        }
        /*
        public Vector2 CustomIconPosition
        {
            get { return customIconPosition; }
            set { customIconPosition = value; }
        }*/
    }
}