using System;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class OnTimeoutEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x060026F6 RID: 9974 RVA: 0x00081C12 File Offset: 0x0007FE12
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x060026F7 RID: 9975 RVA: 0x00081C18 File Offset: 0x0007FE18
	public override Vector3 Midpoint
	{
		get
		{
			if (this.m_charController != null)
			{
				return this.m_charController.Midpoint;
			}
			if (this.m_collider != null)
			{
				return this.m_collider.bounds.center;
			}
			return base.gameObject.transform.position;
		}
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x00081C74 File Offset: 0x0007FE74
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (!this.m_charController)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_effectTriggerEventArray = this.m_rootObj.GetComponentsInChildren<IEffectTriggerEvent_OnTimeout>();
		this.m_invokeTimeoutTrigger = new Action<GameObject>(this.InvokeTimeoutTrigger);
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x00081CD4 File Offset: 0x0007FED4
	private void OnEnable()
	{
		IEffectTriggerEvent_OnTimeout[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnTimeoutEffectTriggerRelay.AddListener(this.m_invokeTimeoutTrigger, false);
		}
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x00081D0C File Offset: 0x0007FF0C
	private void OnDisable()
	{
		IEffectTriggerEvent_OnTimeout[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnTimeoutEffectTriggerRelay.RemoveListener(this.m_invokeTimeoutTrigger);
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x00081D44 File Offset: 0x0007FF44
	private void InvokeTimeoutTrigger(GameObject otherObj)
	{
		GameObject root = otherObj.GetRoot(false);
		IMidpointObj component = root.GetComponent<BaseCharacterController>();
		IMidpointObj midpointObj = component ?? root.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj == null) ? root.transform.position : midpointObj.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(root, effectTriggerEntry.TriggerDirection);
			EffectTriggerDirection effectDirectionFromObject2 = EffectTrigger.GetEffectDirectionFromObject(this.m_rootObj, effectTriggerEntry.TriggerDirection);
			bool flag;
			if ((effectTriggerEntry.TriggerDirection & EffectTriggerDirection.StandingOn) != EffectTriggerDirection.None && (effectTriggerEntry.TriggerDirection & effectTriggerEntry.TriggerDirection - 1) == EffectTriggerDirection.None)
			{
				Debug.Log("<color=yellow>Warning: Invalid Effect Trigger Direction(" + EffectTriggerDirection.StandingOn.ToString() + ") for OnTimeoutEffectTrigger. Effect invoking anyway.");
				flag = true;
			}
			else
			{
				flag = ((effectTriggerEntry.TriggerDirection & effectDirectionFromObject) != EffectTriggerDirection.None || effectTriggerEntry.TriggerDirection == EffectTriggerDirection.Anywhere);
			}
			if (flag)
			{
				EffectTargetType deriveFacing = effectTriggerEntry.DeriveFacing;
				if (deriveFacing != EffectTargetType.None)
				{
					if (deriveFacing != EffectTargetType.Self)
					{
						if (deriveFacing == EffectTargetType.Other)
						{
							EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, root, this.Midpoint, otherObjMidpos, effectDirectionFromObject, null);
						}
					}
					else
					{
						EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, root, this.Midpoint, otherObjMidpos, effectDirectionFromObject2, null);
					}
				}
				else
				{
					EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, root, this.Midpoint, otherObjMidpos, EffectTriggerDirection.None, null);
				}
			}
		}
	}

	// Token: 0x040020C0 RID: 8384
	private IEffectTriggerEvent_OnTimeout[] m_effectTriggerEventArray;

	// Token: 0x040020C1 RID: 8385
	private Collider2D m_collider;

	// Token: 0x040020C2 RID: 8386
	private BaseCharacterController m_charController;

	// Token: 0x040020C3 RID: 8387
	private Action<GameObject> m_invokeTimeoutTrigger;
}
