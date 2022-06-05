using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
[CreateAssetMenu(menuName = "Custom/Libraries/Enemy Class Library")]
public class EnemyClassLibrary : ScriptableObject
{
	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x0600168E RID: 5774 RVA: 0x000464C8 File Offset: 0x000446C8
	private static EnemyClassLibrary Instance
	{
		get
		{
			if (EnemyClassLibrary.m_instance == null)
			{
				EnemyClassLibrary.m_instance = CDGResources.Load<EnemyClassLibrary>("Scriptable Objects/Libraries/EnemyClassLibrary", "", true);
			}
			return EnemyClassLibrary.m_instance;
		}
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x000464F4 File Offset: 0x000446F4
	public static EnemyClassData GetEnemyClassData(EnemyType enemyType)
	{
		EnemyClassData enemyClassData = null;
		if (EnemyClassLibrary.Instance.m_enemyClassLibrary != null)
		{
			EnemyClassLibrary.Instance.m_enemyClassLibrary.TryGetValue(enemyType, out enemyClassData);
			if (enemyClassData == null)
			{
				Debug.LogFormat("<color=red>Class: " + enemyType.ToString() + " not found in Enemy Class Library. Please ensure the class exists in the Enemy Class Library scriptable object.</color>", Array.Empty<object>());
			}
			return enemyClassData;
		}
		throw new Exception("Enemy Class Library is null.");
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x00046560 File Offset: 0x00044760
	public static EnemyData GetEnemyData(EnemyType enemyType, EnemyRank enemyRank)
	{
		EnemyClassData enemyClassData = null;
		if (EnemyClassLibrary.Instance.m_enemyClassLibrary == null)
		{
			throw new Exception("Enemy Class Library is null.");
		}
		EnemyClassLibrary.Instance.m_enemyClassLibrary.TryGetValue(enemyType, out enemyClassData);
		if (enemyClassData == null)
		{
			Debug.LogFormat("<color=red>Enemy Data: " + enemyType.ToString() + " not found in Enemy Class Library. Please ensure the class exists in the Enemy Class Library scriptable object.</color>", Array.Empty<object>());
			return null;
		}
		return enemyClassData.GetEnemyData(enemyRank);
	}

	// Token: 0x040015C5 RID: 5573
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EnemyClassLibrary";

	// Token: 0x040015C6 RID: 5574
	[SerializeField]
	private EnemyTypeEnemyClassDataDictionary m_enemyClassLibrary;

	// Token: 0x040015C7 RID: 5575
	private static EnemyClassLibrary m_instance;
}
