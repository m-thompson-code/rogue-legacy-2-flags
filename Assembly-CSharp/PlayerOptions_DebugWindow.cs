using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class PlayerOptions_DebugWindow : MonoBehaviour
{
	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0000F063 File Offset: 0x0000D263
	public AbilityType[] Weapons
	{
		get
		{
			return this.m_weapons;
		}
	}

	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x06001D20 RID: 7456 RVA: 0x0000F06B File Offset: 0x0000D26B
	public ClassType[] Classes
	{
		get
		{
			return this.m_classes;
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0009BB38 File Offset: 0x00099D38
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

	// Token: 0x06001D22 RID: 7458 RVA: 0x0009BC30 File Offset: 0x00099E30
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

	// Token: 0x06001D23 RID: 7459 RVA: 0x0009BD14 File Offset: 0x00099F14
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

	// Token: 0x06001D24 RID: 7460 RVA: 0x0009BDE8 File Offset: 0x00099FE8
	public void SetPlayerClass(int classIndex)
	{
		ClassType classType = this.m_classes[classIndex];
		Debug.LogFormat("Set Player Class to {0}. NOT IMPLEMENTED AT THIS TIME.", new object[]
		{
			classType
		});
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0009BE18 File Offset: 0x0009A018
	public void SetPlayerWeapon(int weaponIndex)
	{
		AbilityType abilityType = this.m_weapons[weaponIndex];
		Debug.LogFormat("Set Player Weapon to {0}. NOT IMPLEMENTED AT THIS TIME.", new object[]
		{
			abilityType
		});
	}

	// Token: 0x04001A75 RID: 6773
	[SerializeField]
	private TMP_Dropdown m_weaponDropdown;

	// Token: 0x04001A76 RID: 6774
	[SerializeField]
	private TMP_Dropdown m_classDropdown;

	// Token: 0x04001A77 RID: 6775
	private Dictionary<int, ClassType> m_indexToClassTable;

	// Token: 0x04001A78 RID: 6776
	private ClassType[] m_classes;

	// Token: 0x04001A79 RID: 6777
	private Dictionary<int, AbilityType> m_indexToWeaponTable;

	// Token: 0x04001A7A RID: 6778
	private AbilityType[] m_weapons;
}
