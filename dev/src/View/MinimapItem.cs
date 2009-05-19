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

namespace Wof.View
{
    /// <summary>
    /// Reprezentacja obiektów na minimapie
    /// <author>Adam Witczak , Kamil S³awiñski</author>
    /// </summary>
    public class MinimapItem : IDisposable
    {
        public enum ColourType
        {
            SOLID,
            SHADED
        }

        protected SceneManager minimapMgr;

        public SceneManager MinimapMgr
        {
            get { return minimapMgr; }
        }

        protected SceneNode realObjectNode;

        public SceneNode RealObjectNode
        {
            get { return realObjectNode; }
        }


        private Entity entity;

        public Entity Entity
        {
            get { return entity; }
        }


        private SceneNode minimapNode;

        public SceneNode MinimapNode
        {
            get { return minimapNode; }
        }

        private String meshName;

        public String MeshName
        {
            get { return meshName; }
        }


        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
        }


        private Entity sizeEnity;

        public Entity SizeEnity
        {
            set { sizeEnity = value; }
            get { return sizeEnity; }
        }

        private Vector2 scaleOverride;

        public Vector2 ScaleOverride
        {
            set { scaleOverride = value; }
            get { return scaleOverride; }
        }

        private float minimapObjectsDepth = 0.05f;

        public float MinimapObjectsDepth
        {
            set { minimapObjectsDepth = value; }
            get { return minimapObjectsDepth; }
        }

        private MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, Entity sizeEnity,
                            Vector2 scaleOverride)
        {
            this.minimapMgr = minimapMgr;
            this.realObjectNode = realObjectNode;
            this.meshName = meshName;
            this.sizeEnity = sizeEnity;
            ScaleOverride = scaleOverride;
            InitOnScene();
        }

        public ColourValue Colour
        {
            set 
            {
                MaterialPtr matPtr, clonedMaterial;
                string colourTxt = value.r + "." + value.g + "." + value.b;

                if (colourType == ColourType.SOLID)
                {
                    clonedMaterial = MaterialManager.Singleton.GetByName("Colours/FlatColour/" + colourTxt);
                    if (clonedMaterial == null)
                    {
                        matPtr = MaterialManager.Singleton.GetByName("Colours/FlatColour");
                        matPtr.Load();
                        clonedMaterial = matPtr.Clone("Colours/FlatColour/" + colourTxt);

                        TextureUnitState textureUnit1 = clonedMaterial.GetBestTechnique().GetPass(0).GetTextureUnitState(0);
                        textureUnit1.SetColourOperationEx(LayerBlendOperationEx.LBX_SOURCE1, LayerBlendSource.LBS_MANUAL,
                                                          LayerBlendSource.LBS_CURRENT, value);
                    }

                    entity.SetMaterialName(clonedMaterial.Name);
                }
                else if (colourType == ColourType.SHADED)
                {
                    clonedMaterial = MaterialManager.Singleton.GetByName("Colours/Colour/" + colourTxt);
                    if (clonedMaterial == null)
                    {
                        matPtr = MaterialManager.Singleton.GetByName("Colours/Colour");
                        matPtr.Load();
                        clonedMaterial = matPtr.Clone("Colours/Colour/" + colourTxt);
                    }

                    entity.SetMaterialName(clonedMaterial.Name);
                    Pass pass1 = entity.GetSubEntity(0).GetMaterial().GetTechnique(0).GetPass(0);
                    pass1.SetDiffuse(value.r, value.g, value.b, value.a);
                    pass1.SetAmbient(value.r, value.g, value.b);
                }
                matPtr = null;
                clonedMaterial = null;
            }
        }

        private ColourType colourType;
        public ColourType ColourTypeInstance
        {
            set { colourType = value; }
            get { return colourType; }

        }

        private MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, ColourValue colour,
                            ColourType colourType, Entity sizeEnity, Vector2 scaleOverride)
            : this(realObjectNode, minimapMgr, meshName, sizeEnity, scaleOverride)
        {
            this.colourType = colourType;
            this.Colour = colour;
        }


        public MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, ColourValue colour,
                           Vector2 scaleOverride)
            : this(realObjectNode, minimapMgr, meshName, colour, ColourType.SOLID, null, scaleOverride)
        {
        }

        public MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, String materialName,
                           Entity sizeEnity)
            : this(realObjectNode, minimapMgr, meshName, sizeEnity, Vector2.ZERO)
        {
            entity.SetMaterialName(materialName);
        }

        public MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, String materialName,
                           Vector2 scaleOverride)
            : this(realObjectNode, minimapMgr, meshName, materialName, null)
        {
            this.scaleOverride = scaleOverride;
        }


        public MinimapItem(SceneNode realObjectNode, SceneManager minimapMgr, String meshName, ColourValue colour,
                           Entity sizeEntity)
            : this(realObjectNode, minimapMgr, meshName, colour, ColourType.SOLID, sizeEntity, Vector2.ZERO)
        {
        }


        public void Show()
        {
            if (EngineConfig.DisplayMinimap)
            {
                isVisible = true;
                minimapNode.SetVisible(true);
            }
        }

        public void Hide()
        {
            if (EngineConfig.DisplayMinimap)
            {
                isVisible = false;
                minimapNode.SetVisible(false);
            }
        }

        protected void InitOnScene()
        {
            entity = minimapMgr.CreateEntity(realObjectNode.Name + "_OnMinimap", meshName);
            minimapNode = minimapMgr.RootSceneNode.CreateChildSceneNode(realObjectNode.Name + "_MinimapNode");
            minimapNode.AttachObject(entity);
            Refresh();
            isVisible = true;
        }


        public void Refresh()
        {
        
            // minimapNode.SetOrientation(realObjectNode._getDerivedOrientation().w, realObjectNode._getDerivedOrientation().x, realObjectNode._getDerivedOrientation().y, realObjectNode._getDerivedOrientation().z);

            // ROTATION
            Vector3 src = minimapNode._getDerivedOrientation()*Vector3.NEGATIVE_UNIT_Z;
            Vector3 dest = realObjectNode._getDerivedOrientation()*Vector3.NEGATIVE_UNIT_Z;

            src.Normalise();
            dest.Normalise();

            if ((1.0f + src.DotProduct(dest)) < 0.0001f)
                // Working around 180 degree quaternion rotation quirk                         
            {
                minimapNode.Yaw(new Degree(180));
            }
            else
            {
                minimapNode.Rotate(src.GetRotationTo(dest), Node.TransformSpace.TS_WORLD);
            }


            // POSITION
            Vector3 wpos = realObjectNode._getDerivedPosition();
          

            // SCALE
            Vector3 scale = new Vector3(0, 1, 1);
            if (sizeEnity != null)
            {
                scale = ViewHelper.DropXCoordinate(sizeEnity.BoundingBox.Size);
                    // z ma byc rowne 0. Jako ze modele ladowane sa przodem do -Z, obciêcie X daje po obrocie odpowiedni efekt
            }
           
           
            if (scaleOverride.x > 0) scale.z = scaleOverride.x;
            if (scaleOverride.y > 0) scale.y = scaleOverride.y;
            
            scale.x = minimapObjectsDepth;
            minimapNode.SetScale(scale);

            minimapNode.SetPosition(wpos.x + scale.z / 2.0f, wpos.y, minimapNode.Position.z);
        }

        public void Dispose()
        {
            entity.Dispose();
            entity = null;
            sizeEnity.Dispose();
            sizeEnity = null;
            minimapNode.Dispose();
            minimapNode = null;
        }
    }
}