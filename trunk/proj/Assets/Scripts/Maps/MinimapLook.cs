using UnityEngine;

public class MinimapLook : MonoBehaviour
{
    private Transform selfTransform;
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
