using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic;

    [Range(0f, 1f)]
    public float volume = 0.4f;

    void Awake()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.clip = backgroundMusic;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;
    }

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music is missing.");
            return;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        musicSource.volume = volume;
    }
}