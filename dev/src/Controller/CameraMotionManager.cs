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

using Mogre;
using Wof.Misc;
using Wof.Model.Configuration;
using Math=System.Math;
using Plane=Wof.Model.Level.Planes.Plane;

namespace Wof.Controller
{
    /// <summary>
    /// Kontroluje po³o¿enie kamer w trakcie rozgrywki
    /// <author>Adam Witczak</author>
    /// </summary>
    public static class CameraMotionManager
    {
        public static void ManageMini(Camera minimapC, Plane playerPlane, FrameEvent evt)
        {
            // ruch kamery na minimapy
            if (FrameWork.DisplayMinimap)
            {
                Vector2 v = UnitConverter.LogicToWorldUnits(playerPlane.Position);
                Vector3 minimapCamPos = minimapC.Position;
                minimapCamPos.x = v.x; // przesuwamy tylko X
                minimapC.SetPosition(minimapCamPos.x, minimapCamPos.y, minimapCamPos.z);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">Kamera g³ówna</param>
        /// <param name="playerPlane">Obiekt samolotu gracza (model)</param>
        /// <param name="evt">FrameEvent (chodzi o czas od ostatniej klatki)</param>
        /// <param name="manualZoom"></param>
        /// <returns></returns>
        public static void Manage(Camera c, Plane playerPlane, FrameEvent evt, float manualZoom)
        {
            Plane p = null;
            if (playerPlane != null) p = playerPlane;
            Vector3 translateVector = Vector3.ZERO;

            float t = evt.timeSinceLastFrame;
            if (t >= 1.0f) t = 0.99f;

            float minCamDistance = 15;
            float halfMaxHeight = (float) GameConsts.UserPlane.MaxHeight/2.0f;

            float realHalfMaxHeight = halfMaxHeight*0.95f;
            float camAlt = c.WorldPosition.y + c.Position.y;

            float minCamAlt = 4.0f;


            /* bool duringEmergency = false;

             // awaryjne wynurzenie kamery;)
             if (c.WorldPosition.y + c.Position.y <= 0)
             {
                 // metr nad wod¹
                 Vector3 emergency = new Vector3(0, c.Position.y + c.WorldPosition.y + 1.0f, 0);
                 c.MoveRelative(emergency);
                 duringEmergency = true;
             }*/


            // wysokosc
            if (camAlt < minCamAlt)
            {
                translateVector.y = 40*(minCamAlt - camAlt); // wynurzenie kamery znad powierchni wody                
            }
            else if (camAlt > 10)
            {
                // powrot w ozi Y
                if (c.Position.y > 0.01f && EngineConfig.CurrentGameSpeedMultiplier != EngineConfig.GameSpeedMultiplierSlow)
                {
                    translateVector.y = -(c.Position.y)/10.0f;
                }
            }


            // ZOOMING
            float speed = (float) p.Speed;
            float speedFactor = 0;
            float altFactor = 0;

            // wspolczynnik wysokosci
            if (c.WorldPosition.y > 0)
            {
                altFactor = (float) Math.Log(c.WorldPosition.y + 1, 1.011);
                altFactor = Math.Max(0, altFactor);
            }
            float zoomFactor;
            speedFactor = speed; // wsp. szybkosci

            //altFactor*=altFactor * altFactor;
            if (playerPlane.Position.Y < 30) altFactor /= (30 - (float) playerPlane.Position.Y)*0.1f;
            if (altFactor > 250) altFactor = 250;
            zoomFactor = speedFactor*0.1f + altFactor*0.53f + (-manualZoom+ minCamDistance - c.Position.z);
            
            // Bullet-time
            if(EngineConfig.CurrentGameSpeedMultiplier == EngineConfig.GameSpeedMultiplierSlow)
            {
            	zoomFactor +=  -manualZoom + minCamDistance - c.Position.z + 30.0f;
            }
            
            translateVector.z += zoomFactor;
            
            


            // ruch kamery w zaleznosci od wysokosci
            if (c.WorldPosition.y >= realHalfMaxHeight && EngineConfig.CurrentGameSpeedMultiplier != EngineConfig.GameSpeedMultiplierSlow)
            {
                translateVector.y -= (camAlt - realHalfMaxHeight);
            }
            else if (c.WorldPosition.y < (realHalfMaxHeight*0.75f))
            {
                if (c.Position.y < 0)
                {
                    translateVector.y -= c.Position.y;
                }
            }

            

            // ograniczenia awaryjne  
           // translateVector.z += manualZoom;

            translateVector.x = Math.Min(translateVector.x, 500);
            translateVector.y = Math.Min(translateVector.y, 500);
            translateVector.z = Math.Min(translateVector.z, 500);

            translateVector.x = Math.Max(translateVector.x, -500);
            translateVector.y = Math.Max(translateVector.y, -500);
            translateVector.z = Math.Max(translateVector.z, -500);

            
            // uzaleznij od czasu
            translateVector *= t;
            c.MoveRelative(translateVector);
        }
    }
}