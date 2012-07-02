/*
 * Copyright 2008 Adam Witczak, Jakub T�ycki, Kamil S�awi�ski, Tomasz Bilski, Emil Hornung, Micha� Ziober
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
using System.Collections.Generic;
using System.Text;
using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;

namespace Wof.Model.Level.LevelTiles.Watercraft
{
    public abstract class ShipBunkerTile : BunkerTile, ISinkComponent
    {

        protected SinkComponent sinkComponent;
        protected ShipBunkerTile(float yBegin, float yEnd, float viewXShift, Quadrangle hitBound, int soldierNum, int generalNum, int type, List<Quadrangle> collisionRectangle)
            : base(yBegin, yEnd, viewXShift, hitBound, soldierNum, generalNum, type, collisionRectangle)
        {
            sinkComponent = new SinkComponent(this, this);
        }
        
        public override void Fire(int time)
        {
        	if(!IsEmerged)
        	{ 
        		// pod woda
        		return;
        	}
            base.Fire(time);
        }

        /// <summary>
        /// Przesuniecie dzia�ka w widoku od poczatku tile'a bunkra
        /// </summary>
        protected override float GetGunXShift()
        {
            return viewXShift;
        }

        public float DoSubmerge(float time, float timeUnit)
        {
            float amount = sinkComponent.DoSubmerge(time, timeUnit);
            return amount;
        }

        public float DoEmerge(float time, float timeUnit)
        {
            float amount = sinkComponent.DoEmerge(time, timeUnit);
            return amount;
        }

        public float DoSinking(float time, float timeUnit)
        {
            float amount = sinkComponent.DoSinking(time, timeUnit);
            if (amount > 0)
            {

                List<Soldier> soldiers = refToLevel.SoldiersList.FindAll(Predicates.FindSoldierFromStartingIndex(TileIndex));

                foreach (Soldier s in soldiers)
                {
                    s.YPosition -= amount;
                    //   Console.WriteLine("model Y: " + s.Position.Y);
                }
                return amount;
            }
            return 0;

        }


        public override PointD Center
        {
            get
            {
                return Bounds.Center + new PointD(viewXShift, this.hitBound.HighestY);

            }
        }

        #region Implementation of ISinkComponent

        public float Depth
        {
            get { return sinkComponent.Depth; }
        }

        public bool IsSinking
        {
            get { return sinkComponent.IsSinking; }
        }

        public bool IsSunkDown
        {
            get { return sinkComponent.IsSunkDown; }
        }
      

        public bool IsSubmerged
        {
            get { return sinkComponent.IsSubmerged; }
        }

        public bool IsSubmerging
        {
            get { return sinkComponent.IsSubmerging; }
        }

        public bool IsEmerging
        {
            get { return sinkComponent.IsEmerging; }
        }

        public void StartSinking()
        {
            sinkComponent.StartSinking();
        }

        public void StopSinking()
        {
            sinkComponent.StopSinking();
        }

       

        public void StartSubmerging()
        {
            sinkComponent.StartSubmerging();
        }

        public void StopSubmerging()
        {
            sinkComponent.StopSubmerging();
        }

      

        public void StartEmerging()
        {
            sinkComponent.StartEmerging();
        }

        public void StopEmerging()
        {
            sinkComponent.StopEmerging();
        }
        
        public bool IsEmerged {
			get {
				return sinkComponent.IsEmerged;
			}
		}
    	public float SubmergeTimeElapsed {
			get {
				return sinkComponent.SubmergeTimeElapsed;
			}
		}
    	
		public float SinkingTimeElapsed {
			get {
				return sinkComponent.SinkingTimeElapsed;
			}
        }

        public LevelTile Tile
        {
            get
            {
                return sinkComponent.Tile;
            }
        }
	

        #endregion
    	
		
    	
		
    }
}
