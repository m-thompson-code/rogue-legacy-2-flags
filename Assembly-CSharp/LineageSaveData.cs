using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020002CF RID: 719
[Serializable]
public class LineageSaveData : IVersionUpdateable
{
	// Token: 0x06001C87 RID: 7303 RVA: 0x0005C8A8 File Offset: 0x0005AAA8
	public LineageSaveData()
	{
		this.AddCharacterToLineage(new CharacterData
		{
			BodyType = 0,
			EyeType = 0,
			FacialHairType = 0,
			HairColorType = 0,
			MouthType = 0,
			SkinColorType = 0,
			IsFemale = false,
			ClassType = ClassType.SwordClass,
			Weapon = AbilityType.SwordWeapon,
			EdgeEquipmentType = EquipmentType.None,
			CapeEquipmentType = EquipmentType.None,
			HeadEquipmentType = EquipmentType.None,
			ChestEquipmentType = EquipmentType.None,
			TrinketEquipmentType = EquipmentType.None,
			Name = "Lee"
		});
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x0005C958 File Offset: 0x0005AB58
	public void CreatePortraitTest()
	{
		if (this.m_portraitTestComplete)
		{
			return;
		}
		for (int i = 0; i < 52; i++)
		{
			CharacterData characterData = new CharacterData();
			characterData.BodyType = UnityEngine.Random.Range(0, LookLibrary.GetBodyLookData().Count);
			characterData.EyeType = UnityEngine.Random.Range(0, LookLibrary.GetEyeLookData().Count);
			characterData.FacialHairType = UnityEngine.Random.Range(0, LookLibrary.GetFacialHairLookData().Count);
			characterData.HairColorType = UnityEngine.Random.Range(0, LookLibrary.GetHairColorLookData().Count);
			characterData.MouthType = UnityEngine.Random.Range(0, LookLibrary.GetMouthLookData().Count);
			characterData.SkinColorType = UnityEngine.Random.Range(0, LookLibrary.GetSkinColorLookData().Count);
			characterData.IsFemale = (UnityEngine.Random.Range(0, 2) == 1);
			List<ClassType> list = ClassType_RL.TypeArray.ToList<ClassType>();
			list.Remove(ClassType.None);
			characterData.ClassType = list[UnityEngine.Random.Range(0, list.Count)];
			List<AbilityType> list2 = new List<AbilityType>();
			foreach (object obj in Enum.GetValues(typeof(AbilityType)))
			{
				AbilityType abilityType = (AbilityType)obj;
				int num = (int)abilityType;
				if (num > 10 && num < 130 && AbilityLibrary.GetAbility(abilityType) != null)
				{
					list2.Add(abilityType);
				}
			}
			characterData.Weapon = list2[UnityEngine.Random.Range(0, list2.Count)];
			characterData.EdgeEquipmentType = (EquipmentType)UnityEngine.Random.Range(0, 3);
			characterData.CapeEquipmentType = (EquipmentType)UnityEngine.Random.Range(0, 3);
			characterData.HeadEquipmentType = (EquipmentType)UnityEngine.Random.Range(0, 3);
			characterData.ChestEquipmentType = (EquipmentType)UnityEngine.Random.Range(0, 3);
			characterData.TrinketEquipmentType = (EquipmentType)UnityEngine.Random.Range(0, 3);
			string[] availableNames = CharacterCreator.GetAvailableNames(characterData.IsFemale);
			string name = availableNames[UnityEngine.Random.Range(0, availableNames.Length)];
			characterData.Name = name;
			this.AddCharacterToLineage(characterData);
		}
		this.m_portraitTestComplete = true;
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x0005CB58 File Offset: 0x0005AD58
	public void AddCharacterToLineage(CharacterData charData)
	{
		int count = this.LineageHeirList.Count;
		if (count >= 100)
		{
			for (int i = 0; i < count - 1; i++)
			{
				this.LineageHeirList[i] = this.LineageHeirList[i + 1];
			}
			this.LineageHeirList[count - 1] = charData;
		}
		else
		{
			this.LineageHeirList.Add(charData);
		}
		this.NumberOfHeirs++;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x0005CBCC File Offset: 0x0005ADCC
	public void UpdateVersion()
	{
		if (this.REVISION_NUMBER != 1 && this.REVISION_NUMBER <= 0)
		{
			this.NumberOfHeirs = this.LineageHeirList.Count;
			if (this.DuplicateNameCountDict == null)
			{
				this.DuplicateNameCountDict = new Dictionary<string, int>(20);
			}
			if (this.LineageHeirList == null)
			{
				this.LineageHeirList = new List<CharacterData>(100);
			}
			foreach (CharacterData characterData in this.LineageHeirList)
			{
				if (!this.DuplicateNameCountDict.ContainsKey(characterData.Name))
				{
					this.DuplicateNameCountDict.Add(characterData.Name, 0);
				}
				else
				{
					this.DuplicateNameCountDict[characterData.Name] = this.DuplicateNameCountDict[characterData.Name] + 1;
				}
			}
			if (this.LineageHeirList.Count >= 100)
			{
				List<CharacterData> range = this.LineageHeirList.GetRange(this.LineageHeirList.Count - 100, 100);
				this.LineageHeirList.Clear();
				this.LineageHeirList.AddRange(range);
			}
		}
		this.REVISION_NUMBER = 1;
	}

	// Token: 0x040019E4 RID: 6628
	public const int MAX_PORTRAITS = 100;

	// Token: 0x040019E5 RID: 6629
	public int REVISION_NUMBER = 1;

	// Token: 0x040019E6 RID: 6630
	public int FILE_NUMBER;

	// Token: 0x040019E7 RID: 6631
	public List<CharacterData> LineageHeirList = new List<CharacterData>(100);

	// Token: 0x040019E8 RID: 6632
	public Dictionary<string, int> DuplicateNameCountDict = new Dictionary<string, int>(20);

	// Token: 0x040019E9 RID: 6633
	public int NumberOfHeirs;

	// Token: 0x040019EA RID: 6634
	[NonSerialized]
	private bool m_portraitTestComplete;
}
