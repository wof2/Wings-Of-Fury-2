using System.Collections.Generic;
using Mogre;

namespace Wof.View
{
    /// <summary>
    /// Implementuj¹ca klasa posiada scenenode'y do których mo¿e zostaæ przytwierdzona kamera.
    ///  
    /// </summary>
    internal interface CameraHolder
    {
        /// <summary>
        /// Zwraca listê node'ów do których mo¿na przyczepiæ kamerê. Pierwszy element listy to kamera domyœlna
        /// </summary>
        /// <returns></returns>
        List<SceneNode> GetCameraHolders();

        /// <summary>
        /// Resetuje pozycjê node'ów do oryginalnej pozycji
        /// </summary>
        void ResetCameraHolders();
    }
}