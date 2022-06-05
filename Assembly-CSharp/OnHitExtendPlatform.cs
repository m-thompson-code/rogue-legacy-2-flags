using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public class OnHitExtendPlatform : SpecialPlatform, IBodyOnEnterHitResponse, IHitResponse, IRoomConsumer
{
	// Token: 0x170016C6 RID: 5830
	// (get) Token: 0x06003E5A RID: 15962 RVA: 0x000227C8 File Offset: 0x000209C8
	// (set) Token: 0x06003E5B RID: 15963 RVA: 0x000227D0 File Offset: 0x000209D0
	public BaseRoom Room { get; private set; }

	// Token: 0x06003E5C RID: 15964 RVA: 0x000227D9 File Offset: 0x000209D9
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x000227ED File Offset: 0x000209ED
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

	// Token: 0x06003E5E RID: 15966 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E5F RID: 15967 RVA: 0x000FAFF8 File Offset: 0x000F91F8
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

	// Token: 0x06003E60 RID: 15968 RVA: 0x000227FC File Offset: 0x000209FC
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.SetPlatformState(!this.m_platformExtended);
		base.StopAllCoroutines();
		this.m_blinkPulseEffect.StopInvincibilityEffect();
		base.StartCoroutine(this.RetractPlatformCoroutine());
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x0002282B File Offset: 0x00020A2B
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

	// Token: 0x06003E62 RID: 15970 RVA: 0x0002283A File Offset: 0x00020A3A
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetPlatform();
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x000FB098 File Offset: 0x000F9298
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

	// Token: 0x06003E64 RID: 15972 RVA: 0x00022842 File Offset: 0x00020A42
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x00022869 File Offset: 0x00020A69
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x040030F6 RID: 12534
	[SerializeField]
	private SpriteRenderer m_platformSprite;

	// Token: 0x040030F7 RID: 12535
	private BoxCollider2D m_platformCollider;

	// Token: 0x040030F8 RID: 12536
	private Collider2D m_bodyCollider;

	// Token: 0x040030F9 RID: 12537
	private BlinkPulseEffect m_blinkPulseEffect;

	// Token: 0x040030FA RID: 12538
	private Animator m_animator;

	// Token: 0x040030FB RID: 12539
	private int m_storedTerrainLayer;

	// Token: 0x040030FC RID: 12540
	private int m_storedPlatformLayer;

	// Token: 0x040030FD RID: 12541
	private int m_storedBodyLayer;

	// Token: 0x040030FE RID: 12542
	private bool m_platformExtended;

	// Token: 0x040030FF RID: 12543
	private bool m_isInitialized;
}
