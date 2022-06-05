using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B29 RID: 2857
public class EquipmentManager : MonoBehaviour
{
	// Token: 0x06005637 RID: 22071 RVA: 0x0002EE13 File Offset: 0x0002D013
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06005638 RID: 22072 RVA: 0x001469E0 File Offset: 0x00144BE0
	private void Initialize()
	{
		if (Application.isPlaying)
		{
			this.m_foundLevelChangeArgs = new EquipmentFoundLevelChangeEventArgs(EquipmentCategoryType.None, EquipmentType.None, 0);
			this.m_purchasedLevelChangeArgs = new EquipmentPurchasedLevelChangeEventArgs(EquipmentCategoryType.None, EquipmentType.None, 0);
			this.m_onFoundStateChangeArgs = new EquipmentFoundStateChangeEventArgs(EquipmentCategoryType.None, EquipmentType.None, FoundState.NotFound);
			this.m_onEquippedChangeArgs = new EquippedChangeEventArgs(EquipmentCategoryType.None, EquipmentType.None);
			if (!SaveManager.EquipmentSaveData.IsInitialized)
			{
				SaveManager.EquipmentSaveData.Initialize();
			}
			EquipmentManager.m_isInitialized = true;
		}
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x0002EE1B File Offset: 0x0002D01B
	private void OnDestroy()
	{
		EquipmentManager.m_isDisposed = true;
	}

	// Token: 0x17001D1D RID: 7453
	// (get) Token: 0x0600563A RID: 22074 RVA: 0x0002EE23 File Offset: 0x0002D023
	private static EquipmentManager Instance
	{
		get
		{
			bool isDisposed = EquipmentManager.m_isDisposed;
			bool isInitialized = EquipmentManager.m_isInitialized;
			if (EquipmentManager.m_equipmentManager == null)
			{
				EquipmentManager.m_equipmentManager = CDGHelper.FindStaticInstance<EquipmentManager>(false);
			}
			return EquipmentManager.m_equipmentManager;
		}
	}

	// Token: 0x17001D1E RID: 7454
	// (get) Token: 0x0600563B RID: 22075 RVA: 0x0002EE4E File Offset: 0x0002D04E
	public static bool IsInitialized
	{
		get
		{
			return EquipmentManager.m_isInitialized;
		}
	}

	// Token: 0x17001D1F RID: 7455
	// (get) Token: 0x0600563C RID: 22076 RVA: 0x0002EE55 File Offset: 0x0002D055
	public static bool IsDisposed
	{
		get
		{
			return EquipmentManager.m_isDisposed;
		}
	}

	// Token: 0x0600563D RID: 22077 RVA: 0x0002EE5C File Offset: 0x0002D05C
	public static bool DoesEquipmentExist(EquipmentCategoryType category, EquipmentType equipType)
	{
		return EquipmentManager.GetEquipment(category, equipType) != null;
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x00146A48 File Offset: 0x00144C48
	public static List<EquipmentType> GetAllEquipmentWithFoundState(EquipmentCategoryType category, FoundState foundState)
	{
		List<EquipmentType> list = new List<EquipmentType>();
		EquipmentManager.GetAllEquipmentWithFoundState(category, foundState, list);
		return list;
	}

	// Token: 0x0600563F RID: 22079 RVA: 0x00146A64 File Offset: 0x00144C64
	public static void GetAllEquipmentWithFoundState(EquipmentCategoryType category, FoundState foundState, List<EquipmentType> results)
	{
		results.Clear();
		foreach (KeyValuePair<EquipmentType, EquipmentObj> keyValuePair in EquipmentManager.GetEquipmentDict(category))
		{
			EquipmentObj value = keyValuePair.Value;
			if (value.FoundState == foundState)
			{
				results.Add(value.EquipmentType);
			}
		}
	}

	// Token: 0x06005640 RID: 22080 RVA: 0x00146AD4 File Offset: 0x00144CD4
	public static bool CanAfford(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= equipment.GoldCostToUpgrade && SaveManager.PlayerSaveData.EquipmentOreCollected >= equipment.OreCostToUpgrade;
	}

	// Token: 0x06005641 RID: 22081 RVA: 0x00146B10 File Offset: 0x00144D10
	public static bool CanPurchaseEquipment(EquipmentCategoryType category, EquipmentType equipType, bool suppressLogs = true)
	{
		if (!EquipmentManager.DoesEquipmentExist(category, equipType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanPurchaseEquipment({0}, {1}) returned false because equipment obj could not be found.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		FoundState foundState = EquipmentManager.GetFoundState(category, equipType);
		if (foundState <= FoundState.NotFound)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanPurchaseEquipment({0}, {1}) returned false because equipment found state is 'Not Found'.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		if (foundState == FoundState.Purchased)
		{
			int upgradeLevel = EquipmentManager.GetUpgradeLevel(category, equipType);
			int upgradeBlueprintsFound = EquipmentManager.GetUpgradeBlueprintsFound(category, equipType, false);
			if (upgradeLevel >= upgradeBlueprintsFound)
			{
				if (!suppressLogs)
				{
					Debug.LogFormat("<color=yellow>EquipmentManager.CanPurchaseEquipment({0}, {1}) returned false because Blueprints Found level is not high enough.</color>", new object[]
					{
						category,
						equipType
					});
				}
				return false;
			}
		}
		if (!EquipmentManager.CanAfford(category, equipType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanPurchaseEquipment({0}, {1}) returned false because equipment could not be afforded.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x06005642 RID: 22082 RVA: 0x00146BF0 File Offset: 0x00144DF0
	public static bool CanEquip(EquipmentCategoryType category, EquipmentType equipType, bool suppressLogs = true)
	{
		if (!EquipmentManager.DoesEquipmentExist(category, equipType))
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanEquip({0}, {1}) returned false because equipment obj could not be found.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		if (EquipmentManager.GetFoundState(category, equipType) != FoundState.Purchased)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanEquip({0}, {1}) returned false because equipment has not been purchased yet.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		int num = (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight);
		EquipmentObj equipped = EquipmentManager.GetEquipped(category);
		if (equipped != null)
		{
			num -= (int)equipped.GetCurrentStatValue(EquipmentStatType.Weight);
			num = Mathf.Clamp(num, 0, int.MaxValue);
		}
		int num2 = 10000;
		if (PlayerManager.IsInstantiated)
		{
			num2 = PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight;
		}
		if (num + (int)equipment.GetCurrentStatValue(EquipmentStatType.Weight) > num2)
		{
			if (!suppressLogs)
			{
				Debug.LogFormat("<color=yellow>EquipmentManager.CanEquip({0}, {1}) returned false because resulting Equip Weight exceeds allowed Equip Weight.</color>", new object[]
				{
					category,
					equipType
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x06005643 RID: 22083 RVA: 0x00146CDC File Offset: 0x00144EDC
	public static int Get_EquipmentSet_TotalEquippedLevel(EquipmentType equipType)
	{
		int num = 0;
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				EquipmentObj equipped = EquipmentManager.GetEquipped(equipmentCategoryType);
				if (equipped != null && equipped.EquipmentType == equipType)
				{
					num += equipped.EquipmentSetLevel;
				}
			}
		}
		return num;
	}

	// Token: 0x06005644 RID: 22084 RVA: 0x00146D28 File Offset: 0x00144F28
	public static float Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType bonusType)
	{
		EquipmentManager.m_equipTypesUsedHelper.Clear();
		float num = 0f;
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				EquipmentObj equipped = EquipmentManager.GetEquipped(equipmentCategoryType);
				if (equipped != null)
				{
					EquipmentType equipmentType = equipped.EquipmentType;
					if (!EquipmentManager.m_equipTypesUsedHelper.Contains(equipmentType))
					{
						EquipmentManager.m_equipTypesUsedHelper.Add(equipmentType);
						EquipmentSetData equipmentSetData = EquipmentSetLibrary.GetEquipmentSetData(equipmentType);
						int num2 = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType);
						if (num2 >= equipmentSetData.SetRequirement01 && equipmentSetData.SetBonus01.BonusType == bonusType)
						{
							num += equipmentSetData.SetBonus01.StatGain;
						}
						if (num2 >= equipmentSetData.SetRequirement02 && equipmentSetData.SetBonus02.BonusType == bonusType)
						{
							num += equipmentSetData.SetBonus02.StatGain;
						}
						if (num2 >= equipmentSetData.SetRequirement03 && equipmentSetData.SetBonus03.BonusType == bonusType)
						{
							num += equipmentSetData.SetBonus03.StatGain;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06005645 RID: 22085 RVA: 0x0002EE68 File Offset: 0x0002D068
	public static int Get_EquipmentSet_CurrentLevel(EquipmentType equipType)
	{
		if (!SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			return 0;
		}
		return SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType].x;
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x00146E28 File Offset: 0x00145028
	public static void Set_EquipmentSet_CurrentLevel(EquipmentType equipType, int level, bool additive)
	{
		if (SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			int num = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipType);
			int x = additive ? (num + level) : level;
			SerializableVector2Int value = SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType];
			value.x = x;
			SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType] = value;
		}
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x0002EE93 File Offset: 0x0002D093
	public static int Get_EquipmentSet_LevelViewed(EquipmentType equipType)
	{
		if (!SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			return 0;
		}
		return SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType].y;
	}

	// Token: 0x06005648 RID: 22088 RVA: 0x00146E84 File Offset: 0x00145084
	public static void Set_EquipmentSet_LevelViewed(EquipmentType equipType, int level, bool additive)
	{
		if (SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			int num = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipType);
			int y = additive ? (num + level) : level;
			SerializableVector2Int value = SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType];
			value.y = y;
			SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType] = value;
		}
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x00146EE0 File Offset: 0x001450E0
	public static int GetUpgradeBlueprintsFound(EquipmentCategoryType category, EquipmentType equipType, bool ignoreInfinitePurchasePower = false)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment != null)
		{
			return Mathf.Clamp(equipment.UpgradeBlueprintsFound, 0, equipment.MaxLevel);
		}
		return 0;
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x00146F0C File Offset: 0x0014510C
	public static bool SetUpgradeBlueprintsFound(EquipmentCategoryType category, EquipmentType equipType, int level, bool additive, bool runEvents = true)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment == null)
		{
			return false;
		}
		int num = equipment.UpgradeBlueprintsFound;
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (num > equipment.MaxLevel)
		{
			Debug.Log(string.Concat(new string[]
			{
				"<color=red>EquipmentManager.SetUpgradeBlueprintsFound(",
				category.ToString(),
				", ",
				equipType.ToString(),
				") failed.  Upgrading will put blueprints found beyond max level.</color>"
			}));
			return false;
		}
		equipment.UpgradeBlueprintsFound = Mathf.Clamp(num, 0, equipment.MaxLevel);
		if (equipment.FoundState == FoundState.NotFound && equipment.UpgradeBlueprintsFound > 0)
		{
			EquipmentManager.SetFoundState(category, equipType, FoundState.FoundButNotViewed, false, runEvents);
		}
		if (runEvents)
		{
			EquipmentManager.Instance.m_foundLevelChangeArgs.Initialize(category, equipType, equipment.UpgradeBlueprintsFound);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.EquipmentFoundLevelChanged, EquipmentManager.Instance, EquipmentManager.Instance.m_foundLevelChangeArgs);
		}
		return true;
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x00146FF8 File Offset: 0x001451F8
	public static int GetUpgradeLevel(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment != null)
		{
			return equipment.ClampedUpgradeLevel;
		}
		return 0;
	}

	// Token: 0x0600564C RID: 22092 RVA: 0x00147018 File Offset: 0x00145218
	public static bool SetEquipmentUpgradeLevel(EquipmentCategoryType category, EquipmentType equipType, int level, bool additive, bool overrideValues, bool runEvents = true)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment == null)
		{
			Debug.LogFormat("<color=red>EquipmentManager.SetEquipmentUpgradeLevel({0}, {1}) failed. Equipment obj not found.</color>", new object[]
			{
				category,
				equipType
			});
			return false;
		}
		int num = EquipmentManager.GetUpgradeLevel(category, equipType);
		if (additive)
		{
			num += level;
		}
		else
		{
			num = level;
		}
		if (EquipmentManager.GetFoundState(category, equipType) != FoundState.Purchased && !overrideValues)
		{
			Debug.LogFormat("<color=red>EquipmentManager.SetEquipmentUpgradeLevel({0}, {1}) failed. First blueprint not purchased yet. Use overrideValues = true if intended or set Found State to Purchased via EquipmentManager.SetFoundState().</color>", new object[]
			{
				category,
				equipType
			});
			return false;
		}
		if (num > EquipmentManager.GetUpgradeBlueprintsFound(category, equipType, false) && !overrideValues)
		{
			Debug.LogFormat("<color=red>EquipmentManager.SetEquipmentUpgradeLevel({0}, {1}) failed. Blueprint Found Level is less than desired Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				category,
				equipType
			});
			return false;
		}
		if (num < EquipmentManager.GetUpgradeLevel(category, equipType) && !overrideValues)
		{
			Debug.LogFormat("<color=red>EquipmentManager.SetEquipmentUpgradeLevel({0}, {1}) failed. New Upgrade Level less than current Upgrade Level. Use overrideValues = true if intended.</color>", new object[]
			{
				category,
				equipType
			});
			return false;
		}
		equipment.UpgradeLevel = Mathf.Clamp(num, 0, EquipmentManager.GetUpgradeBlueprintsFound(category, equipType, false));
		if (runEvents)
		{
			EquipmentManager.Instance.m_purchasedLevelChangeArgs.Initialize(category, equipType, equipment.UpgradeLevel);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.EquipmentPurchasedLevelChanged, EquipmentManager.Instance, EquipmentManager.Instance.m_purchasedLevelChangeArgs);
		}
		return true;
	}

	// Token: 0x0600564D RID: 22093 RVA: 0x00147148 File Offset: 0x00145348
	public static FoundState GetFoundState(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		FoundState foundState;
		if (equipment != null)
		{
			foundState = equipment.FoundState;
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

	// Token: 0x0600564E RID: 22094 RVA: 0x00147178 File Offset: 0x00145378
	public static bool SetFoundState(EquipmentCategoryType category, EquipmentType equipType, FoundState foundState, bool overrideValues, bool runEvents = true)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment == null)
		{
			Debug.LogFormat("<color=red>Could not set equipment {0} - {1} to FoundState: {2}. Equipment obj not found.</color>", new object[]
			{
				category,
				equipType,
				foundState
			});
			return false;
		}
		FoundState foundState2 = EquipmentManager.GetFoundState(category, equipType);
		if (foundState > foundState2 || overrideValues)
		{
			equipment.UpgradeLevel = (int)foundState;
			if (runEvents)
			{
				EquipmentManager.Instance.m_onFoundStateChangeArgs.Initialize(category, equipType, foundState);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.EquipmentFoundStateChanged, EquipmentManager.Instance, EquipmentManager.Instance.m_onFoundStateChangeArgs);
			}
			return true;
		}
		Debug.LogFormat("<color=red>Could not set equipment {0} - {1} to FoundState: {2}. Current FoundState: {3} is a higher value. Please set overrideValues = true to override this if intended.</color>", new object[]
		{
			category,
			equipType,
			foundState,
			EquipmentManager.GetFoundState(category, equipType)
		});
		return false;
	}

	// Token: 0x0600564F RID: 22095 RVA: 0x0014723C File Offset: 0x0014543C
	public static bool IsEquipped(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(category);
		return equipped != null && equipped.EquipmentType == equipType;
	}

	// Token: 0x06005650 RID: 22096 RVA: 0x00147260 File Offset: 0x00145460
	public static EquipmentObj GetEquipped(EquipmentCategoryType category)
	{
		switch (category)
		{
		case EquipmentCategoryType.Weapon:
			return EquipmentManager.GetEquipment(category, SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType);
		case EquipmentCategoryType.Head:
			return EquipmentManager.GetEquipment(category, SaveManager.PlayerSaveData.CurrentCharacter.HeadEquipmentType);
		case EquipmentCategoryType.Chest:
			return EquipmentManager.GetEquipment(category, SaveManager.PlayerSaveData.CurrentCharacter.ChestEquipmentType);
		case EquipmentCategoryType.Cape:
			return EquipmentManager.GetEquipment(category, SaveManager.PlayerSaveData.CurrentCharacter.CapeEquipmentType);
		case EquipmentCategoryType.Trinket:
			return EquipmentManager.GetEquipment(category, SaveManager.PlayerSaveData.CurrentCharacter.TrinketEquipmentType);
		default:
			return null;
		}
	}

	// Token: 0x06005651 RID: 22097 RVA: 0x001472FC File Offset: 0x001454FC
	public static bool SetEquipped(EquipmentCategoryType category, EquipmentType equipmentToEquip, bool runEvents = true)
	{
		bool flag = false;
		switch (category)
		{
		case EquipmentCategoryType.Weapon:
		{
			EquipmentType edgeEquipmentType = SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType;
			SaveManager.PlayerSaveData.CurrentCharacter.EdgeEquipmentType = equipmentToEquip;
			flag = true;
			break;
		}
		case EquipmentCategoryType.Head:
		{
			EquipmentType headEquipmentType = SaveManager.PlayerSaveData.CurrentCharacter.HeadEquipmentType;
			SaveManager.PlayerSaveData.CurrentCharacter.HeadEquipmentType = equipmentToEquip;
			flag = true;
			break;
		}
		case EquipmentCategoryType.Chest:
		{
			EquipmentType chestEquipmentType = SaveManager.PlayerSaveData.CurrentCharacter.ChestEquipmentType;
			SaveManager.PlayerSaveData.CurrentCharacter.ChestEquipmentType = equipmentToEquip;
			flag = true;
			break;
		}
		case EquipmentCategoryType.Cape:
		{
			EquipmentType capeEquipmentType = SaveManager.PlayerSaveData.CurrentCharacter.CapeEquipmentType;
			SaveManager.PlayerSaveData.CurrentCharacter.CapeEquipmentType = equipmentToEquip;
			flag = true;
			break;
		}
		case EquipmentCategoryType.Trinket:
		{
			EquipmentType trinketEquipmentType = SaveManager.PlayerSaveData.CurrentCharacter.TrinketEquipmentType;
			SaveManager.PlayerSaveData.CurrentCharacter.TrinketEquipmentType = equipmentToEquip;
			flag = true;
			break;
		}
		}
		if (flag)
		{
			int num = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentToEquip);
			if (num > EquipmentManager.Get_EquipmentSet_CurrentLevel(equipmentToEquip))
			{
				EquipmentManager.Set_EquipmentSet_CurrentLevel(equipmentToEquip, num, false);
			}
			if (runEvents)
			{
				EquipmentManager.Instance.m_onEquippedChangeArgs.Initialize(category, equipmentToEquip);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.EquippedChanged, EquipmentManager.Instance, EquipmentManager.Instance.m_onEquippedChangeArgs);
			}
			return flag;
		}
		return false;
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x00147428 File Offset: 0x00145628
	public static float GetMinEquipmentLevelScale()
	{
		int num = 0;
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				foreach (KeyValuePair<EquipmentType, EquipmentObj> keyValuePair in EquipmentManager.GetEquipmentDict(equipmentCategoryType))
				{
					if (keyValuePair.Value.FoundState > FoundState.NotFound && keyValuePair.Value.FoundState < FoundState.Purchased)
					{
						num++;
					}
					num += EquipmentManager.GetUpgradeBlueprintsFound(equipmentCategoryType, keyValuePair.Key, false);
				}
			}
		}
		return Mathf.Ceil((float)num * 0.75f);
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x001474D4 File Offset: 0x001456D4
	public static EquipmentObj GetEquipment(EquipmentCategoryType category, EquipmentType equipType)
	{
		Dictionary<EquipmentType, EquipmentObj> equipmentDict = EquipmentManager.GetEquipmentDict(category);
		EquipmentObj result;
		if (equipmentDict == null || !equipmentDict.TryGetValue(equipType, out result))
		{
			return null;
		}
		return result;
	}

	// Token: 0x06005654 RID: 22100 RVA: 0x001474FC File Offset: 0x001456FC
	public static float GetEquippedStatValue(EquipmentCategoryType categoryType, EquipmentStatType equipmentStatType)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(categoryType);
		if (equipped == null)
		{
			return 0f;
		}
		return equipped.GetCurrentStatValue(equipmentStatType);
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x0002EEBE File Offset: 0x0002D0BE
	public static float GetTotalEquippedStatValue(EquipmentStatType equipmentStatType)
	{
		return 0f + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Weapon, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Head, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Chest, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Cape, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Trinket, equipmentStatType);
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x00147520 File Offset: 0x00145720
	public static int GetWeightLevel()
	{
		float currentStatGain = SkillTreeManager.GetSkillTreeObj(SkillTreeType.Weight_CD_Reduce).CurrentStatGain;
		float num = 0.2f + currentStatGain;
		float num2 = (float)(PlayerManager.IsInstantiated ? PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight : 0);
		float num3 = (num2 > 0f) ? (EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight) / num2) : 0f;
		float num4 = num3 / num;
		float num5 = (float)((int)(num3 / num));
		if (num4 == num5 && num5 > 0f)
		{
			num5 -= 1f;
		}
		return (int)num5;
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x00147598 File Offset: 0x00145798
	public static int Get_EquipmentSet_CurrentUnityTier(EquipmentType equipType)
	{
		int num = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipType);
		EquipmentSetData equipmentSetData = EquipmentSetLibrary.GetEquipmentSetData(equipType);
		if (equipmentSetData != null)
		{
			int setRequirement = equipmentSetData.SetRequirement03;
			int setRequirement2 = equipmentSetData.SetRequirement02;
			int setRequirement3 = equipmentSetData.SetRequirement01;
			if (num >= setRequirement)
			{
				return 3;
			}
			if (num >= setRequirement2)
			{
				return 2;
			}
			if (num >= setRequirement3)
			{
				return 1;
			}
		}
		return 0;
	}

	// Token: 0x06005658 RID: 22104 RVA: 0x0002EEED File Offset: 0x0002D0ED
	public static Dictionary<EquipmentType, EquipmentObj> GetEquipmentDict(EquipmentCategoryType category)
	{
		return SaveManager.EquipmentSaveData.GetEquipmentDict(category);
	}

	// Token: 0x06005659 RID: 22105 RVA: 0x001475E8 File Offset: 0x001457E8
	public static int GetTotalAvailableBlueprints(List<EquipmentObj> populatedList = null)
	{
		if (populatedList != null)
		{
			populatedList.Clear();
		}
		int num = 0;
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
				{
					if (equipmentType != EquipmentType.None)
					{
						EquipmentObj equipment = EquipmentManager.GetEquipment(equipmentCategoryType, equipmentType);
						if (equipment != null && !equipment.EquipmentData.Disabled)
						{
							num++;
							if (populatedList != null)
							{
								populatedList.Add(equipment);
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x04003FE1 RID: 16353
	private const string EQUIPMENT_MANAGER = "EquipmentManager";

	// Token: 0x04003FE2 RID: 16354
	private static bool m_isDisposed;

	// Token: 0x04003FE3 RID: 16355
	private static bool m_isInitialized;

	// Token: 0x04003FE4 RID: 16356
	private EquipmentPurchasedLevelChangeEventArgs m_purchasedLevelChangeArgs;

	// Token: 0x04003FE5 RID: 16357
	private EquipmentFoundLevelChangeEventArgs m_foundLevelChangeArgs;

	// Token: 0x04003FE6 RID: 16358
	private EquipmentFoundStateChangeEventArgs m_onFoundStateChangeArgs;

	// Token: 0x04003FE7 RID: 16359
	private EquippedChangeEventArgs m_onEquippedChangeArgs;

	// Token: 0x04003FE8 RID: 16360
	private static EquipmentManager m_equipmentManager = null;

	// Token: 0x04003FE9 RID: 16361
	private static List<EquipmentType> m_equipTypesUsedHelper = new List<EquipmentType>();
}
