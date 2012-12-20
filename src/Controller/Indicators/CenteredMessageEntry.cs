using System;
using Mogre;
using Wof.Misc;

namespace Wof.Controller.Indicators
{
    public class CenteredMessageEntry : MessageEntry
    {
        private Viewport viewport;
        

        protected void CenterMessage()
        {
               //Vector2 dim = ViewHelper.GetTextDimensions(Message, (FontPtr)Mogre.FontManager.Singleton.GetByName(Wof.Languages.FontManager.CurrentFont), CharHeight * viewport.ActualHeight, viewport);
             //  Vector2 pos = new Vector2((1 - dim.x) * 0.5f,(1 - dim.y)*0.5f);
             

            x = 0.15f; y = 0.30f - CharHeight;
        }

        public CenteredMessageEntry(Viewport viewport, uint time, string message, float charHeight, ColourValue colourTop, ColourValue colourBottom, bool blinking, bool permanent) : base(0, 0, time, message, charHeight, colourTop, colourBottom, blinking, permanent, false)
        {
            this.viewport = viewport;
            this.noBackground = true;
            CenterMessage();
         
        }

    


        public CenteredMessageEntry(Viewport viewport, uint time, string message) : base(time, message)
        {
            this.viewport = viewport;
            this.noBackground = true;
            CenterMessage();
        }

       
        public CenteredMessageEntry(Viewport viewport, string message, bool blinking, bool permanent)
            : base(0, 0, message, blinking, permanent)
        {
            this.viewport = viewport;
            this.noBackground = true;
            CenterMessage();
        }

        public CenteredMessageEntry(Viewport viewport, string message) : base(0, 0, message)
        {
            this.viewport = viewport;
            this.noBackground = true;
            CenterMessage();
        }

        public CenteredMessageEntry(Viewport viewport, string message, uint time) : base(0, 0, message, time)
        {
            this.viewport = viewport;
            this.noBackground = true;
            CenterMessage();
        }

       
    }
}