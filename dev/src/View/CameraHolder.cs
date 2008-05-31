using System.Collections.Generic;
using Mogre;

namespace Wof.View
{
    /// <summary>
    /// Implementuj�ca klasa posiada scenenode'y do kt�rych mo�e zosta� przytwierdzona kamera.
    ///  
    /// </summary>
    internal interface CameraHolder
    {
        /// <summary>
        /// Zwraca list� node'�w do kt�rych mo�na przyczepi� kamer�. Pierwszy element listy to kamera domy�lna
        /// </summary>
        /// <returns></returns>
        List<SceneNode> GetCameraHolders();

        /// <summary>
        /// Resetuje pozycj� node'�w do oryginalnej pozycji
        /// </summary>
        void ResetCameraHolders();
    }
}