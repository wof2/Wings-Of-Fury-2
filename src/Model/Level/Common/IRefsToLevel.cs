using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.Model.Level.Common
{
    public interface IRefsToLevel
    {
        /// <summary>
        /// Ustawia prywatna referencje do planszy.
        /// </summary>
        Level LevelProperties
        { 
            set;
        }


    }
}
