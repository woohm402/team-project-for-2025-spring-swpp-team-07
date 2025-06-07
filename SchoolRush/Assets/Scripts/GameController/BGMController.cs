using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource defaultBGM;
    public AudioSource aegukgaBGM;

    // Start is called before the first frame update
    void Start()
    {
        defaultBGM.Play();
    }

    public void switchToAegukga()
    {
        defaultBGM.Stop();
        aegukgaBGM.Play();
    }
}
