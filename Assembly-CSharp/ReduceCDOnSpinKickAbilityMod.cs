using System;
using RLAudio;
using UnityEngine;

// Token: 0x020002A3 RID: 675
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnSpinKickAbilityMod : MonoBehaviour, IAudioEventEmitter
{
	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x0000A05A File Offset: 0x0000825A
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onPlayerDownstrikeBounce = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDownstrikeBounce);
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x0000A07A File Offset: 0x0000827A
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeBounce, this.m_onPlayerDownstrikeBounce);
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x0000A089 File Offset: 0x00008289
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeBounce, this.m_onPlayerDownstrikeBounce);
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x00086010 File Offset: 0x00084210
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

	// Token: 0x040015D4 RID: 5588
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040015D5 RID: 5589
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040015D6 RID: 5590
	[SerializeField]
	private bool m_grantChargeStatusAfterSpinkick;

	// Token: 0x040015D7 RID: 5591
	private BaseAbility_RL m_ability;

	// Token: 0x040015D8 RID: 5592
	private Action<MonoBehaviour, EventArgs> m_onPlayerDownstrikeBounce;
}
