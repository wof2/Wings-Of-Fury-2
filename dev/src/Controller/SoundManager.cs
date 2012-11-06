/*
 * Copyright 2008 Adam Witczak, Jakub Tê¿ycki, Kamil S³awiñski, Tomasz Bilski, Emil Hornung, Micha³ Ziober
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
using System.IO;
using FSLOgreCS;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX.DirectSound;
using Mogre;
using Wof.Model.Level.Planes;
using Buffer=Microsoft.DirectX.DirectSound.Buffer;
using Math=Mogre.Math;
using Plane=Mogre.Plane;

namespace Wof.Controller
{
   

    /// <summary>
    /// Singleton odpowiadaj¹cy za proste dŸwiêki
    /// </summary>
    internal class SoundManager
    {
     
        private static Device dsDevice;

        public static Device DsDevice
        {
            get { return dsDevice; }
            set { dsDevice = value; }
        }


        private static SoundManager instance;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        private Boolean problemWithSound = false;

        public Boolean ProblemWithSound
        {
            get { return problemWithSound || soundDisabled; }
            set { problemWithSound = value; }
        }

        private Boolean soundDisabled = false;

        public Boolean SoundDisabled
        {
            get { return soundDisabled; }
            set { soundDisabled = value; }
        }

        public int MaxMusicTrackNo
        {
            get { return maxMusicTrackNo; }
        }

        public int LastRandomMusicTrackNo
        {
            get { return lastRandomMusicTrackNo; }
        }

        private int maxMusicTrackNo = 1;
        private int lastRandomMusicTrackNo = 1;





        // ekran gry
        private Audio gearUpSound;
        private Audio gearDownSound;
        private Audio startEngineSound;
        private Audio stopEngineSound;
        private Audio startEngineSound2;
        private Audio stopEngineSound2;
        private Audio failedEngineSound;
        private Audio bunkerFireSound;
        private Audio bunkerFireSound2;
        private Audio flakBunkerFireSound;
        private Audio flakBunkerFireSound2;
        
        private Audio fortressFireSound;
        private Audio ricochetSound;
        
        private Audio shipFireSound;
        
        private Audio explosionSound;
        private Audio explosionSound3;
        
        
        private Audio explosionSound2;
    
        
        private Audio waterExplosionSound;
        private Audio missileSound;
        private Audio smallMissileSound;

        
       
        private Audio torpedoSound;
        private Buffer torpedoRunSound;
      
        
        private Audio catchPlaneSound;
        private Audio collisionSound;

        private Audio bunkerRebuild;
        private Audio startSubmergingSound;     
        private Audio achievementFulFilled;     
        
        private Audio reloadSound;
        private Audio buzzerSound;
        private Audio bombSound;
        private Audio incorrectStart;
        private Audio fanfare;

        private Buffer engineIdleSound, engineIdleSound2, engineIdleSound3;
        private Buffer engineIdleFaultySound;


        private Buffer gunFireBuffer, gunFireBuffer2;
        private Buffer waterBubblesBuffer;
        private Buffer oceanSound;

        private Buffer currentEngineIdleSound;
        private Random random;

        public bool ShouldLoadNextMusic = false;

        private SoundManager()
        {
            try
            {
                int i = 1;
                while(File.Exists("music/music"+i.ToString()+".ogg"))
                {
                    maxMusicTrackNo = MaxMusicTrackNo + 1;
                    i++;
                }
                maxMusicTrackNo = MaxMusicTrackNo - 1;

                random = new Random();


                gearUpSound = new Audio("sounds/gear_up.wav");
                gearDownSound = new Audio("sounds/gear_down.wav");
                startEngineSound = new Audio("sounds/enginestart.wav");
                stopEngineSound = new Audio("sounds/enginestop.wav");
                startEngineSound2 = new Audio("sounds/enginestart_f4u.wav");
                stopEngineSound2 = new Audio("sounds/enginestop_f4u.wav");


                
                failedEngineSound = new Audio("sounds/startengine.wav");
                bunkerFireSound = new Audio("sounds/cannon.wav");
                bunkerFireSound2 = new Audio("sounds/cannon2.wav");                
                flakBunkerFireSound = new Audio("sounds/flak.wav");
                flakBunkerFireSound2 = new Audio("sounds/flak2.wav");
                
                ricochetSound = new Audio("sounds/ricochet.wav");
                
                fortressFireSound = new Audio("sounds/fortress_cannon.wav");
                shipFireSound = new Audio("sounds/ship_cannon.wav");
                
                         
                explosionSound = new Audio("sounds/explosion.wav");
                explosionSound3 = new Audio("sounds/explosion3.wav");
                
                explosionSound2 = new Audio("sounds/explosion2.wav");
            
                waterExplosionSound = new Audio("sounds/watersplash.wav");
                missileSound = new Audio("sounds/missile.wav");
                smallMissileSound = new Audio("sounds/small_missile.wav");              
                
                torpedoSound = new Audio("sounds/torpedo.wav");
            
                catchPlaneSound = new Audio("sounds/landing.wav");
                bunkerRebuild = new Audio("sounds/construction.wav");
                
                achievementFulFilled = new Audio("sounds/achievement.wav");
                
                startSubmergingSound = new Audio("sounds/ship_siren.wav");
                reloadSound = new Audio("sounds/reload.wav");
                buzzerSound = new Audio("sounds/buzzer.wav");
                bombSound = new Audio("sounds/bombwhistle.wav");
                incorrectStart = new Audio("sounds/incorrectstart.wav");
                fanfare = new Audio("sounds/fanfare.wav");

                currentEngineIdleSound = null;
                collisionSound = new Audio("sounds/collision.wav");

                engineIdleSound = new Buffer("sounds/engineidle.wav",
                                             dsDevice);
               

                engineIdleSound2 = new Buffer("sounds/engineidle_f4u.wav",
                                            dsDevice);

                engineIdleSound3 = new Buffer("sounds/engineidle_b25.wav",
                                         dsDevice);

                engineIdleFaultySound = new Buffer("sounds/engineidlefaulty.wav",
                                          dsDevice);
            
                
               /* enemyEngineSound = new Buffer("sounds/engineidle.wav",
                                              dsDevice);

                enemyEngineSound.Frequency =
                    enemyEngineSound.Format.SamplesPerSecond + C_NOMINAL_ENEMY_FREQ;*/


                gunFireBuffer = new Buffer("sounds/machinegun.wav",
                                           dsDevice);

                gunFireBuffer2 = new Buffer("sounds/machinegun_b25.wav",
                                          dsDevice);

                waterBubblesBuffer = new Buffer("sounds/waterbubbles.wav",
                                                dsDevice);
              

                oceanSound = new Buffer("sounds/ocean.wav", dsDevice);
                oceanSound.Volume = -2400;

                torpedoRunSound = new Buffer("sounds/torpedo_run.wav", dsDevice);
               // torpedoRunSound.Volume = -2400;

                
            }
            catch (Exception ex)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "DirectSound init error: "+ex.Message);
                ProblemWithSound = true;
            }
        }
       
        public void PreloadRandomIngameMusic()
        {
            random = new Random();
            if (lastRandomMusicTrackNo == 0)
            {
                lastRandomMusicTrackNo = random.Next(1, MaxMusicTrackNo + 1);
            }else
            {
                lastRandomMusicTrackNo++;
                if(lastRandomMusicTrackNo > maxMusicTrackNo)
                {
                    lastRandomMusicTrackNo = 1;
                }
            }
            
            if (!EngineConfig.AudioStreaming)
            {
                PlayIngameMusic(LastRandomMusicTrackNo, EngineConfig.MusicVolume, true);
            }
        }


        /// <summary>
        /// Plays random music track
        /// </summary>
        public void PlayRandomIngameMusic(int volume)
        {
            PlayIngameMusic(LastRandomMusicTrackNo, volume, false);
        }


        protected void PlayIngameMusic(int no, int volume, bool preloadOnly)
        {
            string music = "music/music" + no + ".ogg";
            SoundManager3D.Instance.PlayAmbientMusic(music, volume, false, preloadOnly);
        }

        public void PlayEndingTheme()
        {
            SoundManager3D.Instance.PlayAmbientMusic("music/ending.ogg", EngineConfig.MusicVolume, false, true, EngineConfig.AudioStreaming);
            // Play(mainTheme);
        }

        public void PlayMainTheme()
        {
            SoundManager3D.Instance.PlayAmbientMusic("music/themesong.ogg", EngineConfig.MusicVolume, false, true, EngineConfig.AudioStreaming);
            // Play(mainTheme);
        }

        public void StopMusic()
        {
            // Stop(mainTheme);
            SoundManager3D.Instance.StopAmbientMusic();
        }


        public void PlayGearUpSound()
        {
            Play(gearUpSound);
        }

        public void PlayGearDownSound()
        {
            Play(gearDownSound);
        }




        public void PlayStartEngineSound(Model.Level.Planes.Plane p, EventHandler startHandler)
        {
            if (ProblemWithSound)
            {
                return;
            }
            startEngineSound.Ending += startHandler;
            if(EngineConfig.CurrentPlayerPlaneType == PlaneType.P47)
            {
               // LoopDXSound(engineIdleSound, -1000);
                Play(startEngineSound);
            }
            else
            {
                //LoopDXSound(engineIdleSound2, -1000);
                Play(startEngineSound2);
            }
            LoopEngineSound(p);
           
           
        }

        public void PlayStopEngineSound()
        {
            if (EngineConfig.CurrentPlayerPlaneType == PlaneType.P47)
            {
                Play(stopEngineSound);
            }
            else
            {
                Play(stopEngineSound2);
            }
        }

        public void PlayFailedEngineSound()
        {
            Play(failedEngineSound);
        }

        public void PlayFailedSoundIfCan()
        {
            if (ProblemWithSound)
            {
                return;
            }
            if (failedEngineSound.CurrentPosition == failedEngineSound.Duration ||
                failedEngineSound.CurrentPosition == 0.0)
            {
                PlayFailedEngineSound();
            }
        }

        public void PlayBunkerFireSound()
        { 
            Play(bunkerFireSound);
        }
        
        public void PlayBunkerFireSound2()
        {          	
          
            Play(bunkerFireSound2);
        }
        public void PlayFlakBunkerFireSound()
        {          	
            if(this.random.NextDouble() > 0.5)
            {
                Play(flakBunkerFireSound);
            }
            else
            {
                Play(flakBunkerFireSound2);
            }
            
          
        }
        
 		public void PlayFortressFireSound()
        {
            Play(fortressFireSound);
        }
 		
 		public void PlayShipFireSound()
        {
            Play(shipFireSound);
        }
 		
 		public void PlayRicochetSound()
        {
            Play(ricochetSound);
        }
 		
 		public bool IsRicochetBeingPlayed()
 		{
 			return  ricochetSound.CurrentPosition > 0 && ricochetSound.CurrentPosition < ricochetSound.Duration;
 		}




        public void PlayHeavyExposionSound()
        {
              Play(explosionSound2, -300);
        }


        public void PlayExposionSound()
        {
            if(this.random.NextDouble() > 0.5)
            {
                Play(explosionSound, 200);
            }
            else
            {
                Play(explosionSound3, 200);
            }
            
        }

        public void PlayWaterExplosionSound()
        {
            Play(waterExplosionSound);
        }

        public void PlayMissleSound()
        {
            Play(missileSound);
        }

        public void PlaySmallMissleSound()
        {
            Play(smallMissileSound);
        }

        
     
        public void PlayTorpedoSound()
        {
            Play(torpedoSound);
        }

        public void LoopTorpedoRunSound()
        {
            LoopDXSound(torpedoRunSound); //, -1000);
        }

        public void HaltTorpedoRunSound()
        {
            HaltDXSound(torpedoRunSound);
        }

     

        public void PlayCatchPlaneSound()
        {
            Play(catchPlaneSound);
        }

        public void PlayCollisionPlaneSound()
        {
            Play(collisionSound, 500);
        }

        public void PlayStartSubmergingSound()
        {  
        	Play(startSubmergingSound);             
        }

        public void PlayBunkerRebuild()
        {
            Play(bunkerRebuild);
        }
        
        public void PlayAchievementFulFilled()
		{
			Play(achievementFulFilled, 2000);
		}

        public void PlayReloadSound()
        {
            Play(reloadSound);
        }

        public void PlayBuzzerSound()
        {
            Play(buzzerSound);
        }

        public void PlayBombSound()
        {
            Play(bombSound);
        }

        public void PlayIncorrectStart()
        {
         //   Console.WriteLine("Incorrect");
            Play(incorrectStart);
        }

        public void PlayFanfare()
        {
            Play(fanfare);
        }

        private void Play(Audio audio, int soundVolumeModifier)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
                if (EngineConfig.SoundVolume == 0)
                {
                    return;
                }

                audio.CurrentPosition = 0.0;
                audio.Play();
                audio.Volume = System.Math.Min(0, getBaseVolume() + soundVolumeModifier);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ProblemWithSound = true;
            }
        }

        private void Play(Audio audio)
        {
            Play(audio, 0);
        }

        private int getBaseVolume()
        {
          
            return 20*(EngineConfig.SoundVolume - 100);
        }

        private void Stop(Audio audio)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
                audio.Stop();
            }
            catch (Exception)
            {
                ProblemWithSound = true;
            }
        }
        
       

        public void OnEngineFaulty(Model.Level.Planes.Plane p)
        {
           if(!p.IsEnemy)
           {
               if (currentEngineIdleSound.Status.Playing)
               {
                    HaltDXSound(currentEngineIdleSound);
                    LoopEngineSound(p);
               }
              
           }
        }

        public void OnEngineRepaired(Model.Level.Planes.Plane p)
        {
            if (!p.IsEnemy)
            {
                if (currentEngineIdleSound!=null && currentEngineIdleSound.Status.Playing)
                {
                    HaltDXSound(currentEngineIdleSound);
                    LoopEngineSound(p);
                }
            }
        }


        public void LoopEngineSound(Model.Level.Planes.Plane p)
        {
            if(p.IsEnemy) return;

       //     Console.WriteLine("Loop engine. faulty: " + p.IsEngineFaulty);
            SelectEngineIdleSound(p);
            LoopDXSound(currentEngineIdleSound, -1200);
        }

        public void HaltEngineSound(Model.Level.Planes.Plane p)
        {
        //    Console.WriteLine("dzwiek - gaszenie");
            HaltDXSound(currentEngineIdleSound);
           
        }

        public void LoopOceanSound()
        {
            LoopDXSound(oceanSound, -1000);
        }

        public void HaltOceanSound()
        {
            HaltDXSound(oceanSound);
        }

      
        private void SelectEngineIdleSound(Model.Level.Planes.Plane p)
        {
            if (p.IsEngineFaulty)
            {
                currentEngineIdleSound = engineIdleFaultySound;
              
            }
            else
            {
                if (EngineConfig.CurrentPlayerPlaneType == PlaneType.P47)
                {
                    currentEngineIdleSound = engineIdleSound;
                }
                else if (EngineConfig.CurrentPlayerPlaneType == PlaneType.F4U)
                {
                    currentEngineIdleSound = engineIdleSound2;
                }
                 else
                {
                    currentEngineIdleSound = engineIdleSound3;
                }

            }
            
        }
        public void SetEngineFrequency(Model.Level.Planes.Plane p)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
                if(currentEngineIdleSound == null)
                {
                    SelectEngineIdleSound(p);
                }
                currentEngineIdleSound.Frequency = currentEngineIdleSound.Format.SamplesPerSecond + (int)p.AirscrewSpeed * 7;
               
            }
            catch (Exception)
            {
                ProblemWithSound = true;
            }
        }

        public void SetEnemyEngineVolume(int dist, int freq)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
               // enemyEngineSound.Volume = getBaseVolume() - 1000 - (int) System.Math.Floor(dist/2.0);
               // enemyEngineSound.Frequency =
               //     enemyEngineSound.Format.SamplesPerSecond + C_NOMINAL_ENEMY_FREQ + freq;
            }
            catch (Exception)
            {
                ProblemWithSound = true;
            }
        }

        private void LoopGunFireSound()
        {
            if(EngineConfig.CurrentPlayerPlaneType == PlaneType.B25)
            {
                LoopDXSound(gunFireBuffer2);
                
            } else
            {
                LoopDXSound(gunFireBuffer);
            }
        
            
        }

        public void LoopGunFireSoundIfCan()
        {
          //  Console.WriteLine("Looping");
            if (ProblemWithSound)
            {
                return;
            }
           
            if (EngineConfig.CurrentPlayerPlaneType == PlaneType.B25)
            {
                if (!gunFireBuffer2.Status.Looping)
                {
                    LoopGunFireSound();
                }
            }
            else
            {
                if (!gunFireBuffer.Status.Looping)
                {
                    LoopGunFireSound();
                }
            }
               
            
        }

        public void HaltGunFireSound()
        {
         //   Console.WriteLine("Halting");
            if (EngineConfig.CurrentPlayerPlaneType == PlaneType.B25)
            {
                HaltDXSound(gunFireBuffer2);
            }
            else
            {
                HaltDXSound(gunFireBuffer);
            }

            
        }

         public void SingleWaterBubblesSound()
         {
             PlayDXSound(waterBubblesBuffer, 0, BufferPlayFlags.Default);
         }
         
     
         
         

        public void LoopWaterBubblesSound()
        {
            LoopDXSound(waterBubblesBuffer);
        }

        public void HaltWaterBubblesSound()
        {
            HaltDXSound(waterBubblesBuffer);
        }

      
        
        private void PlayDXSound(Buffer buffer, int soundVolumeModifier, BufferPlayFlags flags)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
                if (EngineConfig.SoundVolume == 0)
                {
                    return;
                }

                buffer.Play(0, flags);
                buffer.Volume = System.Math.Min(0, getBaseVolume() + soundVolumeModifier);
            }
            catch (Exception)
            {
                ProblemWithSound = true;
            }
        }

        private void LoopDXSound(Buffer buffer, int soundVolumeModifier)
        {
            PlayDXSound(buffer, soundVolumeModifier, BufferPlayFlags.Looping);
        }

        private void LoopDXSound(Buffer buffer)
        {
            LoopDXSound(buffer, 0);
        }

        private void HaltDXSound(Buffer buffer)
        {
            if (ProblemWithSound)
            {
                return;
            }
            try
            {
                buffer.Stop();
            }
            catch (Exception)
            {
                ProblemWithSound = true;
            }
        }

      
    	
		
    }
}