using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio clip")]
    public AudioClip music;
    public AudioClip ballHit;

    private void Start()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
