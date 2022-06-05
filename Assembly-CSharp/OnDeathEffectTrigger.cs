using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class OnDeathEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x060026C7 RID: 9927 RVA: 0x00080EAA File Offset: 0x0007F0AA
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x060026C8 RID: 9928 RVA: 0x00080EB0 File Offset: 0x0007F0B0
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

	// Token: 0x060026C9 RID: 9929 RVA: 0x00080F0C File Offset: 0x0007F10C
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

	// Token: 0x060026CA RID: 9930 RVA: 0x00080F70 File Offset: 0x0007F170
	private void OnEnable()
	{
		IEffectTriggerEvent_OnDeath[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDeathEffectTriggerRelay.AddListener(this.m_invokeOnDeathTrigger, false);
		}
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x00080FA8 File Offset: 0x0007F1A8
	private void OnDisable()
	{
		IEffectTriggerEvent_OnDeath[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnDeathEffectTriggerRelay.RemoveListener(this.m_invokeOnDeathTrigger);
		}
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x00080FE0 File Offset: 0x0007F1E0
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

	// Token: 0x040020AB RID: 8363
	private IEffectTriggerEvent_OnDeath[] m_effectTriggerEventArray;

	// Token: 0x040020AC RID: 8364
	private Collider2D m_collider;

	// Token: 0x040020AD RID: 8365
	private BaseCharacterController m_charController;

	// Token: 0x040020AE RID: 8366
	private Action<GameObject> m_invokeOnDeathTrigger;
}
