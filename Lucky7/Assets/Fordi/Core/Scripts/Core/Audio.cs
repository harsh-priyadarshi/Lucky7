using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fordi.Core
{
    [Serializable]
    public class AudioArgs
    {
        public AudioClip Clip;
        public bool Fade;
        public float FadeTime = 0;
        public Action Done;

        public AudioArgs(AudioClip clip)
        {
            Clip = clip;
        }
    }

    public interface IAudio
    {
        void Play(AudioArgs args);
        void Pause(AudioArgs args);
        void Resume(AudioArgs args);
        void Stop(AudioArgs args);
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class Audio : MonoBehaviour, IAudio
    {
        private AudioSource m_audioSource;

        void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        public void Pause(AudioArgs args)
        {
            if (args.Clip == null)
                return;

            StartCoroutine(CoAudioVolume(0, args.FadeTime, () =>
            {
                m_audioSource.Pause();
                args.Done?.Invoke();
            }));
        }

        public void Play(AudioArgs args)
        {
            //Debug.LogError("Play");
            if (args.Clip == null)
                return;
            m_audioSource.clip = args.Clip;
            m_audioSource.volume = 0.0f;
            m_audioSource.Play();
            if (args.FadeTime == 0)
                m_audioSource.volume = 1;
            else
                StartCoroutine(CoAudioVolume(1, args.FadeTime, args.Done));
        }

        public void Resume(AudioArgs args)
        {
            if (args.Clip == null)
                return;

            m_audioSource.clip = args.Clip;
            m_audioSource.volume = 0;
            m_audioSource.UnPause();
            if (args.FadeTime == 0)
                m_audioSource.volume = 1;
            else
                StartCoroutine(CoAudioVolume(1, args.FadeTime, null));
        }

        public void Stop(AudioArgs args)
        {
            if (args.Clip == null)
                return;
            StartCoroutine(CoAudioVolume(0, args.FadeTime, () =>
            {
                m_audioSource.Stop();
                args.Done?.Invoke();
            }));
        }

        IEnumerator CoAudioVolume(float final, float time, Action done)
        {
            //Debug.LogError(time);
            bool play = m_audioSource.volume < final;

            float initialVolume = m_audioSource.volume;
            float elapsedTime = 0.0f;
            while (elapsedTime < time)
            {

                elapsedTime += Time.deltaTime;
                float currentVolume = Mathf.Lerp(play ? 0 : initialVolume, final, Mathf.Clamp01(elapsedTime / time));
                m_audioSource.volume = currentVolume;
                //Debug.Log(m_audioSource.volume);
                yield return new WaitForEndOfFrame();
            }

            done?.Invoke();
        }
        IEnumerator Fade(float final, float fadeDuration)
        {
            bool play = m_audioSource.volume < final;

            float initialVolume = m_audioSource.volume;
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeDuration)
            {

                elapsedTime += Time.deltaTime;
                float currentVolume = Mathf.Lerp(play ? 0 : initialVolume, play ? initialVolume : 0, Mathf.Clamp01(elapsedTime / fadeDuration));
                m_audioSource.volume = currentVolume;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
