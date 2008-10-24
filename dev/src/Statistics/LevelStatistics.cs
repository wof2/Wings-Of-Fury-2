/*
 * Copyright 2008 Adam Witczak, Jakub Tężycki, Kamil Sławiński, Tomasz Bilski, Emil Hornung, Michał Ziober
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
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace Wof.Statistics
{
    /// <summary>
    /// Statystyki dla danego poziomu.
    /// </summary>
    public class LevelStatistics
    {
        #region Private Fields



        

        /// <summary>
        /// Ilość zestrzelonych samolotów.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mPlanesShotDown;


        /// <summary>
        /// Liczba wystrzelonych rakiet.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mRocketCount;

        /// <summary>
        /// Liczba zrzuconych bomb.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mBombCount;

        /// <summary>
        /// Liczba wystrzelonych pociskow.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mGunCount;

        /// <summary>
        /// Liczba celnych trafien: zniszczony bunkier lub 
        /// zabity zolnierz.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mHitByRocket;
 
        /// <summary>
        /// Liczba trafien bombami: zniszczony bunkier lub 
        /// zabity zolnierz.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mHitByBomb;

        /// <summary>
        /// Liczba trafien dzialkiem: zabici zolnierze
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int mHitByGun;

        #endregion

        #region Public Constructor

        public LevelStatistics()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Pobiera lub ustawia liczbe uzytych rakiet.
        /// </summary>
        public int RocketCount
        {
            get { return this.mRocketCount; }
            set { this.mRocketCount = value; }
        }


         /// <summary>
        /// Pobiera lub ustawia liczbe zestrzelonych / rozbitych samolotów.
        /// </summary>
        public int PlanesShotDown
        {
            get { return this.mPlanesShotDown; }
            set { this.mPlanesShotDown = value; }
        }


        
        /// <summary>
        /// Pobiera lub ustawia liczbe uzytych bomb.
        /// </summary>
        public int BombCount
        {
            get { return this.mBombCount; }
            set { this.mBombCount = value; }
        }

        /// <summary>
        /// Pobiera lub ustawia liczbe wystrzelonych pociskow.
        /// </summary>
        public int GunCount
        {
            get { return this.mGunCount; }
            set { this.mGunCount = value; }
        }

        /// <summary>
        /// Pobiera lub ustawia liczbe trafien przez rakiete.
        /// </summary>
        /// <remarks>Jest to liczba zniszczonych instalacji obronnych plus liczba zabitych zolnierzy.</remarks>
        public int HitByRocket
        {
            get { return this.mHitByRocket; }
            set { this.mHitByRocket = value; }
        }

        /// <summary>
        /// Pobiera lub ustawia liczbe trafien przez bombe.
        /// </summary>
        /// <remarks>Jest to liczba zniszczonych instalacji obronnych plus liczba zabitych zolnierzy.</remarks>
        public int HitByBomb
        {
            get { return this.mHitByBomb; }
            set { this.mHitByBomb = value; }
        }

        /// <summary>
        /// Pobiera lub ustawia liczbe trafien przez dzialko.
        /// </summary>
        /// <remarks>Jest to liczba zabitych zolnierzy.</remarks>
        public int HitByGun
        {
            get { return this.mHitByGun; }
            set { this.mHitByGun = value; }
        }

        /// <summary>
        /// Pobiera procentowa skutecznosc rakiet.
        /// </summary>
        public int RocketStats
        {
            get
            {
                if (this.mRocketCount == 0)
                    return 0;
                return Convert.ToInt32(Math.Ceiling(((double)this.mHitByRocket / (double)this.mRocketCount) * 100));
            }
        }

        /// <summary>
        /// Pobiera procentowa skutecznosc bomb.
        /// </summary>
        public int BombStats
        {
            get 
            {
                if (this.mBombCount == 0)
                    return 0;
                return  Convert.ToInt32(Math.Ceiling(((double)this.mHitByBomb / (double)this.mBombCount) * 100));
            }
        }

        /// <summary>
        /// Pobiera procentowa skutecznosc dzialka.
        /// </summary>
        public int GunStats
        {
            get
            {
                if (this.mGunCount == 0)
                    return 0;
                return Convert.ToInt32(Math.Ceiling(((double)this.mHitByGun / (double)this.mGunCount) * 100));
            }
        }

        #endregion

        #region Override methods

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Skuteczność procentowa:");
            builder.AppendLine("Rakiety: " + RocketStats + "%");
            builder.AppendLine("Bomby: " + BombStats + "%");
            builder.AppendLine("Działko: " + GunStats + "%");
            return builder.ToString();
        }

        #endregion
    }
}
