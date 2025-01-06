using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio clip")]
    public AudioClip music;
    public AudioClip ballHit;

    private void Start()
    {
        musicSource.volume = 0.5f;
        SFXSource.volume = 0.5f;

        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume); ;
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = Mathf.Clamp01(volume); ;
    }
}
