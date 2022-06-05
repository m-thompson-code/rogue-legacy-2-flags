using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003E0 RID: 992
[CreateAssetMenu(menuName = "Custom/Libraries/Enemy Library")]
public class EnemyLibrary : ScriptableObject
{
	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x06002038 RID: 8248 RVA: 0x0001115F File Offset: 0x0000F35F
	private static EnemyLibrary Instance
	{
		get
		{
			if (EnemyLibrary.m_instance == null)
			{
				EnemyLibrary.m_instance = CDGResources.Load<EnemyLibrary>("Scriptable Objects/Libraries/EnemyLibrary", "", true);
				EnemyLibrary.m_instance.Initialize();
			}
			return EnemyLibrary.m_instance;
		}
	}

	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x06002039 RID: 8249 RVA: 0x00011192 File Offset: 0x0000F392
	public static EnemyPrefabEntry[] EnemyPrefabEntryList
	{
		get
		{
			return EnemyLibrary.Instance.m_enemyPrefabEntryList;
		}
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000A4864 File Offset: 0x000A2A64
	private void Initialize()
	{
		this.m_enemyPrefabEntryDict = new Dictionary<EnemyType, EnemyPrefabEntry>();
		foreach (EnemyPrefabEntry enemyPrefabEntry in this.m_enemyPrefabEntryList)
		{
			this.m_enemyPrefabEntryDict.Add(enemyPrefabEntry.EnemyType, enemyPrefabEntry);
		}
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000A48A8 File Offset: 0x000A2AA8
	public static string GetEnemyPrefabPath(EnemyType enemyType, EnemyRank enemyRank)
	{
		EnemyPrefabEntry enemyPrefabEntry;
		if (EnemyLibrary.Instance.m_enemyPrefabEntryDict.TryGetValue(enemyType, out enemyPrefabEntry))
		{
			return enemyPrefabEntry.GetPrefabPath(enemyRank);
		}
		return null;
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x000A48D4 File Offset: 0x000A2AD4
	public static EnemyController GetEnemyPrefab(EnemyType enemyType, EnemyRank enemyRank)
	{
		string enemyPrefabPath = EnemyLibrary.GetEnemyPrefabPath(enemyType, enemyRank);
		if (string.IsNullOrEmpty(enemyPrefabPath))
		{
			return null;
		}
		if (EnemyLibrary.Instance.m_loadedEnemyPrefabDict.ContainsKey(enemyPrefabPath))
		{
			return EnemyLibrary.Instance.m_loadedEnemyPrefabDict[enemyPrefabPath];
		}
		EnemyController component = CDGResources.Load<GameObject>(enemyPrefabPath, "", true).GetComponent<EnemyController>();
		EnemyLibrary.Instance.m_loadedEnemyPrefabDict.Add(enemyPrefabPath, component);
		return component;
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000A493C File Offset: 0x000A2B3C
	public static void ClearLoadedPrefabs()
	{
		foreach (string key in EnemyLibrary.Instance.m_loadedEnemyPrefabDict.Keys.ToArray<string>())
		{
			EnemyLibrary.Instance.m_loadedEnemyPrefabDict[key] = null;
		}
		EnemyLibrary.Instance.m_loadedEnemyPrefabDict.Clear();
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x0001119E File Offset: 0x0000F39E
	private static string GetSpecialEnemyName(EnemyType enemyType, EnemyRank enemyRank)
	{
		if (enemyType != EnemyType.BouncySpike)
		{
			if (enemyType == EnemyType.Dummy)
			{
				return "Dummy_Basic";
			}
		}
		else
		{
			switch (enemyRank)
			{
			case EnemyRank.Advanced:
				return "BouncySpike_Dodgeable_Advanced";
			case EnemyRank.Expert:
				return "BouncySpike_DownStrikeable_Expert";
			case EnemyRank.Miniboss:
				return "BouncySpike_Ricochet_Miniboss";
			}
		}
		return null;
	}

	// Token: 0x04001CD8 RID: 7384
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EnemyLibrary";

	// Token: 0x04001CD9 RID: 7385
	[SerializeField]
	private EnemyPrefabEntry[] m_enemyPrefabEntryList;

	// Token: 0x04001CDA RID: 7386
	private Dictionary<EnemyType, EnemyPrefabEntry> m_enemyPrefabEntryDict;

	// Token: 0x04001CDB RID: 7387
	private Dictionary<string, EnemyController> m_loadedEnemyPrefabDict = new Dictionary<string, EnemyController>();

	// Token: 0x04001CDC RID: 7388
	private static EnemyLibrary m_instance;
}
