using System;
using UnityEngine;

// Token: 0x020003D2 RID: 978
[CreateAssetMenu(menuName = "Custom/Libraries/BurdenLibrary")]
public class BurdenLibrary : ScriptableObject
{
	// Token: 0x17000E44 RID: 3652
	// (get) Token: 0x06001FFE RID: 8190 RVA: 0x00010F00 File Offset: 0x0000F100
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

	// Token: 0x06001FFF RID: 8191 RVA: 0x000A427C File Offset: 0x000A247C
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

	// Token: 0x04001CA3 RID: 7331
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/BurdenLibrary";

	// Token: 0x04001CA4 RID: 7332
	[SerializeField]
	private BurdenTypeBurdenDataDictionary m_burdenLibrary;

	// Token: 0x04001CA5 RID: 7333
	private static BurdenLibrary m_instance;
}
