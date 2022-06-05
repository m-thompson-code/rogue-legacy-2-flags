using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000689 RID: 1673
public class BackgroundsPoolManager : MonoBehaviour
{
	// Token: 0x1700150F RID: 5391
	// (get) Token: 0x06003C6D RID: 15469 RVA: 0x000D0B82 File Offset: 0x000CED82
	// (set) Token: 0x06003C6E RID: 15470 RVA: 0x000D0B89 File Offset: 0x000CED89
	private static BackgroundsPoolManager Instance { get; set; }

	// Token: 0x17001510 RID: 5392
	// (get) Token: 0x06003C6F RID: 15471 RVA: 0x000D0B91 File Offset: 0x000CED91
	// (set) Token: 0x06003C70 RID: 15472 RVA: 0x000D0B98 File Offset: 0x000CED98
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003C71 RID: 15473 RVA: 0x000D0BA0 File Offset: 0x000CEDA0
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

	// Token: 0x06003C72 RID: 15474 RVA: 0x000D0BC7 File Offset: 0x000CEDC7
	private void Initialize()
	{
		this.m_backgroundsTable = new Dictionary<Background, GenericPool_RL<Background>>();
		BackgroundsPoolManager.IsInitialized = true;
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x000D0BDA File Offset: 0x000CEDDA
	private GenericPool_RL<Background> CreatePool(Background backgroundPrefab, int poolSize)
	{
		GenericPool_RL<Background> genericPool_RL = new GenericPool_RL<Background>();
		genericPool_RL.Initialize(backgroundPrefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x000D0BEC File Offset: 0x000CEDEC
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

	// Token: 0x06003C75 RID: 15477 RVA: 0x000D0F5C File Offset: 0x000CF15C
	public static void CreateBackgroundPools(BiomeType biomeType)
	{
		BackgroundsPoolManager.Instance.CreateBackgroundPools_Internal(biomeType);
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x000D0F69 File Offset: 0x000CF169
	public static Background GetBackground(Background bgPrefab)
	{
		return BackgroundsPoolManager.Instance.GetBackground_Internal(bgPrefab);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x000D0F76 File Offset: 0x000CF176
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

	// Token: 0x06003C78 RID: 15480 RVA: 0x000D0FB4 File Offset: 0x000CF1B4
	public static void DestroyPools()
	{
		foreach (KeyValuePair<Background, GenericPool_RL<Background>> keyValuePair in BackgroundsPoolManager.Instance.m_backgroundsTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		BackgroundsPoolManager.Instance.m_backgroundsTable.Clear();
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x000D1020 File Offset: 0x000CF220
	private void OnDestroy()
	{
		BackgroundsPoolManager.DestroyPools();
		BackgroundsPoolManager.Instance = null;
		BackgroundsPoolManager.IsInitialized = false;
	}

	// Token: 0x04002D79 RID: 11641
	private Dictionary<Background, GenericPool_RL<Background>> m_backgroundsTable;
}
