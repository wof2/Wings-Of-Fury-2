using System;
using Mogre;
using Wof.Misc;

namespace Wof.Controller.Indicators
{
    public class IconedMessageEntry : MessageEntry
    {
        private string icon = "";


        public IconedMessageEntry(MessageEntry entry, String icon) : this(entry.X, entry.Y, entry.Time, entry.Message, entry.CharHeight, entry.ColourTop, entry.ColourBottom, entry.Blinking, entry.Permanent)
        {
            this.icon = icon;

        }

        public IconedMessageEntry(float x, float y, uint time, string message, float charHeight, ColourValue colourTop, ColourValue colourBottom, bool blinking, bool permanent) : base(x, y, time, message, charHeight, colourTop, colourBottom, blinking, permanent)
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

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
    }
}