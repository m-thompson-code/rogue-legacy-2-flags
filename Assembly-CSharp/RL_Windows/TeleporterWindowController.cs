using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000DFE RID: 3582
	public class TeleporterWindowController : MapWindowController
	{
		// Token: 0x17002077 RID: 8311
		// (get) Token: 0x060064E6 RID: 25830 RVA: 0x000054B1 File Offset: 0x000036B1
		public override WindowID ID
		{
			get
			{
				return WindowID.Teleporter;
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x00176F8C File Offset: 0x0017518C
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete);
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_changeSelectedTeleporter = new Action<InputActionEventData>(this.ChangeSelectedTeleporter);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x00037AE0 File Offset: 0x00035CE0
		private void OnEnable()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x00037AFA File Offset: 0x00035CFA
		private void OnDisable()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x00037B14 File Offset: 0x00035D14
		private void OnWorldCreationComplete(object sender, EventArgs args)
		{
			base.StartCoroutine(this.InitializeTeleporters());
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x00176FF8 File Offset: 0x001751F8
		private void OnPlayerEnterRoom(object sender, EventArgs args)
		{
			if (!base.IsInitialized)
			{
				return;
			}
			BaseRoom room = (args as RoomViaDoorEventArgs).Room;
			this.SetEntryVisible(room.AppearanceBiomeType, 0);
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x00002FCA File Offset: 0x000011CA
		private void SetEntryVisible(BiomeType biomeType, int index)
		{
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00037B23 File Offset: 0x00035D23
		public override void Initialize()
		{
			base.Initialize();
			if (GameUtility.IsInLevelEditor)
			{
				base.StartCoroutine(this.InitializeTeleporters());
			}
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x00037B3F File Offset: 0x00035D3F
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

		// Token: 0x060064EF RID: 25839 RVA: 0x00177028 File Offset: 0x00175228
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

		// Token: 0x060064F0 RID: 25840 RVA: 0x001770CC File Offset: 0x001752CC
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

		// Token: 0x060064F1 RID: 25841 RVA: 0x00177150 File Offset: 0x00175350
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

		// Token: 0x060064F2 RID: 25842 RVA: 0x0017727C File Offset: 0x0017547C
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

		// Token: 0x060064F3 RID: 25843 RVA: 0x00177318 File Offset: 0x00175518
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

		// Token: 0x060064F4 RID: 25844 RVA: 0x00177684 File Offset: 0x00175884
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

		// Token: 0x060064F5 RID: 25845 RVA: 0x00037B4E File Offset: 0x00035D4E
		private IEnumerator SetTeleporterIconsCoroutine(int teleporterIndex)
		{
			yield return null;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_teleporterEntries);
			this.SetPreviouslySelectedTeleporter(teleporterIndex);
			this.SelectTeleporter(teleporterIndex);
			yield break;
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x001776D4 File Offset: 0x001758D4
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

		// Token: 0x060064F7 RID: 25847 RVA: 0x001778AC File Offset: 0x00175AAC
		private void SetPreviouslySelectedTeleporter(int index)
		{
			float y = this.m_activeSubEntryList[index].transform.position.y;
			Vector3 position = this.m_currentlyVisitedIcon.transform.position;
			position.y = y;
			this.m_currentlyVisitedIcon.transform.position = position;
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x00177900 File Offset: 0x00175B00
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

		// Token: 0x060064F9 RID: 25849 RVA: 0x001779DC File Offset: 0x00175BDC
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

		// Token: 0x060064FA RID: 25850 RVA: 0x00177A6C File Offset: 0x00175C6C
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

		// Token: 0x060064FB RID: 25851 RVA: 0x00177AFC File Offset: 0x00175CFC
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

		// Token: 0x060064FC RID: 25852 RVA: 0x00177B90 File Offset: 0x00175D90
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

		// Token: 0x04005236 RID: 21046
		[SerializeField]
		private RectTransform m_teleporterEntries;

		// Token: 0x04005237 RID: 21047
		[SerializeField]
		private TeleporterEntry m_teleporterEntryPrefab;

		// Token: 0x04005238 RID: 21048
		[SerializeField]
		private RectTransform m_currentlyVisitedIcon;

		// Token: 0x04005239 RID: 21049
		[SerializeField]
		private GameObject m_currentSelectedIcon;

		// Token: 0x0400523A RID: 21050
		[SerializeField]
		private StudioEventEmitter m_changeSelectionSFX;

		// Token: 0x0400523B RID: 21051
		[SerializeField]
		private StudioEventEmitter m_selectedSFX;

		// Token: 0x0400523C RID: 21052
		private SortedDictionary<BiomeType, TeleporterEntry> m_teleporterEntriesDict;

		// Token: 0x0400523D RID: 21053
		private List<TeleporterSubEntry> m_activeSubEntryList;

		// Token: 0x0400523E RID: 21054
		private int m_selectedSubEntryIndex;

		// Token: 0x0400523F RID: 21055
		private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

		// Token: 0x04005240 RID: 21056
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04005241 RID: 21057
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x04005242 RID: 21058
		private Action<InputActionEventData> m_changeSelectedTeleporter;

		// Token: 0x04005243 RID: 21059
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x02000DFF RID: 3583
		private class TeleporterEntrySorter : IComparer<BiomeType>
		{
			// Token: 0x060064FE RID: 25854 RVA: 0x00177BFC File Offset: 0x00175DFC
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
