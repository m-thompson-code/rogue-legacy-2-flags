using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003D9 RID: 985
[CreateAssetMenu(menuName = "Custom/Libraries/Component Color Library")]
public class ComponentColorLibrary : ScriptableObject
{
	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x06002010 RID: 8208 RVA: 0x000A43B4 File Offset: 0x000A25B4
	private static ComponentColorLibrary Instance
	{
		get
		{
			if (ComponentColorLibrary.m_instance == null)
			{
				if (Application.isPlaying)
				{
					ComponentColorLibrary.m_instance = CDGResources.Load<ComponentColorLibrary>("Scriptable Objects/Libraries/ComponentColorLibrary", "", true);
				}
				if (ComponentColorLibrary.m_instance == null)
				{
					Debug.LogFormat("{0}: Unable to find ComponentColorManager at path ({1})", new object[]
					{
						Time.frameCount,
						"Scriptable Objects/Libraries/ComponentColorLibrary"
					});
				}
			}
			return ComponentColorLibrary.m_instance;
		}
	}

	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06002011 RID: 8209 RVA: 0x00010FB7 File Offset: 0x0000F1B7
	public static List<ComponentColorEntry> ComponentColorEntries
	{
		get
		{
			if (ComponentColorLibrary.Instance != null)
			{
				return ComponentColorLibrary.Instance.m_componentColorEntries;
			}
			return new List<ComponentColorEntry>();
		}
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000A4424 File Offset: 0x000A2624
	public static Color GetColor(string component)
	{
		IEnumerable<ComponentColorEntry> source = from entry in ComponentColorLibrary.ComponentColorEntries
		where entry.Component == component
		select entry;
		Color result = ComponentColorLibrary.DEFAULT_COLOR;
		if (source.Count<ComponentColorEntry>() == 1)
		{
			result = source.First<ComponentColorEntry>().Color;
		}
		else if (source.Count<ComponentColorEntry>() > 1)
		{
			Debug.LogFormat("<color=red>[{0}] Found multiple entries matching Component name ({1})</color>", new object[]
			{
				ComponentColorLibrary.Instance.name,
				component
			});
		}
		else
		{
			Debug.LogFormat("<color=red>[{0}] Found zero entries matching Component name ({1})</color>", new object[]
			{
				ComponentColorLibrary.Instance.name,
				component
			});
		}
		return result;
	}

	// Token: 0x04001CBA RID: 7354
	[SerializeField]
	private List<ComponentColorEntry> m_componentColorEntries;

	// Token: 0x04001CBB RID: 7355
	private static ComponentColorLibrary m_instance = null;

	// Token: 0x04001CBC RID: 7356
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/ComponentColorLibrary";

	// Token: 0x04001CBD RID: 7357
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ComponentColorLibrary.asset";

	// Token: 0x04001CBE RID: 7358
	private static Color DEFAULT_COLOR = Color.grey;
}
