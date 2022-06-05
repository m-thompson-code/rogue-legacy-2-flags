using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A3 RID: 1187
public class OnCollisionSpawnProjectileLogic : BaseProjectileLogic, IHasProjectileNameArray
{
	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0009349D File Offset: 0x0009169D
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

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000934C2 File Offset: 0x000916C2
	// (set) Token: 0x06002B6D RID: 11117 RVA: 0x000934CA File Offset: 0x000916CA
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

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06002B6E RID: 11118 RVA: 0x000934D3 File Offset: 0x000916D3
	// (set) Token: 0x06002B6F RID: 11119 RVA: 0x000934DB File Offset: 0x000916DB
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

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x06002B70 RID: 11120 RVA: 0x000934E4 File Offset: 0x000916E4
	// (set) Token: 0x06002B71 RID: 11121 RVA: 0x000934EC File Offset: 0x000916EC
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

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x06002B72 RID: 11122 RVA: 0x000934F5 File Offset: 0x000916F5
	// (set) Token: 0x06002B73 RID: 11123 RVA: 0x000934FD File Offset: 0x000916FD
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

	// Token: 0x06002B74 RID: 11124 RVA: 0x00093506 File Offset: 0x00091706
	private void Start()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x00093528 File Offset: 0x00091728
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

	// Token: 0x06002B76 RID: 11126 RVA: 0x000937B2 File Offset: 0x000919B2
	private void OnDestroy()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.SpawnProjectile));
	}

	// Token: 0x04002351 RID: 9041
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04002352 RID: 9042
	[SerializeField]
	private Vector2 m_offset;

	// Token: 0x04002353 RID: 9043
	[SerializeField]
	private bool m_matchFacing;

	// Token: 0x04002354 RID: 9044
	[SerializeField]
	private float m_angle;

	// Token: 0x04002355 RID: 9045
	[SerializeField]
	private bool m_spawnAtCollisionPoint;

	// Token: 0x04002356 RID: 9046
	[SerializeField]
	private bool m_onlyCollideWithProjectiles;

	// Token: 0x04002357 RID: 9047
	[SerializeField]
	private UnityEvent m_projectileSpawnedUnityEvent;

	// Token: 0x04002358 RID: 9048
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
