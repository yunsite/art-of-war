using UnityEngine;

/// <summary>
/// Represents entity containing single player gameplay informations.
/// </summary>
[ExecuteInEditMode]
public class PlayerInfo : MonoBehaviour
{
    /// <summary>
    /// Players index.
    /// </summary>
    public int Index;

    /// <summary>
    /// Represents players name.
    /// </summary>
    public string Name;

    /// <summary>
    /// References players main camera.
    /// </summary>
    public Camera MainCamera;

    /// <summary>
    /// References players minimap camera.
    /// </summary>
    public Camera MinimapCamera;

    /// <summary>
    /// Includes references to player units.
    /// </summary>
    public Unit[] Units;

    void Awake()
    {
        if (MainCamera == null || MinimapCamera == null)
        {
            Camera[] cameras = GetComponentsInChildren<Camera>();
            foreach (Camera camera in cameras)
            {
                if (MainCamera == null && camera.CompareTag("MainCamera"))
                {
                    MainCamera = camera;
                }
                else if (MinimapCamera == null)
                {
                    MinimapCamera = camera;
                }
            }
        }

        if (Units == null || Units.Length == 0)
        {
            Units = GetComponentsInChildren<Unit>();
            if (Units.Length > 0)
            {
                Index = Units[0].PlayerOwner;
            }
        }
    }
}
