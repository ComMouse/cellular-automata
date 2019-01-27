using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField]
    private SoundInfo[] soundList;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name)
    {
        if (name.IndexOf("Effect_") > -1)
        {
            PlayEffect(name);
            return;
        }

        var sound = soundList.First(s => s.name == name);
        if (sound == null)
            return;

        sound.audio.Play();
    }

    public void PlayEffect(string name)
    {
        var sound = soundList.First(s => s.name == name);
        if (sound == null)
            return;

        var audioSrc = Instantiate(sound.audio.gameObject);
        audioSrc.GetComponent<AudioSource>().Play();

        Destroy(audioSrc, sound.audio.clip.length + 1f);
    }

    public void Play(string name, float volume)
    {
        var sound = soundList.First(s => s.name == name);
        if (sound == null)
            return;

        sound.audio.volume = volume;
        sound.audio.Play();
    }

    public void Stop(string name)
    {
        var sound = soundList.First(s => s.name == name);
        if (sound == null)
            return;

        sound.audio.Stop();
    }

    public void FadeTo(string name, float volume, float time = 1f)
    {
        var sound = soundList.First(s => s.name == name);
        if (sound == null)
            return;

        StartCoroutine(FadeToAsync(sound.audio, volume, time));
    }

    private IEnumerator FadeToAsync(AudioSource audio, float volume, float time)
    {
        float oldVolume = volume;
        float lastTime = 0f;

        yield return null;

        while (lastTime < time)
        {
            lastTime = Mathf.Min(time, lastTime + Time.deltaTime);
            audio.volume = Mathf.Lerp(oldVolume, volume, lastTime / time);

            yield return null;
        }
    }
}

[Serializable]
public class SoundInfo
{
    public string name;
    public AudioSource audio;
}
