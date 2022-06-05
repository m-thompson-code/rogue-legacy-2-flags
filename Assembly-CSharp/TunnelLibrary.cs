using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000416 RID: 1046
[CreateAssetMenu(menuName = "Custom/Libraries/Tunnel Library")]
public class TunnelLibrary : ScriptableObject
{
	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x06002152 RID: 8530 RVA: 0x00011BEA File Offset: 0x0000FDEA
	// (set) Token: 0x06002153 RID: 8531 RVA: 0x00011BF2 File Offset: 0x0000FDF2
	public TunnelLibraryEntry[] Entries
	{
		get
		{
			return this.m_entries;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_entries = value;
			}
		}
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x06002154 RID: 8532 RVA: 0x00011C02 File Offset: 0x0000FE02
	public static TunnelLibrary Instance
	{
		get
		{
			if (TunnelLibrary.m_instance == null)
			{
				TunnelLibrary.m_instance = CDGResources.Load<TunnelLibrary>("Scriptable Objects/Libraries/TunnelLibrary", "", true);
			}
			return TunnelLibrary.m_instance;
		}
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000A6DF8 File Offset: 0x000A4FF8
	public static Tunnel GetPrefab(TunnelCategory category)
	{
		if (TunnelLibrary.m_prefabTable == null)
		{
			TunnelLibrary.m_prefabTable = new Dictionary<TunnelCategory, Tunnel>();
			foreach (TunnelLibraryEntry tunnelLibraryEntry in TunnelLibrary.Instance.Entries)
			{
				TunnelLibrary.m_prefabTable.Add(tunnelLibraryEntry.Category, tunnelLibraryEntry.Prefab);
			}
		}
		if (TunnelLibrary.m_prefabTable.ContainsKey(category))
		{
			return TunnelLibrary.m_prefabTable[category];
		}
		Debug.LogFormat("<color=red>| {0} | Table doesn't contain an entry for Tunnel Category ({1}). Using default.</color>", new object[]
		{
			TunnelLibrary.Instance,
			category
		});
		return TunnelLibrary.m_prefabTable[TunnelCategory.Default];
	}

	// Token: 0x04001E3B RID: 7739
	[SerializeField]
	private TunnelLibraryEntry[] m_entries;

	// Token: 0x04001E3C RID: 7740
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TunnelLibrary";

	// Token: 0x04001E3D RID: 7741
	public static string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/TunnelLibrary.asset";

	// Token: 0x04001E3E RID: 7742
	private static TunnelLibrary m_instance = null;

	// Token: 0x04001E3F RID: 7743
	private static Dictionary<TunnelCategory, Tunnel> m_prefabTable = null;
}
