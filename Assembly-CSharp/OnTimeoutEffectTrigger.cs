using System;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
public class OnTimeoutEffectTrigger : BaseEffectTrigger
{
	// Token: 0x1700144D RID: 5197
	// (get) Token: 0x060035B1 RID: 13745 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x060035B2 RID: 13746 RVA: 0x000E1FB8 File Offset: 0x000E01B8
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

	// Token: 0x060035B3 RID: 13747 RVA: 0x000E2014 File Offset: 0x000E0214
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

	// Token: 0x060035B4 RID: 13748 RVA: 0x000E2074 File Offset: 0x000E0274
	private void OnEnable()
	{
		IEffectTriggerEvent_OnTimeout[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnTimeoutEffectTriggerRelay.AddListener(this.m_invokeTimeoutTrigger, false);
		}
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x000E20AC File Offset: 0x000E02AC
	private void OnDisable()
	{
		IEffectTriggerEvent_OnTimeout[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnTimeoutEffectTriggerRelay.RemoveListener(this.m_invokeTimeoutTrigger);
		}
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x000E20E4 File Offset: 0x000E02E4
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

	// Token: 0x04002BA6 RID: 11174
	private IEffectTriggerEvent_OnTimeout[] m_effectTriggerEventArray;

	// Token: 0x04002BA7 RID: 11175
	private Collider2D m_collider;

	// Token: 0x04002BA8 RID: 11176
	private BaseCharacterController m_charController;

	// Token: 0x04002BA9 RID: 11177
	private Action<GameObject> m_invokeTimeoutTrigger;
}
