using System;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
public class OnTouchEnterEffectTrigger : BaseEffectTrigger, IStandingOnEnter
{
	// Token: 0x1700144F RID: 5199
	// (get) Token: 0x060035B8 RID: 13752 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool RequiresCollider
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001450 RID: 5200
	// (get) Token: 0x060035B9 RID: 13753 RVA: 0x000E2244 File Offset: 0x000E0444
	public override Vector3 Midpoint
	{
		get
		{
			if (this.m_collider)
			{
				return this.m_collider.bounds.center;
			}
			return base.transform.position;
		}
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x000E2280 File Offset: 0x000E0480
	protected override void Awake()
	{
		base.Awake();
		this.m_collider = base.GetComponent<Collider2D>();
		if (!this.m_collider)
		{
			bool flag = false;
			EffectTriggerEntry[] triggerArray = base.TriggerArray;
			for (int i = 0; i < triggerArray.Length; i++)
			{
				if ((triggerArray[i].TriggerDirection & EffectTriggerDirection.StandingOn) != EffectTriggerDirection.None)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Debug.Log("<color=red>ERROR: No Collider2D found on OnTouchEnter Effect Trigger on " + this.m_rootObj.name + "</color>");
			}
		}
		else
		{
			this.m_collider.isTrigger = true;
		}
		Rigidbody2D component = base.GetComponent<Rigidbody2D>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
		base.gameObject.layer = LayerMask.NameToLayer("Prop_Hitbox");
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x0001D772 File Offset: 0x0001B972
	private void OnTriggerEnter2D(Collider2D collision)
	{
		base.StopAllCoroutines();
		this.HandleCollision(collision);
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x000E2330 File Offset: 0x000E0530
	private void HandleCollision(Collider2D collision)
	{
		GameObject root = collision.GetRoot(false);
		if (!root.activeInHierarchy)
		{
			return;
		}
		if (!CDGHelper.DoCollisionTypesCollide_V2(base.CanCollideWith, root))
		{
			return;
		}
		IMidpointObj component = root.GetComponent<BaseCharacterController>();
		IMidpointObj midpointObj = component ?? root.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj == null) ? root.transform.position : midpointObj.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(root, effectTriggerEntry.TriggerDirection);
			EffectTriggerDirection effectDirectionFromObject2 = EffectTrigger.GetEffectDirectionFromObject(this.m_rootObj, effectTriggerEntry.TriggerDirection);
			if ((effectTriggerEntry.TriggerDirection & effectDirectionFromObject) != EffectTriggerDirection.None || effectTriggerEntry.TriggerDirection == EffectTriggerDirection.Anywhere)
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

	// Token: 0x060035BD RID: 13757 RVA: 0x000E2458 File Offset: 0x000E0658
	public void OnStandingEnter(GameObject otherRootObj)
	{
		if (!CDGHelper.DoCollisionTypesCollide_V2(base.CanCollideWith, otherRootObj))
		{
			return;
		}
		IMidpointObj component = otherRootObj.GetComponent<BaseCharacterController>();
		IMidpointObj midpointObj = component ?? otherRootObj.GetComponent<IMidpointObj>();
		Vector3 otherObjMidpos = (midpointObj == null) ? otherRootObj.transform.position : midpointObj.Midpoint;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			if ((effectTriggerEntry.TriggerDirection & EffectTriggerDirection.StandingOn) != EffectTriggerDirection.None)
			{
				EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, otherRootObj, this.Midpoint, otherObjMidpos, EffectTriggerDirection.StandingOn, null);
			}
		}
	}

	// Token: 0x04002BAA RID: 11178
	protected Collider2D m_collider;
}
