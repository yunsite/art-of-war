using UnityEngine;
using System.Collections;

public class MotionController : MonoBehaviour {

    public float targetRadius = 2f;
    public float speed = 20f;
    public float angularSpeed = 5f;

    public void Move(Vector3 target)
    {
        StartCoroutine(AirVehicleMotion(target));
    }

    IEnumerator LandVehicleMotion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        animation.CrossFade("forward");
        while (distance > targetRadius)
        {
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (Vector3.Dot(transform.forward, direction) < 0) cross.Normalize();
            rigidbody.angularVelocity = cross * angularSpeed * Mathf.Min(distance / targetRadius, 1);
            rigidbody.velocity = transform.forward * Mathf.Min(distance * targetRadius, speed);
            audio.volume = rigidbody.velocity.magnitude / speed;
            yield return new WaitForFixedUpdate();
            offset = target - transform.position;
            offset.y = 0;
            direction = offset.normalized;
            distance = offset.magnitude;
        }

        animation.CrossFade("none");
        audio.Stop();
    }

    IEnumerator AirVehicleMotion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        while (distance > targetRadius)
        {
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (Vector3.Dot(transform.forward, direction) < 0) cross.Normalize();
            rigidbody.angularVelocity = cross * Mathf.Min(distance / targetRadius, 1) * angularSpeed;
            rigidbody.velocity = transform.forward * Mathf.Min(distance * targetRadius, speed);
            audio.volume = rigidbody.velocity.magnitude / speed;
            yield return new WaitForFixedUpdate();
            offset = target - transform.position;
            offset.y = 0;
            direction = offset.normalized;
            distance = offset.magnitude;
        }

        audio.Stop();
    }
}
