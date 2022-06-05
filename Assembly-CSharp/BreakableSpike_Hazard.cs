using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000445 RID: 1093
public class BreakableSpike_Hazard : Hazard, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, IEffectTriggerEvent_OnDeath
{
	// Token: 0x17000FC9 RID: 4041
	// (get) Token: 0x06002815 RID: 10261 RVA: 0x00084C14 File Offset: 0x00082E14
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerEventRelay.link;
		}
	}

	// Token: 0x17000FCA RID: 4042
	// (get) Token: 0x06002816 RID: 10262 RVA: 0x00084C21 File Offset: 0x00082E21
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x00084C28 File Offset: 0x00082E28
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_blinkPulseEffect = base.GetComponent<BlinkPulseEffect>();
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x00084C48 File Offset: 0x00082E48
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

	// Token: 0x06002819 RID: 10265 RVA: 0x00084C84 File Offset: 0x00082E84
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

	// Token: 0x0600281A RID: 10266 RVA: 0x00084C9C File Offset: 0x00082E9C
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

	// Token: 0x0600281B RID: 10267 RVA: 0x00084D75 File Offset: 0x00082F75
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x00084D80 File Offset: 0x00082F80
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

	// Token: 0x04002146 RID: 8518
	[SerializeField]
	private bool m_isTallSpike;

	// Token: 0x04002147 RID: 8519
	[SerializeField]
	private UnityEvent ExtendEvent;

	// Token: 0x04002148 RID: 8520
	[SerializeField]
	private UnityEvent CollisionActivatedEvent;

	// Token: 0x04002149 RID: 8521
	[SerializeField]
	private UnityEvent DestroyedEvent;

	// Token: 0x0400214A RID: 8522
	[SerializeField]
	private BoxCollider2D m_hudCollider;

	// Token: 0x0400214B RID: 8523
	private bool m_destroyed;

	// Token: 0x0400214C RID: 8524
	private IHitboxController m_hbController;

	// Token: 0x0400214D RID: 8525
	private BlinkPulseEffect m_blinkPulseEffect;

	// Token: 0x0400214E RID: 8526
	private Relay<GameObject> m_onDeathEffectTriggerEventRelay = new Relay<GameObject>();
}
