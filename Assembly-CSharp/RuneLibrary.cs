using System;
using UnityEngine;

// Token: 0x02000248 RID: 584
[CreateAssetMenu(menuName = "Custom/Libraries/Rune Library")]
public class RuneLibrary : ScriptableObject
{
	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x06001760 RID: 5984 RVA: 0x00048E6D File Offset: 0x0004706D
	private static RuneLibrary Instance
	{
		get
		{
			if (RuneLibrary.m_instance == null)
			{
				RuneLibrary.m_instance = CDGResources.Load<RuneLibrary>("Scriptable Objects/Libraries/RuneLibrary", "", true);
			}
			return RuneLibrary.m_instance;
		}
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00048E98 File Offset: 0x00047098
	public static RuneData GetRuneData(RuneType runeType)
	{
		RuneData result = null;
		if (RuneLibrary.Instance.m_runeLibrary != null)
		{
			RuneLibrary.Instance.m_runeLibrary.TryGetValue(runeType, out result);
			return result;
		}
		throw new Exception("Rune Library is null.");
	}

	// Token: 0x040016D4 RID: 5844
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RuneLibrary";

	// Token: 0x040016D5 RID: 5845
	[Space(10f)]
	[SerializeField]
	private RuneTypeRuneDataDictionary m_runeLibrary;

	// Token: 0x040016D6 RID: 5846
	private static RuneLibrary m_instance;
}
