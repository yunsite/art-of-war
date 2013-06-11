using System;
using UnityEngine;

public class ButtonLogic : MonoBehaviour {

    private UIButton uiButton;
    private UIButtonScale uiButtonScale;
    private UIButtonOffset uiButtonOffset;
    private UIButtonSound uiButtonSound;

	public EventHandler ButtonClicked;

    void Awake()
    {
        uiButton = GetComponent<UIButton>();
        uiButtonScale = GetComponent<UIButtonScale>();
        uiButtonOffset = GetComponent<UIButtonOffset>();
        uiButtonSound = GetComponent<UIButtonSound>();
    }
	
	void OnClick () {
		if (enabled && ButtonClicked != null) {
			ButtonClicked (gameObject, new EventArgs());
		}
	}

    public void Enable()
    {
        if (uiButton) uiButton.enabled = true;
        if (uiButtonScale) uiButtonScale.enabled = true;
        if (uiButtonOffset) uiButtonOffset.enabled = true;
        if (uiButtonSound) uiButtonSound.enabled = true;
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        if (uiButtonSound) uiButtonSound.enabled = false;
        if (uiButtonOffset) uiButtonOffset.enabled = false;
        if (uiButtonScale) uiButtonScale.enabled = false;
        if (uiButton) uiButton.enabled = false;
    }
	
}
