using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX clip is missing.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("SFX AudioSource is missing.");
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }
}