using UnityEngine;
using System.Collections;

public class MusicZoneTrigger : MonoBehaviour
{
    [Header("Music Clips")]
    [SerializeField] private AudioClip zoneMusic;        // Music to play inside zone
    [SerializeField] private AudioClip defaultMusic;     // Music to play outside zone
    
    [Header("Transition Settings")]
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private float musicVolume = 0.7f;
    
    [Header("References")]
    [SerializeField] private AudioSource globalMusicSource; // Reference to your Music GameObject's AudioSource
    
    // Reference to the audio source
    private AudioSource musicSource;
    private Coroutine fadeCoroutine;
    
    private void Start()
    {
        // Try to find the global music source if not assigned
        if (globalMusicSource == null)
        {
            GameObject musicObject = GameObject.Find("Music");
            if (musicObject != null)
            {
                globalMusicSource = musicObject.GetComponent<AudioSource>();
            }
        }
        
        // Use the global music source instead of creating our own
        musicSource = globalMusicSource;
        
        // If we couldn't find the global music source, create our own as fallback
        if (musicSource == null)
        {
            Debug.LogWarning("Could not find Music GameObject - creating local AudioSource instead");
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            
            // Start with default music
            if (defaultMusic != null)
            {
                musicSource.clip = defaultMusic;
                musicSource.Play();
            }
        }
        else if (!musicSource.isPlaying && defaultMusic != null)
        {
            // If the global music source isn't playing anything, we can set our default
            musicSource.clip = defaultMusic;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && zoneMusic != null)
        {
            // Stop any existing fade
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
                
            // Start new fade to zone music
            fadeCoroutine = StartCoroutine(FadeToNewMusic(zoneMusic));
            Debug.Log("Changing to zone music");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && defaultMusic != null)
        {
            // Stop any existing fade
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
                
            // Start new fade to default music
            fadeCoroutine = StartCoroutine(FadeToNewMusic(defaultMusic));
            Debug.Log("Returning to default music");
        }
    }
    
    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        // Fade out current music
        float startVolume = musicSource.volume;
        float timer = 0;
        
        while (timer < fadeTime/2)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, timer/(fadeTime/2));
            yield return null;
        }
        
        // Change clip and fade back in
        musicSource.clip = newClip;
        musicSource.Play();
        timer = 0;
        
        while (timer < fadeTime/2)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, musicVolume, timer/(fadeTime/2));
            yield return null;
        }
        
        musicSource.volume = musicVolume;
    }
}