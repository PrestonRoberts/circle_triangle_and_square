using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Audio source
    private AudioSource audioSource;

    // Sound effects
    [SerializeField] private AudioClip rotateSound;
    [SerializeField] private AudioClip placeArrowSound;
    [SerializeField] private AudioClip removeArrowSound;

    // Grid sounds
    [SerializeField] private AudioClip blueSound;
    [SerializeField] private AudioClip redSound;
    [SerializeField] private AudioClip greenSound;
    [SerializeField] private AudioClip yellowSound;
    [SerializeField] private AudioClip whiteSound;

    // Audio slider control
    public Slider soundEffectsVolumeSlider;
    public Slider musicVolumeSlider;

    // SFX Audio Source
    private AudioSource SFX;

    // Grid sounds
    private AudioSource blueAudioSource;
    private AudioSource redAudioSource;
    private AudioSource greenAudioSource;
    private AudioSource yellowAudioSource;
    private AudioSource whiteAudioSource;


    void Start()
    {
        // Create new audio source component
        SFX = gameObject.AddComponent<AudioSource>();

        // Create new audio sources components for each sound
        blueAudioSource = gameObject.AddComponent<AudioSource>();
        redAudioSource = gameObject.AddComponent<AudioSource>();
        greenAudioSource = gameObject.AddComponent<AudioSource>();
        yellowAudioSource = gameObject.AddComponent<AudioSource>();
        whiteAudioSource = gameObject.AddComponent<AudioSource>();

        // Set sound for audio sources
        blueAudioSource.clip = blueSound;
        redAudioSource.clip = redSound;
        greenAudioSource.clip = greenSound;
        yellowAudioSource.clip = yellowSound;
        whiteAudioSource.clip = whiteSound;
    }

    // Play sound
    public void PlaceArrowSound()
    {
        SFX.clip = placeArrowSound;
        SFX.Play();
    }

    public void RemoveArrowSound()
    {
        SFX.clip = removeArrowSound;
        SFX.Play();
    }

    public void RotateSound()
    {
        SFX.clip = rotateSound;
        SFX.Play();
    }

    // Play grid sound
    public void PlayCircleSound(PlayerCircle.CircleColor circleColor)
    {
        switch (circleColor)
        {
            case PlayerCircle.CircleColor.Blue:
                blueAudioSource.Play();
                break;
            case PlayerCircle.CircleColor.Red:
                redAudioSource.Play();
                break;
            case PlayerCircle.CircleColor.Green:
                greenAudioSource.Play();
                break;
            case PlayerCircle.CircleColor.Yellow:
                yellowAudioSource.Play();
                break;
            case PlayerCircle.CircleColor.White:
                whiteAudioSource.Play();
                break;
        }
    }

    // Update music level
    public void UpdateMusicLevel()
    {
        blueAudioSource.volume = musicVolumeSlider.value;
        redAudioSource.volume = musicVolumeSlider.value;
        greenAudioSource.volume = musicVolumeSlider.value;
        yellowAudioSource.volume = musicVolumeSlider.value;
        whiteAudioSource.volume = musicVolumeSlider.value;
    }

    // Update audio level
    public void UpdateSFXLevel()
    {
        SFX.volume = soundEffectsVolumeSlider.value;
    }
}
