using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class Sky : MonoBehaviour
{
	// Token: 0x17001143 RID: 4419
	// (get) Token: 0x06002D26 RID: 11558 RVA: 0x00099086 File Offset: 0x00097286
	// (set) Token: 0x06002D27 RID: 11559 RVA: 0x0009908E File Offset: 0x0009728E
	public List<BaseRoom> RoomList { get; private set; } = new List<BaseRoom>();

	// Token: 0x17001144 RID: 4420
	// (get) Token: 0x06002D28 RID: 11560 RVA: 0x00099097 File Offset: 0x00097297
	public Renderer SkyRenderer
	{
		get
		{
			return this.m_skyRenderer;
		}
	}

	// Token: 0x17001145 RID: 4421
	// (get) Token: 0x06002D29 RID: 11561 RVA: 0x0009909F File Offset: 0x0009729F
	// (set) Token: 0x06002D2A RID: 11562 RVA: 0x000990A7 File Offset: 0x000972A7
	public bool IsHeirloomSky { get; set; }

	// Token: 0x06002D2B RID: 11563 RVA: 0x000990B0 File Offset: 0x000972B0
	private void Awake()
	{
		this.m_onEnterLineage = new Action<MonoBehaviour, EventArgs>(this.OnEnterLineage);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineage);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		if (this.m_skyRenderer)
		{
			this.m_storedScale = this.m_skyRenderer.transform.localScale;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x00099128 File Offset: 0x00097328
	private void OnDestroy()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineage);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x00099142 File Offset: 0x00097342
	private void OnEnterLineage(object sender, EventArgs args)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x00099160 File Offset: 0x00097360
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			BaseRoom room = roomViaDoorEventArgs.Room;
			if (this.GetShouldSkyBeActive(room))
			{
				if (!base.gameObject.activeInHierarchy)
				{
					base.gameObject.SetActive(true);
				}
				Sky.m_currentSky = this;
			}
			else if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
			}
			if (base.gameObject.activeInHierarchy && room.AppearanceBiomeType != BiomeType.Lineage)
			{
				float x = room.Bounds.center.x;
				float y = room.Bounds.center.y;
				base.transform.position = new Vector2(x, y);
				if (this.m_skyRenderer)
				{
					this.m_skyRenderer.transform.localScale = this.m_storedScale;
					float num = room.Bounds.extents.x / this.m_skyRenderer.bounds.extents.x;
					float num2 = room.Bounds.extents.y / this.m_skyRenderer.bounds.extents.y;
					if (num > 1f || num2 > 1f)
					{
						float num3 = (num > num2) ? num : num2;
						num3 += 0.5f * (CameraController.ZoomLevel - 1f);
						this.m_skyRenderer.transform.localScale = new Vector3(this.m_storedScale.x * num3, this.m_storedScale.y * num3, this.m_storedScale.z);
						return;
					}
				}
			}
		}
		else
		{
			Debug.LogFormat("<color=red>[{0}] Failed to cast eventArgs as RoomViaDoorEventArgs</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x00099334 File Offset: 0x00097534
	private bool GetShouldSkyBeActive(BaseRoom room)
	{
		bool result = false;
		if (this.RoomList.Contains(room))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0400243F RID: 9279
	private const float BG_PERSP_SCALE_OFFSET = 1.5f;

	// Token: 0x04002440 RID: 9280
	[SerializeField]
	[ReadOnlyOnPlay]
	private Renderer m_skyRenderer;

	// Token: 0x04002441 RID: 9281
	private Vector3 m_storedScale = Vector3.one;

	// Token: 0x04002442 RID: 9282
	private static SkyLightChangedEventArgs m_skylightEventArgs;

	// Token: 0x04002443 RID: 9283
	private static Sky m_currentSky;

	// Token: 0x04002444 RID: 9284
	private Action<MonoBehaviour, EventArgs> m_onEnterLineage;

	// Token: 0x04002445 RID: 9285
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
