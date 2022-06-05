using System;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class Ferr2DBuildController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x00010228 File Offset: 0x0000E428
	// (set) Token: 0x06001ED5 RID: 7893 RVA: 0x00010230 File Offset: 0x0000E430
	public BaseRoom Room { get; private set; }

	// Token: 0x06001ED6 RID: 7894 RVA: 0x00010239 File Offset: 0x0000E439
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_onRoomDestroyed = new Action<object, EventArgs>(this.OnRoomDestroyed);
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x0001025F File Offset: 0x0000E45F
	private void Start()
	{
		if (this.m_buildStage == Ferr2DBuildStage.Start)
		{
			this.Build();
		}
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x0001026F File Offset: 0x0000E46F
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

	// Token: 0x06001ED9 RID: 7897 RVA: 0x00010299 File Offset: 0x0000E499
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
			this.Room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
		}
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x000102D6 File Offset: 0x0000E4D6
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!this.m_isBuilt)
		{
			this.Build();
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
			this.Room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x000A1470 File Offset: 0x0009F670
	public void SetRoom(BaseRoom room)
	{
		if (this.m_buildStage == Ferr2DBuildStage.PlayerEnterRoom)
		{
			this.Room = room;
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
			this.Room.RoomDestroyedRelay.AddListener(this.m_onRoomDestroyed, false);
		}
	}

	// Token: 0x04001B91 RID: 7057
	[SerializeField]
	private bool m_buildMeshOnly = true;

	// Token: 0x04001B92 RID: 7058
	[SerializeField]
	private Ferr2DBuildStage m_buildStage;

	// Token: 0x04001B93 RID: 7059
	[SerializeField]
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x04001B94 RID: 7060
	private bool m_isBuilt;

	// Token: 0x04001B95 RID: 7061
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001B96 RID: 7062
	private Action<object, EventArgs> m_onRoomDestroyed;
}
