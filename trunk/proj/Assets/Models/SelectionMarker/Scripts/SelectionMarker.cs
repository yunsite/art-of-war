using System;
using UnityEngine;

public class SelectionMarker : MonoBehaviour {
	
	public SelectionMode Mode;
	public Color DefaultColor = new Color(35f / 255f, 106f / 255f, 251f / 255f, 1f);
	public Color MovementColor = Color.green;
	public Color AttackColor = Color.red;
	public Color SpecialAttackColor = Color.yellow;
	public Color NoActionColor = Color.gray;
	public string MeshName = "markerMesh";
	
	private Material material;
	
	void Awake () {
		Transform meshChild = transform.FindChild(MeshName);
		if (meshChild == null) {
			Debug.LogError("No required mesch child found.");
			return;
		}
		
		SkinnedMeshRenderer mesh = meshChild.GetComponent<SkinnedMeshRenderer>();
		if (mesh == null) {
			Debug.LogError("No required mesch renderer component found.");
			return;
		}
		
		material = mesh.material;
		if (material == null) {
			Debug.LogError("No required material found.");
			return;
		}
	}
	
	void Start () {
		SetMode(Mode);
	}
	
	public Color GetModeColor (SelectionMode mode) {
		switch (mode) {
		case SelectionMode.Default:
			return DefaultColor;
		case SelectionMode.Attack:
			return AttackColor;
		case SelectionMode.Movement:
			return MovementColor;
		case SelectionMode.SpecialAbility:
			return SpecialAttackColor;
		case SelectionMode.NoAction:
			return NoActionColor;
		default:
			throw new InvalidProgramException("Invalid program path reached.");
		}
	}
	
	public void SetMode (SelectionMode mode) {
		material.color = GetModeColor(mode);
	}
}

public enum SelectionMode { Default, Movement, Attack, SpecialAbility, NoAction };