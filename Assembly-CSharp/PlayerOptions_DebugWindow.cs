using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class PlayerOptions_DebugWindow : MonoBehaviour
{
	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x0600140E RID: 5134 RVA: 0x0003CD00 File Offset: 0x0003AF00
	public AbilityType[] Weapons
	{
		get
		{
			return this.m_weapons;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x0600140F RID: 5135 RVA: 0x0003CD08 File Offset: 0x0003AF08
	public ClassType[] Classes
	{
		get
		{
			return this.m_classes;
		}
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x0003CD10 File Offset: 0x0003AF10
	private void Awake()
	{
		this.InitializeIndexToClassTable();
		this.InitializeIndexToWeaponTable();
		this.m_weaponDropdown.ClearOptions();
		int length = "Weapon".Length;
		List<string> list = (from weapon in this.m_weapons
		select weapon.ToString()).ToList<string>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].EndsWith("Weapon"))
			{
				list[i] = list[i].Substring(0, list[i].Length - length);
			}
		}
		this.m_weaponDropdown.AddOptions(list);
		this.m_classDropdown.ClearOptions();
		this.m_classDropdown.AddOptions((from classType in this.m_classes
		select classType.ToString()).ToList<string>());
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x0003CE08 File Offset: 0x0003B008
	private void InitializeIndexToClassTable()
	{
		this.m_indexToClassTable = new Dictionary<int, ClassType>();
		int num = 0;
		foreach (object obj in Enum.GetValues(typeof(ClassType)))
		{
			ClassType classType = (ClassType)obj;
			if (classType != ClassType.None)
			{
				this.m_indexToClassTable.Add(num, classType);
				num++;
			}
		}
		this.m_classes = new ClassType[this.m_indexToClassTable.Count];
		num = 0;
		foreach (KeyValuePair<int, ClassType> keyValuePair in this.m_indexToClassTable)
		{
			this.m_classes[num] = keyValuePair.Value;
			num++;
		}
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x0003CEEC File Offset: 0x0003B0EC
	private void InitializeIndexToWeaponTable()
	{
		this.m_indexToWeaponTable = new Dictionary<int, AbilityType>();
		int num = 0;
		foreach (AbilityType abilityType in AbilityType_RL.GetWeaponAbilityList())
		{
			if (abilityType != AbilityType.None)
			{
				this.m_indexToWeaponTable.Add(num, abilityType);
				num++;
			}
		}
		this.m_weapons = new AbilityType[this.m_indexToWeaponTable.Count];
		num = 0;
		foreach (KeyValuePair<int, AbilityType> keyValuePair in this.m_indexToWeaponTable)
		{
			this.m_weapons[num] = keyValuePair.Value;
			num++;
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x0003CFC0 File Offset: 0x0003B1C0
	public void SetPlayerClass(int classIndex)
	{
		ClassType classType = this.m_classes[classIndex];
		Debug.LogFormat("Set Player Class to {0}. NOT IMPLEMENTED AT THIS TIME.", new object[]
		{
			classType
		});
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0003CFF0 File Offset: 0x0003B1F0
	public void SetPlayerWeapon(int weaponIndex)
	{
		AbilityType abilityType = this.m_weapons[weaponIndex];
		Debug.LogFormat("Set Player Weapon to {0}. NOT IMPLEMENTED AT THIS TIME.", new object[]
		{
			abilityType
		});
	}

	// Token: 0x040013D8 RID: 5080
	[SerializeField]
	private TMP_Dropdown m_weaponDropdown;

	// Token: 0x040013D9 RID: 5081
	[SerializeField]
	private TMP_Dropdown m_classDropdown;

	// Token: 0x040013DA RID: 5082
	private Dictionary<int, ClassType> m_indexToClassTable;

	// Token: 0x040013DB RID: 5083
	private ClassType[] m_classes;

	// Token: 0x040013DC RID: 5084
	private Dictionary<int, AbilityType> m_indexToWeaponTable;

	// Token: 0x040013DD RID: 5085
	private AbilityType[] m_weapons;
}
