using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D3 RID: 1747
public class OnInteractEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17001445 RID: 5189
	// (get) Token: 0x06003595 RID: 13717 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001446 RID: 5190
	// (get) Token: 0x06003596 RID: 13718 RVA: 0x000E19E0 File Offset: 0x000DFBE0
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

	// Token: 0x06003597 RID: 13719 RVA: 0x0001D6C2 File Offset: 0x0001B8C2
	protected override void Awake()
	{
		base.Awake();
		this.m_charController = this.m_rootObj.GetComponent<BaseCharacterController>();
		if (this.m_charController == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
		}
		this.m_interactableComponent = base.GetComponentInParent<Interactable>();
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x0001D701 File Offset: 0x0001B901
	private void OnEnable()
	{
		this.m_interactableComponent.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.TriggerInteractEvent));
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x0001D71F File Offset: 0x0001B91F
	private void OnDisable()
	{
		this.m_interactableComponent.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.TriggerInteractEvent));
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x000E115C File Offset: 0x000DF35C
	private void TriggerInteractEvent(GameObject otherObj)
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

	// Token: 0x04002B97 RID: 11159
	private Interactable m_interactableComponent;

	// Token: 0x04002B98 RID: 11160
	private Collider2D m_collider;

	// Token: 0x04002B99 RID: 11161
	private BaseCharacterController m_charController;
}
