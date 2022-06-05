using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000409 RID: 1033
[CreateAssetMenu(menuName = "Custom/Libraries/Sky Palette Library")]
public class SkyPaletteLibrary : ScriptableObject
{
	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x06002120 RID: 8480 RVA: 0x00011A00 File Offset: 0x0000FC00
	private static SkyPaletteLibrary Instance
	{
		get
		{
			if (SkyPaletteLibrary.m_instance == null)
			{
				SkyPaletteLibrary.m_instance = CDGResources.Load<SkyPaletteLibrary>("Scriptable Objects/Libraries/SkyPaletteLibrary", "", true);
				SkyPaletteLibrary.m_instance.Initialize();
			}
			return SkyPaletteLibrary.m_instance;
		}
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000A692C File Offset: 0x000A4B2C
	public static SkyPaletteEntry GetPalette(SkyPaletteType type)
	{
		SkyPaletteEntry result = null;
		if (SkyPaletteLibrary.Instance.m_skyPaletteDict.TryGetValue(type, out result))
		{
			return result;
		}
		throw new Exception("SkyPaletteType: " + type.ToString() + " not found in SkyPalette Library.");
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000A6974 File Offset: 0x000A4B74
	private void Initialize()
	{
		this.m_skyPaletteDict = new Dictionary<SkyPaletteType, SkyPaletteEntry>();
		foreach (SkyPaletteEntry skyPaletteEntry in this.m_skyPaletteArray)
		{
			if (this.m_skyPaletteDict.ContainsKey(skyPaletteEntry.SkyPaletteType))
			{
				throw new Exception("SkyPaletteType: " + skyPaletteEntry.SkyPaletteType.ToString() + " already found in SkyPaletteLibrary.  Duplicates not allowed.");
			}
			this.m_skyPaletteDict.Add(skyPaletteEntry.SkyPaletteType, skyPaletteEntry);
		}
	}

	// Token: 0x04001DF9 RID: 7673
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkyPaletteLibrary";

	// Token: 0x04001DFA RID: 7674
	[SerializeField]
	private SkyPaletteEntry[] m_skyPaletteArray;

	// Token: 0x04001DFB RID: 7675
	private Dictionary<SkyPaletteType, SkyPaletteEntry> m_skyPaletteDict;

	// Token: 0x04001DFC RID: 7676
	private static SkyPaletteLibrary m_instance;
}
