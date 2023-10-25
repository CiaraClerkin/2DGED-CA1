using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip jumpClip;
    public AudioClip wallClip;
    public AudioClip footstepSound;
    public AudioClip backgroundMusic;

    private AudioSource soundEffectsSource;
    private AudioSource backgroundMusicSource;
    private AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        soundEffectsSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        //backgroundMusicSource.Play();
    }

    public void PlayJumpSound() {
        soundEffectsSource.PlayOneShot(jumpClip);
    }

    public void PlayWallSound() {
        soundEffectsSource.PlayOneShot(wallClip);
    }

    public void PlayFootstepSound() {
        soundEffectsSource.PlayOneShot(footstepSound);
    }

    public void PlayBackgroundMusic() {
        if (!backgroundMusicSource.isPlaying) {
            backgroundMusicSource.Play();
        }
    }

    public void StopBackgroundMusic() {
        backgroundMusicSource.Stop();
    }

    public void SetBackgroundMusicVolume(float volume) {
        backgroundMusicSource.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
