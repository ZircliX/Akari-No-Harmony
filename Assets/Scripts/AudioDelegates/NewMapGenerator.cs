using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        private List<float[]> spectrumBuffers;
        const int sampleRate = 44100; // Assuming a sample rate of 44.1 kHz
        const int bufferSize = 1024; // Assuming a buffer size of 1024 samples

        public int windowSize;
        public float localDeviationMultiplier;
        
        // Start is called before the first frame update
        void Update()
        {
            if (create)
            {
                create = false;

                CreateSongData();
                
                AnalyzePeakTiming();
            
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
            
            spectrumBuffers = new OfflineFFT(songData.songAudio, 1024).SpectrumBuffers;
        }
        
        private void AnalyzePeakTiming()
        {
            var beatTimes = new List<float>();
            
            // Iterate through the spectrum buffers
            for (int i = 0; i < spectrumBuffers.Count; i++)
            {
                float[] spectrumBuffer = spectrumBuffers[i];

                double bufferStartTime = (i * bufferSize) / (double)sampleRate;

                // Analyze the spectrum buffer to detect peaks
                beatTimes.Clear();
                beatTimes = DetectPeaks(spectrumBuffer);
                
                // Associate the peaks with the corresponding beat number
                foreach (float peak in beatTimes)
                {
                    // Process the peak and its timing (beatNumber)

                    int peakIndex = Array.IndexOf(spectrumBuffer, peak);
                    double peakTime = bufferStartTime + (peakIndex / (double)sampleRate);
                    
                    //Debug.Log($"Peak detected at time {peakTime} seconds with value {peak}");
                    
                    songData.songPositionInSeconds.Add(peakTime);
                }
            }
            
            Debug.Log(songData.songPositionInSeconds.Count);
        }
        
        private List<float> DetectPeaks(float[] spectrumBuffer)
        {
            var peaks = new List<float>();
            float[] smoothed = new float[spectrumBuffer.Length];
            float[] localMean = new float[spectrumBuffer.Length];
            float[] localStd = new float[spectrumBuffer.Length];

            // Calculate smoothed signal
            for (int i = 0; i < spectrumBuffer.Length; i++)
            {
                int start = Mathf.Max(0, i - windowSize);
                int end = Mathf.Min(spectrumBuffer.Length - 1, i + windowSize);
                int count = end - start + 1;
                float sum = 0;
                for (int j = start; j <= end; j++)
                {
                    sum += spectrumBuffer[j];
                }
                smoothed[i] = sum / count;
            }

            // Calculate local mean and standard deviation
            for (int i = 0; i < spectrumBuffer.Length; i++)
            {
                int start = Mathf.Max(0, i - windowSize);
                int end = Mathf.Min(spectrumBuffer.Length - 1, i + windowSize);
                int count = end - start + 1;
                float mean = 0;
                float std = 0;
                for (int j = start; j <= end; j++)
                {
                    mean += smoothed[j];
                }
                mean /= count;
                for (int j = start; j <= end; j++)
                {
                    std += (smoothed[j] - mean) * (smoothed[j] - mean);
                }
                localMean[i] = mean;
                localStd[i] = Mathf.Sqrt(std / count);
            }

            // Detect peaks
            for (int i = 1; i < spectrumBuffer.Length - 1; i++)
            {
                if (smoothed[i] > smoothed[i - 1] && smoothed[i] > smoothed[i + 1] && smoothed[i] > localMean[i] + localDeviationMultiplier * localStd[i])
                {
                    peaks.Add(i);
                }
            }

            return peaks;
        }

        private void SaveMap()
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
        }
    }
}