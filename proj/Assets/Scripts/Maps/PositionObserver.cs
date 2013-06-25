using System;
using UnityEngine;

/// <summary>
/// Prepresents terrain position observation utility.
/// </summary>
public class PositionObserver : MonoBehaviour
{

    /// <summary>
    /// Max distance used in raycast test.
    /// </summary>
    public float raycastDistance = 1000;

    /// <summary>
    /// Event handler of terrain click.
    /// </summary>
    public event EventHandler<PositionEventArgs> Clicked;

    void OnMouseUpAsButton()
    {
        if (Clicked != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                Clicked(gameObject, new PositionEventArgs { Position = hit.point });
            }
        }
    }
}

/// <summary>
/// Terrain position click event arguments.
/// </summary>
public class PositionEventArgs : EventArgs
{
    /// <summary>
    /// Selected position.
    /// </summary>
    public Vector3 Position;
}