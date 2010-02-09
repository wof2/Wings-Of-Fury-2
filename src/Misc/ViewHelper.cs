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
using System.Collections;
using System.Collections.Generic;
using Mogre;
using Wof.Controller;
using Wof.Model.Level.Common;
using Wof.View.Effects;
using Math=Mogre.Math;
using Wof.Controller.AdAction;

namespace Wof.Misc
{
    /// <summary>
    /// Klasa pomocnicza View
    /// <author>Adam Witczak</author>
    /// </summary>
    public class ViewHelper
    {

        public const string C_MESH_EXT = ".mesh";
        private static Hashtable helperQuandrangles = new Hashtable();

        public static void RefreshBoundingQuandrangles()
        {
            foreach (Quadrangle qs in helperQuandrangles.Keys)
            {
                (helperQuandrangles[qs] as Quadrangle3D).SetCorners(qs);
            }
        }


        public static void AttachAxes(SceneManager sceneMgr, SceneNode node, float scale)
        {
            if (EngineConfig.DisplayAxes)
            {
                Entity a = sceneMgr.CreateEntity(node.Name + "_Axes", "axes.mesh");
                a.SetMaterialName("Misc/Axes");
                SceneNode axesNode = node.CreateChildSceneNode(node.Name + "_AxesNode");
                float len = node.GetAttachedObject(0).BoundingRadius;
                float rescale = len/a.BoundingRadius;
                axesNode.AttachObject(a);
                axesNode.Scale(new Vector3(rescale*scale, rescale*scale, rescale*scale));
            }
        }

        public static void AttachQuadrangles(SceneManager sceneMgr, IRenderableQuadrangles target)
        {
            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                foreach (Quadrangle q in target.BoundingQuadrangles)
                {
                    if (!helperQuandrangles.ContainsKey(q))
                    {
                        Quadrangle3D quadrangle3D =
                            new Quadrangle3D(sceneMgr, "BoundingQuadrangle_" + target.Name + "_" + q.GetHashCode());
                        quadrangle3D.SetCorners(q);
                        sceneMgr.RootSceneNode.AttachObject(quadrangle3D.ManualObject);

                        helperQuandrangles.Add(q, quadrangle3D);
                    }
                }
            }
        }

        public static void DetachQuadrangles(SceneManager sceneMgr, IRenderableQuadrangles target)
        {
            if (EngineConfig.DisplayBoundingQuadrangles)
            {
                foreach (Quadrangle q in target.BoundingQuadrangles)
                {
                    if (helperQuandrangles.ContainsKey(q))
                    {
                        sceneMgr.RootSceneNode.DetachObject(((Quadrangle3D) helperQuandrangles[q]).ManualObject);
                       // (helperQuandrangles[q] as Quadrangle3D).Dispose();
                        helperQuandrangles.Remove(q);
                    }
                }
            }
        }


        public static void AttachAxes(SceneManager sceneMgr, SceneNode node)
        {
            AttachAxes(sceneMgr, node, 1.0f);
        }


        public static void RemoveAxes(SceneNode node)
        {
            if (EngineConfig.DisplayAxes)
            {
                node.DetachObject("Axes_" + node.Name);
            }
        }


        public static Vector3 DropXCoordinate(Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }


        public static Vector3 RandomVector3(float rangeX, float rangeY, float rangeZ)
        {
            return
                new Vector3(Math.RangeRandom(-rangeX, rangeX), Math.RangeRandom(-rangeY, rangeY),
                            Math.RangeRandom(-rangeZ, rangeZ));
        }

        public static Vector3 RandomVector3(float rangeXYZ)
        {
            float r = Math.RangeRandom(-rangeXYZ, rangeXYZ);
            return new Vector3(r, r, r);
        }

        public static Vector3 UnsignedRandomVector3(float rangeX, float rangeY, float rangeZ)
        {
            return new Vector3(Math.RangeRandom(0, rangeX), Math.RangeRandom(0, rangeY), Math.RangeRandom(0, rangeZ));
        }

        public static Vector3 UnsignedRandomVector3(float rangeXYZ)
        {
            float r = Math.RangeRandom(0, rangeXYZ);
            return new Vector3(r, r, r);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rangeX"></param>
        /// <param name="rangeY"></param>
        /// <returns></returns>
        public static Vector2 RandomVector2(float rangeX, float rangeY)
        {
            return new Vector2(Math.RangeRandom(-rangeX, rangeX), Math.RangeRandom(-rangeY, rangeY));
        }

        public static Vector2 RandomVector2(float rangeXY)
        {
            float r = Math.RangeRandom(-rangeXY, rangeXY);
            return new Vector2(r, r);
        }

        public static Vector2 UnsignedRandomVector2(float rangeX, float rangeY)
        {
            return new Vector2(Math.RangeRandom(0, rangeX), Math.RangeRandom(0, rangeY));
        }

        public static Vector2 UnsignedRandomVector2(float rangeXY)
        {
            float r = Math.RangeRandom(0, rangeXY);
            return new Vector2(r, r);
        }


        public static MaterialPtr CloneMaterial(String orgName, String cloneName)
        {
            MaterialPtr matPtr;
            if (MaterialManager.Singleton.ResourceExists(cloneName))
            {
                matPtr = MaterialManager.Singleton.GetByName(cloneName);
                if (matPtr.CompilationRequired) matPtr.Compile();
                return matPtr;
            }

            matPtr = MaterialManager.Singleton.GetByName(orgName);
            if (matPtr.IsLoaded) matPtr.Load();
            if (matPtr.CompilationRequired) matPtr.Compile();

            return matPtr.Clone(cloneName);
        }

        public static bool CloneAndApplyMaterial(Entity entity, String orgMatName, String cloneMatName)
        {
            SubEntity e;
            MaterialPtr mat = null;
            for (uint i = 0; i < entity.NumSubEntities; i++)
            {
                e = entity.GetSubEntity(i);
                if (String.Compare(e.MaterialName, orgMatName) == 0)
                {
                    // subenity found
                    if (mat == null) mat = CloneMaterial(orgMatName, cloneMatName);
                    e.MaterialName = mat.Name;
                }
            }
            bool ret = (mat != null);
            mat = null;
            return ret;
        }
        public static string[] GetTextureNames(Entity entity)
        {
            List<string> textures = new List<string>();
            //string[] textures = new string[];
            SubEntity e;
            for (uint i = 0; i < entity.NumSubEntities; i++)
            {
                e = entity.GetSubEntity(i);
                MaterialPtr ptr =  MaterialManager.Singleton.GetByName(e.MaterialName);
                if(!ptr.IsLoaded)
                {
                    ptr.Load(false);
                }
                Technique t = ptr.GetBestTechnique();
                Technique.PassIterator iterator =  t.GetPassIterator();
                foreach (Pass pass in iterator)
                {
                    Pass.TextureUnitStateIterator iterator2 = pass.GetTextureUnitStateIterator();
                    foreach (TextureUnitState unit in iterator2)
                    {
                        if (unit.GetContentType() == TextureUnitState.ContentType.CONTENT_NAMED)
                        {

                            if (unit.TextureName.Length > 0 && !textures.Contains(unit.TextureName))
                            {
                                textures.Add(unit.TextureName);
                            }

                        }
                    }

                }

            }

            return textures.ToArray();
        }

        public static void ReplaceMaterial(Entity entity, String findMatName, String replaceMatName)
        {
            SubEntity e;
            for (uint i = 0; i < entity.NumSubEntities; i++)
            {
                e = entity.GetSubEntity(i);
                if (String.Compare(e.MaterialName, findMatName) == 0)
                {
                    e.MaterialName = replaceMatName;
                }
            }
        }

        public static MaterialPtr BuildPreloaderMaterial(int maxTextures)
        {
        	Pass pass = null;           
            Technique t;
            string name = "PreloaderMaterial";
            MaterialPtr mptr;
            if(MaterialManager.Singleton.ResourceExists(name))
            {
            	mptr = (MaterialPtr)MaterialManager.Singleton.Load(name, MaterialManager.DEFAULT_SCHEME_NAME);
            	if(mptr.GetBestTechnique().NumPasses > 1)            		
            	{
            		// material jest juz gotowy
            		return mptr;	
            	}
            	
            }else 
            {
            	return null;
            }
                      
            int freeMB = ((int)TextureManager.Singleton.MemoryUsage / (1024 * 1024));
            
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL,"Building hardware preloader material. Free system memory: " + freeMB);
            
          	t = mptr.GetBestTechnique();     
            ResourceManager.ResourceMapIterator i = TextureManager.Singleton.GetResourceIterator();       
            int j = 0;
            int k = 0;
            int total = 0;
            while(i.MoveNext())
            {
            	if(j <  System.Math.Ceiling(maxTextures / 2.0f)) {
	            	j++;  
                    
	            	TexturePtr texture = (TexturePtr)(i.Current);
                    if (!texture.Name.Contains(AdManager.C_ADS_DIR))
                    {
                        pass = t.CreatePass();
                        pass.CreateTextureUnitState(texture.Name);
                        pass.SetSceneBlending(SceneBlendFactor.SBF_ZERO, SceneBlendFactor.SBF_ONE);
                        LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL, "Material will be preloaded (" + k + ") - " + texture.Name);
                    }
                    else
                    {
                        
                    }
	            	
	            	k++; 
	            	if(i.MoveNext())
	            	{
                        if (!texture.Name.Contains(AdManager.C_ADS_DIR))
                        {
                            texture = (TexturePtr) (i.Current);
                            if(pass == null)
                            {
                                pass = t.CreatePass();
                            }
                            pass.CreateTextureUnitState(texture.Name);
                            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL,
                                                            "Material will be preloaded (" + k + ") - " + texture.Name);
                        }
	            	    k++; total ++;
	            	}
            	    pass = null;
            	} 
            	total ++;
            	
            }
           
            LogManager.Singleton.LogMessage(LogMessageLevel.LML_CRITICAL,"Total materials to be hardware preloaded: " + k + "/"+ total);
           		
            mptr.Load();
            mptr.Compile();
            return mptr;
            
            
        }
        
        public static MaterialPtr CloneBumpMaterial(String textureMapName, String normalMapName, String outMatName)
        {
            Pass pass;
            MaterialPtr m;
            Technique t;
            m = CloneMaterial("BumpMaterial", outMatName);
            t = m.GetBestTechnique();
            if (!t.Name.Equals("Simple"))
            {
                pass = t.GetPass("LightingPass");
                pass.GetTextureUnitState("BaseBump").SetTextureName(normalMapName);
            }
            pass = t.GetPass("Texture");
            pass.GetTextureUnitState("TextureMap").SetTextureName(textureMapName);


            return m;
        }
        public static void AlignTextAreaHorzCenter(OverlayElement textArea, Viewport viewport)
        {
            string fontName = textArea.GetParameter("font_name");

            Font font = (Font)(FontManager.Singleton.GetByName(fontName).Target);
            Vector2 dim = ViewHelper.GetTextDimensions(textArea.Caption, font, viewport);

            textArea.SetPosition((1 - dim.x) / 2.0f, textArea.Top);

        }

        public static void AlignTextAreaHorzRight(OverlayElement textArea, Viewport viewport, float rMargin)
        {
            string fontName = textArea.GetParameter("font_name");
            Font font = (Font)(FontManager.Singleton.GetByName(fontName).Target);
            Vector2 dim = ViewHelper.GetTextDimensions(textArea.Caption, font, viewport);

            textArea.SetPosition((1 - dim.x) - rMargin, textArea.Top);

        }

        public static Vector2 GetTextDimensions(String text, Font font, Viewport viewport)
        {

            TexturePtr fontTexture = (TexturePtr)TextureManager.Singleton.GetByName(font.GetMaterial().GetTechnique(0).GetPass(0).GetTextureUnitState(0).TextureName);
	        float charHeight = Mogre.StringConverter.ParseReal(font.GetParameter("size"));


            Box[] GlyphTexCoords = new Box[text.Length];
            FloatRect glypheTexRect;
            uint charheight = 0;
            uint charwidth = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] != '\t') && (text[i] != '\n') && (text[i] != ' '))
                {
                    glypheTexRect = font.GetGlyphTexCoords(text[i]);
                    GlyphTexCoords[i].left = (uint)(glypheTexRect.left * fontTexture.SrcWidth);
                    GlyphTexCoords[i].top = (uint)(glypheTexRect.top * fontTexture.SrcHeight);
                    GlyphTexCoords[i].right = (uint)(glypheTexRect.right * fontTexture.SrcWidth);
                    GlyphTexCoords[i].bottom = (uint)(glypheTexRect.bottom * fontTexture.SrcHeight);

                    if (GlyphTexCoords[i].Height > charheight)
                        charheight = GlyphTexCoords[i].Height;
                    if (GlyphTexCoords[i].Width > charwidth)
                        charwidth = GlyphTexCoords[i].Width;

                }
            }
         //   charwidth = charheight / 3;



	        Vector2 result = new Vector2(0, 0);

	        for(int i = 0; i < text.Length; i++)
	        {
                if (text[i] == 0x0020)
			        result.x += font.GetGlyphAspectRatio(0x0030);
		        else
			        result.x += font.GetGlyphAspectRatio(text[i]);
	        }

            result.x = (result.x * charwidth) / (float)viewport.ActualWidth;
            result.y = charHeight / (float)viewport.ActualHeight;

	        return result;
        }

        /*
        public static Vector3 GetVerticalRayIntersection(SceneManager sceneMgr, SceneNode parent, Vector3 localPosition)
        {
        	Vector3 startPosition = new Vector3(0,10000,0);
        	Vector3 worldPosition = parent._getDerivedPosition() + localPosition;
        
        	
        	worldPosition.y = 0;
        	RaySceneQuery raySceneQuery = sceneMgr.CreateRayQuery(new Ray(worldPosition + startPosition,
			Vector3.NEGATIVE_UNIT_Y));
	
        	RaySceneQueryResult qryResult = raySceneQuery.Execute();
			RaySceneQueryResult.Iterator i = qryResult.Begin();
			while(true)
			{
				i.MoveNext();
				Console.WriteLine("Intersection result" + i.Value);
				if (i!=qryResult.End() && i.Value.worldFragment != null)
				{
					if (i.Value.worldFragment.singleIntersection.y<0 || i.Value.worldFragment.singleIntersection.y>100)
					{					
						continue;
					}
					Console.WriteLine("Found intersection: "+i.Value.worldFragment.singleIntersection);
					worldPosition.y = i.Value.worldFragment.singleIntersection.y;
				}
				else
				{
					break;
				}
				
			}
			Vector3 ret = parent._getDerivedPosition() + localPosition;
			
			ret.y = worldPosition.y;
			
			return ret - parent._getDerivedPosition();
        	
        }*/
    }
}