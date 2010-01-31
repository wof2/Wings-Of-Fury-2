using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Wof.Controller.AdAction;

namespace Wof.View.Effects
{
    public class AdQuadrangle3D : Quadrangle3D
    {
        private int billboardId;
        private AdManager.Ad ad;
        private bool wasShown = false;

        private SceneNode parent;
        private SceneNode adNode;

        private bool isPersistent = false;
        
        public AdQuadrangle3D(SceneManager sceneMgr, AdManager.Ad ad, bool isPersistent) : base(sceneMgr, "Ad" + ad.id)
        {
            this.ad = ad;
            this.isPersistent = isPersistent;
        }

        private float opacity = 1.0f;


        public bool DecreaseOpacity(float o)
        {
            if(opacity < 0 ) return false;
            opacity -= o;
            if(opacity < 0)
            {
                ManualObject.Visible = false;
                opacity = 0;
                return false;
            }

            TextureUnitState state = ManualObject.GetSection(0).GetMaterial().GetBestTechnique().GetPass(0).GetTextureUnitState(0);
            state.SetAlphaOperation(LayerBlendOperationEx.LBX_MODULATE, LayerBlendSource.LBS_TEXTURE, LayerBlendSource.LBS_MANUAL, 1.0f, opacity);
            return true;
                           
        }

        public AdManager.Ad Ad
        {
            get { return ad; }
        }

        public bool WasShown
        {
            get { return wasShown; }
            set { wasShown = value; }
        }

        public bool IsPersistent
        {
            get { return isPersistent; }
        }

        public void SetSceneNodes(SceneNode parent, SceneNode adNode)
        {
            this.parent = parent;
            this.adNode = adNode;
        }

        public SceneNode GetParent()
        {
            return parent;
        }

        public SceneNode GetAdNode()
        {
            return adNode;
        }

        /// <summary>
        /// Ustawia ID dynamicznego billboardu
        /// </summary>
        /// <param name="billboardId"></param>
        public void SetBillboardId(int billboardId)
        {
            this.billboardId = billboardId;
            
        }

        public int GetBillboardId()
        {
            return billboardId;
        }
    }
}
