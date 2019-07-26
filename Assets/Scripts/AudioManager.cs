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
		DontDestroyOnLoad(this);
		//DontDestroyOnLoad(gameObject);
		_audioSource = transform.Find("Background").GetComponent<AudioSource>();
	    _audioSource.clip = BackgroundClip;
        _audioSource.Play();

		_fxAudioSource = transform.Find ("FX").GetComponent<AudioSource> ();

		int vol = PlayerPrefs.GetInt(UIManager.PLAY_STRING, 1);
		UpdateSoundVolume(vol);
	}

    public void UpdateSoundVolume(float volume)
    {
        _audioSource.volume = volume;
		_fxAudioSource.volume = volume * 0.2f;
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
