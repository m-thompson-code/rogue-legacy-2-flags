using System;
using System.Collections.Generic;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
public class ChallengeManager : MonoBehaviour
{
	// Token: 0x0600555A RID: 21850 RVA: 0x0002E563 File Offset: 0x0002C763
	private void Awake()
	{
		this.m_onStartGlobalTimer = new Action<MonoBehaviour, EventArgs>(this.OnStartGlobalTimer);
		this.m_onStopGlobalTimer = new Action<MonoBehaviour, EventArgs>(this.OnStopGlobalTimer);
		this.m_onPlayerHitIncreaseTimer = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHitIncreaseTimer);
		this.Initialize();
	}

	// Token: 0x0600555B RID: 21851 RVA: 0x001425EC File Offset: 0x001407EC
	private void Initialize()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StartGlobalTimer, this.m_onStartGlobalTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StopGlobalTimer, this.m_onStopGlobalTimer);
		this.m_traitChangedArgs = new TraitChangedEventArgs(TraitType.None, TraitType.None);
		if (!SaveManager.ModeSaveData.IsInitialized)
		{
			SaveManager.ModeSaveData.Initialize();
		}
		ChallengeManager.m_isInitialized = true;
	}

	// Token: 0x0600555C RID: 21852 RVA: 0x0002E5A1 File Offset: 0x0002C7A1
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StartGlobalTimer, this.m_onStartGlobalTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StopGlobalTimer, this.m_onStopGlobalTimer);
		ChallengeManager.m_isDisposed = true;
	}

	// Token: 0x17001CF3 RID: 7411
	// (get) Token: 0x0600555D RID: 21853 RVA: 0x0002E5C3 File Offset: 0x0002C7C3
	public static ChallengeManager Instance
	{
		get
		{
			bool isDisposed = ChallengeManager.m_isDisposed;
			bool isInitialized = ChallengeManager.m_isInitialized;
			if (!ChallengeManager.m_challengeManager)
			{
				ChallengeManager.m_challengeManager = CDGHelper.FindStaticInstance<ChallengeManager>(false);
			}
			return ChallengeManager.m_challengeManager;
		}
	}

	// Token: 0x17001CF4 RID: 7412
	// (get) Token: 0x0600555E RID: 21854 RVA: 0x0002E5ED File Offset: 0x0002C7ED
	public static bool IsInitialized
	{
		get
		{
			return ChallengeManager.m_isInitialized;
		}
	}

	// Token: 0x17001CF5 RID: 7413
	// (get) Token: 0x0600555F RID: 21855 RVA: 0x0002E5F4 File Offset: 0x0002C7F4
	public static bool IsDisposed
	{
		get
		{
			return ChallengeManager.m_isDisposed;
		}
	}

	// Token: 0x17001CF6 RID: 7414
	// (get) Token: 0x06005560 RID: 21856 RVA: 0x0002E5FB File Offset: 0x0002C7FB
	public static bool IsInChallenge
	{
		get
		{
			return !ChallengeManager.ActiveChallenge.IsNativeNull();
		}
	}

	// Token: 0x17001CF7 RID: 7415
	// (get) Token: 0x06005561 RID: 21857 RVA: 0x0002E60A File Offset: 0x0002C80A
	// (set) Token: 0x06005562 RID: 21858 RVA: 0x0002E611 File Offset: 0x0002C811
	public static ChallengeObj ActiveChallenge { get; private set; }

	// Token: 0x17001CF8 RID: 7416
	// (get) Token: 0x06005563 RID: 21859 RVA: 0x0002E619 File Offset: 0x0002C819
	// (set) Token: 0x06005564 RID: 21860 RVA: 0x0002E620 File Offset: 0x0002C820
	public static ChallengeTunnelController ChallengeTunnelController { get; set; }

	// Token: 0x17001CF9 RID: 7417
	// (get) Token: 0x06005565 RID: 21861 RVA: 0x0002E628 File Offset: 0x0002C828
	// (set) Token: 0x06005566 RID: 21862 RVA: 0x0002E62F File Offset: 0x0002C82F
	public static int HitsTaken { get; set; }

	// Token: 0x17001CFA RID: 7418
	// (get) Token: 0x06005567 RID: 21863 RVA: 0x0002E637 File Offset: 0x0002C837
	// (set) Token: 0x06005568 RID: 21864 RVA: 0x0002E63E File Offset: 0x0002C83E
	public static bool NeedsSave { get; set; }

	// Token: 0x06005569 RID: 21865 RVA: 0x0002E646 File Offset: 0x0002C846
	public static bool DoesChallengeExist(ChallengeType challengeType)
	{
		return ChallengeManager.GetChallenge(challengeType) != null;
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x0014263C File Offset: 0x0014083C
	public static bool CanEnterChallenge(ChallengeType challengeType, bool suppressLogs = true)
	{
		if (!ChallengeManager.DoesChallengeExist(challengeType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>ChallengeManager.CanEnterChallenge({0}) returned false because challenge obj could not be found.</color>", new object[]
				{
					challengeType
				});
			}
			return false;
		}
		if (ChallengeManager.GetFoundState(challengeType) <= FoundState.NotFound)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>ChallengeManager.CanEnterChallenge({0}) returned false because challenge found state is 'Not Found'.</color>", new object[]
				{
					challengeType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x00142698 File Offset: 0x00140898
	public static bool CanEquip(ChallengeType challengeType, bool suppressLogs = true)
	{
		if (!ChallengeManager.DoesChallengeExist(challengeType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>ChallengeManager.CanEquip({0}) returned false because challenge obj could not be found.</color>", new object[]
				{
					challengeType
				});
			}
			return false;
		}
		if (ChallengeManager.GetFoundState(challengeType) <= FoundState.NotFound)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>ChallengeManager.CanEquip({0}) returned false because challenge has not been found yet.</color>", new object[]
				{
					challengeType
				});
			}
			return false;
		}
		int challengeEquippedLevel = ChallengeManager.GetChallengeEquippedLevel(challengeType);
		int upgradeBlueprintsFound = ChallengeManager.GetUpgradeBlueprintsFound(challengeType, false);
		if (challengeEquippedLevel >= upgradeBlueprintsFound)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>ChallengeManager.CanEquip({0}) returned false because player has already equipped all handicaps they own of this type.</color>", new object[]
				{
					challengeType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x0002E651 File Offset: 0x0002C851
	public static List<ChallengeType> GetAllChallengesWithFoundState(FoundState foundState)
	{
		ChallengeManager.GetAllChallengesWithFoundState(foundState, ChallengeManager.m_challengeTypeListHelper);
		return ChallengeManager.m_challengeTypeListHelper;
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x00142724 File Offset: 0x00140924
	public static void GetAllChallengesWithFoundState(FoundState foundState, List<ChallengeType> results)
	{
		results.Clear();
		foreach (KeyValuePair<ChallengeType, ChallengeObj> keyValuePair in SaveManager.ModeSaveData.ChallengeDict)
		{
			ChallengeObj value = keyValuePair.Value;
			if (value.FoundState == foundState)
			{
				results.Add(value.ChallengeType);
			}
		}
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x00142798 File Offset: 0x00140998
	public static FoundState GetFoundState(ChallengeType challengeType)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		FoundState foundState;
		if (challenge != null)
		{
			foundState = challenge.FoundState;
		}
		else
		{
			foundState = FoundState.NotFound;
		}
		if (foundState < FoundState.NotFound)
		{
			foundState = FoundState.NotFound;
		}
		return foundState;
	}

	// Token: 0x0600556F RID: 21871 RVA: 0x001427C8 File Offset: 0x001409C8
	public static bool SetFoundState(ChallengeType challengeType, FoundState foundState, bool overrideValues, bool runEvents = true)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge == null)
		{
			Debug.LogFormat("<color=red>Could not set challenge {0} to FoundState: {1}. Challenge obj not found.</color>", new object[]
			{
				challengeType,
				foundState
			});
			return false;
		}
		FoundState foundState2 = ChallengeManager.GetFoundState(challengeType);
		if (foundState > foundState2 || overrideValues)
		{
			challenge.FoundLevel = (int)foundState;
			return true;
		}
		Debug.LogFormat("<color=red>Could not set challenge {0} to FoundState: {1}. Current FoundState: {2} is a higher value. Please set overrideValues = true to override this if intended.</color>", new object[]
		{
			challengeType,
			foundState,
			ChallengeManager.GetFoundState(challengeType)
		});
		return false;
	}

	// Token: 0x06005570 RID: 21872 RVA: 0x0014284C File Offset: 0x00140A4C
	public static int GetUpgradeBlueprintsFound(ChallengeType challengeType, bool ignoreInfinitePurchasePower = false)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			return Mathf.Clamp(challenge.UpgradeBlueprintsFound, 0, challenge.MaxLevel);
		}
		return 0;
	}

	// Token: 0x06005571 RID: 21873 RVA: 0x00142878 File Offset: 0x00140A78
	public static bool SetUpgradeBlueprintsFound(ChallengeType challengeType, int level, bool additive, bool runEvents = true)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge == null)
		{
			return false;
		}
		int num = challenge.UpgradeBlueprintsFound;
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (num > challenge.MaxLevel)
		{
			Debug.Log("<color=red>ChallengeManager.SetUpgradeBlueprintsFound(" + challengeType.ToString() + ") failed.  Upgrading will put blueprints found beyond max level.</color>");
			return false;
		}
		challenge.UpgradeBlueprintsFound = Mathf.Clamp(num, 0, challenge.MaxLevel);
		return true;
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001428E4 File Offset: 0x00140AE4
	public static int GetChallengeEquippedLevel(ChallengeType challengeType)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			return Mathf.Clamp(challenge.EquippedLevel, 0, challenge.MaxEquippableLevel);
		}
		return 0;
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x00142910 File Offset: 0x00140B10
	public static bool SetChallengeEquippedLevel(ChallengeType challengeType, int level, bool additive, bool runEvents = true)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			int num = challenge.EquippedLevel;
			if (additive)
			{
				num += level;
			}
			else
			{
				num = level;
			}
			num = Mathf.Clamp(num, 0, challenge.MaxEquippableLevel);
			if (num != challenge.EquippedLevel)
			{
				challenge.EquippedLevel = num;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x0002E663 File Offset: 0x0002C863
	public static ChallengeObj GetChallenge(ChallengeType challengeType)
	{
		return SaveManager.ModeSaveData.ChallengeDict[challengeType];
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x0014295C File Offset: 0x00140B5C
	public static int GetTotalTrophiesEarned(bool includeTutorialPurified)
	{
		int num = 0;
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None && (includeTutorialPurified || challengeType != ChallengeType.TutorialPurified) && ChallengeManager.GetChallengeTrophyRank(challengeType, true) != ChallengeTrophyRank.None)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x0014299C File Offset: 0x00140B9C
	public static int GetTotalTrophiesEarnedOfRank(ChallengeTrophyRank trophyRank, bool includeTutorialPurified)
	{
		int num = 0;
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None && (includeTutorialPurified || challengeType != ChallengeType.TutorialPurified) && ChallengeManager.GetChallengeTrophyRank(challengeType, true) == trophyRank)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001429E0 File Offset: 0x00140BE0
	public static int GetTotalTrophiesEarnedOfRankOrHigher(ChallengeTrophyRank trophyRank, bool includeTutorialPurified)
	{
		int num = 0;
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None && (includeTutorialPurified || challengeType != ChallengeType.TutorialPurified) && ChallengeManager.GetChallengeTrophyRank(challengeType, true) >= trophyRank)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x0002E675 File Offset: 0x0002C875
	public static void SetActiveChallenge(ChallengeType challengeType)
	{
		if (challengeType == ChallengeType.None)
		{
			ChallengeManager.ActiveChallenge = null;
			return;
		}
		ChallengeManager.ActiveChallenge = ChallengeManager.GetChallenge(challengeType);
		ChallengeManager.HitsTaken = 0;
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x0002E692 File Offset: 0x0002C892
	public static float GetActiveHandicapMod()
	{
		if (ChallengeManager.ActiveChallenge.ChallengeData.ScoringType == ChallengeScoringType.Battle)
		{
			return (float)ChallengeManager.ActiveChallenge.EquippedLevel * ChallengeManager.ActiveChallenge.ChallengeData.ScalingHandicap;
		}
		return 0f;
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x0002E6C7 File Offset: 0x0002C8C7
	public static void ReturnToDriftHouseWithTransition()
	{
		if (!SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			SceneLoader_RL.RunTransitionWithLogic(delegate()
			{
				ChallengeManager.ChallengeTunnelController.ReturnToDriftHouse(true);
			}, TransitionID.ScreenDistortion, false);
		}
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x0002E6F6 File Offset: 0x0002C8F6
	public static ClassType GetChallengeClassOverride(ChallengeType challengeType)
	{
		return ChallengeManager.GetChallenge(challengeType).ChallengeData.ClassOverride;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x00142A24 File Offset: 0x00140C24
	public static void SetupCharacter()
	{
		ChallengeManager.Instance.m_storedCharData = SaveManager.PlayerSaveData.CurrentCharacter.Clone();
		ClassType classType = ChallengeManager.GetChallengeClassOverride(ChallengeManager.ActiveChallenge.ChallengeType);
		if (classType == ClassType.None && (SaveManager.PlayerSaveData.CurrentCharacter.TraitOne == TraitType.RandomizeKit || SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo == TraitType.RandomizeKit))
		{
			classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		}
		if (classType != ClassType.None)
		{
			CharacterCreator.GenerateClass(classType, SaveManager.PlayerSaveData.CurrentCharacter);
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = ChallengeManager.Instance.m_storedCharData.Spell;
		}
		if (SaveManager.PlayerSaveData.CurrentCharacter.Weapon == AbilityType.PacifistWeapon)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Weapon = CharacterCreator.GetAvailableWeapons(SaveManager.PlayerSaveData.CurrentCharacter.ClassType)[0];
		}
		SaveManager.PlayerSaveData.CurrentCharacter.TraitOne = TraitType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo = TraitType.None;
		ChallengeManager.Instance.m_traitChangedArgs.Initialize(TraitType.None, TraitType.None);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, ChallengeManager.Instance, ChallengeManager.Instance.m_traitChangedArgs);
		SaveManager.PlayerSaveData.CurrentCharacter.AntiqueOneOwned = RelicType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.AntiqueTwoOwned = RelicType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.CapeEquipmentType = EquipmentType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.ChestEquipmentType = EquipmentType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType = EquipmentType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.HeadEquipmentType = EquipmentType.None;
		SaveManager.PlayerSaveData.CurrentCharacter.TrinketEquipmentType = EquipmentType.None;
		ChallengeManager.Instance.m_storedRuneDict.Clear();
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in SaveManager.EquipmentSaveData.RuneDict)
		{
			ChallengeManager.Instance.m_storedRuneDict.Add(keyValuePair.Key, keyValuePair.Value.EquippedLevel);
			keyValuePair.Value.EquippedLevel = 0;
		}
		ChallengeManager.Instance.m_storedRelicDict.Clear();
		foreach (KeyValuePair<RelicType, RelicObj> keyValuePair2 in SaveManager.PlayerSaveData.RelicObjTable)
		{
			ChallengeManager.Instance.m_storedRelicDict.Add(keyValuePair2.Key, keyValuePair2.Value.Level);
			keyValuePair2.Value.SetLevel(0, false, true);
		}
		ChallengeManager.Instance.m_storedMasteryXPTable.Clear();
		foreach (ClassType classType2 in ClassType_RL.TypeArray)
		{
			if (classType2 != ClassType.None)
			{
				ChallengeManager.Instance.m_storedMasteryXPTable.Add(classType2, SaveManager.PlayerSaveData.GetClassXP(classType2));
				SaveManager.PlayerSaveData.SetClassXP(classType2, 0, false, true, true);
			}
		}
		ChallengeManager.Instance.m_storedSkillTreeDict.Clear();
		foreach (KeyValuePair<SkillTreeType, SkillTreeObj> keyValuePair3 in SaveManager.EquipmentSaveData.SkillTreeDict)
		{
			ChallengeManager.Instance.m_storedSkillTreeDict.Add(keyValuePair3.Key, keyValuePair3.Value.Level);
			SkillTreeManager.SetSkillObjLevel(keyValuePair3.Key, 0, false, true, false);
		}
		SaveManager.EquipmentSaveData.SkillTreeDict[SkillTreeType.Relic_Cost_Down].Level = 5;
		SaveManager.EquipmentSaveData.SkillTreeDict[SkillTreeType.Reroll_Relic].Level = 3;
		SaveManager.EquipmentSaveData.SkillTreeDict[SkillTreeType.Reroll_Relic_Room_Cap].Level = 1;
		ChallengeManager.Instance.m_storedBurdenDict.Clear();
		foreach (KeyValuePair<BurdenType, BurdenObj> keyValuePair4 in SaveManager.PlayerSaveData.BurdenObjTable)
		{
			ChallengeManager.Instance.m_storedBurdenDict.Add(keyValuePair4.Key, keyValuePair4.Value.CurrentLevel);
			keyValuePair4.Value.SetLevel(0, false, true);
		}
		ChallengeManager.Instance.m_storedNGPlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		SaveManager.PlayerSaveData.NewGamePlusLevel = 0;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.NGPlusChanged, null, null);
		ChallengeManager.Instance.m_storedSoulShopDict.Clear();
		foreach (KeyValuePair<SoulShopType, SoulShopObj> keyValuePair5 in SaveManager.ModeSaveData.SoulShopTable)
		{
			ChallengeManager.Instance.m_storedSoulShopDict.Add(keyValuePair5.Key, keyValuePair5.Value.CurrentEquippedLevel);
			keyValuePair5.Value.SetEquippedLevel(0, false, true);
		}
		ChallengeManager.Instance.m_storedTemporaryMaxHealthMods = SaveManager.PlayerSaveData.TemporaryMaxHealthMods;
		SaveManager.PlayerSaveData.TemporaryMaxHealthMods = 0f;
		ChallengeManager.Instance.m_storedHeirloomLevelDict.Clear();
		foreach (KeyValuePair<HeirloomType, int> keyValuePair6 in SaveManager.PlayerSaveData.HeirloomLevelTable)
		{
			ChallengeManager.Instance.m_storedHeirloomLevelDict.Add(keyValuePair6.Key, keyValuePair6.Value);
		}
		ChallengeType challengeType = ChallengeManager.ActiveChallenge.ChallengeType;
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.PlatformRanger)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = AbilityType.BowWeapon;
		}
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.PlatformBoat)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = AbilityType.FireballSpell;
		}
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.PlatformAxe)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = AbilityType.StraightBoltSpell;
			SaveManager.PlayerSaveData.ResetAllHeirlooms();
			SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.UnlockAirDash, 1, false, false);
			SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.UnlockMemory, 1, false, false);
		}
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.PlatformKatana)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = AbilityType.LightningSpell;
		}
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.PlatformClimb)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = AbilityType.GravityWellSpell;
		}
		BaseCharacterController playerController = PlayerManager.GetPlayerController();
		LineageWindowController.CharacterLoadedFromLineage = true;
		playerController.ResetCharacter();
		LineageWindowController.CharacterLoadedFromLineage = false;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x001430A8 File Offset: 0x001412A8
	public static void RestoreCharacter(bool updateVisuals)
	{
		foreach (KeyValuePair<SoulShopType, int> keyValuePair in ChallengeManager.Instance.m_storedSoulShopDict)
		{
			SaveManager.ModeSaveData.SoulShopTable[keyValuePair.Key].SetEquippedLevel(keyValuePair.Value, false, true);
		}
		SaveManager.PlayerSaveData.CurrentCharacter = ChallengeManager.Instance.m_storedCharData;
		foreach (KeyValuePair<RuneType, int> keyValuePair2 in ChallengeManager.Instance.m_storedRuneDict)
		{
			SaveManager.EquipmentSaveData.RuneDict[keyValuePair2.Key].EquippedLevel = keyValuePair2.Value;
		}
		foreach (KeyValuePair<RelicType, int> keyValuePair3 in ChallengeManager.Instance.m_storedRelicDict)
		{
			RelicObj relicObj = SaveManager.PlayerSaveData.RelicObjTable[keyValuePair3.Key];
			relicObj.SetLevel(keyValuePair3.Value, false, updateVisuals);
			relicObj.SetIntValue(0, false, updateVisuals);
			relicObj.SetFloatValue(0f, false, updateVisuals);
		}
		foreach (KeyValuePair<ClassType, int> keyValuePair4 in ChallengeManager.Instance.m_storedMasteryXPTable)
		{
			SaveManager.PlayerSaveData.SetClassXP(keyValuePair4.Key, keyValuePair4.Value, false, true, true);
		}
		foreach (KeyValuePair<SkillTreeType, int> keyValuePair5 in ChallengeManager.Instance.m_storedSkillTreeDict)
		{
			SkillTreeManager.SetSkillObjLevel(keyValuePair5.Key, keyValuePair5.Value, false, true, true);
		}
		foreach (KeyValuePair<BurdenType, int> keyValuePair6 in ChallengeManager.Instance.m_storedBurdenDict)
		{
			SaveManager.PlayerSaveData.GetBurden(keyValuePair6.Key).SetLevel(keyValuePair6.Value, false, true);
		}
		SaveManager.PlayerSaveData.NewGamePlusLevel = ChallengeManager.Instance.m_storedNGPlusLevel;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.NGPlusChanged, null, null);
		SaveManager.PlayerSaveData.TimesRolledRelic = 0;
		SaveManager.PlayerSaveData.TemporaryMaxHealthMods = ChallengeManager.Instance.m_storedTemporaryMaxHealthMods;
		foreach (KeyValuePair<HeirloomType, int> keyValuePair7 in ChallengeManager.Instance.m_storedHeirloomLevelDict)
		{
			SaveManager.PlayerSaveData.SetHeirloomLevel(keyValuePair7.Key, keyValuePair7.Value, false, true);
		}
		if (!updateVisuals)
		{
			return;
		}
		ChallengeManager.Instance.m_traitChangedArgs.Initialize(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne, SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, ChallengeManager.Instance, ChallengeManager.Instance.m_traitChangedArgs);
		LineageWindowController.CharacterLoadedFromLineage = true;
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.ResetCharacter();
		playerController.CharacterHitResponse.BlinkPulseEffect.StopInvincibilityEffect();
		LineageWindowController.CharacterLoadedFromLineage = false;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x00143424 File Offset: 0x00141624
	public static float ApplyStatCap(float actualStat, bool isDexterityOrFocus = false)
	{
		float num = (float)ChallengeManager.ActiveChallenge.ChallengeData.BaseHandicap;
		if (!isDexterityOrFocus)
		{
			num += 15f;
		}
		return num;
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x00143450 File Offset: 0x00141650
	private void OnStartGlobalTimer(MonoBehaviour sender, EventArgs args)
	{
		if (ChallengeManager.IsInChallenge)
		{
			if (ChallengeManager.ActiveChallenge.ChallengeData.ScoringType == ChallengeScoringType.Platform)
			{
				GlobalTimerHUDController.ElapsedTime = (float)ChallengeManager.ActiveChallenge.EquippedLevel * -ChallengeManager.ActiveChallenge.ChallengeData.ScalingHandicap;
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHitIncreaseTimer);
			}
			PlayerManager.GetPlayerController().TakesNoDamage = false;
		}
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x0002E708 File Offset: 0x0002C908
	private void OnStopGlobalTimer(MonoBehaviour sender, EventArgs args)
	{
		if (ChallengeManager.IsInChallenge)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = true;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHitIncreaseTimer);
		}
	}

	// Token: 0x06005581 RID: 21889 RVA: 0x0002E728 File Offset: 0x0002C928
	private void OnPlayerHitIncreaseTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.ElapsedTime += 0f;
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x001434B0 File Offset: 0x001416B0
	public static void UpdateChallengeScore(ChallengeType challengeType, ClassType classType, int newScore, float newTime, bool usingHandicaps)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (!challenge.ClassHighScores.ContainsKey(classType))
		{
			challenge.ClassHighScores.Add(classType, 0);
			ChallengeManager.NeedsSave = true;
		}
		if (newScore > challenge.ClassHighScores[classType])
		{
			challenge.ClassHighScores[classType] = newScore;
			ChallengeManager.NeedsSave = true;
		}
		int num = 0;
		foreach (KeyValuePair<ClassType, int> keyValuePair in challenge.ClassHighScores)
		{
			num += keyValuePair.Value;
		}
		challenge.TotalHighScore = num;
		if (newTime < challenge.BestTime)
		{
			challenge.BestTime = newTime;
			ChallengeManager.NeedsSave = true;
		}
		if (!usingHandicaps && newTime < challenge.BestTimeWithoutHandicaps)
		{
			challenge.BestTimeWithoutHandicaps = newTime;
			ChallengeManager.NeedsSave = true;
		}
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x0014358C File Offset: 0x0014178C
	public static ChallengeTrophyRank GetChallengeTrophyRank(ChallengeType challengeType, bool checkForSisyphusTrophy = true)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			if (challengeType == ChallengeType.TutorialPurified)
			{
				int num = ChallengeManager.GetTotalTrophyCount() - 1;
				int totalTrophiesEarnedOfRankOrHigher = ChallengeManager.GetTotalTrophiesEarnedOfRankOrHigher(ChallengeTrophyRank.Bronze, false);
				int totalTrophiesEarnedOfRankOrHigher2 = ChallengeManager.GetTotalTrophiesEarnedOfRankOrHigher(ChallengeTrophyRank.Silver, false);
				if (ChallengeManager.GetTotalTrophiesEarnedOfRankOrHigher(ChallengeTrophyRank.Gold, false) >= num && (SaveManager.ModeSaveData.HasGoldSisyphusTrophy || !checkForSisyphusTrophy))
				{
					return ChallengeTrophyRank.Gold;
				}
				if (totalTrophiesEarnedOfRankOrHigher2 >= num && (SaveManager.ModeSaveData.HasSilverSisyphusTrophy || !checkForSisyphusTrophy))
				{
					return ChallengeTrophyRank.Silver;
				}
				if (totalTrophiesEarnedOfRankOrHigher >= num && (SaveManager.ModeSaveData.HasBronzeSisyphusTrophy || !checkForSisyphusTrophy))
				{
					return ChallengeTrophyRank.Bronze;
				}
				return ChallengeTrophyRank.None;
			}
			else
			{
				ChallengeData challengeData = challenge.ChallengeData;
				if (challengeData)
				{
					if (challengeData.Disabled)
					{
						return ChallengeTrophyRank.None;
					}
					if (challengeData.ScoringType == ChallengeScoringType.Battle)
					{
						int totalHighScore = challenge.TotalHighScore;
						if (totalHighScore >= challengeData.GoldReq)
						{
							return ChallengeTrophyRank.Gold;
						}
						if (totalHighScore >= challengeData.SilverReq)
						{
							return ChallengeTrophyRank.Silver;
						}
						if (totalHighScore >= challengeData.BronzeReq)
						{
							return ChallengeTrophyRank.Bronze;
						}
						return ChallengeTrophyRank.None;
					}
					else if (challengeData.ScoringType == ChallengeScoringType.Platform)
					{
						float num2 = Mathf.Max(0f, challenge.BestTime - challengeData.ParTime);
						if (num2 <= 0f)
						{
							return ChallengeTrophyRank.Gold;
						}
						if (num2 <= 3f)
						{
							return ChallengeTrophyRank.Silver;
						}
						if (num2 <= 10f)
						{
							return ChallengeTrophyRank.Bronze;
						}
						return ChallengeTrophyRank.None;
					}
				}
			}
		}
		return ChallengeTrophyRank.None;
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x001436AC File Offset: 0x001418AC
	public static float CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType statType)
	{
		switch (statType)
		{
		case ChallengeCompleteStatsEntryType.BaseScore:
			return 2500f;
		case ChallengeCompleteStatsEntryType.HitsTaken:
			return (float)Mathf.Max(7500 - ChallengeManager.HitsTaken * 500, 0);
		case ChallengeCompleteStatsEntryType.Resolve:
			return (float)Mathf.Max(7500 - Mathf.RoundToInt(SaveManager.PlayerSaveData.GetTotalRelicResolveCost() * 100f) * 50, 0);
		case ChallengeCompleteStatsEntryType.Timer:
		{
			int num = Mathf.Max(Mathf.RoundToInt(GlobalTimerHUDController.ElapsedTime - ChallengeManager.ActiveChallenge.ChallengeData.ParTime), 0);
			return Mathf.Max(7500f - (float)num * 10f * 10f, 0f);
		}
		case ChallengeCompleteStatsEntryType.HandicapMod:
			return Mathf.Max(2f - (float)ChallengeManager.ActiveChallenge.EquippedLevel * 0.1f, 0.1f);
		case ChallengeCompleteStatsEntryType.FinalScore:
			return (float)Mathf.RoundToInt((0f + ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.BaseScore) + ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.HitsTaken) + ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.Timer) + ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.Resolve)) * ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.HandicapMod));
		}
		return 0f;
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x001437BC File Offset: 0x001419BC
	public static string GetChallengeLetterGrade(ChallengeType challengeType, float score)
	{
		int num = ChallengeManager.m_letterGradeArray.Length;
		int num2 = Mathf.Clamp((int)(score / 50000f * (float)num), 0, num - 1);
		return ChallengeManager.m_letterGradeArray[num2];
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x001437F0 File Offset: 0x001419F0
	public static int GetChallengeClassHighScore(ChallengeType challengeType, ClassType classType)
	{
		int result;
		if (ChallengeManager.GetChallenge(challengeType).ClassHighScores.TryGetValue(classType, out result))
		{
			return result;
		}
		return 0;
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x0002E73A File Offset: 0x0002C93A
	public static int GetTotalTrophyCount()
	{
		return Challenge_EV.CHALLENGE_ORDER.Length;
	}

	// Token: 0x04003F6D RID: 16237
	private static bool m_isDisposed;

	// Token: 0x04003F6E RID: 16238
	private static bool m_isInitialized;

	// Token: 0x04003F6F RID: 16239
	private CharacterData m_storedCharData;

	// Token: 0x04003F70 RID: 16240
	private Dictionary<RelicType, int> m_storedRelicDict = new Dictionary<RelicType, int>();

	// Token: 0x04003F71 RID: 16241
	private Dictionary<RuneType, int> m_storedRuneDict = new Dictionary<RuneType, int>();

	// Token: 0x04003F72 RID: 16242
	private Dictionary<ClassType, int> m_storedMasteryXPTable = new Dictionary<ClassType, int>();

	// Token: 0x04003F73 RID: 16243
	private Dictionary<SkillTreeType, int> m_storedSkillTreeDict = new Dictionary<SkillTreeType, int>();

	// Token: 0x04003F74 RID: 16244
	private Dictionary<BurdenType, int> m_storedBurdenDict = new Dictionary<BurdenType, int>();

	// Token: 0x04003F75 RID: 16245
	private Dictionary<SoulShopType, int> m_storedSoulShopDict = new Dictionary<SoulShopType, int>();

	// Token: 0x04003F76 RID: 16246
	private Dictionary<HeirloomType, int> m_storedHeirloomLevelDict = new Dictionary<HeirloomType, int>();

	// Token: 0x04003F77 RID: 16247
	private float m_storedTemporaryMaxHealthMods;

	// Token: 0x04003F78 RID: 16248
	private int m_storedNGPlusLevel;

	// Token: 0x04003F79 RID: 16249
	private TraitChangedEventArgs m_traitChangedArgs;

	// Token: 0x04003F7A RID: 16250
	private Action<MonoBehaviour, EventArgs> m_onStartGlobalTimer;

	// Token: 0x04003F7B RID: 16251
	private Action<MonoBehaviour, EventArgs> m_onStopGlobalTimer;

	// Token: 0x04003F7C RID: 16252
	private Action<MonoBehaviour, EventArgs> m_onPlayerHitIncreaseTimer;

	// Token: 0x04003F7D RID: 16253
	private static ChallengeManager m_challengeManager = null;

	// Token: 0x04003F82 RID: 16258
	private static List<ChallengeType> m_challengeTypeListHelper = new List<ChallengeType>();

	// Token: 0x04003F83 RID: 16259
	private static readonly string[] m_letterGradeArray = new string[]
	{
		"C+",
		"C+",
		"C+",
		"C++",
		"C++",
		"C++",
		"B",
		"B",
		"B",
		"B+",
		"B+",
		"B+",
		"B++",
		"B++",
		"A",
		"A+",
		"A++",
		"S",
		"S+",
		"S++"
	};
}
