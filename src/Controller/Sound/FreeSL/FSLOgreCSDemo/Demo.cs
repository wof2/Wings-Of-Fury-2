//Direct translation of the original fsl ogre demo

using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using FSLOgreCS;

namespace FSLOgreCSDemo
{
    class SoundDemo : Mogre.Demo.ExampleApplication.Example
    {
        FSLSoundManager soundManager = null;

        public override void CreateScene()
        {
            base.sceneMgr.AmbientLight = new ColourValue(0.0f, 0.0f, 0.0f);
            base.sceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;

            //position of camara
            camera.Position = new Vector3(500, 500, 500);
            camera.LookAt(new Vector3(0, 200, -300));
            soundManager = FSLSoundManager.Instance;
            soundManager.InitializeSound(base.camera, FSLOgreCS.FreeSL.FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND); //Init sound system

            Light l;
            Entity ent;
            SceneNode nodo2;
            FSLSoundObject sonido1;
            //testing streaming with this sound
           
            FSLSoundObject sonido2 = soundManager.CreateAmbientSound("../media/sound/Want_You_Bad.ogg", "Ambiente1", false, true); //Create Ambient sound
            sonido2.Play();
            soundManager.UpdateSoundObjects();
            SceneNode node = sceneMgr.RootSceneNode.CreateChildSceneNode("nodoBlender", new Vector3(50, 0, 0));

            ent = sceneMgr.CreateEntity("Suzanne1", "Suzanne.mesh");
            ent.CastShadows = true;
            nodo2 = node.CreateChildSceneNode("nodoSuzanne1", new Vector3(800, 60, 0));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-12));
            //testing zip loading with this sound
            sonido1 = soundManager.CreateSoundEntity("../media/sound/chime.zip","chime1.ogg", nodo2, nodo2.Name, true);
            sonido1.Play();
            l = sceneMgr.CreateLight("Luz1");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(
                nodo2.WorldPosition.x, nodo2.WorldPosition.y + 120,
                nodo2.WorldPosition.z + 20);
            l.CastShadows = true;
            l.DiffuseColour = new ColourValue(0.8f, 0.8f, 0.1f);
            l.SpecularColour = new ColourValue(0.9f, 0.9f, 0.2f);
            l.SetAttenuation(500f, 1f, 0.0005f, 0f);

            ent = sceneMgr.CreateEntity("Suzanne2", "Suzanne.mesh");
            ent.CastShadows = true;
            nodo2 = node.CreateChildSceneNode("nodoSuzanne2", new Vector3(0, 60, -1000));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-12));
            sonido1 = soundManager.CreateSoundEntity("../media/sound/bell1.ogg", nodo2, nodo2.Name, true, false);
            sonido1.Play();
            l = sceneMgr.CreateLight("Luz2");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(
                nodo2.WorldPosition.x,
                nodo2.WorldPosition.y + 120, nodo2.WorldPosition.z + 20);
            l.CastShadows = true;
            l.DiffuseColour = new ColourValue(0.7f, 0.3f, 0.3f);
            l.SpecularColour = new ColourValue(0.75f, 0.35f, 0.35f);
            l.SetAttenuation(500f, 1f, 0.0005f, 0f);

            ent = sceneMgr.CreateEntity("Suzanne3", "Suzanne.mesh");
            ent.CastShadows = true;
            nodo2 = node.CreateChildSceneNode("nodoSuzanne3", new Vector3(-2000, 60, -400));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-12));
            sonido1 = soundManager.CreateSoundEntity("../media/sound/boo1.ogg", nodo2, nodo2.Name, true, false);
            sonido1.Play();
            l = sceneMgr.CreateLight("Luz3");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(
                nodo2.WorldPosition.x,
                nodo2.WorldPosition.y + 120,
                nodo2.WorldPosition.z + 20);
            l.CastShadows = true;
            l.DiffuseColour = new ColourValue(0.35f, 0.67f, 0.41f);
            l.SpecularColour = new ColourValue(0.4f, 0.72f, 0.46f);
            l.SetAttenuation(500f, 1f, 0.0005f, 0f);

            ent = sceneMgr.CreateEntity("Suzanne4", "Suzanne.mesh");
            ent.CastShadows = true;
            nodo2 = node.CreateChildSceneNode("nodoSuzanne4", new Vector3(1600, 60, -800));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-12));
            sonido1 = soundManager.CreateSoundEntity("../media/sound/policesiren1.ogg", nodo2, nodo2.Name, true, false);
            sonido1.Play();
            l = sceneMgr.CreateLight("Luz4");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(nodo2.WorldPosition.x, nodo2.WorldPosition.y + 120, nodo2.WorldPosition.z + 20);
            l.CastShadows = true;
            l.DiffuseColour = new ColourValue(0.24f, 0.22f, 0.71f);
            l.SpecularColour = new ColourValue(0.29f, 0.27f, 0.76f);
            l.SetAttenuation(500f, 1f, 0.0005f, 0f);

            ent = sceneMgr.CreateEntity("Suzanne5", "Suzanne.mesh");
            ent.CastShadows = true;
            nodo2 = node.CreateChildSceneNode("nodoSuzanne5", new Vector3(-1800, 60, -2100));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-12));
            sonido1 = soundManager.CreateSoundEntity("../media/sound/phone1.ogg", nodo2, nodo2.Name, true, false);
            sonido1.Play();
            l = sceneMgr.CreateLight("Luz5");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(nodo2.WorldPosition.x, nodo2.WorldPosition.y + 120, nodo2.WorldPosition.z + 20);
            l.CastShadows = true;
            l.DiffuseColour = new ColourValue(0.5f, 0.5f, 0.5f);
            l.SpecularColour = new ColourValue(1, 1, 1);
            l.SetAttenuation(500f, 1f, 0.0005f, 0f);

            //Scenario
            ent = sceneMgr.CreateEntity("escenario", "ScenaMuestra.mesh");
            nodo2 = node.CreateChildSceneNode("nodoescenario", new Vector3(0, -2, 0));
            nodo2.AttachObject(ent);
            nodo2.Pitch(new Degree(-90));
            ent.CastShadows = false;
        }

        public override void CreateFrameListener()
        {	
            root.FrameStarted += new Mogre.FrameListener.FrameStartedHandler(this.soundManager.FrameStarted); //Add sound listener so it will update every frame
            base.CreateFrameListener();
        }
        public SoundDemo()
            : base()
        {
        }

        ~SoundDemo()
        {
            if (soundManager != null)
            {
                soundManager.Destroy();
                soundManager = null;
            }
        }
    }
}