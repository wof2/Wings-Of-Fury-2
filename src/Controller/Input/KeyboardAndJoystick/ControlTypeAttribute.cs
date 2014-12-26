/*
 * Created by SharpDevelop.
 * User: w
 * Date: 2014-12-25
 * Time: 17:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Wof.Controller.Input.KeyboardAndJoystick
{
	public enum TypeOfControl { Joystick, Keyboard };
	
	
	/// <summary>
	/// Description of ControlTypeAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false,
 Inherited = true)]
	public class ControlTypeAttribute : Attribute
	{
		protected TypeOfControl type;

		public TypeOfControl Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}
		public ControlTypeAttribute(TypeOfControl type)
		{
			this.type = type;
		}
	}
}




