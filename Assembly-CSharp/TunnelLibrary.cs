using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000257 RID: 599
[CreateAssetMenu(menuName = "Custom/Libraries/Tunnel Library")]
public class TunnelLibrary : ScriptableObject
{
	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x06001799 RID: 6041 RVA: 0x0004968F File Offset: 0x0004788F
	// (set) Token: 0x0600179A RID: 6042 RVA: 0x00049697 File Offset: 0x00047897
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

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x0600179B RID: 6043 RVA: 0x000496A7 File Offset: 0x000478A7
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

	// Token: 0x0600179C RID: 6044 RVA: 0x000496D0 File Offset: 0x000478D0
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

	// Token: 0x0400171F RID: 5919
	[SerializeField]
	private TunnelLibraryEntry[] m_entries;

	// Token: 0x04001720 RID: 5920
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TunnelLibrary";

	// Token: 0x04001721 RID: 5921
	public static string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/TunnelLibrary.asset";

	// Token: 0x04001722 RID: 5922
	private static TunnelLibrary m_instance = null;

	// Token: 0x04001723 RID: 5923
	private static Dictionary<TunnelCategory, Tunnel> m_prefabTable = null;
}
