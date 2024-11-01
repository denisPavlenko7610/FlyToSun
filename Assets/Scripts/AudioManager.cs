using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("#BGM")]
        public AudioClip bgmClip;
        public float bgmVolume;
        private AudioSource bgmPlayer;
        private AudioHighPassFilter bgmEffect;

        [Header("#SFX")]
        public AudioClip[] sfxClips;
        public float sfxVolume;
        public int channels;
        private AudioSource[] sfxPlayers;
        private int channelIndex;

        public enum Sfx { Dead, Hit, LevelUp, Lose, Melee, Range, Select, Win }

        private void Awake()
        {
            Instance = this;
            InitializeAudioSources();
        }

        private void InitializeAudioSources()
        {
            bgmPlayer = CreateAudioSource("BgmPlayer", bgmClip, bgmVolume, true);
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

            sfxPlayers = new AudioSource[channels];
            GameObject sfxObject = new GameObject("SfxPlayer");
            sfxObject.transform.parent = transform;

            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                sfxPlayers[i] = CreateAudioSource(sfxObject.name, null, sfxVolume, false);
                sfxPlayers[i].bypassListenerEffects = true;
            }
        }

        private AudioSource CreateAudioSource(string name, AudioClip clip, float volume, bool loop)
        {
            GameObject audioObject = new GameObject(name);
            audioObject.transform.parent = transform;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.playOnAwake = false;
            return audioSource;
        }

        public void PlayBgm(bool play)
        {
            if (play) 
                bgmPlayer.Play();
            else 
                bgmPlayer.Stop();
        }

        public void EffectBgm(bool enable)
        {
            bgmEffect.enabled = enable;
        }

        public void PlaySfx(Sfx sfx)
        {
            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                int index = (i + channelIndex) % sfxPlayers.Length;

                if (sfxPlayers[index].isPlaying)
                    continue;

                int randomOffset = sfx is Sfx.Hit or Sfx.Melee 
                    ? Random.Range(0, 2) 
                    : 0;

                channelIndex = index;
                sfxPlayers[index].clip = sfxClips[(int)sfx + randomOffset];
                sfxPlayers[index].Play();
                break;
            }
        }
    }
}
