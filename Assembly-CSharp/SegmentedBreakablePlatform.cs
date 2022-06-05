using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020007EC RID: 2028
public class SegmentedBreakablePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, ITerrainOnExitHitResponse, IRootObj, IRoomConsumer, IBodyOnEnterHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x170016CB RID: 5835
	// (get) Token: 0x06003E74 RID: 15988 RVA: 0x000228C3 File Offset: 0x00020AC3
	// (set) Token: 0x06003E75 RID: 15989 RVA: 0x000228CB File Offset: 0x00020ACB
	public BaseRoom Room { get; private set; }

	// Token: 0x06003E76 RID: 15990 RVA: 0x000228D4 File Offset: 0x00020AD4
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x000228F9 File Offset: 0x00020AF9
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

	// Token: 0x06003E78 RID: 15992 RVA: 0x000FB300 File Offset: 0x000F9500
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

	// Token: 0x06003E79 RID: 15993 RVA: 0x00022908 File Offset: 0x00020B08
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnStayHitResponse(otherHBController);
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x00022911 File Offset: 0x00020B11
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		if (this.m_crumbling)
		{
			return;
		}
		this.HandleProjectileCollision(otherHBController);
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x00022923 File Offset: 0x00020B23
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x000FB3F8 File Offset: 0x000F95F8
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

	// Token: 0x06003E7D RID: 15997 RVA: 0x0002292C File Offset: 0x00020B2C
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		this.m_platformCollider.tag = "TriggerHazard";
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0002293E File Offset: 0x00020B3E
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

	// Token: 0x06003E7F RID: 15999 RVA: 0x000FB454 File Offset: 0x000F9654
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

	// Token: 0x06003E80 RID: 16000 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x00022954 File Offset: 0x00020B54
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetCrumble();
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0002295C File Offset: 0x00020B5C
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x00022983 File Offset: 0x00020B83
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003109 RID: 12553
	private const string SFX_SPAWN_NAME = "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_spawn";

	// Token: 0x0400310A RID: 12554
	private const string SFX_BREAK_NAME = "event:/SFX/Interactables/sfx_env_prop_mushroomPlatform_destroyed";

	// Token: 0x0400310B RID: 12555
	[SerializeField]
	private SpriteRenderer m_dottedOutlineSprite;

	// Token: 0x0400310C RID: 12556
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x0400310D RID: 12557
	protected BoxCollider2D m_platformCollider;

	// Token: 0x0400310E RID: 12558
	protected BoxCollider2D m_bodyCollider;

	// Token: 0x0400310F RID: 12559
	protected int m_storedPlatformLayer;

	// Token: 0x04003110 RID: 12560
	protected int m_storedTerrainLayer;

	// Token: 0x04003111 RID: 12561
	protected int m_storedBodyLayer;

	// Token: 0x04003112 RID: 12562
	protected bool m_crumbling;

	// Token: 0x04003113 RID: 12563
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04003114 RID: 12564
	protected Animator m_animator;

	// Token: 0x04003115 RID: 12565
	protected Coroutine m_crumbleCoroutine;

	// Token: 0x04003116 RID: 12566
	protected Coroutine m_reformCoroutine;
}
