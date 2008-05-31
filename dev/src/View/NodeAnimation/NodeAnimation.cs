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

namespace Wof.View.NodeAnimation
{
    /// <summary>
    /// Animacje proceduralne WoF
    /// Cechy:
    /// - animacja Node'ów MOGRE
    /// - szybkoœæ animacji niezale¿na od framerate
    /// - mo¿liwoœæ uruchomienia dalszych metod przy rozpoczêciu i zakoñczeniu animacji
    /// - obs³uga funkcji ci¹g³ych i nieci¹g³ych 
    /// - mo¿liwoœæ uruchomienia dalszych animacji w okreœlonych czasie (nietestowane)
    /// - mo¿liwosæ przyspieszenia / spowolnienia / zatrzymania / przewiniêcia / zapêtlenia animacji
    /// <author>Adam Witczak</author>
    /// </summary>
    public abstract class NodeAnimation
    {
        // zmienne pomoznicze
        protected float delta; // ruch w danej klatce

        protected float percent; // bie¿¹cy proces realizacji animacji [0-1]
        protected float percentAfter; // procent realizacji animacji po zastosowaniu klatki animacji [0-1]

        protected float lastAmplitude = 0;
        protected float amplitudeAtEnd;
        protected float amplitudeAtStart;

        protected bool startNewLoop = false;

        public delegate void NotityFinish(object info);

        public delegate void NotityStart(object info);

        public NotityFinish onFinish;
        public NotityStart onStart;

        public object onFinishInfo = null;
        public object onStartInfo = null;


        public NodeAnimation nextAnimation = null;
        public float nextAnimationPercentTime = 0.0f; // czas kiedy ma byc odpalone nextAnimation
        public bool repeatNextAnimation = false;
        protected bool nextAnimationStarted = false;

        protected Radian cycleLength;

        public Radian CycleLength
        {
            get { return cycleLength; }
        }

        protected SceneNode node;

        public SceneNode Node
        {
            get { return node; }
            set { node = value; }
        }

        protected string name; // identyfikator animacji!

        public string Name
        {
            get { return name; }
        }

        protected float duration; // w sekundach

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        protected float timeScale = 1.0f; // przyspieszacz-spowalniacz

        public float TimeScale
        {
            get { return timeScale; }
            set { timeScale = value; }
        }

        protected float animationTime = 0; // bierz¹cy czas animacji w sek

        public float AnimationTime
        {
            get { return animationTime; }
        }

        protected float timeSinceLastFrame = -1; // czas od ostatniej klatki w sek. -1 zeby odro¿niæ pocz¹tek

        public float TimeSinceLastFrame
        {
            get { return timeSinceLastFrame; }
        }


        protected bool enabled = false;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }


        protected bool looped = true;

        public bool Looped
        {
            get { return looped; }
            set { looped = value; }
        }


        public bool Ended
        {
            get { return (getPercent() >= 1.0f); }
        }

        public float getPercent()
        {
            return animationTime/duration;
        }


        public abstract void animate();


        public NodeAnimation(SceneNode node, float animationDuration, string name, Radian cycleLength)
        {
            if (animationDuration <= 0) animationDuration = 1;
            this.node = node;
            duration = animationDuration;
            this.name = name;
            this.cycleLength = cycleLength;

            amplitudeAtEnd = animationFunction((float) cycleLength.ValueRadians);
            amplitudeAtStart = animationFunction(0);
        }

        public void updateTime(float timeSinceLastFrame)
        {
            if (enabled)
            {
                if (this.timeSinceLastFrame == -1 && onStart != null)
                {
                    //Console.WriteLine("Levelview: "+onStart.ToString());
                    onStart(onStartInfo);
                }
                this.timeSinceLastFrame = timeSinceLastFrame;
            }
        }

        public void rewindToRandom()
        {
            animationTime = Math.RangeRandom(0, duration);
            delta = 0;
            percent = getPercent();
            percentAfter = 0;
            lastAmplitude = 0;
            timeSinceLastFrame = -1;
        }

        public void rewind()
        {
            animationTime = 0;
            delta = 0;
            percent = 0;
            percentAfter = 0;
            lastAmplitude = 0;
            timeSinceLastFrame = -1;
        }

        public void rewind(bool enabled)
        {
            rewind();
            this.enabled = enabled;
        }

        protected bool startNextAnimation()
        {
            if (nextAnimation != null)
            {
                if (getPercent() >= nextAnimationPercentTime)
                {
                    if (repeatNextAnimation && !nextAnimationStarted
                        ||
                        !nextAnimationStarted)
                    {
                        nextAnimation.Enabled = true;
                        nextAnimationStarted = true;
                        return true;
                    }
                }
            }
            return false;
        }

        protected abstract float animationFunction(float x);

        public void Destroy()
        {
            node.Creator.DestroySceneNode(node.Name);
            node = null;
        }

        protected void frameInit()
        {
            delta = timeSinceLastFrame*TimeScale;
            percent = getPercent();
            animationTime += delta;
            percentAfter = getPercent();

            if (percentAfter >= 1.0f)
            {
                if (onFinish != null)
                {
                    onFinish(onFinishInfo);
                }
                if (looped)
                {
                    // Zacznij od pocz¹tku. 
                    // trzeba wyzerowaæ licznik i zacz¹æ od pocz¹tku, ¿eby nie przekrêciæ licznika
                    // potrzebne takze w przypadku funkcji nieokresowych (lub niecyklicznych d³ugoœci cykli)
                    animationTime = Duration*(percentAfter%1);
                    percentAfter = percentAfter%1;

                    startNewLoop = true;

                    if (repeatNextAnimation) nextAnimationStarted = false; // aby mozna byl ponownie uruchomic
                }
                else
                {
                    // Koniec animacji. Uló¿ w ostatecznej pozycji                 
                    enabled = false;

                    percentAfter = 1.0f; // ulozenie ostateczne                        
                }
            }
            else
            {
                startNewLoop = false;
            }

            startNextAnimation();
        }
    }
}