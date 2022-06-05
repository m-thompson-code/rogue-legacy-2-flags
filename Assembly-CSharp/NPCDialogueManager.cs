using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class NPCDialogueManager : MonoBehaviour
{
	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06001D48 RID: 7496 RVA: 0x0000F1F4 File Offset: 0x0000D3F4
	// (set) Token: 0x06001D49 RID: 7497 RVA: 0x0000F1FB File Offset: 0x0000D3FB
	private static NPCDialogueManager Instance
	{
		get
		{
			return NPCDialogueManager.m_instance;
		}
		set
		{
			NPCDialogueManager.m_instance = value;
		}
	}

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06001D4A RID: 7498 RVA: 0x0000F203 File Offset: 0x0000D403
	// (set) Token: 0x06001D4B RID: 7499 RVA: 0x0000F20A File Offset: 0x0000D40A
	public static bool IsInitialized { get; private set; }

	// Token: 0x06001D4C RID: 7500 RVA: 0x0000F212 File Offset: 0x0000D412
	private void Awake()
	{
		NPCDialogueManager.Instance = this;
		NPCDialogueManager.IsInitialized = true;
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x0000F220 File Offset: 0x0000D420
	private void OnDestroy()
	{
		NPCDialogueManager.IsInitialized = false;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x0000F228 File Offset: 0x0000D428
	private void OnLevelEditorWorldCreationComplete(object sender, EventArgs args)
	{
		this.PopulateNPCDialogues_Internal(OnPlayManager.CurrentBiome == BiomeType.HubTown);
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x0009C1E0 File Offset: 0x0009A3E0
	private void GetAvailableNPCDialogues(List<NPCType> availableDialogues, List<NPCType> priorityDialogues)
	{
		availableDialogues.Clear();
		priorityDialogues.Clear();
		foreach (NPCType npctype in NPCType_RL.TypeArray)
		{
			NPCDialogueEntry[] array;
			if (npctype != NPCType.None && NPCType_RL.IsNPCUnlocked(npctype) && NPCDialogue_EV.NPCDialogueTable.TryGetValue(npctype, out array))
			{
				int highestUnlockedNPCDialogue = NPCDialogueManager.GetHighestUnlockedNPCDialogue(npctype);
				if (highestUnlockedNPCDialogue != -1)
				{
					int npcdialoguesRead = SaveManager.PlayerSaveData.GetNPCDialoguesRead(npctype);
					if (npcdialoguesRead < highestUnlockedNPCDialogue)
					{
						bool flag = false;
						if (npcdialoguesRead + 1 < array.Length)
						{
							array[npcdialoguesRead + 1].UnlockConditions.Contains(NPCDialogueCondition.LocIDPlayed);
						}
						if (SaveManager.PlayerSaveData.GetNPCDialogueCooldown(npctype) <= 0)
						{
							if (flag)
							{
								priorityDialogues.Add(npctype);
							}
							availableDialogues.Add(npctype);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x0009C298 File Offset: 0x0009A498
	private void PopulateNPCDialogues_Internal(bool inHubtown)
	{
		SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Clear();
		if (!inHubtown)
		{
			if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal))
			{
				int highestUnlockedNPCDialogue = NPCDialogueManager.GetHighestUnlockedNPCDialogue(NPCType.Dragon);
				if (SaveManager.PlayerSaveData.GetNPCDialoguesRead(NPCType.Dragon) < highestUnlockedNPCDialogue)
				{
					SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Add(NPCType.Dragon);
				}
			}
			return;
		}
		List<NPCType> list = new List<NPCType>();
		List<NPCType> list2 = new List<NPCType>();
		this.GetAvailableNPCDialogues(list2, list);
		int i = 99;
		if (inHubtown)
		{
			i = UnityEngine.Random.Range(NPCDialogue_EV.MAX_DIALOGUE_HUB_TOWN.x, NPCDialogue_EV.MAX_DIALOGUE_HUB_TOWN.y + 1);
		}
		if (list2.Count <= i)
		{
			SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.AddRange(list2);
			return;
		}
		list2.Sort(new Comparison<NPCType>(this.DialogueSort));
		list.Sort(new Comparison<NPCType>(this.DialogueSort));
		int num = 0;
		int num2 = 2;
		while (i > 0)
		{
			bool flag = list.Count > 0;
			List<NPCType> list3 = list2;
			if (flag)
			{
				list3 = list;
			}
			int index = 0;
			if (num >= num2)
			{
				index = UnityEngine.Random.Range(0, list3.Count);
			}
			num++;
			NPCType item = list3[index];
			SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Add(item);
			list3.RemoveAt(index);
			if (flag)
			{
				list2.Remove(item);
			}
			i--;
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x0009C3D8 File Offset: 0x0009A5D8
	private int DialogueSort(NPCType a, NPCType b)
	{
		int num = NPCDialogue_EV.NPCDialogueOrderList.IndexOf(a);
		int num2 = NPCDialogue_EV.NPCDialogueOrderList.IndexOf(b);
		if (num > num2)
		{
			return 1;
		}
		if (num < num2)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0009C40C File Offset: 0x0009A60C
	private bool IsPriorityDialogue_Internal(NPCType npcType)
	{
		NPCDialogueEntry[] array;
		if (NPCDialogue_EV.NPCDialogueTable.TryGetValue(npcType, out array))
		{
			int highestUnlockedNPCDialogue = NPCDialogueManager.GetHighestUnlockedNPCDialogue(npcType);
			int npcdialoguesRead = SaveManager.PlayerSaveData.GetNPCDialoguesRead(npcType);
			if (npcdialoguesRead < highestUnlockedNPCDialogue && array[npcdialoguesRead + 1].UnlockConditions.Contains(NPCDialogueCondition.LocIDPlayed))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0000F239 File Offset: 0x0000D439
	public static void PopulateNPCDialogues(bool inHubtown)
	{
		NPCDialogueManager.Instance.PopulateNPCDialogues_Internal(inHubtown);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x0000F246 File Offset: 0x0000D446
	public static void InitializeGlobalDialogueCD()
	{
		if (SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD)
		{
			SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD = false;
			SaveManager.PlayerSaveData.GlobalNPCDialogueCD = 0;
		}
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x0009C458 File Offset: 0x0009A658
	public static int GetHighestUnlockedNPCDialogue(NPCType npcType)
	{
		int result = -1;
		NPCDialogueEntry[] array;
		if (NPCDialogue_EV.NPCDialogueTable.TryGetValue(npcType, out array))
		{
			int num = 0;
			while (num < array.Length && array[num].IsUnlocked())
			{
				result = num;
				num++;
			}
		}
		return result;
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x0009C494 File Offset: 0x0009A694
	public static bool AllNPCDialoguesRead(NPCType npcType)
	{
		int num = 0;
		if (NPCDialogue_EV.NPCDialogueTable.ContainsKey(npcType))
		{
			num = NPCDialogue_EV.NPCDialogueTable[npcType].Length;
		}
		int npcdialoguesRead = SaveManager.PlayerSaveData.GetNPCDialoguesRead(npcType);
		return num > 0 && npcdialoguesRead >= num - 1;
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x0009C4D8 File Offset: 0x0009A6D8
	public static void ReduceAllNPCDialogueCooldowns(int reductionAmount)
	{
		foreach (NPCType npctype in NPCType_RL.TypeArray)
		{
			if (npctype != NPCType.None)
			{
				SaveManager.PlayerSaveData.SetNPCDialogueCooldown(npctype, reductionAmount, true);
			}
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x0009C510 File Offset: 0x0009A710
	public static bool CanSpeak(NPCType npcType)
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.AboveGround)
		{
			return false;
		}
		bool flag = false;
		return (SaveManager.PlayerSaveData.GlobalNPCDialogueCD <= 0 || flag) && SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Contains(npcType);
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x0009C554 File Offset: 0x0009A754
	public static void MarkNPCAsSpoken(NPCType npcType, bool isNPCDialogue, NPCController npcController)
	{
		if (NPCDialogueManager.CanSpeak(npcType))
		{
			int npcdialogueCooldown = NPCDialogue_EV.GetNPCDialogueCooldown(npcType);
			SaveManager.PlayerSaveData.SetNPCDialogueCooldown(npcType, npcdialogueCooldown, false);
			if (isNPCDialogue)
			{
				SaveManager.PlayerSaveData.SetNPCDialoguesRead(npcType, 1, true);
				int npcdialoguesRead = SaveManager.PlayerSaveData.GetNPCDialoguesRead(npcType);
				int num = 0;
				if (NPCDialogue_EV.NPCDialogueTable.ContainsKey(npcType))
				{
					num = NPCDialogue_EV.NPCDialogueTable[npcType].Length;
				}
				if (num > 0 && npcdialoguesRead >= num - 1)
				{
					SoulDrop.FakeSoulCounter_STATIC = 500;
					Vector3 position = npcController.transform.position;
					position.y += 1f;
					ItemDropManager.DropItem(ItemDropType.Soul, 500, position, true, true, false);
					switch (npcType)
					{
					case NPCType.Blacksmith:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCBlacksmith, StoreType.All);
						break;
					case NPCType.Enchantress:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCEnchantress, StoreType.All);
						break;
					case NPCType.Architect:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCArchitect, StoreType.All);
						break;
					case NPCType.Dummy:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCQuinn, StoreType.All);
						break;
					case NPCType.PizzaGirl:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCPizzaGirl, StoreType.All);
						break;
					case NPCType.Totem:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCMasteryTotem, StoreType.All);
						break;
					case NPCType.LivingSafe:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCLivingSafe, StoreType.All);
						break;
					case NPCType.Dragon:
						StoreAPIManager.GiveAchievement(AchievementType.StoryNPCDragon, StoreType.All);
						break;
					}
				}
			}
			SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Remove(npcType);
		}
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x0009C6C4 File Offset: 0x0009A8C4
	public static int GetTotalCompleteDialogues()
	{
		int num = 0;
		foreach (NPCType npctype in NPCType_RL.TypeArray)
		{
			if (npctype != NPCType.None && NPCDialogueManager.AllNPCDialoguesRead(npctype))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0000F26A File Offset: 0x0000D46A
	public static bool IsPriorityDialogue(NPCType npcType)
	{
		return NPCDialogueManager.Instance.IsPriorityDialogue_Internal(npcType);
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0009C6FC File Offset: 0x0009A8FC
	public static bool SageTotemNewNameRevealed()
	{
		int num = -1;
		int num2 = 0;
		NPCDialogueEntry[] array = NPCDialogue_EV.NPCDialogueTable[NPCType.Totem];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].LocID == "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_MY_NEW_NAME_1 ")
			{
				num = num2;
				break;
			}
			num2++;
		}
		return num > -1 && SaveManager.PlayerSaveData.GetNPCDialoguesRead(NPCType.Totem) >= num;
	}

	// Token: 0x04001AB4 RID: 6836
	private static NPCDialogueManager m_instance;
}
