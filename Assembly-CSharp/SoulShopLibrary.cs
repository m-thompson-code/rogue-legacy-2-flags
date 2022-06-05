using System;
using UnityEngine;

// Token: 0x0200024F RID: 591
[CreateAssetMenu(menuName = "Custom/Libraries/SoulShopLibrary")]
public class SoulShopLibrary : ScriptableObject
{
	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06001773 RID: 6003 RVA: 0x000490D2 File Offset: 0x000472D2
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

	// Token: 0x06001774 RID: 6004 RVA: 0x000490FC File Offset: 0x000472FC
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

	// Token: 0x040016F3 RID: 5875
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SoulShopLibrary";

	// Token: 0x040016F4 RID: 5876
	[SerializeField]
	private SoulShopTypeSoulShopDataDictionary m_soulShopLibrary;

	// Token: 0x040016F5 RID: 5877
	private static SoulShopLibrary m_instance;
}
