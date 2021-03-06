﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M22
{

    public class AudioMaster : MonoBehaviour
    {
        static Dictionary<string, AudioClip> loadedAudio = new Dictionary<string, AudioClip>();

        static AudioSource musicSrc;

        static bool _forceStop = false;

        // Courtesy: https://forum.unity3d.com/threads/fade-out-audio-source.335031/
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                if (_forceStop == true)
                {
                    _forceStop = false;
                    yield break;
                }

                audioSource.volume -= startVolume * Time.deltaTime * FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        static public void UnloadAudio()
        {
            foreach (var item in loadedAudio)
            {
                //item.Value.UnloadAudioData();
                Resources.UnloadAsset(item.Value);
            }
            loadedAudio.Clear();
        }

        static public AudioClip GetAudio(string _input)
        {
            AudioClip output;
            loadedAudio.TryGetValue(_input, out output);
            if (output == null)
                return null;
            else return output;
        }

        static public bool IsAudioLoaded(string _input)
        {
            return loadedAudio.ContainsKey(_input);
        }

        // Use this for initialization
        void Start()
        {
            musicSrc = this.GetComponent<AudioSource>();
            if (musicSrc == null)
                Debug.LogError("AudioSource for music not attached to camera!");
        }

        static public bool PlaySting(string name)
        {
            AudioClip sfx;
            loadedAudio.TryGetValue(name, out sfx);
            if (sfx == null)
                return false;
            musicSrc.PlayOneShot(sfx);
            return true;
        }

        public void StopMusic(string _floatInput)
        {
            float speed = float.Parse(_floatInput);
            StartCoroutine(FadeOut(musicSrc, speed));
        }

        static public bool LoadMusic(string name)
        {
            if (loadedAudio.ContainsKey(name)) return true;
            AudioClip temp = Resources.Load("Music/" + name) as AudioClip;
            if (temp != null)
            {
                loadedAudio.Add(name, temp);
                return true;
            }
            else
            {
                Debug.LogError("Failed to load audio: Music/" + name);
                return false;
            }
        }

        static public bool LoadSting(string name)
        {
            if (loadedAudio.ContainsKey(name)) return true;
            AudioClip temp = Resources.Load("SFX/" + name) as AudioClip;
            if (temp != null)
            {
                loadedAudio.Add(name, temp);
                return true;
            }
            else
            {
                Debug.LogError("Failed to load audio: SFX/" + name);
                return false;
            }
        }

        static public void ChangeTrack(AudioClip _track)
        {
            musicSrc.Stop();
            musicSrc.time = 0;
            musicSrc.clip = _track;
            musicSrc.Play();
        }

        static public void ChangeTrack(string _track)
        {
            _forceStop = true;
            musicSrc.Stop();
            musicSrc.time = 0;
            AudioClip track;
            loadedAudio.TryGetValue(_track, out track);
            musicSrc.clip = track;
            musicSrc.Play();
        }

        static public void StopMusic()
        {
            musicSrc.Stop();
            musicSrc.time = 0;
        }
        static public void StartMusic()
        {
            musicSrc.Play();
        }
    }

}