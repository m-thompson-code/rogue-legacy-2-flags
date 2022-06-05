using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x020004CC RID: 1228
[Serializable]
public class PlayerSaveData : IVersionUpdateable
{
	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06002774 RID: 10100 RVA: 0x0001634D File Offset: 0x0001454D
	public CachedPlayerData CachedData
	{
		get
		{
			return this.m_cachedData;
		}
	}

	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x06002775 RID: 10101 RVA: 0x000B9B14 File Offset: 0x000B7D14
	public bool IsInHeirloom
	{
		get
		{
			if (PlayerManager.IsInstantiated)
			{
				BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
				if (currentPlayerRoom != null && !currentPlayerRoom.Equals(null))
				{
					return currentPlayerRoom.SpecialRoomType == SpecialRoomType.Heirloom;
				}
			}
			return false;
		}
	}

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06002776 RID: 10102 RVA: 0x00016355 File Offset: 0x00014555
	// (set) Token: 0x06002777 RID: 10103 RVA: 0x0001635D File Offset: 0x0001455D
	public List<HeirloomType> TemporaryHeirloomList { get; private set; } = new List<HeirloomType>();

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x06002778 RID: 10104 RVA: 0x00016366 File Offset: 0x00014566
	// (set) Token: 0x06002779 RID: 10105 RVA: 0x0001636E File Offset: 0x0001456E
	public bool IsInitialized { get; private set; }

	// Token: 0x0600277A RID: 10106 RVA: 0x00016377 File Offset: 0x00014577
	public bool HasTrait(TraitType traitType)
	{
		return this.CurrentCharacter.TraitOne == traitType || this.CurrentCharacter.TraitTwo == traitType;
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000B9B4C File Offset: 0x000B7D4C
	public PlayerSaveData()
	{
		this.Initialize();
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000B9BF0 File Offset: 0x000B7DF0
	public void Initialize()
	{
		if (this.TemporaryHeirloomList == null)
		{
			this.TemporaryHeirloomList = new List<HeirloomType>();
		}
		if (this.m_cachedData == null)
		{
			this.m_cachedData = new CachedPlayerData();
		}
		if (this.PopulatedNPCDialoguesList == null)
		{
			this.PopulatedNPCDialoguesList = new List<NPCType>();
		}
		this.InitializeFlagTable();
		this.InitializeHeirloomTable();
		this.InitializeRelicTable();
		this.InitializeTraitOddsTable();
		this.InitializeTraitSeenTable();
		this.InitializeSpellSeenTable();
		this.InitializeSongTable();
		this.InitializeInsightTable();
		this.InitializeJournalsReadTable();
		this.InitializeMasteryXPTable();
		this.InitializeTeleporterUnlockTable();
		this.InitializeNPCDialogueReadTable();
		this.InitializeBurdenTable();
		this.IsInitialized = true;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000B9C8C File Offset: 0x000B7E8C
	private void InitializeHeirloomTable()
	{
		if (this.HeirloomLevelTable == null)
		{
			this.HeirloomLevelTable = new Dictionary<HeirloomType, int>();
		}
		foreach (HeirloomType heirloomType in HeirloomType_RL.TypeArray)
		{
			if (heirloomType != HeirloomType.None && !this.HeirloomLevelTable.ContainsKey(heirloomType))
			{
				this.HeirloomLevelTable.Add(heirloomType, 0);
			}
		}
		this.HeirloomLevelTable[HeirloomType.Contract] = 1;
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000B9CF0 File Offset: 0x000B7EF0
	private void InitializeRelicTable()
	{
		if (this.RelicObjTable == null)
		{
			this.RelicObjTable = new Dictionary<RelicType, RelicObj>();
		}
		foreach (RelicType relicType in RelicType_RL.TypeArray)
		{
			if (relicType != RelicType.None && !this.RelicObjTable.ContainsKey(relicType))
			{
				this.RelicObjTable.Add(relicType, new RelicObj(relicType));
			}
		}
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000B9D4C File Offset: 0x000B7F4C
	private void InitializeBurdenTable()
	{
		if (this.BurdenObjTable == null)
		{
			this.BurdenObjTable = new Dictionary<BurdenType, BurdenObj>();
		}
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None && !this.BurdenObjTable.ContainsKey(burdenType))
			{
				this.BurdenObjTable.Add(burdenType, new BurdenObj(burdenType));
			}
		}
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000B9DA8 File Offset: 0x000B7FA8
	private void InitializeFlagTable()
	{
		if (this.FlagTable == null)
		{
			this.FlagTable = new Dictionary<PlayerSaveFlag, bool>();
		}
		foreach (object obj in Enum.GetValues(typeof(PlayerSaveFlag)))
		{
			PlayerSaveFlag playerSaveFlag = (PlayerSaveFlag)obj;
			if (playerSaveFlag != PlayerSaveFlag.None && !this.FlagTable.ContainsKey(playerSaveFlag))
			{
				this.FlagTable.Add(playerSaveFlag, false);
			}
		}
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x000B9E34 File Offset: 0x000B8034
	private void InitializeTraitOddsTable()
	{
		if (this.TraitSpawnOddsTable == null)
		{
			this.TraitSpawnOddsTable = new Dictionary<TraitType, SerializableVector3Int>();
		}
		foreach (TraitType traitType in TraitType_RL.TypeArray)
		{
			if (traitType != TraitType.None && !this.TraitSpawnOddsTable.ContainsKey(traitType))
			{
				this.TraitSpawnOddsTable.Add(traitType, new Vector3Int(0, 0, -3));
			}
		}
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000B9E98 File Offset: 0x000B8098
	private void InitializeTraitSeenTable()
	{
		if (this.TraitSeenTable == null)
		{
			this.TraitSeenTable = new Dictionary<TraitType, TraitSeenState>();
		}
		foreach (TraitType traitType in TraitType_RL.TypeArray)
		{
			if (traitType != TraitType.None && !this.TraitSeenTable.ContainsKey(traitType))
			{
				this.TraitSeenTable.Add(traitType, TraitSeenState.NeverSeen);
			}
		}
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000B9EF0 File Offset: 0x000B80F0
	private void InitializeSpellSeenTable()
	{
		if (this.SpellSeenTable == null)
		{
			this.SpellSeenTable = new Dictionary<AbilityType, bool>();
		}
		foreach (AbilityType key in AbilityType_RL.SpellAbilityArray)
		{
			if (!this.SpellSeenTable.ContainsKey(key))
			{
				this.SpellSeenTable.Add(key, false);
			}
		}
		this.SpellSeenTable[AbilityType.FireballSpell] = true;
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000B9F54 File Offset: 0x000B8154
	private void InitializeSongTable()
	{
		if (this.SongsFoundTable == null)
		{
			this.SongsFoundTable = new Dictionary<SongID, FoundState>();
		}
		foreach (object obj in Enum.GetValues(typeof(SongID)))
		{
			SongID songID = (SongID)obj;
			if (songID != SongID.None && !this.SongsFoundTable.ContainsKey(songID))
			{
				this.SongsFoundTable.Add(songID, FoundState.NotFound);
			}
		}
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000B9FE4 File Offset: 0x000B81E4
	private void InitializeInsightTable()
	{
		if (this.InsightStateTable == null)
		{
			this.InsightStateTable = new Dictionary<InsightType, InsightState>();
		}
		foreach (InsightType insightType in InsightType_RL.TypeArray)
		{
			if (insightType != InsightType.None && !this.InsightStateTable.ContainsKey(insightType))
			{
				this.InsightStateTable.Add(insightType, InsightState.Undiscovered);
			}
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000BA03C File Offset: 0x000B823C
	private void InitializeJournalsReadTable()
	{
		if (this.JournalsReadTable == null)
		{
			this.JournalsReadTable = new Dictionary<JournalCategoryType, SerializableVector2Int>();
		}
		foreach (JournalCategoryType journalCategoryType in JournalType_RL.CategoryTypeArray)
		{
			if (journalCategoryType != JournalCategoryType.None && !this.JournalsReadTable.ContainsKey(journalCategoryType))
			{
				this.JournalsReadTable.Add(journalCategoryType, new SerializableVector2Int(0, 0));
			}
		}
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000BA098 File Offset: 0x000B8298
	private void InitializeNPCDialogueReadTable()
	{
		if (this.NPCDialogueTable == null)
		{
			this.NPCDialogueTable = new Dictionary<NPCType, SerializableVector2Int>();
		}
		foreach (NPCType npctype in NPCType_RL.TypeArray)
		{
			if (npctype != NPCType.None && !this.NPCDialogueTable.ContainsKey(npctype))
			{
				this.NPCDialogueTable.Add(npctype, new SerializableVector2Int(-1, 0));
			}
		}
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000BA0F8 File Offset: 0x000B82F8
	private void InitializeMasteryXPTable()
	{
		if (this.MasteryXPTable == null)
		{
			this.MasteryXPTable = new Dictionary<ClassType, int>();
		}
		foreach (ClassType classType in ClassType_RL.TypeArray)
		{
			if (classType != ClassType.None && !this.MasteryXPTable.ContainsKey(classType))
			{
				this.MasteryXPTable.Add(classType, 0);
			}
		}
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x00016397 File Offset: 0x00014597
	private void InitializeTeleporterUnlockTable()
	{
		if (this.TeleporterUnlockTable == null)
		{
			this.TeleporterUnlockTable = new Dictionary<BiomeType, bool>();
		}
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000BA150 File Offset: 0x000B8350
	public bool GetFlag(PlayerSaveFlag flag)
	{
		bool result;
		if (this.FlagTable.TryGetValue(flag, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Flag: " + flag.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000BA190 File Offset: 0x000B8390
	public void SetFlag(PlayerSaveFlag flag, bool value)
	{
		if (this.FlagTable.ContainsKey(flag))
		{
			if (MainMenuWindowController.splitStep == 3 && this.FlagTable[PlayerSaveFlag.CastleBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 4;
			}
			else if (MainMenuWindowController.splitStep == 4 && this.FlagTable[PlayerSaveFlag.CastleBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 5;
			}
			else if (MainMenuWindowController.splitStep == 7 && this.FlagTable[PlayerSaveFlag.ForestBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 8;
			}
			else if (MainMenuWindowController.splitStep == 8 && this.FlagTable[PlayerSaveFlag.ForestBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 9;
			}
			else if (MainMenuWindowController.splitStep == 11 && this.FlagTable[PlayerSaveFlag.BridgeBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 12;
			}
			else if (MainMenuWindowController.splitStep == 12 && this.FlagTable[PlayerSaveFlag.BridgeBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 13;
			}
			else if (MainMenuWindowController.splitStep == 15 && this.FlagTable[PlayerSaveFlag.StudyBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 16;
			}
			else if (MainMenuWindowController.splitStep == 16 && this.FlagTable[PlayerSaveFlag.StudyBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 17;
			}
			else if (MainMenuWindowController.splitStep == 17 && this.FlagTable[PlayerSaveFlag.TowerBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 18;
			}
			else if (MainMenuWindowController.splitStep == 18 && this.FlagTable[PlayerSaveFlag.TowerBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 19;
			}
			else if (MainMenuWindowController.splitStep == 19 && this.FlagTable[PlayerSaveFlag.CaveBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 20;
			}
			else if (MainMenuWindowController.splitStep == 20 && this.FlagTable[PlayerSaveFlag.CaveBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 21;
			}
			else if (MainMenuWindowController.splitStep == 21 && this.FlagTable[PlayerSaveFlag.GardenBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 22;
			}
			else if (MainMenuWindowController.splitStep == 22 && this.FlagTable[PlayerSaveFlag.GardenBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 23;
			}
			else if (MainMenuWindowController.splitStep == 23 && this.FlagTable[PlayerSaveFlag.FinalBoss_FreeHeal_Used])
			{
				MainMenuWindowController.splitStep = 24;
			}
			else if (MainMenuWindowController.splitStep == 24 && this.FlagTable[PlayerSaveFlag.FinalBoss_Defeated_FirstTime])
			{
				MainMenuWindowController.splitStep = 25;
			}
			this.FlagTable[flag] = value;
			return;
		}
		throw new KeyNotFoundException("Cannot SET Flag: " + flag.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x000BA438 File Offset: 0x000B8638
	public void SetAllFlags(bool value)
	{
		foreach (KeyValuePair<PlayerSaveFlag, bool> keyValuePair in this.FlagTable.ToArray<KeyValuePair<PlayerSaveFlag, bool>>())
		{
			this.FlagTable[keyValuePair.Key] = value;
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x000BA47C File Offset: 0x000B867C
	public int GetHeirloomLevel(HeirloomType heirloomType)
	{
		if (heirloomType == HeirloomType.None)
		{
			return 0;
		}
		int num;
		if (!this.HeirloomLevelTable.TryGetValue(heirloomType, out num))
		{
			throw new KeyNotFoundException("Cannot GET level of Heirloom Type: " + heirloomType.ToString() + " because entry not found in Player Save Data.");
		}
		if (num == 0 && !this.TemporaryHeirloomList.IsNativeNull() && this.TemporaryHeirloomList.Contains(heirloomType))
		{
			return 1;
		}
		return num;
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000BA4E4 File Offset: 0x000B86E4
	public void SetHeirloomLevel(HeirloomType heirloomType, int level, bool additive, bool broadcast)
	{
		if (!this.HeirloomLevelTable.ContainsKey(heirloomType))
		{
			throw new KeyNotFoundException("Cannot SET level of Heirloom Type: " + heirloomType.ToString() + " because entry not found in Player Save Data.");
		}
		if (MainMenuWindowController.splitStep <= 26 && heirloomType == HeirloomType.UnlockEarthShift)
		{
			MainMenuWindowController.splitStep = 27;
		}
		else if (MainMenuWindowController.splitStep <= 14 && heirloomType == HeirloomType.UnlockVoidDash)
		{
			MainMenuWindowController.splitStep = 15;
		}
		else if (MainMenuWindowController.splitStep <= 10 && heirloomType == HeirloomType.UnlockDoubleJump)
		{
			MainMenuWindowController.splitStep = 11;
		}
		else if (MainMenuWindowController.splitStep <= 6 && heirloomType == HeirloomType.UnlockBouncableDownstrike)
		{
			MainMenuWindowController.splitStep = 7;
		}
		else if (MainMenuWindowController.splitStep <= 2 && heirloomType == HeirloomType.UnlockAirDash)
		{
			MainMenuWindowController.splitStep = 3;
		}
		int num = this.HeirloomLevelTable[heirloomType];
		int value = level;
		if (additive)
		{
			value = num + level;
		}
		this.HeirloomLevelTable[heirloomType] = Mathf.Clamp(value, 0, 999);
		if (broadcast && num != this.HeirloomLevelTable[heirloomType])
		{
			if (this.m_heirloomChangedEventArgs == null)
			{
				this.m_heirloomChangedEventArgs = new HeirloomLevelChangedEventArgs(heirloomType, num, this.HeirloomLevelTable[heirloomType]);
			}
			else
			{
				this.m_heirloomChangedEventArgs.Initialize(heirloomType, num, this.HeirloomLevelTable[heirloomType]);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HeirloomLevelChanged, null, this.m_heirloomChangedEventArgs);
			return;
		}
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000BA61C File Offset: 0x000B881C
	public void ResetAllHeirlooms()
	{
		foreach (HeirloomType key in HeirloomType_RL.TypeArray)
		{
			if (this.HeirloomLevelTable.ContainsKey(key))
			{
				this.HeirloomLevelTable[key] = 0;
			}
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000BA65C File Offset: 0x000B885C
	public RelicObj GetRelic(RelicType relicType)
	{
		RelicObj result;
		if (this.RelicObjTable.TryGetValue(relicType, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Relic Type: " + relicType.ToString() + " because entry not found in Player Save Data.");
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000BA69C File Offset: 0x000B889C
	public BurdenObj GetBurden(BurdenType burdenType)
	{
		BurdenObj result;
		if (this.BurdenObjTable.TryGetValue(burdenType, out result))
		{
			return result;
		}
		Debug.Log("Cannot GET Burden Type: " + burdenType.ToString() + " because entry not found in Player Save Data.");
		return null;
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000BA6E0 File Offset: 0x000B88E0
	public bool GetSpellSeenState(AbilityType spellType)
	{
		bool result;
		if (this.SpellSeenTable.TryGetValue(spellType, out result))
		{
			return result;
		}
		Debug.Log("Cannot GET SpellSeen state for spell type: " + spellType.ToString() + " because entry not found in Player Save Data.");
		return false;
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000163AC File Offset: 0x000145AC
	public void SetSpellSeenState(AbilityType spellType, bool seenState)
	{
		if (this.SpellSeenTable.ContainsKey(spellType))
		{
			this.SpellSeenTable[spellType] = seenState;
			return;
		}
		Debug.Log("Cannot SET SpellSeen state for spell type: " + spellType.ToString() + " because entry not found in Player Save Data.");
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000BA724 File Offset: 0x000B8924
	public int GetTotalUniqueRelics()
	{
		int num = 0;
		foreach (RelicType relicType in RelicType_RL.TypeArray)
		{
			if (relicType != RelicType.None && this.GetRelic(relicType).Level > 0)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000BA764 File Offset: 0x000B8964
	public int GetTotalRelicLevel()
	{
		int num = 0;
		foreach (RelicType relicType in RelicType_RL.TypeArray)
		{
			if (relicType != RelicType.None)
			{
				num += this.GetRelic(relicType).Level;
			}
		}
		return num;
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000BA7A0 File Offset: 0x000B89A0
	public float GetTotalRelicResolveCost()
	{
		float num = 0f;
		float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
		foreach (RelicType relicType in RelicType_RL.TypeArray)
		{
			if (relicType != RelicType.None)
			{
				RelicObj relic = this.GetRelic(relicType);
				float num2 = RelicLibrary.GetRelicData(relicType).CostAmount;
				if (num2 > 0f)
				{
					num2 -= relicCostMod;
				}
				num += num2 * (float)relic.Level;
			}
		}
		return num;
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000BA80C File Offset: 0x000B8A0C
	public void ResetAllRelics(bool broadcast)
	{
		foreach (KeyValuePair<RelicType, RelicObj> keyValuePair in this.RelicObjTable)
		{
			keyValuePair.Value.SetFloatValue(0f, false, false);
			keyValuePair.Value.SetIntValue(0, false, false);
			keyValuePair.Value.SetLevel(0, false, false);
			keyValuePair.Value.SetRelicMod(RelicModType.None);
			keyValuePair.Value.IsFreeRelic = false;
		}
		if (broadcast)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicsReset, null, null);
		}
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000BA8B0 File Offset: 0x000B8AB0
	public FoundState GetSongFoundState(SongID songType)
	{
		FoundState result;
		if (this.SongsFoundTable.TryGetValue(songType, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET foundState for song: " + songType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000163EB File Offset: 0x000145EB
	public void SetSongFoundState(SongID songType, FoundState foundState)
	{
		if (this.SongsFoundTable.ContainsKey(songType))
		{
			this.SongsFoundTable[songType] = foundState;
			return;
		}
		throw new KeyNotFoundException("Cannot SET foundState for song: " + songType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x000BA8F0 File Offset: 0x000B8AF0
	public InsightState GetInsightState(InsightType insightType)
	{
		InsightState insightState;
		if (!this.InsightStateTable.TryGetValue(insightType, out insightState))
		{
			throw new KeyNotFoundException("Cannot GET InsightState for Insight: " + insightType.ToString() + " not found in Player Save Data.");
		}
		if (InsightState.Undiscovered > insightState)
		{
			return InsightState.Undiscovered;
		}
		return insightState;
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x000BA938 File Offset: 0x000B8B38
	public void SetInsightState(InsightType insightType, InsightState insightState, bool forceOverride)
	{
		if (!forceOverride && insightState <= this.GetInsightState(insightType))
		{
			return;
		}
		if (this.InsightStateTable.ContainsKey(insightType))
		{
			this.InsightStateTable[insightType] = insightState;
			return;
		}
		throw new KeyNotFoundException("Cannot SET InsightState for Insight: " + insightType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x000BA990 File Offset: 0x000B8B90
	public int GetJournalsRead(JournalCategoryType journalCategoryType, JournalType journalType)
	{
		SerializableVector2Int serializableVector2Int;
		if (this.JournalsReadTable.TryGetValue(journalCategoryType, out serializableVector2Int))
		{
			int a;
			if (journalType == JournalType.Journal)
			{
				a = serializableVector2Int.x;
			}
			else
			{
				a = serializableVector2Int.y;
			}
			return Mathf.Min(a, Journal_EV.GetNumJournals(journalCategoryType, journalType));
		}
		throw new KeyNotFoundException("Cannot GET JournalsRead for Journal: " + journalCategoryType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000BA9F0 File Offset: 0x000B8BF0
	public void SetJournalsRead(JournalCategoryType journalCategoryType, JournalType journalType, int value, bool additive, bool forceOverride)
	{
		if (!this.JournalsReadTable.ContainsKey(journalCategoryType))
		{
			throw new KeyNotFoundException("Cannot SET JournalsRead for Journal: " + journalCategoryType.ToString() + " not found in Player Save Data.");
		}
		int journalsRead = this.GetJournalsRead(journalCategoryType, journalType);
		int num = additive ? (journalsRead + 1) : value;
		num = Mathf.Clamp(num, 0, Journal_EV.GetNumJournals(journalCategoryType, journalType));
		if (num > journalsRead || forceOverride)
		{
			SerializableVector2Int value2 = this.JournalsReadTable[journalCategoryType];
			if (journalType == JournalType.Journal)
			{
				value2.x = num;
			}
			else
			{
				value2.y = num;
			}
			this.JournalsReadTable[journalCategoryType] = value2;
			return;
		}
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000BAA88 File Offset: 0x000B8C88
	public int GetNPCDialoguesRead(NPCType npcType)
	{
		SerializableVector2Int serializableVector2Int;
		if (this.NPCDialogueTable.TryGetValue(npcType, out serializableVector2Int))
		{
			return serializableVector2Int.x;
		}
		throw new KeyNotFoundException("Cannot GET NPCDialogueRead for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000BAAD0 File Offset: 0x000B8CD0
	public void SetNPCDialoguesRead(NPCType npcType, int value, bool additive)
	{
		if (this.NPCDialogueTable.ContainsKey(npcType))
		{
			int npcdialoguesRead = this.GetNPCDialoguesRead(npcType);
			int num = additive ? (npcdialoguesRead + value) : value;
			int max = NPCDialogue_EV.NPCDialogueTable.ContainsKey(npcType) ? NPCDialogue_EV.NPCDialogueTable[npcType].Length : 0;
			num = Mathf.Clamp(num, -1, max);
			SerializableVector2Int value2 = this.NPCDialogueTable[npcType];
			value2.x = num;
			this.NPCDialogueTable[npcType] = value2;
			return;
		}
		throw new KeyNotFoundException("Cannot SET NPCDialogueRead for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000BAB68 File Offset: 0x000B8D68
	public int GetNPCDialogueCooldown(NPCType npcType)
	{
		SerializableVector2Int serializableVector2Int;
		if (this.NPCDialogueTable.TryGetValue(npcType, out serializableVector2Int))
		{
			return serializableVector2Int.y;
		}
		throw new KeyNotFoundException("Cannot GET NPCDialogueCD for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000BABB0 File Offset: 0x000B8DB0
	public void SetNPCDialogueCooldown(NPCType npcType, int value, bool additive)
	{
		if (this.NPCDialogueTable.ContainsKey(npcType))
		{
			int npcdialogueCooldown = this.GetNPCDialogueCooldown(npcType);
			int num = additive ? (npcdialogueCooldown + value) : value;
			num = Mathf.Clamp(num, 0, int.MaxValue);
			SerializableVector2Int value2 = this.NPCDialogueTable[npcType];
			value2.y = num;
			this.NPCDialogueTable[npcType] = value2;
			return;
		}
		throw new KeyNotFoundException("Cannot SET NPCDialogueRead for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000BAC30 File Offset: 0x000B8E30
	public int GetClassXP(ClassType classType)
	{
		int b;
		if (this.MasteryXPTable.TryGetValue(classType, out b))
		{
			return Mathf.Max(0, b);
		}
		throw new KeyNotFoundException("Cannot GET ClassXP for Class: " + classType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x000BAC78 File Offset: 0x000B8E78
	public void SetClassXP(ClassType classType, int value, bool additive, bool forceOverride, bool ignoreMaxLevelClamping)
	{
		if (!this.MasteryXPTable.ContainsKey(classType))
		{
			throw new KeyNotFoundException("Cannot SET ClassXP for Class: " + classType.ToString() + " not found in Player Save Data.");
		}
		int classXP = this.GetClassXP(classType);
		int num = additive ? (classXP + value) : value;
		if (num > classXP || forceOverride)
		{
			if (!ignoreMaxLevelClamping)
			{
				num = Mathf.Clamp(num, 0, Mastery_EV.XP_REQUIRED[Mastery_EV.GetMaxMasteryRank()]);
			}
			else
			{
				num = Mathf.Clamp(num, 0, Mastery_EV.XP_REQUIRED[Mastery_EV.XP_REQUIRED.Length - 1]);
			}
			this.MasteryXPTable[classType] = num;
			return;
		}
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x000BAD10 File Offset: 0x000B8F10
	public int GetClassMasteryRank(ClassType classType)
	{
		int num = Mastery_EV.CalculateRankV2(this.GetClassXP(classType));
		if (num < 0)
		{
			return 0;
		}
		return Mathf.Clamp(num, 0, Mastery_EV.GetMaxMasteryRank() + 1);
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000BAD40 File Offset: 0x000B8F40
	public int GetDriftingWorldsMasteryRank(ClassType classType)
	{
		int classXP = this.GetClassXP(classType);
		int result = 0;
		for (int i = 0; i < Mastery_EV.DRIFTING_WORLDS_XP_REQUIRED.Length; i++)
		{
			if (classXP < Mastery_EV.DRIFTING_WORLDS_XP_REQUIRED[i])
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x0001642A File Offset: 0x0001462A
	public bool GetTeleporterIsUnlocked(BiomeType biomeType)
	{
		if (!this.TeleporterUnlockTable.ContainsKey(biomeType))
		{
			this.TeleporterUnlockTable.Add(biomeType, false);
		}
		return this.TeleporterUnlockTable[biomeType];
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x00016453 File Offset: 0x00014653
	public void SetTeleporterIsUnlocked(BiomeType biomeType, bool state)
	{
		if (!this.TeleporterUnlockTable.ContainsKey(biomeType))
		{
			this.TeleporterUnlockTable.Add(biomeType, false);
		}
		this.TeleporterUnlockTable[biomeType] = state;
	}

	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x060027A8 RID: 10152 RVA: 0x0001647D File Offset: 0x0001467D
	public int GoldCollectedIncludingBank
	{
		get
		{
			return this.GoldCollected + this.GoldSaved;
		}
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000BAD78 File Offset: 0x000B8F78
	public void SubtractFromGoldIncludingBank(int value)
	{
		if (value > this.GoldCollectedIncludingBank)
		{
			Debug.Log("<color=red>Attempting to subtract a value higher than the actual gold own.  This should not be possible.</color>");
			return;
		}
		if (value >= this.GoldCollected)
		{
			int num = value - this.GoldCollected;
			this.GoldCollected = 0;
			this.GoldSaved -= num;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldSavedChanged, null, null);
			return;
		}
		this.GoldCollected -= value;
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000BADDC File Offset: 0x000B8FDC
	public string GetActualAvailableGoldString()
	{
		return this.GoldCollectedIncludingBank.ToString();
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x0001648C File Offset: 0x0001468C
	public void UpdateCachedData()
	{
		this.CachedData.UpdateData();
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000BADF8 File Offset: 0x000B8FF8
	public void UpdateVersion()
	{
		this.Initialize();
		if (this.CurrentCharacter.TraitOne == TraitType.Antique_Old)
		{
			this.CurrentCharacter.TraitOne = TraitType.Antique;
		}
		if (this.CurrentCharacter.TraitTwo == TraitType.Antique_Old)
		{
			this.CurrentCharacter.TraitTwo = TraitType.Antique;
		}
		if (this.REVISION_NUMBER != 13)
		{
			if (this.REVISION_NUMBER <= 0)
			{
				this.SetFlag(PlayerSaveFlag.CastleBoss_Defeated, false);
			}
			if (this.REVISION_NUMBER <= 1)
			{
				this.UnequipAllGear();
				int num = (int)((float)this.GoldSpent * 0.7f);
				this.GoldCollected += num;
				this.GoldCollectedBackup = this.GoldCollected;
				this.GoldSpent = 0;
				this.RuneOreCollected += this.RuneOreSpent;
				this.RuneOreSpent = 0;
				this.EquipmentOreCollected += this.EquipmentOreSpent;
				this.EquipmentOreSpent = 0;
				this.SetFlag(PlayerSaveFlag.PlayFarShoresWarning, true);
			}
			if (this.REVISION_NUMBER <= 2)
			{
				int goldSpentOnSkills = this.GoldSpentOnSkills;
				this.GoldCollected += goldSpentOnSkills;
				this.GoldCollectedBackup = this.GoldCollected;
				this.GoldSpentOnSkills = 0;
				this.UnequipAllGear();
				this.SetFlag(PlayerSaveFlag.PlayArcaneHallowsWarning, true);
			}
			if (this.REVISION_NUMBER <= 3)
			{
				this.SetFlag(PlayerSaveFlag.CastleBoss_Defeated_FirstTime, this.GetFlag(PlayerSaveFlag.CastleBoss_Defeated));
				this.SetFlag(PlayerSaveFlag.BridgeBoss_Defeated_FirstTime, this.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated));
				this.SetFlag(PlayerSaveFlag.ForestBoss_Defeated_FirstTime, this.GetFlag(PlayerSaveFlag.ForestBoss_Defeated));
				this.SetFlag(PlayerSaveFlag.StudyBoss_Defeated_FirstTime, this.GetFlag(PlayerSaveFlag.StudyBoss_Defeated));
				this.UnequipAllGear();
				int num2 = Mathf.RoundToInt((float)this.GoldSpentOnSkills);
				this.GoldCollected += num2;
				this.GoldCollectedBackup = this.GoldCollected;
				this.GoldSpentOnSkills = 0;
				foreach (ClassType classType in ClassType_RL.TypeArray)
				{
					if (classType != ClassType.None)
					{
						int classXP = this.GetClassXP(classType);
						this.SetClassXP(classType, classXP * 5, false, true, true);
					}
				}
				this.SetFlag(PlayerSaveFlag.PlayDriftingWorldsWarning, true);
			}
			if (this.REVISION_NUMBER <= 4)
			{
				if (this.GetFlag(PlayerSaveFlag.CastleBoss_Defeated_FirstTime))
				{
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(GameModeType.Regular, BossID.Castle_Boss, 0, false);
				}
				if (this.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated_FirstTime))
				{
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(GameModeType.Regular, BossID.Bridge_Boss, 0, false);
				}
				if (this.GetFlag(PlayerSaveFlag.ForestBoss_Defeated_FirstTime))
				{
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(GameModeType.Regular, BossID.Forest_Boss, 0, false);
				}
				if (this.GetFlag(PlayerSaveFlag.StudyBoss_Defeated_FirstTime))
				{
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(GameModeType.Regular, BossID.Study_Boss, 0, false);
				}
				if (this.GetFlag(PlayerSaveFlag.TowerBoss_Defeated_FirstTime))
				{
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(GameModeType.Regular, BossID.Tower_Boss, 0, false);
				}
				if (this.NewGamePlusLevel > 0)
				{
					foreach (BiomeType biomeType in this.TeleporterUnlockTable.Keys.ToList<BiomeType>())
					{
						this.SetTeleporterIsUnlocked(biomeType, false);
					}
				}
			}
			if (this.REVISION_NUMBER <= 5)
			{
				this.UnequipAllGear();
				this.SetFlag(PlayerSaveFlag.PlayPizzaMundiWarning, true);
			}
			if (this.REVISION_NUMBER <= 7)
			{
				this.GoldAcceptedByCharon = this.GoldGivenToCharon;
				if (!this.GetFlag(PlayerSaveFlag.TowerBoss_Defeated_FirstTime))
				{
					this.HighestNGPlusBeaten = -1;
				}
				this.Assist_EnemyHealthMod = 1f;
				this.Assist_EnemyDamageMod = 1f;
				foreach (ClassType classType2 in ClassType_RL.TypeArray)
				{
					if (classType2 != ClassType.None)
					{
						int num3 = this.GetClassXP(classType2);
						int driftingWorldsMasteryRank = this.GetDriftingWorldsMasteryRank(classType2);
						num3 = Mathf.RoundToInt((float)num3 * (1f + (float)driftingWorldsMasteryRank * 0.0675f));
						this.SetClassXP(classType2, num3, false, true, true);
					}
				}
			}
			if (this.REVISION_NUMBER <= 8)
			{
				foreach (JournalCategoryType journalCategoryType in JournalType_RL.CategoryTypeArray)
				{
					if (journalCategoryType != JournalCategoryType.None)
					{
						int journalsRead = this.GetJournalsRead(journalCategoryType, JournalType.Journal);
						int journalsRead2 = this.GetJournalsRead(journalCategoryType, JournalType.MemoryFragment);
						if (journalsRead > 0 || journalsRead2 > 0)
						{
							this.SetFlag(PlayerSaveFlag.JournalReadOnce, true);
							break;
						}
					}
				}
			}
			if (this.REVISION_NUMBER <= 9)
			{
				this.SetFlag(PlayerSaveFlag.PlayDragonsVowWarning, true);
				this.UnequipAllGear();
				int num4 = Mathf.RoundToInt((float)this.GoldSpentOnSkills);
				this.GoldCollected += num4;
				this.GoldSpentOnSkills = 0;
				if (this.RelicObjTable.ContainsKey((RelicType)460))
				{
					this.RelicObjTable.Remove((RelicType)460);
				}
				if (this.Assist_EnemyDamageMod == 0f)
				{
					this.Assist_EnemyDamageMod = 1f;
				}
				if (this.Assist_EnemyHealthMod < 0.5f)
				{
					this.Assist_EnemyHealthMod = 0.5f;
				}
				this.GoldCollectedBackup = this.GoldCollected;
			}
			if (this.REVISION_NUMBER <= 10 && this.Assist_AimTimeSlow == 0f)
			{
				this.Assist_AimTimeSlow = 1f;
			}
			if (this.REVISION_NUMBER <= 11 && this.HeirloomLevelTable.ContainsKey(HeirloomType.Reliquary) && this.HeirloomLevelTable[HeirloomType.Reliquary] > 0)
			{
				this.HeirloomLevelTable[HeirloomType.Reliquary] = 0;
			}
			if (this.REVISION_NUMBER <= 12 && this.Assist_BurdenRequirementsMod == 0f)
			{
				this.Assist_BurdenRequirementsMod = 1f;
			}
		}
		this.REVISION_NUMBER = 13;
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x00016499 File Offset: 0x00014699
	private void UnequipAllGear()
	{
		this.CurrentCharacter.EdgeEquipmentType = EquipmentType.None;
		this.CurrentCharacter.HeadEquipmentType = EquipmentType.None;
		this.CurrentCharacter.ChestEquipmentType = EquipmentType.None;
		this.CurrentCharacter.CapeEquipmentType = EquipmentType.None;
		this.CurrentCharacter.TrinketEquipmentType = EquipmentType.None;
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000BB338 File Offset: 0x000B9538
	public void ResetForNewGamePlus(bool resetRelics)
	{
		if (resetRelics)
		{
			RelicType relicType = RelicType.None;
			RelicType relicType2 = RelicType.None;
			if (SaveManager.PlayerSaveData.CurrentCharacter.TraitOne == TraitType.Antique)
			{
				relicType = SaveManager.PlayerSaveData.CurrentCharacter.AntiqueOneOwned;
			}
			if (SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo == TraitType.Antique)
			{
				relicType2 = SaveManager.PlayerSaveData.CurrentCharacter.AntiqueTwoOwned;
			}
			foreach (KeyValuePair<RelicType, RelicObj> keyValuePair in this.RelicObjTable)
			{
				if (keyValuePair.Key != RelicType.ExtraLife_Unity && keyValuePair.Key != RelicType.ExtraLife_UnityUsed)
				{
					keyValuePair.Value.SetFloatValue(0f, false, false);
					keyValuePair.Value.SetIntValue(0, false, false);
					keyValuePair.Value.SetRelicMod(RelicModType.None);
					if (keyValuePair.Key == relicType || keyValuePair.Key == relicType2)
					{
						keyValuePair.Value.SetLevel(1, false, false);
					}
					else
					{
						keyValuePair.Value.SetLevel(0, false, false);
						keyValuePair.Value.IsFreeRelic = false;
					}
				}
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicsReset, null, null);
		}
		this.SetFlag(PlayerSaveFlag.CastleBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.BridgeBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.ForestBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.StudyBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.StudyMiniboss_SpearKnight_Defeated, false);
		this.SetFlag(PlayerSaveFlag.StudyMiniboss_SwordKnight_Defeated, false);
		this.SetFlag(PlayerSaveFlag.TowerBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.CaveBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated, false);
		this.SetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated, false);
		this.SetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened, false);
		this.SetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened, false);
		this.SetFlag(PlayerSaveFlag.GardenBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.FinalBoss_Defeated, false);
		this.SetFlag(PlayerSaveFlag.BlackChest_Weapon_Opened, false);
		this.SetFlag(PlayerSaveFlag.BlackChest_Helm_Opened, false);
		this.SetFlag(PlayerSaveFlag.BlackChest_Chest_Opened, false);
		this.SetFlag(PlayerSaveFlag.BlackChest_Cape_Opened, false);
		this.SetFlag(PlayerSaveFlag.BlackChest_Trinket_Opened, false);
		this.SetInsightState(InsightType.CastleBoss_DoorOpened, InsightState.Undiscovered, true);
		this.SetInsightState(InsightType.ForestBoss_DoorOpened, InsightState.Undiscovered, true);
		this.SetInsightState(InsightType.BridgeBoss_GateRaised, InsightState.Undiscovered, true);
		this.SetInsightState(InsightType.StudyBoss_DoorOpened, InsightState.Undiscovered, true);
		this.SetInsightState(InsightType.CaveBoss_DoorOpened, InsightState.Undiscovered, true);
		this.SetFlag(PlayerSaveFlag.DragonDialogue_Intro, false);
		this.SetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen, false);
		this.SetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal, false);
		this.SetFlag(PlayerSaveFlag.DragonDialogue_Sleep, false);
		this.SetFlag(PlayerSaveFlag.CaveTuningForkTriggered, false);
		this.SetFlag(PlayerSaveFlag.Johan_First_Death_Intro, false);
		this.SetFlag(PlayerSaveFlag.Johan_Getting_Memory_Heirloom, false);
		this.SetFlag(PlayerSaveFlag.Johan_Entering_Secret_Tower, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Beating_Castle_Boss, false);
		this.SetFlag(PlayerSaveFlag.Johan_Finding_On_Bridge, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Beating_Bridge_Boss, false);
		this.SetFlag(PlayerSaveFlag.Johan_Sitting_At_Far_Shore, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Beating_Forest_Boss, false);
		this.SetFlag(PlayerSaveFlag.Johan_Reaching_Sun_Tower_Top, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Beating_Study_Boss, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Beating_Tower_Boss, false);
		this.SetFlag(PlayerSaveFlag.Johan_After_Getting_Heirloom_Lantern, false);
		this.SetFlag(PlayerSaveFlag.SeenParade, false);
		this.SetFlag(PlayerSaveFlag.SeenNGConfirmWindow, false);
		this.SetFlag(PlayerSaveFlag.SeenTrueEnding, false);
		this.SetHeirloomLevel(HeirloomType.Fruit, 0, false, false);
		this.CastleLockState = CastleLockState.NotLocked;
		this.TimesCastleLocked = 0;
		this.TimesRolledRelic = 0;
		this.InCastle = false;
		this.InHubTown = false;
		this.SpokenToFinalBoss = false;
		this.SpokenToTraitor = false;
		this.EndingSpawnRoom = EndingSpawnRoomType.None;
		foreach (BiomeType biomeType in this.TeleporterUnlockTable.Keys.ToList<BiomeType>())
		{
			this.SetTeleporterIsUnlocked(biomeType, false);
		}
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000BB71C File Offset: 0x000B991C
	public void GiveMoneyToCharon(MonoBehaviour caller)
	{
		if (this.GoldCollected > 0)
		{
			int num = this.GoldCollected;
			if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Gold_Saved_Unlock) > 0)
			{
				float num2 = 0.1f;
				num2 += SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Saved_Amount_Saved).CurrentStatGain;
				int num3 = 1000;
				num3 += (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Saved_Cap_Up).CurrentStatGain;
				this.GoldSaved = Mathf.Clamp(this.GoldSaved, 0, num3);
				int num4 = num3 - this.GoldSaved;
				if (num4 > 0)
				{
					int num5 = Mathf.Clamp(Mathf.RoundToInt((float)num * num2), 0, num4);
					this.GoldSaved += num5;
					this.GoldSaved = Mathf.Clamp(this.GoldSaved, 0, num3);
					num -= num5;
					if (num5 > 0)
					{
						if (this.m_goldSavedEventArgs == null)
						{
							this.m_goldSavedEventArgs = new GoldChangedEventArgs(this.GoldSaved - num5, this.GoldSaved);
						}
						else
						{
							this.m_goldSavedEventArgs.Initialize(this.GoldSaved - num5, this.GoldSaved);
						}
						Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldSavedChanged, caller, this.m_goldSavedEventArgs);
					}
				}
			}
			PlayerController playerController = PlayerManager.GetPlayerController();
			Vector2 absPos = playerController.Midpoint;
			absPos.y += playerController.CollisionBounds.height / 2f;
			string text = string.Format(LocalizationManager.GetString("LOC_ID_GOLD_UI_NEGATIVE_GOLD_POPUP_1", false, false), num);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerHit, text, absPos, null, TextAlignmentOptions.Center);
			this.GoldGivenToCharon += num;
			this.GoldCollected = 0;
			if (this.GoldAcceptedByCharon < SkillTree_EV.GetMaxCharonAcceptableGold())
			{
				this.GoldAcceptedByCharon = Mathf.Min(this.GoldAcceptedByCharon + num, SkillTree_EV.GetMaxCharonAcceptableGold());
			}
			if (this.m_goldChangedEventArgs == null)
			{
				this.m_goldChangedEventArgs = new GoldChangedEventArgs(num, 0);
			}
			else
			{
				this.m_goldChangedEventArgs.Initialize(num, 0);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, caller, this.m_goldChangedEventArgs);
		}
	}

	// Token: 0x0400227E RID: 8830
	public int REVISION_NUMBER = 13;

	// Token: 0x0400227F RID: 8831
	public int FILE_NUMBER;

	// Token: 0x04002280 RID: 8832
	public string DateTimeString;

	// Token: 0x04002281 RID: 8833
	public GameModeType GameModeType;

	// Token: 0x04002282 RID: 8834
	public CharacterData CurrentCharacter = new CharacterData();

	// Token: 0x04002283 RID: 8835
	public int MasterSeed = -1;

	// Token: 0x04002284 RID: 8836
	public int PreviousMasterSeed = -1;

	// Token: 0x04002285 RID: 8837
	public int GoldCollected;

	// Token: 0x04002286 RID: 8838
	public int GoldCollectedBackup;

	// Token: 0x04002287 RID: 8839
	public int GoldSaved;

	// Token: 0x04002288 RID: 8840
	public int GoldGivenToCharon;

	// Token: 0x04002289 RID: 8841
	public int GoldAcceptedByCharon;

	// Token: 0x0400228A RID: 8842
	public int EquipmentOreCollected = 1500;

	// Token: 0x0400228B RID: 8843
	public int RuneOreCollected = 1500;

	// Token: 0x0400228C RID: 8844
	public int GoldSpent;

	// Token: 0x0400228D RID: 8845
	public int GoldSpentOnEquipment;

	// Token: 0x0400228E RID: 8846
	public int GoldSpentOnRunes;

	// Token: 0x0400228F RID: 8847
	public int GoldSpentOnSkills;

	// Token: 0x04002290 RID: 8848
	public int EquipmentOreSpent;

	// Token: 0x04002291 RID: 8849
	public int RuneOreSpent;

	// Token: 0x04002292 RID: 8850
	public int NumAncestors;

	// Token: 0x04002293 RID: 8851
	public int NewGamePlusLevel;

	// Token: 0x04002294 RID: 8852
	public uint SecondsPlayed;

	// Token: 0x04002295 RID: 8853
	public int TimesDied;

	// Token: 0x04002296 RID: 8854
	public int EnemiesKilled;

	// Token: 0x04002297 RID: 8855
	public byte TimesCastleLocked;

	// Token: 0x04002298 RID: 8856
	public byte TimesRolledRelic;

	// Token: 0x04002299 RID: 8857
	public int HighestNGPlusBeaten = -1;

	// Token: 0x0400229A RID: 8858
	public bool SpokenToTraitor;

	// Token: 0x0400229B RID: 8859
	public int TimesBeatenTraitor;

	// Token: 0x0400229C RID: 8860
	public byte TraitorPreFightRepeatDialogueIndex;

	// Token: 0x0400229D RID: 8861
	public byte TraitorPostFightRepeatDialogueIndex;

	// Token: 0x0400229E RID: 8862
	public bool SpokenToFinalBoss;

	// Token: 0x0400229F RID: 8863
	public bool IsDead;

	// Token: 0x040022A0 RID: 8864
	public bool InHubTown;

	// Token: 0x040022A1 RID: 8865
	public bool InCastle;

	// Token: 0x040022A2 RID: 8866
	public bool FirstTimeGoldReceived;

	// Token: 0x040022A3 RID: 8867
	public bool GoldilocksReceived;

	// Token: 0x040022A4 RID: 8868
	public CastleLockState CastleLockState;

	// Token: 0x040022A5 RID: 8869
	public bool HasStartedGame;

	// Token: 0x040022A6 RID: 8870
	public bool UnlockAllTraitorMemories;

	// Token: 0x040022A7 RID: 8871
	public int TimesDiedSinceHestia;

	// Token: 0x040022A8 RID: 8872
	public byte HestiaCutsceneDisplayCount;

	// Token: 0x040022A9 RID: 8873
	public byte TreeCutsceneDisplayCount;

	// Token: 0x040022AA RID: 8874
	public EndingSpawnRoomType EndingSpawnRoom;

	// Token: 0x040022AB RID: 8875
	public float TemporaryMaxHealthMods;

	// Token: 0x040022AC RID: 8876
	public bool EnableHouseRules;

	// Token: 0x040022AD RID: 8877
	public float Assist_EnemyHealthMod = 1f;

	// Token: 0x040022AE RID: 8878
	public float Assist_EnemyDamageMod = 1f;

	// Token: 0x040022AF RID: 8879
	public float Assist_AimTimeSlow = 1f;

	// Token: 0x040022B0 RID: 8880
	public bool Assist_EnableFlightToggle;

	// Token: 0x040022B1 RID: 8881
	public bool Assist_DisableEnemyContactDamage;

	// Token: 0x040022B2 RID: 8882
	public bool Assist_DisableTraits;

	// Token: 0x040022B3 RID: 8883
	public bool Assist_EnableDifficultyDisplay;

	// Token: 0x040022B4 RID: 8884
	public float Assist_BurdenRequirementsMod = 1f;

	// Token: 0x040022B5 RID: 8885
	public bool DisableAchievementUnlocks;

	// Token: 0x040022B6 RID: 8886
	private CachedPlayerData m_cachedData;

	// Token: 0x040022B7 RID: 8887
	public int LineageSeed = -1;

	// Token: 0x040022B8 RID: 8888
	public int TimesRolledLineage;

	// Token: 0x040022B9 RID: 8889
	public Dictionary<PlayerSaveFlag, bool> FlagTable;

	// Token: 0x040022BA RID: 8890
	public Dictionary<RelicType, RelicObj> RelicObjTable;

	// Token: 0x040022BB RID: 8891
	public Dictionary<BurdenType, BurdenObj> BurdenObjTable;

	// Token: 0x040022BC RID: 8892
	public Dictionary<BiomeType, bool> TeleporterUnlockTable;

	// Token: 0x040022BD RID: 8893
	public SerializableVector2Int TeleporterUnlockDialogueIndex = new SerializableVector2Int(-1, -1);

	// Token: 0x040022BE RID: 8894
	public Dictionary<TraitType, SerializableVector3Int> TraitSpawnOddsTable;

	// Token: 0x040022BF RID: 8895
	public Dictionary<TraitType, TraitSeenState> TraitSeenTable;

	// Token: 0x040022C0 RID: 8896
	public Dictionary<AbilityType, bool> SpellSeenTable;

	// Token: 0x040022C1 RID: 8897
	public Dictionary<HeirloomType, int> HeirloomLevelTable;

	// Token: 0x040022C2 RID: 8898
	public Dictionary<SongID, FoundState> SongsFoundTable;

	// Token: 0x040022C3 RID: 8899
	public Dictionary<InsightType, InsightState> InsightStateTable;

	// Token: 0x040022C4 RID: 8900
	public Dictionary<JournalCategoryType, SerializableVector2Int> JournalsReadTable;

	// Token: 0x040022C5 RID: 8901
	public Dictionary<NPCType, SerializableVector2Int> NPCDialogueTable;

	// Token: 0x040022C6 RID: 8902
	public List<NPCType> PopulatedNPCDialoguesList;

	// Token: 0x040022C7 RID: 8903
	public int GlobalNPCDialogueCD;

	// Token: 0x040022C8 RID: 8904
	public bool TriggerGlobalNPCDialogueCD;

	// Token: 0x040022C9 RID: 8905
	public Dictionary<ClassType, int> MasteryXPTable;

	// Token: 0x040022CA RID: 8906
	public int RunAccumulatedXP;

	// Token: 0x040022CB RID: 8907
	[NonSerialized]
	private HeirloomLevelChangedEventArgs m_heirloomChangedEventArgs;

	// Token: 0x040022CC RID: 8908
	[NonSerialized]
	private GoldChangedEventArgs m_goldChangedEventArgs;

	// Token: 0x040022CD RID: 8909
	[NonSerialized]
	private GoldChangedEventArgs m_goldSavedEventArgs;
}
