using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000172 RID: 370
public class FlameBarrier_Ability : BaseAbility_RL, ISpell, IAbility, IPersistentAbility
{
	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0002739C File Offset: 0x0002559C
	// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x000273A4 File Offset: 0x000255A4
	public bool IsPersistentActive { get; private set; }

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x000273AD File Offset: 0x000255AD
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000273B4 File Offset: 0x000255B4
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x000273BB File Offset: 0x000255BB
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000273C2 File Offset: 0x000255C2
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x000273C9 File Offset: 0x000255C9
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000273D0 File Offset: 0x000255D0
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000273D7 File Offset: 0x000255D7
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000273DE File Offset: 0x000255DE
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000273E5 File Offset: 0x000255E5
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000273EC File Offset: 0x000255EC
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x000273F3 File Offset: 0x000255F3
	protected override void Awake()
	{
		base.Awake();
		this.m_projectileArray = new Projectile_RL[this.m_numberOfFireballs];
		this.m_rotationDiff = 360f / (float)this.m_numberOfFireballs;
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00027420 File Offset: 0x00025620
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_carouselGO = new GameObject();
		this.m_carouselGO.name = "CarouselGO";
		this.m_carouselGO.transform.SetParent(this.m_abilityController.PlayerController.transform);
		this.ReinitializeCarousel();
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00027476 File Offset: 0x00025676
	public override void PreCastAbility()
	{
		this.ReinitializeCarousel();
		if (this.IsPersistentActive)
		{
			this.StopFlameBarrier();
			this.StopAbility(true);
			return;
		}
		this.m_hasAnimation = false;
		this.IsPersistentActive = true;
		base.PreCastAbility();
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000274A8 File Offset: 0x000256A8
	private void ReinitializeCarousel()
	{
		Vector3 a = Vector2.zero;
		float d = this.m_abilityController.PlayerController.transform.lossyScale.x / this.m_abilityController.PlayerController.BaseScaleToOffsetWith;
		Vector2 vector = this.ProjectileOffset * d;
		a.x += vector.x;
		a.y += vector.y;
		this.m_carouselGO.transform.position = a + this.m_abilityController.PlayerController.transform.localPosition;
		float num = 1f / this.m_abilityController.PlayerController.transform.lossyScale.x;
		this.m_carouselGO.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00027583 File Offset: 0x00025783
	public override IEnumerator CastAbility()
	{
		if (!this.IsPersistentActive)
		{
			yield break;
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00027592 File Offset: 0x00025792
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x000275AC File Offset: 0x000257AC
	protected override void FireProjectile()
	{
		if (!this.IsPersistentActive)
		{
			return;
		}
		if (this.ProjectileName != null)
		{
			foreach (Projectile_RL projectile_RL in this.m_projectileArray)
			{
				if (projectile_RL)
				{
					projectile_RL.gameObject.SetActive(false);
					projectile_RL.transform.SetParent(null);
				}
			}
			Vector2 thePoint = new Vector2(this.m_radius, 0f);
			for (int j = 0; j < this.m_numberOfFireballs; j++)
			{
				float theRotation = (float)j * this.m_rotationDiff;
				Vector2 vector = CDGHelper.RotatedPoint(thePoint, Vector2.zero, theRotation);
				this.m_projectileArray[j] = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, vector, false, 0f, 1f, false, true, true, true);
				this.m_projectileArray[j].transform.SetParent(this.m_carouselGO.transform, false);
				this.m_projectileArray[j].transform.localPosition = vector;
				this.m_abilityController.InitializeProjectile(this.m_projectileArray[j]);
			}
			this.ApplyAbilityCosts();
			this.m_carouselGO.transform.eulerAngles = Vector3.zero;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_rotationDirection = -1;
				return;
			}
			this.m_rotationDirection = 1;
		}
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00027708 File Offset: 0x00025908
	protected override void ApplyAbilityCosts()
	{
		if ((base.MaxAmmo > 0 && base.CurrentAmmo <= 0) || (base.ActualCost > 0 && this.m_abilityController.PlayerController.CurrentManaAsInt <= 0))
		{
			this.StopAbility(true);
			this.StopFlameBarrier();
			return;
		}
		this.m_ticRateStartTime = Time.time;
		base.ApplyAbilityCosts();
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00027764 File Offset: 0x00025964
	private void StopFlameBarrier()
	{
		this.IsPersistentActive = false;
		this.m_hasAnimation = true;
		if (this.m_stopCastingEvent != null)
		{
			this.m_stopCastingEvent.Invoke();
		}
		foreach (Projectile_RL projectile_RL in this.m_projectileArray)
		{
			if (projectile_RL && projectile_RL.isActiveAndEnabled)
			{
				projectile_RL.FlagForDestruction(null);
			}
		}
		this.StartCooldownTimer();
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x000277C8 File Offset: 0x000259C8
	public void StopPersistentAbility()
	{
		this.StopFlameBarrier();
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x000277D0 File Offset: 0x000259D0
	protected override void Update()
	{
		base.Update();
		if (!this.IsPersistentActive)
		{
			return;
		}
		if (this.m_abilityController && this.m_abilityController.PlayerController.ConditionState == CharacterStates.CharacterConditions.Stunned && Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.StopPersistentAbility();
		}
		if (this.m_projectileArray[0] && this.m_projectileArray[0].isActiveAndEnabled)
		{
			Vector3 eulerAngles = this.m_carouselGO.transform.eulerAngles;
			eulerAngles.z += this.m_rotationSpeed * Time.deltaTime * (float)this.m_rotationDirection;
			this.m_carouselGO.transform.eulerAngles = eulerAngles;
			Projectile_RL[] projectileArray = this.m_projectileArray;
			for (int i = 0; i < projectileArray.Length; i++)
			{
				projectileArray[i].transform.eulerAngles = Vector3.zero;
			}
			if (Time.time >= this.m_ticRateStartTime + 0.5f)
			{
				this.ApplyAbilityCosts();
			}
		}
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000278D4 File Offset: 0x00025AD4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		bool flag = this.m_carouselGO;
		foreach (Projectile_RL projectile_RL in this.m_projectileArray)
		{
			if (projectile_RL && flag && projectile_RL.transform.parent == this.m_carouselGO.transform)
			{
				projectile_RL.gameObject.SetActive(false);
				projectile_RL.transform.SetParent(null);
			}
		}
		if (flag)
		{
			UnityEngine.Object.Destroy(this.m_carouselGO);
		}
	}

	// Token: 0x040010AD RID: 4269
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_numberOfFireballs = 8;

	// Token: 0x040010AE RID: 4270
	[SerializeField]
	private float m_radius;

	// Token: 0x040010AF RID: 4271
	[SerializeField]
	private float m_rotationSpeed = 360f;

	// Token: 0x040010B0 RID: 4272
	[SerializeField]
	private UnityEvent m_stopCastingEvent;

	// Token: 0x040010B1 RID: 4273
	private Projectile_RL[] m_projectileArray;

	// Token: 0x040010B2 RID: 4274
	private float m_rotationDiff;

	// Token: 0x040010B3 RID: 4275
	private GameObject m_carouselGO;

	// Token: 0x040010B4 RID: 4276
	private float m_ticRateStartTime;

	// Token: 0x040010B5 RID: 4277
	private sbyte m_rotationDirection;
}
