using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base class of all units in game.
/// </summary>
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour
{
    /// <summary>
    /// Transform component reference.
    /// </summary>
    protected Transform selfTransform;

    void Awake()
    {
        selfTransform = transform;
    }

    /// <summary>
    /// Type of unit.
    /// </summary>
    public UnitTypeEnum UnitType;

    #region Selection items
    /// <summary>
    /// Index of player owns unit.
    /// </summary>
    public int PlayerOwner;

    /// <summary>
    /// Selector script reference.
    /// </summary>
    public SelectionMarker Selector;

    /// <summary>
    /// Action range projector reference.
    /// </summary>
    public RangeProjector RangeProjector;

    /// <summary>
    /// Owner unit minimap icon reference.
    /// </summary>
    public GameObject ownerIcon;

    /// <summary>
    /// Enemy unit minimap icon reference.
    /// </summary>
    public GameObject enemyIcon;

    /// <summary>
    /// Selects unit for no action.
    /// </summary>
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

    /// <summary>
    /// Selects unit for movement action.
    /// </summary>
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

    /// <summary>
    /// Selects unit for attack.
    /// </summary>
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

    /// <summary>
    /// Selects unit as target for helicopter special ability.
    /// </summary>
    public void SelectAsTargetForHelicopterSpecial()
    {
        SelectRange(SelectionMode.TargetForHelicopter, 0.0f);
    }

    /// <summary>
    /// Selects unit for special ability.
    /// </summary>
    public virtual void SelectSpecialAbility() { }

    /// <summary>
    /// Selects unit with specified mode and range radius.
    /// </summary>
    /// <param name="mode">Selection mode.</param>
    /// <param name="range">Radius of range. If zero then no range displayed.</param>
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

    /// <summary>
    /// Deselects unit.
    /// </summary>
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

    #region Turns items
    /// <summary>
    /// Triggers actions for begin of turn.
    /// </summary>
    public virtual void BeginTurn()
    {
        SwitchToOwnerIcon();
    }

    /// <summary>
    /// Triggers actions for end of turn.
    /// </summary>
    public virtual void EndTurn()
    {
        SwitchToEnemyIcon();
        ResetMovementPoints();
        ResetAttackPoints();
    }
    #endregion

    #region Movement items
    /// <summary>
    /// Contains unit movement properties.
    /// </summary>
    [Serializable]
    public class UnitMovementStatistics
    {
        /// <summary>
        /// Remaining movement range.
        /// </summary>
        public float RemainingRange = 40;
        
        /// <summary>
        /// Total movement range per turn.
        /// </summary>
        public float TotalRange = 40;

        /// <summary>
        /// Difficult terrain movement ability level.
        /// </summary>
        public DifficultTerrainMoveAbilityEnum DifficultTerrainMoveAbility;

        /// <summary>
        /// Maximum target radius treshold.
        /// </summary>
        public float TargetRadius = 2f;

        /// <summary>
        /// Speed of units straight line motion.
        /// </summary>
        public float MotionSpeed = 5f;

        /// <summary>
        /// Speed of units rotation.
        /// </summary>
        public float RotationSpeed = 5f;
    }

    /// <summary>
    /// Unit movement properties.
    /// </summary>
    public UnitMovementStatistics MovementStatistics = new UnitMovementStatistics();

    /// <summary>
    /// Starts movement process.
    /// </summary>
    /// <param name="worldPosition">Target position.</param>
    public virtual void MoveToPosition(Vector3 worldPosition)
    {
        float distance = (worldPosition - selfTransform.position).magnitude;
        if (CanMove(worldPosition))
        {
            MovementStatistics.RemainingRange -= distance;
            StartCoroutine(ProcessMotion(worldPosition));
        }
    }

    /// <summary>
    /// Returns true if unit can move at all.
    /// </summary>
    /// <returns>Movement ability indicator.</returns>
    public bool CanMove()
    {
        return MovementStatistics.RemainingRange > MovementStatistics.TargetRadius;
    }

    /// <summary>
    /// Returns true if unit can move to specified target.
    /// </summary>
    /// <param name="targetPosition">Target point.</param>
    /// <returns>Movement ability indicator.</returns>
    public bool CanMove(Vector3 targetPosition)
    {
        float distance = (targetPosition - selfTransform.position).magnitude;
        return CanMove() && MovementStatistics.RemainingRange >= distance;
    }

    private void ResetMovementPoints()
    {
        MovementStatistics.RemainingRange = MovementStatistics.TotalRange;
    }
	
	private void ResetAttackPoints()
	{
		AttackStatistics.RemainingQuantity = AttackStatistics.TotalQuantity;
	}

    IEnumerator ProcessMotion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        animation.CrossFade("forward");
        yield return new WaitForFixedUpdate();
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

    /// <summary>
    /// Contains attack properties.
    /// </summary>
    [Serializable]
    public class UnitAttackStatistics
    {
        /// <summary>
        /// Remaining attack quantity.
        /// </summary>
        public int RemainingQuantity = 2;

        /// <summary>
        /// Total attack quantity per turn.
        /// </summary>
        public int TotalQuantity = 2;

        /// <summary>
        /// Power of attack.
        /// </summary>
        public float Power = 5;

        /// <summary>
        /// Attack range radius.
        /// </summary>
        public float Range = 60;

        /// <summary>
        /// Attack area.
        /// </summary>
        public AttackAreaEnum Area;

        /// <summary>
        /// Emited projectile prefab object.
        /// </summary>
        public ProjectileController ProjectilePrefab;

        /// <summary>
        /// Turret root transform.
        /// </summary>
        public Transform Turrent;

        /// <summary>
        /// Cannon root transform.
        /// </summary>
        public Transform Cannon;

        /// <summary>
        /// Cannon tip transform.
        /// </summary>
        public Transform CannonTip;

        /// <summary>
        /// Speed of target aim.
        /// </summary>
        public float AimSpeed = 5f;

        /// <summary>
        /// Initial speed of projectile.
        /// </summary>
        public float ProjectileSpeed = 100f;
    }

    /// <summary>
    /// Attack properties.
    /// </summary>
    public UnitAttackStatistics AttackStatistics = new UnitAttackStatistics();

    /// <summary>
    /// Starts procedure of unit attack on enemy.
    /// </summary>
    /// <param name="enemy">Enemy attacked.</param>
    public virtual void Attack(Unit enemy)
    {
        if (CanAttack(enemy.transform.position))
        {
            --AttackStatistics.RemainingQuantity;
            StartCoroutine(ProcessAttack(enemy.transform.position));
        }
    }

    /// <summary>
    /// Starts procedure of field attack on target point.
    /// Not supported by default. Available only in Artillery unit.
    /// </summary>
    /// <param name="target">Target point.</param>
    public virtual void Attack(Vector3 target)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Returns true if there is a target unit can attack.
    /// </summary>
    /// <returns>Attack ability indicator.</returns>
    public bool CanAttack()
    {
        return AttackStatistics.RemainingQuantity > 0;
    }

    /// <summary>
    /// Returns true if specified point can be attacked.
    /// </summary>
    /// <param name="targetPosition">Target point.</param>
    /// <returns>Attack ability indicator.</returns>
    public bool CanAttack(Vector3 targetPosition)
    {
        float distance = (targetPosition - selfTransform.position).magnitude;
        return CanAttack() && distance <= AttackStatistics.Range;
    }

    /// <summary>
    /// Processes attack action.
    /// </summary>
    /// <param name="target">Target point.</param>
    /// <returns>Returning IEnumerator supports reentrant functions used by Unity Engine.</returns>
    protected internal IEnumerator ProcessAttack(Vector3 target)
    {
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
        Vector3 turretDirection = target - AttackStatistics.Turrent.position;
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
        Vector3 direction = (target - AttackStatistics.Cannon.position).normalized;
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

    #region Special ability items
    /// <summary>
    /// Starts procedure of using special ability.
    /// Not supported for base class.
    /// </summary>
    public virtual void UseSpecial()
    {
        throw new NotSupportedException();
    }
    #endregion

    #region Health items
    /// <summary>
    /// Contains unit health and deffence properties.
    /// </summary>
    [Serializable]
    public class UnitHealthStatistics
    {
        /// <summary>
        /// Remaining health points.
        /// </summary>
        public float RemainingPoints = 100;

        /// <summary>
        /// Total health points.
        /// </summary>
        public float TotalPoints = 100;

        /// <summary>
        /// Unit deffence retio.
        /// </summary>
        public float Deffence = 4;
    }

    /// <summary>
    /// Unit health and deffence properties.
    /// </summary>
    public UnitHealthStatistics HealthStatistics = new UnitHealthStatistics();

    /// <summary>
    /// Processes damage receive.
    /// </summary>
    /// <param name="damage">Amount of damage received.</param>
    /// <param name="attacker">Attacking unit.</param>
    /// <returns>Amount of damage applied.</returns>
    public float GetDamadge(float damage, Unit attacker)
    {
        HealthStatistics.RemainingPoints -= damage;
        if (HealthStatistics.RemainingPoints <= 0) Die();
        return HealthStatistics.RemainingPoints;
    }

    /// <summary>
    /// Processes unit destruction.
    /// </summary>
    public void Die()
    {
		GameObject det = Instantiate(Resources.Load("Prefab_Detonator"), transform.position, Quaternion.identity) as GameObject;
		Destroy(det, 20);
        Destroy(gameObject);
    }
    #endregion

    #region Events
    /// <summary>
    /// Unit clicked event.
    /// </summary>
    public event EventHandler Clicked;

    /// <summary>
    /// Raises unit clicked event.
    /// </summary>
    /// <param name="sender">Clicked unit.</param>
    /// <param name="e">Empty args.</param>
    protected void OnClicked(object sender, EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(sender, e);
        }
    }

    void OnMouseUpAsButton()
    {
        OnClicked(this, EventArgs.Empty);
    }

    /// <summary>
    /// Action execution completed event.
    /// </summary>
    public event EventHandler ActionCompleted;

    /// <summary>
    /// Raises action execution event.
    /// </summary>
    protected void OnActionCompleted()
    {
        if (ActionCompleted != null)
            ActionCompleted(this, EventArgs.Empty);
    }
    #endregion
}

/// <summary>
/// Type of unit enumeration.
/// </summary>
public enum UnitTypeEnum
{
    IFV, Tank, HeavyTank, Helicopter, Artillery
}

/// <summary>
/// Difficult terrain move ability level enumeration.
/// </summary>
public enum DifficultTerrainMoveAbilityEnum
{
    Poor, Medium, Good
}

/// <summary>
/// Types of attack enumeration.
/// </summary>
public enum AttackAreaEnum
{
    Unit, Field
}
