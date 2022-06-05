using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x020008B2 RID: 2226
	[Serializable]
	public class RoomEventTrackerState : IRoomEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x060048A8 RID: 18600 RVA: 0x00104BA4 File Offset: 0x00102DA4
		// (set) Token: 0x060048A9 RID: 18601 RVA: 0x00104BAC File Offset: 0x00102DAC
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

		// Token: 0x060048AA RID: 18602 RVA: 0x00104BB5 File Offset: 0x00102DB5
		public RoomEventTrackerState(List<RoomTrackerData> roomsEntered)
		{
			this.Initialise(roomsEntered);
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00104BC4 File Offset: 0x00102DC4
		public void Initialise(List<RoomTrackerData> roomsEntered)
		{
			this.RoomsEntered = roomsEntered;
		}

		// Token: 0x04003D4C RID: 15692
		private List<RoomTrackerData> m_roomsEntered;
	}
}
