using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float damage;
    public Unit attacker;
    public ParticleSystem explosion;

    IEnumerator OnCollisionEnter(Collision other)
    {
        Destroy(rigidbody);
        Destroy(collider);
        explosion.Play();
        Unit target = other.gameObject.GetComponent<Unit>();
        if (target != null)
        {
            target.GetDamadge(damage, attacker);
        }

        yield return new WaitForSeconds(explosion.duration);
        Destroy(gameObject);
    }
}
