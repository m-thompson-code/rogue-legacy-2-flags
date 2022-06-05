using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000690 RID: 1680
public class ChestManager : MonoBehaviour
{
	// Token: 0x1700152F RID: 5423
	// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x000D3704 File Offset: 0x000D1904
	// (set) Token: 0x06003CF9 RID: 15609 RVA: 0x000D370B File Offset: 0x000D190B
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

	// Token: 0x17001530 RID: 5424
	// (get) Token: 0x06003CFA RID: 15610 RVA: 0x000D3713 File Offset: 0x000D1913
	// (set) Token: 0x06003CFB RID: 15611 RVA: 0x000D371A File Offset: 0x000D191A
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003CFC RID: 15612 RVA: 0x000D3724 File Offset: 0x000D1924
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

	// Token: 0x06003CFD RID: 15613 RVA: 0x000D3770 File Offset: 0x000D1970
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (ChestManager.IsInitialized)
		{
			ChestManager.DisableAllChests();
		}
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x000D377E File Offset: 0x000D197E
	private void OnDestroy()
	{
		ChestManager.DestroyPools();
		ChestManager.m_instance = null;
		ChestManager.m_chestTable = null;
		ChestManager.IsInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x000D37A8 File Offset: 0x000D19A8
	private void Initialize()
	{
		ChestManager.m_chestTable = new Dictionary<ChestType, GenericPool_RL<ChestObj>>();
		ChestManager.IsInitialized = true;
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x000D37BC File Offset: 0x000D19BC
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

	// Token: 0x06003D01 RID: 15617 RVA: 0x000D3AB8 File Offset: 0x000D1CB8
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

	// Token: 0x06003D02 RID: 15618 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
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

	// Token: 0x06003D03 RID: 15619 RVA: 0x000D3B10 File Offset: 0x000D1D10
	public static void DestroyPools()
	{
		foreach (KeyValuePair<ChestType, GenericPool_RL<ChestObj>> keyValuePair in ChestManager.m_chestTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		ChestManager.m_chestTable.Clear();
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x000D3B74 File Offset: 0x000D1D74
	public static void CreateBiomePools(BiomeType biome)
	{
		ChestManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x000D3B84 File Offset: 0x000D1D84
	public static void DisableAllChests()
	{
		foreach (KeyValuePair<ChestType, GenericPool_RL<ChestObj>> keyValuePair in ChestManager.m_chestTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04002DC0 RID: 11712
	private const int CULLING_GROUP_SIZE = 100;

	// Token: 0x04002DC1 RID: 11713
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04002DC2 RID: 11714
	private static ChestManager m_instance;

	// Token: 0x04002DC3 RID: 11715
	private static Dictionary<ChestType, GenericPool_RL<ChestObj>> m_chestTable;

	// Token: 0x04002DC4 RID: 11716
	public const string RESOURCES_PATH = "Prefabs/Managers/HazardManager";
}
