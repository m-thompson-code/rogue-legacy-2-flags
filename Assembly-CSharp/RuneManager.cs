using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
public class RuneManager : MonoBehaviour
{
	// Token: 0x06005788 RID: 22408 RVA: 0x0002FA82 File Offset: 0x0002DC82
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x0014CCFC File Offset: 0x0014AEFC
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

	// Token: 0x0600578A RID: 22410 RVA: 0x0002FA8A File Offset: 0x0002DC8A
	private void OnDestroy()
	{
		RuneManager.m_isDisposed = true;
	}

	// Token: 0x17001D60 RID: 7520
	// (get) Token: 0x0600578B RID: 22411 RVA: 0x0002FA92 File Offset: 0x0002DC92
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

	// Token: 0x17001D61 RID: 7521
	// (get) Token: 0x0600578C RID: 22412 RVA: 0x0002FABD File Offset: 0x0002DCBD
	public static bool IsInitialized
	{
		get
		{
			return RuneManager.m_isInitialized;
		}
	}

	// Token: 0x17001D62 RID: 7522
	// (get) Token: 0x0600578D RID: 22413 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
	public static bool IsDisposed
	{
		get
		{
			return RuneManager.m_isDisposed;
		}
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x0002FACB File Offset: 0x0002DCCB
	public static bool DoesRuneExist(RuneType runeType)
	{
		return RuneManager.GetRune(runeType) != null;
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x0014CD84 File Offset: 0x0014AF84
	public static bool CanAfford(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= rune.GoldCostToUpgrade && SaveManager.PlayerSaveData.RuneOreCollected >= rune.OreCostToUpgrade;
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x0014CDC0 File Offset: 0x0014AFC0
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

	// Token: 0x06005791 RID: 22417 RVA: 0x0014CE78 File Offset: 0x0014B078
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

	// Token: 0x06005792 RID: 22418 RVA: 0x0002FAD6 File Offset: 0x0002DCD6
	public static List<RuneType> GetAllRunesWithFoundState(FoundState foundState)
	{
		RuneManager.GetAllRunesWithFoundState(foundState, RuneManager.m_runeTypeListHelper);
		return RuneManager.m_runeTypeListHelper;
	}

	// Token: 0x06005793 RID: 22419 RVA: 0x0014CF64 File Offset: 0x0014B164
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

	// Token: 0x06005794 RID: 22420 RVA: 0x0014CFD8 File Offset: 0x0014B1D8
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

	// Token: 0x06005795 RID: 22421 RVA: 0x0014D024 File Offset: 0x0014B224
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

	// Token: 0x06005796 RID: 22422 RVA: 0x0014D04C File Offset: 0x0014B24C
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

	// Token: 0x06005797 RID: 22423 RVA: 0x0014D0FC File Offset: 0x0014B2FC
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

	// Token: 0x06005798 RID: 22424 RVA: 0x0014D12C File Offset: 0x0014B32C
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

	// Token: 0x06005799 RID: 22425 RVA: 0x0014D1E4 File Offset: 0x0014B3E4
	public static int GetUpgradeLevel(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune != null)
		{
			return rune.ClampedUpgradeLevel;
		}
		return 0;
	}

	// Token: 0x0600579A RID: 22426 RVA: 0x0014D204 File Offset: 0x0014B404
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

	// Token: 0x0600579B RID: 22427 RVA: 0x0014D308 File Offset: 0x0014B508
	public static int GetRuneEquippedLevel(RuneType runeType)
	{
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune != null)
		{
			return Mathf.Clamp(rune.EquippedLevel, 0, rune.ClampedUpgradeLevel);
		}
		return 0;
	}

	// Token: 0x0600579C RID: 22428 RVA: 0x0014D334 File Offset: 0x0014B534
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

	// Token: 0x0600579D RID: 22429 RVA: 0x0014D3B0 File Offset: 0x0014B5B0
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

	// Token: 0x0600579E RID: 22430 RVA: 0x0014D420 File Offset: 0x0014B620
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

	// Token: 0x0600579F RID: 22431 RVA: 0x0014D480 File Offset: 0x0014B680
	public static RuneObj GetRune(RuneType runeType)
	{
		RuneObj result;
		if (SaveManager.EquipmentSaveData.RuneDict.TryGetValue(runeType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x040040B0 RID: 16560
	private static bool m_isDisposed;

	// Token: 0x040040B1 RID: 16561
	private static bool m_isInitialized;

	// Token: 0x040040B2 RID: 16562
	private RuneFoundLevelChangeEventArgs m_onRuneBlueprintsFoundChangedArgs;

	// Token: 0x040040B3 RID: 16563
	private RuneEquippedLevelChangeEventArgs m_onRuneEquippedLevelChangeArgs;

	// Token: 0x040040B4 RID: 16564
	private RunePurchaseLevelChangeEventArgs m_onRunePurchaseLevelChangeArgs;

	// Token: 0x040040B5 RID: 16565
	private RuneFoundStateChangeEventArgs m_onRuneFoundStateChangeArgs;

	// Token: 0x040040B6 RID: 16566
	private static RuneManager m_runeManager = null;

	// Token: 0x040040B7 RID: 16567
	private static List<RuneType> m_runeTypeListHelper = new List<RuneType>();
}
