using System;
using System.Collections.Generic;
using FftSharp;
using UnityEngine;

namespace AudioDelegates
{
    /// <summary>
    /// Class <c>OfflineFFT</c> pre-generates a buffer of spectrum
    /// in the manner of AudioSource.GetSpectrum() but without the need of
    /// playing the AudioClip.
    /// 
    /// This class requires to have FftSharp installed in your project.
    /// </summary>
    public class OfflineFFT
    {
        private readonly AudioClip audioClip;
        private readonly int batchSize;
        private readonly Window window = new FftSharp.Windows.Hanning();
        
        public OfflineFFT(AudioClip audioClip, int batchSize)
        {
            this.audioClip = audioClip;
            this.batchSize = batchSize;
            GenerateSpectrumBuffers();
        }

        public List<float[]> SpectrumBuffers;

        private static float[] GetAudioData(AudioClip audioClip)
        {
            float[] data = new float[audioClip.samples];
            audioClip.GetData(data, 0);
            return data;
        }

        private void GenerateSpectrumBuffers()
        {
            SpectrumBuffers = new List<float[]>();
            float[] audioData = GetAudioData(audioClip);

            for (int i = 0; i < audioData.Length / batchSize; i++)
            {
                double[] batch = new double[batchSize];
                Array.ConstrainedCopy(audioData, i * batchSize, batch, 0, batchSize);
                SpectrumBuffers.Add(GetSpectrum(batch, window));
            }
        }

        private static float[] GetSpectrum(double[] audioBuffer, IWindow window)
        {
            double[] windowed = window.Apply(audioBuffer, true);
            var spectrum = FFT.ForwardReal(windowed);

            float[] magnitude = new float[spectrum.Length];
            for (int j = 0; j < magnitude.Length; j++)
                magnitude[j] = (float)spectrum[j].Magnitude;

            return magnitude;
        }
    }
}