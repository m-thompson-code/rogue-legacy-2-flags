using System;
using UnityEngine;

// Token: 0x020003DF RID: 991
[CreateAssetMenu(menuName = "Custom/Libraries/Enemy Class Library")]
public class EnemyClassLibrary : ScriptableObject
{
	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06002033 RID: 8243 RVA: 0x00011136 File Offset: 0x0000F336
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

	// Token: 0x06002034 RID: 8244 RVA: 0x000A4784 File Offset: 0x000A2984
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

	// Token: 0x06002035 RID: 8245 RVA: 0x000A47F0 File Offset: 0x000A29F0
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

	// Token: 0x04001CD5 RID: 7381
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EnemyClassLibrary";

	// Token: 0x04001CD6 RID: 7382
	[SerializeField]
	private EnemyTypeEnemyClassDataDictionary m_enemyClassLibrary;

	// Token: 0x04001CD7 RID: 7383
	private static EnemyClassLibrary m_instance;
}
