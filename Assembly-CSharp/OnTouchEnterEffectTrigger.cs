using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class OnTouchEnterEffectTrigger : BaseEffectTrigger, IStandingOnEnter
{
	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x060026FD RID: 9981 RVA: 0x00081EAA File Offset: 0x000800AA
	public override bool RequiresCollider
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x060026FE RID: 9982 RVA: 0x00081EB0 File Offset: 0x000800B0
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

	// Token: 0x060026FF RID: 9983 RVA: 0x00081EEC File Offset: 0x000800EC
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

	// Token: 0x06002700 RID: 9984 RVA: 0x00081F9A File Offset: 0x0008019A
	private void OnTriggerEnter2D(Collider2D collision)
	{
		base.StopAllCoroutines();
		this.HandleCollision(collision);
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x00081FAC File Offset: 0x000801AC
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

	// Token: 0x06002702 RID: 9986 RVA: 0x000820D4 File Offset: 0x000802D4
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

	// Token: 0x040020C4 RID: 8388
	protected Collider2D m_collider;
}
