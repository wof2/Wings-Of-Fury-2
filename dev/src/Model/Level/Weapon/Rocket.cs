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
using System.Collections.Generic;
using System.Diagnostics;

using Wof.Model.Configuration;
using Wof.Model.Level.Common;
using Wof.Model.Level.Infantry;
using Wof.Model.Level.LevelTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.EnemyInstallationTiles;
using Wof.Model.Level.LevelTiles.IslandTiles.ExplosiveObjects;
using Wof.Model.Level.LevelTiles.Watercraft;
using Wof.Model.Level.Planes;
using Math = Mogre.Math;
using Plane = Wof.Model.Level.Planes.Plane;

namespace Wof.Model.Level.Weapon
{
    /// <summary>
    /// Klasa implementujaca zachownie rakiet na planszy.
    /// </summary>
    /// <author>Michal Ziober</author>
    public class Rocket : MissileBase
    {
        #region Fields




        #endregion

        #region Public Constructor

        /// <summary>
        /// Konstruktor szescioparametrowy. Tworzy 
        /// nowa rakiete na planszy.
        /// </summary>
        /// <param name="x">Wspolrzedna x.</param>
        /// <param name="y">Wspolrzedna y.</param>
        /// <param name="initialVelocity">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <param name="angle">Kat nachylenia.</param>
        /// <param name="owner">Wlasciciel amunicji.</param>
        /// <author>Michal Ziober</author>
        public Rocket(float x, float y, PointD initialVelocity, Level level, float angle, IObject2D owner)
            : base(x,y,initialVelocity, level, angle, owner)
        {
           
            
           
           
        }

        /// <summary>
        /// Konstruktor piecioparametrowy.Tworzy
        /// nowa rakiete na planszy.
        /// </summary>
        /// <param name="position">Pozycja rakiety.</param>
        /// <param name="initialVelocity">Wektor ruchu.</param>
        /// <param name="level">Referencja do obiektu planszy.</param>
        /// <param name="angle">Kat nachylenia.</param>
        /// <param name="owner">Wlasciciel rakiety.</param>
        /// <author>Michal Ziober</author>
        public Rocket(PointD position, PointD initialVelocity, Level level, float angle, IObject2D owner)
            : this(position.X, position.Y, initialVelocity, level, angle, owner)
        {
        }

        

        #endregion

        #region IMove Members

        /// <summary>
        /// Zmienia pozycje rakiety w zaleznosci od czasu oraz
        /// sprawdza kolizje z obiektami na planszy.
        /// </summary>
        /// <param name="time">Czas od ostatniej zmieny</param>
        /// <author>Michal Ziober</author>
        public override void Move(int time)
        {
            base.Move(time);
            //zmienia pozycje.            
        }

      

        /// <summary> 
        /// Zmienia pozycje rakiety.
        /// </summary>
        /// <param name="time">Czas od ostatniego przesuniecia.</param>
        /// <author>Michal Ziober</author>
        protected override void ChangePosition(int time)
        {
        	base.ChangePosition(time);
        }

        /// <summary> 
        /// Sprawdza kolizje z wrogimi samolotami 
        /// oraz obsluguje zderzania z nimi.
        /// </summary>
        /// <author>Michal Ziober</author>
        protected override  void CheckCollisionWithEnemyPlanes()
        {
        	base.CheckCollisionWithEnemyPlanes();
        }
        
        protected override  void CheckCollisionWithUserPlane()
        {
        	Plane p = refToLevel.UserPlane;
            if (p != null)
            {
                if (boundRectangle.Intersects(p.Bounds))
                {
                   
                   
                    if(Owner is Soldier)
                    {
                        p.Hit(p.MaxOil * 0.1f, 0); // trafienie przez bazooke
                    }else
                    if (Owner is ShipConcreteBunkerTile)
                    {
                        p.Hit(p.MaxOil * 0.1f, 0); // trafienie przez bazooke
                    }
                    else
                    {
                        p.Hit(p.MaxOil * 0.5f, 0); 
                      //  if(p.Oil <= 0)
                      //  {
                            // trafienie przez rakiete samolotowa
                           // refToLevel.Controller.OnPlaneDestroyed(p); //odrejestruje samolot gracza.
                           // p.Destroy();
                      //  }
                      
                    }

                    refToLevel.Controller.OnRocketHitPlane(this, p);
                    //odrejestruje rakiete.
                    refToLevel.Controller.OnUnregisterAmmunition(this);

                    //niszcze rakiete.
                    state = MissileState.Destroyed;
                }
            }
        }

 		protected override  void CheckCollisionWithGround()
        {
        	int index = Mathematics.PositionToIndex(Position.X);
            LevelTile tile;
            if (index > -1 && index < refToLevel.LevelTiles.Count)
            {
            	tile = refToLevel.LevelTiles[index];
            	if (tile is ShipTile)
            	{
            		//boundRectangle.Intersects(
            	}
            	//CollisionType c = CollisionType.None;
            	CollisionType c = tile.InCollision(this.boundRectangle);
            	if (c == CollisionType.None) return;
            	    
                //jesli nie da sie zniszczyc dany obiekt rakieta.
                if(c == CollisionType.Hitbound || c == CollisionType.CollisionRectagle)
                {
                    if (tile is BarrelTile)
	                {
	                    BarrelTile destroyTile = tile as BarrelTile;
	                    if (!destroyTile.IsDestroyed)
	                    {
	                        destroyTile.Destroy();
	                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                            refToLevel.Statistics.HitByRocket += refToLevel.KillVulnerableSoldiers(index, 2, true);
	                    }
                        else
                            refToLevel.Controller.OnTileBombed(tile, this);
	                }
	                else if (tile is EnemyInstallationTile)
	                {
                        FortressBunkerTile fortressTile = null;
	                    EnemyInstallationTile enemyTile = null;
	                    LevelTile destroyTile = tile;
                            //Obsluga fortress bunker
                            if ((fortressTile = destroyTile as FortressBunkerTile) != null && !fortressTile.IsDestroyed)
                            {
                                //Trafienie zniszczonego fortress bunker
                                if (fortressTile.IsDestroyed)
                                {
                                    refToLevel.Controller.OnTileBombed(destroyTile, this);
                                }
                                else
                                {
                                    fortressTile.Hit();
                                    //Ostatnie trafienie!
                                    if (fortressTile.ShouldBeDestroyed)
                                    {
                                        refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                                        refToLevel.Statistics.HitByRocket++;
                                        fortressTile.Destroy();
                                    }
                                    //Trafienie rakiety uszkadzaj¹ce fortress bunker
                                    else
                                    {
                                    	refToLevel.Controller.OnFortressHit(fortressTile, this);
                                        refToLevel.Statistics.HitByRocket++;
                                    }
                                }
                            }
                            else if ((enemyTile = destroyTile as EnemyInstallationTile) != null && !enemyTile.IsDestroyed)
                            {
                                refToLevel.Controller.OnTileDestroyed(destroyTile, this);
                                refToLevel.Statistics.HitByRocket++;
                                enemyTile.Destroy();
                            }
                            else
                                refToLevel.Controller.OnTileBombed(tile, this);
	                }
                    else
                        refToLevel.Controller.OnTileBombed(tile, this);                	
                } 
                else if(c == CollisionType.Altitude) 
                {
                	refToLevel.Controller.OnTileBombed(tile, this);
                }
              

                //zabija zolnierzy, ktorzy sa w zasiegu.
                refToLevel.Statistics.HitByRocket += refToLevel.KillVulnerableSoldiers(index, 1, true);

                //niszcze rakiete
                state = MissileState.Destroyed;
            }
        }


     
        

        
        #endregion

        #region Static Method

       
     
        #endregion

     
    }
}