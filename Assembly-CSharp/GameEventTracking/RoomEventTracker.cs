using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEventTracking
{
	// Token: 0x02000DE7 RID: 3559
	public class RoomEventTracker : MonoBehaviour, IGameEventTracker<IRoomEventTrackerState>
	{
		// Token: 0x17002050 RID: 8272
		// (get) Token: 0x06006412 RID: 25618 RVA: 0x000373EC File Offset: 0x000355EC
		// (set) Token: 0x06006413 RID: 25619 RVA: 0x000373F4 File Offset: 0x000355F4
		public List<RoomTrackerData> RoomsEntered
		{
			get
			{
				return this.m_roomsEntered;
			}
			private set
			{
				this.m_roomsEntered = value;
			}
		}

		// Token: 0x06006414 RID: 25620 RVA: 0x000373FD File Offset: 0x000355FD
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
			this.m_onPlayerEnteredMergeRoomChunk = new Action<Vector2>(this.OnPlayerEnteredMergeRoomChunk);
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x00037435 File Offset: 0x00035635
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x00037450 File Offset: 0x00035650
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}

		// Token: 0x06006417 RID: 25623 RVA: 0x0003746B File Offset: 0x0003566B
		public IEnumerable<IGameEventData> GetGameEvents()
		{
			foreach (RoomTrackerData roomTrackerData in this.RoomsEntered)
			{
				yield return roomTrackerData;
			}
			List<RoomTrackerData>.Enumerator enumerator = default(List<RoomTrackerData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x00173784 File Offset: 0x00171984
		private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			if (SceneManager.GetActiveScene().name != "World")
			{
				return;
			}
			RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
			if (roomViaDoorEventArgs == null)
			{
				Debug.LogFormat("<color=red>| {0} | Failed to cast event args as RoomViaDoorEventArgs</color>", new object[]
				{
					this
				});
				return;
			}
			BaseRoom room = roomViaDoorEventArgs.Room;
			if (ReenactmentController.IsTunnelRoom(room))
			{
				return;
			}
			if (room.BiomeType == BiomeType.Garden)
			{
				return;
			}
			if (room is MergeRoom && room.BiomeType == BiomeType.Stone)
			{
				this.m_mergeRoomChunkManager = room.gameObject.GetComponentInChildren<MergeRoomChunkManager>();
				this.m_mergeRoomChunkManager.PlayerEnteredChunkRelay.AddListener(this.m_onPlayerEnteredMergeRoomChunk, false);
				return;
			}
			bool viaBiomeTransitionDoor = roomViaDoorEventArgs.ViaDoor != null && roomViaDoorEventArgs.ViaDoor.IsBiomeTransitionPoint;
			this.RoomsEntered.Add(new RoomTrackerData(room.BiomeType, room.BiomeControllerIndex, default(Vector2), viaBiomeTransitionDoor));
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x00173868 File Offset: 0x00171A68
		private void OnPlayerEnteredMergeRoomChunk(Vector2 mergeRoomChunkWorldPosition)
		{
			mergeRoomChunkWorldPosition.x += mergeRoomChunkWorldPosition.y - mergeRoomChunkWorldPosition.x;
			mergeRoomChunkWorldPosition.y = this.m_mergeRoomChunkManager.MergeRoom.Bounds.center.y;
			this.RoomsEntered.Add(new RoomTrackerData(this.m_mergeRoomChunkManager.MergeRoom.BiomeType, this.m_mergeRoomChunkManager.MergeRoom.BiomeControllerIndex, mergeRoomChunkWorldPosition, false));
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x0003747B File Offset: 0x0003567B
		private void OnPlayerExitRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			if (this.m_mergeRoomChunkManager != null)
			{
				this.m_mergeRoomChunkManager.PlayerEnteredChunkRelay.RemoveListener(this.m_onPlayerEnteredMergeRoomChunk);
			}
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x000374A2 File Offset: 0x000356A2
		public void Reset()
		{
			if (this.RoomsEntered != null)
			{
				this.RoomsEntered.Clear();
			}
			if (this.m_mergeRoomChunkManager != null)
			{
				this.m_mergeRoomChunkManager.PlayerEnteredChunkRelay.RemoveListener(this.m_onPlayerEnteredMergeRoomChunk);
			}
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x000374DC File Offset: 0x000356DC
		public void RestoreState(IRoomEventTrackerState state)
		{
			this.RoomsEntered = state.RoomsEntered;
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x000374EA File Offset: 0x000356EA
		public IRoomEventTrackerState SaveState()
		{
			if (this.m_state == null)
			{
				this.m_state = new RoomEventTrackerState(this.RoomsEntered);
			}
			else
			{
				this.m_state.Initialise(this.RoomsEntered);
			}
			return this.m_state;
		}

		// Token: 0x0400519B RID: 20891
		private IRoomEventTrackerState m_state;

		// Token: 0x0400519C RID: 20892
		private List<RoomTrackerData> m_roomsEntered = new List<RoomTrackerData>();

		// Token: 0x0400519D RID: 20893
		private MergeRoomChunkManager m_mergeRoomChunkManager;

		// Token: 0x0400519E RID: 20894
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x0400519F RID: 20895
		private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

		// Token: 0x040051A0 RID: 20896
		private Action<Vector2> m_onPlayerEnteredMergeRoomChunk;
	}
}
