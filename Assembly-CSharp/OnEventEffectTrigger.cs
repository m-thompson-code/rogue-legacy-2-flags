using System;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
public class OnEventEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17001443 RID: 5187
	// (get) Token: 0x06003590 RID: 13712 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001444 RID: 5188
	// (get) Token: 0x06003591 RID: 13713 RVA: 0x0001D67D File Offset: 0x0001B87D
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

	// Token: 0x06003592 RID: 13714 RVA: 0x0001D6A9 File Offset: 0x0001B8A9
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000E18E4 File Offset: 0x000DFAE4
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

	// Token: 0x04002B96 RID: 11158
	private BaseCharacterController m_charController;
}
