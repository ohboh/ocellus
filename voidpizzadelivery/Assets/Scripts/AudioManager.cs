using UnityEngine.Audio;
using System;
using UnityEngine;

// from a Brackey's video
public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	private bool isPlayed;

	public ResonanceAudioSource reSource;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = s.audioSource.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.spatialBlend = 1f;
			s.source.rolloffMode = AudioRolloffMode.Linear;
			s.source.dopplerLevel = 3f;
			s.source.spread = 360f;
			s.source.maxDistance = 500f;
			s.source.spatialize = true;
			s.source.spatializePostEffects = true;
			reSource = s.audioSource.AddComponent<ResonanceAudioSource>();
			reSource.occlusionEnabled = true;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if (!s.source.isPlaying)
		{
			s.source.Play();
		}
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source.isPlaying)
		{
			s.source.Stop();
		}
	}

	public void StopAll()
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			Sound s = sounds[i];
			if (s.source == s.source.isPlaying)
			{
				s.source.Stop();
			}
			i++;
		}
	}

	//there's a much cleaner implementation but eh...
	public void Run(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s.source.isPlaying)
		{
			s.source.pitch = 1.4f;
			s.source.volume = 1f;
		}
	}

	public void Slow(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s.source.isPlaying)
		{
			s.source.pitch = 0.7f;
		}
	}

}
