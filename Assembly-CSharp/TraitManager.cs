using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5B RID: 2907
public class TraitManager : MonoBehaviour
{
	// Token: 0x17001D8B RID: 7563
	// (get) Token: 0x0600586F RID: 22639 RVA: 0x000300F8 File Offset: 0x0002E2F8
	// (set) Token: 0x06005870 RID: 22640 RVA: 0x000300FF File Offset: 0x0002E2FF
	public static bool IsInitialized { get; private set; }

	// Token: 0x17001D8C RID: 7564
	// (get) Token: 0x06005871 RID: 22641 RVA: 0x00030107 File Offset: 0x0002E307
	// (set) Token: 0x06005872 RID: 22642 RVA: 0x0003010E File Offset: 0x0002E30E
	public static TraitManager Instance { get; private set; }

	// Token: 0x17001D8D RID: 7565
	// (get) Token: 0x06005873 RID: 22643 RVA: 0x00030116 File Offset: 0x0002E316
	public static List<BaseTrait> ActiveTraitList
	{
		get
		{
			return TraitManager.Instance.m_activeTraitsList;
		}
	}

	// Token: 0x17001D8E RID: 7566
	// (get) Token: 0x06005874 RID: 22644 RVA: 0x00030122 File Offset: 0x0002E322
	public static List<TraitType> ActiveTraitTypeList
	{
		get
		{
			return TraitManager.Instance.m_activeTraitTypeList;
		}
	}

	// Token: 0x06005875 RID: 22645 RVA: 0x001519BC File Offset: 0x0014FBBC
	public static float GetActualTraitGoldGain(TraitType traitType)
	{
		if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Traits_Give_Gold) > 0)
		{
			TraitData traitData = TraitLibrary.GetTraitData(traitType);
			if (traitData != null)
			{
				return traitData.GoldBonus * (1f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Traits_Give_Gold_Gain_Mod).CurrentStatGain);
			}
		}
		return 0f;
	}

	// Token: 0x06005876 RID: 22646 RVA: 0x00151A08 File Offset: 0x0014FC08
	public static bool IsTraitActive(TraitType traitType)
	{
		if (traitType == TraitType.None)
		{
			return true;
		}
		if (!TraitManager.IsInitialized)
		{
			return false;
		}
		if (!TraitManager.Instance || TraitManager.Instance.m_activeTraitTypeList == null)
		{
			if (!TraitManager.Instance)
			{
				Debug.LogException(new Exception("IsTraitActive Error. Instance is Null even though IsInitialized == true"));
			}
			else
			{
				Debug.LogException(new Exception("IsTraitActive Error. Instance.m_activeTraitTypeList is Null."));
			}
			return false;
		}
		return TraitManager.Instance.m_activeTraitTypeList.Contains(traitType);
	}

	// Token: 0x06005877 RID: 22647 RVA: 0x00151A7C File Offset: 0x0014FC7C
	public static BaseTrait GetActiveTrait(TraitType traitType)
	{
		if (TraitManager.IsInitialized)
		{
			foreach (BaseTrait baseTrait in TraitManager.Instance.m_activeTraitsList)
			{
				if (baseTrait.TraitType == traitType)
				{
					return baseTrait;
				}
			}
		}
		return null;
	}

	// Token: 0x17001D8F RID: 7567
	// (get) Token: 0x06005878 RID: 22648 RVA: 0x0003012E File Offset: 0x0002E32E
	private static Dictionary<TraitType, SerializableVector3Int> TraitSpawnOddsTable
	{
		get
		{
			return SaveManager.PlayerSaveData.TraitSpawnOddsTable;
		}
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x0003013A File Offset: 0x0002E33A
	private void Awake()
	{
		if (!TraitManager.Instance)
		{
			TraitManager.Instance = this;
			this.Initialize();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x00151AE4 File Offset: 0x0014FCE4
	private void Initialize()
	{
		TraitManager.IsInitialized = true;
		this.m_onTraitsChanged = new Action<MonoBehaviour, EventArgs>(this.OnTraitsChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.TraitsChanged, this.m_onTraitsChanged);
		if (PlayerManager.IsInstantiated)
		{
			TraitType traitOne = SaveManager.PlayerSaveData.CurrentCharacter.TraitOne;
			TraitType traitTwo = SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo;
			this.OnTraitsChanged(this, new TraitChangedEventArgs(traitOne, traitTwo));
			return;
		}
		Debug.Log("<color=red>WARNING: TRAITMANAGER INITIALIZED BEFORE PLAYERMANAGER. THIS IS BAD.</color>");
	}

	// Token: 0x0600587B RID: 22651 RVA: 0x00151B58 File Offset: 0x0014FD58
	private void OnTraitsChanged(object sender, EventArgs args)
	{
		TraitChangedEventArgs traitChangedEventArgs = args as TraitChangedEventArgs;
		List<BaseTrait> list = new List<BaseTrait>(this.m_activeTraitsList);
		this.m_activeTraitsList.Clear();
		this.m_activeTraitTypeList.Clear();
		foreach (BaseTrait baseTrait in list)
		{
			UnityEngine.Object.DestroyImmediate(baseTrait.gameObject);
		}
		this.InstantiateTrait(traitChangedEventArgs.TraitOne, true);
		this.InstantiateTrait(traitChangedEventArgs.TraitTwo, false);
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x00151BEC File Offset: 0x0014FDEC
	private void InstantiateTrait(TraitType traitType, bool isTraitOne)
	{
		BaseTrait baseTrait = TraitLibrary.GetTrait(traitType);
		if (baseTrait)
		{
			baseTrait = UnityEngine.Object.Instantiate<BaseTrait>(baseTrait);
			baseTrait.transform.SetParent(base.transform);
			if (isTraitOne)
			{
				baseTrait.IsFirstTrait = true;
			}
			else
			{
				baseTrait.IsFirstTrait = false;
				baseTrait.AssignGreenMask();
			}
			this.m_activeTraitsList.Add(baseTrait);
			this.m_activeTraitTypeList.Add(traitType);
			if (PlayerManager.IsInstantiated)
			{
				PlayerManager.GetPlayerController().InitializeHealthMods();
				PlayerManager.GetPlayerController().InitializeManaMods();
			}
		}
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x00151C6C File Offset: 0x0014FE6C
	public static FoundState GetTraitFoundState(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (!TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			throw new KeyNotFoundException("Cannot GET trait found state [" + traitType.ToString() + "] because entry not found in Player Save Data.");
		}
		if (serializableVector3Int.z > 0)
		{
			return FoundState.Purchased;
		}
		return (FoundState)serializableVector3Int.z;
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x00151CBC File Offset: 0x0014FEBC
	public static void SetTraitFoundState(TraitType traitType, FoundState foundState)
	{
		if (!TraitManager.TraitSpawnOddsTable.ContainsKey(traitType))
		{
			throw new KeyNotFoundException("Cannot SET trait found state [" + traitType.ToString() + "] because entry not found in Player Save Data.");
		}
		Vector3Int rValue = TraitManager.TraitSpawnOddsTable[traitType];
		rValue.z = (int)foundState;
		TraitManager.TraitSpawnOddsTable[traitType] = rValue;
		if (foundState == FoundState.Purchased)
		{
			TraitManager.SetTraitUpgradeLevel(traitType, 1, false, false);
			return;
		}
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x00151D30 File Offset: 0x0014FF30
	public static TraitSpawnOdds GetTraitSpawnOdds(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return (TraitSpawnOdds)Mathf.Clamp(serializableVector3Int.x, 0, TraitManager.GetTraitUpgradeLevel(traitType));
		}
		throw new KeyNotFoundException("Cannot GET trait spawn odds [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06005880 RID: 22656 RVA: 0x00151D80 File Offset: 0x0014FF80
	public static void SetTraitSpawnOdds(TraitType traitType, int odds, bool additive)
	{
		if (!TraitManager.TraitSpawnOddsTable.ContainsKey(traitType))
		{
			throw new KeyNotFoundException("Cannot SET trait odds [" + traitType.ToString() + "] because entry not found in Player Save Data.");
		}
		Vector3Int rValue = TraitManager.TraitSpawnOddsTable[traitType];
		if (TraitManager.GetTraitFoundState(traitType) < FoundState.Purchased)
		{
			Debug.Log("<color=red>Cannot set trait spawn odds.  Trait has not been purchased.  Call SetTraitFoundState() first.</color>");
			return;
		}
		int num = rValue.x;
		if (additive)
		{
			num += odds;
		}
		else
		{
			num = odds;
		}
		num = Mathf.Clamp(num, 0, 3);
		rValue.x = num;
		TraitManager.TraitSpawnOddsTable[traitType] = rValue;
	}

	// Token: 0x06005881 RID: 22657 RVA: 0x00151E18 File Offset: 0x00150018
	public static void SetTraitSpawnOdds(TraitType traitType, TraitSpawnOdds traitSpawnOdds)
	{
		if (TraitManager.TraitSpawnOddsTable.ContainsKey(traitType))
		{
			Vector3Int rValue = TraitManager.TraitSpawnOddsTable[traitType];
			rValue.x = (int)traitSpawnOdds;
			TraitManager.TraitSpawnOddsTable[traitType] = rValue;
			return;
		}
		throw new KeyNotFoundException("Cannot SET trait spawn odds [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06005882 RID: 22658 RVA: 0x00151E80 File Offset: 0x00150080
	public static int GetTraitBlueprintLevel(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return serializableVector3Int.y;
		}
		throw new KeyNotFoundException("Cannot GET trait blueprint level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06005883 RID: 22659 RVA: 0x00151EC4 File Offset: 0x001500C4
	public static void SetTraitBlueprintLevel(TraitType traitType, int level, bool additive)
	{
		if (TraitManager.TraitSpawnOddsTable.ContainsKey(traitType))
		{
			Vector3Int rValue = TraitManager.TraitSpawnOddsTable[traitType];
			int num = rValue.y;
			if (additive)
			{
				num += level;
			}
			else
			{
				num = level;
			}
			rValue.y = Mathf.Clamp(num, 0, 3);
			TraitManager.TraitSpawnOddsTable[traitType] = rValue;
			return;
		}
		throw new KeyNotFoundException("Cannot SET trait blueprint level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06005884 RID: 22660 RVA: 0x00151F44 File Offset: 0x00150144
	public static int GetTraitUpgradeLevel(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return serializableVector3Int.z;
		}
		throw new KeyNotFoundException("Cannot GET trait upgrade level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x00151F88 File Offset: 0x00150188
	public static bool SetTraitUpgradeLevel(TraitType traitType, int level, bool additive, bool overrideValues)
	{
		if (!TraitManager.TraitSpawnOddsTable.ContainsKey(traitType))
		{
			throw new KeyNotFoundException("Cannot SET trait upgrade level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
		}
		if (TraitManager.GetTraitFoundState(traitType) != FoundState.Purchased && !overrideValues)
		{
			Debug.LogFormat("<color=red>TraitManager.SetTraitUpgradeLevel({0}) failed. First blueprint not purchased yet. Use overrideValues = true if intended or set Found State to Purchased via TraitManager.SetFoundState().</color>", new object[]
			{
				traitType
			});
			return false;
		}
		Vector3Int rValue = TraitManager.TraitSpawnOddsTable[traitType];
		int num = rValue.z;
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (num > TraitManager.GetTraitBlueprintLevel(traitType) && !overrideValues)
		{
			Debug.LogFormat("<color=red>TraitManager.SetTraitUpgradeLevel({0}) failed. Blueprint Found Level is less than desired Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				traitType
			});
			return false;
		}
		if (num < rValue.z && !overrideValues)
		{
			Debug.LogFormat("<color=red>TraitManager.SetTraitUpgradeLevel({0}) failed. New Upgrade Level less than current Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				traitType
			});
			return false;
		}
		rValue.z = Mathf.Clamp(num, 0, 3);
		TraitManager.TraitSpawnOddsTable[traitType] = rValue;
		return true;
	}

	// Token: 0x06005886 RID: 22662 RVA: 0x00030160 File Offset: 0x0002E360
	public static bool CanAfford(TraitType traitType)
	{
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= 0 && SaveManager.PlayerSaveData.EquipmentOreCollected >= 0;
	}

	// Token: 0x06005887 RID: 22663 RVA: 0x00152084 File Offset: 0x00150284
	public static bool CanPurchaseTrait(TraitType traitType)
	{
		FoundState traitFoundState = TraitManager.GetTraitFoundState(traitType);
		int traitBlueprintLevel = TraitManager.GetTraitBlueprintLevel(traitType);
		return traitFoundState > FoundState.NotFound && traitFoundState < FoundState.Purchased && traitBlueprintLevel > 0 && TraitManager.CanAfford(traitType);
	}

	// Token: 0x06005888 RID: 22664 RVA: 0x001520B8 File Offset: 0x001502B8
	public static bool CanUpgradeTrait(TraitType traitType)
	{
		bool traitFoundState = TraitManager.GetTraitFoundState(traitType) != FoundState.Purchased;
		int traitBlueprintLevel = TraitManager.GetTraitBlueprintLevel(traitType);
		int traitUpgradeLevel = TraitManager.GetTraitUpgradeLevel(traitType);
		return !traitFoundState && traitUpgradeLevel < traitBlueprintLevel && traitUpgradeLevel < 3 && TraitManager.CanAfford(traitType);
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x001520F0 File Offset: 0x001502F0
	public static TraitSeenState GetTraitSeenState(TraitType traitType)
	{
		if (traitType == TraitType.Antique)
		{
			return TraitSeenState.SeenTwice;
		}
		TraitSeenState result;
		if (SaveManager.PlayerSaveData.TraitSeenTable.TryGetValue(traitType, out result))
		{
			return result;
		}
		return TraitSeenState.NeverSeen;
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x0003017F File Offset: 0x0002E37F
	public static void SetTraitSeenState(TraitType traitType, TraitSeenState seenState, bool forceState)
	{
		if (SaveManager.PlayerSaveData.TraitSeenTable.ContainsKey(traitType) && (SaveManager.PlayerSaveData.TraitSeenTable[traitType] < seenState || forceState))
		{
			SaveManager.PlayerSaveData.TraitSeenTable[traitType] = seenState;
		}
	}

	// Token: 0x0600588B RID: 22667 RVA: 0x000301BA File Offset: 0x0002E3BA
	private void OnDestroy()
	{
		TraitManager.IsInitialized = false;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.TraitsChanged, this.m_onTraitsChanged);
	}

	// Token: 0x0400413E RID: 16702
	private List<BaseTrait> m_activeTraitsList = new List<BaseTrait>();

	// Token: 0x0400413F RID: 16703
	private List<TraitType> m_activeTraitTypeList = new List<TraitType>();

	// Token: 0x04004140 RID: 16704
	private Action<MonoBehaviour, EventArgs> m_onTraitsChanged;
}
