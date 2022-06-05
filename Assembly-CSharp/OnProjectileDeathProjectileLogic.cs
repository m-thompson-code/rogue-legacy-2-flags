using System;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
public class OnProjectileDeathProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x170015F4 RID: 5620
	// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x0002106C File Offset: 0x0001F26C
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

	// Token: 0x170015F5 RID: 5621
	// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x00021091 File Offset: 0x0001F291
	// (set) Token: 0x06003BE9 RID: 15337 RVA: 0x00021099 File Offset: 0x0001F299
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

	// Token: 0x170015F6 RID: 5622
	// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000210A2 File Offset: 0x0001F2A2
	// (set) Token: 0x06003BEB RID: 15339 RVA: 0x000210AA File Offset: 0x0001F2AA
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

	// Token: 0x170015F7 RID: 5623
	// (get) Token: 0x06003BEC RID: 15340 RVA: 0x000210B3 File Offset: 0x0001F2B3
	// (set) Token: 0x06003BED RID: 15341 RVA: 0x000210BB File Offset: 0x0001F2BB
	public bool MatchFacing
	{
		get
		{
			return this.m_matchFacing;
		}
		set
		{
			this.m_matchFacing = value;
		}
	}

	// Token: 0x170015F8 RID: 5624
	// (get) Token: 0x06003BEE RID: 15342 RVA: 0x000210C4 File Offset: 0x0001F2C4
	// (set) Token: 0x06003BEF RID: 15343 RVA: 0x000210CC File Offset: 0x0001F2CC
	public float Angle
	{
		get
		{
			return this.m_angle;
		}
		set
		{
			this.m_angle = value;
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x000210D5 File Offset: 0x0001F2D5
	private void Start()
	{
		base.SourceProjectile.OnDeathRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x000F5080 File Offset: 0x000F3280
	protected virtual void SpawnProjectile(Projectile_RL projectile, GameObject colliderObj)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_ignoreTimeoutDeath && base.SourceProjectile.LifespanTimer <= 0f)
		{
			return;
		}
		if (!colliderObj && this.m_spawnCollisionType == OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType.CharactersOnly)
		{
			return;
		}
		bool flag = true;
		if (colliderObj)
		{
			OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType spawnCollisionType = this.m_spawnCollisionType;
			if (spawnCollisionType != OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType.PlatformsOnly)
			{
				if (spawnCollisionType == OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType.CharactersOnly)
				{
					if (!colliderObj.CompareTag("Enemy") && !colliderObj.CompareTag("Player") && !colliderObj.CompareTag("Player_Dodging"))
					{
						flag = false;
					}
				}
			}
			else if (!colliderObj.CompareTag("Platform") && !colliderObj.CompareTag("OneWay"))
			{
				flag = false;
			}
		}
		if (flag)
		{
			Vector2 offset = base.SourceProjectile.Midpoint;
			Projectile_RL projectile_RL = ProjectileManager.FireProjectile(base.SourceProjectile.Owner, this.m_projectileToSpawn, offset, this.MatchFacing, this.Angle, 1f, true, true, true, true);
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
			projectile_RL.RelicDamageTypeString = base.SourceProjectile.RelicDamageTypeString;
			if (this.m_useSourceProjectileActualCritChance)
			{
				projectile_RL.ActualCritChance = base.SourceProjectile.ActualCritChance;
			}
			if (this.m_spawnAtCollisionPoint)
			{
				Vector3 localPosition = base.SourceProjectile.Midpoint;
				if (base.SourceProjectile.HitboxController.LastCollidedWith != null)
				{
					localPosition = base.SourceProjectile.HitboxController.LastCollidedWith.ClosestPoint(base.SourceProjectile.Midpoint);
				}
				projectile_RL.transform.localPosition = localPosition;
			}
			else if (base.SourceProjectile.SnapToOwner)
			{
				projectile_RL.transform.localPosition = base.SourceProjectile.transform.position;
			}
			else
			{
				projectile_RL.transform.localPosition = base.SourceProjectile.transform.localPosition;
			}
			Vector2 offset2 = this.Offset;
			if (this.MatchFacing && base.SourceProjectile.IsFlipped)
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

	// Token: 0x06003BF2 RID: 15346 RVA: 0x000210F6 File Offset: 0x0001F2F6
	private void OnDestroy()
	{
		base.SourceProjectile.OnDeathRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002F8B RID: 12171
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002F8C RID: 12172
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04002F8D RID: 12173
	[SerializeField]
	private bool m_matchFacing;

	// Token: 0x04002F8E RID: 12174
	[SerializeField]
	private float m_angle;

	// Token: 0x04002F8F RID: 12175
	[SerializeField]
	private bool m_spawnAtCollisionPoint;

	// Token: 0x04002F90 RID: 12176
	[SerializeField]
	private bool m_ignoreTimeoutDeath;

	// Token: 0x04002F91 RID: 12177
	[SerializeField]
	private OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType m_spawnCollisionType;

	// Token: 0x04002F92 RID: 12178
	[SerializeField]
	[Tooltip("Added this specifically for the AstroWand. Allows the spawned projectile to use its own crit chance calculation in the OnEnable method of its ProjectileLogic, rather than being overwritten with its source's crit chance.")]
	private bool m_useSourceProjectileActualCritChance = true;

	// Token: 0x04002F93 RID: 12179
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x020007B1 RID: 1969
	private enum SpawnProjectileCollisionType
	{
		// Token: 0x04002F95 RID: 12181
		Any,
		// Token: 0x04002F96 RID: 12182
		PlatformsOnly,
		// Token: 0x04002F97 RID: 12183
		CharactersOnly
	}
}
