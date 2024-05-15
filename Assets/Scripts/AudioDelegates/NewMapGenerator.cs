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
        
        private List<System.Numerics.Complex[]> spectrum; // Your list of spectrum data
        
        // Start is called before the first frame update
        void Update()
        {
            if (create)
            {
                create = false;

                CreateSongData();
                AnalyseSound();
                ProcessPeaks();
            
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
            
            spectrum = new OfflineFFT(songData.songAudio, 1024).SpectrumBuffers;
        }
        
        // Sample rate of the audio data
        int sampleRate = 44100; // Assuming a sample rate of 44.1 kHz

        // Buffer size (number of samples per frame)
        int bufferSize = 1024;

        // Threshold for peak detection (adjust this value to control the number of peaks)
        double magnitudeThreshold = 0.001; // Adjust this value based on your requirements

        // Lists to store the peak values, frequencies, and times
        List<double> peakMagnitudes = new List<double>();
        List<double> peakFrequencies = new List<double>();
        List<float> peakTimes = new List<float>();

        private void AnalyseSound()
        {
            // Iterate through each spectrum buffer
            for (int i = 0; i < spectrum.Count; i++)
            {
                System.Numerics.Complex[] spectrumBuffer = spectrum[i];

                // Compute the magnitude spectrum
                double[] magnitude = FftSharp.FFT.Magnitude(spectrumBuffer);

                // Compute the frequency scale
                double[] frequencies = FftSharp.FFT.FrequencyScale(magnitude.Length, sampleRate);

                // Iterate through the magnitude spectrum
                for (int j = 0; j < magnitude.Length; j++)
                {
                    // Check if the magnitude is above the threshold
                    if (magnitude[j] >= magnitudeThreshold)
                    {
                        // Calculate the time for the current bin
                        float time = (i * bufferSize + j) / (float)sampleRate;

                        // Add the peak magnitude, frequency, and time to the respective lists
                        peakMagnitudes.Add(magnitude[j]);
                        peakFrequencies.Add(frequencies[j]);
                        peakTimes.Add(time);
                    }
                }
            }
        }

        private void ProcessPeaks()
        {
            foreach (var time in peakTimes)
            {
                songData.songPositionInSeconds.Add(time);
                Debug.Log(time);
            }
        }

        private void SaveMap()
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
        }
    }
}