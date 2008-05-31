using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace FSLOgreCSDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SoundDemo app = new SoundDemo();
                app.Go();
            }
            catch
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    Mogre.Demo.ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}
