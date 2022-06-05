using System;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class OnDeathEffectTrigger : BaseEffectTrigger
{
	// Token: 0x1700143F RID: 5183
	// (get) Token: 0x0600357C RID: 13692 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001440 RID: 5184
	// (get) Token: 0x0600357D RID: 13693 RVA: 0x000E14E0 File Offset: 0x000DF6E0
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

	// Token: 0x0600357E RID: 13694 RVA: 0x000E153C File Offset: 0x000DF73C
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (this.m_charController == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_effectTriggerEventArray = this.m_rootObj.GetComponentsInChildren<IEffectTriggerEvent_OnDeath>();
		this.m_invokeOnDeathTrigger = new Action<GameObject>(this.InvokeOnDeathTrigger);
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x000E15A0 File Offset: 0x000DF7A0
	private void OnEnable()
	{
		IEffectTriggerEvent_OnDeath[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDeathEffectTriggerRelay.AddListener(this.m_invokeOnDeathTrigger, false);
		}
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x000E15D8 File Offset: 0x000DF7D8
	private void OnDisable()
	{
		IEffectTriggerEvent_OnDeath[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDeathEffectTriggerRelay.RemoveListener(this.m_invokeOnDeathTrigger);
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x000E1610 File Offset: 0x000DF810
	public void InvokeOnDeathTrigger(GameObject otherObj)
	{
		BaseCharacterController baseCharacterController;
		GameObject gameObject;
		if (otherObj == null)
		{
			baseCharacterController = PlayerManager.GetPlayerController();
			gameObject = baseCharacterController.gameObject;
		}
		else
		{
			gameObject = otherObj.GetRoot(false);
			baseCharacterController = gameObject.GetComponent<BaseCharacterController>();
		}
		IMidpointObj midpointObj = baseCharacterController;
		IMidpointObj midpointObj2 = midpointObj ?? gameObject.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj2 == null) ? gameObject.transform.position : midpointObj2.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(gameObject, effectTriggerEntry.TriggerDirection);
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
							EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, this.Midpoint, otherObjMidpos, effectDirectionFromObject, null);
						}
					}
					else
					{
						EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, this.Midpoint, otherObjMidpos, effectDirectionFromObject2, null);
					}
				}
				else
				{
					EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, this.Midpoint, otherObjMidpos, EffectTriggerDirection.None, null);
				}
			}
		}
	}

	// Token: 0x04002B8E RID: 11150
	private IEffectTriggerEvent_OnDeath[] m_effectTriggerEventArray;

	// Token: 0x04002B8F RID: 11151
	private Collider2D m_collider;

	// Token: 0x04002B90 RID: 11152
	private BaseCharacterController m_charController;

	// Token: 0x04002B91 RID: 11153
	private Action<GameObject> m_invokeOnDeathTrigger;
}
