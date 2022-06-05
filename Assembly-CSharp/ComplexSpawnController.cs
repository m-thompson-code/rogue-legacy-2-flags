using System;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public abstract class ComplexSpawnController : MonoBehaviour, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer
{
	// Token: 0x170013B9 RID: 5049
	// (get) Token: 0x060037AA RID: 14250 RVA: 0x000BEC8F File Offset: 0x000BCE8F
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x170013BA RID: 5050
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x000BEC97 File Offset: 0x000BCE97
	// (set) Token: 0x060037AC RID: 14252 RVA: 0x000BEC9F File Offset: 0x000BCE9F
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

	// Token: 0x170013BB RID: 5051
	// (get) Token: 0x060037AD RID: 14253 RVA: 0x000BECA8 File Offset: 0x000BCEA8
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x170013BC RID: 5052
	// (get) Token: 0x060037AE RID: 14254 RVA: 0x000BECC5 File Offset: 0x000BCEC5
	// (set) Token: 0x060037AF RID: 14255 RVA: 0x000BECCD File Offset: 0x000BCECD
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

	// Token: 0x170013BD RID: 5053
	// (get) Token: 0x060037B0 RID: 14256 RVA: 0x000BECD6 File Offset: 0x000BCED6
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

	// Token: 0x060037B1 RID: 14257 RVA: 0x000BECFC File Offset: 0x000BCEFC
	protected virtual void Awake()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component)
		{
			component.enabled = false;
		}
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x000BED1F File Offset: 0x000BCF1F
	private void OnDisable()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Reset();
		}
	}

	// Token: 0x060037B3 RID: 14259
	protected abstract void Reset();

	// Token: 0x060037B4 RID: 14260 RVA: 0x000BED34 File Offset: 0x000BCF34
	public void SetID(int id)
	{
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x000BED38 File Offset: 0x000BCF38
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

	// Token: 0x060037B6 RID: 14262
	protected abstract void Spawn();

	// Token: 0x060037B7 RID: 14263 RVA: 0x000BEE0A File Offset: 0x000BD00A
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.ShouldSpawn)
		{
			this.Spawn();
		}
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x000BEE1A File Offset: 0x000BD01A
	private void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.ShouldSpawn)
		{
			this.Reset();
		}
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x000BEE2C File Offset: 0x000BD02C
	private void OnRoomDestroyed(object sender, EventArgs e)
	{
		this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		this.Room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x000BEE98 File Offset: 0x000BD098
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x04002AB8 RID: 10936
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04002AB9 RID: 10937
	private BaseRoom m_room;

	// Token: 0x04002ABA RID: 10938
	private bool m_hasCheckedForSpawnController;
}
