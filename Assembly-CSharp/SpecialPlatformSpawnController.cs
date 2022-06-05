using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
public class SpecialPlatformSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, IRoomConsumer
{
	// Token: 0x17001BE0 RID: 7136
	// (get) Token: 0x060050E9 RID: 20713 RVA: 0x0002C25E File Offset: 0x0002A45E
	// (set) Token: 0x060050EA RID: 20714 RVA: 0x0002C266 File Offset: 0x0002A466
	public SpecialPlatform SpecialPlatformInstance { get; private set; }

	// Token: 0x17001BE1 RID: 7137
	// (get) Token: 0x060050EB RID: 20715 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001BE2 RID: 7138
	// (get) Token: 0x060050EC RID: 20716 RVA: 0x0002C26F File Offset: 0x0002A46F
	// (set) Token: 0x060050ED RID: 20717 RVA: 0x0002C277 File Offset: 0x0002A477
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

	// Token: 0x17001BE3 RID: 7139
	// (get) Token: 0x060050EE RID: 20718 RVA: 0x0002C280 File Offset: 0x0002A480
	// (set) Token: 0x060050EF RID: 20719 RVA: 0x0002C288 File Offset: 0x0002A488
	public StateID InitialState
	{
		get
		{
			return this.m_initialState;
		}
		set
		{
			this.m_initialState = value;
		}
	}

	// Token: 0x17001BE4 RID: 7140
	// (get) Token: 0x060050F0 RID: 20720 RVA: 0x0002C291 File Offset: 0x0002A491
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001BE5 RID: 7141
	// (get) Token: 0x060050F1 RID: 20721 RVA: 0x0002C2AE File Offset: 0x0002A4AE
	// (set) Token: 0x060050F2 RID: 20722 RVA: 0x0002C2B6 File Offset: 0x0002A4B6
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

	// Token: 0x17001BE6 RID: 7142
	// (get) Token: 0x060050F3 RID: 20723 RVA: 0x0002C2BF File Offset: 0x0002A4BF
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

	// Token: 0x17001BE7 RID: 7143
	// (get) Token: 0x060050F4 RID: 20724 RVA: 0x0002C2E2 File Offset: 0x0002A4E2
	// (set) Token: 0x060050F5 RID: 20725 RVA: 0x0002C2EA File Offset: 0x0002A4EA
	public SpecialPlatformType Type
	{
		get
		{
			return this.m_platformType;
		}
		set
		{
			this.m_platformType = value;
		}
	}

	// Token: 0x17001BE8 RID: 7144
	// (get) Token: 0x060050F6 RID: 20726 RVA: 0x0002C2F3 File Offset: 0x0002A4F3
	// (set) Token: 0x060050F7 RID: 20727 RVA: 0x0002C2FB File Offset: 0x0002A4FB
	public int Width
	{
		get
		{
			return this.m_width;
		}
		private set
		{
			this.m_width = value;
		}
	}

	// Token: 0x060050F8 RID: 20728 RVA: 0x0012D1D4 File Offset: 0x0012B3D4
	private void Start()
	{
		SpriteRenderer component = base.gameObject.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
			return;
		}
		Debug.LogFormat("<color=red>| {0} | No SpriteRenderer found. If you see this message please add a bug report to Pivotal</color>", new object[]
		{
			this
		});
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnDrawGizmos()
	{
	}

	// Token: 0x060050FA RID: 20730 RVA: 0x0002C304 File Offset: 0x0002A504
	public void SetColor(Color color)
	{
		if (!Application.isPlaying)
		{
			base.GetComponent<SpriteRenderer>().color = color;
			this.ID = EnemySpawnController.ColorTable[color];
		}
	}

	// Token: 0x060050FB RID: 20731 RVA: 0x0002C32A File Offset: 0x0002A52A
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060050FC RID: 20732 RVA: 0x0002C352 File Offset: 0x0002A552
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060050FD RID: 20733 RVA: 0x0002C37F File Offset: 0x0002A57F
	protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs roomArgs)
	{
		if (this.SpecialPlatformInstance)
		{
			this.SpecialPlatformInstance.SetState(this.InitialState);
		}
	}

	// Token: 0x060050FE RID: 20734 RVA: 0x001334A8 File Offset: 0x001316A8
	public void SetWidth(int width)
	{
		if (!Application.isPlaying)
		{
			this.Width = Mathf.Clamp(width, 1, 32);
			SpriteRenderer component = base.GetComponent<SpriteRenderer>();
			component.size = new Vector2((float)this.Width, component.size.y);
			BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
			component2.size = new Vector2((float)this.Width, component2.size.y);
		}
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x00133514 File Offset: 0x00131714
	public bool Spawn()
	{
		if (this.ShouldSpawn)
		{
			if (this.Type == SpecialPlatformType.BiomeSpecific)
			{
				SpecialPlatformType[] platformTypesInBiome = SpecialPlatformLibrary.GetPlatformTypesInBiome(this.Room.AppearanceBiomeType);
				int num = 0;
				int num2 = platformTypesInBiome.Length;
				if (num2 > 1)
				{
					num = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Get Random Platform Index", 0, num2);
				}
				this.Type = platformTypesInBiome[num];
			}
			this.SpecialPlatformInstance = SpecialPlatformLibrary.CreatePlatformInstance(this.Type, true);
			if (this.SpecialPlatformInstance)
			{
				this.SpecialPlatformInstance.transform.SetParent(this.Room.gameObject.transform);
				this.SpecialPlatformInstance.transform.position = base.transform.position;
				this.SpecialPlatformInstance.Width = (float)this.Width;
				List<IRoomConsumer> roomConsumerListHelper_STATIC = SimpleSpawnController.m_roomConsumerListHelper_STATIC;
				roomConsumerListHelper_STATIC.Clear();
				this.SpecialPlatformInstance.gameObject.GetComponentsInChildren<IRoomConsumer>(roomConsumerListHelper_STATIC);
				foreach (IRoomConsumer roomConsumer in roomConsumerListHelper_STATIC)
				{
					roomConsumer.SetRoom(this.Room);
				}
				if (this.InitialState == StateID.Random)
				{
					if (SpecialPlatformSpawnController.m_potentialStates_STATIC == null)
					{
						SpecialPlatformSpawnController.m_potentialStates_STATIC = (Enum.GetValues(typeof(StateID)) as StateID[]);
					}
					int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Get Random Platform State Index", 0, SpecialPlatformSpawnController.m_potentialStates_STATIC.Length - 1);
					this.InitialState = (StateID)SpecialPlatformSpawnController.m_potentialStates_STATIC.GetValue(randomNumber);
				}
				this.SpecialPlatformInstance.SetState(this.InitialState);
			}
			base.gameObject.SetActive(false);
			return this.SpecialPlatformInstance;
		}
		return false;
	}

	// Token: 0x06005102 RID: 20738 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003D20 RID: 15648
	[SerializeField]
	[HideInInspector]
	private SpecialPlatformType m_platformType;

	// Token: 0x04003D21 RID: 15649
	[SerializeField]
	[HideInInspector]
	private StateID m_initialState;

	// Token: 0x04003D22 RID: 15650
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x04003D23 RID: 15651
	[SerializeField]
	[ReadOnly]
	private int m_width = 4;

	// Token: 0x04003D24 RID: 15652
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04003D25 RID: 15653
	private BaseRoom m_room;

	// Token: 0x04003D26 RID: 15654
	public const int MIN_WIDTH = 1;

	// Token: 0x04003D27 RID: 15655
	public const int MAX_WIDTH = 32;

	// Token: 0x04003D28 RID: 15656
	private static StateID[] m_potentialStates_STATIC;

	// Token: 0x04003D2A RID: 15658
	private bool m_hasCheckedForSpawnController;
}
