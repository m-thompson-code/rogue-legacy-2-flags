using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200041B RID: 1051
public class OnInteractEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060026E0 RID: 9952 RVA: 0x0008150D File Offset: 0x0007F70D
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00081510 File Offset: 0x0007F710
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

	// Token: 0x060026E2 RID: 9954 RVA: 0x00081569 File Offset: 0x0007F769
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

	// Token: 0x060026E3 RID: 9955 RVA: 0x000815A8 File Offset: 0x0007F7A8
	private void OnEnable()
	{
		this.m_interactableComponent.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.TriggerInteractEvent));
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000815C6 File Offset: 0x0007F7C6
	private void OnDisable()
	{
		this.m_interactableComponent.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.TriggerInteractEvent));
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000815E4 File Offset: 0x0007F7E4
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

	// Token: 0x040020B4 RID: 8372
	private Interactable m_interactableComponent;

	// Token: 0x040020B5 RID: 8373
	private Collider2D m_collider;

	// Token: 0x040020B6 RID: 8374
	private BaseCharacterController m_charController;
}
