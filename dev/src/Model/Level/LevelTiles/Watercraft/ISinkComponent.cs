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
        /// Toniêcie tile'a. Zwraca o ile tile zaton¹³, lub 0 w przypadku zakoñczenia toniêcia
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