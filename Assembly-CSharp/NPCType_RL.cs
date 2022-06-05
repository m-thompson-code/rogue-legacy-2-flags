using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C16 RID: 3094
public class NPCType_RL
{
	// Token: 0x17001E38 RID: 7736
	// (get) Token: 0x06005A9A RID: 23194 RVA: 0x00031AAD File Offset: 0x0002FCAD
	public static NPCType[] TypeArray
	{
		get
		{
			if (NPCType_RL.m_typeArray == null)
			{
				NPCType_RL.m_typeArray = (Enum.GetValues(typeof(NPCType)) as NPCType[]);
			}
			return NPCType_RL.m_typeArray;
		}
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x0015650C File Offset: 0x0015470C
	public static bool IsNPCUnlocked(NPCType npcType)
	{
		switch (npcType)
		{
		case NPCType.Blacksmith:
			return SkillTreeLogicHelper.IsSmithyUnlocked();
		case NPCType.Enchantress:
			return SkillTreeLogicHelper.IsEnchantressUnlocked();
		case NPCType.Architect:
			return SkillTreeLogicHelper.IsArchitectUnlocked();
		case NPCType.Charon:
			return true;
		case NPCType.Dummy:
			return SkillTreeLogicHelper.IsDummyUnlocked();
		case NPCType.PizzaGirl:
			return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked);
		case NPCType.Totem:
			return SkillTreeLogicHelper.IsTotemUnlocked();
		case NPCType.LivingSafe:
			return SkillTreeLogicHelper.IsLivingSafeUnlocked();
		case NPCType.ChallengeHood:
		case NPCType.NewGamePlusHood:
		case NPCType.SoulShopHood:
			return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DriftHouseUnlocked);
		default:
			return true;
		}
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x00156594 File Offset: 0x00154794
	public static bool IsNPCDialogueConditionUnlocked(NPCDialogueEntry entry, NPCDialogueCondition condition)
	{
		if (condition <= NPCDialogueCondition.EnchantressUnlocked)
		{
			if (condition <= NPCDialogueCondition.StudyBossDefeated)
			{
				if (condition <= NPCDialogueCondition.CastleBossDefeated)
				{
					if (condition == NPCDialogueCondition.ContentLock)
					{
						return false;
					}
					if (condition == NPCDialogueCondition.CastleBossDefeated)
					{
						return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated_FirstTime);
					}
				}
				else
				{
					if (condition == NPCDialogueCondition.BridgeBossDefeated)
					{
						return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated_FirstTime);
					}
					if (condition == NPCDialogueCondition.ForestBossDefeated)
					{
						return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated_FirstTime);
					}
					if (condition == NPCDialogueCondition.StudyBossDefeated)
					{
						return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated_FirstTime);
					}
				}
			}
			else if (condition <= NPCDialogueCondition.CaveBossDefeated)
			{
				if (condition == NPCDialogueCondition.TowerBossDefeated)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated_FirstTime);
				}
				if (condition == NPCDialogueCondition.CaveBossDefeated)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated_FirstTime);
				}
			}
			else
			{
				if (condition == NPCDialogueCondition.FinalBossDefeated)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Defeated_FirstTime);
				}
				if (condition == NPCDialogueCondition.BlacksmithUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.Blacksmith);
				}
				if (condition == NPCDialogueCondition.EnchantressUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.Enchantress);
				}
			}
		}
		else if (condition <= NPCDialogueCondition.ArchitectUnlocked)
		{
			if (condition <= NPCDialogueCondition.LivingSafeUnlocked)
			{
				if (condition == NPCDialogueCondition.TotemUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.Totem);
				}
				if (condition == NPCDialogueCondition.LivingSafeUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.LivingSafe);
				}
			}
			else
			{
				if (condition == NPCDialogueCondition.DummyUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.Dummy);
				}
				if (condition == NPCDialogueCondition.PizzaGirlUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.PizzaGirl);
				}
				if (condition == NPCDialogueCondition.ArchitectUnlocked)
				{
					return NPCType_RL.IsNPCUnlocked(NPCType.Architect);
				}
			}
		}
		else if (condition <= NPCDialogueCondition.NewGamePlusHoodUnlocked)
		{
			if (condition == NPCDialogueCondition.CharonUnlocked)
			{
				return NPCType_RL.IsNPCUnlocked(NPCType.Charon);
			}
			if (condition == NPCDialogueCondition.NewGamePlusHoodUnlocked)
			{
				return NPCType_RL.IsNPCUnlocked(NPCType.NewGamePlusHood);
			}
		}
		else
		{
			if (condition == NPCDialogueCondition.ChallengeHoodUnlocked)
			{
				return NPCType_RL.IsNPCUnlocked(NPCType.ChallengeHood);
			}
			if (condition == NPCDialogueCondition.SoulShopHoodUnlocked)
			{
				return NPCType_RL.IsNPCUnlocked(NPCType.SoulShopHood);
			}
			if (condition == NPCDialogueCondition.LocIDPlayed)
			{
				NPCType npctype = NPCType.None;
				int num = -1;
				foreach (KeyValuePair<NPCType, NPCDialogueEntry[]> keyValuePair in NPCDialogue_EV.NPCDialogueTable)
				{
					NPCDialogueEntry[] value = keyValuePair.Value;
					for (int i = 0; i < value.Length; i++)
					{
						if (value[i].LocID == entry.ConditionValue)
						{
							npctype = keyValuePair.Key;
							num = i;
							break;
						}
					}
					if (npctype != NPCType.None)
					{
						break;
					}
				}
				if (npctype != NPCType.None)
				{
					return SaveManager.PlayerSaveData.GetNPCDialoguesRead(npctype) >= num;
				}
				Debug.Log("NPCDialogueCondition.LocIDPlayed - Could not find matching LocID in NPCDialogue_EV.NPCDialogueTable for LocID: " + entry.ConditionValue);
				return false;
			}
		}
		return true;
	}

	// Token: 0x040047FF RID: 18431
	private static NPCType[] m_typeArray;
}
