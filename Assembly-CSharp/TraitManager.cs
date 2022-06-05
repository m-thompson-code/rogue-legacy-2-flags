using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
public class TraitManager : MonoBehaviour
{
	// Token: 0x17001593 RID: 5523
	// (get) Token: 0x06003F3B RID: 16187 RVA: 0x000E1698 File Offset: 0x000DF898
	// (set) Token: 0x06003F3C RID: 16188 RVA: 0x000E169F File Offset: 0x000DF89F
	public static bool IsInitialized { get; private set; }

	// Token: 0x17001594 RID: 5524
	// (get) Token: 0x06003F3D RID: 16189 RVA: 0x000E16A7 File Offset: 0x000DF8A7
	// (set) Token: 0x06003F3E RID: 16190 RVA: 0x000E16AE File Offset: 0x000DF8AE
	public static TraitManager Instance { get; private set; }

	// Token: 0x17001595 RID: 5525
	// (get) Token: 0x06003F3F RID: 16191 RVA: 0x000E16B6 File Offset: 0x000DF8B6
	public static List<BaseTrait> ActiveTraitList
	{
		get
		{
			return TraitManager.Instance.m_activeTraitsList;
		}
	}

	// Token: 0x17001596 RID: 5526
	// (get) Token: 0x06003F40 RID: 16192 RVA: 0x000E16C2 File Offset: 0x000DF8C2
	public static List<TraitType> ActiveTraitTypeList
	{
		get
		{
			return TraitManager.Instance.m_activeTraitTypeList;
		}
	}

	// Token: 0x06003F41 RID: 16193 RVA: 0x000E16D0 File Offset: 0x000DF8D0
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

	// Token: 0x06003F42 RID: 16194 RVA: 0x000E171C File Offset: 0x000DF91C
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

	// Token: 0x06003F43 RID: 16195 RVA: 0x000E1790 File Offset: 0x000DF990
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

	// Token: 0x17001597 RID: 5527
	// (get) Token: 0x06003F44 RID: 16196 RVA: 0x000E17F8 File Offset: 0x000DF9F8
	private static Dictionary<TraitType, SerializableVector3Int> TraitSpawnOddsTable
	{
		get
		{
			return SaveManager.PlayerSaveData.TraitSpawnOddsTable;
		}
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x000E1804 File Offset: 0x000DFA04
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

	// Token: 0x06003F46 RID: 16198 RVA: 0x000E182C File Offset: 0x000DFA2C
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

	// Token: 0x06003F47 RID: 16199 RVA: 0x000E18A0 File Offset: 0x000DFAA0
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

	// Token: 0x06003F48 RID: 16200 RVA: 0x000E1934 File Offset: 0x000DFB34
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

	// Token: 0x06003F49 RID: 16201 RVA: 0x000E19B4 File Offset: 0x000DFBB4
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

	// Token: 0x06003F4A RID: 16202 RVA: 0x000E1A04 File Offset: 0x000DFC04
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

	// Token: 0x06003F4B RID: 16203 RVA: 0x000E1A78 File Offset: 0x000DFC78
	public static TraitSpawnOdds GetTraitSpawnOdds(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return (TraitSpawnOdds)Mathf.Clamp(serializableVector3Int.x, 0, TraitManager.GetTraitUpgradeLevel(traitType));
		}
		throw new KeyNotFoundException("Cannot GET trait spawn odds [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x000E1AC8 File Offset: 0x000DFCC8
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

	// Token: 0x06003F4D RID: 16205 RVA: 0x000E1B60 File Offset: 0x000DFD60
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

	// Token: 0x06003F4E RID: 16206 RVA: 0x000E1BC8 File Offset: 0x000DFDC8
	public static int GetTraitBlueprintLevel(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return serializableVector3Int.y;
		}
		throw new KeyNotFoundException("Cannot GET trait blueprint level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x000E1C0C File Offset: 0x000DFE0C
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

	// Token: 0x06003F50 RID: 16208 RVA: 0x000E1C8C File Offset: 0x000DFE8C
	public static int GetTraitUpgradeLevel(TraitType traitType)
	{
		SerializableVector3Int serializableVector3Int;
		if (TraitManager.TraitSpawnOddsTable.TryGetValue(traitType, out serializableVector3Int))
		{
			return serializableVector3Int.z;
		}
		throw new KeyNotFoundException("Cannot GET trait upgrade level [" + traitType.ToString() + "] because entry not found in Player Save Data.");
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x000E1CD0 File Offset: 0x000DFED0
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

	// Token: 0x06003F52 RID: 16210 RVA: 0x000E1DC9 File Offset: 0x000DFFC9
	public static bool CanAfford(TraitType traitType)
	{
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= 0 && SaveManager.PlayerSaveData.EquipmentOreCollected >= 0;
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x000E1DE8 File Offset: 0x000DFFE8
	public static bool CanPurchaseTrait(TraitType traitType)
	{
		FoundState traitFoundState = TraitManager.GetTraitFoundState(traitType);
		int traitBlueprintLevel = TraitManager.GetTraitBlueprintLevel(traitType);
		return traitFoundState > FoundState.NotFound && traitFoundState < FoundState.Purchased && traitBlueprintLevel > 0 && TraitManager.CanAfford(traitType);
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x000E1E1C File Offset: 0x000E001C
	public static bool CanUpgradeTrait(TraitType traitType)
	{
		bool traitFoundState = TraitManager.GetTraitFoundState(traitType) != FoundState.Purchased;
		int traitBlueprintLevel = TraitManager.GetTraitBlueprintLevel(traitType);
		int traitUpgradeLevel = TraitManager.GetTraitUpgradeLevel(traitType);
		return !traitFoundState && traitUpgradeLevel < traitBlueprintLevel && traitUpgradeLevel < 3 && TraitManager.CanAfford(traitType);
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x000E1E54 File Offset: 0x000E0054
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

	// Token: 0x06003F56 RID: 16214 RVA: 0x000E1E82 File Offset: 0x000E0082
	public static void SetTraitSeenState(TraitType traitType, TraitSeenState seenState, bool forceState)
	{
		if (SaveManager.PlayerSaveData.TraitSeenTable.ContainsKey(traitType) && (SaveManager.PlayerSaveData.TraitSeenTable[traitType] < seenState || forceState))
		{
			SaveManager.PlayerSaveData.TraitSeenTable[traitType] = seenState;
		}
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x000E1EBD File Offset: 0x000E00BD
	private void OnDestroy()
	{
		TraitManager.IsInitialized = false;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.TraitsChanged, this.m_onTraitsChanged);
	}

	// Token: 0x04002EF5 RID: 12021
	private List<BaseTrait> m_activeTraitsList = new List<BaseTrait>();

	// Token: 0x04002EF6 RID: 12022
	private List<TraitType> m_activeTraitTypeList = new List<TraitType>();

	// Token: 0x04002EF7 RID: 12023
	private Action<MonoBehaviour, EventArgs> m_onTraitsChanged;
}
