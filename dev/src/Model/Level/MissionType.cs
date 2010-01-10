namespace Wof.Model.Level
{
    /// <summary>
    /// Rodzaj misji
    /// </summary>
    /// <author>Adam Witczak</author>
    public enum MissionType
    {
        /// <summary>
        /// Zabi� wszystkich �o�nierzy
        /// </summary>
        [StringValue("BombingRun")]
        BombingRun,

        /// <summary>
        /// Zabi� genera�a
        /// </summary>
        [StringValue("Assassination")]
        Assassination,

        /// <summary>
        /// zniszczy� wszystkie samoloty
        /// </summary>
        [StringValue("Dogfight")]
        Dogfight,

        /// <summary>
        /// zniszczy� wszystkie statki
        /// </summary>
        [StringValue("Naval")]
        Naval
    }
}
