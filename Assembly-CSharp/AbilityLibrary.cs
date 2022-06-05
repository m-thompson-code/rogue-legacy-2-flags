using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000218 RID: 536
[CreateAssetMenu(menuName = "Custom/Libraries/Ability Library")]
public class AbilityLibrary : ScriptableObject
{
	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600164C RID: 5708 RVA: 0x00045A7C File Offset: 0x00043C7C
	private static AbilityLibrary Instance
	{
		get
		{
			if (!AbilityLibrary.m_instance)
			{
				AbilityLibrary.m_instance = CDGResources.Load<AbilityLibrary>("Scriptable Objects/Libraries/AbilityLibrary", "", true);
				AbilityLibrary.m_instance.Initialize();
			}
			return AbilityLibrary.m_instance;
		}
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x00045AB0 File Offset: 0x00043CB0
	public void Initialize()
	{
		if (this.m_abilityLibrary == null)
		{
			this.m_abilityLibrary = new Dictionary<AbilityType, BaseAbility_RL>();
		}
		else
		{
			this.m_abilityLibrary.Clear();
		}
		foreach (BaseAbility_RL baseAbility_RL in this.m_abilityList)
		{
			if (!(baseAbility_RL == null))
			{
				AbilityType abilityType = baseAbility_RL.AbilityType;
				if (this.m_abilityLibrary.ContainsKey(abilityType))
				{
					throw new Exception("Entry in Ability Library for " + abilityType.ToString() + " already found. Ability Library cannot have dupilcates.");
				}
				this.m_abilityLibrary.Add(abilityType, baseAbility_RL);
			}
		}
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00045B48 File Offset: 0x00043D48
	public static BaseAbility_RL GetAbility(AbilityType abilityType)
	{
		BaseAbility_RL baseAbility_RL = null;
		if (AbilityLibrary.Instance.m_abilityLibrary != null)
		{
			AbilityLibrary.Instance.m_abilityLibrary.TryGetValue(abilityType, out baseAbility_RL);
		}
		if (!baseAbility_RL)
		{
			Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find Ability ({2}) in Ability Library.</color>", new object[]
			{
				Time.frameCount,
				AbilityLibrary.Instance,
				abilityType
			});
		}
		return baseAbility_RL;
	}

	// Token: 0x04001589 RID: 5513
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AbilityLibrary";

	// Token: 0x0400158A RID: 5514
	[SerializeField]
	private BaseAbility_RL[] m_abilityList;

	// Token: 0x0400158B RID: 5515
	private static AbilityLibrary m_instance;

	// Token: 0x0400158C RID: 5516
	private Dictionary<AbilityType, BaseAbility_RL> m_abilityLibrary;
}
