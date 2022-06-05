using System;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class OnBounceEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x060026B9 RID: 9913 RVA: 0x00080A43 File Offset: 0x0007EC43
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x060026BA RID: 9914 RVA: 0x00080A46 File Offset: 0x0007EC46
	public override Vector3 Midpoint
	{
		get
		{
			if (this.m_midpointObj != null)
			{
				return this.m_midpointObj.Midpoint;
			}
			return base.transform.position;
		}
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x00080A68 File Offset: 0x0007EC68
	protected override void Awake()
	{
		base.Awake();
		this.m_midpointObj = this.m_rootObj.GetComponent<IMidpointObj>();
		this.m_bounce = this.m_rootObj.GetComponentInChildren<BounceCollision>();
		if (this.m_bounce == null)
		{
			Debug.Log("<color=yellow>WARNING: OnBounce Effect Trigger listening on object that has no BounceCollision component.</color>");
		}
		this.m_triggerBounceEvent = new Action<GameObject>(this.TriggerBounceEvent);
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x00080AC7 File Offset: 0x0007ECC7
	private void OnEnable()
	{
		if (this.m_bounce)
		{
			this.m_bounce.OnBounceRelay.AddListener(this.m_triggerBounceEvent, false);
		}
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x00080AEE File Offset: 0x0007ECEE
	private void OnDisable()
	{
		if (this.m_bounce)
		{
			this.m_bounce.OnBounceRelay.RemoveListener(this.m_triggerBounceEvent);
		}
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x00080B14 File Offset: 0x0007ED14
	private void TriggerBounceEvent(GameObject otherObj)
	{
		GameObject root = otherObj.GetRoot(false);
		IMidpointObj component = root.GetComponent<BaseCharacterController>();
		IMidpointObj midpointObj = component ?? root.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj == null) ? root.transform.position : midpointObj.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(root, effectTriggerEntry.TriggerDirection);
			EffectTriggerDirection effectDirectionFromObject2 = EffectTrigger.GetEffectDirectionFromObject(this.m_rootObj, effectTriggerEntry.TriggerDirection);
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
				EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, root, this.Midpoint, otherObjMidpos, EffectTriggerDirection.Anywhere, null);
			}
		}
	}

	// Token: 0x040020A4 RID: 8356
	private IMidpointObj m_midpointObj;

	// Token: 0x040020A5 RID: 8357
	private BounceCollision m_bounce;

	// Token: 0x040020A6 RID: 8358
	private Action<GameObject> m_triggerBounceEvent;
}
