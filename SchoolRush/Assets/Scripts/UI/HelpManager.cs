using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    public GameObject HelpImage;

    void Start()
    {
        if (HelpImage != null)
        {
            HelpImage.SetActive(false);
        }
    }

    public void ToggleImage()
    {
        if (HelpImage != null)
        {
            HelpImage.SetActive(!HelpImage.activeSelf);
        }
    }
}
