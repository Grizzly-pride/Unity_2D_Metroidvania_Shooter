using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public AudioSource mainMenuMusic, levelMusic, bossMusic;
    public AudioSource[] sfx;

    public void PlayMainMenuMusic()
    {
        mainMenuMusic.Play();
        levelMusic.Stop();
        bossMusic.Stop();
    }

    public void PlayLevelMusic()
    {
        if (!levelMusic.isPlaying)
        {
            mainMenuMusic.Stop();
            levelMusic.Play();
            bossMusic.Stop();
        }

    }

    public void PlayBossMusic()
    {
        levelMusic.Stop();
        bossMusic.Play();
    }


    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
 
    }

    public void PlaySFXAdjusted(int sfxToAdjust)
    {
        sfx[sfxToAdjust].pitch = Random.Range(0.8f, 1.2f);
        PlaySFX(sfxToAdjust);
    }
}
