using UnityEngine;

[ExecuteInEditMode]
public class PlayerInfo : MonoBehaviour
{
    public int Index;
    public string Name;
    public Camera MainCamera;
    public Unit[] Units;

    void Awake()
    {
        if (MainCamera == null)
        {
            MainCamera = GetComponentInChildren<Camera>();
        }

        if (Units == null || Units.Length == 0)
        {
            InferUnits();
        }
    }

    private void InferUnits()
    {
        Units = GetComponentsInChildren<Unit>();
        if (Units.Length > 0)
        {
            Index = Units[0].PlayerOwner;
        }
    }
}
