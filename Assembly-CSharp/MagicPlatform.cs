using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class MagicPlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, IRootObj, IRoomConsumer, IForcePlatformCollision
{
	// Token: 0x1700114A RID: 4426
	// (get) Token: 0x06002D5F RID: 11615 RVA: 0x000998AF File Offset: 0x00097AAF
	// (set) Token: 0x06002D60 RID: 11616 RVA: 0x000998B7 File Offset: 0x00097AB7
	public BaseRoom Room { get; private set; }

	// Token: 0x06002D61 RID: 11617 RVA: 0x000998C0 File Offset: 0x00097AC0
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x000998E6 File Offset: 0x00097AE6
	protected override IEnumerator Start()
	{
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_terrainCollider = (this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		this.m_platformCollider.tag = "MagicPlatform";
		if (base.Width > 0f)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			this.m_outlineSprite = componentsInChildren[0];
			this.m_platformSprite = componentsInChildren[1];
			this.m_outlineSprite.size = new Vector2(base.Width, this.m_outlineSprite.size.y);
			this.m_platformSprite.size = new Vector2(this.m_spawnedPlatformWidth + 2f, this.m_platformSprite.size.y);
			this.m_origColliderPosition = this.m_terrainCollider.transform.position;
			this.m_terrainCollider.size = new Vector2(base.Width, this.m_terrainCollider.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		}
		MagicPlatform.SpawnPlatformRelay.AddListener(new Action<MonoBehaviour, EventArgs>(this.OnSpawnPlatform), false);
		yield break;
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x000998F5 File Offset: 0x00097AF5
	public void ForcePlatformCollision()
	{
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x000998F7 File Offset: 0x00097AF7
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x00099900 File Offset: 0x00097B00
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (!this.m_active)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController.IsGrounded && playerController.ControllerCorgi.StandingOnCollider == this.m_platformCollider)
			{
				MagicPlatform.SpawnPlatformRelay.Dispatch(this, null);
				this.m_active = true;
				this.m_animator.SetBool("Active", true);
				float num = base.Width / 2f - this.m_spawnedPlatformWidth / 2f;
				float x = Mathf.Clamp(PlayerManager.GetPlayer().transform.position.x, base.transform.position.x - num, base.transform.position.x + num);
				this.m_platformSprite.transform.position = new Vector2(x, this.m_platformSprite.transform.position.y);
				this.m_terrainCollider.size = new Vector2(this.m_spawnedPlatformWidth, this.m_terrainCollider.size.y);
				this.m_terrainCollider.transform.position = new Vector2(x, this.m_terrainCollider.transform.position.y);
				this.m_platformCollider.size = new Vector2(this.m_spawnedPlatformWidth, this.m_platformCollider.size.y);
				this.m_platformCollider.transform.position = new Vector2(x, this.m_platformCollider.transform.position.y);
				this.m_platformCollider.gameObject.layer = 8;
			}
		}
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x00099AAD File Offset: 0x00097CAD
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		this.m_playerIsInRoom = true;
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x00099AC3 File Offset: 0x00097CC3
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		this.m_playerIsInRoom = false;
		this.SetInactive();
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x00099ADF File Offset: 0x00097CDF
	private void OnPlayerJump(object sender, EventArgs eventArgs)
	{
		this.SetInactive();
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x00099AE7 File Offset: 0x00097CE7
	private void OnSpawnPlatform(object sender, EventArgs eventArgs)
	{
		this.SetInactive();
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x00099AEF File Offset: 0x00097CEF
	private IEnumerator SetInactiveCoroutine(float delay)
	{
		if (!this.m_active)
		{
			yield break;
		}
		delay = Time.time + delay;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.SetInactive();
		yield break;
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x00099B08 File Offset: 0x00097D08
	private void SetInactive()
	{
		if (!this.m_active)
		{
			return;
		}
		this.m_active = false;
		this.m_terrainCollider.size = new Vector2(base.Width, this.m_terrainCollider.size.y);
		this.m_terrainCollider.transform.position = this.m_origColliderPosition;
		this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		this.m_platformCollider.transform.position = this.m_origColliderPosition;
		this.m_platformCollider.gameObject.layer = 11;
		this.m_animator.SetBool("Active", false);
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x00099BC0 File Offset: 0x00097DC0
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x00099BC4 File Offset: 0x00097DC4
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x00099C14 File Offset: 0x00097E14
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
		MagicPlatform.SpawnPlatformRelay.RemoveListener(new Action<MonoBehaviour, EventArgs>(this.OnSpawnPlatform));
		if (this.m_playerIsInRoom)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump));
		}
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x00099CB9 File Offset: 0x00097EB9
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002461 RID: 9313
	[SerializeField]
	private float m_spawnedPlatformWidth = 4f;

	// Token: 0x04002462 RID: 9314
	private BoxCollider2D m_terrainCollider;

	// Token: 0x04002463 RID: 9315
	private BoxCollider2D m_platformCollider;

	// Token: 0x04002464 RID: 9316
	private SpriteRenderer m_outlineSprite;

	// Token: 0x04002465 RID: 9317
	private SpriteRenderer m_platformSprite;

	// Token: 0x04002466 RID: 9318
	private Vector3 m_origColliderPosition;

	// Token: 0x04002467 RID: 9319
	private bool m_active;

	// Token: 0x04002468 RID: 9320
	private Animator m_animator;

	// Token: 0x04002469 RID: 9321
	private bool m_playerIsInRoom;

	// Token: 0x0400246A RID: 9322
	private Coroutine m_setInactiveCoroutine;

	// Token: 0x0400246B RID: 9323
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;

	// Token: 0x0400246C RID: 9324
	private static Relay<MonoBehaviour, EventArgs> SpawnPlatformRelay = new Relay<MonoBehaviour, EventArgs>();
}
