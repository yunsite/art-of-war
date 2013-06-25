using UnityEngine;

/// <summary>
/// Base clas of all ui elements with show/hide utility.
/// </summary>
public class UILogic : MonoBehaviour
{
    /// <summary>
    /// Show underlying game object.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide underlying game object.
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
