using System;
using UnityEngine;

// Token: 0x020002A2 RID: 674
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnPlayerHitAbilityMod : MonoBehaviour
{
	// Token: 0x060013A5 RID: 5029 RVA: 0x0000A01E File Offset: 0x0000821E
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0000A03E File Offset: 0x0000823E
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x0000A04C File Offset: 0x0000824C
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x00085FCC File Offset: 0x000841CC
	private void OnPlayerHit(object sender, EventArgs args)
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
		this.m_ability.ReduceCooldown(amount);
	}

	// Token: 0x040015D0 RID: 5584
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040015D2 RID: 5586
	private BaseAbility_RL m_ability;

	// Token: 0x040015D3 RID: 5587
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
