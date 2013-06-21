using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour
{
    protected Transform selfTransform;
    protected bool isBusy = false;

    void Awake()
    {
        selfTransform = transform;
    }

    public UnitTypeEnum UnitType;

    #region Selection items

    public int PlayerOwner;
    public SelectionMarker Selector;
    public RangeProjector RangeProjector;
    public GameObject ownerIcon;
    public GameObject enemyIcon;

    public void Select()
    {
        SelectionMode mode;
        if (CanMove() || CanAttack())
        {
            mode = SelectionMode.Default;
        }
        else
        {
            mode = SelectionMode.NoAction;
        }

        SelectRange(mode, 0);
    }

    public void SelectMovement()
    {
        SelectionMode mode;
        float range;
        if (CanMove())
        {
            mode = SelectionMode.Movement;
            range = MovementStatistics.RemainingRange;
        }
        else
        {
            mode = SelectionMode.NoAction;
            range = MovementStatistics.TotalRange;
        }

        SelectRange(mode, range);
    }
    public void SelectAttack()
    {
        SelectionMode mode;
        if (CanAttack())
        {
            mode = SelectionMode.Attack;
        }
        else
        {
            mode = SelectionMode.NoAction;
        }

        SelectRange(mode, AttackStatistics.Range);
    }

    public void SelectAsTargetForHelicopterSpecial()
    {
        SelectRange(SelectionMode.TargetForHelicopter, 0.0f);
    }

    public virtual void SelectSpecialAbility()
    {
        /*
         * Nie bardzo rozumiem tą implementację więc ją zakomentowywuję
         * zmieniam metode na virtualną (Marek Kokot) (dot. issue 0056)
        SelectionMode mode;
        if (CanAttack())
        {
            mode = SelectionMode.SpecialAbility;
        }
        else
        {
            mode = SelectionMode.NoAction;
        }

        SelectRange(mode, AttackStatistics.Range);
        */
    }

    protected void SelectRange(SelectionMode mode, float range)
    {

        Selector.gameObject.SetActive(true);
        Selector.SetMode(mode);
        if (range > 0)
        {
            RangeProjector.gameObject.SetActive(true);
            RangeProjector.SetColor(Selector.GetModeColor(mode));
            RangeProjector.SetRange(range);
        }
        else
        {
            RangeProjector.gameObject.SetActive(false);
        }
    }

    public void Deselect()
    {
        Selector.gameObject.SetActive(false);
        RangeProjector.gameObject.SetActive(false);
    }

    private void SwitchToOwnerIcon()
    {
        Debug.Log(this + " owner");
        enemyIcon.SetActive(false);
        ownerIcon.SetActive(true);
    }

    private void SwitchToEnemyIcon()
    {
        Debug.Log(this + " enemy");
        ownerIcon.SetActive(false);
        enemyIcon.SetActive(true);
    }

    #endregion

    #region Movement items

    [Serializable]
    public class UnitMovementStatistics
    {
        public float RemainingRange = 40;
        public float TotalRange = 40;
        public DifficultTerrainMoveAbilityEnum DifficultTerrainMoveAbility;
        public float TargetRadius = 2f;
        public float MotionSpeed = 5f;
        public float RotationSpeed = 5f;
    }

    public UnitMovementStatistics MovementStatistics = new UnitMovementStatistics();

    public virtual void MoveToPosition(Vector3 worldPosition)
    {
        float distance = (worldPosition - selfTransform.position).magnitude;
        if (!isBusy && CanMove(worldPosition))
        {
            MovementStatistics.RemainingRange -= distance;
            StartCoroutine(ProcessMotion(worldPosition));
        }
    }

    public bool CanMove()
    {
        return MovementStatistics.RemainingRange > MovementStatistics.TargetRadius;
    }

    public bool CanMove(Vector3 targetPosition)
    {
        float distance = (targetPosition - selfTransform.position).magnitude;
        return CanMove() && MovementStatistics.RemainingRange >= distance;
    }

    public void ResetMovementPoints()
    {
        MovementStatistics.RemainingRange = MovementStatistics.TotalRange;
    }

    IEnumerator ProcessMotion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        animation.CrossFade("forward");
        while (distance > MovementStatistics.TargetRadius)
        {
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (Vector3.Dot(transform.forward, direction) < 0) cross.Normalize();
            rigidbody.angularVelocity =
                cross * MovementStatistics.RotationSpeed * Mathf.Min(distance / MovementStatistics.TargetRadius, 1);
            rigidbody.velocity =
                transform.forward * Mathf.Min(distance * MovementStatistics.TargetRadius, MovementStatistics.MotionSpeed);
            audio.volume = rigidbody.velocity.magnitude / MovementStatistics.MotionSpeed;
            yield return new WaitForFixedUpdate();
            offset = target - transform.position;
            offset.y = 0;
            direction = offset.normalized;
            distance = offset.magnitude;
        }

        animation.CrossFade("none");
        audio.Stop();
        OnActionCompleted();
    }

    #endregion

    #region Attack items

    [Serializable]
    public class UnitAttackStatistics
    {
        public int RemainingQuantity = 2;
        public int TotalQuantity = 2;
        public float Power = 5;
        public float Range = 60;
        public AttackAreaEnum Area;
        public ProjectileController ProjectilePrefab;
        public Transform Turrent;
        public Transform Cannon;
        public Transform CannonTip;
        public float AimSpeed = 5f;
        public float ProjectileSpeed = 100f;
    }

    public UnitAttackStatistics AttackStatistics = new UnitAttackStatistics();

    public virtual void Attack(Unit enemy)
    {
        Debug.Log("Unit.Attack(Unit enemy)");
        if (!isBusy && CanAttack(enemy.transform.position))
        {
            --AttackStatistics.RemainingQuantity;
            StartCoroutine(ProcessAttack(enemy));
        }
    }

    public void Attack(Vector3 target)
    {
        throw new NotSupportedException();
    }

    public bool CanAttack()
    {
        return AttackStatistics.RemainingQuantity > 0;
    }

    public bool CanAttack(Vector3 targetPosition)
    {
        float distance = (targetPosition - selfTransform.position).magnitude;
        return CanAttack() && distance <= AttackStatistics.Range;
    }

    IEnumerator ProcessAttack(Unit enemy)
    {
        Vector3 target = enemy.transform.position;
        Quaternion turretBackup = AttackStatistics.Turrent.rotation;
        yield return StartCoroutine(AimTurret(target));
        Debug.Log("Turret aimed");
        Quaternion cannonBackup = AttackStatistics.Cannon.rotation;
        yield return StartCoroutine(AimCannon(target));
        Debug.Log("Cannon aimed");
        bool gravity = rigidbody.useGravity;
        rigidbody.useGravity = false;
        collider.isTrigger = true;
        Debug.Log("Befor emiting");
        EmitProjectile();
        Debug.Log("Projectile emited");
        yield return StartCoroutine(BackupCannon(cannonBackup));
        yield return StartCoroutine(BackupTurret(turretBackup));
        collider.isTrigger = false;
        rigidbody.useGravity = gravity;
        OnActionCompleted();
    }

    IEnumerator AimTurret(Vector3 target)
    {
        Vector3 turretDirection = target - transform.position;
        turretDirection.y = 0;
        turretDirection.Normalize();

        Quaternion turretRotation = Quaternion.LookRotation(
            AttackStatistics.Turrent.forward, Vector3.Cross(turretDirection, AttackStatistics.Turrent.forward));
        while (Mathf.Abs(Quaternion.Dot(AttackStatistics.Turrent.rotation, turretRotation)) < 1f)
        {
            AttackStatistics.Turrent.rotation = Quaternion.Slerp(
                AttackStatistics.Turrent.rotation, turretRotation, Time.deltaTime * AttackStatistics.AimSpeed);
            yield return null;
        }
    }

    IEnumerator AimCannon(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion cannonRotation = Quaternion.LookRotation(
            Vector3.Cross(-direction, AttackStatistics.Cannon.up), AttackStatistics.Cannon.up);
        while (Mathf.Abs(Quaternion.Dot(AttackStatistics.Cannon.rotation, cannonRotation)) < 1f)
        {
            AttackStatistics.Cannon.rotation = Quaternion.Slerp(
                AttackStatistics.Cannon.rotation, cannonRotation, Time.deltaTime * AttackStatistics.AimSpeed);
            yield return null;
        }
    }

    void EmitProjectile()
    {
        ProjectileController projectile = (ProjectileController)Instantiate(
            AttackStatistics.ProjectilePrefab, AttackStatistics.CannonTip.position, AttackStatistics.CannonTip.rotation);
        projectile.rigidbody.velocity = AttackStatistics.CannonTip.forward * AttackStatistics.ProjectileSpeed;
        projectile.damage = AttackStatistics.Power;
        projectile.attacker = this;
    }

    IEnumerator BackupCannon(Quaternion cannonBackup)
    {
        while (Mathf.Abs(Quaternion.Dot(AttackStatistics.Cannon.rotation, cannonBackup)) < 1f)
        {
            AttackStatistics.Cannon.rotation = Quaternion.Slerp(
                AttackStatistics.Cannon.rotation, cannonBackup, Time.deltaTime * AttackStatistics.AimSpeed);
            yield return null;
        }
    }

    IEnumerator BackupTurret(Quaternion turretBackup)
    {
        while (Mathf.Abs(Quaternion.Dot(AttackStatistics.Turrent.rotation, turretBackup)) < 1f)
        {
            AttackStatistics.Turrent.rotation = Quaternion.Slerp(
                AttackStatistics.Turrent.rotation, turretBackup, Time.deltaTime * AttackStatistics.AimSpeed);
            yield return null;
        }
    }

    #endregion

    #region Health items

    [Serializable]
    public class UnitHealthStatistics
    {
        public float RemainingPoints = 100;
        public float TotalPoints = 100;
        public float Deffence = 4;
    }

    public UnitHealthStatistics HealthStatistics = new UnitHealthStatistics();

    public float GetDamadge(float damage, Unit attacker)
    {
        // Na razie podstawowa funkcjonalność.
        HealthStatistics.RemainingPoints -= damage;
        if (HealthStatistics.RemainingPoints <= 0) Die();
        return HealthStatistics.RemainingPoints; // Co ma być wartością zwracana ?
    }

    //nie wiem jak implementowac tu pewnie maja leciec jakies efekty umierania najpierw
    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Events

    public event EventHandler Clicked;
    protected void OnClicked(object sender, EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(sender, e);
        }
    }

    void OnMouseUpAsButton()
    {
        OnClicked(this, new EventArgs());
    }

    public event EventHandler ActionCompleted;
    protected void OnActionCompleted()
    {
        if (ActionCompleted != null)
            ActionCompleted(this, new EventArgs());
    }
    #endregion

    #region Public Methods

    public virtual void BeginTurn()
    {
        SwitchToOwnerIcon();
    }

    public virtual void EndTurn()
    {
        SwitchToEnemyIcon();
        ResetMovementPoints();
    }
    
    public virtual void UseSpecial()
    {
        throw new NotSupportedException();
    }

    #endregion
}

public enum UnitTypeEnum
{
    IFV, Tank, HeavyTank, Helicopter, Artillery
}

public enum DifficultTerrainMoveAbilityEnum
{
    Poor, Medium, Good
}

public enum AttackAreaEnum
{
    Unit, Field
}
