using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class OnLandedEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x060026E7 RID: 9959 RVA: 0x000816DF File Offset: 0x0007F8DF
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000816E4 File Offset: 0x0007F8E4
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

	// Token: 0x060026E9 RID: 9961 RVA: 0x00081740 File Offset: 0x0007F940
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (this.m_charController == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_effectTriggerEventArray = this.m_rootObj.GetComponentsInChildren<IEffectTriggerEvent_OnLanded>();
		this.m_corgiController = this.m_rootObj.GetComponent<CorgiController_RL>();
		this.m_invokeOnLandedTrigger = new Action<CorgiController_RL>(this.InvokeOnLandedTrigger);
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x000817B2 File Offset: 0x0007F9B2
	private void OnEnable()
	{
		base.StartCoroutine(this.AddTrigger());
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x000817C1 File Offset: 0x0007F9C1
	private IEnumerator AddTrigger()
	{
		while (!this.m_corgiController.IsInitialized)
		{
			yield return null;
		}
		IEffectTriggerEvent_OnLanded[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnLandedEffectTriggerRelay.AddListener(this.m_invokeOnLandedTrigger, false);
		}
		yield break;
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x000817D0 File Offset: 0x0007F9D0
	private void OnDisable()
	{
		IEffectTriggerEvent_OnLanded[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnLandedEffectTriggerRelay.RemoveListener(this.m_invokeOnLandedTrigger);
		}
	}

	// Token: 0x060026ED RID: 9965 RVA: 0x00081808 File Offset: 0x0007FA08
	private void InvokeOnLandedTrigger(CorgiController_RL otherCorgi)
	{
		GameObject gameObject = otherCorgi.gameObject;
		BaseCharacterController baseCharacterController;
		GameObject gameObject2;
		if (gameObject == null)
		{
			baseCharacterController = PlayerManager.GetPlayerController();
			gameObject2 = baseCharacterController.gameObject;
		}
		else
		{
			gameObject2 = gameObject.GetRoot(false);
			baseCharacterController = gameObject2.GetComponent<BaseCharacterController>();
		}
		IMidpointObj midpointObj = baseCharacterController;
		IMidpointObj midpointObj2 = midpointObj ?? gameObject2.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj2 == null) ? gameObject2.transform.position : midpointObj2.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectTriggerDirection = EffectTriggerDirection.StandingOn;
			EffectTriggerDirection otherDirection = EffectTriggerDirection.StandingOn;
			bool flag;
			if ((effectTriggerEntry.TriggerDirection & EffectTriggerDirection.StandingOn) != EffectTriggerDirection.None && (effectTriggerEntry.TriggerDirection & effectTriggerEntry.TriggerDirection - 1) == EffectTriggerDirection.None)
			{
				Debug.Log("<color=yellow>Warning: Invalid Effect Trigger Direction(" + EffectTriggerDirection.StandingOn.ToString() + ") for OnDeathEffectTrigger. Effect invoking anyway.");
				flag = true;
			}
			else
			{
				flag = ((effectTriggerEntry.TriggerDirection & effectTriggerDirection) != EffectTriggerDirection.None || effectTriggerEntry.TriggerDirection == EffectTriggerDirection.Anywhere);
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
							EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject2, this.Midpoint, otherObjMidpos, effectTriggerDirection, null);
						}
					}
					else
					{
						EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject2, this.Midpoint, otherObjMidpos, otherDirection, null);
					}
				}
				else
				{
					EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject2, this.Midpoint, otherObjMidpos, EffectTriggerDirection.None, null);
				}
			}
		}
	}

	// Token: 0x040020B7 RID: 8375
	private IEffectTriggerEvent_OnLanded[] m_effectTriggerEventArray;

	// Token: 0x040020B8 RID: 8376
	private Collider2D m_collider;

	// Token: 0x040020B9 RID: 8377
	private BaseCharacterController m_charController;

	// Token: 0x040020BA RID: 8378
	private CorgiController_RL m_corgiController;

	// Token: 0x040020BB RID: 8379
	private Action<CorgiController_RL> m_invokeOnLandedTrigger;
}
