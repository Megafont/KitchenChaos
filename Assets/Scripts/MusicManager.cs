using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MusicManager : MonoBehaviour
{
    private const string PLAYERPREFS_MUSIC_VOLUME = "MusicVolume";


    public static MusicManager Instance { get; private set; }


    private AudioSource _AudioSource;
    private float _Volume = 0.3f;



    private void Awake()
    {
        Instance = this;

        _AudioSource = GetComponent<AudioSource>();

        _Volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSIC_VOLUME, 0.3f);
        _AudioSource.volume = _Volume;
    }

    public void ChangeVolume()
    {
        _Volume += 0.1f;

        if (_Volume >= 1.1f)
            _Volume = 0f;

        _AudioSource.volume = _Volume;

        PlayerPrefs.SetFloat(PLAYERPREFS_MUSIC_VOLUME, _Volume);
        PlayerPrefs.Save(); // Save manually in case Unity crashes before it saves them itself.     
    }

    public float GetVolume()
    {
        return _Volume;
    }
}
