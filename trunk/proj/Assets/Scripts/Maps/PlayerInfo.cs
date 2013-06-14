using UnityEngine;

[ExecuteInEditMode]
public class PlayerInfo : MonoBehaviour
{
    public int Index;
    public string Name;
    public Camera MainCamera;
    public Camera MinimapCamera;
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
