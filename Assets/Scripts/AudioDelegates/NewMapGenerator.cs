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
                TrackPeaks();
            
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

            spectra = new OfflineFFT(songData.songAudio, bufferSize).SpectrumBuffers;
        }

        // Variable to keep track of the current frame index
        private int currentFrameIndex = 0;
        
        // Sample rate of the audio data
        private int sampleRate = 44100; // Assuming a sample rate of 44.1 kHz

        // Buffer size (number of samples per frame)
        private int bufferSize = 1024;
        
        // Threshold for peak detection (adjust this value to control the number of peaks)
        private float peakThreshold = 0.05f;

        // Method to find and store the peak values with their frame indices and time
        private void TrackPeaks()
        {
            // Clear the existing peak values, frame indices, and time
            songData.songPositionInSeconds.Clear();

            // Iterate through each spectrum buffer
            foreach (var spectrumBuffer in spectra)
            {
                // Find the maximum value (peak) in the current buffer
                float peak = Mathf.Max(spectrumBuffer);

                // Calculate the time in seconds for the current frame
                float timeInSeconds = (float)currentFrameIndex * bufferSize / sampleRate;
                
                if (timeInSeconds >= 3 && peak >= peakThreshold)
                {
                    // Add the peak value, frame index, and time to the list
                    songData.songPositionInSeconds.Add(timeInSeconds);
                }

                // Increment the frame index
                currentFrameIndex++;
            }
        }
        
        private void SaveMap()
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
        }
    }
}