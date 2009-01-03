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
using Mogre;
using Wof.Controller;
using Wof.Misc;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.View.Effects;
using Math=Mogre.Math;

namespace Wof.View.TileViews
{
    public abstract class EnemyInstallationTileView : MiddleIslandTileView
    {
        private BillboardSet lightBillboardSet;
        private Billboard lightBillboard;
        private Light light;


        protected Entity installationEntity;
        protected AnimationState animationState;


        protected SceneNode gunNode;

        public SceneNode GunNode
        {
            get { return gunNode; }
        }

        public EnemyInstallationTileView(LevelTile levelTile, FrameWork framework)
            : base(levelTile, framework)
        {
        }

        public abstract void Restore();

        /// <summary>
        /// Wizualizacja ostrza³u prowadzonego przez instalacjê
        /// </summary>
        public abstract void GunFire();


        protected void SetLightFlareVisibility(bool visible)
        {
            if (lightBillboardSet != null)
            {
                lightBillboardSet.Visible = visible;
                light.Visible = visible;
            }
        }

        protected void InitLightFlare(ColourValue c1, Vector3 localPosition, Vector2 scale)
        {
            lightBillboardSet = sceneMgr.CreateBillboardSet("EnemyInstallation" + tileID + "_lightflare", 1);
            lightBillboardSet.MaterialName = "Examples/Flare";
            lightBillboard = lightBillboardSet.CreateBillboard(localPosition, c1);
            lightBillboard.SetDimensions(scale.x, scale.y);
            installationNode.AttachObject(lightBillboardSet);

            light = sceneMgr.CreateLight("EnemyInstallation" + tileID + "_light");
            light.Type = Light.LightTypes.LT_POINT;
            light.SetAttenuation(5.0f, 0.5f, 0.9f, 0.01f);
            light.DiffuseColour = c1;
            light.SpecularColour = new ColourValue(0.05f, 0.05f, 0.05f);
            light.CastShadows = false;
            SceneNode lightNode = installationNode.CreateChildSceneNode(localPosition);
            lightNode.AttachObject(light);
        }


        public virtual void Destroy(bool smoke, bool firePossibility, bool switchToDieAnimationState)
        {

            if (installationEntity!=null && switchToDieAnimationState && installationEntity.HasVertexAnimation)
            {
                animationState = installationEntity.GetAnimationState("die");
                if (animationState!=null)
                {
                    animationState.Enabled = true;
                    animationState.Loop = false;
                }
               
            }

            if(smoke) EffectsManager.Singleton.Smoke(sceneMgr, installationNode, new Vector3(0, -1, 0), Vector3.UNIT_Y);
            SetLightFlareVisibility(false);

            if (firePossibility && Math.RangeRandom(0.0f, 1.0f) > 0.8f)
            {
                EffectsManager.Singleton.Sprite(sceneMgr, InstallationNode,
                                                new Vector3(2, 2.4f, Math.RangeRandom(-4, 4)), new Vector2(5, 5),
                                                EffectsManager.EffectType.FIRE, true, 0);
            }
        }

        public virtual void Destroy()
        {
            Destroy(true, true, true);
        }

        public override void updateTime(float timeSinceLastFrameUpdate)
        {
            base.updateTime(timeSinceLastFrameUpdate);
            if (animationState != null && !animationState.HasEnded)
            {
                animationState.AddTime(timeSinceLastFrameUpdate);
            }

        }
    }
}