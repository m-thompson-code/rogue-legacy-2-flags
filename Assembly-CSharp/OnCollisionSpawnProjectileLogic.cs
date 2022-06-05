using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007AE RID: 1966
public class OnCollisionSpawnProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x170015EF RID: 5615
	// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x00020FBA File Offset: 0x0001F1BA
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

	// Token: 0x170015F0 RID: 5616
	// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x00020FDF File Offset: 0x0001F1DF
	// (set) Token: 0x06003BDA RID: 15322 RVA: 0x00020FE7 File Offset: 0x0001F1E7
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

	// Token: 0x170015F1 RID: 5617
	// (get) Token: 0x06003BDB RID: 15323 RVA: 0x00020FF0 File Offset: 0x0001F1F0
	// (set) Token: 0x06003BDC RID: 15324 RVA: 0x00020FF8 File Offset: 0x0001F1F8
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

	// Token: 0x170015F2 RID: 5618
	// (get) Token: 0x06003BDD RID: 15325 RVA: 0x00021001 File Offset: 0x0001F201
	// (set) Token: 0x06003BDE RID: 15326 RVA: 0x00021009 File Offset: 0x0001F209
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

	// Token: 0x170015F3 RID: 5619
	// (get) Token: 0x06003BDF RID: 15327 RVA: 0x00021012 File Offset: 0x0001F212
	// (set) Token: 0x06003BE0 RID: 15328 RVA: 0x0002101A File Offset: 0x0001F21A
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

	// Token: 0x06003BE1 RID: 15329 RVA: 0x00021023 File Offset: 0x0001F223
	private void Start()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x000F4D74 File Offset: 0x000F2F74
	protected virtual void SpawnProjectile(Projectile_RL projectile, GameObject colliderObj)
	{
		if (this.m_onlyCollideWithProjectiles && !CollisionType_RL.IsProjectile(colliderObj))
		{
			return;
		}
		if (this.m_projectileSpawnedUnityEvent != null)
		{
			this.m_projectileSpawnedUnityEvent.Invoke();
		}
		Vector2 offset = base.SourceProjectile.Midpoint;
		Projectile_RL projectile_RL = ProjectileManager.FireProjectile(base.SourceProjectile.Owner, this.m_projectileToSpawn, offset, this.MatchFacing, this.Angle, 1f, true, true, true, true);
		PlayerController playerController = base.SourceProjectile.OwnerController as PlayerController;
		if (playerController)
		{
			bool flag = playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_FreeCrit);
			float duration = 0f;
			if (flag)
			{
				duration = playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_FreeCrit).EndTime - Time.time;
				playerController.StatusEffectController.StopStatusEffect(StatusEffectType.Player_FreeCrit, true);
			}
			CastAbilityType lastCastAbilityTypeCasted = playerController.CastAbility.LastCastAbilityTypeCasted;
			playerController.CastAbility.SetLastCastAbilityTypeOverride(base.SourceProjectile.CastAbilityType);
			playerController.CastAbility.InitializeProjectile(projectile_RL);
			playerController.CastAbility.SetLastCastAbilityTypeOverride(lastCastAbilityTypeCasted);
			if (flag)
			{
				playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, duration, null);
			}
			if (!projectile_RL.HitboxController.ResponseMethodsInitialized && colliderObj)
			{
				projectile_RL.ForceDispatchOnCollisionRelay(colliderObj);
			}
		}
		projectile_RL.CastAbilityType = base.SourceProjectile.CastAbilityType;
		if (this.m_spawnAtCollisionPoint)
		{
			Vector3 localPosition = base.SourceProjectile.Midpoint;
			if (base.SourceProjectile.HitboxController.LastCollidedWith)
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

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00021044 File Offset: 0x0001F244
	private void OnDestroy()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002F83 RID: 12163
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002F84 RID: 12164
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04002F85 RID: 12165
	[SerializeField]
	private bool m_matchFacing;

	// Token: 0x04002F86 RID: 12166
	[SerializeField]
	private float m_angle;

	// Token: 0x04002F87 RID: 12167
	[SerializeField]
	private bool m_spawnAtCollisionPoint;

	// Token: 0x04002F88 RID: 12168
	[SerializeField]
	private bool m_onlyCollideWithProjectiles;

	// Token: 0x04002F89 RID: 12169
	[SerializeField]
	private UnityEvent m_projectileSpawnedUnityEvent;

	// Token: 0x04002F8A RID: 12170
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
