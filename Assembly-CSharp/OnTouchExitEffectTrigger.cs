using System;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class OnTouchExitEffectTrigger : BaseEffectTrigger, IStandingOnExit
{
	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x06002704 RID: 9988 RVA: 0x00082166 File Offset: 0x00080366
	public override bool RequiresCollider
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x06002705 RID: 9989 RVA: 0x0008216C File Offset: 0x0008036C
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

	// Token: 0x06002706 RID: 9990 RVA: 0x000821A8 File Offset: 0x000803A8
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

	// Token: 0x06002707 RID: 9991 RVA: 0x00082216 File Offset: 0x00080416
	private void OnTriggerExit2D(Collider2D collision)
	{
		base.StopAllCoroutines();
		if (!SceneLoader_RL.IsLoading)
		{
			this.HandleCollision(collision);
		}
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x0008222C File Offset: 0x0008042C
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

	// Token: 0x06002709 RID: 9993 RVA: 0x00082360 File Offset: 0x00080560
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

	// Token: 0x040020C5 RID: 8389
	protected Collider2D m_collider;
}
