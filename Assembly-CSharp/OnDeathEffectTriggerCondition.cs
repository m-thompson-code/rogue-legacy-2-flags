using System;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class OnDeathEffectTriggerCondition : MonoBehaviour
{
	// Token: 0x060026CE RID: 9934 RVA: 0x00081166 File Offset: 0x0007F366
	private void Awake()
	{
		this.m_onDeathEffectObj = this.GetRoot(false).GetComponentInChildren<IEffectTriggerEvent_OnDeath>();
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x0008117A File Offset: 0x0007F37A
	private void Start()
	{
		this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.RemoveAll(true, true);
		this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.AddListener(new Action<GameObject>(this.TriggerConditionalEffect), false);
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x000811AC File Offset: 0x0007F3AC
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

	// Token: 0x060026D1 RID: 9937 RVA: 0x0008120C File Offset: 0x0007F40C
	private void OnDestroy()
	{
		if (this.m_onDeathEffectObj != null)
		{
			this.m_onDeathEffectObj.OnDeathEffectTriggerRelay.RemoveListener(new Action<GameObject>(this.TriggerConditionalEffect));
		}
	}

	// Token: 0x040020AF RID: 8367
	[SerializeField]
	public OnDeathEffectTrigger[] OnDeathCollidedEffectArray;

	// Token: 0x040020B0 RID: 8368
	[SerializeField]
	public OnDeathEffectTrigger[] OnDeathAttackedEffectArray;

	// Token: 0x040020B1 RID: 8369
	private IEffectTriggerEvent_OnDeath m_onDeathEffectObj;
}
