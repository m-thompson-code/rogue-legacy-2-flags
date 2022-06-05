using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A1B RID: 2587
public class ChestSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, ILevelConsumer, ISetSpawnType
{
	// Token: 0x17001AFD RID: 6909
	// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0002A78E File Offset: 0x0002898E
	// (set) Token: 0x06004E14 RID: 19988 RVA: 0x0002A796 File Offset: 0x00028996
	public SpecialItemType SpecialItemOverride
	{
		get
		{
			return this.m_specialItemOverride;
		}
		set
		{
			this.m_specialItemOverride = value;
		}
	}

	// Token: 0x17001AFE RID: 6910
	// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0002A79F File Offset: 0x0002899F
	// (set) Token: 0x06004E16 RID: 19990 RVA: 0x0002A7A7 File Offset: 0x000289A7
	public BossID BossID
	{
		get
		{
			return this.m_bossID;
		}
		set
		{
			if (this.ChestType == ChestType.Boss)
			{
				this.m_bossID = value;
			}
		}
	}

	// Token: 0x17001AFF RID: 6911
	// (get) Token: 0x06004E17 RID: 19991 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001B00 RID: 6912
	// (get) Token: 0x06004E18 RID: 19992 RVA: 0x0002A7BA File Offset: 0x000289BA
	// (set) Token: 0x06004E19 RID: 19993 RVA: 0x0002A7C2 File Offset: 0x000289C2
	public int Gold
	{
		get
		{
			return this.m_gold;
		}
		set
		{
			if ((Application.isPlaying && !this.OverrideGold) || !Application.isPlaying)
			{
				this.m_gold = value;
			}
		}
	}

	// Token: 0x17001B01 RID: 6913
	// (get) Token: 0x06004E1A RID: 19994 RVA: 0x0002A7E1 File Offset: 0x000289E1
	// (set) Token: 0x06004E1B RID: 19995 RVA: 0x0002A7E9 File Offset: 0x000289E9
	public bool IsInitialised { get; private set; }

	// Token: 0x17001B02 RID: 6914
	// (get) Token: 0x06004E1C RID: 19996 RVA: 0x0002A7F2 File Offset: 0x000289F2
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001B03 RID: 6915
	// (get) Token: 0x06004E1D RID: 19997 RVA: 0x0002A80F File Offset: 0x00028A0F
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x0002A817 File Offset: 0x00028A17
	public void SetLevel(int value)
	{
		if ((Application.isPlaying && !this.OverrideLevel) || !Application.isPlaying)
		{
			this.m_level = value;
		}
	}

	// Token: 0x17001B04 RID: 6916
	// (get) Token: 0x06004E1F RID: 19999 RVA: 0x0002A836 File Offset: 0x00028A36
	// (set) Token: 0x06004E20 RID: 20000 RVA: 0x0002A83E File Offset: 0x00028A3E
	public bool OverrideGold
	{
		get
		{
			return this.m_overrideGold;
		}
		set
		{
			this.m_overrideGold = value;
		}
	}

	// Token: 0x17001B05 RID: 6917
	// (get) Token: 0x06004E21 RID: 20001 RVA: 0x0002A847 File Offset: 0x00028A47
	// (set) Token: 0x06004E22 RID: 20002 RVA: 0x0002A84F File Offset: 0x00028A4F
	public bool OverrideLevel
	{
		get
		{
			return this.m_overrideLevel;
		}
		set
		{
			this.m_overrideLevel = value;
		}
	}

	// Token: 0x17001B06 RID: 6918
	// (get) Token: 0x06004E23 RID: 20003 RVA: 0x0002A858 File Offset: 0x00028A58
	// (set) Token: 0x06004E24 RID: 20004 RVA: 0x0002A860 File Offset: 0x00028A60
	public bool OverrideType
	{
		get
		{
			return this.m_overrideType;
		}
		set
		{
			this.m_overrideType = value;
		}
	}

	// Token: 0x17001B07 RID: 6919
	// (get) Token: 0x06004E25 RID: 20005 RVA: 0x0002A869 File Offset: 0x00028A69
	// (set) Token: 0x06004E26 RID: 20006 RVA: 0x0002A871 File Offset: 0x00028A71
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

	// Token: 0x17001B08 RID: 6920
	// (get) Token: 0x06004E27 RID: 20007 RVA: 0x0002A87A File Offset: 0x00028A7A
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			if (!this.m_hasCheckedForSpawnLogicController)
			{
				this.m_hasCheckedForSpawnLogicController = true;
				this.m_spawnLogic = base.GetComponent<SpawnLogicController>();
			}
			return this.m_spawnLogic;
		}
	}

	// Token: 0x17001B09 RID: 6921
	// (get) Token: 0x06004E28 RID: 20008 RVA: 0x0002A89D File Offset: 0x00028A9D
	// (set) Token: 0x06004E29 RID: 20009 RVA: 0x0002A8A5 File Offset: 0x00028AA5
	public ChestType ChestType
	{
		get
		{
			return this.m_chestType;
		}
		set
		{
			if ((Application.isPlaying && !this.OverrideType) || !Application.isPlaying)
			{
				this.m_chestType = value;
			}
		}
	}

	// Token: 0x17001B0A RID: 6922
	// (get) Token: 0x06004E2A RID: 20010 RVA: 0x0002A8C4 File Offset: 0x00028AC4
	// (set) Token: 0x06004E2B RID: 20011 RVA: 0x0002A8CC File Offset: 0x00028ACC
	public int ChestIndex { get; private set; }

	// Token: 0x17001B0B RID: 6923
	// (get) Token: 0x06004E2C RID: 20012 RVA: 0x0002A8D5 File Offset: 0x00028AD5
	// (set) Token: 0x06004E2D RID: 20013 RVA: 0x0002A8DD File Offset: 0x00028ADD
	public ChestObj ChestInstance { get; private set; }

	// Token: 0x06004E2E RID: 20014 RVA: 0x0012D1D4 File Offset: 0x0012B3D4
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

	// Token: 0x06004E2F RID: 20015 RVA: 0x0002A8E6 File Offset: 0x00028AE6
	private void Initialise()
	{
		if (GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
		this.IsInitialised = true;
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x0002A8FC File Offset: 0x00028AFC
	public void SetSpawnType()
	{
		this.InitializeChestType();
		this.InitializeGoldAmount();
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x0002A90A File Offset: 0x00028B0A
	public void SetChestIndex(int index)
	{
		this.ChestIndex = index;
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x0012D214 File Offset: 0x0012B414
	private void InitializeChestType()
	{
		int roll = RNGManager.GetRandomNumber(RngID.Chest_RoomSeed, string.Format("(ChestSpawnController) InitializeChestType", Array.Empty<object>()), 0, 100);
		List<KeyValuePair<ChestType, Vector2Int>> list = (from keyValuePair in Economy_EV.GetChestTypeRollRanges()
		where roll >= keyValuePair.Value.x && roll <= keyValuePair.Value.y
		select keyValuePair).ToList<KeyValuePair<ChestType, Vector2Int>>();
		if (list.Count == 1)
		{
			this.ChestType = list.First<KeyValuePair<ChestType, Vector2Int>>().Key;
			return;
		}
		Debug.LogFormat("{0}: Failed to find a matching entry in Chest Roll Ranges.", new object[]
		{
			Time.frameCount
		});
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x0012D2A0 File Offset: 0x0012B4A0
	private void InitializeGoldAmount()
	{
		if (this.ChestType != ChestType.Boss && this.ChestType != ChestType.Black)
		{
			try
			{
				Vector2Int vector2Int = Economy_EV.BASE_GOLD_DROP_AMOUNT[this.m_chestType];
				int randomNumber = RNGManager.GetRandomNumber(RngID.Chest_RoomSeed, string.Format("(ChestSpawnController) InitializeGoldAmount - baseGoldAmount", Array.Empty<object>()), vector2Int.x, vector2Int.y + 1);
				float num = RNGManager.GetRandomNumber(RngID.Chest_RoomSeed, string.Format("(ChestSpawnController) InitializeGoldAmount - levelAdd", Array.Empty<object>()), Economy_EV.CHEST_GOLD_DROP_PER_LEVEL_ADD.x, Economy_EV.CHEST_GOLD_DROP_PER_LEVEL_ADD.y);
				num *= (float)this.Level;
				float num2 = Economy_EV.CHEST_TYPE_GOLD_MOD[this.m_chestType];
				int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
				this.Gold = (int)(((float)randomNumber + num - (float)newGamePlusLevel) * num2);
				this.Gold *= Economy_EV.GetItemDropValue(ItemDropType.Coin, false);
				return;
			}
			catch (KeyNotFoundException)
			{
				Debug.LogFormat("<color=red>|{0}| Key ({1}) is invalid. Check Chest ({2}) and ensure Override Type field has valid value.</color>", new object[]
				{
					this,
					this.ChestType,
					base.gameObject.FullHierarchyPath()
				});
				throw;
			}
		}
		if (this.BossID != BossID.None)
		{
			BossDrop bossDrop = Economy_EV.BOSS_DROP_TABLE[this.BossID];
			this.Gold = bossDrop.GoldAmount;
			return;
		}
		if (this.ChestType == ChestType.Boss)
		{
			Debug.LogFormat("<color=red>[{0}] Chest is of Type, Boss, but its BossID is set to None. You must specify a Boss ID</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x0012D404 File Offset: 0x0012B604
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		Room room2 = room as Room;
		if (room2)
		{
			this.m_gridPointManager = room2.GridPointManager;
		}
	}

	// Token: 0x06004E35 RID: 20021 RVA: 0x0012D434 File Offset: 0x0012B634
	public bool Spawn()
	{
		bool result = false;
		if (this.ShouldSpawn && this.Level != -1)
		{
			this.Initialise();
			result = true;
		}
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		return result;
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x0012D478 File Offset: 0x0012B678
	public void InitializeChestInstance()
	{
		if (!this.ShouldSpawn)
		{
			this.ChestInstance = null;
			return;
		}
		this.ChestInstance = ChestManager.GetChest(this.ChestType);
		if (this.ChestInstance)
		{
			this.ChestInstance.gameObject.SetActive(true);
			this.ChestInstance.SetChestIndex(this.ChestIndex);
			this.ChestInstance.gameObject.transform.SetParent(base.transform.parent);
			this.ChestInstance.Initialise(this.ChestType, this.Level, this.Gold, this.BossID);
			this.ChestInstance.SpecialItemOverride = this.SpecialItemOverride;
			List<IRoomConsumer> roomConsumerListHelper_STATIC = SimpleSpawnController.m_roomConsumerListHelper_STATIC;
			roomConsumerListHelper_STATIC.Clear();
			this.ChestInstance.gameObject.GetComponentsInChildren<IRoomConsumer>(roomConsumerListHelper_STATIC);
			foreach (IRoomConsumer roomConsumer in roomConsumerListHelper_STATIC)
			{
				roomConsumer.SetRoom(this.Room);
			}
			this.ChestInstance.gameObject.transform.position = base.transform.position;
			return;
		}
		Debug.LogFormat("<color=red>m_prefab Field is null on ({0})</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003AFF RID: 15103
	[SerializeField]
	private SpecialItemType m_specialItemOverride;

	// Token: 0x04003B00 RID: 15104
	[SerializeField]
	private bool m_overrideGold;

	// Token: 0x04003B01 RID: 15105
	[SerializeField]
	private bool m_overrideLevel;

	// Token: 0x04003B02 RID: 15106
	[SerializeField]
	private bool m_overrideType;

	// Token: 0x04003B03 RID: 15107
	[SerializeField]
	private BossID m_bossID;

	// Token: 0x04003B04 RID: 15108
	[SerializeField]
	private ChestType m_chestType;

	// Token: 0x04003B05 RID: 15109
	[SerializeField]
	private int m_gold;

	// Token: 0x04003B06 RID: 15110
	[SerializeField]
	private int m_level = -1;

	// Token: 0x04003B07 RID: 15111
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04003B08 RID: 15112
	private BaseRoom m_room;

	// Token: 0x04003B09 RID: 15113
	private GridPointManager m_gridPointManager;

	// Token: 0x04003B0B RID: 15115
	private bool m_hasCheckedForSpawnLogicController;
}
