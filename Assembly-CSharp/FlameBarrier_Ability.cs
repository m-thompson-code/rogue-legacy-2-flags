using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002AB RID: 683
public class FlameBarrier_Ability : BaseAbility_RL, ISpell, IAbility, IPersistentAbility
{
	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0000A235 File Offset: 0x00008435
	// (set) Token: 0x060013F6 RID: 5110 RVA: 0x0000A23D File Offset: 0x0000843D
	public bool IsPersistentActive { get; private set; }

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x060013FA RID: 5114 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x060013FB RID: 5115 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x060013FC RID: 5116 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x060013FD RID: 5117 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x060013FE RID: 5118 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x060013FF RID: 5119 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0000A246 File Offset: 0x00008446
	protected override void Awake()
	{
		base.Awake();
		this.m_projectileArray = new Projectile_RL[this.m_numberOfFireballs];
		this.m_rotationDiff = 360f / (float)this.m_numberOfFireballs;
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x00086408 File Offset: 0x00084608
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_carouselGO = new GameObject();
		this.m_carouselGO.name = "CarouselGO";
		this.m_carouselGO.transform.SetParent(this.m_abilityController.PlayerController.transform);
		this.ReinitializeCarousel();
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0000A272 File Offset: 0x00008472
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

	// Token: 0x06001404 RID: 5124 RVA: 0x00086460 File Offset: 0x00084660
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

	// Token: 0x06001405 RID: 5125 RVA: 0x0000A2A4 File Offset: 0x000084A4
	public override IEnumerator CastAbility()
	{
		if (!this.IsPersistentActive)
		{
			yield break;
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x0000A2B3 File Offset: 0x000084B3
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x0008653C File Offset: 0x0008473C
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

	// Token: 0x06001408 RID: 5128 RVA: 0x00086698 File Offset: 0x00084898
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

	// Token: 0x06001409 RID: 5129 RVA: 0x000866F4 File Offset: 0x000848F4
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

	// Token: 0x0600140A RID: 5130 RVA: 0x0000A2CC File Offset: 0x000084CC
	public void StopPersistentAbility()
	{
		this.StopFlameBarrier();
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x00086758 File Offset: 0x00084958
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

	// Token: 0x0600140C RID: 5132 RVA: 0x0008685C File Offset: 0x00084A5C
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

	// Token: 0x040015EC RID: 5612
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_numberOfFireballs = 8;

	// Token: 0x040015ED RID: 5613
	[SerializeField]
	private float m_radius;

	// Token: 0x040015EE RID: 5614
	[SerializeField]
	private float m_rotationSpeed = 360f;

	// Token: 0x040015EF RID: 5615
	[SerializeField]
	private UnityEvent m_stopCastingEvent;

	// Token: 0x040015F0 RID: 5616
	private Projectile_RL[] m_projectileArray;

	// Token: 0x040015F1 RID: 5617
	private float m_rotationDiff;

	// Token: 0x040015F2 RID: 5618
	private GameObject m_carouselGO;

	// Token: 0x040015F3 RID: 5619
	private float m_ticRateStartTime;

	// Token: 0x040015F4 RID: 5620
	private sbyte m_rotationDirection;
}
