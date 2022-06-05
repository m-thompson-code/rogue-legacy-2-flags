using System;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class OnSpawnEffectTrigger : BaseEffectTrigger
{
	// Token: 0x1700144B RID: 5195
	// (get) Token: 0x060035AA RID: 13738 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700144C RID: 5196
	// (get) Token: 0x060035AB RID: 13739 RVA: 0x000E1D30 File Offset: 0x000DFF30
	public override Vector3 Midpoint
	{
		get
		{
			if (this.m_charController)
			{
				return this.m_charController.Midpoint;
			}
			if (this.m_collider)
			{
				return this.m_collider.bounds.center;
			}
			return base.gameObject.transform.position;
		}
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000E1D88 File Offset: 0x000DFF88
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (!this.m_charController)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_effectTriggerEventArray = this.m_rootObj.GetComponentsInChildren<IEffectTriggerEvent_OnSpawn>();
		this.m_invokeSpawnTrigger = new Action<GameObject>(this.InvokeSpawnTrigger);
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x000E1DE8 File Offset: 0x000DFFE8
	private void OnEnable()
	{
		IEffectTriggerEvent_OnSpawn[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnSpawnEffectTriggerRelay.AddListener(this.m_invokeSpawnTrigger, false);
		}
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x000E1E20 File Offset: 0x000E0020
	private void OnDisable()
	{
		IEffectTriggerEvent_OnSpawn[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnSpawnEffectTriggerRelay.RemoveListener(this.m_invokeSpawnTrigger);
		}
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x000E1E58 File Offset: 0x000E0058
	private void InvokeSpawnTrigger(GameObject otherObj)
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
				Debug.Log("<color=yellow>Warning: Invalid Effect Trigger Direction(" + EffectTriggerDirection.StandingOn.ToString() + ") for OnSpawnEffectTrigger. Effect invoking anyway.");
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

	// Token: 0x04002BA2 RID: 11170
	private IEffectTriggerEvent_OnSpawn[] m_effectTriggerEventArray;

	// Token: 0x04002BA3 RID: 11171
	private Collider2D m_collider;

	// Token: 0x04002BA4 RID: 11172
	private BaseCharacterController m_charController;

	// Token: 0x04002BA5 RID: 11173
	private Action<GameObject> m_invokeSpawnTrigger;
}
