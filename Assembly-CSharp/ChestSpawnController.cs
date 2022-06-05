using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class ChestSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, ILevelConsumer, ISetSpawnType
{
	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x06003784 RID: 14212 RVA: 0x000BE6F6 File Offset: 0x000BC8F6
	// (set) Token: 0x06003785 RID: 14213 RVA: 0x000BE6FE File Offset: 0x000BC8FE
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

	// Token: 0x170013AB RID: 5035
	// (get) Token: 0x06003786 RID: 14214 RVA: 0x000BE707 File Offset: 0x000BC907
	// (set) Token: 0x06003787 RID: 14215 RVA: 0x000BE70F File Offset: 0x000BC90F
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

	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x06003788 RID: 14216 RVA: 0x000BE722 File Offset: 0x000BC922
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x170013AD RID: 5037
	// (get) Token: 0x06003789 RID: 14217 RVA: 0x000BE72A File Offset: 0x000BC92A
	// (set) Token: 0x0600378A RID: 14218 RVA: 0x000BE732 File Offset: 0x000BC932
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

	// Token: 0x170013AE RID: 5038
	// (get) Token: 0x0600378B RID: 14219 RVA: 0x000BE751 File Offset: 0x000BC951
	// (set) Token: 0x0600378C RID: 14220 RVA: 0x000BE759 File Offset: 0x000BC959
	public bool IsInitialised { get; private set; }

	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x0600378D RID: 14221 RVA: 0x000BE762 File Offset: 0x000BC962
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600378E RID: 14222 RVA: 0x000BE77F File Offset: 0x000BC97F
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x000BE787 File Offset: 0x000BC987
	public void SetLevel(int value)
	{
		if ((Application.isPlaying && !this.OverrideLevel) || !Application.isPlaying)
		{
			this.m_level = value;
		}
	}

	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x06003790 RID: 14224 RVA: 0x000BE7A6 File Offset: 0x000BC9A6
	// (set) Token: 0x06003791 RID: 14225 RVA: 0x000BE7AE File Offset: 0x000BC9AE
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

	// Token: 0x170013B2 RID: 5042
	// (get) Token: 0x06003792 RID: 14226 RVA: 0x000BE7B7 File Offset: 0x000BC9B7
	// (set) Token: 0x06003793 RID: 14227 RVA: 0x000BE7BF File Offset: 0x000BC9BF
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

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x06003794 RID: 14228 RVA: 0x000BE7C8 File Offset: 0x000BC9C8
	// (set) Token: 0x06003795 RID: 14229 RVA: 0x000BE7D0 File Offset: 0x000BC9D0
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

	// Token: 0x170013B4 RID: 5044
	// (get) Token: 0x06003796 RID: 14230 RVA: 0x000BE7D9 File Offset: 0x000BC9D9
	// (set) Token: 0x06003797 RID: 14231 RVA: 0x000BE7E1 File Offset: 0x000BC9E1
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

	// Token: 0x170013B5 RID: 5045
	// (get) Token: 0x06003798 RID: 14232 RVA: 0x000BE7EA File Offset: 0x000BC9EA
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

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x06003799 RID: 14233 RVA: 0x000BE80D File Offset: 0x000BCA0D
	// (set) Token: 0x0600379A RID: 14234 RVA: 0x000BE815 File Offset: 0x000BCA15
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

	// Token: 0x170013B7 RID: 5047
	// (get) Token: 0x0600379B RID: 14235 RVA: 0x000BE834 File Offset: 0x000BCA34
	// (set) Token: 0x0600379C RID: 14236 RVA: 0x000BE83C File Offset: 0x000BCA3C
	public int ChestIndex { get; private set; }

	// Token: 0x170013B8 RID: 5048
	// (get) Token: 0x0600379D RID: 14237 RVA: 0x000BE845 File Offset: 0x000BCA45
	// (set) Token: 0x0600379E RID: 14238 RVA: 0x000BE84D File Offset: 0x000BCA4D
	public ChestObj ChestInstance { get; private set; }

	// Token: 0x0600379F RID: 14239 RVA: 0x000BE858 File Offset: 0x000BCA58
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

	// Token: 0x060037A0 RID: 14240 RVA: 0x000BE896 File Offset: 0x000BCA96
	private void Initialise()
	{
		if (GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
		this.IsInitialised = true;
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x000BE8AC File Offset: 0x000BCAAC
	public void SetSpawnType()
	{
		this.InitializeChestType();
		this.InitializeGoldAmount();
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x000BE8BA File Offset: 0x000BCABA
	public void SetChestIndex(int index)
	{
		this.ChestIndex = index;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x000BE8C4 File Offset: 0x000BCAC4
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

	// Token: 0x060037A4 RID: 14244 RVA: 0x000BE950 File Offset: 0x000BCB50
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

	// Token: 0x060037A5 RID: 14245 RVA: 0x000BEAB4 File Offset: 0x000BCCB4
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		Room room2 = room as Room;
		if (room2)
		{
			this.m_gridPointManager = room2.GridPointManager;
		}
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x000BEAE4 File Offset: 0x000BCCE4
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

	// Token: 0x060037A7 RID: 14247 RVA: 0x000BEB28 File Offset: 0x000BCD28
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

	// Token: 0x060037A9 RID: 14249 RVA: 0x000BEC87 File Offset: 0x000BCE87
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002AA8 RID: 10920
	[SerializeField]
	private SpecialItemType m_specialItemOverride;

	// Token: 0x04002AA9 RID: 10921
	[SerializeField]
	private bool m_overrideGold;

	// Token: 0x04002AAA RID: 10922
	[SerializeField]
	private bool m_overrideLevel;

	// Token: 0x04002AAB RID: 10923
	[SerializeField]
	private bool m_overrideType;

	// Token: 0x04002AAC RID: 10924
	[SerializeField]
	private BossID m_bossID;

	// Token: 0x04002AAD RID: 10925
	[SerializeField]
	private ChestType m_chestType;

	// Token: 0x04002AAE RID: 10926
	[SerializeField]
	private int m_gold;

	// Token: 0x04002AAF RID: 10927
	[SerializeField]
	private int m_level = -1;

	// Token: 0x04002AB0 RID: 10928
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04002AB1 RID: 10929
	private BaseRoom m_room;

	// Token: 0x04002AB2 RID: 10930
	private GridPointManager m_gridPointManager;

	// Token: 0x04002AB4 RID: 10932
	private bool m_hasCheckedForSpawnLogicController;
}
