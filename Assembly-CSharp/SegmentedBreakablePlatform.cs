using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class SegmentedBreakablePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, ITerrainOnExitHitResponse, IRootObj, IRoomConsumer, IBodyOnEnterHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x1700114C RID: 4428
	// (get) Token: 0x06002D80 RID: 11648 RVA: 0x00099EAD File Offset: 0x000980AD
	// (set) Token: 0x06002D81 RID: 11649 RVA: 0x00099EB5 File Offset: 0x000980B5
	public BaseRoom Room { get; private set; }

	// Token: 0x06002D82 RID: 11650 RVA: 0x00099EBE File Offset: 0x000980BE
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00099EE3 File Offset: 0x000980E3
	protected override IEnumerator Start()
	{
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_terrainCollider = (this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		this.m_bodyCollider = (this.m_hbController.GetCollider(HitboxType.Body) as BoxCollider2D);
		this.m_platformCollider.tag = "TriggerHazard";
		if (base.Width > 0f)
		{
			SpriteRenderer componentInChildren = base.GetComponentInChildren<SpriteRenderer>();
			componentInChildren.size = new Vector2(base.Width, componentInChildren.size.y);
			this.m_terrainCollider.size = new Vector2(base.Width, this.m_terrainCollider.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
			this.m_bodyCollider.size = new Vector2(base.Width, this.m_bodyCollider.size.y);
			this.m_dottedOutlineSprite.size = new Vector2(base.Width, this.m_dottedOutlineSprite.size.y);
		}
		this.m_storedTerrainLayer = this.m_terrainCollider.gameObject.layer;
		this.m_storedPlatformLayer = this.m_platformCollider.gameObject.layer;
		this.m_storedBodyLayer = this.m_bodyCollider.gameObject.layer;
		yield break;
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00099EF4 File Offset: 0x000980F4
	private void HandleProjectileCollision(IHitboxController otherHBController)
	{
		Projectile_RL component = otherHBController.RootGameObject.GetComponent<Projectile_RL>();
		if (component && (component.DieOnCharacterCollision || component.DieOnWallCollision))
		{
			component.FlagForDestruction(null);
		}
		this.m_crumbling = true;
		this.m_platformCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_terrainCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_bodyCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_animator.SetTrigger("Cracked");
		this.m_animator.SetTrigger("Crumble");
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_destroyed", base.transform.position);
		if (this.m_reformCoroutine != null)
		{
			base.StopCoroutine(this.m_reformCoroutine);
			this.m_reformCoroutine = null;
		}
		this.m_reformCoroutine = base.StartCoroutine(this.ReformCoroutine(4f));
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x00099FE9 File Offset: 0x000981E9
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnStayHitResponse(otherHBController);
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x00099FF2 File Offset: 0x000981F2
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		if (this.m_crumbling)
		{
			return;
		}
		this.HandleProjectileCollision(otherHBController);
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x0009A004 File Offset: 0x00098204
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x0009A010 File Offset: 0x00098210
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
		}
		if (otherHBController.CollisionType != CollisionType.Player)
		{
			this.HandleProjectileCollision(otherHBController);
		}
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x0009A06C File Offset: 0x0009826C
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		this.m_platformCollider.tag = "TriggerHazard";
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x0009A07E File Offset: 0x0009827E
	protected IEnumerator ReformCoroutine(float duration)
	{
		this.m_waitYield.CreateNew(duration, false);
		yield return this.m_waitYield;
		this.m_crumbling = false;
		this.m_animator.SetTrigger("Reform");
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_spawn", base.transform.position);
		this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
		this.m_terrainCollider.gameObject.layer = this.m_storedTerrainLayer;
		this.m_bodyCollider.gameObject.layer = this.m_storedBodyLayer;
		yield break;
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x0009A094 File Offset: 0x00098294
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
		this.m_bodyCollider.gameObject.layer = this.m_storedBodyLayer;
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x0009A146 File Offset: 0x00098346
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x0009A148 File Offset: 0x00098348
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetCrumble();
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x0009A150 File Offset: 0x00098350
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x0009A177 File Offset: 0x00098377
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x0009A1AC File Offset: 0x000983AC
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002479 RID: 9337
	private const string SFX_SPAWN_NAME = "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_spawn";

	// Token: 0x0400247A RID: 9338
	private const string SFX_BREAK_NAME = "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_destroyed";

	// Token: 0x0400247B RID: 9339
	[SerializeField]
	private SpriteRenderer m_dottedOutlineSprite;

	// Token: 0x0400247C RID: 9340
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x0400247D RID: 9341
	protected BoxCollider2D m_platformCollider;

	// Token: 0x0400247E RID: 9342
	protected BoxCollider2D m_bodyCollider;

	// Token: 0x0400247F RID: 9343
	protected int m_storedPlatformLayer;

	// Token: 0x04002480 RID: 9344
	protected int m_storedTerrainLayer;

	// Token: 0x04002481 RID: 9345
	protected int m_storedBodyLayer;

	// Token: 0x04002482 RID: 9346
	protected bool m_crumbling;

	// Token: 0x04002483 RID: 9347
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04002484 RID: 9348
	protected Animator m_animator;

	// Token: 0x04002485 RID: 9349
	protected Coroutine m_crumbleCoroutine;

	// Token: 0x04002486 RID: 9350
	protected Coroutine m_reformCoroutine;
}
