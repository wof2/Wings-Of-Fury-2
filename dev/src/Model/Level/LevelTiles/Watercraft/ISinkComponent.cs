namespace Wof.Model.Level.LevelTiles.Watercraft
{
    public interface ISinkComponent
    {
        /// <summary>
        /// Zwraca informacje o zanurzeniu statku.
        /// </summary>
        float Depth { get; }

        bool IsSinking { get; }
        bool IsSunkDown { get; }
        float SinkingTime { get; }
        float SubmergeTime { get; }
        bool IsSubmerged { get; }
        bool IsSubmerging { get; }
        bool IsEubmerging { get; }

        void StartSinking();
        void StopSinking();

        /// <summary>
        /// Toni�cie tile'a. Zwraca o ile tile zaton��, lub 0 w przypadku zako�czenia toni�cia
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnit"></param>
        float DoSinking(float time, float timeUnit);

        void StartSubmerging();
        void StopSubmerging();
        float DoSubmerge(float time, float timeUnit);
        float DoEmerge(float time, float timeUnit);
        void StartEmerging();
        void StopEmerging();
    }
}