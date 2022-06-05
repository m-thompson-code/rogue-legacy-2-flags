using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003CB RID: 971
[CreateAssetMenu(menuName = "Custom/Libraries/Ability Library")]
public class AbilityLibrary : ScriptableObject
{
	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x00010DFC File Offset: 0x0000EFFC
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

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000A4050 File Offset: 0x000A2250
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

	// Token: 0x06001FEA RID: 8170 RVA: 0x000A40E8 File Offset: 0x000A22E8
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

	// Token: 0x04001C8C RID: 7308
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AbilityLibrary";

	// Token: 0x04001C8D RID: 7309
	[SerializeField]
	private BaseAbility_RL[] m_abilityList;

	// Token: 0x04001C8E RID: 7310
	private static AbilityLibrary m_instance;

	// Token: 0x04001C8F RID: 7311
	private Dictionary<AbilityType, BaseAbility_RL> m_abilityLibrary;
}
