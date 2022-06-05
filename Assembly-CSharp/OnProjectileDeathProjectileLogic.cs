using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class OnProjectileDeathProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x06002B7A RID: 11130 RVA: 0x00093861 File Offset: 0x00091A61
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

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x06002B7B RID: 11131 RVA: 0x00093886 File Offset: 0x00091A86
	// (set) Token: 0x06002B7C RID: 11132 RVA: 0x0009388E File Offset: 0x00091A8E
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

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x06002B7D RID: 11133 RVA: 0x00093897 File Offset: 0x00091A97
	// (set) Token: 0x06002B7E RID: 11134 RVA: 0x0009389F File Offset: 0x00091A9F
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

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000938A8 File Offset: 0x00091AA8
	// (set) Token: 0x06002B80 RID: 11136 RVA: 0x000938B0 File Offset: 0x00091AB0
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

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06002B81 RID: 11137 RVA: 0x000938B9 File Offset: 0x00091AB9
	// (set) Token: 0x06002B82 RID: 11138 RVA: 0x000938C1 File Offset: 0x00091AC1
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

	// Token: 0x06002B83 RID: 11139 RVA: 0x000938CA File Offset: 0x00091ACA
	private void Start()
	{
		base.SourceProjectile.OnDeathRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x000938EC File Offset: 0x00091AEC
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

	// Token: 0x06002B85 RID: 11141 RVA: 0x00093BF7 File Offset: 0x00091DF7
	private void OnDestroy()
	{
		base.SourceProjectile.OnDeathRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002359 RID: 9049
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x0400235A RID: 9050
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x0400235B RID: 9051
	[SerializeField]
	private bool m_matchFacing;

	// Token: 0x0400235C RID: 9052
	[SerializeField]
	private float m_angle;

	// Token: 0x0400235D RID: 9053
	[SerializeField]
	private bool m_spawnAtCollisionPoint;

	// Token: 0x0400235E RID: 9054
	[SerializeField]
	private bool m_ignoreTimeoutDeath;

	// Token: 0x0400235F RID: 9055
	[SerializeField]
	private OnProjectileDeathProjectileLogic.SpawnProjectileCollisionType m_spawnCollisionType;

	// Token: 0x04002360 RID: 9056
	[SerializeField]
	[Tooltip("Added this specifically for the AstroWand. Allows the spawned projectile to use its own crit chance calculation in the OnEnable method of its ProjectileLogic, rather than being overwritten with its source's crit chance.")]
	private bool m_useSourceProjectileActualCritChance = true;

	// Token: 0x04002361 RID: 9057
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x02000C84 RID: 3204
	private enum SpawnProjectileCollisionType
	{
		// Token: 0x040050C7 RID: 20679
		Any,
		// Token: 0x040050C8 RID: 20680
		PlatformsOnly,
		// Token: 0x040050C9 RID: 20681
		CharactersOnly
	}
}
