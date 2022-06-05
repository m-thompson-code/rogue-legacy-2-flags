using System;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class Turret_Hazard : Hazard, IHasProjectileNameArray
{
	// Token: 0x17001548 RID: 5448
	// (get) Token: 0x0600394D RID: 14669 RVA: 0x0001F767 File Offset: 0x0001D967
	public virtual string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileName
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17001549 RID: 5449
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x0001F78C File Offset: 0x0001D98C
	protected bool UseCustomLogic
	{
		get
		{
			return this.m_logicType == TurretLogicType.Custom;
		}
	}

	// Token: 0x1700154A RID: 5450
	// (get) Token: 0x0600394F RID: 14671 RVA: 0x000EAA5C File Offset: 0x000E8C5C
	public virtual float ProjectileSpeedMod
	{
		get
		{
			switch (this.m_logicType)
			{
			case TurretLogicType.Normal:
				return 10f;
			case TurretLogicType.Slow:
				return 10f;
			case TurretLogicType.Fast:
				return 10f;
			default:
				return this.m_projectileSpeedMod;
			}
		}
	}

	// Token: 0x1700154B RID: 5451
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x000EAA9C File Offset: 0x000E8C9C
	public virtual float LoopFireDelay
	{
		get
		{
			switch (this.m_logicType)
			{
			case TurretLogicType.Normal:
				return 2.5f;
			case TurretLogicType.Slow:
				return 3.5f;
			case TurretLogicType.Fast:
				return 1.5f;
			default:
				return this.m_loopFireDelay;
			}
		}
	}

	// Token: 0x1700154C RID: 5452
	// (get) Token: 0x06003951 RID: 14673 RVA: 0x0001F797 File Offset: 0x0001D997
	public float InitialFireDelay
	{
		get
		{
			if (this.m_useHalfLoopDelay)
			{
				return this.LoopFireDelay * 0.5f + this.m_initialFireDelay + 0.875f;
			}
			return this.m_initialFireDelay + 0.875f;
		}
	}

	// Token: 0x1700154D RID: 5453
	// (get) Token: 0x06003952 RID: 14674 RVA: 0x00006E96 File Offset: 0x00005096
	public override float BaseDamage
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x1700154E RID: 5454
	// (get) Token: 0x06003953 RID: 14675 RVA: 0x000EAADC File Offset: 0x000E8CDC
	public bool InAttackRange
	{
		get
		{
			return this.m_forceActivate || (PlayerManager.IsInstantiated && Vector2.Distance(PlayerManager.GetPlayerController().Midpoint, base.transform.position) <= 55f * CameraController.ZoomLevel);
		}
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x0001F7C7 File Offset: 0x0001D9C7
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs e)
	{
		if (base.isActiveAndEnabled)
		{
			this.m_timePlayerEnteredRoom = Time.time;
			this.ResetHazard();
			this.PrepFiringLogic();
		}
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x0001F801 File Offset: 0x0001DA01
	private void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs e)
	{
		if (base.isActiveAndEnabled)
		{
			this.ResetHazard();
		}
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x0001F811 File Offset: 0x0001DA11
	protected virtual void PrepFiringLogic()
	{
		this.m_tellPlayed = false;
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x000EAB30 File Offset: 0x000E8D30
	public override void Initialize(HazardArgs hazardArgs)
	{
		if (hazardArgs is TurretHazardArgs)
		{
			TurretHazardArgs turretHazardArgs = hazardArgs as TurretHazardArgs;
			base.InitialState = turretHazardArgs.InitialState;
			StateID initialState = base.InitialState;
			if (initialState != StateID.One)
			{
				if (initialState != StateID.Two)
				{
					if (initialState == StateID.Random)
					{
						if (CDGHelper.RandomPlusMinus() > 0)
						{
							this.m_forceActivate = true;
						}
						else
						{
							this.m_forceActivate = false;
						}
					}
				}
				else
				{
					this.m_forceActivate = true;
				}
			}
			else
			{
				this.m_forceActivate = false;
			}
			this.m_logicType = turretHazardArgs.LogicType;
			this.m_loopFireDelay = turretHazardArgs.LoopFireDelay;
			this.m_projectileSpeedMod = turretHazardArgs.ProjectileSpeedMod;
			this.m_useHalfLoopDelay = turretHazardArgs.UseHalfLoopDelay;
			this.m_initialFireDelay = turretHazardArgs.InitialFireDelay;
		}
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x000EABDC File Offset: 0x000E8DDC
	protected virtual void Update()
	{
		float num = this.m_timePlayerEnteredRoom + this.InitialFireDelay + (float)this.m_timesFired * this.LoopFireDelay;
		if (Time.time < num - 0.75f)
		{
			return;
		}
		if (this.InAttackRange && !this.m_tellPlayed)
		{
			this.PlayTell();
		}
		if (Time.time < num)
		{
			return;
		}
		this.FireProjectile();
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x0001F81A File Offset: 0x0001DA1A
	protected virtual void PlayTell()
	{
		this.m_tellPlayed = true;
		base.Animator.SetTrigger("Tell");
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x000EAC3C File Offset: 0x000E8E3C
	protected virtual void FireProjectile()
	{
		if (this.InAttackRange)
		{
			float num = this.m_startingProjectileAngle + base.transform.localEulerAngles.z;
			Vector2 projectileOffset = this.m_projectileOffset;
			if (base.transform.localScale.x < 0f)
			{
				num += 180f;
				projectileOffset.x = -projectileOffset.x;
			}
			if (base.transform.localScale.y < 0f)
			{
				num = -num;
				projectileOffset.y = -projectileOffset.y;
			}
			ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, projectileOffset, true, num, this.ProjectileSpeedMod, false, true, true, true);
			base.Animator.SetTrigger("Fire");
			base.Animator.ResetTrigger("Tell");
		}
		else
		{
			base.Animator.ResetTrigger("Fire");
			base.Animator.ResetTrigger("Tell");
			base.Animator.SetTrigger("Reset");
		}
		this.m_timesFired++;
		this.PrepFiringLogic();
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x0001F833 File Offset: 0x0001DA33
	private void OnEnable()
	{
		this.m_timePlayerEnteredRoom = Time.time;
		this.ResetHazard();
		this.PrepFiringLogic();
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x0001F84C File Offset: 0x0001DA4C
	public override void ResetHazard()
	{
		this.m_tellPlayed = false;
		this.m_timesFired = 0;
		base.Animator.ResetTrigger("Fire");
		base.Animator.ResetTrigger("Tell");
		base.Animator.SetTrigger("Reset");
	}

	// Token: 0x04002DDA RID: 11738
	[Header("Projectile Setup")]
	[SerializeField]
	protected string m_projectileName;

	// Token: 0x04002DDB RID: 11739
	[SerializeField]
	protected Vector2 m_projectileOffset;

	// Token: 0x04002DDC RID: 11740
	[SerializeField]
	protected float m_startingProjectileAngle;

	// Token: 0x04002DDD RID: 11741
	[Header("Fire Rate Setup")]
	[SerializeField]
	protected bool m_useHalfLoopDelay = true;

	// Token: 0x04002DDE RID: 11742
	[SerializeField]
	protected float m_initialFireDelay;

	// Token: 0x04002DDF RID: 11743
	[SerializeField]
	protected TurretLogicType m_logicType;

	// Token: 0x04002DE0 RID: 11744
	[SerializeField]
	[ConditionalHide("UseCustomLogic", true)]
	protected float m_loopFireDelay = 1f;

	// Token: 0x04002DE1 RID: 11745
	[SerializeField]
	[ConditionalHide("UseCustomLogic", true)]
	protected float m_projectileSpeedMod = 1f;

	// Token: 0x04002DE2 RID: 11746
	[SerializeField]
	protected bool m_forceActivate;

	// Token: 0x04002DE3 RID: 11747
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04002DE4 RID: 11748
	protected float m_timePlayerEnteredRoom;

	// Token: 0x04002DE5 RID: 11749
	protected bool m_tellPlayed;

	// Token: 0x04002DE6 RID: 11750
	protected int m_timesFired;

	// Token: 0x04002DE7 RID: 11751
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
