using System;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Sound
{
    public AudioClip clip;
    public SoundName name;
    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;

    public void Init()
    {
        source.clip = clip;
        source.volume = volume;
    }
}

public enum SoundName
{
    DestroyingPlayer,
    DestroyingEnemy,
    DestroyingBullet,
    PlayerShooting,
    Clicking
}

public class AudioManager : MonoBehaviour
{
    public float timeBetweenBackgroundMusic = 2f;
    public AudioSource mainAudioSource;
    public List<AudioClip> backgroundAudioClips;


    public Sound[] sounds;
    private bool canPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.Init();
        }
        RandomMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainAudioSource.isPlaying && canPlay && mainAudioSource != null)
            StartCoroutine(PlayBackgroundMusicInTime());
    }
    private IEnumerator PlayBackgroundMusicInTime()
    {
        mainAudioSource.Stop();
        canPlay = false;

        yield return new WaitForSeconds(timeBetweenBackgroundMusic);

        canPlay = true;
        RandomMusic();
    }
    private void RandomMusic()
    {
        var randomClip = backgroundAudioClips[UnityEngine.Random.Range(0, backgroundAudioClips.Count)];
        mainAudioSource.clip = randomClip;
        mainAudioSource.Play();
    }
    public void PlaySound(SoundName name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Play();
    }
}
