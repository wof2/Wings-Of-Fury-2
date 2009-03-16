/*
 * Created by SharpDevelop.
 * User: Adam
 * Date: 2009-03-16
 * Time: 12:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Wof.Model.Level.Common;

namespace Wof.Model.Level
{
	/// <summary>
	/// Description of IAttractorTarget.
	/// </summary>
	public interface IAttractorTarget
	{
		void AddAttractor(IAttractorSource source, string name);
		
		void ClearAttractors();
		
		void RemoveAttaractor(string name);
		
		PointD GetAttractorsMeanForce();
		
		
	}
}
