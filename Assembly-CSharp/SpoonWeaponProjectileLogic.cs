using System;
using UnityEngine;

// Token: 0x020007BB RID: 1979
public class SpoonWeaponProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x170015FF RID: 5631
	// (get) Token: 0x06003C1D RID: 15389 RVA: 0x000212FF File Offset: 0x0001F4FF
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

	// Token: 0x17001600 RID: 5632
	// (get) Token: 0x06003C1E RID: 15390 RVA: 0x00021324 File Offset: 0x0001F524
	// (set) Token: 0x06003C1F RID: 15391 RVA: 0x0002132C File Offset: 0x0001F52C
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

	// Token: 0x17001601 RID: 5633
	// (get) Token: 0x06003C20 RID: 15392 RVA: 0x00021335 File Offset: 0x0001F535
	// (set) Token: 0x06003C21 RID: 15393 RVA: 0x0002133D File Offset: 0x0001F53D
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

	// Token: 0x06003C22 RID: 15394 RVA: 0x00021346 File Offset: 0x0001F546
	private void Start()
	{
		base.SourceProjectile.OnDeathRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x000F5B0C File Offset: 0x000F3D0C
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

	// Token: 0x06003C24 RID: 15396 RVA: 0x00021366 File Offset: 0x0001F566
	private void OnDestroy()
	{
		base.SourceProjectile.OnDeathRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002FB8 RID: 12216
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002FB9 RID: 12217
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04002FBA RID: 12218
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
