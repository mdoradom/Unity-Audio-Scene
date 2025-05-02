using UnityEngine;
using UnityEngine.Audio;

public class CaveAudioTransition : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot outsideSnapshot;
    [SerializeField] private AudioMixerSnapshot caveSnapshot;
    [SerializeField] private float transitionTime = 0.5f;
    
    // Called when any collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is entering the cave
            caveSnapshot.TransitionTo(transitionTime);
            Debug.Log("Entered cave - applying cave acoustics");
        }
    }
    
    // Called when any collider exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is leaving the cave
            outsideSnapshot.TransitionTo(transitionTime);
            Debug.Log("Exited cave - returning to normal acoustics");
        }
    }
}