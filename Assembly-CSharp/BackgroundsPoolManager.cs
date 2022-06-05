using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B0E RID: 2830
public class BackgroundsPoolManager : MonoBehaviour
{
	// Token: 0x17001CD7 RID: 7383
	// (get) Token: 0x060054EC RID: 21740 RVA: 0x0002E15D File Offset: 0x0002C35D
	// (set) Token: 0x060054ED RID: 21741 RVA: 0x0002E164 File Offset: 0x0002C364
	private static BackgroundsPoolManager Instance { get; set; }

	// Token: 0x17001CD8 RID: 7384
	// (get) Token: 0x060054EE RID: 21742 RVA: 0x0002E16C File Offset: 0x0002C36C
	// (set) Token: 0x060054EF RID: 21743 RVA: 0x0002E173 File Offset: 0x0002C373
	public static bool IsInitialized { get; private set; }

	// Token: 0x060054F0 RID: 21744 RVA: 0x0002E17B File Offset: 0x0002C37B
	private void Awake()
	{
		if (BackgroundsPoolManager.Instance == null)
		{
			BackgroundsPoolManager.Instance = this;
			this.Initialize();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x0002E1A2 File Offset: 0x0002C3A2
	private void Initialize()
	{
		this.m_backgroundsTable = new Dictionary<Background, GenericPool_RL<Background>>();
		BackgroundsPoolManager.IsInitialized = true;
	}

	// Token: 0x060054F2 RID: 21746 RVA: 0x0002E1B5 File Offset: 0x0002C3B5
	private GenericPool_RL<Background> CreatePool(Background backgroundPrefab, int poolSize)
	{
		GenericPool_RL<Background> genericPool_RL = new GenericPool_RL<Background>();
		genericPool_RL.Initialize(backgroundPrefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x001411C8 File Offset: 0x0013F3C8
	private void CreateBackgroundPools_Internal(BiomeType biomeType)
	{
		Dictionary<Background, int> dictionary = new Dictionary<Background, int>();
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
			Dictionary<Background, int> dictionary2 = new Dictionary<Background, int>();
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
				if (baseRoom2.Backgrounds != null)
				{
					dictionary2.Clear();
					foreach (BackgroundPoolEntry backgroundPoolEntry in baseRoom2.Backgrounds)
					{
						if (backgroundPoolEntry.BackgroundPrefab)
						{
							if (dictionary2.ContainsKey(backgroundPoolEntry.BackgroundPrefab))
							{
								Dictionary<Background, int> dictionary3 = dictionary2;
								Background backgroundPrefab = backgroundPoolEntry.BackgroundPrefab;
								dictionary3[backgroundPrefab]++;
							}
							else
							{
								dictionary2.Add(backgroundPoolEntry.BackgroundPrefab, 1);
							}
						}
					}
					foreach (KeyValuePair<Background, int> keyValuePair in dictionary2)
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
		}
		List<Background> list2 = this.m_backgroundsTable.Keys.ToList<Background>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<Background, int> keyValuePair2 in dictionary)
		{
			if (keyValuePair2.Key)
			{
				list2.Remove(keyValuePair2.Key);
				if (!this.m_backgroundsTable.ContainsKey(keyValuePair2.Key))
				{
					this.m_backgroundsTable.Add(keyValuePair2.Key, this.CreatePool(keyValuePair2.Key, keyValuePair2.Value));
					num2++;
				}
				else
				{
					this.m_backgroundsTable[keyValuePair2.Key].ResizePool(keyValuePair2.Value);
					num++;
				}
			}
		}
		foreach (Background key in list2)
		{
			if (this.m_backgroundsTable.ContainsKey(key))
			{
				this.m_backgroundsTable[key].DestroyPool();
				this.m_backgroundsTable.Remove(key);
			}
		}
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x0002E1C6 File Offset: 0x0002C3C6
	public static void CreateBackgroundPools(BiomeType biomeType)
	{
		BackgroundsPoolManager.Instance.CreateBackgroundPools_Internal(biomeType);
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x0002E1D3 File Offset: 0x0002C3D3
	public static Background GetBackground(Background bgPrefab)
	{
		return BackgroundsPoolManager.Instance.GetBackground_Internal(bgPrefab);
	}

	// Token: 0x060054F6 RID: 21750 RVA: 0x0002E1E0 File Offset: 0x0002C3E0
	private Background GetBackground_Internal(Background bgPrefab)
	{
		if (this.m_backgroundsTable.ContainsKey(bgPrefab))
		{
			return this.m_backgroundsTable[bgPrefab].GetFreeObj();
		}
		Debug.LogFormat("<color=red>| BackgroundsPoolManager | Background Table does not contain an entry for Background named ({0})</color>", new object[]
		{
			bgPrefab.name
		});
		return null;
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x00141538 File Offset: 0x0013F738
	public static void DestroyPools()
	{
		foreach (KeyValuePair<Background, GenericPool_RL<Background>> keyValuePair in BackgroundsPoolManager.Instance.m_backgroundsTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		BackgroundsPoolManager.Instance.m_backgroundsTable.Clear();
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x0002E21C File Offset: 0x0002C41C
	private void OnDestroy()
	{
		BackgroundsPoolManager.DestroyPools();
		BackgroundsPoolManager.Instance = null;
		BackgroundsPoolManager.IsInitialized = false;
	}

	// Token: 0x04003F37 RID: 16183
	private Dictionary<Background, GenericPool_RL<Background>> m_backgroundsTable;
}
