using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007DE RID: 2014
public class CrumblePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, ITerrainOnExitHitResponse, IRootObj, IRoomConsumer, IForcePlatformCollision
{
	// Token: 0x170016B6 RID: 5814
	// (get) Token: 0x06003E03 RID: 15875 RVA: 0x00022528 File Offset: 0x00020728
	// (set) Token: 0x06003E04 RID: 15876 RVA: 0x00022530 File Offset: 0x00020730
	public BaseRoom Room { get; private set; }

	// Token: 0x06003E05 RID: 15877 RVA: 0x00022539 File Offset: 0x00020739
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetCrumble();
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00022541 File Offset: 0x00020741
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x00022566 File Offset: 0x00020766
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

	// Token: 0x06003E08 RID: 15880 RVA: 0x00022575 File Offset: 0x00020775
	public void ForcePlatformCollision()
	{
		this.TerrainOnStayHitResponse(null);
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x0002257E File Offset: 0x0002077E
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x000FA4BC File Offset: 0x000F86BC
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

	// Token: 0x06003E0B RID: 15883 RVA: 0x00022587 File Offset: 0x00020787
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		this.m_isTouchingPlatform = false;
		this.m_platformCollider.tag = "MagicPlatform";
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x000225A0 File Offset: 0x000207A0
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

	// Token: 0x06003E0D RID: 15885 RVA: 0x000225AF File Offset: 0x000207AF
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

	// Token: 0x06003E0E RID: 15886 RVA: 0x000FA554 File Offset: 0x000F8754
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

	// Token: 0x06003E0F RID: 15887 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x000225C5 File Offset: 0x000207C5
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x000225EC File Offset: 0x000207EC
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040030C3 RID: 12483
	[SerializeField]
	private SpriteRenderer m_dottedOutlineSprite;

	// Token: 0x040030C4 RID: 12484
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x040030C5 RID: 12485
	protected BoxCollider2D m_platformCollider;

	// Token: 0x040030C6 RID: 12486
	protected bool m_crumbling;

	// Token: 0x040030C7 RID: 12487
	protected int m_storedPlatformLayer;

	// Token: 0x040030C8 RID: 12488
	protected int m_storedTerrainLayer;

	// Token: 0x040030C9 RID: 12489
	protected WaitRL_Yield m_waitYield;

	// Token: 0x040030CA RID: 12490
	protected Animator m_animator;

	// Token: 0x040030CB RID: 12491
	protected Coroutine m_crumbleCoroutine;

	// Token: 0x040030CC RID: 12492
	protected Coroutine m_reformCoroutine;

	// Token: 0x040030CD RID: 12493
	private bool m_isTouchingPlatform;
}
