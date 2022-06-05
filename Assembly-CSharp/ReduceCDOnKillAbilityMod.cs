using System;
using UnityEngine;

// Token: 0x020002A1 RID: 673
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnKillAbilityMod : MonoBehaviour
{
	// Token: 0x060013A0 RID: 5024 RVA: 0x00009FE0 File Offset: 0x000081E0
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onEnemyDeath = new Action<MonoBehaviour, EventArgs>(this.OnEnemyDeath);
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x0000A000 File Offset: 0x00008200
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0000A00F File Offset: 0x0000820F
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00085F88 File Offset: 0x00084188
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

	// Token: 0x040015CC RID: 5580
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040015CD RID: 5581
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040015CE RID: 5582
	private BaseAbility_RL m_ability;

	// Token: 0x040015CF RID: 5583
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;
}
