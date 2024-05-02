using System.Collections.Generic;

public class SongAnalizer
{
    // Song information
    public float songBpm;
    public float firstBeatOffset;

    // Precomputed timing data
    public List<float> songPositionInBeats;
    public List<float> songPositionInSeconds;

    public SongAnalizer(float songBpm, float firstBeatOffset)
    {
        this.songBpm = songBpm;
        this.firstBeatOffset = firstBeatOffset;
        this.songPositionInBeats = new List<float>();
        this.songPositionInSeconds = new List<float>();
    }

    public void ProcessSong(float songDuration)
    {
        float secPerBeat = 60f / songBpm;
        float currentBeat = 0f;
        float currentSecond = 0f;

        while (currentSecond < songDuration)
        {
            if (currentBeat % 2 == 0)
            {
                songPositionInBeats.Add(currentBeat);
                songPositionInSeconds.Add(currentSecond);
            }
            
            currentBeat += 1f;
            currentSecond += secPerBeat;
        }
    }
}