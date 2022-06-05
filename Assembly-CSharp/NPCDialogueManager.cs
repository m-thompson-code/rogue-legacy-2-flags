using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class NPCDialogueManager : MonoBehaviour
{
	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06001433 RID: 5171 RVA: 0x0003D543 File Offset: 0x0003B743
	// (set) Token: 0x06001434 RID: 5172 RVA: 0x0003D54A File Offset: 0x0003B74A
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

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06001435 RID: 5173 RVA: 0x0003D552 File Offset: 0x0003B752
	// (set) Token: 0x06001436 RID: 5174 RVA: 0x0003D559 File Offset: 0x0003B759
	public static bool IsInitialized { get; private set; }

	// Token: 0x06001437 RID: 5175 RVA: 0x0003D561 File Offset: 0x0003B761
	private void Awake()
	{
		NPCDialogueManager.Instance = this;
		NPCDialogueManager.IsInitialized = true;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0003D56F File Offset: 0x0003B76F
	private void OnDestroy()
	{
		NPCDialogueManager.IsInitialized = false;
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x0003D577 File Offset: 0x0003B777
	private void OnLevelEditorWorldCreationComplete(object sender, EventArgs args)
	{
		this.PopulateNPCDialogues_Internal(OnPlayManager.CurrentBiome == BiomeType.HubTown);
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0003D588 File Offset: 0x0003B788
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

	// Token: 0x0600143B RID: 5179 RVA: 0x0003D640 File Offset: 0x0003B840
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

	// Token: 0x0600143C RID: 5180 RVA: 0x0003D780 File Offset: 0x0003B980
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

	// Token: 0x0600143D RID: 5181 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
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

	// Token: 0x0600143E RID: 5182 RVA: 0x0003D800 File Offset: 0x0003BA00
	public static void PopulateNPCDialogues(bool inHubtown)
	{
		NPCDialogueManager.Instance.PopulateNPCDialogues_Internal(inHubtown);
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x0003D80D File Offset: 0x0003BA0D
	public static void InitializeGlobalDialogueCD()
	{
		if (SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD)
		{
			SaveManager.PlayerSaveData.TriggerGlobalNPCDialogueCD = false;
			SaveManager.PlayerSaveData.GlobalNPCDialogueCD = 0;
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x0003D834 File Offset: 0x0003BA34
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

	// Token: 0x06001441 RID: 5185 RVA: 0x0003D870 File Offset: 0x0003BA70
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

	// Token: 0x06001442 RID: 5186 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
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

	// Token: 0x06001443 RID: 5187 RVA: 0x0003D8EC File Offset: 0x0003BAEC
	public static bool CanSpeak(NPCType npcType)
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.AboveGround)
		{
			return false;
		}
		bool flag = false;
		return (SaveManager.PlayerSaveData.GlobalNPCDialogueCD <= 0 || flag) && SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Contains(npcType);
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0003D930 File Offset: 0x0003BB30
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

	// Token: 0x06001445 RID: 5189 RVA: 0x0003DAA0 File Offset: 0x0003BCA0
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

	// Token: 0x06001446 RID: 5190 RVA: 0x0003DAD8 File Offset: 0x0003BCD8
	public static bool IsPriorityDialogue(NPCType npcType)
	{
		return NPCDialogueManager.Instance.IsPriorityDialogue_Internal(npcType);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0003DAE8 File Offset: 0x0003BCE8
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

	// Token: 0x04001414 RID: 5140
	private static NPCDialogueManager m_instance;
}
