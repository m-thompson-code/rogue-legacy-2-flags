using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000224 RID: 548
[CreateAssetMenu(menuName = "Custom/Libraries/Component Color Library")]
public class ComponentColorLibrary : ScriptableObject
{
	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x06001673 RID: 5747 RVA: 0x00046000 File Offset: 0x00044200
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

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x06001674 RID: 5748 RVA: 0x0004606D File Offset: 0x0004426D
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

	// Token: 0x06001675 RID: 5749 RVA: 0x0004608C File Offset: 0x0004428C
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

	// Token: 0x040015AE RID: 5550
	[SerializeField]
	private List<ComponentColorEntry> m_componentColorEntries;

	// Token: 0x040015AF RID: 5551
	private static ComponentColorLibrary m_instance = null;

	// Token: 0x040015B0 RID: 5552
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/ComponentColorLibrary";

	// Token: 0x040015B1 RID: 5553
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ComponentColorLibrary.asset";

	// Token: 0x040015B2 RID: 5554
	private static Color DEFAULT_COLOR = Color.grey;
}
