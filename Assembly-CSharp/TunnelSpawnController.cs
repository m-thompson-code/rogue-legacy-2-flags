using System;
using System.Collections.Generic;
using Rooms;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000A79 RID: 2681
public class TunnelSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController
{
	// Token: 0x17001BEA RID: 7146
	// (get) Token: 0x0600510A RID: 20746 RVA: 0x0002C42F File Offset: 0x0002A62F
	public TransitionID TransitionType
	{
		get
		{
			return this.m_transitionType;
		}
	}

	// Token: 0x17001BEB RID: 7147
	// (get) Token: 0x0600510B RID: 20747 RVA: 0x0002C437 File Offset: 0x0002A637
	public GameObject TunnelPrefabOverride
	{
		get
		{
			return this.m_prefab;
		}
	}

	// Token: 0x17001BEC RID: 7148
	// (get) Token: 0x0600510C RID: 20748 RVA: 0x0002C43F File Offset: 0x0002A63F
	// (set) Token: 0x0600510D RID: 20749 RVA: 0x0002C447 File Offset: 0x0002A647
	public Room DestinationRoomPrefab { get; private set; }

	// Token: 0x17001BED RID: 7149
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001BEE RID: 7150
	// (get) Token: 0x0600510F RID: 20751 RVA: 0x0002C450 File Offset: 0x0002A650
	// (set) Token: 0x06005110 RID: 20752 RVA: 0x0002C458 File Offset: 0x0002A658
	public bool HasBeenSpawned { get; private set; }

	// Token: 0x17001BEF RID: 7151
	// (get) Token: 0x06005111 RID: 20753 RVA: 0x0002C461 File Offset: 0x0002A661
	public bool ShouldSpawn
	{
		get
		{
			return !this.SpawnLogicController || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001BF0 RID: 7152
	// (get) Token: 0x06005112 RID: 20754 RVA: 0x0002C47D File Offset: 0x0002A67D
	// (set) Token: 0x06005113 RID: 20755 RVA: 0x0002C485 File Offset: 0x0002A685
	public int Index
	{
		get
		{
			return this.m_index;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_index = value;
			}
		}
	}

	// Token: 0x17001BF1 RID: 7153
	// (get) Token: 0x06005114 RID: 20756 RVA: 0x00133710 File Offset: 0x00131910
	public RoomType LeadsToRoomType
	{
		get
		{
			TunnelCategory category = this.Category;
			if (category <= TunnelCategory.Bonus)
			{
				if (category == TunnelCategory.Boss)
				{
					return RoomType.Boss;
				}
				if (category == TunnelCategory.Bonus)
				{
					return RoomType.Bonus;
				}
			}
			else
			{
				if (category == TunnelCategory.Final)
				{
					return RoomType.Boss;
				}
				if (category == TunnelCategory.Fairy)
				{
					return RoomType.Fairy;
				}
			}
			return RoomType.Standard;
		}
	}

	// Token: 0x17001BF2 RID: 7154
	// (get) Token: 0x06005115 RID: 20757 RVA: 0x0002C495 File Offset: 0x0002A695
	public bool IsLocked
	{
		get
		{
			return this.m_isLocked;
		}
	}

	// Token: 0x17001BF3 RID: 7155
	// (get) Token: 0x06005116 RID: 20758 RVA: 0x0002C49D File Offset: 0x0002A69D
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x17001BF4 RID: 7156
	// (get) Token: 0x06005117 RID: 20759 RVA: 0x0002C4A5 File Offset: 0x0002A6A5
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			return this.m_spawnLogicController;
		}
	}

	// Token: 0x17001BF5 RID: 7157
	// (get) Token: 0x06005118 RID: 20760 RVA: 0x0002C4AD File Offset: 0x0002A6AD
	// (set) Token: 0x06005119 RID: 20761 RVA: 0x0002C4B5 File Offset: 0x0002A6B5
	public Tunnel Tunnel { get; private set; }

	// Token: 0x17001BF6 RID: 7158
	// (get) Token: 0x0600511A RID: 20762 RVA: 0x0002C4BE File Offset: 0x0002A6BE
	// (set) Token: 0x0600511B RID: 20763 RVA: 0x0002C4C6 File Offset: 0x0002A6C6
	public TunnelCategory Category
	{
		get
		{
			return this.m_tunnelCategory;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_tunnelCategory = value;
			}
		}
	}

	// Token: 0x17001BF7 RID: 7159
	// (get) Token: 0x0600511C RID: 20764 RVA: 0x0002C4D6 File Offset: 0x0002A6D6
	// (set) Token: 0x0600511D RID: 20765 RVA: 0x0002C4DE File Offset: 0x0002A6DE
	public TunnelDirection Direction
	{
		get
		{
			return this.m_tunnelType;
		}
		set
		{
			this.m_tunnelType = value;
		}
	}

	// Token: 0x17001BF8 RID: 7160
	// (get) Token: 0x0600511E RID: 20766 RVA: 0x0002C4E7 File Offset: 0x0002A6E7
	// (set) Token: 0x0600511F RID: 20767 RVA: 0x0002C4EF File Offset: 0x0002A6EF
	public RoomMetaData[] DestinationRoomPrefabsOverride
	{
		get
		{
			return this.m_destinationRoomPrefabsOverride;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_destinationRoomPrefabsOverride = value;
			}
		}
	}

	// Token: 0x17001BF9 RID: 7161
	// (get) Token: 0x06005120 RID: 20768 RVA: 0x0002C4FF File Offset: 0x0002A6FF
	public bool IsDestinationRoot
	{
		get
		{
			return this.m_leadsToRoot;
		}
	}

	// Token: 0x17001BFA RID: 7162
	// (get) Token: 0x06005121 RID: 20769 RVA: 0x0002C507 File Offset: 0x0002A707
	public bool ShowTunnel
	{
		get
		{
			return this.m_showTunnel;
		}
	}

	// Token: 0x17001BFB RID: 7163
	// (get) Token: 0x06005122 RID: 20770 RVA: 0x0002C50F File Offset: 0x0002A70F
	// (set) Token: 0x06005123 RID: 20771 RVA: 0x0002C517 File Offset: 0x0002A717
	public RoomMetaData RoomMetaData
	{
		get
		{
			return this.m_roomMetaData;
		}
		private set
		{
			this.m_roomMetaData = value;
		}
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x00133750 File Offset: 0x00131950
	private RoomMetaData GetRandomRoomMetaData(RoomMetaData[] rooms)
	{
		if (rooms == null)
		{
			Debug.LogFormat("<color=red>| {0} | Destination Room Prefabs is NULL, but shouldn't be.</color>", new object[]
			{
				this
			});
			return null;
		}
		int num = rooms.Length;
		if (num > 0)
		{
			int num2 = 0;
			if (num > 1)
			{
				num2 = RNGManager.GetRandomNumber(RngID.Tunnel_RoomSeed, "Tunnel Spawn Controller: Get Random Tunnel Destination", 0, num);
			}
			return rooms[num2];
		}
		Debug.LogFormat("<color=red>| {0} | Destination Room Prefabs is empty, but shouldn't be.</color>", new object[]
		{
			this
		});
		return null;
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x0002C520 File Offset: 0x0002A720
	public void SetRoom(BaseRoom room)
	{
		this.m_room = room;
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001337B0 File Offset: 0x001319B0
	public void SetTunnelInstance(Tunnel instance)
	{
		this.Tunnel = instance;
		if (this.Tunnel && this.Tunnel.Interactable)
		{
			this.Tunnel.Interactable.ForceDisableInteractPrompt(this.m_disableInteractIcon);
		}
		if (this.Room && this.Tunnel)
		{
			List<IRoomConsumer> roomConsumerListHelper_STATIC = SimpleSpawnController.m_roomConsumerListHelper_STATIC;
			roomConsumerListHelper_STATIC.Clear();
			this.Tunnel.GameObject.GetComponentsInChildren<IRoomConsumer>(roomConsumerListHelper_STATIC);
			foreach (IRoomConsumer roomConsumer in roomConsumerListHelper_STATIC)
			{
				roomConsumer.SetRoom(this.Room);
			}
		}
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x00133874 File Offset: 0x00131A74
	public bool Spawn()
	{
		bool result = false;
		if (this.ShouldSpawn && !this.HasBeenSpawned && this.Direction != TunnelDirection.None)
		{
			this.HasBeenSpawned = true;
			if (this.Direction == TunnelDirection.Entrance)
			{
				this.RoomMetaData = null;
				if (!this.IsDestinationRoot && this.DestinationRoomPrefabsOverride.Length != 0)
				{
					this.RoomMetaData = this.GetRandomRoomMetaData(this.DestinationRoomPrefabsOverride);
					if (!this.RoomMetaData)
					{
						Debug.LogFormat("<color=red>| {0} | A destination override entry is null on Tunnel Spawn Controller (<b>{1}</b>) in Room (<b>{2}</b>)</color>", new object[]
						{
							this,
							string.Format("{0}/{1}", base.transform.parent.name, base.name),
							this.Room.gameObject.name
						});
					}
				}
				this.SetDestinationRoomPrefab(this.RoomMetaData);
			}
			result = true;
		}
		base.gameObject.SetActive(false);
		return result;
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x0002C529 File Offset: 0x0002A729
	public void SetDestinationRoomPrefab(RoomMetaData roomMetaData)
	{
		if (roomMetaData)
		{
			this.RoomMetaData = roomMetaData;
			this.DestinationRoomPrefab = roomMetaData.GetPrefab(true);
		}
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003D2E RID: 15662
	[SerializeField]
	private TunnelDirection m_tunnelType = TunnelDirection.Entrance;

	// Token: 0x04003D2F RID: 15663
	[SerializeField]
	private TransitionID m_transitionType = TransitionID.QuickSwipe;

	// Token: 0x04003D30 RID: 15664
	[SerializeField]
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04003D31 RID: 15665
	[SerializeField]
	private GameObject m_prefab;

	// Token: 0x04003D32 RID: 15666
	[SerializeField]
	private RoomMetaData[] m_destinationRoomPrefabsOverride;

	// Token: 0x04003D33 RID: 15667
	[SerializeField]
	[ReadOnly]
	private RoomID[] m_destinationRoomIds;

	// Token: 0x04003D34 RID: 15668
	[SerializeField]
	[Range(0f, 10f)]
	private int m_index;

	// Token: 0x04003D35 RID: 15669
	[SerializeField]
	private TunnelCategory m_tunnelCategory;

	// Token: 0x04003D36 RID: 15670
	[SerializeField]
	public TunnelSpawnControllerVisuals Visuals;

	// Token: 0x04003D37 RID: 15671
	[SerializeField]
	private bool m_isLocked;

	// Token: 0x04003D38 RID: 15672
	[SerializeField]
	private bool m_leadsToRoot;

	// Token: 0x04003D39 RID: 15673
	[SerializeField]
	private bool m_showTunnel = true;

	// Token: 0x04003D3A RID: 15674
	[SerializeField]
	private bool m_disableInteractIcon;

	// Token: 0x04003D3B RID: 15675
	private BaseRoom m_room;

	// Token: 0x04003D3C RID: 15676
	private RoomMetaData m_roomMetaData;
}
