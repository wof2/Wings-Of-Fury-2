using System;
using System.Collections.Generic;
using System.Text;
using Wof.Controller;

namespace Wof.Model.Exceptions
{
   
    public class LevelUnavailableException : Exception
    {
        public LevelUnavailableException(string fileName)
            : base(String.Format("Level: {0} can be run under " + EngineConfig.C_GAME_NAME + " Enhanced version", fileName))
        {
            base.Source = fileName;
        }
    }
}
