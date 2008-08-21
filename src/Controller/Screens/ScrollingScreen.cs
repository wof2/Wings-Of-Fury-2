using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BetaGUI;
using Mogre;
using Wof.Languages;
using FontManager=Mogre.FontManager;

namespace Wof.Controller.Screens
{
    /// <summary>
    /// Abstrakcyjna klasa reprezentuj¹ca automatycznie przewijaj¹cy siê screen w menu
    /// <author>Adam Witczak</author>
    /// </summary>
    abstract class ScrollingScreen : AbstractScreen, BetaGUIListener
    {
        protected bool startFromBottom = true;
        
        /// <summary>
        /// Margines na dole ekranu
        /// </summary>
        protected float bottomMargin = 10.0f;
        public float BottomMargin
        {
            get { return bottomMargin; }
        }

        protected float speed = 15.0f;
        /// <summary>
        /// Szybkoœæ przewijania elementów wyra¿ona w px na sek.
        /// </summary>
        public float Speed
        {
            get { return speed; }
        }

        protected bool enabled = false;
        public bool Enabled
        {
            set { enabled = value; }
            get { return enabled; }
        }

        protected List<PositionedMessage> messages;
        private List<OverlayContainer> messageOverlays;


        protected Callback cc;
        protected Window guiWindow;

        public ScrollingScreen(GameEventListener gameEventListener, SceneManager sceneMgr, Viewport viewport,
                               Camera camera, bool startFromBottom, float speed) : base(gameEventListener, sceneMgr, viewport, camera)
        {
            messages = new List<PositionedMessage>();
            messageOverlays = new List<OverlayContainer>();
            this.startFromBottom = startFromBottom;
            this.speed = speed;
                      
        }

        
     
        protected override void CreateGUI()
        {
            mGui = new GUI(Wof.Languages.FontManager.CurrentFont, 22);
            createMouse();
            string message = "";
           
           
            guiWindow = mGui.createWindow(new Vector4(0, 0, viewport.ActualWidth, viewport.ActualHeight),
                                          "bgui.window", (int)wt.NONE, message);
            cc = new Callback(this);
            List<Button> temp = buildButtons();

            initButtons(temp.Count, (int)getBackButtonIndex());
            buttons = temp.ToArray();
            messages = buildMessages();
            
            float y = 0;
            
            if (startFromBottom)
            {
                y += guiWindow.h;
                foreach (Button b in buttons)
                {
                    b.Translate(new Vector2(0, y));
                }
                
            }

            foreach(PositionedMessage m in messages)
            {
                 
                 messageOverlays.Add(guiWindow.createStaticText(new Vector4(m.X, y, m.Width, m.Height), m.Message));
                 y += m.YSpace;
            }
                     
            selectButton(getBackButtonIndex());
            guiWindow.show();
            enabled = true;
        }

        protected abstract List<Button> buildButtons();

        /// <summary>
        /// Zwraca index przycisku 'back'. Jeœli nie ma przycisku 'back' powinno zwróciæ -1
        /// </summary>
        /// <returns></returns>
        protected abstract int getBackButtonIndex();

        /// <summary>
        /// Buduje listê obiektów PositionedMessage - wyœwietlanych tekstów
        /// </summary>
        /// <returns></returns>
        protected abstract List<PositionedMessage> buildMessages();
        

        /// <summary>
        /// Przewija wszystkie elementy
        /// </summary>
        /// <param name="evt"></param>
        public override void FrameStarted(FrameEvent evt)
        {
           
            if (!Enabled) return;
            base.FrameStarted(evt);
           
            float newTop;
            float maxY = float.MinValue;
            float step = (speed*evt.timeSinceLastFrame) * (viewport.ActualHeight / 1050.0f); // normalizacja do szybkosci scrollowania na ekranie 1680/1050


            foreach(OverlayContainer o in messageOverlays)
            {
                float top = StringConverter.ParseReal(o.GetParameter("top"));
                newTop = (top - step);
                o.SetParameter("top",  StringConverter.ToString(newTop));

                if (newTop + StringConverter.ParseReal(o.GetParameter("height")) + bottomMargin > maxY)
                {
                    maxY = newTop + StringConverter.ParseReal(o.GetParameter("height")) + bottomMargin;
                }
            }

            for (int j = 0; j < buttons.Length; j++ )
            {
                Button b = buttons[j];
                if (b.y + b.h < 0  && j != backButtonIndex)
                {
                    buttons[j].killButton();
                    continue;
                }
                b.Translate(new Vector2(0, -step));
                if (b.y + b.h > maxY + bottomMargin)
                {
                    maxY = b.y + b.h + bottomMargin;
                }
            }

            // stop condition
            if(maxY < guiWindow.h) enabled = false;

        }

        #region BetaGUIListener Members

        public abstract void onButtonPress(Button referer);
        

        #endregion
    }
}
