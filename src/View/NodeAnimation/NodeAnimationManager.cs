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

namespace Wof.View.NodeAnimation
{
    /// <summary>
    /// Manager do zarz¹dzania animacjami
    /// <author>Adam Witczak</author>
    /// </summary>
    public abstract class NodeAnimationManager : IEnumerable
    {
        protected Hashtable animations;

        protected NodeAnimation currentAnimation;

        public NodeAnimation CurrentAnimation
        {
            get { return currentAnimation; }
        }

        public NodeAnimation this[string animationName]
        {
            get
            {
                if (animationName == null) return null;
                if (!animations.ContainsKey(animationName))
                {
                    return null;
                }
                return (animations[animationName] as NodeAnimation);
            }
            set { animations[animationName] = value; }
        }

        public NodeAnimation this[Enum enumValue]
        {
            get { return this[getNameByType(enumValue)]; }
            set
            {
                string name = Enum.GetName(enumValue.GetType(), enumValue);
                animations[name] = value;
            }
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return animations.Values.GetEnumerator();
        }

        #endregion

        protected string currentName;


        protected float currentTime = 0;

        public float CurrentTime
        {
            get { return currentTime; }
        }

        protected bool Ended
        {
            get { return this[currentName].Ended; }
        }

        public bool Looped
        {
            get { return this[currentName].Looped; }
            set { this[currentName].Looped = value; }
        }

        public float Percent
        {
            get { return this[currentName].AnimationTime/this[currentName].Duration; }
        }


        public bool Enabled
        {
            get { return this[currentName].Enabled; }
            set { this[currentName].Enabled = value; }
        }

        public float TimeScale
        {
            get { return this[currentName].TimeScale; }
            set { this[currentName].TimeScale = value; }
        }


        public NodeAnimationManager(float currentTime)
        {
            this.currentTime = currentTime;
            animations = new Hashtable();
        }


        protected virtual string getNameByType(Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }

        public bool isCurrentAnimation(Enum animationType)
        {
            return (currentAnimation == this[animationType]);
        }


        protected void register(NodeAnimation animation)
        {
            animations[animation.Name] = animation;
            if (animations.Count == 0) currentName = animation.Name;
        }


        protected void unregister(String animationName)
        {
            if (isRegistered(animationName))
            {
                animations.Remove(animationName);
                // ud³¹cz bie¿¹c¹ animacjê 
                if (currentName == animationName)
                {
                    if (animations.Count > 0)
                    {
                        // prze³¹cz na pierwsz¹ wartoœæ
                        IDictionaryEnumerator e = animations.GetEnumerator();
                        e.Reset();
                        currentName = (e.Value as NodeAnimation).Name;
                    }
                    else currentName = null;
                }
            }
        }

        protected void unregister(NodeAnimation animation)
        {
            unregister(this[animation.Name]);
        }

        public void unregister(Enum animationType)
        {
            string animationName = getNameByType(animationType);
            unregister(this[animationName]);
        }


        public bool isRegistered(String animationName)
        {
            return (this[animationName] != null);
        }

        public bool isRegistered(NodeAnimation animation)
        {
            return isRegistered(animation.Name);
        }


        public bool isRegistered(Enum animationType)
        {
            string animationName = getNameByType(animationType);
            return isRegistered(animationName);
        }

        public bool switchTo(string animationName)
        {
            if (this[animationName] != null)
            {
                currentName = animationName;
                currentAnimation = this[currentName];
                currentAnimation.TimeScale = 1.0f;
                return true;
            }
            else return false;
        }

        public bool switchTo(Enum enumValue)
        {
            return switchTo(getNameByType(enumValue));
        }

        public bool updateTime(float timeSinceLastFrame, string animationName)
        {
            if (!isRegistered(animationName)) return false;
            this[animationName].updateTime(timeSinceLastFrame);
            return true;
        }

        public bool updateTime(float timeSinceLastFrame, Enum animationType)
        {
            return updateTime(timeSinceLastFrame, getNameByType(animationType));
        }

        public void updateTimeAll(float timeSinceLastFrame)
        {
            foreach (NodeAnimation a in this) a.updateTime(timeSinceLastFrame);
        }

        public void animate()
        {
            animate(currentName);
        }

        private bool animate(string animationName)
        {
            if (!isRegistered(animationName)) return false;
            this[animationName].animate();
            return true;
        }

        public void animateAll()
        {
            foreach (NodeAnimation a in this) a.animate();
        }

        public void rewind()
        {
            this[currentName].rewind();
        }

        public void rewind(bool enabled)
        {
            this[currentName].rewind(enabled);
        }

        public void rewindAll()
        {
            foreach (NodeAnimation a in this) a.rewind();
        }

        public void rewindAll(bool enabled)
        {
            foreach (NodeAnimation a in this) a.rewind(enabled);
        }

        public void enableAll()
        {
            foreach (NodeAnimation a in this) a.Enabled = true;
        }

        public void disableAll()
        {
            foreach (NodeAnimation a in this) a.Enabled = false;
        }
    }
}