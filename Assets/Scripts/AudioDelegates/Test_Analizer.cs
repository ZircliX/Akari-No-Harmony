using System.Collections.Generic;
using UnityEngine;

namespace AudioDelegates
{
    public class Test_Analizer
    {
        public AudioClip audioClip;
        public int fftSize = 1024;
        public float[] spectrum;

        private float[] samples;
        private float[] window;


        public List<float> songPositionInBeats;
        public int bpm;
        public float songOffset;
        private float spectrumIntensityThreshold = .2f;

        void GetAudioSamples()
        {
            // Get the audio samples from the AudioClip
            audioClip.GetData(samples, 0);
        }

        void GenerateHanningWindow(float[] window)
        {
            // Generate a Hanning window
            for (int i = 0; i < window.Length; i++)
            {
                window[i] = 0.5f - 0.5f * Mathf.Cos(2 * Mathf.PI * i / (window.Length - 1));
            }
        }

        float[] PerformFFT(float[] samples, float[] window)
        {
            // Apply the window to the samples
            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] *= window[i];
            }

            // Perform the FFT
            float[] spectrum = new float[samples.Length];
            int n = samples.Length;
            int logn = (int)Mathf.Log(n, 2);

            // Bit-reverse the input samples
            BitReverse(samples);

            // Compute the FFT
            for (int s = 1; s <= logn; s++)
            {
                int m = (int)Mathf.Pow(2, s);
                int m2 = m / 2;
                //float wm = 0.0f;
                float wr = Mathf.Cos(Mathf.PI / m2);
                float wi = -Mathf.Sin(Mathf.PI / m2);

                for (int j = 0; j < m2; j++)
                {
                    for (int i = j; i < n; i += m)
                    {
                        int i1 = i;
                        int i2 = i + m2;
                        float t1 = samples[i1];
                        float t2 = samples[i2];
                        samples[i1] = t1 + wr * t2 - wi * spectrum[i2];
                        samples[i2] = t1 - wr * t2 + wi * spectrum[i1];
                    }

                    float wtemp = wr;
                    wr = wr * Mathf.Cos(Mathf.PI / m2) - wi * Mathf.Sin(Mathf.PI / m2);
                    wi = wtemp * Mathf.Sin(Mathf.PI / m2) + wi * Mathf.Cos(Mathf.PI / m2);
                }
            }

            // Copy the real part of the FFT to the spectrum array
            for (int i = 0; i < samples.Length; i++)
            {
                spectrum[i] = samples[i];
            }

            return spectrum;
        }

        void BitReverse(float[] samples)
        {
            int n = samples.Length;
            int j = 0;

            for (int i = 0; i < n; i++)
            {
                if (i < j)
                {
                    (samples[i], samples[j]) = (samples[j], samples[i]);
                }

                int m = n / 2;
                while (j >= m && m >= 2)
                {
                    j -= m;
                    m /= 2;
                }
                j += m;
            }
        }

        void ProcessSpectrum(float[] spectrum)
        {
            // Process the spectrum data here
            // For example, you can find the peak frequencies or visualize the spectrum
        }

        public Test_Analizer(int songBpm, float firstBeatOffset, AudioClip audio)
        {
            // Initialize spectrum array
            spectrum = new float[1024];

            this.bpm = songBpm;
            this.songOffset = firstBeatOffset;
            this.audioClip = audio;
            
            // Initialize the samples and window arrays
            samples = new float[fftSize];
            window = new float[fftSize];

            // Generate a Hanning window
            GenerateHanningWindow(window);
            
            // Get the audio samples
            GetAudioSamples();

            // Perform the FFT
            spectrum = PerformFFT(samples, window);

            // Process the spectrum data here
            ProcessSpectrum(spectrum);
            
            AnalyzeSpectrum();
        }

        void AnalyzeSpectrum()
        {
            float songLength = audioClip.length;
            float beatLength = 60f / bpm;
            float currentTime = songOffset;
            int beatCount = 0;

            while (currentTime < songLength)
            {
                // Check if the spectrum intensity exceeds the threshold
                if (GetSpectrumIntensity(spectrum) > spectrumIntensityThreshold)
                {
                    songPositionInBeats.Add(beatCount);
                }

                currentTime += beatLength;
                beatCount++;
            }
        }

        float GetSpectrumIntensity(float[] spectrum)
        {
            float intensity = 0f;
            foreach (float value in spectrum)
            {
                intensity += value;
            }
            
            Debug.Log(intensity / spectrum.Length);
            return intensity / spectrum.Length;
        }
    }
}