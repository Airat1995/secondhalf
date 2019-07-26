using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip WinAudioClip;

	public AudioClip LoseAudioClip;

    public AudioClip BackgroundClip;

    private AudioSource _audioSource;

	private AudioSource _fxAudioSource;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
		_audioSource = transform.FindChild("Background").GetComponent<AudioSource>();
	    _audioSource.clip = BackgroundClip;
        _audioSource.Play();

		_fxAudioSource = transform.FindChild ("FX").GetComponent<AudioSource> ();
	}

    public void UpdateSoundVolume(float volume)
    {
        _audioSource.volume = volume;
    }

	public void LevelWin()
	{
		_fxAudioSource.clip = WinAudioClip;
		_fxAudioSource.Play ();
	}

	public void Lose()
	{
		_fxAudioSource.clip = LoseAudioClip;
		_fxAudioSource.Play ();
	}
}
