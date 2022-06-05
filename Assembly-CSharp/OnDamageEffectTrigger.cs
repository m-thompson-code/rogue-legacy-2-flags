using System;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class OnDamageEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x060026C0 RID: 9920 RVA: 0x00080C0F File Offset: 0x0007EE0F
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x060026C1 RID: 9921 RVA: 0x00080C14 File Offset: 0x0007EE14
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

	// Token: 0x060026C2 RID: 9922 RVA: 0x00080C70 File Offset: 0x0007EE70
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (this.m_charController == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_effectTriggerEventArray = this.m_rootObj.GetComponentsInChildren<IEffectTriggerEvent_OnDamage>();
		this.m_invokeOnDamageTrigger = new Action<GameObject, float, bool>(this.InvokeOnDamageTrigger);
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x00080CD4 File Offset: 0x0007EED4
	private void OnEnable()
	{
		IEffectTriggerEvent_OnDamage[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDamageEffectTriggerRelay.AddListener(this.m_invokeOnDamageTrigger, false);
		}
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x00080D0C File Offset: 0x0007EF0C
	private void OnDisable()
	{
		IEffectTriggerEvent_OnDamage[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDamageEffectTriggerRelay.RemoveListener(this.m_invokeOnDamageTrigger);
		}
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x00080D44 File Offset: 0x0007EF44
	private void InvokeOnDamageTrigger(GameObject attacker, float damageTaken, bool isCrit)
	{
		GameObject root = attacker.GetRoot(false);
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
				Debug.Log("<color=yellow>Warning: Invalid Effect Trigger Direction(" + EffectTriggerDirection.StandingOn.ToString() + ") for OnDeathEffectTrigger. Effect invoking anyway.");
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

	// Token: 0x040020A7 RID: 8359
	private IEffectTriggerEvent_OnDamage[] m_effectTriggerEventArray;

	// Token: 0x040020A8 RID: 8360
	private Collider2D m_collider;

	// Token: 0x040020A9 RID: 8361
	private BaseCharacterController m_charController;

	// Token: 0x040020AA RID: 8362
	private Action<GameObject, float, bool> m_invokeOnDamageTrigger;
}
