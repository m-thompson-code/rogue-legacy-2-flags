using System;
using UnityEngine;

// Token: 0x02000405 RID: 1029
[CreateAssetMenu(menuName = "Custom/Libraries/Rune Library")]
public class RuneLibrary : ScriptableObject
{
	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06002113 RID: 8467 RVA: 0x00011985 File Offset: 0x0000FB85
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

	// Token: 0x06002114 RID: 8468 RVA: 0x000A6878 File Offset: 0x000A4A78
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

	// Token: 0x04001DEC RID: 7660
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RuneLibrary";

	// Token: 0x04001DED RID: 7661
	[Space(10f)]
	[SerializeField]
	private RuneTypeRuneDataDictionary m_runeLibrary;

	// Token: 0x04001DEE RID: 7662
	private static RuneLibrary m_instance;
}
