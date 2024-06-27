using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets._Scripts.Core_Scripts.Managers
{
    [System.Serializable]
    public class Sound
    {
        public string Name;

        [Space]
        public AudioClip Clip;
        public AudioMixerGroup Group;

        [Space(15), Range(0f, 1f)] public float Volume = 0.1f;
        [Range(0.1f, 3f)] public float Pitch = 1f;

        [Space(30)]
        public bool Loop;

        [HideInInspector]
        public AudioSource Source;
    }

    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance => _instance;

        [Header("UI Elements")]
        public AudioMixer MasterMixer;

        [Space] 
        public Slider SfxSlider;
        public Slider MusicSlider;
        public Slider MasterSlider;

        [Space] 
        public string FirstSongToPlay;
        public Sound[] Sounds;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            // Initialize audio sources
            foreach (Sound s in Sounds)
            {
                // setup source and clip
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
            
                // set up remaining audio
                s.Source.volume = s.Volume;
                s.Source.pitch = s.Pitch;
                s.Source.outputAudioMixerGroup = s.Group;
                s.Source.loop = s.Loop;
            }
        }

        void Start()
        {
            Play(FirstSongToPlay);
            UpdateSliders();
        }
        
        /// <summary>
        /// Plays the specified sound.
        /// </summary>
        /// <param name="soundName">The name of the sound to play.</param>
        public void Play(string soundName)
        {
            Sound s = Array.Find(Sounds, sound => sound.Name == soundName);
            if (s == null)
            {
                Debug.LogWarning($"Sound: {soundName} was not found!\n" +
                                 "Did you do a typo?");
                return;
            }
            
            s.Source.Play();
        }
        
        /// <summary>
        /// Stops the specified sound.
        /// </summary>
        /// <param name="soundName">The name of the sound to stop.</param>
        public void Stop(string soundName)
        {
            Sound s = Array.Find(Sounds, sound => sound.Name == soundName);
        
            if (s == null)
            {
                Debug.LogWarning($"Sound: {soundName} was not found!\n" +
                                 "Did you do a typo?");
                return;
            }
        
            s.Source.Stop();
        }

        private void OnEnable()
        {
            UpdateSliders();
        }

        private void UpdateSliders()
        {
            // Update audio levels
            SetSfxLevel(GetSfxLevel());
            SetMusicLvl(GetMusicLevel());
            SetMasterLvl(GetMasterLevel());

            // Update sliders
            SfxSlider.value = GetSfxLevel();
            MusicSlider.value = GetMusicLevel();
            MasterSlider.value = GetMasterLevel();
        }

        // Getter methods
        public float GetSfxLevel()
        {
            return PlayerPrefs.GetFloat("SfxVolume", 0f);
        }

        public float GetMusicLevel()
        {
            return PlayerPrefs.GetFloat("MusicVolume", 0f);
        }

        public float GetMasterLevel()
        {
            return PlayerPrefs.GetFloat("MasterVolume", 0f);
        }

        // Setter methods
        public void SetSfxLevel(float sfxLevel)
        {
            MasterMixer.SetFloat("SfxVolume", sfxLevel);
            PlayerPrefs.SetFloat("SfxVolume", sfxLevel); // Store the value in PlayerPrefs

        }

        public void SetMusicLvl(float musicLevel)
        {
            MasterMixer.SetFloat("MusicVolume", musicLevel);
            PlayerPrefs.SetFloat("MusicVolume", musicLevel); // Store the value in PlayerPrefs
        }

        public void SetMasterLvl(float masterLevel)
        {
            MasterMixer.SetFloat("MasterVolume", masterLevel);
            PlayerPrefs.SetFloat("MusicVolume", masterLevel); // Store the value in PlayerPrefs
        }

        // Methods for Slider's OnValueChanged
        public void OnSfxSliderChanged()
        {
            SetSfxLevel(SfxSlider.value);
        }

        public void OnMusicSliderChanged()
        {
            SetMusicLvl(MusicSlider.value);
        }

        public void OnMasterSliderChanged()
        {
            SetMasterLvl(MasterSlider.value);
        }
    }
}
