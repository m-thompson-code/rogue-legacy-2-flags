using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000255 RID: 597
[CreateAssetMenu(menuName = "Custom/Libraries/Trait Library")]
public class TraitLibrary : ScriptableObject
{
	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x06001790 RID: 6032 RVA: 0x00049532 File Offset: 0x00047732
	// (set) Token: 0x06001791 RID: 6033 RVA: 0x0004953E File Offset: 0x0004773E
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

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x06001792 RID: 6034 RVA: 0x0004954B File Offset: 0x0004774B
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

	// Token: 0x06001793 RID: 6035 RVA: 0x00049580 File Offset: 0x00047780
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

	// Token: 0x06001794 RID: 6036 RVA: 0x00049608 File Offset: 0x00047808
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

	// Token: 0x06001795 RID: 6037 RVA: 0x0004964C File Offset: 0x0004784C
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

	// Token: 0x04001719 RID: 5913
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TraitLibrary";

	// Token: 0x0400171A RID: 5914
	[SerializeField]
	private BaseTrait[] m_traitLibrary;

	// Token: 0x0400171B RID: 5915
	private Dictionary<TraitType, BaseTrait> m_traitDict;

	// Token: 0x0400171C RID: 5916
	private static TraitLibrary m_instance;
}
