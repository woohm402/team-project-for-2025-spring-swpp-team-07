using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Turns Car AI on and off based on distance from player-camera.
/// </summary>
[RequireComponent(typeof(CarAIController), typeof(Rigidbody))]
public class DistanceBasedCarActivator : MonoBehaviour
{
    [Tooltip("AI will be activated when within this distance.")]
    public float activateDistance = 200f;
    [Tooltip("AI will be deactivated when beyond this distance. (hysteresis)")]
    public float deactivateDistance = 240f;

    Transform player;
    CarAIController ai;
    Rigidbody rb;
    Renderer[] renderers;
    Collider[] colliders;
    WheelCollider[] wheelColliders;
    bool isActive = true;

    void Awake()
    {
        player          = Camera.main.transform; // Replace with player transform if needed
        ai              = GetComponent<CarAIController>();
        rb              = GetComponent<Rigidbody>();
        renderers       = GetComponentsInChildren<Renderer>(true);
        colliders       = GetComponentsInChildren<Collider>(true);
        wheelColliders  = GetComponentsInChildren<WheelCollider>(true);
    }

    void Update()
    {
        float sqr = (player.position - transform.position).sqrMagnitude;

        if (isActive && sqr > deactivateDistance * deactivateDistance)
            SetActive(false);
        else if (!isActive && sqr < activateDistance * activateDistance)
            SetActive(true);
    }

    void SetActive(bool state)
    {
        if (state == isActive) return;
        isActive = state;

        // ───────── Visual + Physics ─────────
        foreach (var r in renderers)  r.enabled        = state;
        foreach (var c in colliders)  c.enabled        = state;
        foreach (var w in wheelColliders) w.enabled    = state;   // Trigger checkpoints also disabled

        rb.isKinematic = !state;            // Stop Rigidbody physics when inactive
        ai.enabled   = state;               // Stop AI logic

        // Remove remaining torque & brake when stopping
        if (!state)
        {
            ai.Break(0);
            ai.Accelerate(0);
        }
    }
}
