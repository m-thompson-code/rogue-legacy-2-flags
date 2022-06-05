using System;
using System.Collections.Generic;
using Rooms;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200064A RID: 1610
public class TunnelSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController
{
	// Token: 0x17001483 RID: 5251
	// (get) Token: 0x06003A25 RID: 14885 RVA: 0x000C5892 File Offset: 0x000C3A92
	public TransitionID TransitionType
	{
		get
		{
			return this.m_transitionType;
		}
	}

	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x06003A26 RID: 14886 RVA: 0x000C589A File Offset: 0x000C3A9A
	public GameObject TunnelPrefabOverride
	{
		get
		{
			return this.m_prefab;
		}
	}

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x06003A27 RID: 14887 RVA: 0x000C58A2 File Offset: 0x000C3AA2
	// (set) Token: 0x06003A28 RID: 14888 RVA: 0x000C58AA File Offset: 0x000C3AAA
	public Room DestinationRoomPrefab { get; private set; }

	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x06003A29 RID: 14889 RVA: 0x000C58B3 File Offset: 0x000C3AB3
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x06003A2A RID: 14890 RVA: 0x000C58BB File Offset: 0x000C3ABB
	// (set) Token: 0x06003A2B RID: 14891 RVA: 0x000C58C3 File Offset: 0x000C3AC3
	public bool HasBeenSpawned { get; private set; }

	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x06003A2C RID: 14892 RVA: 0x000C58CC File Offset: 0x000C3ACC
	public bool ShouldSpawn
	{
		get
		{
			return !this.SpawnLogicController || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x06003A2D RID: 14893 RVA: 0x000C58E8 File Offset: 0x000C3AE8
	// (set) Token: 0x06003A2E RID: 14894 RVA: 0x000C58F0 File Offset: 0x000C3AF0
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

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x06003A2F RID: 14895 RVA: 0x000C5900 File Offset: 0x000C3B00
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

	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x06003A30 RID: 14896 RVA: 0x000C593E File Offset: 0x000C3B3E
	public bool IsLocked
	{
		get
		{
			return this.m_isLocked;
		}
	}

	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x06003A31 RID: 14897 RVA: 0x000C5946 File Offset: 0x000C3B46
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x1700148D RID: 5261
	// (get) Token: 0x06003A32 RID: 14898 RVA: 0x000C594E File Offset: 0x000C3B4E
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			return this.m_spawnLogicController;
		}
	}

	// Token: 0x1700148E RID: 5262
	// (get) Token: 0x06003A33 RID: 14899 RVA: 0x000C5956 File Offset: 0x000C3B56
	// (set) Token: 0x06003A34 RID: 14900 RVA: 0x000C595E File Offset: 0x000C3B5E
	public Tunnel Tunnel { get; private set; }

	// Token: 0x1700148F RID: 5263
	// (get) Token: 0x06003A35 RID: 14901 RVA: 0x000C5967 File Offset: 0x000C3B67
	// (set) Token: 0x06003A36 RID: 14902 RVA: 0x000C596F File Offset: 0x000C3B6F
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

	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x06003A37 RID: 14903 RVA: 0x000C597F File Offset: 0x000C3B7F
	// (set) Token: 0x06003A38 RID: 14904 RVA: 0x000C5987 File Offset: 0x000C3B87
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

	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x06003A39 RID: 14905 RVA: 0x000C5990 File Offset: 0x000C3B90
	// (set) Token: 0x06003A3A RID: 14906 RVA: 0x000C5998 File Offset: 0x000C3B98
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

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x06003A3B RID: 14907 RVA: 0x000C59A8 File Offset: 0x000C3BA8
	public bool IsDestinationRoot
	{
		get
		{
			return this.m_leadsToRoot;
		}
	}

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x000C59B0 File Offset: 0x000C3BB0
	public bool ShowTunnel
	{
		get
		{
			return this.m_showTunnel;
		}
	}

	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x06003A3D RID: 14909 RVA: 0x000C59B8 File Offset: 0x000C3BB8
	// (set) Token: 0x06003A3E RID: 14910 RVA: 0x000C59C0 File Offset: 0x000C3BC0
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

	// Token: 0x06003A3F RID: 14911 RVA: 0x000C59CC File Offset: 0x000C3BCC
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

	// Token: 0x06003A40 RID: 14912 RVA: 0x000C5A2B File Offset: 0x000C3C2B
	public void SetRoom(BaseRoom room)
	{
		this.m_room = room;
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x000C5A34 File Offset: 0x000C3C34
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

	// Token: 0x06003A42 RID: 14914 RVA: 0x000C5AF8 File Offset: 0x000C3CF8
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

	// Token: 0x06003A43 RID: 14915 RVA: 0x000C5BD8 File Offset: 0x000C3DD8
	public void SetDestinationRoomPrefab(RoomMetaData roomMetaData)
	{
		if (roomMetaData)
		{
			this.RoomMetaData = roomMetaData;
			this.DestinationRoomPrefab = roomMetaData.GetPrefab(true);
		}
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x000C5C14 File Offset: 0x000C3E14
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002C98 RID: 11416
	[SerializeField]
	private TunnelDirection m_tunnelType = TunnelDirection.Entrance;

	// Token: 0x04002C99 RID: 11417
	[SerializeField]
	private TransitionID m_transitionType = TransitionID.QuickSwipe;

	// Token: 0x04002C9A RID: 11418
	[SerializeField]
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04002C9B RID: 11419
	[SerializeField]
	private GameObject m_prefab;

	// Token: 0x04002C9C RID: 11420
	[SerializeField]
	private RoomMetaData[] m_destinationRoomPrefabsOverride;

	// Token: 0x04002C9D RID: 11421
	[SerializeField]
	[ReadOnly]
	private RoomID[] m_destinationRoomIds;

	// Token: 0x04002C9E RID: 11422
	[SerializeField]
	[Range(0f, 10f)]
	private int m_index;

	// Token: 0x04002C9F RID: 11423
	[SerializeField]
	private TunnelCategory m_tunnelCategory;

	// Token: 0x04002CA0 RID: 11424
	[SerializeField]
	public TunnelSpawnControllerVisuals Visuals;

	// Token: 0x04002CA1 RID: 11425
	[SerializeField]
	private bool m_isLocked;

	// Token: 0x04002CA2 RID: 11426
	[SerializeField]
	private bool m_leadsToRoot;

	// Token: 0x04002CA3 RID: 11427
	[SerializeField]
	private bool m_showTunnel = true;

	// Token: 0x04002CA4 RID: 11428
	[SerializeField]
	private bool m_disableInteractIcon;

	// Token: 0x04002CA5 RID: 11429
	private BaseRoom m_room;

	// Token: 0x04002CA6 RID: 11430
	private RoomMetaData m_roomMetaData;
}
