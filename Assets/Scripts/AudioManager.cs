using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [SerializeField] AudioClip bgm;
    [SerializeField] AudioClip[] sfx;


    enum SFX { Clear, Coin, Damaged, Hit, Jump1, Jump2, Start };
    SFX sfxType;

    AudioSource[] audioSources; 



    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        audioSources = GetComponents<AudioSource>();

    }

    public void BGMSTART()
    {
        audioSources[0].clip = bgm;
        audioSources[0].Play();
    }

    public void SFXPlayer(string sfxType)
    {
        this.sfxType = (SFX)Enum.Parse(typeof(SFX), sfxType);
        for (int i = 1; i < audioSources.Length; i++)
        {

            if (audioSources[i].isPlaying && audioSources[i].clip != sfx[(int)this.sfxType])
                continue;
            else
            {
                audioSources[i].clip = sfx[(int)this.sfxType];
                audioSources[i].Play();
                return;
            }
        }
    }

}
