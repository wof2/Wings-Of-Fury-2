using System;
using System.Collections.Generic;
using System.Text;
using Wof.Controller;

namespace wingitor
{
    public interface IGameTest
    {
        String LevelFilename
        { 
            get;
        }
       

        IList<ISceneTest> SceneTests
        {
            get;
        }
    }
}
