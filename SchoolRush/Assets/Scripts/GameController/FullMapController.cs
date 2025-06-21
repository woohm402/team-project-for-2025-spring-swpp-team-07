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

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (!cam.isActiveAndEnabled) return;

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

        bool right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
        bool left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        bool down = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
        bool up = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        int r = right ? 1 : 0;
        int l = left ? 1 : 0;
        int d = down ? 1 : 0;
        int u = up ? 1 : 0;

        transform.Translate(cam.fieldOfView * moveSpeed * Time.unscaledDeltaTime * new Vector3(r - l, 0, u - d), Space.World);

    }

    private void OnEnable()
    {
        transform.position = pos0;
        cam.fieldOfView = fov0;
    }

}
