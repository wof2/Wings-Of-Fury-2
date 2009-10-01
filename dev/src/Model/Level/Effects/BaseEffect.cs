/*
 * WiiMote - Zastosowanie zaawansowanych kontrolerów gier do stworzenia naturalnych
interfejsów użytkownika.
*/
using System;

namespace Wof.Model.Level.Effects
{
	/// <summary>
	/// Description of BaseEffect.
	/// </summary>
	public abstract class BaseEffect
	{
		public BaseEffect()
		{
		}
		
		/// <summary>
        /// Metoda aktualizuje efekt.
        /// </summary>
        /// <param name="time">Liczba milisekund, ktora uplynela od ostatniej aktualizacji.</param>
        public abstract void Update(int time);
	}
}
