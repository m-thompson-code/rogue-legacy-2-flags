using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnKillAbilityMod : MonoBehaviour
{
	// Token: 0x06000C8D RID: 3213 RVA: 0x00026DD2 File Offset: 0x00024FD2
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onEnemyDeath = new Action<MonoBehaviour, EventArgs>(this.OnEnemyDeath);
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x00026DF2 File Offset: 0x00024FF2
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00026E01 File Offset: 0x00025001
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00026E10 File Offset: 0x00025010
	private void OnEnemyDeath(object sender, EventArgs args)
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

	// Token: 0x04001098 RID: 4248
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x04001099 RID: 4249
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x0400109A RID: 4250
	private BaseAbility_RL m_ability;

	// Token: 0x0400109B RID: 4251
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;
}
