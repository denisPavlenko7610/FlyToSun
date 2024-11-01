using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("#BGM")]
        public AudioClip bgmClip;
        public float bgmVolume;
        AudioSource bgmPlayer;
        AudioHighPassFilter bgmEffect;

        [Header("#SFX")]
        public AudioClip[] sfxClips;
        public float sfxVolume;
        public int channels;
        AudioSource[] sfxPlayers;
        int channelIndex;

        public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

        void Awake()
        {
            instance = this;
            Init();
        }

        void Init()
        {
            // 배경음 플레이어 초기화
            GameObject bgmObject = new GameObject("BgmPlayer");
            bgmObject.transform.parent = transform;
            bgmPlayer = bgmObject.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmClip;
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

            // 효과음 플레이어 초기화
            GameObject sfxObject = new GameObject("SfxPlayer");
            sfxObject.transform.parent = transform;
            sfxPlayers = new AudioSource[channels];

            for (int index = 0; index < sfxPlayers.Length; index++) {
                sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
                sfxPlayers[index].playOnAwake = false;
                sfxPlayers[index].bypassListenerEffects = true;
                sfxPlayers[index].volume = sfxVolume;
            }
        }

        public void PlayBgm(bool isPlay)
        {
            if (isPlay) {
                bgmPlayer.Play();
            }
            else {
                bgmPlayer.Stop();
            }
        }

        public void EffectBgm(bool isPlay)
        {
            bgmEffect.enabled = isPlay;
        }

        public void PlaySfx(Sfx sfx)
        {
            for (int index = 0; index < sfxPlayers.Length; index++) {
                int loopIndex = (index + channelIndex) % sfxPlayers.Length;

                if (sfxPlayers[loopIndex].isPlaying)
                    continue;

                int ranIndex = 0;
                if (sfx == Sfx.Hit || sfx == Sfx.Melee) {
                    ranIndex = Random.Range(0, 2);
                }

                channelIndex = loopIndex;
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
                sfxPlayers[loopIndex].Play();
                break;
            }
        }
    }
}
