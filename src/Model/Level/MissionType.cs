namespace Wof.Model.Level
{
    /// <summary>
    /// Rodzaj misji
    /// </summary>
    /// <author>Adam Witczak</author>
    public enum MissionType
    {
        /// <summary>
        /// Zabiæ wszystkich ¿o³nierzy
        /// </summary>
        [StringValue("BombingRun")]
        BombingRun,

        /// <summary>
        /// Zabiæ genera³a
        /// </summary>
        [StringValue("Assassination")]
        Assassination,

        /// <summary>
        /// zniszczyæ wszystkie samoloty
        /// </summary>
        [StringValue("Dogfight")]
        Dogfight,

        /// <summary>
        /// zniszczyæ wszystkie statki
        /// </summary>
        [StringValue("Naval")]
        Naval
    }
}
