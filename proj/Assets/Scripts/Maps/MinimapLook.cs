using UnityEngine;

/// <summary>
/// Script representing minimap camera following main camera target.
/// </summary>
public class MinimapLook : MonoBehaviour
{
    private Transform selfTransform;

    /// <summary>
    /// Main camera target.
    /// </summary>
    public Transform target;

    void Awake()
    {
        selfTransform = transform;
    }

    void Update()
    {
        selfTransform.position = target.position;
    }
}
