using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.Model.Level.Infantry
{
    /// <author>Kamil S³awiñski</author>
    public class General : Soldier
    {

        /// <summary>
        /// Publiczny konstruktor jednoparametrowy.
        /// </summary>
        /// <param name="posX">Pozycja startowa zolnierza (mierzona w tilesIndex.</param>
        /// <param name="direct">Kierunek w ktorym sie porusza.(Prawo,Lewo)</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <author>Kamil S³awiñski</author>
        /// <param name="offset"></param>
        public General(float posX, Direction direct, Level level, float offset)
            : base(posX,direct,level,offset)
        {

        }


    }
}
