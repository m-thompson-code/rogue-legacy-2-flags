using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x020008BD RID: 2237
	public class TeleporterWindowController : MapWindowController
	{
		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x0600492A RID: 18730 RVA: 0x00107A5C File Offset: 0x00105C5C
		public override WindowID ID
		{
			get
			{
				return WindowID.Teleporter;
			}
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x00107A60 File Offset: 0x00105C60
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete);
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_changeSelectedTeleporter = new Action<InputActionEventData>(this.ChangeSelectedTeleporter);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x00107AC9 File Offset: 0x00105CC9
		private void OnEnable()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x00107AE3 File Offset: 0x00105CE3
		private void OnDisable()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x00107AFD File Offset: 0x00105CFD
		private void OnWorldCreationComplete(object sender, EventArgs args)
		{
			base.StartCoroutine(this.InitializeTeleporters());
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x00107B0C File Offset: 0x00105D0C
		private void OnPlayerEnterRoom(object sender, EventArgs args)
		{
			if (!base.IsInitialized)
			{
				return;
			}
			BaseRoom room = (args as RoomViaDoorEventArgs).Room;
			this.SetEntryVisible(room.AppearanceBiomeType, 0);
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00107B3B File Offset: 0x00105D3B
		private void SetEntryVisible(BiomeType biomeType, int index)
		{
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00107B3D File Offset: 0x00105D3D
		public override void Initialize()
		{
			base.Initialize();
			if (GameUtility.IsInLevelEditor)
			{
				base.StartCoroutine(this.InitializeTeleporters());
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00107B59 File Offset: 0x00105D59
		private IEnumerator InitializeTeleporters()
		{
			while (!MapController.IsInitialized)
			{
				yield return null;
			}
			this.m_activeSubEntryList = new List<TeleporterSubEntry>();
			this.m_teleporterEntriesDict = new SortedDictionary<BiomeType, TeleporterEntry>(new TeleporterWindowController.TeleporterEntrySorter());
			foreach (GridPointManager gridPointManager in MapController.GridPointManagersContainingTeleporters)
			{
				this.AddTeleporterEntry(gridPointManager);
			}
			this.SortTeleporterEntries();
			this.SortTeleporterSubEntries();
			yield break;
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00107B68 File Offset: 0x00105D68
		public void AddTeleporterEntry(GridPointManager gridPointManager)
		{
			TeleporterEntry teleporterEntry = null;
			BiomeType groupedBiomeType = BiomeType_RL.GetGroupedBiomeType(gridPointManager.Biome);
			RoomType roomType = gridPointManager.RoomType;
			GlobalTeleporterType subEntryType;
			if (roomType != RoomType.Transition)
			{
				if (roomType != RoomType.BossEntrance)
				{
					subEntryType = GlobalTeleporterType.SpecialRoom;
				}
				else
				{
					subEntryType = GlobalTeleporterType.BossEntrance;
				}
			}
			else
			{
				subEntryType = GlobalTeleporterType.TransitionRoom;
			}
			if (this.m_teleporterEntriesDict.TryGetValue(groupedBiomeType, out teleporterEntry))
			{
				teleporterEntry.AddSubEntry(subEntryType, gridPointManager);
				return;
			}
			TeleporterEntry teleporterEntry2 = UnityEngine.Object.Instantiate<TeleporterEntry>(this.m_teleporterEntryPrefab, this.m_teleporterEntries.transform);
			teleporterEntry2.Initialize();
			teleporterEntry2.SetBiomeType(groupedBiomeType);
			teleporterEntry2.AddSubEntry(subEntryType, gridPointManager);
			teleporterEntry2.gameObject.SetActive(false);
			this.m_teleporterEntriesDict.Add(groupedBiomeType, teleporterEntry2);
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00107C0C File Offset: 0x00105E0C
		private void SortTeleporterEntries()
		{
			foreach (KeyValuePair<BiomeType, TeleporterEntry> keyValuePair in this.m_teleporterEntriesDict)
			{
				TeleporterEntry value = keyValuePair.Value;
				int num = Map_EV.BIOME_DISPLAY_ORDER.IndexOf(value.BiomeType);
				if (num == -1)
				{
					value.transform.SetAsLastSibling();
				}
				else
				{
					value.transform.SetSiblingIndex(num);
				}
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00107C90 File Offset: 0x00105E90
		private void SortTeleporterSubEntries()
		{
			foreach (KeyValuePair<BiomeType, TeleporterEntry> keyValuePair in this.m_teleporterEntriesDict)
			{
				TeleporterEntry value = keyValuePair.Value;
				TeleporterSubEntry teleporterSubEntry = null;
				TeleporterSubEntry teleporterSubEntry2 = null;
				foreach (TeleporterSubEntry teleporterSubEntry3 in value.SubEntriesList)
				{
					if (teleporterSubEntry3.SubEntryType == GlobalTeleporterType.BossEntrance)
					{
						teleporterSubEntry = teleporterSubEntry3;
					}
					else if (teleporterSubEntry3.SubEntryType == GlobalTeleporterType.TransitionRoom)
					{
						teleporterSubEntry2 = teleporterSubEntry3;
					}
					if (teleporterSubEntry && teleporterSubEntry2)
					{
						break;
					}
				}
				if (teleporterSubEntry)
				{
					value.SubEntriesList.Remove(teleporterSubEntry);
					value.SubEntriesList.Add(teleporterSubEntry);
					teleporterSubEntry.transform.SetAsLastSibling();
				}
				if (teleporterSubEntry2)
				{
					value.SubEntriesList.Remove(teleporterSubEntry2);
					value.SubEntriesList.Insert(0, teleporterSubEntry2);
					teleporterSubEntry2.transform.SetAsFirstSibling();
				}
			}
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00107DBC File Offset: 0x00105FBC
		private int SubEntrySorter(TeleporterSubEntry entry1, TeleporterSubEntry entry2)
		{
			if (entry1 == entry2)
			{
				return 0;
			}
			Vector3 position = PlayerManager.GetPlayer().transform.position;
			Vector3 position2 = MapController.GetMapRoomEntry(entry1.BiomeType, entry1.BiomeControllerIndex).Terrain.transform.position;
			Vector3 position3 = MapController.GetMapRoomEntry(entry2.BiomeType, entry2.BiomeControllerIndex).Terrain.transform.position;
			float num = CDGHelper.DistanceBetweenPts(position, position2);
			float num2 = CDGHelper.DistanceBetweenPts(position, position3);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00107E58 File Offset: 0x00106058
		protected override void OnOpen()
		{
			foreach (KeyValuePair<BiomeType, TeleporterEntry> keyValuePair in this.m_teleporterEntriesDict)
			{
				TeleporterEntry value = keyValuePair.Value;
				foreach (TeleporterSubEntry teleporterSubEntry in value.SubEntriesList)
				{
					teleporterSubEntry.gameObject.SetActive(false);
				}
				value.gameObject.SetActive(false);
			}
			this.m_activeSubEntryList.Clear();
			foreach (KeyValuePair<BiomeType, TeleporterEntry> keyValuePair2 in this.m_teleporterEntriesDict)
			{
				BiomeType key = keyValuePair2.Key;
				TeleporterEntry value2 = keyValuePair2.Value;
				bool flag = false;
				foreach (TeleporterSubEntry teleporterSubEntry2 in value2.SubEntriesList)
				{
					if (MapController.WasRoomVisited(key, teleporterSubEntry2.BiomeControllerIndex))
					{
						teleporterSubEntry2.gameObject.SetActive(true);
						this.m_activeSubEntryList.Add(teleporterSubEntry2);
						flag = true;
					}
				}
				if (flag && !value2.gameObject.activeSelf)
				{
					value2.gameObject.SetActive(true);
				}
			}
			if (TraitManager.IsTraitActive(TraitType.MapReveal))
			{
				MapController.SetPlayerIconVisible(true);
				BiomeType biomeType = PlayerManager.GetCurrentPlayerRoom().BiomeType;
				using (List<TeleporterSubEntry>.Enumerator enumerator2 = this.m_activeSubEntryList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TeleporterSubEntry teleporterSubEntry3 = enumerator2.Current;
						if (teleporterSubEntry3.BiomeType != biomeType)
						{
							MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(teleporterSubEntry3.BiomeType, teleporterSubEntry3.BiomeControllerIndex);
							if (mapRoomEntry)
							{
								mapRoomEntry.gameObject.SetActive(true);
								RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(teleporterSubEntry3.BiomeType, teleporterSubEntry3.BiomeControllerIndex);
								if (roomSaveData.IsNativeNull() || !roomSaveData.RoomVisited)
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.Teleporter, -1, true);
									mapRoomEntry.ToggleTerrainVisibility(true);
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
								}
							}
						}
					}
					goto IL_2C8;
				}
			}
			BiomeType biomeType2 = PlayerManager.GetCurrentPlayerRoom().BiomeType;
			foreach (TeleporterSubEntry teleporterSubEntry4 in this.m_activeSubEntryList)
			{
				MapRoomEntry mapRoomEntry2 = MapController.GetMapRoomEntry(teleporterSubEntry4.BiomeType, teleporterSubEntry4.BiomeControllerIndex);
				if (mapRoomEntry2 && mapRoomEntry2.WasVisited && mapRoomEntry2.RoomType != RoomType.BossEntrance)
				{
					if (mapRoomEntry2.HasTeleporterIcon)
					{
						mapRoomEntry2.ToggleIconVisibility(MapIconType.Teleporter, -1, true);
						mapRoomEntry2.ToggleIconVisibility(MapIconType.SpecialRoom, -1, false);
						mapRoomEntry2.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
					}
					mapRoomEntry2.ToggleTerrainVisibility(true);
				}
			}
			IL_2C8:
			if (this.m_activeSubEntryList.Count > 0)
			{
				BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
				int activeTeleporterIndex = this.GetActiveTeleporterIndex(currentPlayerRoom);
				if (activeTeleporterIndex != -1)
				{
					base.StartCoroutine(this.SetTeleporterIconsCoroutine(activeTeleporterIndex));
				}
				else
				{
					Debug.LogWarning("<color=red>You somehow opened the teleporter window in a room without a teleporter in it.</color>");
				}
			}
			MapController.SetTeleporterLineVisible(true);
			base.OnOpen();
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x001081C4 File Offset: 0x001063C4
		private int GetActiveTeleporterIndex(BaseRoom room)
		{
			for (int i = 0; i < this.m_activeSubEntryList.Count; i++)
			{
				TeleporterSubEntry teleporterSubEntry = this.m_activeSubEntryList[i];
				if (teleporterSubEntry.BiomeControllerIndex == room.BiomeControllerIndex && teleporterSubEntry.BiomeType == room.BiomeType)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x00108213 File Offset: 0x00106413
		private IEnumerator SetTeleporterIconsCoroutine(int teleporterIndex)
		{
			yield return null;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_teleporterEntries);
			this.SetPreviouslySelectedTeleporter(teleporterIndex);
			this.SelectTeleporter(teleporterIndex);
			yield break;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x0010822C File Offset: 0x0010642C
		protected override void OnClose()
		{
			MapController.SetTeleporterLineVisible(false);
			if (TraitManager.IsTraitActive(TraitType.MapReveal))
			{
				MapController.SetPlayerIconVisible(false);
				BiomeType biomeType = PlayerManager.GetCurrentPlayerRoom().BiomeType;
				using (List<TeleporterSubEntry>.Enumerator enumerator = this.m_activeSubEntryList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TeleporterSubEntry teleporterSubEntry = enumerator.Current;
						if (teleporterSubEntry.BiomeType != biomeType)
						{
							MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(teleporterSubEntry.BiomeType, teleporterSubEntry.BiomeControllerIndex);
							if (mapRoomEntry)
							{
								mapRoomEntry.gameObject.SetActive(false);
								RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(teleporterSubEntry.BiomeType, teleporterSubEntry.BiomeControllerIndex);
								if (roomSaveData.IsNativeNull() || !roomSaveData.RoomVisited)
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.Teleporter, -1, false);
									mapRoomEntry.ToggleTerrainVisibility(false);
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
								}
							}
						}
					}
					goto IL_1A7;
				}
			}
			foreach (TeleporterSubEntry teleporterSubEntry2 in this.m_activeSubEntryList)
			{
				MapRoomEntry mapRoomEntry2 = MapController.GetMapRoomEntry(teleporterSubEntry2.BiomeType, teleporterSubEntry2.BiomeControllerIndex);
				if (mapRoomEntry2 && mapRoomEntry2.WasVisited && mapRoomEntry2.RoomType != RoomType.BossEntrance)
				{
					RoomSaveData roomSaveData2 = SaveManager.StageSaveData.GetRoomSaveData(teleporterSubEntry2.BiomeType, teleporterSubEntry2.BiomeControllerIndex);
					if (!roomSaveData2.IsNativeNull() && mapRoomEntry2.HasTeleporterIcon)
					{
						mapRoomEntry2.ToggleIconVisibility(MapIconType.Teleporter, -1, !mapRoomEntry2.HasSpecialRoomIcon);
						mapRoomEntry2.ToggleIconVisibility(MapIconType.SpecialRoom, -1, !roomSaveData2.IsRoomComplete);
						mapRoomEntry2.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, roomSaveData2.IsRoomComplete);
					}
					mapRoomEntry2.ToggleTerrainVisibility(true);
				}
			}
			IL_1A7:
			base.OnClose();
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x00108404 File Offset: 0x00106604
		private void SetPreviouslySelectedTeleporter(int index)
		{
			float y = this.m_activeSubEntryList[index].transform.position.y;
			Vector3 position = this.m_currentlyVisitedIcon.transform.position;
			position.y = y;
			this.m_currentlyVisitedIcon.transform.position = position;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00108458 File Offset: 0x00106658
		private void SelectTeleporter(int index)
		{
			this.m_selectedSubEntryIndex = index;
			foreach (TeleporterSubEntry teleporterSubEntry in this.m_activeSubEntryList)
			{
				teleporterSubEntry.SetSelected(false);
			}
			TeleporterSubEntry teleporterSubEntry2 = this.m_activeSubEntryList[this.m_selectedSubEntryIndex];
			float y = teleporterSubEntry2.transform.position.y;
			Vector3 position = this.m_currentSelectedIcon.transform.position;
			position.y = y;
			this.m_currentSelectedIcon.transform.position = position;
			this.m_activeSubEntryList[index].SetSelected(true);
			MapController.TweenCameraToPosition(MapController.GetMapPositionFromWorld(teleporterSubEntry2.GridPointManager.Bounds.center, false), 0.15f, false);
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00108534 File Offset: 0x00106734
		protected override void SubscribeToRewiredInputEvents()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.AddInputEventDelegate(this.m_changeSelectedTeleporter, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
				base.RewiredPlayer.AddInputEventDelegate(this.m_changeSelectedTeleporter, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x001085C4 File Offset: 0x001067C4
		protected override void UnsubscribeFromRewiredInputEvents()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_changeSelectedTeleporter, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_changeSelectedTeleporter, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x00108654 File Offset: 0x00106854
		protected void ChangeSelectedTeleporter(InputActionEventData inputActionEventData)
		{
			if (this.m_activeSubEntryList.Count <= 0)
			{
				return;
			}
			int selectedSubEntryIndex = this.m_selectedSubEntryIndex;
			int num = this.m_selectedSubEntryIndex;
			float num2 = inputActionEventData.GetAxis();
			if (num2 == 0f)
			{
				num2 = -inputActionEventData.GetAxis();
			}
			if (num2 < 0f)
			{
				num++;
			}
			else
			{
				num--;
			}
			if (num < 0)
			{
				num = this.m_activeSubEntryList.Count - 1;
			}
			else if (num > this.m_activeSubEntryList.Count - 1)
			{
				num = 0;
			}
			if (num != selectedSubEntryIndex)
			{
				this.m_changeSelectionSFX.Play();
				this.SelectTeleporter(num);
			}
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x001086E8 File Offset: 0x001068E8
		protected override void OnConfirmButtonDown(InputActionEventData obj)
		{
			if (this.m_activeSubEntryList != null && this.m_selectedSubEntryIndex >= 0 && this.m_selectedSubEntryIndex < this.m_activeSubEntryList.Count)
			{
				WindowManager.CloseAllOpenWindows();
				this.m_selectedSFX.Play();
				TeleporterSubEntry teleporterSubEntry = this.m_activeSubEntryList[this.m_selectedSubEntryIndex];
				GlobalTeleporterController.ActiveTeleporter.OnEnterTeleportPlayer_V2(teleporterSubEntry.BiomeType, teleporterSubEntry.GridPointManager);
			}
		}

		// Token: 0x04003DBA RID: 15802
		[SerializeField]
		private RectTransform m_teleporterEntries;

		// Token: 0x04003DBB RID: 15803
		[SerializeField]
		private TeleporterEntry m_teleporterEntryPrefab;

		// Token: 0x04003DBC RID: 15804
		[SerializeField]
		private RectTransform m_currentlyVisitedIcon;

		// Token: 0x04003DBD RID: 15805
		[SerializeField]
		private GameObject m_currentSelectedIcon;

		// Token: 0x04003DBE RID: 15806
		[SerializeField]
		private StudioEventEmitter m_changeSelectionSFX;

		// Token: 0x04003DBF RID: 15807
		[SerializeField]
		private StudioEventEmitter m_selectedSFX;

		// Token: 0x04003DC0 RID: 15808
		private SortedDictionary<BiomeType, TeleporterEntry> m_teleporterEntriesDict;

		// Token: 0x04003DC1 RID: 15809
		private List<TeleporterSubEntry> m_activeSubEntryList;

		// Token: 0x04003DC2 RID: 15810
		private int m_selectedSubEntryIndex;

		// Token: 0x04003DC3 RID: 15811
		private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

		// Token: 0x04003DC4 RID: 15812
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04003DC5 RID: 15813
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x04003DC6 RID: 15814
		private Action<InputActionEventData> m_changeSelectedTeleporter;

		// Token: 0x04003DC7 RID: 15815
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x02000EB2 RID: 3762
		private class TeleporterEntrySorter : IComparer<BiomeType>
		{
			// Token: 0x06006DFD RID: 28157 RVA: 0x00199FF4 File Offset: 0x001981F4
			public int Compare(BiomeType x, BiomeType y)
			{
				int num = Map_EV.BIOME_DISPLAY_ORDER.IndexOf(x);
				int num2 = Map_EV.BIOME_DISPLAY_ORDER.IndexOf(y);
				if (num > num2)
				{
					return 1;
				}
				if (num < num2)
				{
					return -1;
				}
				return 0;
			}
		}
	}
}
