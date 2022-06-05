using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class SpecialPlatformSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, IRoomConsumer
{
	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x06003A04 RID: 14852 RVA: 0x000C53FC File Offset: 0x000C35FC
	// (set) Token: 0x06003A05 RID: 14853 RVA: 0x000C5404 File Offset: 0x000C3604
	public SpecialPlatform SpecialPlatformInstance { get; private set; }

	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x06003A06 RID: 14854 RVA: 0x000C540D File Offset: 0x000C360D
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000C5415 File Offset: 0x000C3615
	// (set) Token: 0x06003A08 RID: 14856 RVA: 0x000C541D File Offset: 0x000C361D
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

	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000C5426 File Offset: 0x000C3626
	// (set) Token: 0x06003A0A RID: 14858 RVA: 0x000C542E File Offset: 0x000C362E
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

	// Token: 0x1700147D RID: 5245
	// (get) Token: 0x06003A0B RID: 14859 RVA: 0x000C5437 File Offset: 0x000C3637
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x1700147E RID: 5246
	// (get) Token: 0x06003A0C RID: 14860 RVA: 0x000C5454 File Offset: 0x000C3654
	// (set) Token: 0x06003A0D RID: 14861 RVA: 0x000C545C File Offset: 0x000C365C
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

	// Token: 0x1700147F RID: 5247
	// (get) Token: 0x06003A0E RID: 14862 RVA: 0x000C5465 File Offset: 0x000C3665
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

	// Token: 0x17001480 RID: 5248
	// (get) Token: 0x06003A0F RID: 14863 RVA: 0x000C5488 File Offset: 0x000C3688
	// (set) Token: 0x06003A10 RID: 14864 RVA: 0x000C5490 File Offset: 0x000C3690
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

	// Token: 0x17001481 RID: 5249
	// (get) Token: 0x06003A11 RID: 14865 RVA: 0x000C5499 File Offset: 0x000C3699
	// (set) Token: 0x06003A12 RID: 14866 RVA: 0x000C54A1 File Offset: 0x000C36A1
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

	// Token: 0x06003A13 RID: 14867 RVA: 0x000C54AC File Offset: 0x000C36AC
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

	// Token: 0x06003A14 RID: 14868 RVA: 0x000C54EA File Offset: 0x000C36EA
	private void OnDrawGizmos()
	{
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x000C54EC File Offset: 0x000C36EC
	public void SetColor(Color color)
	{
		if (!Application.isPlaying)
		{
			base.GetComponent<SpriteRenderer>().color = color;
			this.ID = EnemySpawnController.ColorTable[color];
		}
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x000C5512 File Offset: 0x000C3712
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x000C553A File Offset: 0x000C373A
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x000C5567 File Offset: 0x000C3767
	protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs roomArgs)
	{
		if (this.SpecialPlatformInstance)
		{
			this.SpecialPlatformInstance.SetState(this.InitialState);
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x000C5588 File Offset: 0x000C3788
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

	// Token: 0x06003A1A RID: 14874 RVA: 0x000C55F4 File Offset: 0x000C37F4
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

	// Token: 0x06003A1D RID: 14877 RVA: 0x000C57B1 File Offset: 0x000C39B1
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002C8A RID: 11402
	[SerializeField]
	[HideInInspector]
	private SpecialPlatformType m_platformType;

	// Token: 0x04002C8B RID: 11403
	[SerializeField]
	[HideInInspector]
	private StateID m_initialState;

	// Token: 0x04002C8C RID: 11404
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x04002C8D RID: 11405
	[SerializeField]
	[ReadOnly]
	private int m_width = 4;

	// Token: 0x04002C8E RID: 11406
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04002C8F RID: 11407
	private BaseRoom m_room;

	// Token: 0x04002C90 RID: 11408
	public const int MIN_WIDTH = 1;

	// Token: 0x04002C91 RID: 11409
	public const int MAX_WIDTH = 32;

	// Token: 0x04002C92 RID: 11410
	private static StateID[] m_potentialStates_STATIC;

	// Token: 0x04002C94 RID: 11412
	private bool m_hasCheckedForSpawnController;
}
