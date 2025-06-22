using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapController : MonoBehaviour
{
    private Camera cam;
    private Vector3 pos0 = new(0, 3829, -454);
    private const float fov0 = 80f;
    private const float fovSpeed = 40f;
    private const float moveSpeed = 60f;

    private bool hasInited = false;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!cam.isActiveAndEnabled) { hasInited = false; return; }

        if (!hasInited) { Init(); hasInited = true;}

        bool expand = Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus);
        bool shrink = Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus);

        if (expand ^ shrink)
        {
            if (expand)
            {
                cam.fieldOfView -= fovSpeed * Time.unscaledDeltaTime;
                cam.fieldOfView = Mathf.Max(cam.fieldOfView, 1f);
            }
            else
            {
                cam.fieldOfView += fovSpeed * Time.unscaledDeltaTime;
                cam.fieldOfView = Mathf.Min(cam.fieldOfView, 100f);
            }
        }

        int right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;
        int left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0;
        int down = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0;
        int up = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0;

        transform.Translate(cam.fieldOfView * moveSpeed * Time.unscaledDeltaTime 
            * new Vector3(right - left, 0, up - down), Space.World);
    }

    private void Init()
    {
        transform.position = pos0;
        cam.fieldOfView = fov0;
    }
}
