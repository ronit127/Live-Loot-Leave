using UnityEngine;

public class DelayedMusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource combatSource;
    public GameObject tutorialPanel;

    [Header("Timing")]
    public float delay = 15f;

    void Start()
    {
        Invoke("SwitchMusic", delay);
    }

    void SwitchMusic()
    {
        // stop the background music
        if (backgroundSource != null && backgroundSource.isPlaying)
        {
            backgroundSource.Stop();
        }

        // start the combat music
        if (combatSource != null)
        {
            combatSource.loop = true;
            combatSource.Play();
        }

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Destroy(tutorialPanel, 10f);
        }
    }
}