using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public AudioSource bgMusic;
    public AudioSource pick;
    public AudioSource click;
    public AudioSource win;
    public AudioSource lose;
    public AudioSource paper;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }
    public void PlayBgMusic()
    {
        bgMusic.Play();
    }
    public void MuteMusic()
    {
        bgMusic.mute = true;

    }
    public void UnmuteMusic()
    {
        bgMusic.mute = false;
    }
    public void MuteSfx()
    {
        pick.mute = true;
        click.mute = true;
        win.mute = true;
        lose.mute = true;
    }
    public void UnmuteSfx()
    {
        pick.mute = false;
        click.mute = false;
        win.mute = false;
        lose.mute = false;
    }
    public void PlayPick()
    {
        pick.Play();
    }
    public void PlayClick()
    {
        click.Play();
    }
    public void PlayWin()
    {
        win.Play();
    }
    public void PlayLose()
    {
        lose.Play();
    }
    public void PlayPaper()
    {
        paper.Play();
    }   
}
