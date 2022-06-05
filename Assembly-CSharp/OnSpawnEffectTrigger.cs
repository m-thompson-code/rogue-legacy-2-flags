using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public class OnSpawnEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060026EF RID: 9967 RVA: 0x0008197E File Offset: 0x0007FB7E
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060026F0 RID: 9968 RVA: 0x00081984 File Offset: 0x0007FB84
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

	// Token: 0x060026F1 RID: 9969 RVA: 0x000819DC File Offset: 0x0007FBDC
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

	// Token: 0x060026F2 RID: 9970 RVA: 0x00081A3C File Offset: 0x0007FC3C
	private void OnEnable()
	{
		IEffectTriggerEvent_OnSpawn[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnSpawnEffectTriggerRelay.AddListener(this.m_invokeSpawnTrigger, false);
		}
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x00081A74 File Offset: 0x0007FC74
	private void OnDisable()
	{
		IEffectTriggerEvent_OnSpawn[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnSpawnEffectTriggerRelay.RemoveListener(this.m_invokeSpawnTrigger);
		}
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x00081AAC File Offset: 0x0007FCAC
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

	// Token: 0x040020BC RID: 8380
	private IEffectTriggerEvent_OnSpawn[] m_effectTriggerEventArray;

	// Token: 0x040020BD RID: 8381
	private Collider2D m_collider;

	// Token: 0x040020BE RID: 8382
	private BaseCharacterController m_charController;

	// Token: 0x040020BF RID: 8383
	private Action<GameObject> m_invokeSpawnTrigger;
}
