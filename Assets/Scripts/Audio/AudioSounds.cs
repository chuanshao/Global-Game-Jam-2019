using UnityEngine;

[System.Serializable]
public class AudioSounds{

	public string name;

	public AudioClip clip;

	[Range(0, 1)]
	public float Volume;
	[Range(0, 1)]
	public float Pitch;

	[HideInInspector]
	public AudioSource source;
}
