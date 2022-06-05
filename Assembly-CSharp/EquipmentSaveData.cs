using System;
using System.Collections.Generic;

// Token: 0x020002CC RID: 716
[Serializable]
public class EquipmentSaveData : IVersionUpdateable
{
	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06001C6C RID: 7276 RVA: 0x0005BDFD File Offset: 0x00059FFD
	// (set) Token: 0x06001C6D RID: 7277 RVA: 0x0005BE05 File Offset: 0x0005A005
	public bool IsInitialized { get; private set; }

	// Token: 0x06001C6E RID: 7278 RVA: 0x0005BE0E File Offset: 0x0005A00E
	public EquipmentSaveData()
	{
		this.Initialize();
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x0005BE23 File Offset: 0x0005A023
	public void Initialize()
	{
		this.InitializeSkills();
		this.InitializeRunes();
		this.InitializeEquipment();
		this.InitializeLoadouts();
		this.SetStartingEquipmentSaveData();
		this.IsInitialized = true;
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x0005BE4C File Offset: 0x0005A04C
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

	// Token: 0x06001C71 RID: 7281 RVA: 0x0005BEE0 File Offset: 0x0005A0E0
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

	// Token: 0x06001C72 RID: 7282 RVA: 0x0005BF3C File Offset: 0x0005A13C
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

	// Token: 0x06001C73 RID: 7283 RVA: 0x0005C0A8 File Offset: 0x0005A2A8
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

	// Token: 0x06001C74 RID: 7284 RVA: 0x0005C134 File Offset: 0x0005A334
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

	// Token: 0x06001C75 RID: 7285 RVA: 0x0005C23C File Offset: 0x0005A43C
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

	// Token: 0x06001C76 RID: 7286 RVA: 0x0005C28C File Offset: 0x0005A48C
	public EquipmentLoadout GetEquipmentLoadout(ClassType classType)
	{
		EquipmentLoadout result;
		if (this.EquipmentLoadoutDict.TryGetValue(classType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x0005C2AC File Offset: 0x0005A4AC
	public RuneLoadout GetRuneLoadout(ClassType classType)
	{
		RuneLoadout result;
		if (this.RuneLoadoutDict.TryGetValue(classType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x0005C2CC File Offset: 0x0005A4CC
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

	// Token: 0x06001C79 RID: 7289 RVA: 0x0005C454 File Offset: 0x0005A654
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

	// Token: 0x06001C7A RID: 7290 RVA: 0x0005C4B4 File Offset: 0x0005A6B4
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

	// Token: 0x06001C7B RID: 7291 RVA: 0x0005C528 File Offset: 0x0005A728
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

	// Token: 0x06001C7C RID: 7292 RVA: 0x0005C580 File Offset: 0x0005A780
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

	// Token: 0x06001C7D RID: 7293 RVA: 0x0005C5E0 File Offset: 0x0005A7E0
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

	// Token: 0x040019CE RID: 6606
	public int REVISION_NUMBER = 8;

	// Token: 0x040019CF RID: 6607
	public int FILE_NUMBER;

	// Token: 0x040019D0 RID: 6608
	public Dictionary<RuneType, RuneObj> RuneDict;

	// Token: 0x040019D1 RID: 6609
	public Dictionary<SkillTreeType, SkillTreeObj> SkillTreeDict;

	// Token: 0x040019D2 RID: 6610
	public Dictionary<EquipmentType, EquipmentObj> WeaponEquipmentDict;

	// Token: 0x040019D3 RID: 6611
	public Dictionary<EquipmentType, EquipmentObj> HeadEquipmentDict;

	// Token: 0x040019D4 RID: 6612
	public Dictionary<EquipmentType, EquipmentObj> ChestEquipmentDict;

	// Token: 0x040019D5 RID: 6613
	public Dictionary<EquipmentType, EquipmentObj> CapeEquipmentDict;

	// Token: 0x040019D6 RID: 6614
	public Dictionary<EquipmentType, EquipmentObj> TrinketEquipmentDict;

	// Token: 0x040019D7 RID: 6615
	public Dictionary<ClassType, EquipmentLoadout> EquipmentLoadoutDict;

	// Token: 0x040019D8 RID: 6616
	public Dictionary<ClassType, RuneLoadout> RuneLoadoutDict;

	// Token: 0x040019D9 RID: 6617
	public bool EquipmentLoadoutEnabled;

	// Token: 0x040019DA RID: 6618
	public bool RuneLoadoutEnabled;

	// Token: 0x040019DB RID: 6619
	public bool DisableAchievementUnlocks;

	// Token: 0x040019DC RID: 6620
	public Dictionary<EquipmentType, SerializableVector2Int> EquipmentSetStateDict;
}
