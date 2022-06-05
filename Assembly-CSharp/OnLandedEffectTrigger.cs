using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
public class OnLandedEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17001447 RID: 5191
	// (get) Token: 0x0600359C RID: 13724 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001448 RID: 5192
	// (get) Token: 0x0600359D RID: 13725 RVA: 0x000E1A3C File Offset: 0x000DFC3C
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

	// Token: 0x0600359E RID: 13726 RVA: 0x000E1A98 File Offset: 0x000DFC98
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

	// Token: 0x0600359F RID: 13727 RVA: 0x0001D73D File Offset: 0x0001B93D
	private void OnEnable()
	{
		base.StartCoroutine(this.AddTrigger());
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x0001D74C File Offset: 0x0001B94C
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

	// Token: 0x060035A1 RID: 13729 RVA: 0x000E1B0C File Offset: 0x000DFD0C
	private void OnDisable()
	{
		IEffectTriggerEvent_OnLanded[] effectTriggerEventArray = this.m_effectTriggerEventArray;
		for (int i = 0; i < effectTriggerEventArray.Length; i++)
		{
			effectTriggerEventArray[i].OnLandedEffectTriggerRelay.RemoveListener(this.m_invokeOnLandedTrigger);
		}
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x000E1B44 File Offset: 0x000DFD44
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

	// Token: 0x04002B9A RID: 11162
	private IEffectTriggerEvent_OnLanded[] m_effectTriggerEventArray;

	// Token: 0x04002B9B RID: 11163
	private Collider2D m_collider;

	// Token: 0x04002B9C RID: 11164
	private BaseCharacterController m_charController;

	// Token: 0x04002B9D RID: 11165
	private CorgiController_RL m_corgiController;

	// Token: 0x04002B9E RID: 11166
	private Action<CorgiController_RL> m_invokeOnLandedTrigger;
}
