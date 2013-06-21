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
        Destroy(renderer);
        explosion.Play();
        Unit target = other.gameObject.GetComponent<Unit>();
        if (target != null && attacker.AttackStatistics.Area == AttackAreaEnum.Unit)
        {
            target.GetDamadge(damage, attacker);
        }
        else if (attacker.AttackStatistics.Area == AttackAreaEnum.Field)
        {
            RaycastHit[] results = Physics.SphereCastAll(transform.position, damage, transform.up);
            foreach (RaycastHit hit in results)
            {
                target = hit.collider.GetComponent<Unit>();
                if (target != null)
                {
                    Debug.Log("Target " + target + " " + target.PlayerOwner + " damage" + (damage - Vector3.Distance(transform.position, target.transform.position)));
                    target.GetDamadge(damage - Vector3.Distance(transform.position, target.transform.position), attacker);
                }
            }
        }

        yield return new WaitForSeconds(explosion.duration);
        Destroy(gameObject);
    }
}
