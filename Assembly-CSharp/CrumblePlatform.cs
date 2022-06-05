using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class CrumblePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, ITerrainOnExitHitResponse, IRootObj, IRoomConsumer, IForcePlatformCollision
{
	// Token: 0x17001147 RID: 4423
	// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000994C7 File Offset: 0x000976C7
	// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000994CF File Offset: 0x000976CF
	public BaseRoom Room { get; private set; }

	// Token: 0x06002D41 RID: 11585 RVA: 0x000994D8 File Offset: 0x000976D8
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetCrumble();
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000994E0 File Offset: 0x000976E0
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x00099505 File Offset: 0x00097705
	protected override IEnumerator Start()
	{
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_terrainCollider = (this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		this.m_platformCollider.tag = "MagicPlatform";
		if (base.Width > 0f)
		{
			SpriteRenderer componentInChildren = base.GetComponentInChildren<SpriteRenderer>();
			componentInChildren.size = new Vector2(base.Width, componentInChildren.size.y);
			this.m_terrainCollider.size = new Vector2(base.Width, this.m_terrainCollider.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
			this.m_dottedOutlineSprite.size = new Vector2(base.Width, this.m_dottedOutlineSprite.size.y);
		}
		this.m_storedTerrainLayer = this.m_terrainCollider.gameObject.layer;
		this.m_storedPlatformLayer = this.m_platformCollider.gameObject.layer;
		yield break;
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x00099514 File Offset: 0x00097714
	public void ForcePlatformCollision()
	{
		this.TerrainOnStayHitResponse(null);
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x0009951D File Offset: 0x0009771D
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x00099528 File Offset: 0x00097728
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (this.m_crumbling)
		{
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsGrounded && playerController.ControllerCorgi.StandingOnCollider == this.m_platformCollider)
		{
			this.m_platformCollider.tag = "TriggerHazard";
			this.m_isTouchingPlatform = true;
			this.m_crumbling = true;
			this.m_animator.SetTrigger("Cracked");
			if (this.m_crumbleCoroutine != null)
			{
				base.StopCoroutine(this.m_crumbleCoroutine);
				this.m_crumbleCoroutine = null;
			}
			this.m_crumbleCoroutine = base.StartCoroutine(this.StartCrumbleCoroutine());
		}
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x000995BF File Offset: 0x000977BF
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		this.m_isTouchingPlatform = false;
		this.m_platformCollider.tag = "MagicPlatform";
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000995D8 File Offset: 0x000977D8
	protected virtual IEnumerator StartCrumbleCoroutine()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (playerController.IsGrounded && this.m_isTouchingPlatform)
		{
			yield return null;
		}
		this.m_platformCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_terrainCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_animator.SetTrigger("Crumble");
		if (this.m_reformCoroutine != null)
		{
			base.StopCoroutine(this.m_reformCoroutine);
			this.m_reformCoroutine = null;
		}
		this.m_reformCoroutine = base.StartCoroutine(this.ReformCoroutine(2f));
		yield break;
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x000995E7 File Offset: 0x000977E7
	protected IEnumerator ReformCoroutine(float duration)
	{
		this.m_waitYield.CreateNew(duration, false);
		yield return this.m_waitYield;
		this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
		this.m_terrainCollider.gameObject.layer = this.m_storedTerrainLayer;
		this.m_animator.SetTrigger("Reform");
		this.m_crumbling = false;
		yield break;
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x00099600 File Offset: 0x00097800
	public virtual void ResetCrumble()
	{
		if (this.m_crumbling)
		{
			this.m_animator.SetTrigger("Reset");
		}
		base.StopAllCoroutines();
		this.m_crumbleCoroutine = null;
		this.m_reformCoroutine = null;
		this.m_crumbling = false;
		this.m_animator.ResetTrigger("Cracked");
		this.m_animator.ResetTrigger("Reform");
		this.m_animator.ResetTrigger("Crumble");
		this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
		this.m_terrainCollider.gameObject.layer = this.m_storedTerrainLayer;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x0009969C File Offset: 0x0009789C
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x0009969E File Offset: 0x0009789E
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x000996C5 File Offset: 0x000978C5
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x000996F9 File Offset: 0x000978F9
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002450 RID: 9296
	[SerializeField]
	private SpriteRenderer m_dottedOutlineSprite;

	// Token: 0x04002451 RID: 9297
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x04002452 RID: 9298
	protected BoxCollider2D m_platformCollider;

	// Token: 0x04002453 RID: 9299
	protected bool m_crumbling;

	// Token: 0x04002454 RID: 9300
	protected int m_storedPlatformLayer;

	// Token: 0x04002455 RID: 9301
	protected int m_storedTerrainLayer;

	// Token: 0x04002456 RID: 9302
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04002457 RID: 9303
	protected Animator m_animator;

	// Token: 0x04002458 RID: 9304
	protected Coroutine m_crumbleCoroutine;

	// Token: 0x04002459 RID: 9305
	protected Coroutine m_reformCoroutine;

	// Token: 0x0400245A RID: 9306
	private bool m_isTouchingPlatform;
}
