using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class OnHitExtendPlatform : SpecialPlatform, IBodyOnEnterHitResponse, IHitResponse, IRoomConsumer
{
	// Token: 0x1700114B RID: 4427
	// (get) Token: 0x06002D72 RID: 11634 RVA: 0x00099CC1 File Offset: 0x00097EC1
	// (set) Token: 0x06002D73 RID: 11635 RVA: 0x00099CC9 File Offset: 0x00097EC9
	public BaseRoom Room { get; private set; }

	// Token: 0x06002D74 RID: 11636 RVA: 0x00099CD2 File Offset: 0x00097ED2
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00099CE6 File Offset: 0x00097EE6
	protected override IEnumerator Start()
	{
		yield return base.Start();
		this.m_blinkPulseEffect = base.GetComponent<BlinkPulseEffect>();
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		this.m_bodyCollider = this.m_hbController.GetCollider(HitboxType.Body);
		if (base.Width > 0f)
		{
			this.m_platformSprite.size = new Vector2(base.Width, this.m_platformSprite.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		}
		this.m_storedPlatformLayer = this.m_platformCollider.gameObject.layer;
		this.m_storedBodyLayer = this.m_bodyCollider.gameObject.layer;
		this.m_isInitialized = true;
		this.SetPlatformState(this.m_platformExtended);
		yield break;
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x00099CF5 File Offset: 0x00097EF5
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x00099CF8 File Offset: 0x00097EF8
	private void SetPlatformState(bool platformExtended)
	{
		this.m_platformExtended = platformExtended;
		if (this.m_platformExtended)
		{
			this.m_animator.SetBool("Open", true);
			this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
			this.m_bodyCollider.gameObject.layer = 2;
			return;
		}
		this.m_animator.SetBool("Open", false);
		this.m_animator.SetBool("Wobble", false);
		this.m_platformCollider.gameObject.layer = 2;
		this.m_bodyCollider.gameObject.layer = this.m_storedBodyLayer;
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x00099D96 File Offset: 0x00097F96
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.SetPlatformState(!this.m_platformExtended);
		base.StopAllCoroutines();
		this.m_blinkPulseEffect.StopInvincibilityEffect();
		base.StartCoroutine(this.RetractPlatformCoroutine());
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x00099DC5 File Offset: 0x00097FC5
	private IEnumerator RetractPlatformCoroutine()
	{
		float startTime = Time.time;
		bool isBlinking = false;
		while (Time.time < startTime + 3.5f)
		{
			if (!isBlinking && Time.time > startTime + 3.5f - 0.5f)
			{
				this.m_blinkPulseEffect.StartInvincibilityEffect(-1f);
				this.m_animator.SetBool("Wobble", true);
				isBlinking = true;
			}
			yield return null;
		}
		this.m_blinkPulseEffect.StopInvincibilityEffect();
		this.SetPlatformState(!this.m_platformExtended);
		yield break;
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x00099DD4 File Offset: 0x00097FD4
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetPlatform();
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x00099DDC File Offset: 0x00097FDC
	private void ResetPlatform()
	{
		base.StopAllCoroutines();
		this.m_blinkPulseEffect.StopInvincibilityEffect();
		this.m_platformExtended = false;
		this.m_animator.SetBool("Open", false);
		this.m_animator.SetBool("Wobble", false);
		this.m_platformCollider.gameObject.layer = 2;
		this.m_bodyCollider.gameObject.layer = this.m_storedBodyLayer;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x00099E4A File Offset: 0x0009804A
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x00099E71 File Offset: 0x00098071
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x0400246E RID: 9326
	[SerializeField]
	private SpriteRenderer m_platformSprite;

	// Token: 0x0400246F RID: 9327
	private BoxCollider2D m_platformCollider;

	// Token: 0x04002470 RID: 9328
	private Collider2D m_bodyCollider;

	// Token: 0x04002471 RID: 9329
	private BlinkPulseEffect m_blinkPulseEffect;

	// Token: 0x04002472 RID: 9330
	private Animator m_animator;

	// Token: 0x04002473 RID: 9331
	private int m_storedTerrainLayer;

	// Token: 0x04002474 RID: 9332
	private int m_storedPlatformLayer;

	// Token: 0x04002475 RID: 9333
	private int m_storedBodyLayer;

	// Token: 0x04002476 RID: 9334
	private bool m_platformExtended;

	// Token: 0x04002477 RID: 9335
	private bool m_isInitialized;
}
