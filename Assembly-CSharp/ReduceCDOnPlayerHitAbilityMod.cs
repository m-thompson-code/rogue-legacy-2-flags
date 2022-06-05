using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnPlayerHitAbilityMod : MonoBehaviour
{
	// Token: 0x06000C92 RID: 3218 RVA: 0x00026E5B File Offset: 0x0002505B
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00026E7B File Offset: 0x0002507B
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00026E89 File Offset: 0x00025089
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00026E98 File Offset: 0x00025098
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

	// Token: 0x0400109C RID: 4252
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x0400109D RID: 4253
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x0400109E RID: 4254
	private BaseAbility_RL m_ability;

	// Token: 0x0400109F RID: 4255
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
