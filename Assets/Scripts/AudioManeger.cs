using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AudioManeger : SingletonMonoBehaviour<AudioManeger>
{
    public List<AudioClip> BGMList;
    public List<AudioClip> SEList;
    public int maxSE = 10;

    private AudioSource bgmSource = null;
    private List<AudioSource> seSource = null;
    private Dictionary<string,AudioClip> bgmDict = null;
    private Dictionary<string, AudioClip> seDict = null;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if(FindObjectsOfType(typeof(AudioListener)).All(o => !((AudioListener)o).enabled))
        {
            this.gameObject.AddComponent<AudioListener>();
        }
    }

    private void Start()
    {
        
    }
}
