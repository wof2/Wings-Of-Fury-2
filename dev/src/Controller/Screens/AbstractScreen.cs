/*
 * Copyright 2008 Adam Witczak, Jakub T�ycki, Kamil S�awi�ski, Tomasz Bilski, Emil Hornung, Micha� Ziober
 *
 * This file is part of Wings Of Fury 2.
 * 
 * Freeware Licence Agreement
 * 
 * This licence agreement only applies to the free version of this software.
 * Terms and Conditions
 * 
 * BY DOWNLOADING, INSTALLING, USING, TRANSMITTING, DISTRIBUTING OR COPYING THIS SOFTWARE ("THE SOFTWARE"), YOU AGREE TO THE TERMS OF THIS AGREEMENT (INCLUDING THE SOFTWARE LICENCE AND DISCLAIMER OF WARRANTY) WITH WINGSOFFURY2.COM THE OWNER OF ALL RIGHTS IN RESPECT OF THE SOFTWARE.
 * 
 * PLEASE READ THIS DOCUMENT CAREFULLY BEFORE USING THE SOFTWARE.
 *  
 * IF YOU DO NOT AGREE TO ANY OF THE TERMS OF THIS LICENCE THEN DO NOT DOWNLOAD, INSTALL, USE, TRANSMIT, DISTRIBUTE OR COPY THE SOFTWARE.
 * 
 * THIS DOCUMENT CONSTITUES A LICENCE TO USE THE SOFTWARE ON THE TERMS AND CONDITIONS APPEARING BELOW.
 * 
 * The Software is licensed to you without charge for use only upon the terms of this licence, and WINGSOFFURY2.COM reserves all rights not expressly granted to you. WINGSOFFURY2.COM retains ownership of all copies of the Software.
 * 1. Licence
 * 
 * You may use the Software without charge.
 *  
 * You may distribute exact copies of the Software to anyone.
 * 2. Restrictions
 * 
 * WINGSOFFURY2.COM reserves the right to revoke the above distribution right at any time, for any or no reason.
 *  
 * YOU MAY NOT MODIFY, ADAPT, TRANSLATE, RENT, LEASE, LOAN, SELL, REQUEST DONATIONS OR CREATE DERIVATE WORKS BASED UPON THE SOFTWARE OR ANY PART THEREOF.
 * 
 * The Software contains trade secrets and to protect them you may not decompile, reverse engineer, disassemble or otherwise reduce the Software to a humanly perceivable form. You agree not to divulge, directly or indirectly, until such trade secrets cease to be confidential, for any reason not your own fault.
 * 3. Termination
 * 
 * This licence is effective until terminated. The Licence will terminate automatically without notice from WINGSOFFURY2.COM if you fail to comply with any provision of this Licence. Upon termination you must destroy the Software and all copies thereof. You may terminate this Licence at any time by destroying the Software and all copies thereof. Upon termination of this licence for any reason you shall continue to be bound by the provisions of Section 2 above. Termination will be without prejudice to any rights WINGSOFFURY2.COM may have as a result of this agreement.
 * 4. Disclaimer of Warranty, Limitation of Remedies
 * 
 * TO THE FULL EXTENT PERMITTED BY LAW, WINGSOFFURY2.COM HEREBY EXCLUDES ALL CONDITIONS AND WARRANTIES, WHETHER IMPOSED BY STATUTE OR BY OPERATION OF LAW OR OTHERWISE, NOT EXPRESSLY SET OUT HEREIN. THE SOFTWARE, AND ALL ACCOMPANYING FILES, DATA AND MATERIALS ARE DISTRIBUTED "AS IS" AND WITH NO WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED. WINGSOFFURY2.COM DOES NOT WARRANT, GUARANTEE OR MAKE ANY REPRESENTATIONS REGARDING THE USE, OR THE RESULTS OF THE USE, OF THE SOFTWARE WITH RESPECT TO ITS CORRECTNESS, ACCURACY, RELIABILITY, CURRENTNESS OR OTHERWISE. THE ENTIRE RISK OF USING THE SOFTWARE IS ASSUMED BY YOU. WINGSOFFURY2.COM MAKES NO EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS INCLUDING, WITHOUT LIMITATION, THE WARRANTIES OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE WITH RESPECT TO THE SOFTWARE. NO ORAL OR WRITTEN INFORMATION OR ADVICE GIVEN BY WINGSOFFURY2.COM, IT'S DISTRIBUTORS, AGENTS OR EMPLOYEES SHALL CREATE A WARRANTY, AND YOU MAY NOT RELY ON ANY SUCH INFORMATION OR ADVICE.
 * 
 * IMPORTANT NOTE: Nothing in this Agreement is intended or shall be construed as excluding or modifying any statutory rights, warranties or conditions which by virtue of any national or state Fair Trading, Trade Practices or other such consumer legislation may not be modified or excluded. If permitted by such legislation, however, WINGSOFFURY2.COM' liability for any breach of any such warranty or condition shall be and is hereby limited to the supply of the Software licensed hereunder again as WINGSOFFURY2.COM at its sole discretion may determine to be necessary to correct the said breach.
 * 
 * IN NO EVENT SHALL WINGSOFFURY2.COM BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, AND THE LOSS OF BUSINESS INFORMATION OR COMPUTER PROGRAMS), EVEN IF WINGSOFFURY2.COM OR ANY WINGSOFFURY2.COM REPRESENTATIVE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. IN ADDITION, IN NO EVENT DOES WINGSOFFURY2.COM AUTHORISE YOU TO USE THE SOFTWARE IN SITUATIONS WHERE FAILURE OF THE SOFTWARE TO PERFORM CAN REASONABLY BE EXPECTED TO RESULT IN A PHYSICAL INJURY, OR IN LOSS OF LIFE. ANY SUCH USE BY YOU IS ENTIRELY AT YOUR OWN RISK, AND YOU AGREE TO HOLD WINGSOFFURY2.COM HARMLESS FROM ANY CLAIMS OR LOSSES RELATING TO SUCH UNAUTHORISED USE.
 * 5. General
 * 
 * All rights of any kind in the Software which are not expressly granted in this Agreement are entirely and exclusively reserved to and by WINGSOFFURY2.COM.
 * 
 * 
 */


using System;
using System.Collections;
using System.Collections.Generic;
using BetaGUI;
using FSLOgreCS;
using Mogre;
using MOIS;
using Wof.Languages;
using Wof.Model.Configuration;
using Wof.View;
using Wof.View.Effects;
using FontManager=Wof.Languages.FontManager;
using Math=Mogre.Math;
using Vector3=Mogre.Vector3;

namespace Wof.Controller.Screens
{

    /// <summary>
    /// Przechowuje informacje o aktualnie pokazywanych obiektach na scenie oraz o poprzednim po�o�eniu myszki
    /// </summary>
    class ScreenState
    {
        public List<PlaneView> PlaneViews
        {
            get { return planeViews; }
            set { planeViews = value; }
        }
        protected List<PlaneView> planeViews;
        

        public uint MousePosX
        {
            get { return mousePosX; }
            set { mousePosX = value; }
        }
        protected uint mousePosX;

        public uint MousePosY
        {
            get { return mousePosY; }
            set { mousePosY = value; }
        }
        protected uint mousePosY;

        public ScreenState(List<PlaneView> planeViews, uint mousePosX, uint mousePosY)
        {
            this.planeViews = planeViews;
            this.mousePosX = mousePosX;
            this.mousePosY = mousePosY;
        }

    }
    /// <summary>
    /// Klasa odpowiadaj�ca za wy�wietlanie screen�w w menu
    /// <author>Adam Witczak, Jakub T�ycki</author>
    /// </summary>
    class AbstractScreen : MenuScreen
    {
        protected uint fontSize;
       
        public void SetFontSize(uint fontSize)
        {
           this.fontSize = fontSize;
        }
        public uint GetFontSize()
        {
            return fontSize;   
        }

        public uint GetTextVSpacing()
        {
            return (uint)(fontSize * 1.2f);
        }


        protected enum KeyDirection
        {
            UP,
            DOWN,
            NONE
        } ;

        public const float C_RESPONSE_DELAY = 0.14f;
                           // czas w sekunach w jakim klawisze reaguj� na przyciskanie, a mysz 'klika'

        protected GUI mGui;
        protected GameEventListener gameEventListener;
        protected SceneManager sceneMgr;
        protected Viewport viewport;
        protected Camera camera;
        protected float screenTime;

        protected delegate void VoidDelegateVoid();

        /// <summary>
        /// Wszystkie cheaty dost�pne w menu. 
        /// Kluczem jest sekwencja znak�w wywo�uj�ca cheat. Warto�ci� jest para: delegat, kt�ry ma zosta� wykonany oraz liczba informuj�ca na kt�rej literze aktualnie znajduje si� user. Na pocz�tku liczba ta jest 0. 
        /// </summary>
        protected IDictionary<KeyCode[], KeyValuePair<VoidDelegateVoid, uint>> cheats;

        // KC_GRAVE oznacza tyld�
        protected KeyCode[] cheatGodMode = { KeyCode.KC_GRAVE, KeyCode.KC_I, KeyCode.KC_D, KeyCode.KC_D, KeyCode.KC_Q, KeyCode.KC_D};
        protected KeyCode[] cheatPlane = { KeyCode.KC_GRAVE, KeyCode.KC_P, KeyCode.KC_L };
        protected KeyCode[] cheatAllLevels = { KeyCode.KC_GRAVE, KeyCode.KC_I, KeyCode.KC_D, KeyCode.KC_K, KeyCode.KC_F, KeyCode.KC_A };



        protected List<PlaneView> planeViews;

        public List<PlaneView> PlaneViews
        {
            get { return planeViews; }
            set { planeViews = value; }
        }


        protected Button[] buttons = null; // przyciski na ekranie

        protected uint mousePosX, mousePosY;
        protected Boolean wasLeftMousePressed;

        protected Boolean wasUpKeyPressed;
        protected Boolean wasDownKeyPressed;
        protected Boolean wasEnterKeyPressed;
        protected Timer keyDelay; // op�nienie naciskania klawiszy

        protected int currentButton = 0; // aktualnie zaznaczony przycisk (klawiatur�)
        protected int buttonsCount = 0; // ilo�� przycisk�w w menu
        protected int backButtonIndex = -1; // indeks przycisku 'powr�t'.

        public uint MousePosX
        {
            get { return mousePosX; }
            set { mousePosX = value; }
        }

        public uint MousePosY
        {
            get { return mousePosY; }
            set { mousePosY = value; }
        }

     

        protected Boolean initialized;
        protected FSLSoundObject clickSound;
        protected FSLSoundObject cheatSound;
    
        /// <summary>
        /// Czy w poprzedniej klatce wszystkie przyciski by�y puszczone
        /// </summary>
        private bool wereAllKeysReleased = false; 

        public AbstractScreen(GameEventListener gameEventListener,
                              SceneManager sceneMgr, Viewport viewport, Camera camera) : this(gameEventListener, sceneMgr, viewport, camera, 0)
        {
           

        }

        public AbstractScreen(GameEventListener gameEventListener,
                              SceneManager sceneMgr, Viewport viewport, Camera camera, uint fontSize)
        {
            clickSound = SoundManager3D.Instance.GetSound("menuClick");
            if (clickSound == null || !clickSound.HasSound()) clickSound = SoundManager3D.Instance.CreateAmbientSound(SoundManager3D.C_MENU_CLICK, "menuClick", false, false); // destroyed together with SoundManager3D singleton

            cheatSound = SoundManager3D.Instance.GetSound("cheatSound");
            if (cheatSound == null || !cheatSound.HasSound()) cheatSound = SoundManager3D.Instance.CreateAmbientSound(SoundManager3D.C_MENU_CHEAT, "cheatSound", false, false); // destroyed together with SoundManager3D singleton



            this.gameEventListener = gameEventListener;
            this.sceneMgr = sceneMgr;
            this.viewport = viewport;
            this.camera = camera;
            initialized = false;
            screenTime = 0;
            mousePosX = (uint) viewport.ActualWidth/2;
            mousePosY = (uint) viewport.ActualHeight/2;
            wasUpKeyPressed = false;
            wasDownKeyPressed = false;
            wasEnterKeyPressed = false;

            cheats = new Dictionary<KeyCode[], KeyValuePair<VoidDelegateVoid, uint>>();
            cheats[cheatGodMode] = new KeyValuePair<VoidDelegateVoid, uint>(doCheatGodMode, 0);
            cheats[cheatPlane] = new KeyValuePair<VoidDelegateVoid, uint>(doCheatPlane, 0); ;
            cheats[cheatAllLevels] = new KeyValuePair<VoidDelegateVoid, uint>(doCheatAllLevels, 0); ;

            TextureManager.Singleton.UnloadUnreferencedResources();
            MaterialManager.Singleton.UnloadUnreferencedResources();
            MeshManager.Singleton.UnloadUnreferencedResources();

            if(fontSize == 0)
            {
                this.fontSize = (uint)(viewport.ActualHeight * EngineConfig.C_FONT_SIZE);
            } else
            {
                this.fontSize = fontSize;
            }
            
            keyDelay = new Timer();
        }


      
        

        /// <summary>
        /// Pobiera ze screenu aktualny stan: samoloty oraz pozycje myszki
        /// </summary>
        /// <returns></returns>
        public ScreenState GetScreenState()
        {
            return new ScreenState(planeViews, mousePosX, mousePosY);
        }
        /// <summary>
        /// Wymusza nowy stan screena: samoloty oraz po�o�enie myszki
        /// </summary>
        /// <param name="ss"></param>
        public void SetScreenState(ScreenState ss)
        {
            planeViews = ss.PlaneViews;
            mousePosX = ss.MousePosX;
            mousePosY = ss.MousePosY;
        }

        protected virtual void CreateSkybox()
        {
            // Set the material
            sceneMgr.SetSkyBox(true, "Skybox/Morning", 5000);
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
            // create a default point light

            Light light = sceneMgr.CreateLight("MainLight");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.Position = new Vector3(0, 1000, 0);
            light.Direction = new Vector3(0, -5, 0);
            light.DiffuseColour = new ColourValue(1.0f, 1.0f, 1.0f);
            light.SpecularColour = new ColourValue(0.05f, 0.05f, 0.05f);

            sceneMgr.ShadowFarDistance = 1000;
            sceneMgr.ShadowColour = new ColourValue(0.8f, 0.8f, 0.8f);

            EffectsManager.Singleton.AddClouds(sceneMgr, new Vector3(0, 70, -800), new Vector2(1000, 100), new Degree(2),
                                               10);
        }

        public virtual void CreateOcean()
        {
            // OCEAN          

            if (!MeshManager.Singleton.ResourceExists("OceanPlane"))
            {
                Plane plane; // = new Plane();
                plane.normal = Vector3.UNIT_Y;
                plane.d = 0;
                MeshManager.Singleton.CreatePlane("OceanPlane",
                                                  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                                                  5000, 5000, 10, 10, true, 1, 10, 10, Vector3.UNIT_Z);
            }

            Entity ocean = sceneMgr.CreateEntity("Ocean", "OceanPlane");
            MaterialPtr m = MaterialManager.Singleton.GetByName("Ocean2_HLSL_GLSL");
            m.Load();
            Pass p = m.GetBestTechnique().GetPass(0);
            TextureUnitState tu = p.GetTextureUnitState("Reflection");
            if (tu != null)
            {
                tu.SetCubicTextureName("morning.jpg", true);
            }
            if (p.HasFragmentProgram)
            {
                GpuProgramParametersSharedPtr param = p.GetVertexProgramParameters();
                param.SetNamedConstant("bumpSpeed", new Vector3(0.015f, - 1f, 0));
                p.SetVertexProgramParameters(param);
            }
            ocean.SetMaterialName("Ocean2_HLSL_GLSL");
            ocean.CastShadows = false;

            sceneMgr.RootSceneNode.AttachObject(ocean);
            // OCEAN
        }

        public virtual void CreateScene()
        {
            if (planeViews == null)
            {
                // add planes
                planeViews = new List<PlaneView>();
                for (int i = 0; i < 3; i++)
                {
                    P47PlaneView p = new P47PlaneView(null, sceneMgr, sceneMgr.RootSceneNode, "Plane");

                    if (i == 0)
                    {
                        p.PlaneNode.Translate(-10, 17, 40);
                    }
                    else if (i == 1)
                    {
                        p.PlaneNode.Translate(0, 20, 40);
                    }
                    else if (i == 2)
                    {
                        p.PlaneNode.Translate(10, 17, 40);
                    }
                    p.AnimationMgr.SetGearsVisible(false);
                    p.PlaneNode.Yaw(Math.PI);
                    p.AnimationMgr.switchToIdle(false);
                    p.AnimationMgr.CurrentAnimation.TimeScale = Math.RangeRandom(0.2f, 0.3f);
                    p.AnimationMgr.enableBlade();
                    p.AnimationMgr.changeBladeSpeed(1000);
                    p.SwitchToFastEngine();
                    planeViews.Add(p);
                }
            }
        }


        protected virtual void PlaceCamera()
        {
            camera.NearClipDistance = 1;
            camera.Position = new Vector3(0, 20, 70);
        }

        protected void createMouse()
        {
            if (mGui == null) mGui = new GUI(FontManager.CurrentFont, fontSize);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
            mGui.injectMouse((uint)(viewport.ActualWidth + 1), (uint)(viewport.ActualHeight + 1), false);  
        }

        protected virtual void CreateGUI()
        {
          
            buttonsCount = 0;
            mGui = new GUI(FontManager.CurrentFont, fontSize);
            mGui.createMousePointer(new Vector2(30, 30), "bgui.pointer");
        }


        public void DisplayGUI(Boolean justMenu)
        {
            CreateGUI();

            if (!justMenu)
            {
                PlaceCamera();
                CreateSkybox();
                CreateOcean();
                CreateScene();
            }
          
            initialized = true;
        }
      
        public void CleanUp(Boolean justMenu)
        {
            if (mGui != null)
            {
                mGui.killGUI();
            }

            if (!justMenu)
            {
                FrameWork.DestroyScenes();

                //MaterialManager.Singleton.UnloadUnreferencedResources();
                EffectsManager.Singleton.Clear();


                viewport.Dispose();
                viewport = null;
            }
           // clickSound.Destroy();
            initialized = false;
        }

        public virtual void KeyReceived(String key)
        {
        }

        public virtual void MouseReceived(String button)
        {
        }

        public virtual void FrameStarted(FrameEvent evt)
        {
            EffectsManager.Singleton.UpdateTimeAndAnimateAll(evt.timeSinceLastFrame);
        }
        
        public void HandleInput(FrameEvent evt, Mouse inputMouse, Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            screenTime += evt.timeSinceLastFrame;
            if (initialized)
            {
                FrameStarted(evt);

                if (planeViews != null)
                {
                    for (int i = 0; i < planeViews.Count; i++)
                    {
                        planeViews[i].AnimationMgr.updateTimeAll(evt.timeSinceLastFrame);
                        planeViews[i].AnimationMgr.animateAll();
                    }
                }

                inputMouse.Capture();
                inputKeyboard.Capture();
                if (inputJoystick != null) inputJoystick.Capture();
                             
                receiveKeys(inputKeyboard, inputJoystick);

              
                

                if (inputMouse.MouseState.ButtonDown(MouseButtonID.MB_Left))
                {
                    MouseReceived("LEFT");
                }
                if (inputMouse.MouseState.ButtonDown(MouseButtonID.MB_Middle))
                {
                    MouseReceived("MIDDLE");
                }
                if (inputMouse.MouseState.ButtonDown(MouseButtonID.MB_Right))
                {
                    MouseReceived("RIGHT");
                }

                if (mGui != null)
                {
                    MouseState_NativePtr mouseState = inputMouse.MouseState;

                    uint screenx;
                    uint screeny;

                    getMouseScreenCoordinates(mouseState, out screenx, out screeny);
                    doCheating(inputKeyboard, inputJoystick);


                    if (inputKeyboard.IsKeyDown(KeyCode.KC_BACK))
                    {
                        //mGui.injectBackspace(screenx, screeny);
                        mGui.injectKey("!b", screenx, screeny);
                        KeyReceived("!b");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_1))
                    {
                        mGui.injectKey("1", screenx, screeny);
                        KeyReceived("1");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_2))
                    {
                        mGui.injectKey("2", screenx, screeny);
                        KeyReceived("2");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_3))
                    {
                        mGui.injectKey("3", screenx, screeny);
                        KeyReceived("3");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_4))
                    {
                        mGui.injectKey("4", screenx, screeny);
                        KeyReceived("4");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_5))
                    {
                        mGui.injectKey("5", screenx, screeny);
                        KeyReceived("5");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_6))
                    {
                        mGui.injectKey("6", screenx, screeny);
                        KeyReceived("6");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_7))
                    {
                        mGui.injectKey("7", screenx, screeny);
                        KeyReceived("7");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_8))
                    {
                        mGui.injectKey("8", screenx, screeny);
                        KeyReceived("8");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_9))
                    {
                        mGui.injectKey("9", screenx, screeny);
                        KeyReceived("9");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_A))
                    {
                        mGui.injectKey("a", screenx, screeny);
                        KeyReceived("a");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_B))
                    {
                        mGui.injectKey("b", screenx, screeny);
                        KeyReceived("b");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_C))
                    {
                        mGui.injectKey("c", screenx, screeny);
                        KeyReceived("c");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_D))
                    {
                        mGui.injectKey("d", screenx, screeny);
                        KeyReceived("d");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_E))
                    {
                        mGui.injectKey("e", screenx, screeny);
                        KeyReceived("e");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_F))
                    {
                        mGui.injectKey("f", screenx, screeny);
                        KeyReceived("f");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_G))
                    {
                        mGui.injectKey("g", screenx, screeny);
                        KeyReceived("g");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_H))
                    {
                        mGui.injectKey("h", screenx, screeny);
                        KeyReceived("h");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_I))
                    {
                        mGui.injectKey("i", screenx, screeny);
                        KeyReceived("i");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_J))
                    {
                        mGui.injectKey("j", screenx, screeny);
                        KeyReceived("j");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_K))
                    {
                        mGui.injectKey("k", screenx, screeny);
                        KeyReceived("k");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_L))
                    {
                        mGui.injectKey("l", screenx, screeny);
                        KeyReceived("l");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_M))
                    {
                        mGui.injectKey("m", screenx, screeny);
                        KeyReceived("m");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_N))
                    {
                        mGui.injectKey("n", screenx, screeny);
                        KeyReceived("n");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_O))
                    {
                        mGui.injectKey("o", screenx, screeny);
                        KeyReceived("o");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_P))
                    {
                        mGui.injectKey("p", screenx, screeny);
                        KeyReceived("p");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_Q))
                    {
                        mGui.injectKey("q", screenx, screeny);
                        KeyReceived("q");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_R))
                    {
                        mGui.injectKey("r", screenx, screeny);
                        KeyReceived("r");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_S))
                    {
                        mGui.injectKey("s", screenx, screeny);
                        KeyReceived("s");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_T))
                    {
                        mGui.injectKey("t", screenx, screeny);
                        KeyReceived("t");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_U))
                    {
                        mGui.injectKey("u", screenx, screeny);
                        KeyReceived("u");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_V))
                    {
                        mGui.injectKey("v", screenx, screeny);
                        KeyReceived("v");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_W))
                    {
                        mGui.injectKey("w", screenx, screeny);
                        KeyReceived("w");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_X))
                    {
                        mGui.injectKey("x", screenx, screeny);
                        KeyReceived("x");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_Y))
                    {
                        mGui.injectKey("y", screenx, screeny);
                        KeyReceived("y");
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_Z))
                    {
                        mGui.injectKey("z", screenx, screeny);
                        KeyReceived("z");
                    }
                    if (wereAllKeysReleased && (inputKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Escape))) 
                    {
                        KeyReceived("ESC");
                        if (tryToPressBackButton()) return;
                    }
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_SPACE))
                    {
                        KeyReceived("SPACE");
                    }


                    if (wereAllKeysReleased && (inputKeyboard.IsKeyDown(KeyCode.KC_RETURN) || FrameWork.GetJoystickButton(inputJoystick, EngineConfig.JoystickButtons.Enter))) 
                    {
                        KeyReceived("ENTER");
                        wasEnterKeyPressed = true;
                        if (buttons != null && Button.TryToPressButton(buttons[currentButton])) return;
                    }
                    else
                    {
                        wasEnterKeyPressed = false;
                    }

                    Vector2 joyVector = FrameWork.GetJoystickVector(inputJoystick);

                    if (inputKeyboard.IsKeyDown(KeyCode.KC_UP))
                    {
                        mGui.injectKey("up", screenx, screeny);
                        KeyReceived("UP");
                        wasUpKeyPressed = true;
                    }
                    else
                    {
                        if (joyVector.y > 0)
                        {
                            mGui.injectKey("up", screenx, screeny);
                            KeyReceived("UP");
                            wasUpKeyPressed = true;
                        }else
                        {
                            wasUpKeyPressed = false;   
                        }
                       
                    }

                    if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN))
                    {
                        mGui.injectKey("down", screenx, screeny);
                        KeyReceived("DOWN");
                        wasUpKeyPressed = true;
                    }
                    else
                    {

                        if (joyVector.y < 0)
                        {
                            mGui.injectKey("down", screenx, screeny);
                            KeyReceived("DOWN");
                            wasDownKeyPressed = true;
                        }
                        else
                        {
                            wasDownKeyPressed = false;
                        }
                        
                    }

                   
                    int id = -1;
                   
                    if (mouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
                    {
                       wasLeftMousePressed = true;
                    }
                    else if (wasLeftMousePressed)
                    {
                       // w poprzedniej klatce uzytkownik
                       // trzymal wcisniety przycisk myszki
                       // a teraz go zwolnil
                       id = mGui.injectMouse(screenx, screeny, true);
                       wasLeftMousePressed = false;
                    }
                    else
                    {
                        id = mGui.injectMouse(screenx, screeny, false);

                        // zaznacz te na ktore pokazuje klawiatura
                        if (wasDownKeyPressed)
                        {
                            tryToChangeSelectedButton(currentButton + 1);
                        }
                        else if (wasUpKeyPressed)
                        {
                            tryToChangeSelectedButton(currentButton - 1);
                        } else if(id!= -1)
                        {
                            selectButton(id, true); currentButton = id;
                        }
                    }
                }
            }

            wereAllKeysReleased = areAllKeysReleased(inputKeyboard, inputJoystick);
        }

        private void receiveKeys(Keyboard inputKeyboard, JoyStick joystick)
        {
            Vector2 joyVector = FrameWork.GetJoystickVector(joystick);
            if(joyVector != Vector2.ZERO)
            {
                //Console.WriteLine(joyVector);
                if (joyVector.y > 0)
                {
                    wasUpKeyPressed = true;
                    KeyReceived("UP");    
                }
                if (joyVector.y < 0)
                {
                    wasDownKeyPressed = true;
                    KeyReceived("DOWN");
                }
                   
            }
           

            if (inputKeyboard.IsKeyDown(KeyCode.KC_RETURN) || FrameWork.GetJoystickButton(joystick, EngineConfig.JoystickButtons.Enter)) 
            {
                wasEnterKeyPressed = true;
                KeyReceived("ENTER");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_UP))
            {
                wasUpKeyPressed = true;
                KeyReceived("UP");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_DOWN))
            {
                wasDownKeyPressed = true;
                KeyReceived("DOWN");
            }


            if (inputKeyboard.IsKeyDown(KeyCode.KC_BACK))
            {
                KeyReceived("BACK");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_1))
            {
                KeyReceived("1");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_2))
            {
                KeyReceived("2");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_3))
            {
                KeyReceived("3");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_4))
            {
                KeyReceived("4");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_5))
            {
                KeyReceived("5");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_6))
            {
                KeyReceived("6");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_7))
            {
                KeyReceived("7");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_8))
            {
                KeyReceived("8");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_9))
            {
                KeyReceived("9");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_A))
            {
                KeyReceived("a");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_B))
            {
                KeyReceived("b");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_C))
            {
                KeyReceived("c");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_D))
            {
                KeyReceived("d");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_E))
            {
                KeyReceived("e");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_F))
            {
                KeyReceived("f");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_G))
            {
                KeyReceived("g");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_H))
            {
                KeyReceived("h");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_I))
            {
                KeyReceived("i");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_J))
            {
                KeyReceived("j");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_K))
            {
                KeyReceived("k");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_L))
            {
                KeyReceived("l");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_M))
            {
                KeyReceived("m");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_N))
            {
                KeyReceived("n");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_O))
            {
                KeyReceived("o");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_P))
            {
                KeyReceived("p");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_Q))
            {
                KeyReceived("q");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_R))
            {
                KeyReceived("r");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_S))
            {
                KeyReceived("s");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_T))
            {
                KeyReceived("t");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_U))
            {
                KeyReceived("u");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_W))
            {
                KeyReceived("w");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_X))
            {
                KeyReceived("x");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_Y))
            {
                KeyReceived("y");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_Z))
            {
                KeyReceived("z");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_ESCAPE) || FrameWork.GetJoystickButton(joystick, EngineConfig.JoystickButtons.Escape)) 
            {
                KeyReceived("ESC");
            }
            if (inputKeyboard.IsKeyDown(KeyCode.KC_SPACE))
            {
                KeyReceived("SPACE");
            }
        }

        /// <summary>
        /// Sprawdza czy w bie��cej klatce �aden klawisz / button na joysticku (bez osi) nie jest wci�ni�ty.
        /// </summary>
        /// <param name="inputKeyboard">Mo�na przekaza� null</param>
        /// <param name="inputJoystick">Mo�na przekaza� null</param>
        /// <returns>True je�li nic nie by�o wci�ni�te</returns>
        protected virtual bool areAllKeysReleased(Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            if(inputKeyboard != null)
            {
                IEnumerator k = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
                while (k.MoveNext())
                {
                    if (inputKeyboard.IsKeyDown((KeyCode)k.Current)) return false;
                }

            }
          
            if (inputJoystick != null)
            {
                for (int i = 0; i < inputJoystick.JoyStickState.ButtonCount; i++)
                {
                    if (inputJoystick.JoyStickState.GetButton(i)) return false;
                }
            }
           
            return true;
        }

        protected virtual bool doCheating(Keyboard inputKeyboard, JoyStick inputJoystick)
        {
            if (wereAllKeysReleased)
            {
                if (areAllKeysReleased(inputKeyboard, inputJoystick)) return true;

                KeyValuePair<VoidDelegateVoid, uint> info;
                ICollection<KeyCode[]> keys = cheats.Keys;
                KeyCode[][] tempCodes = new KeyCode[cheats.Keys.Count][];
                IEnumerator<KeyCode[]> e = keys.GetEnumerator();
                int i = 0;
              
                while (e.MoveNext())
                {
                    tempCodes[i] = new KeyCode[e.Current.Length];
                    tempCodes[i] = e.Current;
                    i++;
                }
               // Console.WriteLine("czy cheat?");
                foreach (KeyCode[] code in tempCodes)
                {
                    info = cheats[code];

                    if(inputKeyboard.IsKeyDown(code[info.Value]))
                    {
                        //Console.WriteLine("klawisz sie zgadza: " + code[info.Value].ToString());
                        if (info.Value + 1 >= code.Length)
                        {
                            //Console.WriteLine("invoke");
                            // udany cheat
                            info.Key.Invoke();
                            PlayCheatSound();
                            cheats[code] = new KeyValuePair<VoidDelegateVoid, uint>(info.Key, 0);
                            return true;
                        }

                        // krok naprz�d
                        cheats[code] = new KeyValuePair<VoidDelegateVoid, uint>(info.Key, info.Value + 1);
                    } else
                    {
                        //Console.WriteLine("reset");
                        // zerujemy
                        cheats[code] = new KeyValuePair<VoidDelegateVoid, uint>(info.Key, 0);
                    }

                }

            }
            return false;

        }



        public void doCheatGodMode()
        {
            GameConsts.UserPlane.GodMode = !GameConsts.UserPlane.GodMode;
        }

        public void doCheatPlane()
        {
            GameConsts.UserPlane.PlaneCheat = !GameConsts.UserPlane.PlaneCheat;
        }

        public void doCheatAllLevels()
        {
            GameConsts.Game.AllLevelsCheat = !GameConsts.Game.AllLevelsCheat;
        }


        
     
        private void getMouseScreenCoordinates(MouseState_NativePtr mouseState, out uint screenx, out uint screeny)
        {
            mousePosX = (uint) (mousePosX + mouseState.X.rel*2);
            mousePosY = (uint) (mousePosY + mouseState.Y.rel*2);

            if ((int) mousePosX + (int) mouseState.X.rel*2 < 0)
            {
                mousePosX = 0;
            }
            if ((int) mousePosY + (int) mouseState.Y.rel*2 < 0)
            {
                mousePosY = 0;
            }

            if (mousePosX > viewport.ActualWidth)
            {
                mousePosX = (uint) viewport.ActualWidth;
            }
            if (mousePosY > viewport.ActualHeight)
            {
                mousePosY = (uint) viewport.ActualHeight;
            }

            screenx = mousePosX;
            screeny = mousePosY;
        }


        protected Boolean tryToPressBackButton()
        {
            if (backButtonIndex != -1)
            {
                return Button.TryToPressButton(buttons[backButtonIndex]);
            }
            return false;
        }

        protected void tryToChangeSelectedButton(int newCurrentOption)
        {
            if (newCurrentOption > buttonsCount - 1)
            {
                newCurrentOption = 0;
            }
            if (newCurrentOption < 0)
            {
                newCurrentOption = buttonsCount - 1;
            }
            if (keyDelay.Milliseconds > (int) (C_RESPONSE_DELAY*1000))
            {
                currentButton = newCurrentOption;
                deselectButtons();
                selectButton(currentButton);
                keyDelay.Reset();
            }
        }


        protected void deselectButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].activate(false);
            }
        }

        protected void deselectButtons(int exceptIndex)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != exceptIndex)
                {
                    buttons[i].activate(false);
                }
            }
        }
        protected void selectButton(int i, bool playSound)
        {
           if (i == -1) return;
           buttons[i].activate(true);
           

        }
        protected void selectButton(int i)
        {
            selectButton(i, false);
        }

        protected void initButtons(int count)
        {
            buttonsCount = count;
            buttons = new Button[buttonsCount];
        }

        protected void initButtons(int count, int backButtonIndex)
        {
            buttonsCount = count;
            buttons = new Button[buttonsCount];
            this.backButtonIndex = backButtonIndex;
        }


        public void PlayClickSound()
        {
            if (EngineConfig.SoundEnabled && !clickSound.IsPlaying()) clickSound.Play();
        }


        public void PlayCheatSound()
        {
            if (EngineConfig.SoundEnabled && !cheatSound.IsPlaying()) cheatSound.Play();
        }

        


        public static void SetOverlayColor(OverlayContainer c, ColourValue top, ColourValue bottom)
        {
            foreach (OverlayElement element in c.GetChildIterator())
            {
                element.SetParameter("colour_top", String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(top.r), StringConverter.ToString(top.g), StringConverter.ToString(top.b)));
                element.SetParameter("colour_bottom", String.Format("{0:f} {1:f} {2:f}", StringConverter.ToString(bottom.r), StringConverter.ToString(bottom.g), StringConverter.ToString(bottom.b)));
            }

        }
    }
}