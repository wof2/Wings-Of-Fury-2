using System;
using System.Collections.Generic;
using System.Text;
using Wof.Controller;
using Wof.Model.Level;

namespace wingitor
{
    public interface ISceneTest
    {
        void OnRegisterLevel(Level currentLevel);
       
        IFrameWork Framework
        {
            set; get;
        }

      
    }
}
