using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DE4 RID: 3556
	public class ReenactmentController : MonoBehaviour
	{
		// Token: 0x17002046 RID: 8262
		// (get) Token: 0x060063F1 RID: 25585 RVA: 0x0003728F File Offset: 0x0003548F
		public IRelayLink<RoomTrackerData> OnRoomTrackerTriggeredRelay
		{
			get
			{
				return this.m_roomTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x17002047 RID: 8263
		// (get) Token: 0x060063F2 RID: 25586 RVA: 0x0003729C File Offset: 0x0003549C
		public IRelayLink<EnemyTrackerData> OnEnemyTrackerTriggeredRelay
		{
			get
			{
				return this.m_enemyTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x17002048 RID: 8264
		// (get) Token: 0x060063F3 RID: 25587 RVA: 0x000372A9 File Offset: 0x000354A9
		public IRelayLink<ChestTrackerData> OnChestTrackerTriggeredRelay
		{
			get
			{
				return this.m_chestTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x17002049 RID: 8265
		// (get) Token: 0x060063F4 RID: 25588 RVA: 0x000372B6 File Offset: 0x000354B6
		public IRelayLink<ItemTrackerData> OnItemTrackerTriggeredRelay
		{
			get
			{
				return this.m_itemTrackerTriggeredRelay.link;
			}
		}

		// Token: 0x1700204A RID: 8266
		// (get) Token: 0x060063F5 RID: 25589 RVA: 0x000372C3 File Offset: 0x000354C3
		// (set) Token: 0x060063F6 RID: 25590 RVA: 0x000372CB File Offset: 0x000354CB
		public bool IsComplete { get; private set; }

		// Token: 0x1700204B RID: 8267
		// (get) Token: 0x060063F7 RID: 25591 RVA: 0x000372D4 File Offset: 0x000354D4
		// (set) Token: 0x060063F8 RID: 25592 RVA: 0x000372DC File Offset: 0x000354DC
		public bool IsFastforwarding { get; private set; }

		// Token: 0x060063F9 RID: 25593 RVA: 0x000372E5 File Offset: 0x000354E5
		private void Awake()
		{
			this.m_waitYield = new WaitRL_Yield(0f, false);
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x00172FC4 File Offset: 0x001711C4
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

		// Token: 0x060063FB RID: 25595 RVA: 0x00173030 File Offset: 0x00171230
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

		// Token: 0x060063FC RID: 25596 RVA: 0x001730B0 File Offset: 0x001712B0
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

		// Token: 0x060063FD RID: 25597 RVA: 0x000372F8 File Offset: 0x000354F8
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

		// Token: 0x060063FE RID: 25598 RVA: 0x0003730E File Offset: 0x0003550E
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

		// Token: 0x060063FF RID: 25599 RVA: 0x00173130 File Offset: 0x00171330
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

		// Token: 0x06006400 RID: 25600 RVA: 0x001732B8 File Offset: 0x001714B8
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

		// Token: 0x06006401 RID: 25601 RVA: 0x00037339 File Offset: 0x00035539
		public void Run()
		{
			if (this.m_coroutine == null)
			{
				this.m_coroutine = base.StartCoroutine(this.RunCoroutine());
			}
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x00037355 File Offset: 0x00035555
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

		// Token: 0x06006403 RID: 25603 RVA: 0x00173308 File Offset: 0x00171508
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

		// Token: 0x0400517E RID: 20862
		private const float TIME_BETWEEN_PREVIEWS = 0.15f;

		// Token: 0x0400517F RID: 20863
		private const float FASTFORWARD_TIME_BETWEEN_PREVIEWS = 0.01f;

		// Token: 0x04005180 RID: 20864
		private const int NUM_ROWS_DISPLAYED = 5;

		// Token: 0x04005181 RID: 20865
		[SerializeField]
		private RectTransform m_mapArea;

		// Token: 0x04005182 RID: 20866
		[SerializeField]
		private Transform m_previewLocation;

		// Token: 0x04005183 RID: 20867
		[SerializeField]
		private ChestPreviewController m_chestPreviewPrefab;

		// Token: 0x04005184 RID: 20868
		[SerializeField]
		private EnemyPreviewController m_enemyPreviewPrefab;

		// Token: 0x04005185 RID: 20869
		private Coroutine m_coroutine;

		// Token: 0x04005186 RID: 20870
		private float m_intervalTime;

		// Token: 0x04005187 RID: 20871
		private WaitRL_Yield m_waitYield;

		// Token: 0x04005188 RID: 20872
		private Vector3[] m_worldCorners = new Vector3[4];

		// Token: 0x04005189 RID: 20873
		private bool m_skipWaiting;

		// Token: 0x0400518A RID: 20874
		private int m_previewCount;

		// Token: 0x0400518B RID: 20875
		private int m_rowConstraintCount;

		// Token: 0x0400518C RID: 20876
		private int m_previewIndex;

		// Token: 0x0400518D RID: 20877
		private Relay<RoomTrackerData> m_roomTrackerTriggeredRelay = new Relay<RoomTrackerData>();

		// Token: 0x0400518E RID: 20878
		private Relay<EnemyTrackerData> m_enemyTrackerTriggeredRelay = new Relay<EnemyTrackerData>();

		// Token: 0x0400518F RID: 20879
		private Relay<ChestTrackerData> m_chestTrackerTriggeredRelay = new Relay<ChestTrackerData>();

		// Token: 0x04005190 RID: 20880
		private Relay<ItemTrackerData> m_itemTrackerTriggeredRelay = new Relay<ItemTrackerData>();
	}
}
