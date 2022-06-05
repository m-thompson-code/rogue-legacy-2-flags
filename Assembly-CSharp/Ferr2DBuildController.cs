using System;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class Ferr2DBuildController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x0600157A RID: 5498 RVA: 0x00042D5A File Offset: 0x00040F5A
	// (set) Token: 0x0600157B RID: 5499 RVA: 0x00042D62 File Offset: 0x00040F62
	public BaseRoom Room { get; private set; }

	// Token: 0x0600157C RID: 5500 RVA: 0x00042D6B File Offset: 0x00040F6B
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_onRoomDestroyed = new Action<object, EventArgs>(this.OnRoomDestroyed);
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x00042D91 File Offset: 0x00040F91
	private void Start()
	{
		if (this.m_buildStage == Ferr2DBuildStage.Start)
		{
			this.Build();
		}
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x00042DA1 File Offset: 0x00040FA1
	private void Build()
	{
		this.m_isBuilt = true;
		if (this.m_buildMeshOnly)
		{
			this.m_terrain.BuildMeshOnly(false);
			return;
		}
		this.m_terrain.Build(true);
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x00042DCB File Offset: 0x00040FCB
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
			this.Room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
		}
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x00042E08 File Offset: 0x00041008
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!this.m_isBuilt)
		{
			this.Build();
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
			this.Room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
		}
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00042E48 File Offset: 0x00041048
	public void SetRoom(BaseRoom room)
	{
		if (this.m_buildStage == Ferr2DBuildStage.PlayerEnterRoom)
		{
			this.Room = room;
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
			this.Room.RoomDestroyedRelay.AddListener(this.m_onRoomDestroyed, false);
		}
	}

	// Token: 0x040014C2 RID: 5314
	[SerializeField]
	private bool m_buildMeshOnly = true;

	// Token: 0x040014C3 RID: 5315
	[SerializeField]
	private Ferr2DBuildStage m_buildStage;

	// Token: 0x040014C4 RID: 5316
	[SerializeField]
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x040014C5 RID: 5317
	private bool m_isBuilt;

	// Token: 0x040014C6 RID: 5318
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x040014C7 RID: 5319
	private Action<object, EventArgs> m_onRoomDestroyed;
}
