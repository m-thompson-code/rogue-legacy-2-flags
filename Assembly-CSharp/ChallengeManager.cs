using System;
using System.Collections.Generic;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200068E RID: 1678
public class ChallengeManager : MonoBehaviour
{
	// Token: 0x06003CC8 RID: 15560 RVA: 0x000D21C8 File Offset: 0x000D03C8
	private void Awake()
	{
		this.m_onStartGlobalTimer = new Action<MonoBehaviour, EventArgs>(this.OnStartGlobalTimer);
		this.m_onStopGlobalTimer = new Action<MonoBehaviour, EventArgs>(this.OnStopGlobalTimer);
		this.m_onPlayerHitIncreaseTimer = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHitIncreaseTimer);
		this.Initialize();
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x000D2208 File Offset: 0x000D0408
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

	// Token: 0x06003CCA RID: 15562 RVA: 0x000D2258 File Offset: 0x000D0458
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StartGlobalTimer, this.m_onStartGlobalTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StopGlobalTimer, this.m_onStopGlobalTimer);
		ChallengeManager.m_isDisposed = true;
	}

	// Token: 0x17001527 RID: 5415
	// (get) Token: 0x06003CCB RID: 15563 RVA: 0x000D227A File Offset: 0x000D047A
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

	// Token: 0x17001528 RID: 5416
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x000D22A4 File Offset: 0x000D04A4
	public static bool IsInitialized
	{
		get
		{
			return ChallengeManager.m_isInitialized;
		}
	}

	// Token: 0x17001529 RID: 5417
	// (get) Token: 0x06003CCD RID: 15565 RVA: 0x000D22AB File Offset: 0x000D04AB
	public static bool IsDisposed
	{
		get
		{
			return ChallengeManager.m_isDisposed;
		}
	}

	// Token: 0x1700152A RID: 5418
	// (get) Token: 0x06003CCE RID: 15566 RVA: 0x000D22B2 File Offset: 0x000D04B2
	public static bool IsInChallenge
	{
		get
		{
			return !ChallengeManager.ActiveChallenge.IsNativeNull();
		}
	}

	// Token: 0x1700152B RID: 5419
	// (get) Token: 0x06003CCF RID: 15567 RVA: 0x000D22C1 File Offset: 0x000D04C1
	// (set) Token: 0x06003CD0 RID: 15568 RVA: 0x000D22C8 File Offset: 0x000D04C8
	public static ChallengeObj ActiveChallenge { get; private set; }

	// Token: 0x1700152C RID: 5420
	// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x000D22D0 File Offset: 0x000D04D0
	// (set) Token: 0x06003CD2 RID: 15570 RVA: 0x000D22D7 File Offset: 0x000D04D7
	public static ChallengeTunnelController ChallengeTunnelController { get; set; }

	// Token: 0x1700152D RID: 5421
	// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x000D22DF File Offset: 0x000D04DF
	// (set) Token: 0x06003CD4 RID: 15572 RVA: 0x000D22E6 File Offset: 0x000D04E6
	public static int HitsTaken { get; set; }

	// Token: 0x1700152E RID: 5422
	// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x000D22EE File Offset: 0x000D04EE
	// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x000D22F5 File Offset: 0x000D04F5
	public static bool NeedsSave { get; set; }

	// Token: 0x06003CD7 RID: 15575 RVA: 0x000D22FD File Offset: 0x000D04FD
	public static bool DoesChallengeExist(ChallengeType challengeType)
	{
		return ChallengeManager.GetChallenge(challengeType) != null;
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x000D2308 File Offset: 0x000D0508
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

	// Token: 0x06003CD9 RID: 15577 RVA: 0x000D2364 File Offset: 0x000D0564
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

	// Token: 0x06003CDA RID: 15578 RVA: 0x000D23EF File Offset: 0x000D05EF
	public static List<ChallengeType> GetAllChallengesWithFoundState(FoundState foundState)
	{
		ChallengeManager.GetAllChallengesWithFoundState(foundState, ChallengeManager.m_challengeTypeListHelper);
		return ChallengeManager.m_challengeTypeListHelper;
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x000D2404 File Offset: 0x000D0604
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

	// Token: 0x06003CDC RID: 15580 RVA: 0x000D2478 File Offset: 0x000D0678
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

	// Token: 0x06003CDD RID: 15581 RVA: 0x000D24A8 File Offset: 0x000D06A8
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

	// Token: 0x06003CDE RID: 15582 RVA: 0x000D252C File Offset: 0x000D072C
	public static int GetUpgradeBlueprintsFound(ChallengeType challengeType, bool ignoreInfinitePurchasePower = false)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			return Mathf.Clamp(challenge.UpgradeBlueprintsFound, 0, challenge.MaxLevel);
		}
		return 0;
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x000D2558 File Offset: 0x000D0758
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

	// Token: 0x06003CE0 RID: 15584 RVA: 0x000D25C4 File Offset: 0x000D07C4
	public static int GetChallengeEquippedLevel(ChallengeType challengeType)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			return Mathf.Clamp(challenge.EquippedLevel, 0, challenge.MaxEquippableLevel);
		}
		return 0;
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x000D25F0 File Offset: 0x000D07F0
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

	// Token: 0x06003CE2 RID: 15586 RVA: 0x000D263A File Offset: 0x000D083A
	public static ChallengeObj GetChallenge(ChallengeType challengeType)
	{
		return SaveManager.ModeSaveData.ChallengeDict[challengeType];
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x000D264C File Offset: 0x000D084C
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

	// Token: 0x06003CE4 RID: 15588 RVA: 0x000D268C File Offset: 0x000D088C
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

	// Token: 0x06003CE5 RID: 15589 RVA: 0x000D26D0 File Offset: 0x000D08D0
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

	// Token: 0x06003CE6 RID: 15590 RVA: 0x000D2711 File Offset: 0x000D0911
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

	// Token: 0x06003CE7 RID: 15591 RVA: 0x000D272E File Offset: 0x000D092E
	public static float GetActiveHandicapMod()
	{
		if (ChallengeManager.ActiveChallenge.ChallengeData.ScoringType == ChallengeScoringType.Battle)
		{
			return (float)ChallengeManager.ActiveChallenge.EquippedLevel * ChallengeManager.ActiveChallenge.ChallengeData.ScalingHandicap;
		}
		return 0f;
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x000D2763 File Offset: 0x000D0963
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

	// Token: 0x06003CE9 RID: 15593 RVA: 0x000D2792 File Offset: 0x000D0992
	public static ClassType GetChallengeClassOverride(ChallengeType challengeType)
	{
		return ChallengeManager.GetChallenge(challengeType).ChallengeData.ClassOverride;
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x000D27A4 File Offset: 0x000D09A4
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

	// Token: 0x06003CEB RID: 15595 RVA: 0x000D2E28 File Offset: 0x000D1028
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

	// Token: 0x06003CEC RID: 15596 RVA: 0x000D31A4 File Offset: 0x000D13A4
	public static float ApplyStatCap(float actualStat, bool isDexterityOrFocus = false)
	{
		float num = (float)ChallengeManager.ActiveChallenge.ChallengeData.BaseHandicap;
		if (!isDexterityOrFocus)
		{
			num += 15f;
		}
		return num;
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x000D31D0 File Offset: 0x000D13D0
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

	// Token: 0x06003CEE RID: 15598 RVA: 0x000D322D File Offset: 0x000D142D
	private void OnStopGlobalTimer(MonoBehaviour sender, EventArgs args)
	{
		if (ChallengeManager.IsInChallenge)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = true;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHitIncreaseTimer);
		}
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x000D324D File Offset: 0x000D144D
	private void OnPlayerHitIncreaseTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.ElapsedTime += 0f;
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x000D3260 File Offset: 0x000D1460
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

	// Token: 0x06003CF1 RID: 15601 RVA: 0x000D333C File Offset: 0x000D153C
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

	// Token: 0x06003CF2 RID: 15602 RVA: 0x000D345C File Offset: 0x000D165C
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

	// Token: 0x06003CF3 RID: 15603 RVA: 0x000D356C File Offset: 0x000D176C
	public static string GetChallengeLetterGrade(ChallengeType challengeType, float score)
	{
		int num = ChallengeManager.m_letterGradeArray.Length;
		int num2 = Mathf.Clamp((int)(score / 50000f * (float)num), 0, num - 1);
		return ChallengeManager.m_letterGradeArray[num2];
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x000D35A0 File Offset: 0x000D17A0
	public static int GetChallengeClassHighScore(ChallengeType challengeType, ClassType classType)
	{
		int result;
		if (ChallengeManager.GetChallenge(challengeType).ClassHighScores.TryGetValue(classType, out result))
		{
			return result;
		}
		return 0;
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x000D35C5 File Offset: 0x000D17C5
	public static int GetTotalTrophyCount()
	{
		return Challenge_EV.CHALLENGE_ORDER.Length;
	}

	// Token: 0x04002DA4 RID: 11684
	private static bool m_isDisposed;

	// Token: 0x04002DA5 RID: 11685
	private static bool m_isInitialized;

	// Token: 0x04002DA6 RID: 11686
	private CharacterData m_storedCharData;

	// Token: 0x04002DA7 RID: 11687
	private Dictionary<RelicType, int> m_storedRelicDict = new Dictionary<RelicType, int>();

	// Token: 0x04002DA8 RID: 11688
	private Dictionary<RuneType, int> m_storedRuneDict = new Dictionary<RuneType, int>();

	// Token: 0x04002DA9 RID: 11689
	private Dictionary<ClassType, int> m_storedMasteryXPTable = new Dictionary<ClassType, int>();

	// Token: 0x04002DAA RID: 11690
	private Dictionary<SkillTreeType, int> m_storedSkillTreeDict = new Dictionary<SkillTreeType, int>();

	// Token: 0x04002DAB RID: 11691
	private Dictionary<BurdenType, int> m_storedBurdenDict = new Dictionary<BurdenType, int>();

	// Token: 0x04002DAC RID: 11692
	private Dictionary<SoulShopType, int> m_storedSoulShopDict = new Dictionary<SoulShopType, int>();

	// Token: 0x04002DAD RID: 11693
	private Dictionary<HeirloomType, int> m_storedHeirloomLevelDict = new Dictionary<HeirloomType, int>();

	// Token: 0x04002DAE RID: 11694
	private float m_storedTemporaryMaxHealthMods;

	// Token: 0x04002DAF RID: 11695
	private int m_storedNGPlusLevel;

	// Token: 0x04002DB0 RID: 11696
	private TraitChangedEventArgs m_traitChangedArgs;

	// Token: 0x04002DB1 RID: 11697
	private Action<MonoBehaviour, EventArgs> m_onStartGlobalTimer;

	// Token: 0x04002DB2 RID: 11698
	private Action<MonoBehaviour, EventArgs> m_onStopGlobalTimer;

	// Token: 0x04002DB3 RID: 11699
	private Action<MonoBehaviour, EventArgs> m_onPlayerHitIncreaseTimer;

	// Token: 0x04002DB4 RID: 11700
	private static ChallengeManager m_challengeManager = null;

	// Token: 0x04002DB9 RID: 11705
	private static List<ChallengeType> m_challengeTypeListHelper = new List<ChallengeType>();

	// Token: 0x04002DBA RID: 11706
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
