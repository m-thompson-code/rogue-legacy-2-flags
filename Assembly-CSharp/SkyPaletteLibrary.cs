using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200024C RID: 588
[CreateAssetMenu(menuName = "Custom/Libraries/Sky Palette Library")]
public class SkyPaletteLibrary : ScriptableObject
{
	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x0600176D RID: 5997 RVA: 0x00048FC6 File Offset: 0x000471C6
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

	// Token: 0x0600176E RID: 5998 RVA: 0x00048FFC File Offset: 0x000471FC
	public static SkyPaletteEntry GetPalette(SkyPaletteType type)
	{
		SkyPaletteEntry result = null;
		if (SkyPaletteLibrary.Instance.m_skyPaletteDict.TryGetValue(type, out result))
		{
			return result;
		}
		throw new Exception("SkyPaletteType: " + type.ToString() + " not found in SkyPalette Library.");
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x00049044 File Offset: 0x00047244
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

	// Token: 0x040016E1 RID: 5857
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkyPaletteLibrary";

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	private SkyPaletteEntry[] m_skyPaletteArray;

	// Token: 0x040016E3 RID: 5859
	private Dictionary<SkyPaletteType, SkyPaletteEntry> m_skyPaletteDict;

	// Token: 0x040016E4 RID: 5860
	private static SkyPaletteLibrary m_instance;
}
