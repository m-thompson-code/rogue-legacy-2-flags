using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DE9 RID: 3561
	[Serializable]
	public class RoomEventTrackerState : IRoomEventTrackerState, IGameEventTrackerState
	{
		// Token: 0x17002053 RID: 8275
		// (get) Token: 0x06006428 RID: 25640 RVA: 0x00037575 File Offset: 0x00035775
		// (set) Token: 0x06006429 RID: 25641 RVA: 0x0003757D File Offset: 0x0003577D
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

		// Token: 0x0600642A RID: 25642 RVA: 0x00037586 File Offset: 0x00035786
		public RoomEventTrackerState(List<RoomTrackerData> roomsEntered)
		{
			this.Initialise(roomsEntered);
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x00037595 File Offset: 0x00035795
		public void Initialise(List<RoomTrackerData> roomsEntered)
		{
			this.RoomsEntered = roomsEntered;
		}

		// Token: 0x040051A6 RID: 20902
		private List<RoomTrackerData> m_roomsEntered;
	}
}
