using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class SpoonWeaponProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06002B9E RID: 11166 RVA: 0x0009433C File Offset: 0x0009253C
	public virtual string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileToSpawn
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06002B9F RID: 11167 RVA: 0x00094361 File Offset: 0x00092561
	// (set) Token: 0x06002BA0 RID: 11168 RVA: 0x00094369 File Offset: 0x00092569
	public string ProjectileToSpawn
	{
		get
		{
			return this.m_projectileToSpawn;
		}
		set
		{
			this.m_projectileToSpawn = value;
		}
	}

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x00094372 File Offset: 0x00092572
	// (set) Token: 0x06002BA2 RID: 11170 RVA: 0x0009437A File Offset: 0x0009257A
	public Vector2 Offset
	{
		get
		{
			return this.m_offset;
		}
		set
		{
			this.m_offset = value;
		}
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x00094383 File Offset: 0x00092583
	private void Start()
	{
		base.SourceProjectile.OnDeathRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000943A4 File Offset: 0x000925A4
	private void SpawnProjectile(Projectile_RL projectile, GameObject colliderObj)
	{
		if (!base.enabled)
		{
			return;
		}
		if (base.SourceProjectile.LifespanTimer <= 0f)
		{
			return;
		}
		bool flag = true;
		if (colliderObj && !colliderObj.CompareTag("Platform") && !colliderObj.CompareTag("OneWay"))
		{
			flag = false;
		}
		Vector3 b = base.SourceProjectile.Midpoint;
		if (base.SourceProjectile.HitboxController.LastCollidedWith != null)
		{
			b = base.SourceProjectile.HitboxController.LastCollidedWith.ClosestPoint(base.SourceProjectile.Midpoint);
		}
		if (flag)
		{
			Vector2 velocity = base.SourceProjectile.Velocity;
			velocity.Normalize();
			Vector2 vector = base.SourceProjectile.Midpoint - b;
			vector.Normalize();
			Vector2 vector2;
			if (vector == Vector2.zero)
			{
				vector2 = velocity;
				vector2.x *= -1f;
			}
			else
			{
				Vector2 vector3 = new Vector2(vector.y, vector.x * -1f);
				vector2 = 2f * (Vector2.Dot(velocity, vector3) / Vector2.Dot(vector3, vector3)) * vector3 - velocity;
				if ((vector2.y > 0f && velocity.y > 0f && vector.x > 0f && velocity.x > 0f) || (vector2.y > 0f && velocity.y > 0f && vector.x < 0f && velocity.x < 0f))
				{
					vector2.y *= -1f;
				}
			}
			vector2.Normalize();
			float angleInDeg = CDGHelper.VectorToAngle(vector2);
			Vector2 offset = base.SourceProjectile.Midpoint;
			Projectile_RL projectile_RL = ProjectileManager.FireProjectile(base.SourceProjectile.Owner, this.m_projectileToSpawn, offset, false, angleInDeg, 0.8f, true, true, true, true);
			PlayerController playerController = base.SourceProjectile.OwnerController as PlayerController;
			if (playerController)
			{
				CastAbilityType lastCastAbilityTypeCasted = playerController.CastAbility.LastCastAbilityTypeCasted;
				playerController.CastAbility.SetLastCastAbilityTypeOverride(base.SourceProjectile.CastAbilityType);
				playerController.CastAbility.InitializeProjectile(projectile_RL);
				playerController.CastAbility.SetLastCastAbilityTypeOverride(lastCastAbilityTypeCasted);
				if (!projectile_RL.HitboxController.ResponseMethodsInitialized && colliderObj)
				{
					projectile_RL.ForceDispatchOnCollisionRelay(colliderObj);
				}
			}
			projectile_RL.CastAbilityType = base.SourceProjectile.CastAbilityType;
			projectile_RL.Strength = base.SourceProjectile.Strength;
			projectile_RL.Magic = base.SourceProjectile.Magic;
			projectile_RL.DamageMod = base.SourceProjectile.DamageMod;
			projectile_RL.ActualCritChance = base.SourceProjectile.ActualCritChance;
			projectile_RL.transform.localPosition += vector * 0.5f;
			Vector2 offset2 = this.Offset;
			if (base.SourceProjectile.IsFlipped)
			{
				offset2.x = -offset2.x;
			}
			projectile_RL.transform.localPosition += offset2;
			if (base.SourceProjectile.StatusEffectTypes != null)
			{
				foreach (StatusEffectType type in base.SourceProjectile.StatusEffectTypes)
				{
					projectile_RL.AttachStatusEffect(type, 0f);
				}
			}
			projectile_RL.StopChangeCollisionPointCoroutine();
		}
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x00094728 File Offset: 0x00092928
	private void OnDestroy()
	{
		base.SourceProjectile.OnDeathRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002377 RID: 9079
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002378 RID: 9080
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04002379 RID: 9081
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
