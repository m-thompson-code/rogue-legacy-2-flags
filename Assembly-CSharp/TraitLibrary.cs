using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000414 RID: 1044
[CreateAssetMenu(menuName = "Custom/Libraries/Trait Library")]
public class TraitLibrary : ScriptableObject
{
	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06002149 RID: 8521 RVA: 0x00011B90 File Offset: 0x0000FD90
	// (set) Token: 0x0600214A RID: 8522 RVA: 0x00011B9C File Offset: 0x0000FD9C
	public static BaseTrait[] TraitArray
	{
		get
		{
			return TraitLibrary.Instance.m_traitLibrary;
		}
		set
		{
			TraitLibrary.Instance.m_traitLibrary = value;
		}
	}

	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x0600214B RID: 8523 RVA: 0x00011BA9 File Offset: 0x0000FDA9
	public static TraitLibrary Instance
	{
		get
		{
			if (!TraitLibrary.m_instance)
			{
				TraitLibrary.m_instance = CDGResources.Load<TraitLibrary>("Scriptable Objects/Libraries/TraitLibrary", "", true);
				TraitLibrary.m_instance.Initialize();
			}
			return TraitLibrary.m_instance;
		}
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000A6D00 File Offset: 0x000A4F00
	private void Initialize()
	{
		this.m_traitDict = new Dictionary<TraitType, BaseTrait>(this.m_traitLibrary.Length);
		foreach (BaseTrait baseTrait in this.m_traitLibrary)
		{
			if (this.m_traitDict.ContainsKey(baseTrait.TraitType))
			{
				throw new Exception("Duplicate trait: " + baseTrait.TraitType.ToString() + " found in Trait Library.");
			}
			this.m_traitDict.Add(baseTrait.TraitType, baseTrait);
		}
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000A6D88 File Offset: 0x000A4F88
	public static BaseTrait GetTrait(TraitType traitType)
	{
		if (traitType == TraitType.None)
		{
			return null;
		}
		BaseTrait result = null;
		if (TraitLibrary.Instance.m_traitDict != null)
		{
			TraitLibrary.Instance.m_traitDict.TryGetValue(traitType, out result);
			return result;
		}
		throw new Exception("Trait Library is null.");
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000A6DCC File Offset: 0x000A4FCC
	public static TraitData GetTraitData(TraitType traitType)
	{
		if (traitType == TraitType.None)
		{
			return null;
		}
		BaseTrait trait = TraitLibrary.GetTrait(traitType);
		if (trait != null)
		{
			return trait.TraitData;
		}
		return null;
	}

	// Token: 0x04001E35 RID: 7733
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TraitLibrary";

	// Token: 0x04001E36 RID: 7734
	[SerializeField]
	private BaseTrait[] m_traitLibrary;

	// Token: 0x04001E37 RID: 7735
	private Dictionary<TraitType, BaseTrait> m_traitDict;

	// Token: 0x04001E38 RID: 7736
	private static TraitLibrary m_instance;
}
