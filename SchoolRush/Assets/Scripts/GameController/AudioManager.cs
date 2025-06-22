using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip checkpointAudio;
    public AudioClip clickAudio;
    public AudioClip hitPassengerAudio;
    public AudioClip hitTrafficAudio;
    public AudioClip hitWallAudio;
    public AudioClip jumpAudio;
    public AudioClip mapAudio;
    public AudioClip pauseAudio;
    public AudioClip resumeAudio;
    public AudioClip upgradeAudio;

    private AudioSource AS;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip ac, float volumnScale = 0.6f)
    {
        Debug.Log("play audio");
        AS.PlayOneShot(ac, volumnScale);
    }
}
