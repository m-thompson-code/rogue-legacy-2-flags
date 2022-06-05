using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
public class MagicPlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnEnterHitResponse, IRootObj, IRoomConsumer, IForcePlatformCollision
{
	// Token: 0x170016C1 RID: 5825
	// (get) Token: 0x06003E3B RID: 15931 RVA: 0x000226DC File Offset: 0x000208DC
	// (set) Token: 0x06003E3C RID: 15932 RVA: 0x000226E4 File Offset: 0x000208E4
	public BaseRoom Room { get; private set; }

	// Token: 0x06003E3D RID: 15933 RVA: 0x000226ED File Offset: 0x000208ED
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x00022713 File Offset: 0x00020913
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

	// Token: 0x06003E3F RID: 15935 RVA: 0x00002FCA File Offset: 0x000011CA
	public void ForcePlatformCollision()
	{
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x00022722 File Offset: 0x00020922
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnStayHitResponse(otherHBController);
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x000FAAD8 File Offset: 0x000F8CD8
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

	// Token: 0x06003E42 RID: 15938 RVA: 0x0002272B File Offset: 0x0002092B
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		this.m_playerIsInRoom = true;
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x00022741 File Offset: 0x00020941
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		this.m_playerIsInRoom = false;
		this.SetInactive();
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0002275D File Offset: 0x0002095D
	private void OnPlayerJump(object sender, EventArgs eventArgs)
	{
		this.SetInactive();
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x0002275D File Offset: 0x0002095D
	private void OnSpawnPlatform(object sender, EventArgs eventArgs)
	{
		this.SetInactive();
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x00022765 File Offset: 0x00020965
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

	// Token: 0x06003E47 RID: 15943 RVA: 0x000FAC88 File Offset: 0x000F8E88
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

	// Token: 0x06003E48 RID: 15944 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x000FAD40 File Offset: 0x000F8F40
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x000FAD90 File Offset: 0x000F8F90
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

	// Token: 0x06003E4D RID: 15949 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040030E2 RID: 12514
	[SerializeField]
	private float m_spawnedPlatformWidth = 4f;

	// Token: 0x040030E3 RID: 12515
	private BoxCollider2D m_terrainCollider;

	// Token: 0x040030E4 RID: 12516
	private BoxCollider2D m_platformCollider;

	// Token: 0x040030E5 RID: 12517
	private SpriteRenderer m_outlineSprite;

	// Token: 0x040030E6 RID: 12518
	private SpriteRenderer m_platformSprite;

	// Token: 0x040030E7 RID: 12519
	private Vector3 m_origColliderPosition;

	// Token: 0x040030E8 RID: 12520
	private bool m_active;

	// Token: 0x040030E9 RID: 12521
	private Animator m_animator;

	// Token: 0x040030EA RID: 12522
	private bool m_playerIsInRoom;

	// Token: 0x040030EB RID: 12523
	private Coroutine m_setInactiveCoroutine;

	// Token: 0x040030EC RID: 12524
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;

	// Token: 0x040030ED RID: 12525
	private static Relay<MonoBehaviour, EventArgs> SpawnPlatformRelay = new Relay<MonoBehaviour, EventArgs>();
}
