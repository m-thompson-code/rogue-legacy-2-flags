using System;
using System.Collections.Generic;

// Token: 0x020004C1 RID: 1217
[Serializable]
public class EquipmentSaveData : IVersionUpdateable
{
	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06002736 RID: 10038 RVA: 0x00016139 File Offset: 0x00014339
	// (set) Token: 0x06002737 RID: 10039 RVA: 0x00016141 File Offset: 0x00014341
	public bool IsInitialized { get; private set; }

	// Token: 0x06002738 RID: 10040 RVA: 0x0001614A File Offset: 0x0001434A
	public EquipmentSaveData()
	{
		this.Initialize();
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x0001615F File Offset: 0x0001435F
	public void Initialize()
	{
		this.InitializeSkills();
		this.InitializeRunes();
		this.InitializeEquipment();
		this.InitializeLoadouts();
		this.SetStartingEquipmentSaveData();
		this.IsInitialized = true;
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x000B8370 File Offset: 0x000B6570
	private void InitializeSkills()
	{
		if (this.SkillTreeDict == null)
		{
			this.SkillTreeDict = new Dictionary<SkillTreeType, SkillTreeObj>();
		}
		foreach (object obj in Enum.GetValues(typeof(SkillTreeType)))
		{
			SkillTreeType skillTreeType = (SkillTreeType)obj;
			if (skillTreeType != SkillTreeType.None && !this.SkillTreeDict.ContainsKey(skillTreeType))
			{
				this.SkillTreeDict.Add(skillTreeType, new SkillTreeObj(skillTreeType));
			}
		}
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x000B8404 File Offset: 0x000B6604
	private void InitializeRunes()
	{
		if (this.RuneDict == null)
		{
			this.RuneDict = new Dictionary<RuneType, RuneObj>();
		}
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None && !this.RuneDict.ContainsKey(runeType))
			{
				this.RuneDict.Add(runeType, new RuneObj(runeType));
			}
		}
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x000B8460 File Offset: 0x000B6660
	private void InitializeEquipment()
	{
		if (this.WeaponEquipmentDict == null)
		{
			this.WeaponEquipmentDict = new Dictionary<EquipmentType, EquipmentObj>();
		}
		if (this.HeadEquipmentDict == null)
		{
			this.HeadEquipmentDict = new Dictionary<EquipmentType, EquipmentObj>();
		}
		if (this.ChestEquipmentDict == null)
		{
			this.ChestEquipmentDict = new Dictionary<EquipmentType, EquipmentObj>();
		}
		if (this.CapeEquipmentDict == null)
		{
			this.CapeEquipmentDict = new Dictionary<EquipmentType, EquipmentObj>();
		}
		if (this.TrinketEquipmentDict == null)
		{
			this.TrinketEquipmentDict = new Dictionary<EquipmentType, EquipmentObj>();
		}
		if (this.EquipmentSetStateDict == null)
		{
			this.EquipmentSetStateDict = new Dictionary<EquipmentType, SerializableVector2Int>();
		}
		foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
		{
			if (equipmentType != EquipmentType.None)
			{
				if (!this.WeaponEquipmentDict.ContainsKey(equipmentType))
				{
					this.WeaponEquipmentDict.Add(equipmentType, new EquipmentObj(EquipmentCategoryType.Weapon, equipmentType));
				}
				if (!this.HeadEquipmentDict.ContainsKey(equipmentType))
				{
					this.HeadEquipmentDict.Add(equipmentType, new EquipmentObj(EquipmentCategoryType.Head, equipmentType));
				}
				if (!this.ChestEquipmentDict.ContainsKey(equipmentType))
				{
					this.ChestEquipmentDict.Add(equipmentType, new EquipmentObj(EquipmentCategoryType.Chest, equipmentType));
				}
				if (!this.CapeEquipmentDict.ContainsKey(equipmentType))
				{
					this.CapeEquipmentDict.Add(equipmentType, new EquipmentObj(EquipmentCategoryType.Cape, equipmentType));
				}
				if (!this.TrinketEquipmentDict.ContainsKey(equipmentType))
				{
					this.TrinketEquipmentDict.Add(equipmentType, new EquipmentObj(EquipmentCategoryType.Trinket, equipmentType));
				}
				if (!this.EquipmentSetStateDict.ContainsKey(equipmentType))
				{
					this.EquipmentSetStateDict.Add(equipmentType, new SerializableVector2Int(0, 0));
				}
			}
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000B85CC File Offset: 0x000B67CC
	private void InitializeLoadouts()
	{
		if (this.EquipmentLoadoutDict == null)
		{
			this.EquipmentLoadoutDict = new Dictionary<ClassType, EquipmentLoadout>();
		}
		if (this.RuneLoadoutDict == null)
		{
			this.RuneLoadoutDict = new Dictionary<ClassType, RuneLoadout>();
		}
		foreach (ClassType classType in ClassType_RL.TypeArray)
		{
			if (classType != ClassType.None)
			{
				if (!this.EquipmentLoadoutDict.ContainsKey(classType))
				{
					this.EquipmentLoadoutDict.Add(classType, new EquipmentLoadout());
				}
				if (!this.RuneLoadoutDict.ContainsKey(classType))
				{
					this.RuneLoadoutDict.Add(classType, new RuneLoadout());
				}
			}
		}
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000B8658 File Offset: 0x000B6858
	private void SetStartingEquipmentSaveData()
	{
		if (this.SkillTreeDict[SkillTreeType.Sword_Class_Unlock].Level < 1)
		{
			this.SkillTreeDict[SkillTreeType.Sword_Class_Unlock].Level = 1;
		}
		if (this.HeadEquipmentDict[EquipmentType.GEAR_BONUS_WEIGHT].UpgradeLevel <= -3)
		{
			this.HeadEquipmentDict[EquipmentType.GEAR_BONUS_WEIGHT].UpgradeLevel = -2;
		}
		if (this.RuneDict[RuneType.Lifesteal].UpgradeLevel <= -3)
		{
			this.RuneDict[RuneType.Lifesteal].UpgradeLevel = -2;
		}
		if (this.RuneDict[RuneType.Lifesteal].UpgradeBlueprintsFound < 1)
		{
			this.RuneDict[RuneType.Lifesteal].UpgradeBlueprintsFound = 1;
		}
		if (this.RuneDict[RuneType.Magnet].UpgradeLevel <= -3)
		{
			this.RuneDict[RuneType.Magnet].UpgradeLevel = -2;
		}
		if (this.RuneDict[RuneType.Magnet].UpgradeBlueprintsFound < 1)
		{
			this.RuneDict[RuneType.Magnet].UpgradeBlueprintsFound = 1;
		}
	}

	// Token: 0x0600273F RID: 10047 RVA: 0x000B8760 File Offset: 0x000B6960
	public Dictionary<EquipmentType, EquipmentObj> GetEquipmentDict(EquipmentCategoryType category)
	{
		switch (category)
		{
		case EquipmentCategoryType.Weapon:
			return this.WeaponEquipmentDict;
		case EquipmentCategoryType.Head:
			return this.HeadEquipmentDict;
		case EquipmentCategoryType.Chest:
			return this.ChestEquipmentDict;
		case EquipmentCategoryType.Cape:
			return this.CapeEquipmentDict;
		case EquipmentCategoryType.Trinket:
			return this.TrinketEquipmentDict;
		default:
			return null;
		}
	}

	// Token: 0x06002740 RID: 10048 RVA: 0x000B87B0 File Offset: 0x000B69B0
	public EquipmentLoadout GetEquipmentLoadout(ClassType classType)
	{
		EquipmentLoadout result;
		if (this.EquipmentLoadoutDict.TryGetValue(classType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x000B87D0 File Offset: 0x000B69D0
	public RuneLoadout GetRuneLoadout(ClassType classType)
	{
		RuneLoadout result;
		if (this.RuneLoadoutDict.TryGetValue(classType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x000B87F0 File Offset: 0x000B69F0
	public void UpdateVersion()
	{
		this.Initialize();
		if (this.REVISION_NUMBER != 8)
		{
			if (this.REVISION_NUMBER <= 0)
			{
				this.ResetEquipment(EquipmentCategoryType.Weapon);
				this.ResetEquipment(EquipmentCategoryType.Head);
				this.ResetEquipment(EquipmentCategoryType.Chest);
				this.ResetEquipment(EquipmentCategoryType.Cape);
				this.ResetEquipment(EquipmentCategoryType.Trinket);
				this.ResetEquipmentSetLevels();
				this.ResetSkillTree();
				this.ResetRunes();
				this.SetStartingEquipmentSaveData();
			}
			if (this.REVISION_NUMBER <= 1)
			{
				this.ResetSkillTree();
			}
			if (this.REVISION_NUMBER <= 2)
			{
				this.UnequipAllRunes();
			}
			if (this.REVISION_NUMBER <= 3)
			{
				this.UnequipAllRunes();
				this.ResetSkillTree();
			}
			if (this.REVISION_NUMBER <= 4)
			{
				foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in this.RuneDict)
				{
					RuneObj value = keyValuePair.Value;
					if (value != null)
					{
						if (value.UpgradeLevel > -3 && value.UpgradeBlueprintsFound < 1)
						{
							value.UpgradeBlueprintsFound = 1;
						}
						else if (value.UpgradeBlueprintsFound > 0 && value.UpgradeLevel < -2)
						{
							value.UpgradeLevel = -2;
						}
					}
				}
			}
			if (this.REVISION_NUMBER <= 5)
			{
				this.UnequipAllRunes();
			}
			if (this.REVISION_NUMBER <= 6)
			{
				this.UnequipAllRunes();
				this.ResetSkillTree();
				this.SetStartingEquipmentSaveData();
			}
			if (this.REVISION_NUMBER <= 7)
			{
				this.SkillTreeDict[SkillTreeType.Gold_Gain_Up_4].Level = 0;
				this.SkillTreeDict[SkillTreeType.Gold_Gain_Up_5].Level = 0;
			}
		}
		this.REVISION_NUMBER = 8;
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x000B8978 File Offset: 0x000B6B78
	private void UnequipAllRunes()
	{
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in this.RuneDict)
		{
			RuneObj value = keyValuePair.Value;
			if (value != null)
			{
				value.EquippedLevel = 0;
			}
		}
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000B89D8 File Offset: 0x000B6BD8
	private void ResetEquipment(EquipmentCategoryType categoryType)
	{
		int num = -1;
		Dictionary<EquipmentType, EquipmentObj> equipmentDict = this.GetEquipmentDict(categoryType);
		if (equipmentDict != null)
		{
			foreach (KeyValuePair<EquipmentType, EquipmentObj> keyValuePair in equipmentDict)
			{
				EquipmentObj value = keyValuePair.Value;
				if (value != null && value.UpgradeLevel > num)
				{
					value.UpgradeLevel = num;
				}
			}
		}
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000B8A4C File Offset: 0x000B6C4C
	private void ResetEquipmentSetLevels()
	{
		foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
		{
			if (equipmentType != EquipmentType.None && this.EquipmentSetStateDict.ContainsKey(equipmentType))
			{
				SerializableVector2Int value = this.EquipmentSetStateDict[equipmentType];
				value.x = 0;
				this.EquipmentSetStateDict[equipmentType] = value;
			}
		}
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x000B8AA4 File Offset: 0x000B6CA4
	private void ResetSkillTree()
	{
		foreach (KeyValuePair<SkillTreeType, SkillTreeObj> keyValuePair in this.SkillTreeDict)
		{
			SkillTreeObj value = keyValuePair.Value;
			if (value != null)
			{
				value.Level = 0;
			}
		}
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x000B8B04 File Offset: 0x000B6D04
	private void ResetRunes()
	{
		int num = -1;
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in this.RuneDict)
		{
			RuneObj value = keyValuePair.Value;
			if (value != null)
			{
				if (value.UpgradeLevel > num)
				{
					value.UpgradeLevel = num;
				}
				value.EquippedLevel = 0;
			}
		}
	}

	// Token: 0x040021D5 RID: 8661
	public int REVISION_NUMBER = 8;

	// Token: 0x040021D6 RID: 8662
	public int FILE_NUMBER;

	// Token: 0x040021D7 RID: 8663
	public Dictionary<RuneType, RuneObj> RuneDict;

	// Token: 0x040021D8 RID: 8664
	public Dictionary<SkillTreeType, SkillTreeObj> SkillTreeDict;

	// Token: 0x040021D9 RID: 8665
	public Dictionary<EquipmentType, EquipmentObj> WeaponEquipmentDict;

	// Token: 0x040021DA RID: 8666
	public Dictionary<EquipmentType, EquipmentObj> HeadEquipmentDict;

	// Token: 0x040021DB RID: 8667
	public Dictionary<EquipmentType, EquipmentObj> ChestEquipmentDict;

	// Token: 0x040021DC RID: 8668
	public Dictionary<EquipmentType, EquipmentObj> CapeEquipmentDict;

	// Token: 0x040021DD RID: 8669
	public Dictionary<EquipmentType, EquipmentObj> TrinketEquipmentDict;

	// Token: 0x040021DE RID: 8670
	public Dictionary<ClassType, EquipmentLoadout> EquipmentLoadoutDict;

	// Token: 0x040021DF RID: 8671
	public Dictionary<ClassType, RuneLoadout> RuneLoadoutDict;

	// Token: 0x040021E0 RID: 8672
	public bool EquipmentLoadoutEnabled;

	// Token: 0x040021E1 RID: 8673
	public bool RuneLoadoutEnabled;

	// Token: 0x040021E2 RID: 8674
	public bool DisableAchievementUnlocks;

	// Token: 0x040021E3 RID: 8675
	public Dictionary<EquipmentType, SerializableVector2Int> EquipmentSetStateDict;
}
