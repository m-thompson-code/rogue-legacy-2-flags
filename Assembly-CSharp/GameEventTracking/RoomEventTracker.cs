using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEventTracking
{
	// Token: 0x020008B1 RID: 2225
	public class RoomEventTracker : MonoBehaviour, IGameEventTracker<IRoomEventTrackerState>
	{
		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600489B RID: 18587 RVA: 0x001048FD File Offset: 0x00102AFD
		// (set) Token: 0x0600489C RID: 18588 RVA: 0x00104905 File Offset: 0x00102B05
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

		// Token: 0x0600489D RID: 18589 RVA: 0x0010490E File Offset: 0x00102B0E
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
			this.m_onPlayerEnteredMergeRoomChunk = new Action<Vector2>(this.OnPlayerEnteredMergeRoomChunk);
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00104946 File Offset: 0x00102B46
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00104961 File Offset: 0x00102B61
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x0010497C File Offset: 0x00102B7C
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

		// Token: 0x060048A1 RID: 18593 RVA: 0x0010498C File Offset: 0x00102B8C
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

		// Token: 0x060048A2 RID: 18594 RVA: 0x00104A70 File Offset: 0x00102C70
		private void OnPlayerEnteredMergeRoomChunk(Vector2 mergeRoomChunkWorldPosition)
		{
			mergeRoomChunkWorldPosition.x += mergeRoomChunkWorldPosition.y - mergeRoomChunkWorldPosition.x;
			mergeRoomChunkWorldPosition.y = this.m_mergeRoomChunkManager.MergeRoom.Bounds.center.y;
			this.RoomsEntered.Add(new RoomTrackerData(this.m_mergeRoomChunkManager.MergeRoom.BiomeType, this.m_mergeRoomChunkManager.MergeRoom.BiomeControllerIndex, mergeRoomChunkWorldPosition, false));
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00104AEE File Offset: 0x00102CEE
		private void OnPlayerExitRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			if (this.m_mergeRoomChunkManager != null)
			{
				this.m_mergeRoomChunkManager.PlayerEnteredChunkRelay.RemoveListener(this.m_onPlayerEnteredMergeRoomChunk);
			}
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00104B15 File Offset: 0x00102D15
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

		// Token: 0x060048A5 RID: 18597 RVA: 0x00104B4F File Offset: 0x00102D4F
		public void RestoreState(IRoomEventTrackerState state)
		{
			this.RoomsEntered = state.RoomsEntered;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00104B5D File Offset: 0x00102D5D
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

		// Token: 0x04003D46 RID: 15686
		private IRoomEventTrackerState m_state;

		// Token: 0x04003D47 RID: 15687
		private List<RoomTrackerData> m_roomsEntered = new List<RoomTrackerData>();

		// Token: 0x04003D48 RID: 15688
		private MergeRoomChunkManager m_mergeRoomChunkManager;

		// Token: 0x04003D49 RID: 15689
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04003D4A RID: 15690
		private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

		// Token: 0x04003D4B RID: 15691
		private Action<Vector2> m_onPlayerEnteredMergeRoomChunk;
	}
}
