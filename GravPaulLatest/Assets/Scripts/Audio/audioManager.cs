using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine;

public class audioManager : MonoBehaviour {

    public Sound[] sounds;

    [SerializeField]
    float killTime;

    public static audioManager instance;

    void Awake () {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    void Start ()
    {
        Play("level_1_music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
            return;
        }
            s.source.Play();
    }

    public float killMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
        }
        StartCoroutine(slowSound(s, killTime));
        return killTime;
    }

    public void reviveMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
            return;
        }
        s.source.Stop();
        s.source.pitch = 1;
        Play(name);
    }

    IEnumerator slowSound(Sound s, float time)
    {
        while (time > 0)
        {
            time -= (killTime / 100);
            s.source.pitch -= (1f / 100f);
            yield return new WaitForSeconds(killTime / 100);
        }
    }
}
