using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
public class DecoManager : MonoBehaviour
{
	// Token: 0x17001D04 RID: 7428
	// (get) Token: 0x060055B3 RID: 21939 RVA: 0x0002E915 File Offset: 0x0002CB15
	// (set) Token: 0x060055B4 RID: 21940 RVA: 0x0002E91C File Offset: 0x0002CB1C
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

	// Token: 0x17001D05 RID: 7429
	// (get) Token: 0x060055B5 RID: 21941 RVA: 0x0002E924 File Offset: 0x0002CB24
	// (set) Token: 0x060055B6 RID: 21942 RVA: 0x0002E92B File Offset: 0x0002CB2B
	public static bool IsInitialized { get; private set; }

	// Token: 0x060055B7 RID: 21943 RVA: 0x0002E933 File Offset: 0x0002CB33
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

	// Token: 0x060055B8 RID: 21944 RVA: 0x0002E959 File Offset: 0x0002CB59
	private void OnDestroy()
	{
		DecoManager.DestroyPools();
		DecoManager.m_instance = null;
		DecoManager.m_decoTable = null;
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x0002E96C File Offset: 0x0002CB6C
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

	// Token: 0x060055BA RID: 21946 RVA: 0x0002E987 File Offset: 0x0002CB87
	public static Deco GetDeco(Deco deco)
	{
		return DecoManager.GetDeco(deco.NameHash);
	}

	// Token: 0x060055BB RID: 21947 RVA: 0x0002E994 File Offset: 0x0002CB94
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

	// Token: 0x060055BC RID: 21948 RVA: 0x0002E9CE File Offset: 0x0002CBCE
	private void Initialize()
	{
		DecoManager.m_decoTable = new Dictionary<int, GenericPool_RL<Deco>>();
		DecoManager.IsInitialized = true;
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x00143F08 File Offset: 0x00142108
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

	// Token: 0x060055BE RID: 21950 RVA: 0x00144344 File Offset: 0x00142544
	public static void DestroyPools()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Deco>> keyValuePair in DecoManager.m_decoTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		DecoManager.m_decoTable.Clear();
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x0002E9E0 File Offset: 0x0002CBE0
	public static void CreateBiomePools(BiomeType biome)
	{
		DecoManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x001443A8 File Offset: 0x001425A8
	public static void DisableAllDecos()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Deco>> keyValuePair in DecoManager.m_decoTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04003F9B RID: 16283
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04003F9C RID: 16284
	private static DecoManager m_instance;

	// Token: 0x04003F9D RID: 16285
	private static Dictionary<int, GenericPool_RL<Deco>> m_decoTable;
}
