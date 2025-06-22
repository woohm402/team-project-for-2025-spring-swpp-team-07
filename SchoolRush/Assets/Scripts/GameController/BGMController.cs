using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public List<AudioClip> racingBGMs;
    public AudioClip aegukgaBGM;

    private AudioSource audioSource;
    private int prevBGMIndex;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        prevBGMIndex = Random.Range(0, racingBGMs.Count);
        audioSource.clip = racingBGMs[prevBGMIndex];
        audioSource.Play();

        StartCoroutine(nameof(CheckBGMEnd));
    }

    public void switchToAegukga()
    {
        audioSource.Stop();
        audioSource.clip = aegukgaBGM;
        audioSource.Play();
    }

    IEnumerator CheckBGMEnd()
    {
        while (true)
        {
            while (audioSource.isPlaying)
            {
                yield return new WaitForSecondsRealtime(2);
            }

            ChangeBGM();
        }
    }

    private void ChangeBGM(int index = -1)
    {

        if (index < 0 || index >= racingBGMs.Count)
        {
            do
            {
                index = Random.Range(0, racingBGMs.Count);
            }
            while (index == prevBGMIndex);
        }

        audioSource.Stop();
        audioSource.clip = racingBGMs[index];
        audioSource.Play();
    }
}
