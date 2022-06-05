using System;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class Turret_Hazard : Hazard, IHasProjectileNameArray
{
	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x0600294F RID: 10575 RVA: 0x00088988 File Offset: 0x00086B88
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

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x06002950 RID: 10576 RVA: 0x000889AD File Offset: 0x00086BAD
	protected bool UseCustomLogic
	{
		get
		{
			return this.m_logicType == TurretLogicType.Custom;
		}
	}

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06002951 RID: 10577 RVA: 0x000889B8 File Offset: 0x00086BB8
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

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06002952 RID: 10578 RVA: 0x000889F8 File Offset: 0x00086BF8
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

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06002953 RID: 10579 RVA: 0x00088A38 File Offset: 0x00086C38
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

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x00088A68 File Offset: 0x00086C68
	public override float BaseDamage
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06002955 RID: 10581 RVA: 0x00088A70 File Offset: 0x00086C70
	public bool InAttackRange
	{
		get
		{
			return this.m_forceActivate || (PlayerManager.IsInstantiated && Vector2.Distance(PlayerManager.GetPlayerController().Midpoint, base.transform.position) <= 55f * CameraController.ZoomLevel);
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x00088AC4 File Offset: 0x00086CC4
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x00088ADD File Offset: 0x00086CDD
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs e)
	{
		if (base.isActiveAndEnabled)
		{
			this.m_timePlayerEnteredRoom = Time.time;
			this.ResetHazard();
			this.PrepFiringLogic();
		}
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x00088AFE File Offset: 0x00086CFE
	private void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs e)
	{
		if (base.isActiveAndEnabled)
		{
			this.ResetHazard();
		}
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x00088B0E File Offset: 0x00086D0E
	protected virtual void PrepFiringLogic()
	{
		this.m_tellPlayed = false;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x00088B18 File Offset: 0x00086D18
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

	// Token: 0x0600295B RID: 10587 RVA: 0x00088BC4 File Offset: 0x00086DC4
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

	// Token: 0x0600295C RID: 10588 RVA: 0x00088C22 File Offset: 0x00086E22
	protected virtual void PlayTell()
	{
		this.m_tellPlayed = true;
		base.Animator.SetTrigger("Tell");
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x00088C3C File Offset: 0x00086E3C
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

	// Token: 0x0600295E RID: 10590 RVA: 0x00088D4D File Offset: 0x00086F4D
	private void OnEnable()
	{
		this.m_timePlayerEnteredRoom = Time.time;
		this.ResetHazard();
		this.PrepFiringLogic();
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x00088D66 File Offset: 0x00086F66
	public override void ResetHazard()
	{
		this.m_tellPlayed = false;
		this.m_timesFired = 0;
		base.Animator.ResetTrigger("Fire");
		base.Animator.ResetTrigger("Tell");
		base.Animator.SetTrigger("Reset");
	}

	// Token: 0x04002209 RID: 8713
	[Header("Projectile Setup")]
	[SerializeField]
	protected string m_projectileName;

	// Token: 0x0400220A RID: 8714
	[SerializeField]
	protected Vector2 m_projectileOffset;

	// Token: 0x0400220B RID: 8715
	[SerializeField]
	protected float m_startingProjectileAngle;

	// Token: 0x0400220C RID: 8716
	[Header("Fire Rate Setup")]
	[SerializeField]
	protected bool m_useHalfLoopDelay = true;

	// Token: 0x0400220D RID: 8717
	[SerializeField]
	protected float m_initialFireDelay;

	// Token: 0x0400220E RID: 8718
	[SerializeField]
	protected TurretLogicType m_logicType;

	// Token: 0x0400220F RID: 8719
	[SerializeField]
	[ConditionalHide("UseCustomLogic", true)]
	protected float m_loopFireDelay = 1f;

	// Token: 0x04002210 RID: 8720
	[SerializeField]
	[ConditionalHide("UseCustomLogic", true)]
	protected float m_projectileSpeedMod = 1f;

	// Token: 0x04002211 RID: 8721
	[SerializeField]
	protected bool m_forceActivate;

	// Token: 0x04002212 RID: 8722
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04002213 RID: 8723
	protected float m_timePlayerEnteredRoom;

	// Token: 0x04002214 RID: 8724
	protected bool m_tellPlayed;

	// Token: 0x04002215 RID: 8725
	protected int m_timesFired;

	// Token: 0x04002216 RID: 8726
	[NonSerialized]
	protected string[] m_projectileNameArray;
}
