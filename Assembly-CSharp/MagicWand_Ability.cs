using System;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class MagicWand_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06001022 RID: 4130 RVA: 0x0002EE94 File Offset: 0x0002D094
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_strikeProjectileName
		};
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x06001023 RID: 4131 RVA: 0x0002EEB4 File Offset: 0x0002D0B4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06001024 RID: 4132 RVA: 0x0002EEBB File Offset: 0x0002D0BB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06001025 RID: 4133 RVA: 0x0002EEC2 File Offset: 0x0002D0C2
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x0002EEC9 File Offset: 0x0002D0C9
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06001027 RID: 4135 RVA: 0x0002EED0 File Offset: 0x0002D0D0
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06001028 RID: 4136 RVA: 0x0002EED7 File Offset: 0x0002D0D7
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06001029 RID: 4137 RVA: 0x0002EEDE File Offset: 0x0002D0DE
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x0002EEE5 File Offset: 0x0002D0E5
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x0600102B RID: 4139 RVA: 0x0002EEEC File Offset: 0x0002D0EC
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x0002EEF3 File Offset: 0x0002D0F3
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x0600102D RID: 4141 RVA: 0x0002EEFA File Offset: 0x0002D0FA
	protected virtual Vector2 ProjectileIndicatorPosition
	{
		get
		{
			return new Vector2(7.2f, 1f);
		}
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0002EF0C File Offset: 0x0002D10C
	protected override void Awake()
	{
		this.m_onPlayerScaleChanged = new Action<MonoBehaviour, EventArgs>(this.OnPlayerScaleChanged);
		this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
		this.m_onPlayerManaChange = new Action<MonoBehaviour, EventArgs>(this.OnPlayerManaChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerScaleChanged, this.m_onPlayerScaleChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerManaChange, this.m_onPlayerManaChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		base.Awake();
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0002EF7C File Offset: 0x0002D17C
	public override void OnPreDestroy()
	{
		this.m_aimIndicator.transform.SetParent(null);
		base.OnPreDestroy();
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0002EF95 File Offset: 0x0002D195
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.m_aimIndicator.SetActive(false);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0002EFA4 File Offset: 0x0002D1A4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.m_aimIndicator);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerScaleChanged, this.m_onPlayerScaleChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_onPlayerManaChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		this.m_aimIndicator = null;
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0002EFF0 File Offset: 0x0002D1F0
	private void OnPlayerScaleChanged(object sender, EventArgs args)
	{
		this.InitializeAimIndicator();
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0002EFF8 File Offset: 0x0002D1F8
	private void OnPlayerManaChange(object sender, EventArgs args)
	{
		ManaChangeEventArgs manaChangeEventArgs = args as ManaChangeEventArgs;
		float num = manaChangeEventArgs.PrevManaValue - manaChangeEventArgs.NewManaValue;
		bool flag = (float)manaChangeEventArgs.Player.ActualMaxMana == manaChangeEventArgs.NewManaValue;
		bool flag2 = this.m_abilityController.PlayerController.CastAbility.AbilityInProgress(CastAbilityType.Spell) || this.m_abilityController.PlayerController.CastAbility.AbilityInProgress(CastAbilityType.Talent);
		if (num > 0f && !flag && flag2)
		{
			this.m_manaSpentForCrit += num;
			if (this.m_manaSpentForCrit >= 50f)
			{
				this.m_manaSpentForCrit = 0f;
				this.m_abilityController.PlayerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 3f, null);
			}
		}
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0002F0B8 File Offset: 0x0002D2B8
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_aimIndicatorStartingScale = this.m_aimIndicator.transform.localScale.x;
		this.m_aimIndicator.transform.SetParent(this.m_abilityController.PlayerController.Visuals.transform, true);
		this.m_manaSpentForCrit = 0f;
		this.InitializeAimIndicator();
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0002F11F File Offset: 0x0002D31F
	public override void Reinitialize()
	{
		base.Reinitialize();
		if (!this.m_aimIndicator.activeSelf)
		{
			this.m_aimIndicator.SetActive(true);
		}
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0002F140 File Offset: 0x0002D340
	private void InitializeAimIndicator()
	{
		float num = this.m_abilityController.PlayerController.BaseScaleToOffsetWith / this.m_abilityController.PlayerController.transform.lossyScale.x;
		Vector2 projectileIndicatorPosition = this.ProjectileIndicatorPosition;
		if (!this.m_abilityController.PlayerController.IsFacingRight)
		{
			projectileIndicatorPosition.x = -this.ProjectileIndicatorPosition.x;
		}
		this.m_aimIndicator.transform.position = this.m_abilityController.PlayerController.transform.position + projectileIndicatorPosition;
		num += this.m_aimIndicatorStartingScale * num - num;
		this.m_aimIndicator.transform.localScale = new Vector3(num, num, num);
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0002F1FA File Offset: 0x0002D3FA
	public override void PreCastAbility()
	{
		this.InitializeAimIndicator();
		if (this.m_abilityController.PlayerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_FreeCrit))
		{
			this.m_animator.SetBool("MagicWand_ForceCrit", true);
		}
		base.PreCastAbility();
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0002F238 File Offset: 0x0002D438
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			Vector2 vector = this.ProjectileOffset;
			if (!this.m_abilityController.PlayerController.IsFacingRight)
			{
				vector.x = -vector.x;
			}
			vector += this.m_abilityController.PlayerController.transform.localPosition;
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, vector, true, 0f, 1f, true, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
		this.m_strikeProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.m_strikeProjectileName, this.m_strikeProjectileOffset, true, 0f, 1f, false, true, true, true);
		this.m_abilityController.InitializeProjectile(this.m_strikeProjectile);
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0002F322 File Offset: 0x0002D522
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		this.m_animator.SetBool("MagicWand_ForceCrit", false);
		if (this.m_strikeProjectile)
		{
			this.m_strikeProjectile.FlagForDestruction(null);
			this.m_strikeProjectile = null;
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0002F35C File Offset: 0x0002D55C
	protected override void PlayFreeCritReleaseAudio()
	{
	}

	// Token: 0x040011A8 RID: 4520
	[SerializeField]
	private GameObject m_aimIndicator;

	// Token: 0x040011A9 RID: 4521
	[Header("Strike Projectile")]
	[SerializeField]
	private string m_strikeProjectileName;

	// Token: 0x040011AA RID: 4522
	[SerializeField]
	private Vector2 m_strikeProjectileOffset;

	// Token: 0x040011AB RID: 4523
	private Projectile_RL m_strikeProjectile;

	// Token: 0x040011AC RID: 4524
	private float m_aimIndicatorStartingScale;

	// Token: 0x040011AD RID: 4525
	private float m_manaSpentForCrit;

	// Token: 0x040011AE RID: 4526
	private Action<MonoBehaviour, EventArgs> m_onPlayerScaleChanged;

	// Token: 0x040011AF RID: 4527
	private Action<MonoBehaviour, EventArgs> m_onPlayerManaChange;

	// Token: 0x040011B0 RID: 4528
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;
}
