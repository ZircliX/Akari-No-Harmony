using System.Collections.Generic;
using UnityEngine;

namespace AudioDelegates
{
    [ExecuteInEditMode]
    public class NewMapGenerator : MonoBehaviour
    {
        public bool create;
        public GameObject[] circlesType;

        [Header("Song Data")]
        public new AudioClip audio;
        public int bpm;
        public float offset;
        public string songName;
        private Song songData;
        private OfflineFFT fft;
    
        [Space]
        [Header("Map Data")]
        public string mapName;
        public int typeChangeProba;
        private int currentTypeIndex;
        private Map mapData;
        
        private List<float[]> spectra; // Your list of spectrum data
        
        // Start is called before the first frame update
        void Update()
        {
            if (create)
            {
                create = false;

                CreateSongData();
                AnalyzeSpectra();
            
                CreateMap();
                SaveMap();
            }
        }

        private void CreateMap()
        {
            mapData = new Map
            {
                mapName = mapName,
                songData = songData
            };

            foreach (var time in songData.songPositionInSeconds)
            {
                if (Random.Range(0, typeChangeProba) == 0) ChangeCircleType();
            
                var newCircle = new Circle
                {
                    typeIndex = currentTypeIndex,
                    downSpeed = 4f,
                    timeToSpawn = time - 2f,
                    timeToBeat = time,
                    columnIndex = Random.Range(0, 3)
                };
            
                mapData.circles.Add(newCircle);
            }
        }

        private void ChangeCircleType()
        {
            currentTypeIndex = Random.Range(0, circlesType.Length);
        }

        private void CreateSongData()
        {
            songData = new Song
            {
                songAudio = audio,
                songBPM = bpm,
                songOffset = offset,
                songName = songName
            };

            spectra = new OfflineFFT(songData.songAudio, 1024).SpectrumBuffers;
        }

        private void AnalyzeSpectra()
        {
            foreach (float[] spectrum in spectra)
            {
                List<(int peakIndex, float peakTime)> peaksWithTime = FindPeaksWithTime(spectrum, 0.1f);

                foreach (var peakInfo in peaksWithTime)
                {
                    int peakIndex = peakInfo.peakIndex;
                    float peakTime = peakInfo.peakTime;

                    Debug.Log($"Peak found at index {peakIndex} and time {peakTime:F3} seconds");
                    songData.songPositionInSeconds.Add(peakTime);
                }
            }
        }

        private List<(int peakIndex, float peakTime)> FindPeaksWithTime(float[] spectrum, float threshold)
        {
            List<(int peakIndex, float peakTime)> peaksWithTime = new List<(int peakIndex, float peakTime)>();
            float samplingRate = AudioSettings.outputSampleRate;
            int fftSize = spectrum.Length;
            float frequencyResolution = samplingRate / fftSize;

            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                if (spectrum[i] > spectrum[i - 1] && spectrum[i] > spectrum[i + 1] && spectrum[i] > threshold)
                {
                    float peakTime = (i * frequencyResolution) / samplingRate;
                    peaksWithTime.Add((i, peakTime));
                }
            }
            return peaksWithTime;
        }
        
        private void SaveMap()
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
        }
    }
}