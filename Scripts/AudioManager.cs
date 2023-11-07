using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{static AudioManager current;

    [Header("背景音乐")]
    public AudioClip BgClips;
    public AudioClip MenuClips;
    public AudioClip EndingClips;

    [Header("角色音效")]
    public AudioClip JumpClips;

    [Header("环境交互音效")]
    public AudioClip GetAtpClips;

    [Header("UI音效")]
    public AudioClip ButtonClips;
    public AudioClip PauseClips;

    AudioSource playerSource;
    AudioSource suspendSource;
    AudioSource musicSource;
    AudioSource interactSource;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);

        playerSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        interactSource = gameObject.AddComponent<AudioSource>();
        suspendSource = gameObject.AddComponent<AudioSource>();

        current.suspendSource.volume *= 0.5f;

        MainMenuAudio();
    }
    public static void StartLevelAudio()
    {
        // current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.BgClips;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }

    public static void CloseLevelAudio()
    {
        current.musicSource.Stop();
    }
    public static void PauseLevelAudio()
    {
        if (current.musicSource.mute == true)
            current.musicSource.mute = false;
        else
            current.musicSource.mute = true;

    }

    public static void MainMenuAudio()
    {
        // current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.MenuClips;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }
    public static void EndingAudio()
    {
        current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.EndingClips;
        current.musicSource.Play();
    }

    public static void JumpAudio()
    {
        current.playerSource.clip = current.JumpClips;
        current.playerSource.Play();
    }

}
