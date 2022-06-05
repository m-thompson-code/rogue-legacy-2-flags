using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000229 RID: 553
[CreateAssetMenu(menuName = "Custom/Libraries/Enemy Library")]
public class EnemyLibrary : ScriptableObject
{
	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x06001693 RID: 5779 RVA: 0x000465DD File Offset: 0x000447DD
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

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06001694 RID: 5780 RVA: 0x00046610 File Offset: 0x00044810
	public static EnemyPrefabEntry[] EnemyPrefabEntryList
	{
		get
		{
			return EnemyLibrary.Instance.m_enemyPrefabEntryList;
		}
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0004661C File Offset: 0x0004481C
	private void Initialize()
	{
		this.m_enemyPrefabEntryDict = new Dictionary<EnemyType, EnemyPrefabEntry>();
		foreach (EnemyPrefabEntry enemyPrefabEntry in this.m_enemyPrefabEntryList)
		{
			this.m_enemyPrefabEntryDict.Add(enemyPrefabEntry.EnemyType, enemyPrefabEntry);
		}
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x00046660 File Offset: 0x00044860
	public static string GetEnemyPrefabPath(EnemyType enemyType, EnemyRank enemyRank)
	{
		EnemyPrefabEntry enemyPrefabEntry;
		if (EnemyLibrary.Instance.m_enemyPrefabEntryDict.TryGetValue(enemyType, out enemyPrefabEntry))
		{
			return enemyPrefabEntry.GetPrefabPath(enemyRank);
		}
		return null;
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0004668C File Offset: 0x0004488C
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

	// Token: 0x06001698 RID: 5784 RVA: 0x000466F4 File Offset: 0x000448F4
	public static void ClearLoadedPrefabs()
	{
		foreach (string key in EnemyLibrary.Instance.m_loadedEnemyPrefabDict.Keys.ToArray<string>())
		{
			EnemyLibrary.Instance.m_loadedEnemyPrefabDict[key] = null;
		}
		EnemyLibrary.Instance.m_loadedEnemyPrefabDict.Clear();
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x00046748 File Offset: 0x00044948
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

	// Token: 0x040015C8 RID: 5576
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EnemyLibrary";

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	private EnemyPrefabEntry[] m_enemyPrefabEntryList;

	// Token: 0x040015CA RID: 5578
	private Dictionary<EnemyType, EnemyPrefabEntry> m_enemyPrefabEntryDict;

	// Token: 0x040015CB RID: 5579
	private Dictionary<string, EnemyController> m_loadedEnemyPrefabDict = new Dictionary<string, EnemyController>();

	// Token: 0x040015CC RID: 5580
	private static EnemyLibrary m_instance;
}
