using System;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class OnTouchExitEffectTrigger : BaseEffectTrigger, IStandingOnExit
{
	// Token: 0x17001451 RID: 5201
	// (get) Token: 0x060035BF RID: 13759 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool RequiresCollider
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x060035C0 RID: 13760 RVA: 0x000E24E4 File Offset: 0x000E06E4
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

	// Token: 0x060035C1 RID: 13761 RVA: 0x000E2520 File Offset: 0x000E0720
	protected override void Awake()
	{
		base.Awake();
		this.m_collider = base.GetComponent<Collider2D>();
		if (!this.m_collider)
		{
			Debug.Log("<color=red>ERROR: No Collider2D found on OnTouchExit Effect Trigger.</color>");
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

	// Token: 0x060035C2 RID: 13762 RVA: 0x0001D781 File Offset: 0x0001B981
	private void OnTriggerExit2D(Collider2D collision)
	{
		base.StopAllCoroutines();
		if (!SceneLoader_RL.IsLoading)
		{
			this.HandleCollision(collision);
		}
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000E2590 File Offset: 0x000E0790
	private void HandleCollision(Collider2D collision)
	{
		if (CollisionType_RL.IsProjectile(collision.gameObject))
		{
			return;
		}
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

	// Token: 0x060035C4 RID: 13764 RVA: 0x000E2458 File Offset: 0x000E0658
	public void OnStandingExit(GameObject otherRootObj)
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

	// Token: 0x04002BAB RID: 11179
	protected Collider2D m_collider;
}
