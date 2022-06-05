using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnBurnAbilityMod : MonoBehaviour
{
	// Token: 0x06000C83 RID: 3203 RVA: 0x00026BD2 File Offset: 0x00024DD2
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onEnemyHit = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00026BF2 File Offset: 0x00024DF2
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHit, this.m_onEnemyHit);
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x00026C00 File Offset: 0x00024E00
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHit);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x00026C10 File Offset: 0x00024E10
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

	// Token: 0x0400108A RID: 4234
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x0400108B RID: 4235
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x0400108C RID: 4236
	private BaseAbility_RL m_ability;

	// Token: 0x0400108D RID: 4237
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;
}
