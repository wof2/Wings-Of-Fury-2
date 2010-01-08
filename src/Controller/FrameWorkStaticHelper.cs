using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FSLOgreCS;
using Mogre;
using MOIS;
using Wof.Controller.Input.KeyboardAndJoystick;
using Math=System.Math;

namespace Wof.Controller
{
    public class FrameWorkStaticHelper
    {
    	
    	public const String C_VIDEO_MODE = "Video Mode";
        public const String C_VSYNC = "VSync";
        public const String C_ANTIALIASING = "Anti aliasing";
        public const String C_USE_NV_PERFHUD = "Allow NVPerfHUD";
        
        public static void ShowOgreException()
        {
            if (OgreException.IsThrown)
                MessageBox.Show(OgreException.LastException.FullDescription, EngineConfig.C_GAME_NAME + " - Engine error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWofException(Exception ex)
        {
            MessageBox.Show(ex.Message + "\r\n" + "Stack trace: "+ex.StackTrace, EngineConfig.C_GAME_NAME + " - Runtime error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static List<String> GetAntialiasingModes(Root root)
        {
            List<String> availableModes = new List<String>();

            ConfigOption_NativePtr videoModeOption;

            // staram sie znalezc opcje konfiguracyjna Video Mode
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();
            foreach (KeyValuePair<string, ConfigOption_NativePtr> m in map)
            {
                if (m.Value.name.Equals(FrameWorkStaticHelper.C_ANTIALIASING))
                {
                    videoModeOption = m.Value;
                    break;
                }
            }

            // nie ma takiej mozliwosci, zebym nie znalazl
            // konwertuje wektor na liste
            foreach (String s in videoModeOption.possibleValues)
            {
                availableModes.Add(s);
            }

            return availableModes;
        }

        public static String GetCurrentColourDepth(Root root)
        {
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();
            root.RestoreConfig();
            foreach (KeyValuePair<string, Mogre.ConfigOption_NativePtr> s in map)
            {
                if (s.Key.Equals(C_VIDEO_MODE))
                {
                    string str = s.Value.currentValue;
                    Match match = Regex.Match(str, "@ (.*)-bit");
                    return Int32.Parse(match.Groups[1].Value).ToString();
                }


            }
            return "";

        }

        public static int GetCurrentUseNVPerfHUD(Root root)
        {
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();

            foreach (KeyValuePair<string, Mogre.ConfigOption_NativePtr> s in map)
            {
                if (s.Key.Equals(C_USE_NV_PERFHUD))
                {
                    string str = s.Value.currentValue;
                    int ret = str.Equals("Yes") ? 1 : 0;
                    return ret;
                }


            }
            return 0;
            
        }

        public static int[] GetCurrentFSAA(Root root)
        {
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();

            foreach (KeyValuePair<string, Mogre.ConfigOption_NativePtr> s in map)
            {
                if (s.Key.Equals(C_ANTIALIASING))
                {
                    string str = s.Value.currentValue;
                    Match match = Regex.Match(str, "(.*) (.*)");
                    string type;
                    string quality;
                  
                    if (match.Groups[1].Value.Equals("NonMaskable"))
                    {
                        type = "1"; //D3DMULTISAMPLE_NONMASKABLE
                        quality = (int.Parse(match.Groups[2].Value) - 1).ToString();
                    }
                    else
                    {
                        type = match.Groups[2].Value; // D3DMULTISAMPLE_X_SAMPLES 
                        quality = "0";
                    }

                    return new int[] { int.Parse(type), int.Parse(quality) };
                }


            }
            return new int[] { 0, 0 };
            

        }


        public static int GetCurrentVsync(Root root)
        {
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();

            foreach (KeyValuePair<string, Mogre.ConfigOption_NativePtr> s in map)
            {
                if (s.Key.Equals(C_VSYNC))
                {
                    return s.Value.currentValue.Equals("Yes") ? 1 : 0;
                   
                }
            }
            return 0;

        }
        
   		public static Vector2 GetCurrentVideoMode(Root root)
        {
   			ConfigOptionMap map = root.RenderSystem.GetConfigOptions();
   	
   			foreach(KeyValuePair<string, Mogre.ConfigOption_NativePtr> s in map)
   			{
   				if(s.Key.Equals(C_VIDEO_MODE))
   				{
   					string str =  s.Value.currentValue;
   					Match match = Regex.Match(str, "([0-9]+) x ([0-9]+).*");
                    int xRes = Int32.Parse(match.Groups[1].Value);
                    int yRes = Int32.Parse(match.Groups[2].Value);
                    return new Vector2(xRes, yRes);
   				}
   				
   				
   			}
   			return Vector2.ZERO;
  
   		}
        public static List<String> GetVideoModes(Root root, bool only32Bit, int minXResolution, int minYResolution)
        {
            List<String> availableModes = new List<String>();

            ConfigOption_NativePtr videoModeOption;

            // staram sie znalezc opcje konfiguracyjna Video Mode
            ConfigOptionMap map = root.RenderSystem.GetConfigOptions();
            foreach (KeyValuePair<string, ConfigOption_NativePtr> m in map)
            {
                if (m.Value.name.Equals(FrameWorkStaticHelper.C_VIDEO_MODE))
                {
                    videoModeOption = m.Value;
                    break;
                }
            }

            // nie ma takiej mozliwosci, zebym nie znalazl
            // konwertuje wektor na liste
            foreach (String s in videoModeOption.possibleValues)
            {
                bool add = true;
                if (only32Bit)
                {
                    if(!s.Contains("32-bit")) add = false;
                }

                if (minXResolution > 0 || minYResolution > 0)
                {
                    try
                    {
                        Match match = Regex.Match(s, "([0-9]+) x ([0-9]+).*");

                        int xRes = Int32.Parse(match.Groups[1].Value);
                        int yRes = Int32.Parse(match.Groups[2].Value);

                        if (xRes < minXResolution || yRes < minYResolution)
                        {
                            add = false;
                        }
                    }
                    catch
                    {
                    }
                   
                }

                if(add)
                {
                    availableModes.Add(s);
                }
                

                
            }

            return availableModes;
        }

        public static List<String> GetVideoModes(Root root)
        {
            return GetVideoModes(root, false, 0,0);
        }

        public static bool CreateSoundSystem(Camera camera, FreeSL.FSL_SOUND_SYSTEM ss)
        {
            return SoundManager3D.Instance.InitializeSound(camera, ss);
        }

        public static bool GetJoystickButton(JoyStick j, int button)
        {
            if(j!=null)
            {
                if ((int)button - 1 < j.JoyStickState.ButtonCount)// indexed from 0
                {
                    try
                    {
                        return j.JoyStickState.GetButton((int)button - 1); 
                    }
                    catch (Exception ex)
                    {
                        LogManager.Singleton.LogMessage("Unable to read joystick button state (" + button +"), please check your joystick and Keymap.ini file");
                        return false;
                    }
                  
                }
            }
            return false;
        }

        public static Vector2 GetJoystickVector(JoyStick j)
        {
            if(j!=null)
            {
                if(j.JoyStickState.VectorCount > 0)
                {
                    MOIS.Vector3 v = j.JoyStickState.GetVector(0);
                    return new Vector2(v.x, v.y);
                } else 
                {
                   
                    int num = j.JoyStickState.AxisCount;
                    if(num >= 2)
                    {
                        int axisCount = j.JoyStickState.AxisCount;

                        if (KeyMap.Instance.JoystickVerticalAxisNo > axisCount - 1)
                        {
                            throw new Exception("JoystickVerticalAxisNo is greater than number of axes. Please change it in your KeyMap.ini");
                        }

                        if (KeyMap.Instance.JoystickHorizontalAxisNo > axisCount - 1)
                        {
                            throw new Exception("JoystickHorizontalAxisNo is greater than number of axes. Please change it in your KeyMap.ini");
                        }


                        Axis_NativePtr axisV = j.JoyStickState.GetAxis(KeyMap.Instance.JoystickVerticalAxisNo);
                        Axis_NativePtr axisH = j.JoyStickState.GetAxis(KeyMap.Instance.JoystickHorizontalAxisNo);
                     

                        double v = (1.0 * axisV.abs / JoyStick.MAX_AXIS);
                        double h = (1.0 * axisH.abs / JoyStick.MAX_AXIS);

                        // Console.WriteLine(h + " " + v);
                     
                      
                        if (Math.Abs(v) < KeyMap.Instance.JoystickDeadZone) v = 0;
                        else if (v > 1) v = 1;
                        else if (v < -1) v = -1;

                        if (Math.Abs(h) < KeyMap.Instance.JoystickDeadZone) h = 0;
                        else if (h > 1) h = 1;
                        else if (h < -1) h = -1;
                     
                      
                        return new Vector2((float)h, (float)-v);
                    } else
                    {
                        // no joys and no POVs 
                        return Vector2.ZERO;
                    }
                }
            } else
            {
                return Vector2.ZERO;
            }

        }

        public static void DestroyScenes(IFrameWork framework)
        {
            try
            {
                if (framework.SceneMgr != null)
                {
                    framework.SceneMgr.DestroyAllBillboardSets();
                    framework.SceneMgr.DestroyAllEntities();
                    framework.SceneMgr.DestroyAllManualObjects();
                    framework.SceneMgr.DestroyAllInstancedGeometry();
                    framework.SceneMgr.DestroyAllMovableObjects();
                    framework.SceneMgr.ClearScene();
                    // sceneMgr.DestroyAllCameras();
                    framework.SceneMgr.Dispose();
                    Root.Singleton.DestroySceneManager(framework.SceneMgr);
                    framework.SceneMgr = null;
                }
                if (framework.MinimapMgr != null)
                {
                    framework.MinimapMgr.DestroyAllCameras();
                    framework.MinimapMgr.DestroyAllEntities();
                    framework.MinimapMgr.DestroyAllEntities();
                    framework.MinimapMgr.ClearScene();
                    Root.Singleton.DestroySceneManager(framework.MinimapMgr);
                    framework.MinimapMgr = null;
                }

                if (framework.OverlayMgr != null)
                {
                    framework.OverlayMgr.DestroyAllCameras();
                    framework.OverlayMgr.ClearScene();
                    Root.Singleton.DestroySceneManager(framework.OverlayMgr);
                    framework.OverlayMgr = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception)
            {
                
              
            }
           
        }
    }
}