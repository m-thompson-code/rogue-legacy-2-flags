using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008B0 RID: 2224
	public class ReenactmentController : MonoBehaviour
	{
		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x0010442A File Offset: 0x0010262A
		public IRelayLink<RoomTrackerData> OnRoomTrackerTriggeredRelay
		{
			get
			{
				return this.m_roomTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x06004888 RID: 18568 RVA: 0x00104437 File Offset: 0x00102637
		public IRelayLink<EnemyTrackerData> OnEnemyTrackerTriggeredRelay
		{
			get
			{
				return this.m_enemyTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x00104444 File Offset: 0x00102644
		public IRelayLink<ChestTrackerData> OnChestTrackerTriggeredRelay
		{
			get
			{
				return this.m_chestTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x00104451 File Offset: 0x00102651
		public IRelayLink<ItemTrackerData> OnItemTrackerTriggeredRelay
		{
			get
			{
				return this.m_itemTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x0010445E File Offset: 0x0010265E
		// (set) Token: 0x0600488C RID: 18572 RVA: 0x00104466 File Offset: 0x00102666
		public bool IsComplete { get; private set; }

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x0010446F File Offset: 0x0010266F
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00104477 File Offset: 0x00102677
		public bool IsFastforwarding { get; private set; }

		// Token: 0x0600488F RID: 18575 RVA: 0x00104480 File Offset: 0x00102680
		private void Awake()
		{
			this.m_waitYield = new WaitRL_Yield(0f, false);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x00104494 File Offset: 0x00102694
		private void CreateChestPreview(IGameEventData gameEvent)
		{
			ChestType chestType = ((ChestTrackerData)gameEvent).ChestType;
			if (AssetPreviewManager_V2.GetChestPreviewEntry(chestType))
			{
				UnityEngine.Object.Instantiate<GameObject>(AssetPreviewManager_V2.GetChestPreviewEntry(chestType), this.m_previewLocation);
				this.m_previewCount++;
				this.TrimPreviewCount();
				return;
			}
			Debug.LogFormat("Failed to find chest preview for {0}", new object[]
			{
				chestType
			});
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x00104500 File Offset: 0x00102700
		private void CreateEnemyPreview(IGameEventData gameEvent)
		{
			EnemyTrackerData enemyTrackerData = (EnemyTrackerData)gameEvent;
			GameObject enemyPreviewEntry = AssetPreviewManager_V2.GetEnemyPreviewEntry(enemyTrackerData.EnemyType, enemyTrackerData.EnemyRank);
			if (enemyPreviewEntry)
			{
				UnityEngine.Object.Instantiate<GameObject>(enemyPreviewEntry, this.m_previewLocation);
				this.m_previewCount++;
				this.TrimPreviewCount();
				return;
			}
			Debug.LogFormat("Failed to find enemy preview for {0}-{1}", new object[]
			{
				enemyTrackerData.EnemyType,
				enemyTrackerData.EnemyRank
			});
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x00104580 File Offset: 0x00102780
		private void TrimPreviewCount()
		{
			int num = this.m_rowConstraintCount * 5;
			if (this.m_previewCount > num)
			{
				for (int i = this.m_previewIndex; i < this.m_previewIndex + this.m_rowConstraintCount; i++)
				{
					base.StartCoroutine(this.TrimPreviewCoroutine(this.m_previewLocation.GetChild(i).gameObject));
				}
				this.m_previewIndex += this.m_rowConstraintCount;
				this.m_previewCount -= this.m_rowConstraintCount;
			}
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x00104600 File Offset: 0x00102800
		private IEnumerator TrimPreviewCoroutine(GameObject previewObj)
		{
			if (this.IsFastforwarding)
			{
				yield return TweenManager.TweenTo_UnscaledTime(previewObj.GetComponent<CanvasGroup>(), 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					0
				}).TweenCoroutine;
			}
			else
			{
				yield return TweenManager.TweenTo_UnscaledTime(previewObj.GetComponent<CanvasGroup>(), 0.5f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					0
				}).TweenCoroutine;
			}
			previewObj.SetActive(false);
			yield break;
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x00104616 File Offset: 0x00102816
		public void FastForward()
		{
			if (this.m_coroutine != null)
			{
				if (this.IsFastforwarding)
				{
					this.m_skipWaiting = true;
				}
				this.m_intervalTime = 0.01f;
				this.IsFastforwarding = true;
			}
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x00104644 File Offset: 0x00102844
		public void Initialise(int constraintCount)
		{
			this.IsFastforwarding = false;
			this.m_intervalTime = 0.15f;
			this.m_skipWaiting = false;
			this.m_rowConstraintCount = constraintCount;
			if (MapController.IsInitialized)
			{
				MapController.SetAllBiomeVisibility(false, false, false);
				MapController.DeathMapCamera.gameObject.SetActive(true);
				MapController.SetCameraFollowIsOn(false);
				MapController.DeathMapCamera.orthographicSize = 3f;
				this.m_mapArea.GetWorldCorners(this.m_worldCorners);
				Vector3 vector = CameraController.UICamera.WorldToScreenPoint(this.m_worldCorners[0]);
				Vector3 vector2 = CameraController.UICamera.WorldToScreenPoint(this.m_worldCorners[2]);
				MapController.DeathMapCamera.pixelRect = new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
				if (GameUtility.IsInGame && GameEventTrackerManager.RoomEventTracker.RoomsEntered.Count == 0)
				{
					GameEventTrackerManager.RoomEventTracker.RoomsEntered.Add(new RoomTrackerData(BiomeType.Castle, 0, default(Vector2), false));
				}
				if (GameEventTrackerManager.RoomEventTracker.RoomsEntered.Count > 0)
				{
					RoomTrackerData roomTrackerData = GameEventTrackerManager.RoomEventTracker.RoomsEntered[0];
					MapController.SetPlayerIconInRoom(roomTrackerData.Biome, roomTrackerData.BiomeControllerIndex, default(Vector2));
					MapController.SetRoomVisible(roomTrackerData.Biome, roomTrackerData.BiomeControllerIndex, true, true);
				}
				else
				{
					MapController.SetPlayerIconInRoom(PlayerManager.GetCurrentPlayerRoom().BiomeType, 0, default(Vector2));
				}
				MapController.CentreCameraAroundPlayerIcon();
			}
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x001047CC File Offset: 0x001029CC
		public void Reset()
		{
			base.StopAllCoroutines();
			this.m_coroutine = null;
			this.IsComplete = false;
			for (int i = 0; i < this.m_previewLocation.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.m_previewLocation.GetChild(i).gameObject);
			}
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x00104819 File Offset: 0x00102A19
		public void Run()
		{
			if (this.m_coroutine == null)
			{
				this.m_coroutine = base.StartCoroutine(this.RunCoroutine());
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x00104835 File Offset: 0x00102A35
		private IEnumerator RunCoroutine()
		{
			this.m_previewIndex = 0;
			this.m_previewCount = 0;
			List<IGameEventData> gameEvents = GameEventTrackerManager.GetGameEvents();
			foreach (IGameEventData gameEventData in gameEvents)
			{
				if (gameEventData is EnemyTrackerData)
				{
					EnemyTrackerData t = (EnemyTrackerData)gameEventData;
					MapController.SetIconVisibility(MapIconType.EnemyKilled, t.Biome, t.BiomeControllerIndex, true, t.EnemyIndex);
					this.m_enemyTrackerTriggeredRelay.Dispatch(t);
					this.CreateEnemyPreview(gameEventData);
					if (!this.m_skipWaiting)
					{
						this.m_waitYield.CreateNew(this.m_intervalTime / 2f, true);
						yield return this.m_waitYield;
					}
				}
				else if (gameEventData is ChestTrackerData)
				{
					ChestTrackerData t2 = (ChestTrackerData)gameEventData;
					this.m_chestTrackerTriggeredRelay.Dispatch(t2);
					MapController.SetIconVisibility(MapIconType.ChestOpened, t2.Biome, t2.BiomeControllerIndex, true, t2.ChestIndex);
					this.CreateChestPreview(gameEventData);
					if (!this.m_skipWaiting)
					{
						this.m_waitYield.CreateNew(this.m_intervalTime, true);
						yield return this.m_waitYield;
					}
				}
				else if (gameEventData is ItemTrackerData)
				{
					this.m_itemTrackerTriggeredRelay.Dispatch((ItemTrackerData)gameEventData);
				}
				else if (gameEventData is RoomTrackerData)
				{
					RoomTrackerData t3 = (RoomTrackerData)gameEventData;
					MapController.SetRoomVisible(t3.Biome, t3.BiomeControllerIndex, true, true);
					MapController.SetPlayerIconInRoom(t3.Biome, t3.BiomeControllerIndex, default(Vector2));
					this.m_roomTrackerTriggeredRelay.Dispatch(t3);
					if (!this.m_skipWaiting)
					{
						this.m_waitYield.CreateNew(this.m_intervalTime, true);
						yield return this.m_waitYield;
					}
					if (MapController.FollowMode == CameraFollowMode.Constant)
					{
						while (MapController.IsCameraMoving)
						{
							yield return null;
						}
					}
				}
			}
			List<IGameEventData>.Enumerator enumerator = default(List<IGameEventData>.Enumerator);
			MapController.SetCameraFollowIsOn(false);
			this.m_coroutine = null;
			this.IsComplete = true;
			yield break;
			yield break;
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00104844 File Offset: 0x00102A44
		public static bool IsTunnelRoom(BaseRoom room)
		{
			Room room2 = room as Room;
			MergeRoom mergeRoom = room as MergeRoom;
			bool result = false;
			if (room2 != null && room2)
			{
				if (room2.GridPointManager.IsTunnelDestination)
				{
					result = true;
				}
			}
			else if (mergeRoom != null && mergeRoom)
			{
				GridPointManager[] standaloneGridPointManagers = mergeRoom.StandaloneGridPointManagers;
				for (int i = 0; i < standaloneGridPointManagers.Length; i++)
				{
					if (standaloneGridPointManagers[i].IsTunnelDestination)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x04003D31 RID: 15665
		private const float TIME_BETWEEN_PREVIEWS = 0.15f;

		// Token: 0x04003D32 RID: 15666
		private const float FASTFORWARD_TIME_BETWEEN_PREVIEWS = 0.01f;

		// Token: 0x04003D33 RID: 15667
		private const int NUM_ROWS_DISPLAYED = 5;

		// Token: 0x04003D34 RID: 15668
		[SerializeField]
		private RectTransform m_mapArea;

		// Token: 0x04003D35 RID: 15669
		[SerializeField]
		private Transform m_previewLocation;

		// Token: 0x04003D36 RID: 15670
		[SerializeField]
		private ChestPreviewController m_chestPreviewPrefab;

		// Token: 0x04003D37 RID: 15671
		[SerializeField]
		private EnemyPreviewController m_enemyPreviewPrefab;

		// Token: 0x04003D38 RID: 15672
		private Coroutine m_coroutine;

		// Token: 0x04003D39 RID: 15673
		private float m_intervalTime;

		// Token: 0x04003D3A RID: 15674
		private WaitRL_Yield m_waitYield;

		// Token: 0x04003D3B RID: 15675
		private Vector3[] m_worldCorners = new Vector3[4];

		// Token: 0x04003D3C RID: 15676
		private bool m_skipWaiting;

		// Token: 0x04003D3D RID: 15677
		private int m_previewCount;

		// Token: 0x04003D3E RID: 15678
		private int m_rowConstraintCount;

		// Token: 0x04003D3F RID: 15679
		private int m_previewIndex;

		// Token: 0x04003D40 RID: 15680
		private Relay<RoomTrackerData> m_roomTrackerTriggeredRelay = new Relay<RoomTrackerData>();

		// Token: 0x04003D41 RID: 15681
		private Relay<EnemyTrackerData> m_enemyTrackerTriggeredRelay = new Relay<EnemyTrackerData>();

		// Token: 0x04003D42 RID: 15682
		private Relay<ChestTrackerData> m_chestTrackerTriggeredRelay = new Relay<ChestTrackerData>();

		// Token: 0x04003D43 RID: 15683
		private Relay<ItemTrackerData> m_itemTrackerTriggeredRelay = new Relay<ItemTrackerData>();
	}
}
