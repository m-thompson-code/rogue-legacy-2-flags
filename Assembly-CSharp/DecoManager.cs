using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000694 RID: 1684
public class DecoManager : MonoBehaviour
{
	// Token: 0x17001538 RID: 5432
	// (get) Token: 0x06003D1E RID: 15646 RVA: 0x000D3E99 File Offset: 0x000D2099
	// (set) Token: 0x06003D1F RID: 15647 RVA: 0x000D3EA0 File Offset: 0x000D20A0
	private static DecoManager Instance
	{
		get
		{
			return DecoManager.m_instance;
		}
		set
		{
			DecoManager.m_instance = value;
		}
	}

	// Token: 0x17001539 RID: 5433
	// (get) Token: 0x06003D20 RID: 15648 RVA: 0x000D3EA8 File Offset: 0x000D20A8
	// (set) Token: 0x06003D21 RID: 15649 RVA: 0x000D3EAF File Offset: 0x000D20AF
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003D22 RID: 15650 RVA: 0x000D3EB7 File Offset: 0x000D20B7
	private void Awake()
	{
		if (!DecoManager.Instance)
		{
			DecoManager.Instance = this;
			this.Initialize();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x000D3EDD File Offset: 0x000D20DD
	private void OnDestroy()
	{
		DecoManager.DestroyPools();
		DecoManager.m_instance = null;
		DecoManager.m_decoTable = null;
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x000D3EF0 File Offset: 0x000D20F0
	private GenericPool_RL<Deco> CreatePool(Deco prefab, int poolSize)
	{
		if (!prefab)
		{
			return null;
		}
		GenericPool_RL<Deco> genericPool_RL = new GenericPool_RL<Deco>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x000D3F0B File Offset: 0x000D210B
	public static Deco GetDeco(Deco deco)
	{
		return DecoManager.GetDeco(deco.NameHash);
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x000D3F18 File Offset: 0x000D2118
	public static Deco GetDeco(int decoNameHash)
	{
		if (DecoManager.m_decoTable.ContainsKey(decoNameHash))
		{
			return DecoManager.m_decoTable[decoNameHash].GetFreeObj();
		}
		Debug.LogFormat("<color=red>| DecoManager | Deco Object Pool Table does not contain an entry for Deco named ({0})</color>", new object[]
		{
			decoNameHash
		});
		return null;
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x000D3F52 File Offset: 0x000D2152
	private void Initialize()
	{
		DecoManager.m_decoTable = new Dictionary<int, GenericPool_RL<Deco>>();
		DecoManager.IsInitialized = true;
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x000D3F64 File Offset: 0x000D2164
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		Dictionary<Deco, int> dictionary = new Dictionary<Deco, int>();
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
			Dictionary<Deco, int> dictionary2 = new Dictionary<Deco, int>();
			List<BaseRoom> list = new List<BaseRoom>();
			list.AddRange(biomeController.Rooms);
			if (biomeType != BiomeType.Castle)
			{
				BiomeController biomeController2 = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(BiomeType.Castle);
				BaseRoom baseRoom = (biomeController2 != null) ? biomeController2.TransitionRoom : null;
				if (baseRoom != null)
				{
					list.Add(baseRoom);
				}
			}
			foreach (BaseRoom baseRoom2 in list)
			{
				dictionary2.Clear();
				foreach (PropSpawnController propSpawnController in baseRoom2.SpawnControllerManager.PropSpawnControllers)
				{
					Prop propPrefab = propSpawnController.PropPrefab;
					if (propPrefab)
					{
						DecoController[] prefabDecoControllers = PropSpawnController.GetPrefabDecoControllers(propPrefab);
						DecoSpawnData[][] decoSpawnData = propSpawnController.DecoSpawnData;
						int num = prefabDecoControllers.Length;
						for (int j = 0; j < num; j++)
						{
							DecoController decoController = prefabDecoControllers[j];
							int num2 = decoController.DecoLocations.Length;
							for (int k = 0; k < num2; k++)
							{
								DecoLocation decoLocation = decoController.DecoLocations[k];
								DecoSpawnData decoSpawnData2 = decoSpawnData[j][k];
								if (decoSpawnData2.ShouldSpawn)
								{
									Deco deco = decoLocation.PotentialDecos[(int)decoSpawnData2.DecoPropIndex].Deco;
									if (dictionary2.ContainsKey(deco))
									{
										Dictionary<Deco, int> dictionary3 = dictionary2;
										Deco key = deco;
										dictionary3[key]++;
									}
									else
									{
										dictionary2.Add(deco, 1);
									}
								}
							}
						}
					}
				}
				foreach (KeyValuePair<Deco, int> keyValuePair in dictionary2)
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
		List<int> list2 = DecoManager.m_decoTable.Keys.ToList<int>();
		int num3 = 0;
		int num4 = 0;
		foreach (KeyValuePair<Deco, int> keyValuePair2 in dictionary)
		{
			if (keyValuePair2.Key)
			{
				int nameHash = keyValuePair2.Key.NameHash;
				list2.Remove(nameHash);
				if (!DecoManager.m_decoTable.ContainsKey(nameHash))
				{
					DecoManager.m_decoTable.Add(nameHash, this.CreatePool(keyValuePair2.Key, keyValuePair2.Value));
					foreach (Deco deco2 in DecoManager.m_decoTable[nameHash].ObjectList)
					{
						deco2.NameHash = nameHash;
					}
					num4++;
				}
				else
				{
					DecoManager.m_decoTable[nameHash].ResizePool(keyValuePair2.Value);
					num3++;
				}
			}
		}
		foreach (int key2 in list2)
		{
			if (DecoManager.m_decoTable.ContainsKey(key2))
			{
				DecoManager.m_decoTable[key2].DestroyPool();
				DecoManager.m_decoTable.Remove(key2);
			}
		}
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x000D43A0 File Offset: 0x000D25A0
	public static void DestroyPools()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Deco>> keyValuePair in DecoManager.m_decoTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		DecoManager.m_decoTable.Clear();
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x000D4404 File Offset: 0x000D2604
	public static void CreateBiomePools(BiomeType biome)
	{
		DecoManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x000D4414 File Offset: 0x000D2614
	public static void DisableAllDecos()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Deco>> keyValuePair in DecoManager.m_decoTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04002DD0 RID: 11728
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04002DD1 RID: 11729
	private static DecoManager m_instance;

	// Token: 0x04002DD2 RID: 11730
	private static Dictionary<int, GenericPool_RL<Deco>> m_decoTable;
}
