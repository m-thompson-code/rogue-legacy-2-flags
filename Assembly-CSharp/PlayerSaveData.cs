using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x020002D7 RID: 727
[Serializable]
public class PlayerSaveData : IVersionUpdateable
{
	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06001CAA RID: 7338 RVA: 0x0005D7CB File Offset: 0x0005B9CB
	public CachedPlayerData CachedData
	{
		get
		{
			return this.m_cachedData;
		}
	}

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06001CAB RID: 7339 RVA: 0x0005D7D4 File Offset: 0x0005B9D4
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

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06001CAC RID: 7340 RVA: 0x0005D80C File Offset: 0x0005BA0C
	// (set) Token: 0x06001CAD RID: 7341 RVA: 0x0005D814 File Offset: 0x0005BA14
	public List<HeirloomType> TemporaryHeirloomList { get; private set; } = new List<HeirloomType>();

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06001CAE RID: 7342 RVA: 0x0005D81D File Offset: 0x0005BA1D
	// (set) Token: 0x06001CAF RID: 7343 RVA: 0x0005D825 File Offset: 0x0005BA25
	public bool IsInitialized { get; private set; }

	// Token: 0x06001CB0 RID: 7344 RVA: 0x0005D82E File Offset: 0x0005BA2E
	public bool HasTrait(TraitType traitType)
	{
		return this.CurrentCharacter.TraitOne == traitType || this.CurrentCharacter.TraitTwo == traitType;
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x0005D850 File Offset: 0x0005BA50
	public PlayerSaveData()
	{
		this.Initialize();
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x0005D8F4 File Offset: 0x0005BAF4
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

	// Token: 0x06001CB3 RID: 7347 RVA: 0x0005D990 File Offset: 0x0005BB90
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

	// Token: 0x06001CB4 RID: 7348 RVA: 0x0005D9F4 File Offset: 0x0005BBF4
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

	// Token: 0x06001CB5 RID: 7349 RVA: 0x0005DA50 File Offset: 0x0005BC50
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

	// Token: 0x06001CB6 RID: 7350 RVA: 0x0005DAAC File Offset: 0x0005BCAC
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

	// Token: 0x06001CB7 RID: 7351 RVA: 0x0005DB38 File Offset: 0x0005BD38
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

	// Token: 0x06001CB8 RID: 7352 RVA: 0x0005DB9C File Offset: 0x0005BD9C
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

	// Token: 0x06001CB9 RID: 7353 RVA: 0x0005DBF4 File Offset: 0x0005BDF4
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

	// Token: 0x06001CBA RID: 7354 RVA: 0x0005DC58 File Offset: 0x0005BE58
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

	// Token: 0x06001CBB RID: 7355 RVA: 0x0005DCE8 File Offset: 0x0005BEE8
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

	// Token: 0x06001CBC RID: 7356 RVA: 0x0005DD40 File Offset: 0x0005BF40
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

	// Token: 0x06001CBD RID: 7357 RVA: 0x0005DD9C File Offset: 0x0005BF9C
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

	// Token: 0x06001CBE RID: 7358 RVA: 0x0005DDFC File Offset: 0x0005BFFC
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

	// Token: 0x06001CBF RID: 7359 RVA: 0x0005DE52 File Offset: 0x0005C052
	private void InitializeTeleporterUnlockTable()
	{
		if (this.TeleporterUnlockTable == null)
		{
			this.TeleporterUnlockTable = new Dictionary<BiomeType, bool>();
		}
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x0005DE68 File Offset: 0x0005C068
	public bool GetFlag(PlayerSaveFlag flag)
	{
		bool result;
		if (this.FlagTable.TryGetValue(flag, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Flag: " + flag.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x0005DEA8 File Offset: 0x0005C0A8
	public void SetFlag(PlayerSaveFlag flag, bool value)
	{
		if (this.FlagTable.ContainsKey(flag))
		{
			this.FlagTable[flag] = value;
			return;
		}
		throw new KeyNotFoundException("Cannot SET Flag: " + flag.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x0005DEE8 File Offset: 0x0005C0E8
	public void SetAllFlags(bool value)
	{
		foreach (KeyValuePair<PlayerSaveFlag, bool> keyValuePair in this.FlagTable.ToArray<KeyValuePair<PlayerSaveFlag, bool>>())
		{
			this.FlagTable[keyValuePair.Key] = value;
		}
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x0005DF2C File Offset: 0x0005C12C
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

	// Token: 0x06001CC4 RID: 7364 RVA: 0x0005DF94 File Offset: 0x0005C194
	public void SetHeirloomLevel(HeirloomType heirloomType, int level, bool additive, bool broadcast)
	{
		if (!this.HeirloomLevelTable.ContainsKey(heirloomType))
		{
			throw new KeyNotFoundException("Cannot SET level of Heirloom Type: " + heirloomType.ToString() + " because entry not found in Player Save Data.");
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

	// Token: 0x06001CC5 RID: 7365 RVA: 0x0005E060 File Offset: 0x0005C260
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

	// Token: 0x06001CC6 RID: 7366 RVA: 0x0005E0A0 File Offset: 0x0005C2A0
	public RelicObj GetRelic(RelicType relicType)
	{
		RelicObj result;
		if (this.RelicObjTable.TryGetValue(relicType, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Relic Type: " + relicType.ToString() + " because entry not found in Player Save Data.");
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x0005E0E0 File Offset: 0x0005C2E0
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

	// Token: 0x06001CC8 RID: 7368 RVA: 0x0005E124 File Offset: 0x0005C324
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

	// Token: 0x06001CC9 RID: 7369 RVA: 0x0005E165 File Offset: 0x0005C365
	public void SetSpellSeenState(AbilityType spellType, bool seenState)
	{
		if (this.SpellSeenTable.ContainsKey(spellType))
		{
			this.SpellSeenTable[spellType] = seenState;
			return;
		}
		Debug.Log("Cannot SET SpellSeen state for spell type: " + spellType.ToString() + " because entry not found in Player Save Data.");
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x0005E1A4 File Offset: 0x0005C3A4
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

	// Token: 0x06001CCB RID: 7371 RVA: 0x0005E1E4 File Offset: 0x0005C3E4
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

	// Token: 0x06001CCC RID: 7372 RVA: 0x0005E220 File Offset: 0x0005C420
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

	// Token: 0x06001CCD RID: 7373 RVA: 0x0005E28C File Offset: 0x0005C48C
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

	// Token: 0x06001CCE RID: 7374 RVA: 0x0005E330 File Offset: 0x0005C530
	public FoundState GetSongFoundState(SongID songType)
	{
		FoundState result;
		if (this.SongsFoundTable.TryGetValue(songType, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET foundState for song: " + songType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x0005E370 File Offset: 0x0005C570
	public void SetSongFoundState(SongID songType, FoundState foundState)
	{
		if (this.SongsFoundTable.ContainsKey(songType))
		{
			this.SongsFoundTable[songType] = foundState;
			return;
		}
		throw new KeyNotFoundException("Cannot SET foundState for song: " + songType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x0005E3B0 File Offset: 0x0005C5B0
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

	// Token: 0x06001CD1 RID: 7377 RVA: 0x0005E3F8 File Offset: 0x0005C5F8
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

	// Token: 0x06001CD2 RID: 7378 RVA: 0x0005E450 File Offset: 0x0005C650
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

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0005E4B0 File Offset: 0x0005C6B0
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

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0005E548 File Offset: 0x0005C748
	public int GetNPCDialoguesRead(NPCType npcType)
	{
		SerializableVector2Int serializableVector2Int;
		if (this.NPCDialogueTable.TryGetValue(npcType, out serializableVector2Int))
		{
			return serializableVector2Int.x;
		}
		throw new KeyNotFoundException("Cannot GET NPCDialogueRead for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x0005E590 File Offset: 0x0005C790
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

	// Token: 0x06001CD6 RID: 7382 RVA: 0x0005E628 File Offset: 0x0005C828
	public int GetNPCDialogueCooldown(NPCType npcType)
	{
		SerializableVector2Int serializableVector2Int;
		if (this.NPCDialogueTable.TryGetValue(npcType, out serializableVector2Int))
		{
			return serializableVector2Int.y;
		}
		throw new KeyNotFoundException("Cannot GET NPCDialogueCD for type: " + npcType.ToString() + ". Not found in Player Save Data.");
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x0005E670 File Offset: 0x0005C870
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

	// Token: 0x06001CD8 RID: 7384 RVA: 0x0005E6F0 File Offset: 0x0005C8F0
	public int GetClassXP(ClassType classType)
	{
		int b;
		if (this.MasteryXPTable.TryGetValue(classType, out b))
		{
			return Mathf.Max(0, b);
		}
		throw new KeyNotFoundException("Cannot GET ClassXP for Class: " + classType.ToString() + " not found in Player Save Data.");
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x0005E738 File Offset: 0x0005C938
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

	// Token: 0x06001CDA RID: 7386 RVA: 0x0005E7D0 File Offset: 0x0005C9D0
	public int GetClassMasteryRank(ClassType classType)
	{
		int num = Mastery_EV.CalculateRankV2(this.GetClassXP(classType));
		if (num < 0)
		{
			return 0;
		}
		return Mathf.Clamp(num, 0, Mastery_EV.GetMaxMasteryRank() + 1);
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x0005E800 File Offset: 0x0005CA00
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

	// Token: 0x06001CDC RID: 7388 RVA: 0x0005E838 File Offset: 0x0005CA38
	public bool GetTeleporterIsUnlocked(BiomeType biomeType)
	{
		if (!this.TeleporterUnlockTable.ContainsKey(biomeType))
		{
			this.TeleporterUnlockTable.Add(biomeType, false);
		}
		return this.TeleporterUnlockTable[biomeType];
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x0005E861 File Offset: 0x0005CA61
	public void SetTeleporterIsUnlocked(BiomeType biomeType, bool state)
	{
		if (!this.TeleporterUnlockTable.ContainsKey(biomeType))
		{
			this.TeleporterUnlockTable.Add(biomeType, false);
		}
		this.TeleporterUnlockTable[biomeType] = state;
	}

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06001CDE RID: 7390 RVA: 0x0005E88B File Offset: 0x0005CA8B
	public int GoldCollectedIncludingBank
	{
		get
		{
			return this.GoldCollected + this.GoldSaved;
		}
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0005E89C File Offset: 0x0005CA9C
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

	// Token: 0x06001CE0 RID: 7392 RVA: 0x0005E900 File Offset: 0x0005CB00
	public string GetActualAvailableGoldString()
	{
		return this.GoldCollectedIncludingBank.ToString();
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0005E91B File Offset: 0x0005CB1B
	public void UpdateCachedData()
	{
		this.CachedData.UpdateData();
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0005E928 File Offset: 0x0005CB28
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

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0005EE68 File Offset: 0x0005D068
	private void UnequipAllGear()
	{
		this.CurrentCharacter.EdgeEquipmentType = EquipmentType.None;
		this.CurrentCharacter.HeadEquipmentType = EquipmentType.None;
		this.CurrentCharacter.ChestEquipmentType = EquipmentType.None;
		this.CurrentCharacter.CapeEquipmentType = EquipmentType.None;
		this.CurrentCharacter.TrinketEquipmentType = EquipmentType.None;
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x0005EEA8 File Offset: 0x0005D0A8
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

	// Token: 0x06001CE5 RID: 7397 RVA: 0x0005F28C File Offset: 0x0005D48C
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

	// Token: 0x04001A77 RID: 6775
	public int REVISION_NUMBER = 13;

	// Token: 0x04001A78 RID: 6776
	public int FILE_NUMBER;

	// Token: 0x04001A79 RID: 6777
	public string DateTimeString;

	// Token: 0x04001A7A RID: 6778
	public GameModeType GameModeType;

	// Token: 0x04001A7B RID: 6779
	public CharacterData CurrentCharacter = new CharacterData();

	// Token: 0x04001A7C RID: 6780
	public int MasterSeed = -1;

	// Token: 0x04001A7D RID: 6781
	public int PreviousMasterSeed = -1;

	// Token: 0x04001A7E RID: 6782
	public int GoldCollected;

	// Token: 0x04001A7F RID: 6783
	public int GoldCollectedBackup;

	// Token: 0x04001A80 RID: 6784
	public int GoldSaved;

	// Token: 0x04001A81 RID: 6785
	public int GoldGivenToCharon;

	// Token: 0x04001A82 RID: 6786
	public int GoldAcceptedByCharon;

	// Token: 0x04001A83 RID: 6787
	public int EquipmentOreCollected = 1500;

	// Token: 0x04001A84 RID: 6788
	public int RuneOreCollected = 1500;

	// Token: 0x04001A85 RID: 6789
	public int GoldSpent;

	// Token: 0x04001A86 RID: 6790
	public int GoldSpentOnEquipment;

	// Token: 0x04001A87 RID: 6791
	public int GoldSpentOnRunes;

	// Token: 0x04001A88 RID: 6792
	public int GoldSpentOnSkills;

	// Token: 0x04001A89 RID: 6793
	public int EquipmentOreSpent;

	// Token: 0x04001A8A RID: 6794
	public int RuneOreSpent;

	// Token: 0x04001A8B RID: 6795
	public int NumAncestors;

	// Token: 0x04001A8C RID: 6796
	public int NewGamePlusLevel;

	// Token: 0x04001A8D RID: 6797
	public uint SecondsPlayed;

	// Token: 0x04001A8E RID: 6798
	public int TimesDied;

	// Token: 0x04001A8F RID: 6799
	public int EnemiesKilled;

	// Token: 0x04001A90 RID: 6800
	public byte TimesCastleLocked;

	// Token: 0x04001A91 RID: 6801
	public byte TimesRolledRelic;

	// Token: 0x04001A92 RID: 6802
	public int HighestNGPlusBeaten = -1;

	// Token: 0x04001A93 RID: 6803
	public bool SpokenToTraitor;

	// Token: 0x04001A94 RID: 6804
	public int TimesBeatenTraitor;

	// Token: 0x04001A95 RID: 6805
	public byte TraitorPreFightRepeatDialogueIndex;

	// Token: 0x04001A96 RID: 6806
	public byte TraitorPostFightRepeatDialogueIndex;

	// Token: 0x04001A97 RID: 6807
	public bool SpokenToFinalBoss;

	// Token: 0x04001A98 RID: 6808
	public bool IsDead;

	// Token: 0x04001A99 RID: 6809
	public bool InHubTown;

	// Token: 0x04001A9A RID: 6810
	public bool InCastle;

	// Token: 0x04001A9B RID: 6811
	public bool FirstTimeGoldReceived;

	// Token: 0x04001A9C RID: 6812
	public bool GoldilocksReceived;

	// Token: 0x04001A9D RID: 6813
	public CastleLockState CastleLockState;

	// Token: 0x04001A9E RID: 6814
	public bool HasStartedGame;

	// Token: 0x04001A9F RID: 6815
	public bool UnlockAllTraitorMemories;

	// Token: 0x04001AA0 RID: 6816
	public int TimesDiedSinceHestia;

	// Token: 0x04001AA1 RID: 6817
	public byte HestiaCutsceneDisplayCount;

	// Token: 0x04001AA2 RID: 6818
	public byte TreeCutsceneDisplayCount;

	// Token: 0x04001AA3 RID: 6819
	public EndingSpawnRoomType EndingSpawnRoom;

	// Token: 0x04001AA4 RID: 6820
	public float TemporaryMaxHealthMods;

	// Token: 0x04001AA5 RID: 6821
	public bool EnableHouseRules;

	// Token: 0x04001AA6 RID: 6822
	public float Assist_EnemyHealthMod = 1f;

	// Token: 0x04001AA7 RID: 6823
	public float Assist_EnemyDamageMod = 1f;

	// Token: 0x04001AA8 RID: 6824
	public float Assist_AimTimeSlow = 1f;

	// Token: 0x04001AA9 RID: 6825
	public bool Assist_EnableFlightToggle;

	// Token: 0x04001AAA RID: 6826
	public bool Assist_DisableEnemyContactDamage;

	// Token: 0x04001AAB RID: 6827
	public bool Assist_DisableTraits;

	// Token: 0x04001AAC RID: 6828
	public bool Assist_EnableDifficultyDisplay;

	// Token: 0x04001AAD RID: 6829
	public float Assist_BurdenRequirementsMod = 1f;

	// Token: 0x04001AAE RID: 6830
	public bool DisableAchievementUnlocks;

	// Token: 0x04001AAF RID: 6831
	private CachedPlayerData m_cachedData;

	// Token: 0x04001AB0 RID: 6832
	public int LineageSeed = -1;

	// Token: 0x04001AB1 RID: 6833
	public int TimesRolledLineage;

	// Token: 0x04001AB2 RID: 6834
	public Dictionary<PlayerSaveFlag, bool> FlagTable;

	// Token: 0x04001AB3 RID: 6835
	public Dictionary<RelicType, RelicObj> RelicObjTable;

	// Token: 0x04001AB4 RID: 6836
	public Dictionary<BurdenType, BurdenObj> BurdenObjTable;

	// Token: 0x04001AB5 RID: 6837
	public Dictionary<BiomeType, bool> TeleporterUnlockTable;

	// Token: 0x04001AB6 RID: 6838
	public SerializableVector2Int TeleporterUnlockDialogueIndex = new SerializableVector2Int(-1, -1);

	// Token: 0x04001AB7 RID: 6839
	public Dictionary<TraitType, SerializableVector3Int> TraitSpawnOddsTable;

	// Token: 0x04001AB8 RID: 6840
	public Dictionary<TraitType, TraitSeenState> TraitSeenTable;

	// Token: 0x04001AB9 RID: 6841
	public Dictionary<AbilityType, bool> SpellSeenTable;

	// Token: 0x04001ABA RID: 6842
	public Dictionary<HeirloomType, int> HeirloomLevelTable;

	// Token: 0x04001ABB RID: 6843
	public Dictionary<SongID, FoundState> SongsFoundTable;

	// Token: 0x04001ABC RID: 6844
	public Dictionary<InsightType, InsightState> InsightStateTable;

	// Token: 0x04001ABD RID: 6845
	public Dictionary<JournalCategoryType, SerializableVector2Int> JournalsReadTable;

	// Token: 0x04001ABE RID: 6846
	public Dictionary<NPCType, SerializableVector2Int> NPCDialogueTable;

	// Token: 0x04001ABF RID: 6847
	public List<NPCType> PopulatedNPCDialoguesList;

	// Token: 0x04001AC0 RID: 6848
	public int GlobalNPCDialogueCD;

	// Token: 0x04001AC1 RID: 6849
	public bool TriggerGlobalNPCDialogueCD;

	// Token: 0x04001AC2 RID: 6850
	public Dictionary<ClassType, int> MasteryXPTable;

	// Token: 0x04001AC3 RID: 6851
	public int RunAccumulatedXP;

	// Token: 0x04001AC4 RID: 6852
	[NonSerialized]
	private HeirloomLevelChangedEventArgs m_heirloomChangedEventArgs;

	// Token: 0x04001AC5 RID: 6853
	[NonSerialized]
	private GoldChangedEventArgs m_goldChangedEventArgs;

	// Token: 0x04001AC6 RID: 6854
	[NonSerialized]
	private GoldChangedEventArgs m_goldSavedEventArgs;
}
