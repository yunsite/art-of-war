using UnityEngine;

/// <summary>
/// Camera controller moving forward, backward, sideways and rotating yaw and pitch.
/// </summary>
public class StrategyLook : MonoBehaviour
{
    /// <summary>
    /// Speed od camera motion.
    /// </summary>
    public float motionSpeed = 2f;

    /// <summary>
    /// Speed of camera rotation.
    /// </summary>
    public float rotationSpeed = 15f;

    /// <summary>
    /// Minimum camera yaw angle.
    /// </summary>
    public float minYaw = -15f;

    /// <summary>
    /// Maximum camera yaw angle.
    /// </summary>
    public float maxYaw = 15f;

    private float yaw;

    void Start()
    {
        yaw = -transform.localEulerAngles.x;
    }

    void Update()
    {
        Vector3 movement = transform.forward;
        movement.y = 0;
        movement = Input.GetAxis("Vertical") * movement.normalized
            + transform.right * Input.GetAxis("Horizontal");
        transform.Translate(movement.normalized * motionSpeed, Space.World);

        Vector3 pos = transform.position;
        float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");
        pos.y += mouse_wheel * 10;
        if (Terrain.activeTerrain != null && pos.y < Terrain.activeTerrain.SampleHeight(pos) + 5)
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + 5;
        transform.position = pos;

        if (Input.GetButton("Fire2"))
        {
            Vector3 euler = transform.localEulerAngles;
            yaw = Mathf.Clamp(yaw + Input.GetAxis("Mouse Y") * rotationSpeed, minYaw, maxYaw);
            euler.x = -yaw;
            euler.y += Input.GetAxis("Mouse X") * rotationSpeed;
            transform.localEulerAngles = euler;
        }
    }
}
