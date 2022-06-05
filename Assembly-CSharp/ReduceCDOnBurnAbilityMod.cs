using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnBurnAbilityMod : MonoBehaviour
{
	// Token: 0x06001396 RID: 5014 RVA: 0x00009F66 File Offset: 0x00008166
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onEnemyHit = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x00009F86 File Offset: 0x00008186
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHit, this.m_onEnemyHit);
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x00009F94 File Offset: 0x00008194
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHit);
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00085E14 File Offset: 0x00084014
	private void OnEnemyHit(object sender, EventArgs args)
	{
		if ((args as CharacterHitEventArgs).Victim.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Burn))
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
	}

	// Token: 0x040015BE RID: 5566
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040015BF RID: 5567
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040015C0 RID: 5568
	private BaseAbility_RL m_ability;

	// Token: 0x040015C1 RID: 5569
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;
}
