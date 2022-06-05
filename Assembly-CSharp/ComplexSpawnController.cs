using System;
using UnityEngine;

// Token: 0x02000A1D RID: 2589
public abstract class ComplexSpawnController : MonoBehaviour, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer
{
	// Token: 0x17001B0C RID: 6924
	// (get) Token: 0x06004E3B RID: 20027 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001B0D RID: 6925
	// (get) Token: 0x06004E3C RID: 20028 RVA: 0x0002A922 File Offset: 0x00028B22
	// (set) Token: 0x06004E3D RID: 20029 RVA: 0x0002A92A File Offset: 0x00028B2A
	public int ID
	{
		get
		{
			return this.m_id;
		}
		private set
		{
			this.m_id = value;
		}
	}

	// Token: 0x17001B0E RID: 6926
	// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0002A933 File Offset: 0x00028B33
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001B0F RID: 6927
	// (get) Token: 0x06004E3F RID: 20031 RVA: 0x0002A950 File Offset: 0x00028B50
	// (set) Token: 0x06004E40 RID: 20032 RVA: 0x0002A958 File Offset: 0x00028B58
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
		private set
		{
			this.m_room = value;
		}
	}

	// Token: 0x17001B10 RID: 6928
	// (get) Token: 0x06004E41 RID: 20033 RVA: 0x0002A961 File Offset: 0x00028B61
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			if (!this.m_hasCheckedForSpawnController)
			{
				this.m_hasCheckedForSpawnController = true;
				this.m_spawnLogicController = base.GetComponent<SpawnLogicController>();
			}
			return this.m_spawnLogicController;
		}
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x0012D608 File Offset: 0x0012B808
	protected virtual void Awake()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x0002A984 File Offset: 0x00028B84
	private void OnDisable()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Reset();
		}
	}

	// Token: 0x06004E44 RID: 20036
	protected abstract void Reset();

	// Token: 0x06004E45 RID: 20037 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetID(int id)
	{
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x0012D62C File Offset: 0x0012B82C
	public virtual void SetRoom(BaseRoom room)
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
			this.Room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
		}
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
		this.Room.RoomDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomDestroyed), false);
	}

	// Token: 0x06004E47 RID: 20039
	protected abstract void Spawn();

	// Token: 0x06004E48 RID: 20040 RVA: 0x0002A999 File Offset: 0x00028B99
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.ShouldSpawn)
		{
			this.Spawn();
		}
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x0002A9A9 File Offset: 0x00028BA9
	private void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.ShouldSpawn)
		{
			this.Reset();
		}
	}

	// Token: 0x06004E4A RID: 20042 RVA: 0x0012D700 File Offset: 0x0012B900
	private void OnRoomDestroyed(object sender, EventArgs e)
	{
		this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		this.Room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
	}

	// Token: 0x06004E4C RID: 20044 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003B0F RID: 15119
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x04003B10 RID: 15120
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04003B11 RID: 15121
	private BaseRoom m_room;

	// Token: 0x04003B12 RID: 15122
	private bool m_hasCheckedForSpawnController;
}
