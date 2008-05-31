using System;

namespace Wof.Languages
{
    public static class FontManager
    {
        public static class AvailableFonts
        {
            /// <summary>
            /// Font do wyswietlenia domyslnych(angielskich) znakow.
            /// </summary>
            public const string FontMaterialDefault = @"Wof_en-GB";

            /// <summary>
            /// Zwraca format jezyka.
            /// </summary>
            public const string FontFormat = @"Wof_{0}";
        }

        /// <summary>
        /// Font, ktory jest aktualnie uzyty.
        /// </summary>
        public static string CurrentFont = AvailableFonts.FontMaterialDefault;

        public static void SetCurrentFont(string cultureType)
        {
            CurrentFont = String.Format(AvailableFonts.FontFormat, cultureType);
        }
    }
}