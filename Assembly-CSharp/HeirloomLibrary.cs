using System;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
[CreateAssetMenu(menuName = "Custom/Libraries/Heirloom Library")]
public class HeirloomLibrary : ScriptableObject
{
	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x0600207E RID: 8318 RVA: 0x00011377 File Offset: 0x0000F577
	private static HeirloomLibrary Instance
	{
		get
		{
			if (HeirloomLibrary.m_instance == null)
			{
				HeirloomLibrary.m_instance = CDGResources.Load<HeirloomLibrary>("Scriptable Objects/Libraries/HeirloomLibrary", "", true);
			}
			return HeirloomLibrary.m_instance;
		}
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000A52F0 File Offset: 0x000A34F0
	public static HeirloomData GetHeirloomData(HeirloomType heirloomType)
	{
		HeirloomData result = null;
		if (HeirloomLibrary.Instance.m_heirloomLibrary != null)
		{
			HeirloomLibrary.Instance.m_heirloomLibrary.TryGetValue(heirloomType, out result);
			return result;
		}
		throw new Exception("Heirloom Library is null.");
	}

	// Token: 0x04001D5A RID: 7514
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/HeirloomLibrary";

	// Token: 0x04001D5B RID: 7515
	[SerializeField]
	private HeirloomTypeHeirloomDataDictionary m_heirloomLibrary;

	// Token: 0x04001D5C RID: 7516
	private static HeirloomLibrary m_instance;
}
