using System;
using UnityEngine;

// Token: 0x02000236 RID: 566
[CreateAssetMenu(menuName = "Custom/Libraries/Heirloom Library")]
public class HeirloomLibrary : ScriptableObject
{
	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x060016CB RID: 5835 RVA: 0x00047274 File Offset: 0x00045474
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

	// Token: 0x060016CC RID: 5836 RVA: 0x000472A0 File Offset: 0x000454A0
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

	// Token: 0x04001642 RID: 5698
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/HeirloomLibrary";

	// Token: 0x04001643 RID: 5699
	[SerializeField]
	private HeirloomTypeHeirloomDataDictionary m_heirloomLibrary;

	// Token: 0x04001644 RID: 5700
	private static HeirloomLibrary m_instance;
}
