using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200016D RID: 365
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnSpinKickAbilityMod : MonoBehaviour, IAudioEventEmitter
{
	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06000C97 RID: 3223 RVA: 0x00026EE3 File Offset: 0x000250E3
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x00026EEB File Offset: 0x000250EB
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onPlayerDownstrikeBounce = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDownstrikeBounce);
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00026F0B File Offset: 0x0002510B
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeBounce, this.m_onPlayerDownstrikeBounce);
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00026F1A File Offset: 0x0002511A
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeBounce, this.m_onPlayerDownstrikeBounce);
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00026F2C File Offset: 0x0002512C
	private void OnPlayerDownstrikeBounce(object sender, EventArgs args)
	{
		float amount;
		if (this.m_isFlat)
		{
			amount = this.m_cooldownReductionAmount;
		}
		else
		{
			amount = this.m_ability.ActualCooldownTime * this.m_cooldownReductionAmount;
		}
		float cooldownTimer = this.m_ability.CooldownTimer;
		this.m_ability.ReduceCooldown(amount);
		if (this.m_ability.CooldownTimer == 0f && cooldownTimer != 0f)
		{
			AudioManager.PlayOneShotAttached(this, "event:/SFX/Interactables/sfx_pickups_ammoGain", base.gameObject);
		}
		if (this.m_grantChargeStatusAfterSpinkick)
		{
			PlayerManager.GetPlayerController().StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 1f, null);
		}
	}

	// Token: 0x040010A0 RID: 4256
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040010A1 RID: 4257
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040010A2 RID: 4258
	[SerializeField]
	private bool m_grantChargeStatusAfterSpinkick;

	// Token: 0x040010A3 RID: 4259
	private BaseAbility_RL m_ability;

	// Token: 0x040010A4 RID: 4260
	private Action<MonoBehaviour, EventArgs> m_onPlayerDownstrikeBounce;
}
