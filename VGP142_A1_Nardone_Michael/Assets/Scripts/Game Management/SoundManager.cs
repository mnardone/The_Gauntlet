using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    static SoundManager _instance = null;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip titleMusic;
    public AudioClip levelMusic;
    public AudioClip victoryMusic;
    public AudioClip failureMusic;

	// Use this for initialization
	void Start () {

        if (_instance)
            DestroyImmediate(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        playMusic(titleMusic);
	
	}

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            playMusic(titleMusic);
        }
        else if (level == 1)
        {
            playMusic(levelMusic);
        }
        else if (level == 2)
        {
            playMusic(failureMusic);
        }
        else if (level == 3)
        {
            playMusic(victoryMusic);
        }
    }

    public void playSFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void playMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void stopMusic()
    {
        musicSource.Stop();
    }

    public void changeVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
}
