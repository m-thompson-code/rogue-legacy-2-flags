using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000713 RID: 1811
public class BreakableSpike_Hazard : Hazard, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, IEffectTriggerEvent_OnDeath
{
	// Token: 0x170014B6 RID: 5302
	// (get) Token: 0x06003745 RID: 14149 RVA: 0x0001E65E File Offset: 0x0001C85E
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerEventRelay.link;
		}
	}

	// Token: 0x170014B7 RID: 5303
	// (get) Token: 0x06003746 RID: 14150 RVA: 0x00005391 File Offset: 0x00003591
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0001E66B File Offset: 0x0001C86B
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_blinkPulseEffect = base.GetComponent<BlinkPulseEffect>();
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0001E68B File Offset: 0x0001C88B
	private void OnEnable()
	{
		if (this.m_isTallSpike)
		{
			base.StartCoroutine(this.ExtendCoroutine(0.2f));
		}
		else
		{
			base.StartCoroutine(this.ExtendCoroutine(0.2f));
		}
		this.m_hudCollider.enabled = true;
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x0001E6C7 File Offset: 0x0001C8C7
	private IEnumerator ExtendCoroutine(float extendDelay)
	{
		float startTime = Time.time;
		while (Time.time < startTime + extendDelay)
		{
			yield return null;
		}
		this.m_destroyed = false;
		this.m_hbController.SetHitboxActiveState(HitboxType.Body, true);
		if (this.ExtendEvent != null)
		{
			this.ExtendEvent.Invoke();
		}
		base.Animator.SetBool("Extend", true);
		if (this.m_isTallSpike)
		{
			while (Time.time < startTime + extendDelay + 1.3f)
			{
				yield return null;
			}
		}
		else
		{
			while (Time.time < startTime + extendDelay + 1f)
			{
				yield return null;
			}
		}
		if (this.CollisionActivatedEvent != null)
		{
			this.CollisionActivatedEvent.Invoke();
		}
		this.m_blinkPulseEffect.BlinkOnHitTint = new Color(1f, 0f, 0f, 0.8f);
		this.m_blinkPulseEffect.StartSingleBlinkEffect();
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, true);
		yield break;
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x000E559C File Offset: 0x000E379C
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!this.m_destroyed)
		{
			if (this.DestroyedEvent != null)
			{
				this.DestroyedEvent.Invoke();
			}
			base.StopAllCoroutines();
			this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
			this.m_hbController.SetHitboxActiveState(HitboxType.Body, false);
			this.m_blinkPulseEffect.BlinkOnHitTint = new Color(1f, 1f, 1f, 0.8f);
			this.m_blinkPulseEffect.StartSingleBlinkEffect();
			base.Animator.SetBool("Extend", false);
			base.Animator.Play("Retract");
			this.m_onDeathEffectTriggerEventRelay.Dispatch(otherHBController.RootGameObject);
			this.m_hudCollider.enabled = false;
			this.m_destroyed = true;
			if (this.m_isTallSpike)
			{
				base.StartCoroutine(this.ExtendCoroutine(6.5f));
			}
		}
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x0001E6DD File Offset: 0x0001C8DD
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x000E5678 File Offset: 0x000E3878
	public override void ResetHazard()
	{
		this.m_destroyed = false;
		this.m_hbController.SetHitboxActiveState(HitboxType.Body, false);
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		base.Animator.SetBool("Extend", false);
		if (base.gameObject.activeInHierarchy)
		{
			base.Animator.Update(0f);
			base.Animator.Update(0f);
		}
	}

	// Token: 0x04002C89 RID: 11401
	[SerializeField]
	private bool m_isTallSpike;

	// Token: 0x04002C8A RID: 11402
	[SerializeField]
	private UnityEvent ExtendEvent;

	// Token: 0x04002C8B RID: 11403
	[SerializeField]
	private UnityEvent CollisionActivatedEvent;

	// Token: 0x04002C8C RID: 11404
	[SerializeField]
	private UnityEvent DestroyedEvent;

	// Token: 0x04002C8D RID: 11405
	[SerializeField]
	private BoxCollider2D m_hudCollider;

	// Token: 0x04002C8E RID: 11406
	private bool m_destroyed;

	// Token: 0x04002C8F RID: 11407
	private IHitboxController m_hbController;

	// Token: 0x04002C90 RID: 11408
	private BlinkPulseEffect m_blinkPulseEffect;

	// Token: 0x04002C91 RID: 11409
	private Relay<GameObject> m_onDeathEffectTriggerEventRelay = new Relay<GameObject>();
}
