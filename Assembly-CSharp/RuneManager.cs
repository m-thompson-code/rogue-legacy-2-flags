using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006AA RID: 1706
public class RuneManager : MonoBehaviour
{
	// Token: 0x06003E8D RID: 16013 RVA: 0x000DC930 File Offset: 0x000DAB30
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x000DC938 File Offset: 0x000DAB38
	private void Initialize()
	{
		this.m_onRuneBlueprintsFoundChangedArgs = new RuneFoundLevelChangeEventArgs(RuneType.None, 0);
		this.m_onRuneEquippedLevelChangeArgs = new RuneEquippedLevelChangeEventArgs(RuneType.None, 0);
		this.m_onRunePurchaseLevelChangeArgs = new RunePurchaseLevelChangeEventArgs(RuneType.None, 0);
		this.m_onRuneFoundStateChangeArgs = new RuneFoundStateChangeEventArgs(RuneType.None, FoundState.NotFound);
		if (!SaveManager.EquipmentSaveData.IsInitialized)
		{
			SaveManager.EquipmentSaveData.Initialize();
		}
		Array.Sort<RuneType>(RuneType_RL.TypeArray, delegate(RuneType a, RuneType b)
		{
			if (a == RuneType.None || b == RuneType.None)
			{
				return 0;
			}
			return RuneLibrary.GetRuneData(a).Location.CompareTo(RuneLibrary.GetRuneData(b).Location);
		});
		RuneManager.m_isInitialized = true;
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x000DC9BF File Offset: 0x000DABBF
	private void OnDestroy()
	{
		RuneManager.m_isDisposed = true;
	}

	// Token: 0x17001576 RID: 5494
	// (get) Token: 0x06003E90 RID: 16016 RVA: 0x000DC9C7 File Offset: 0x000DABC7
	public static RuneManager Instance
	{
		get
		{
			bool isDisposed = RuneManager.m_isDisposed;
			bool isInitialized = RuneManager.m_isInitialized;
			if (RuneManager.m_runeManager == null)
			{
				RuneManager.m_runeManager = CDGHelper.FindStaticInstance<RuneManager>(false);
			}
			return RuneManager.m_runeManager;
		}
	}

	// Token: 0x17001577 RID: 5495
	// (get) Token: 0x06003E91 RID: 16017 RVA: 0x000DC9F2 File Offset: 0x000DABF2
	public static bool IsInitialized
	{
		get
		{
			return RuneManager.m_isInitialized;
		}
	}

	// Token: 0x17001578 RID: 5496
	// (get) Token: 0x06003E92 RID: 16018 RVA: 0x000DC9F9 File Offset: 0x000DABF9
	public static bool IsDisposed
	{
		get
		{
			return RuneManager.m_isDisposed;
		}
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x000DCA00 File Offset: 0x000DAC00
	public static bool DoesRuneExist(RuneType runeType)
	{
		return RuneManager.GetRune(runeType) != null;
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x000DCA0C File Offset: 0x000DAC0C
	public static bool CanAfford(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= rune.GoldCostToUpgrade && SaveManager.PlayerSaveData.RuneOreCollected >= rune.OreCostToUpgrade;
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x000DCA48 File Offset: 0x000DAC48
	public static bool CanPurchaseRune(RuneType runeType, bool suppressLogs = true)
	{
		if (!RuneManager.DoesRuneExist(runeType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanPurchaseRune({0}) returned false because rune obj could not be found.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		FoundState foundState = RuneManager.GetFoundState(runeType);
		if (foundState <= FoundState.NotFound)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanPurchaseRune({0}) returned false because rune found state is 'Not Found'.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		if (foundState == FoundState.Purchased)
		{
			int upgradeLevel = RuneManager.GetUpgradeLevel(runeType);
			int upgradeBlueprintsFound = RuneManager.GetUpgradeBlueprintsFound(runeType, false);
			if (upgradeLevel >= upgradeBlueprintsFound)
			{
				if (!suppressLogs)
				{
					Debug.LogFormat("<color=yellow>RuneManager.CanPurchaseRune({0}) returned false because Blueprints Found level is not high enough.</color>", new object[]
					{
						runeType
					});
				}
				return false;
			}
		}
		if (!RuneManager.CanAfford(runeType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanPurchaseRune({0}) returned false because rune could not be afforded.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x000DCB00 File Offset: 0x000DAD00
	public static bool CanEquip(RuneType runeType, bool suppressLogs = true)
	{
		if (!RuneManager.DoesRuneExist(runeType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanEquip({0}) returned false because rune obj could not be found.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		if (RuneManager.GetFoundState(runeType) != FoundState.Purchased)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanEquip({0}) returned false because rune has not been purchased yet.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(runeType);
		int upgradeLevel = RuneManager.GetUpgradeLevel(runeType);
		if (runeEquippedLevel >= upgradeLevel)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanEquip({0}) returned false because player has already equipped all runes they own of this type.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		RuneObj rune = RuneManager.GetRune(runeType);
		int totalEquippedWeight = RuneManager.GetTotalEquippedWeight();
		int num = 10000;
		if (PlayerManager.IsInstantiated)
		{
			num = PlayerManager.GetPlayerController().ActualRuneWeight;
		}
		int num2 = rune.GetWeightAtLevel(rune.EquippedLevel + 1) - rune.GetWeightAtLevel(rune.EquippedLevel);
		if (totalEquippedWeight + num2 > num)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>RuneManager.CanEquip({0}) returned false because resulting Rune Weight exceeds allowed Rune Weight.</color>", new object[]
				{
					runeType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x000DCBEB File Offset: 0x000DADEB
	public static List<RuneType> GetAllRunesWithFoundState(FoundState foundState)
	{
		RuneManager.GetAllRunesWithFoundState(foundState, RuneManager.m_runeTypeListHelper);
		return RuneManager.m_runeTypeListHelper;
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x000DCC00 File Offset: 0x000DAE00
	public static void GetAllRunesWithFoundState(FoundState foundState, List<RuneType> results)
	{
		results.Clear();
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in SaveManager.EquipmentSaveData.RuneDict)
		{
			RuneObj value = keyValuePair.Value;
			if (value.FoundState == foundState)
			{
				results.Add(value.RuneType);
			}
		}
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x000DCC74 File Offset: 0x000DAE74
	public static int GetTotalEquippedWeight()
	{
		int num = 0;
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneObj rune = RuneManager.GetRune(runeType);
				if (rune != null && rune.EquippedLevel > 0)
				{
					num += rune.CurrentWeight;
				}
			}
		}
		return num;
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x000DCCC0 File Offset: 0x000DAEC0
	public static FoundState GetFoundState(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		FoundState result;
		if (rune != null)
		{
			result = rune.FoundState;
		}
		else
		{
			result = FoundState.NotFound;
		}
		return result;
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x000DCCE8 File Offset: 0x000DAEE8
	public static bool SetFoundState(RuneType runeType, FoundState foundState, bool overrideValues, bool runEvents = true)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune == null)
		{
			Debug.LogFormat("<color=red>Could not set rune {0} to FoundState: {1}. Rune obj not found.</color>", new object[]
			{
				runeType,
				foundState
			});
			return false;
		}
		FoundState foundState2 = RuneManager.GetFoundState(runeType);
		if (foundState > foundState2 || overrideValues)
		{
			rune.UpgradeLevel = (int)foundState;
			if (runEvents)
			{
				RuneManager.Instance.m_onRuneFoundStateChangeArgs.Initialize(runeType, foundState);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.RuneFoundStateChanged, RuneManager.Instance, RuneManager.Instance.m_onRuneFoundStateChangeArgs);
			}
			return true;
		}
		Debug.LogFormat("<color=red>Could not set rune {0} to FoundState: {1}. Current FoundState: {2} is a higher value. Please set overrideValues = true to override this if intended.</color>", new object[]
		{
			runeType,
			foundState,
			RuneManager.GetFoundState(runeType)
		});
		return false;
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x000DCD98 File Offset: 0x000DAF98
	public static int GetUpgradeBlueprintsFound(RuneType runeType, bool ignoreInfinitePurchasePower = false)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		int upgradeBlueprintsFound = rune.UpgradeBlueprintsFound;
		if (rune != null)
		{
			return Mathf.Clamp(upgradeBlueprintsFound, 0, rune.MaxLevel);
		}
		return 0;
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x000DCDC8 File Offset: 0x000DAFC8
	public static bool SetUpgradeBlueprintsFound(RuneType runeType, int level, bool additive, bool runEvents = true)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune == null)
		{
			return false;
		}
		int num = rune.UpgradeBlueprintsFound;
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (num > rune.MaxLevel)
		{
			Debug.Log("<color=red>RuneManager.SetUpgradeBlueprintsFound(" + runeType.ToString() + ") failed.  Upgrading will put blueprints found beyond max level.</color>");
			return false;
		}
		if (num > 0 && RuneManager.GetFoundState(runeType) < FoundState.FoundButNotViewed)
		{
			RuneManager.SetFoundState(runeType, FoundState.FoundButNotViewed, false, true);
		}
		rune.UpgradeBlueprintsFound = Mathf.Clamp(num, 0, rune.MaxLevel);
		if (runEvents)
		{
			RuneManager.Instance.m_onRuneBlueprintsFoundChangedArgs.Initialize(runeType, rune.UpgradeBlueprintsFound);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.RuneBlueprintsFoundChanged, RuneManager.Instance, RuneManager.Instance.m_onRuneBlueprintsFoundChangedArgs);
		}
		return true;
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x000DCE80 File Offset: 0x000DB080
	public static int GetUpgradeLevel(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune != null)
		{
			return rune.ClampedUpgradeLevel;
		}
		return 0;
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x000DCEA0 File Offset: 0x000DB0A0
	public static bool SetRuneUpgradeLevel(RuneType runeType, int level, bool additive, bool overrideValues, bool runEvents = true)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune == null)
		{
			Debug.LogFormat("<color=red>RuneManager.SetRuneUpgradeLevel({0}) failed. Rune obj not found.</color>", new object[]
			{
				runeType
			});
			return false;
		}
		int num = RuneManager.GetUpgradeLevel(runeType);
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (RuneManager.GetFoundState(runeType) != FoundState.Purchased && !overrideValues)
		{
			Debug.LogFormat("<color=red>RuneManager.SetRuneUpgradeLevel({0}) failed. First blueprint not purchased yet. Use overrideValues = true if intended or set Found State to Purchased via RuneManager.SetFoundState().</color>", new object[]
			{
				runeType
			});
			return false;
		}
		if (num > RuneManager.GetUpgradeBlueprintsFound(runeType, false) && !overrideValues)
		{
			Debug.LogFormat("<color=red>RuneManager.SetRuneUpgradeLevel({0}) failed. Blueprint Found Level is less than desired Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				runeType
			});
			return false;
		}
		if (num < RuneManager.GetUpgradeLevel(runeType) && !overrideValues)
		{
			Debug.LogFormat("<color=red>RuneManager.SetRuneUpgradeLevel({0}) failed. New Upgrade Level less than current Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				runeType
			});
			return false;
		}
		rune.UpgradeLevel = Mathf.Clamp(num, 0, RuneManager.GetUpgradeBlueprintsFound(runeType, false));
		if (runEvents)
		{
			RuneManager.Instance.m_onRunePurchaseLevelChangeArgs.Initialize(runeType, rune.UpgradeLevel);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.RunePurchaseLevelChanged, RuneManager.Instance, RuneManager.Instance.m_onRunePurchaseLevelChangeArgs);
		}
		return true;
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x000DCFA4 File Offset: 0x000DB1A4
	public static int GetRuneEquippedLevel(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune != null)
		{
			return Mathf.Clamp(rune.EquippedLevel, 0, rune.ClampedUpgradeLevel);
		}
		return 0;
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x000DCFD0 File Offset: 0x000DB1D0
	public static bool SetRuneEquippedLevel(RuneType runeType, int level, bool additive, bool runEvents = true)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune == null)
		{
			return false;
		}
		int num = rune.EquippedLevel;
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		num = Mathf.Clamp(num, 0, rune.ClampedUpgradeLevel);
		if (num != rune.EquippedLevel)
		{
			rune.EquippedLevel = num;
			if (runEvents)
			{
				RuneManager.Instance.m_onRuneEquippedLevelChangeArgs.Initialize(runeType, rune.EquippedLevel);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.RuneEquippedLevelChanged, RuneManager.Instance, RuneManager.Instance.m_onRuneEquippedLevelChangeArgs);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x000DD04C File Offset: 0x000DB24C
	public static float GetMinRuneLevelScale()
	{
		int num = 0;
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in SaveManager.EquipmentSaveData.RuneDict)
		{
			num += RuneManager.GetUpgradeBlueprintsFound(keyValuePair.Key, false);
		}
		num -= 2;
		return (float)num * 1.25f;
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x000DD0BC File Offset: 0x000DB2BC
	public static int GetTotalAvailableRuneBlueprints(List<RuneObj> populatedList = null)
	{
		if (populatedList != null)
		{
			populatedList.Clear();
		}
		int num = 0;
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneObj rune = RuneManager.GetRune(runeType);
				if (rune != null && !rune.RuneData.Disabled)
				{
					num++;
					if (populatedList != null)
					{
						populatedList.Add(rune);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x000DD11C File Offset: 0x000DB31C
	public static RuneObj GetRune(RuneType runeType)
	{
		RuneObj result;
		if (SaveManager.EquipmentSaveData.RuneDict.TryGetValue(runeType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x04002E8E RID: 11918
	private static bool m_isDisposed;

	// Token: 0x04002E8F RID: 11919
	private static bool m_isInitialized;

	// Token: 0x04002E90 RID: 11920
	private RuneFoundLevelChangeEventArgs m_onRuneBlueprintsFoundChangedArgs;

	// Token: 0x04002E91 RID: 11921
	private RuneEquippedLevelChangeEventArgs m_onRuneEquippedLevelChangeArgs;

	// Token: 0x04002E92 RID: 11922
	private RunePurchaseLevelChangeEventArgs m_onRunePurchaseLevelChangeArgs;

	// Token: 0x04002E93 RID: 11923
	private RuneFoundStateChangeEventArgs m_onRuneFoundStateChangeArgs;

	// Token: 0x04002E94 RID: 11924
	private static RuneManager m_runeManager = null;

	// Token: 0x04002E95 RID: 11925
	private static List<RuneType> m_runeTypeListHelper = new List<RuneType>();
}
