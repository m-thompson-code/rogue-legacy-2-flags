using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B19 RID: 2841
public class ChestManager : MonoBehaviour
{
	// Token: 0x17001CFB RID: 7419
	// (get) Token: 0x0600558D RID: 21901 RVA: 0x0002E75C File Offset: 0x0002C95C
	// (set) Token: 0x0600558E RID: 21902 RVA: 0x0002E763 File Offset: 0x0002C963
	private static ChestManager Instance
	{
		get
		{
			return ChestManager.m_instance;
		}
		set
		{
			ChestManager.m_instance = value;
		}
	}

	// Token: 0x17001CFC RID: 7420
	// (get) Token: 0x0600558F RID: 21903 RVA: 0x0002E76B File Offset: 0x0002C96B
	// (set) Token: 0x06005590 RID: 21904 RVA: 0x0002E772 File Offset: 0x0002C972
	public static bool IsInitialized { get; private set; }

	// Token: 0x06005591 RID: 21905 RVA: 0x0014394C File Offset: 0x00141B4C
	private void Awake()
	{
		if (!ChestManager.Instance)
		{
			ChestManager.Instance = this;
			this.Initialize();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			Debug.Log("<color=green>Creating Chest Manager...</color>");
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x0002E77A File Offset: 0x0002C97A
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (ChestManager.IsInitialized)
		{
			ChestManager.DisableAllChests();
		}
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x0002E788 File Offset: 0x0002C988
	private void OnDestroy()
	{
		ChestManager.DestroyPools();
		ChestManager.m_instance = null;
		ChestManager.m_chestTable = null;
		ChestManager.IsInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x0002E7B2 File Offset: 0x0002C9B2
	private void Initialize()
	{
		ChestManager.m_chestTable = new Dictionary<ChestType, GenericPool_RL<ChestObj>>();
		ChestManager.IsInitialized = true;
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x00143998 File Offset: 0x00141B98
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		Dictionary<ChestType, int> dictionary = new Dictionary<ChestType, int>();
		BiomeController biomeController;
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		else
		{
			biomeController = WorldBuilder.GetBiomeController(biomeType);
		}
		if (biomeController != null)
		{
			Dictionary<ChestType, int> dictionary2 = new Dictionary<ChestType, int>();
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				dictionary2.Clear();
				foreach (ChestSpawnController chestSpawnController in baseRoom.SpawnControllerManager.ChestSpawnControllers)
				{
					if (dictionary2.ContainsKey(chestSpawnController.ChestType))
					{
						Dictionary<ChestType, int> dictionary3 = dictionary2;
						ChestType chestType = chestSpawnController.ChestType;
						dictionary3[chestType]++;
					}
					else
					{
						dictionary2.Add(chestSpawnController.ChestType, 1);
					}
				}
				foreach (KeyValuePair<ChestType, int> keyValuePair in dictionary2)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						if (dictionary[keyValuePair.Key] < dictionary2[keyValuePair.Key])
						{
							dictionary[keyValuePair.Key] = dictionary2[keyValuePair.Key];
						}
					}
					else
					{
						dictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}
		List<ChestType> list = ChestManager.m_chestTable.Keys.ToList<ChestType>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<ChestType, int> keyValuePair2 in dictionary)
		{
			if (keyValuePair2.Key != ChestType.None)
			{
				list.Remove(keyValuePair2.Key);
				if (!ChestManager.m_chestTable.ContainsKey(keyValuePair2.Key))
				{
					ChestObj chestPrefab = ChestLibrary.GetChestPrefab(keyValuePair2.Key);
					ChestManager.m_chestTable.Add(keyValuePair2.Key, this.CreatePool(chestPrefab, keyValuePair2.Value));
					num2++;
				}
				else
				{
					ChestManager.m_chestTable[keyValuePair2.Key].ResizePool(keyValuePair2.Value);
					num++;
				}
			}
		}
		foreach (ChestType key in list)
		{
			if (ChestManager.m_chestTable.ContainsKey(key))
			{
				ChestManager.m_chestTable[key].DestroyPool();
				ChestManager.m_chestTable.Remove(key);
			}
		}
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x0002E7C4 File Offset: 0x0002C9C4
	private GenericPool_RL<ChestObj> CreatePool(ChestObj prefab, int poolSize)
	{
		if (!prefab)
		{
			return null;
		}
		GenericPool_RL<ChestObj> genericPool_RL = new GenericPool_RL<ChestObj>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x0002E7DF File Offset: 0x0002C9DF
	public static ChestObj GetChest(ChestType chestType)
	{
		if (ChestManager.m_chestTable.ContainsKey(chestType))
		{
			return ChestManager.m_chestTable[chestType].GetFreeObj();
		}
		Debug.LogFormat("<color=red>| ChestManager | Chest Object Pool Table does not contain an entry for Chest Type ({0})</color>", new object[]
		{
			chestType
		});
		return null;
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x00143C94 File Offset: 0x00141E94
	public static void DestroyPools()
	{
		foreach (KeyValuePair<ChestType, GenericPool_RL<ChestObj>> keyValuePair in ChestManager.m_chestTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		ChestManager.m_chestTable.Clear();
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x0002E819 File Offset: 0x0002CA19
	public static void CreateBiomePools(BiomeType biome)
	{
		ChestManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x00143CF8 File Offset: 0x00141EF8
	public static void DisableAllChests()
	{
		foreach (KeyValuePair<ChestType, GenericPool_RL<ChestObj>> keyValuePair in ChestManager.m_chestTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04003F8B RID: 16267
	private const int CULLING_GROUP_SIZE = 100;

	// Token: 0x04003F8C RID: 16268
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04003F8D RID: 16269
	private static ChestManager m_instance;

	// Token: 0x04003F8E RID: 16270
	private static Dictionary<ChestType, GenericPool_RL<ChestObj>> m_chestTable;

	// Token: 0x04003F8F RID: 16271
	public const string RESOURCES_PATH = "Prefabs/Managers/HazardManager";
}
