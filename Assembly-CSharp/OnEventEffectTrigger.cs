using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class OnEventEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060026DB RID: 9947 RVA: 0x000813C3 File Offset: 0x0007F5C3
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060026DC RID: 9948 RVA: 0x000813C6 File Offset: 0x0007F5C6
	public override Vector3 Midpoint
	{
		get
		{
			if (this.m_charController != null)
			{
				return this.m_charController.Midpoint;
			}
			return base.gameObject.transform.position;
		}
	}

	// Token: 0x060026DD RID: 9949 RVA: 0x000813F2 File Offset: 0x0007F5F2
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
	}

	// Token: 0x060026DE RID: 9950 RVA: 0x0008140C File Offset: 0x0007F60C
	public void TriggerEffect()
	{
		GameObject rootObj = this.m_rootObj;
		GameObject rootObj2 = this.m_rootObj;
		IMidpointObj component = rootObj2.GetComponent<BaseCharacterController>();
		IMidpointObj midpointObj = component ?? rootObj2.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj == null) ? rootObj2.transform.position : midpointObj.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(rootObj2, effectTriggerEntry.TriggerDirection);
			EffectTriggerDirection effectDirectionFromObject2 = EffectTrigger.GetEffectDirectionFromObject(this.m_rootObj, effectTriggerEntry.TriggerDirection);
			EffectTargetType deriveFacing = effectTriggerEntry.DeriveFacing;
			if (deriveFacing != EffectTargetType.None)
			{
				if (deriveFacing != EffectTargetType.Self)
				{
					if (deriveFacing == EffectTargetType.Other)
					{
						EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, rootObj2, this.Midpoint, otherObjMidpos, effectDirectionFromObject, null);
					}
				}
				else
				{
					EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, rootObj2, this.Midpoint, otherObjMidpos, effectDirectionFromObject2, null);
				}
			}
			else
			{
				EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, rootObj2, this.Midpoint, otherObjMidpos, EffectTriggerDirection.Anywhere, null);
			}
		}
	}

	// Token: 0x040020B3 RID: 8371
	private BaseCharacterController m_charController;
}
