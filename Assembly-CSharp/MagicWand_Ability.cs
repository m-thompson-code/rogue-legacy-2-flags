using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class MagicWand_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06001833 RID: 6195 RVA: 0x0000C2BE File Offset: 0x0000A4BE
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_strikeProjectileName
		};
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06001834 RID: 6196 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06001835 RID: 6197 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06001836 RID: 6198 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06001837 RID: 6199 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06001838 RID: 6200 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06001839 RID: 6201 RVA: 0x0000452F File Offset: 0x0000272F
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x0600183A RID: 6202 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x0600183B RID: 6203 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x0600183C RID: 6204 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x0600183E RID: 6206 RVA: 0x0000C2DE File Offset: 0x0000A4DE
	protected virtual Vector2 ProjectileIndicatorPosition
	{
		get
		{
			return new Vector2(7.2f, 1f);
		}
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x0008D878 File Offset: 0x0008BA78
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

	// Token: 0x06001840 RID: 6208 RVA: 0x0000C2EF File Offset: 0x0000A4EF
	public override void OnPreDestroy()
	{
		this.m_aimIndicator.transform.SetParent(null);
		base.OnPreDestroy();
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x0000C308 File Offset: 0x0000A508
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.m_aimIndicator.SetActive(false);
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x0008D8E8 File Offset: 0x0008BAE8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.m_aimIndicator);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerScaleChanged, this.m_onPlayerScaleChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_onPlayerManaChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		this.m_aimIndicator = null;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0000C316 File Offset: 0x0000A516
	private void OnPlayerScaleChanged(object sender, EventArgs args)
	{
		this.InitializeAimIndicator();
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0008D934 File Offset: 0x0008BB34
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

	// Token: 0x06001845 RID: 6213 RVA: 0x0008D9F4 File Offset: 0x0008BBF4
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_aimIndicatorStartingScale = this.m_aimIndicator.transform.localScale.x;
		this.m_aimIndicator.transform.SetParent(this.m_abilityController.PlayerController.Visuals.transform, true);
		this.m_manaSpentForCrit = 0f;
		this.InitializeAimIndicator();
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0000C31E File Offset: 0x0000A51E
	public override void Reinitialize()
	{
		base.Reinitialize();
		if (!this.m_aimIndicator.activeSelf)
		{
			this.m_aimIndicator.SetActive(true);
		}
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x0008DA5C File Offset: 0x0008BC5C
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

	// Token: 0x06001848 RID: 6216 RVA: 0x0000C33F File Offset: 0x0000A53F
	public override void PreCastAbility()
	{
		this.InitializeAimIndicator();
		if (this.m_abilityController.PlayerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_FreeCrit))
		{
			this.m_animator.SetBool("MagicWand_ForceCrit", true);
		}
		base.PreCastAbility();
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x0008DB18 File Offset: 0x0008BD18
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

	// Token: 0x0600184A RID: 6218 RVA: 0x0000C37A File Offset: 0x0000A57A
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

	// Token: 0x0600184B RID: 6219 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void PlayFreeCritReleaseAudio()
	{
	}

	// Token: 0x04001785 RID: 6021
	[SerializeField]
	private GameObject m_aimIndicator;

	// Token: 0x04001786 RID: 6022
	[Header("Strike Projectile")]
	[SerializeField]
	private string m_strikeProjectileName;

	// Token: 0x04001787 RID: 6023
	[SerializeField]
	private Vector2 m_strikeProjectileOffset;

	// Token: 0x04001788 RID: 6024
	private Projectile_RL m_strikeProjectile;

	// Token: 0x04001789 RID: 6025
	private float m_aimIndicatorStartingScale;

	// Token: 0x0400178A RID: 6026
	private float m_manaSpentForCrit;

	// Token: 0x0400178B RID: 6027
	private Action<MonoBehaviour, EventArgs> m_onPlayerScaleChanged;

	// Token: 0x0400178C RID: 6028
	private Action<MonoBehaviour, EventArgs> m_onPlayerManaChange;

	// Token: 0x0400178D RID: 6029
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;
}
