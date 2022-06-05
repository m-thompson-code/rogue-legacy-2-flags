using System;
using UnityEngine;

// Token: 0x020006CE RID: 1742
public class OnDamageEffectTrigger : BaseEffectTrigger
{
	// Token: 0x1700143D RID: 5181
	// (get) Token: 0x06003575 RID: 13685 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700143E RID: 5182
	// (get) Token: 0x06003576 RID: 13686 RVA: 0x000E1250 File Offset: 0x000DF450
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

	// Token: 0x06003577 RID: 13687 RVA: 0x000E12AC File Offset: 0x000DF4AC
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

	// Token: 0x06003578 RID: 13688 RVA: 0x000E1310 File Offset: 0x000DF510
	private void OnEnable()
	{
		IEffectTriggerEvent_OnDamage[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDamageEffectTriggerRelay.AddListener(this.m_invokeOnDamageTrigger, false);
		}
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x000E1348 File Offset: 0x000DF548
	private void OnDisable()
	{
		IEffectTriggerEvent_OnDamage[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDamageEffectTriggerRelay.RemoveListener(this.m_invokeOnDamageTrigger);
		}
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x000E1380 File Offset: 0x000DF580
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

	// Token: 0x04002B8A RID: 11146
	private IEffectTriggerEvent_OnDamage[] m_effectTriggerEventArray;

	// Token: 0x04002B8B RID: 11147
	private Collider2D m_collider;

	// Token: 0x04002B8C RID: 11148
	private BaseCharacterController m_charController;

	// Token: 0x04002B8D RID: 11149
	private Action<GameObject, float, bool> m_invokeOnDamageTrigger;
}
