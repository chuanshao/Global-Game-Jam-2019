using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

	public AudioSounds[] sounds;
	public static AudioManager instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(this);

		foreach(AudioSounds s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			
			if(s.name == "Background 1")
			{
				s.source.loop = true;
			}
			else
			{
				s.source.loop = false;
			}

			s.source.volume = s.Volume;
			s.source.pitch = s.Pitch;
		}
	}

	public void Play(string name)
	{
		AudioSounds s = Array.Find(sounds, sound => sound.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + "not found!");
			return;
		}

		s.source.Play();
	}
}
