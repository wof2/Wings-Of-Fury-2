/*
 * Created by SharpDevelop.
 * User: awitczak
 * Date: 2012-10-29
 * Time: 13:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Wof.Model.Level.Planes
{
	/// <summary>
	/// Description of EnemyBomber.
	/// </summary>
	public class EnemyBomber : EnemyPlaneBase
	{
		#region Constants

        #endregion

        #region Fields

        //komentarz do wywalenia

        #endregion

        #region Constructors

        public EnemyBomber(Level level, float width, float height)
            : base(level, width, height, Planes.PlaneType.Betty)
        {
        }

        /// <summary>
        /// Tworzy samolot z wylosawnym położeniem (któryś z krańców planszy).
        /// </summary>
        /// <param name="level"></param>
        public EnemyBomber(Level level)
            : base(level, Planes.PlaneType.Betty)
        {
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

       
        #endregion

        #region Properties

        #endregion
	}
}
