using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.Model.Level.LevelTiles
{
    interface IDestroyable
    {

        bool IsDestroyed
        {
            get;
        }

        void Destroy();

    }
}
