using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.Controller.Screens
{
    class PositionedMessage
    {

        private float x;
        public float X
        {
            get { return x; }
        }


        private float ySpace;
        /// <summary>
        /// Odstêp w osi Y pod tym elementem
        /// </summary>
        public float YSpace
        {
            get { return ySpace; }
        }


        private float width;

        public float Width
        {
            get { return width; }
        }


        private float height;

        public float Height
        {
            get { return height; }
        }

        private String message;

        public String Message
        {
            get { return message; }
        }



        public PositionedMessage(float x, float ySpace, float width, float height, String message)
        {
            this.x = x;
            this.ySpace = ySpace;
            this.width = width;
            this.height = height;
            this.message = message;
        }
    }
}
