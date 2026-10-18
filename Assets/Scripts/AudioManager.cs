using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    LinkedList<AudioSource> sources;

    [SerializeField] int numSources;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        sources = new LinkedList<AudioSource>();

        for (int i = 0; i < numSources; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            sources.AddLast(source);
        }
    }

    public AudioSource PlaySound(
        AudioClip clip, 
        float volume = 1f, 
        float delay = 0f, 
        bool important = false)
    {
        AudioSource source = sources.First.Value;

        if (source.isPlaying)
        {
            if (!important)
            {
                Debug.LogWarning(
                    "Too many sounds are playing. " 
                    + clip.name 
                    + " ignored. NumSources = " 
                    + numSources);
                return null;
            }
            else
            {
                Debug.LogWarning(
                    "Too many sounds are playing. " 
                    + clip.name 
                    + " will override" 
                    + source.clip.name 
                    + ". NumSources = " 
                    + numSources);
            }
        }

        source.clip = clip;
        source.volume = volume;

        if (delay > 0f)
        {
            source.PlayDelayed(delay);
        }
        else
        {
            source.Play();
        }

        sources.RemoveFirst();
        sources.AddLast(source);

        return source;
    }

    public AudioSource PlayRandomSound(
        AudioClip[] clips, 
        float volume = 1f, 
        float delay = 0f, 
        bool important = false)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips in list");
            return null;
        }

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        return PlaySound(clip, volume, delay, important);
    }
}