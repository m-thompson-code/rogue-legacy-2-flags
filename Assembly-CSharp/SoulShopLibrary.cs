using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
[CreateAssetMenu(menuName = "Custom/Libraries/SoulShopLibrary")]
public class SoulShopLibrary : ScriptableObject
{
	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06002126 RID: 8486 RVA: 0x00011A33 File Offset: 0x0000FC33
	private static SoulShopLibrary Instance
	{
		get
		{
			if (SoulShopLibrary.m_instance == null)
			{
				SoulShopLibrary.m_instance = CDGResources.Load<SoulShopLibrary>("Scriptable Objects/Libraries/SoulShopLibrary", "", true);
			}
			return SoulShopLibrary.m_instance;
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000A69F0 File Offset: 0x000A4BF0
	public static SoulShopData GetSoulShopData(SoulShopType soulShopType)
	{
		SoulShopData soulShopData;
		if (SoulShopLibrary.Instance.m_soulShopLibrary.TryGetValue(soulShopType, out soulShopData))
		{
			return soulShopData;
		}
		Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find SoulShopData ({2}) in SoulShop Library.</color>", new object[]
		{
			Time.frameCount,
			SoulShopLibrary.Instance,
			soulShopData
		});
		return null;
	}

	// Token: 0x04001E0B RID: 7691
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SoulShopLibrary";

	// Token: 0x04001E0C RID: 7692
	[SerializeField]
	private SoulShopTypeSoulShopDataDictionary m_soulShopLibrary;

	// Token: 0x04001E0D RID: 7693
	private static SoulShopLibrary m_instance;
}
