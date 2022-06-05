using System;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
public class OnDeathEffectTriggerCondition : MonoBehaviour
{
	// Token: 0x06003583 RID: 13699 RVA: 0x0001D589 File Offset: 0x0001B789
	private void Awake()
	{
		this.m_onDeathEffectObj = this.GetRoot(false).GetComponentInChildren<IEffectTriggerEvent_OnDeath>();
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x0001D59D File Offset: 0x0001B79D
	private void Start()
	{
		this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.RemoveAll(true, true);
		this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.AddListener(new Action<GameObject>(this.TriggerConditionalEffect), false);
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x000E1790 File Offset: 0x000DF990
	private void TriggerConditionalEffect(GameObject attacker)
	{
		OnDeathEffectTrigger[] array;
		if (attacker != null && attacker.CompareTag("PlayerProjectile"))
		{
			array = this.OnDeathAttackedEffectArray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].InvokeOnDeathTrigger(attacker);
			}
			return;
		}
		array = this.OnDeathCollidedEffectArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].InvokeOnDeathTrigger(attacker);
		}
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x0001D5CF File Offset: 0x0001B7CF
	private void OnDestroy()
	{
		if (this.m_onDeathEffectObj != null)
		{
			this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.RemoveListener(new Action<GameObject>(this.TriggerConditionalEffect));
		}
	}

	// Token: 0x04002B92 RID: 11154
	[SerializeField]
	public OnDeathEffectTrigger[] OnDeathCollidedEffectArray;

	// Token: 0x04002B93 RID: 11155
	[SerializeField]
	public OnDeathEffectTrigger[] OnDeathAttackedEffectArray;

	// Token: 0x04002B94 RID: 11156
	private IEffectTriggerEvent_OnDeath m_onDeathEffectObj;
}
