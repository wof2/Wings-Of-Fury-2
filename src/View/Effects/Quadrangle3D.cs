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
using Wof.Misc;
using Wof.Model.Level.Common;

namespace Wof.View.Effects
{
    public class Quadrangle3D 
    {
    	private string name;
        private ManualObject manualObject;
        

        /// <summary>
        /// Pobiera encje (reprezentacje ogre) quada
        /// </summary>
		public ManualObject ManualObject {
			get { return manualObject; }
		}

        public Quadrangle Quadrangle
        {
            get { return quadrangle; }
        }


        /// <summary>
        /// Wczorok¹t w view. Klasa pomocnicza
        /// <author>Adam Witczak</author>
        /// </summary>
        /// <param name="name"></param>
        public Quadrangle3D(SceneManager sceneMgr, String name) 
        {
            if(sceneMgr.HasManualObject(name))
            {
                sceneMgr.DestroyManualObject(name);
            }
        	manualObject = sceneMgr.CreateManualObject(name);
          
          
        }
        
        /// <summary>
        /// Wierzcholki quadrangle'a 3D w porzadku CCW
        /// </summary>
        private float[][] corners;

        
        
        /// <summary>
        /// Pobiera rogi quada w kolejnosci CCW
        /// </summary>
        /// <returns></returns>
        public float[][] GetCorners3DArray()
        {
        	return corners;
        }


       

        private Quadrangle quadrangle;
        
        /// <summary>
        /// Buduje quada o wymaganych parametrach. Quad bedzie mogl byc wyswietlony w przestrzeni 3D. Ma nakladn¹ teksture wg textureName
        /// </summary>
        /// <param name="quadrangle"></param>
        /// <param name="origin"></param>
        /// <param name="textureName"></param>
        public void SetCorners3D(Quadrangle quadrangle, Vector3 origin, String textureName)
        {

            this.quadrangle = quadrangle;
        	Vector2 leftBottom = quadrangle.Peaks[0].ToVector2();
            Vector2 leftTop = quadrangle.Peaks[1].ToVector2();
            Vector2 rightTop = quadrangle.Peaks[2].ToVector2();
            Vector2 rightBottom = quadrangle.Peaks[3].ToVector2();

            float extend = 1.1f;


            corners = new float[4][];
            corners[0] = new float[3];
            corners[0][0] = extend * rightBottom.x + origin.x;
            corners[0][1] = extend * rightBottom.y + origin.y;
            corners[0][2] = origin.z;
            
            
            corners[1] = new float[3];
            corners[1][0] = extend * rightTop.x + origin.x;
            corners[1][1] = extend * rightTop.y + origin.y;
            corners[1][2] = origin.z;
            
            
            corners[2] = new float[3];
            corners[2][0] = leftTop.x + origin.x;
            corners[2][1] = leftTop.y + origin.y;
            corners[2][2] = origin.z;
            
            
            corners[3] = new float[3];
            corners[3][0] = leftBottom.x + origin.x;
            corners[3][1] = leftBottom.y + origin.y;
            corners[3][2] = origin.z;
            
            
            float textureTop    = 0;
	        float textureLeft   = 0;
	        float textureBottom = 1;
	        float textureRight  = 1;

      //      manualObject.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_MAIN - 1;

            manualObject.Clear();
            manualObject.Begin("Misc/BoundingQuadrangle", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manualObject.Position( origin.x + leftBottom.x, origin.y + leftBottom.y, origin.z);
            manualObject.TextureCoord(textureLeft, textureBottom);
            manualObject.Position( origin.x + leftTop.x, origin.y + leftTop.y,  origin.z);
            manualObject.TextureCoord(textureLeft, textureTop);
            manualObject.Position( origin.x + rightTop.x, origin.y + rightTop.y,  origin.z);
            manualObject.TextureCoord(textureRight, textureTop);
            manualObject.Position( origin.x + rightBottom.x, origin.y + rightBottom.y,  origin.z);
            manualObject.TextureCoord(textureRight, textureBottom);
        /*    manualObject.Position( origin.x + leftBottom.x, origin.y + leftBottom.y,  origin.z);
            manualObject.TextureCoord(0, 0);*/
            manualObject.Quad(3,2,1,0);
            manualObject.End();
            
           

            AxisAlignedBox box = new AxisAlignedBox();
            float hwidth = 1.1f * (quadrangle.RightMostX - quadrangle.LeftMostX) * 0.5f;
            float hheight = 1.1f * (quadrangle.HighestY - quadrangle.LowestY) * 0.5f;
            box.SetMinimum(origin.x - hwidth, origin.y - hheight, origin.z - 10);
            box.SetMaximum(origin.x + hwidth, origin.y + hheight, origin.z + 10);

            manualObject.BoundingBox = box;

            MaterialPtr mat = ViewHelper.CloneMaterial("AdMaterial", manualObject.Name + "AdMaterial");
            Pass pass = mat.GetBestTechnique().GetPass(0);
            pass.DepthWriteEnabled = true;
            pass.SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);

            TextureUnitState state = pass.GetTextureUnitState(0);
            state.SetTextureName(textureName);

            manualObject.SetMaterialName(0, mat.Name);
          //  manualObject.SetMaterialName(0, "Concrete");
        }
         

        public void SetCorners(Quadrangle quadrangle)
        {
            manualObject.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
            manualObject.QueryFlags = 0;
            Vector2 leftBottom = UnitConverter.LogicToWorldUnits(quadrangle.Peaks[0]);
            Vector2 leftTop = UnitConverter.LogicToWorldUnits(quadrangle.Peaks[1]);
            Vector2 rightTop = UnitConverter.LogicToWorldUnits(quadrangle.Peaks[2]);
            Vector2 rightBottom = UnitConverter.LogicToWorldUnits(quadrangle.Peaks[3]);


            manualObject.Clear();
            manualObject.Begin("Misc/BoundingQuadrangle", RenderOperation.OperationTypes.OT_LINE_STRIP);
            manualObject.Position(leftBottom.x, leftBottom.y, 0);
            manualObject.TextureCoord(0, 0);
            manualObject.Position(leftTop.x, leftTop.y, 0);
            manualObject.TextureCoord(0, 1);
            manualObject.Position(rightTop.x, rightTop.y, 0);
            manualObject.TextureCoord(1, 1);
            manualObject.Position(rightBottom.x, rightBottom.y, 0);
            manualObject.TextureCoord(0, 1);
            manualObject.Position(leftBottom.x, leftBottom.y, 0);
            manualObject.TextureCoord(0, 0);
            manualObject.End();

            AxisAlignedBox box = new AxisAlignedBox();
            box.SetInfinite();
            manualObject.BoundingBox = box;
        }
    }
}