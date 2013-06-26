using UnityEngine;

/// <summary>
/// Script animating tracks.
/// </summary>
[RequireComponent(typeof(Animation))]
public class TracksAnimation : MonoBehaviour
{
    private Material leftTrackMaterial;
    private Material rightTrackMaterial;
    private new Animation animation;

    /// <summary>
    /// Forward clip animation name.
    /// </summary>
    public string forwardClipName = "forward";

    /// <summary>
    /// Backward clip animation name.
    /// </summary>
    public string backwardClipName = "backward";

    /// <summary>
    /// Turn left clip animation name.
    /// </summary>
    public string turnLeftClipName = "turnLeft";

    /// <summary>
    /// Turn right clip animation name.
    /// </summary>
    public string turnRightClipName = "turnRight";

    /// <summary>
    /// Animation speed.
    /// </summary>
    public float speed = 1.5f;

    /// <summary>
    /// Mesh name.
    /// </summary>
    public string meshName = "hullMesh";

    /// <summary>
    /// Left track material name.
    /// </summary>
    public string leftTrackMaterialName = "leftTrackMaterial";

    /// <summary>
    /// Left track speed trimmer.
    /// </summary>
    public float leftSpeedTrim = 1.0f;

    /// <summary>
    /// Right track material name.
    /// </summary>
    public string rightTrackMaterialName = "rightTrackMaterial";

    /// <summary>
    /// Right track speed trimmer.
    /// </summary>
    public float rightSpeedTrim = 1.0f;

    void Awake()
    {
        animation = base.animation;

        Transform hull = transform.FindChild(meshName);
        SkinnedMeshRenderer renderer = hull.GetComponent<SkinnedMeshRenderer>();
        foreach (Material material in renderer.materials)
        {
            if (material.name == leftTrackMaterialName + " (Instance)")
            {
                if (material.shader.name != "Diffuse")
                    Debug.LogWarning("TracksAnimation Script works with Diffuse materials only");
                leftTrackMaterial = material;
            }
            else if (material.name == rightTrackMaterialName + " (Instance)")
            {
                if (material.shader.name != "Diffuse")
                    Debug.LogWarning("TracksAnimation Script works with Diffuse materials only");
                rightTrackMaterial = material;
            }
        }
    }

    void Update()
    {
        float forwardWeight = animation[forwardClipName].weight;
        float backwardWeight = animation[backwardClipName].weight;
        float turnLeftWeight = animation[turnLeftClipName].weight;
        float turnRightWeight = animation[turnRightClipName].weight;

        float leftDirection = forwardWeight - backwardWeight - turnLeftWeight + turnRightWeight;
        float rightDirection = forwardWeight - backwardWeight + turnLeftWeight - turnRightWeight;

        Vector2 offset = rightTrackMaterial.mainTextureOffset;
        offset.x += speed * rightSpeedTrim * rightDirection * Time.deltaTime;
        rightTrackMaterial.mainTextureOffset = offset;
        offset = leftTrackMaterial.mainTextureOffset;
        offset.x -= speed * leftSpeedTrim * leftDirection * Time.deltaTime;
        leftTrackMaterial.mainTextureOffset = offset;
    }
}
