using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
[CreateAssetMenu(menuName = "Custom/Libraries/BurdenLibrary")]
public class BurdenLibrary : ScriptableObject
{
	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06001662 RID: 5730 RVA: 0x00045DE6 File Offset: 0x00043FE6
	private static BurdenLibrary Instance
	{
		get
		{
			if (BurdenLibrary.m_instance == null)
			{
				BurdenLibrary.m_instance = CDGResources.Load<BurdenLibrary>("Scriptable Objects/Libraries/BurdenLibrary", "", true);
			}
			return BurdenLibrary.m_instance;
		}
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x00045E10 File Offset: 0x00044010
	public static BurdenData GetBurdenData(BurdenType burdenType)
	{
		BurdenData result;
		if (BurdenLibrary.Instance.m_burdenLibrary.TryGetValue(burdenType, out result))
		{
			return result;
		}
		Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find BurdenData ({2}) in Burden Library.</color>", new object[]
		{
			Time.frameCount,
			BurdenLibrary.Instance,
			burdenType
		});
		return null;
	}

	// Token: 0x040015A0 RID: 5536
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/BurdenLibrary";

	// Token: 0x040015A1 RID: 5537
	[SerializeField]
	private BurdenTypeBurdenDataDictionary m_burdenLibrary;

	// Token: 0x040015A2 RID: 5538
	private static BurdenLibrary m_instance;
}
