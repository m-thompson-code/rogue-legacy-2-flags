using System;
using UnityEngine;

// Token: 0x020006CD RID: 1741
public class OnBounceEffectTrigger : BaseEffectTrigger
{
	// Token: 0x1700143B RID: 5179
	// (get) Token: 0x0600356E RID: 13678 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700143C RID: 5180
	// (get) Token: 0x0600356F RID: 13679 RVA: 0x0001D513 File Offset: 0x0001B713
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

	// Token: 0x06003570 RID: 13680 RVA: 0x000E10FC File Offset: 0x000DF2FC
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

	// Token: 0x06003571 RID: 13681 RVA: 0x0001D534 File Offset: 0x0001B734
	private void OnEnable()
	{
		if (this.m_bounce)
		{
			this.m_bounce.OnBounceRelay.AddListener(this.m_triggerBounceEvent, false);
		}
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x0001D55B File Offset: 0x0001B75B
	private void OnDisable()
	{
		if (this.m_bounce)
		{
			this.m_bounce.OnBounceRelay.RemoveListener(this.m_triggerBounceEvent);
		}
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x000E115C File Offset: 0x000DF35C
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

	// Token: 0x04002B87 RID: 11143
	private IMidpointObj m_midpointObj;

	// Token: 0x04002B88 RID: 11144
	private BounceCollision m_bounce;

	// Token: 0x04002B89 RID: 11145
	private Action<GameObject> m_triggerBounceEvent;
}
