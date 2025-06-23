using UnityEngine;
using System.Collections;

public class AnimationDelay : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Start()
    {
        StartCoroutine(PlayAnimationAfterLoad());
    }

    IEnumerator PlayAnimationAfterLoad()
    {
        yield return new WaitForEndOfFrame();
        animator.enabled = true;
    }
}
