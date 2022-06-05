using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
public class Sky : MonoBehaviour
{
	// Token: 0x170016B0 RID: 5808
	// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x00022426 File Offset: 0x00020626
	// (set) Token: 0x06003DE5 RID: 15845 RVA: 0x0002242E File Offset: 0x0002062E
	public List<BaseRoom> RoomList { get; private set; } = new List<BaseRoom>();

	// Token: 0x170016B1 RID: 5809
	// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x00022437 File Offset: 0x00020637
	public Renderer SkyRenderer
	{
		get
		{
			return this.m_skyRenderer;
		}
	}

	// Token: 0x170016B2 RID: 5810
	// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x0002243F File Offset: 0x0002063F
	// (set) Token: 0x06003DE8 RID: 15848 RVA: 0x00022447 File Offset: 0x00020647
	public bool IsHeirloomSky { get; set; }

	// Token: 0x06003DE9 RID: 15849 RVA: 0x000FA0B0 File Offset: 0x000F82B0
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

	// Token: 0x06003DEA RID: 15850 RVA: 0x00022450 File Offset: 0x00020650
	private void OnDestroy()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineage);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x0001AF26 File Offset: 0x00019126
	private void OnEnterLineage(object sender, EventArgs args)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x000FA128 File Offset: 0x000F8328
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

	// Token: 0x06003DED RID: 15853 RVA: 0x000FA2FC File Offset: 0x000F84FC
	private bool GetShouldSkyBeActive(BaseRoom room)
	{
		bool result = false;
		if (this.RoomList.Contains(room))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x040030AF RID: 12463
	private const float BG_PERSP_SCALE_OFFSET = 1.5f;

	// Token: 0x040030B0 RID: 12464
	[SerializeField]
	[ReadOnlyOnPlay]
	private Renderer m_skyRenderer;

	// Token: 0x040030B1 RID: 12465
	private Vector3 m_storedScale = Vector3.one;

	// Token: 0x040030B2 RID: 12466
	private static SkyLightChangedEventArgs m_skylightEventArgs;

	// Token: 0x040030B3 RID: 12467
	private static Sky m_currentSky;

	// Token: 0x040030B4 RID: 12468
	private Action<MonoBehaviour, EventArgs> m_onEnterLineage;

	// Token: 0x040030B5 RID: 12469
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
