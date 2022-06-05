using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000698 RID: 1688
public class EquipmentManager : MonoBehaviour
{
	// Token: 0x06003D76 RID: 15734 RVA: 0x000D61AE File Offset: 0x000D43AE
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x000D61B8 File Offset: 0x000D43B8
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

	// Token: 0x06003D78 RID: 15736 RVA: 0x000D6220 File Offset: 0x000D4420
	private void OnDestroy()
	{
		EquipmentManager.m_isDisposed = true;
	}

	// Token: 0x17001543 RID: 5443
	// (get) Token: 0x06003D79 RID: 15737 RVA: 0x000D6228 File Offset: 0x000D4428
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

	// Token: 0x17001544 RID: 5444
	// (get) Token: 0x06003D7A RID: 15738 RVA: 0x000D6253 File Offset: 0x000D4453
	public static bool IsInitialized
	{
		get
		{
			return EquipmentManager.m_isInitialized;
		}
	}

	// Token: 0x17001545 RID: 5445
	// (get) Token: 0x06003D7B RID: 15739 RVA: 0x000D625A File Offset: 0x000D445A
	public static bool IsDisposed
	{
		get
		{
			return EquipmentManager.m_isDisposed;
		}
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x000D6261 File Offset: 0x000D4461
	public static bool DoesEquipmentExist(EquipmentCategoryType category, EquipmentType equipType)
	{
		return EquipmentManager.GetEquipment(category, equipType) != null;
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x000D6270 File Offset: 0x000D4470
	public static List<EquipmentType> GetAllEquipmentWithFoundState(EquipmentCategoryType category, FoundState foundState)
	{
		List<EquipmentType> list = new List<EquipmentType>();
		EquipmentManager.GetAllEquipmentWithFoundState(category, foundState, list);
		return list;
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x000D628C File Offset: 0x000D448C
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

	// Token: 0x06003D7F RID: 15743 RVA: 0x000D62FC File Offset: 0x000D44FC
	public static bool CanAfford(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		return SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= equipment.GoldCostToUpgrade && SaveManager.PlayerSaveData.EquipmentOreCollected >= equipment.OreCostToUpgrade;
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x000D6338 File Offset: 0x000D4538
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

	// Token: 0x06003D81 RID: 15745 RVA: 0x000D6418 File Offset: 0x000D4618
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

	// Token: 0x06003D82 RID: 15746 RVA: 0x000D6504 File Offset: 0x000D4704
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

	// Token: 0x06003D83 RID: 15747 RVA: 0x000D6550 File Offset: 0x000D4750
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

	// Token: 0x06003D84 RID: 15748 RVA: 0x000D664E File Offset: 0x000D484E
	public static int Get_EquipmentSet_CurrentLevel(EquipmentType equipType)
	{
		if (!SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			return 0;
		}
		return SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType].x;
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x000D667C File Offset: 0x000D487C
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

	// Token: 0x06003D86 RID: 15750 RVA: 0x000D66D6 File Offset: 0x000D48D6
	public static int Get_EquipmentSet_LevelViewed(EquipmentType equipType)
	{
		if (!SaveManager.EquipmentSaveData.EquipmentSetStateDict.ContainsKey(equipType))
		{
			return 0;
		}
		return SaveManager.EquipmentSaveData.EquipmentSetStateDict[equipType].y;
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x000D6704 File Offset: 0x000D4904
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

	// Token: 0x06003D88 RID: 15752 RVA: 0x000D6760 File Offset: 0x000D4960
	public static int GetUpgradeBlueprintsFound(EquipmentCategoryType category, EquipmentType equipType, bool ignoreInfinitePurchasePower = false)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment != null)
		{
			return Mathf.Clamp(equipment.UpgradeBlueprintsFound, 0, equipment.MaxLevel);
		}
		return 0;
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x000D678C File Offset: 0x000D498C
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

	// Token: 0x06003D8A RID: 15754 RVA: 0x000D6878 File Offset: 0x000D4A78
	public static int GetUpgradeLevel(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(category, equipType);
		if (equipment != null)
		{
			return equipment.ClampedUpgradeLevel;
		}
		return 0;
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x000D6898 File Offset: 0x000D4A98
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

	// Token: 0x06003D8C RID: 15756 RVA: 0x000D69C8 File Offset: 0x000D4BC8
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

	// Token: 0x06003D8D RID: 15757 RVA: 0x000D69F8 File Offset: 0x000D4BF8
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

	// Token: 0x06003D8E RID: 15758 RVA: 0x000D6ABC File Offset: 0x000D4CBC
	public static bool IsEquipped(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(category);
		return equipped != null && equipped.EquipmentType == equipType;
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x000D6AE0 File Offset: 0x000D4CE0
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

	// Token: 0x06003D90 RID: 15760 RVA: 0x000D6B7C File Offset: 0x000D4D7C
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

	// Token: 0x06003D91 RID: 15761 RVA: 0x000D6CA8 File Offset: 0x000D4EA8
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

	// Token: 0x06003D92 RID: 15762 RVA: 0x000D6D54 File Offset: 0x000D4F54
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

	// Token: 0x06003D93 RID: 15763 RVA: 0x000D6D7C File Offset: 0x000D4F7C
	public static float GetEquippedStatValue(EquipmentCategoryType categoryType, EquipmentStatType equipmentStatType)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(categoryType);
		if (equipped == null)
		{
			return 0f;
		}
		return equipped.GetCurrentStatValue(equipmentStatType);
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x000D6DA0 File Offset: 0x000D4FA0
	public static float GetTotalEquippedStatValue(EquipmentStatType equipmentStatType)
	{
		return 0f + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Weapon, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Head, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Chest, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Cape, equipmentStatType) + EquipmentManager.GetEquippedStatValue(EquipmentCategoryType.Trinket, equipmentStatType);
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x000D6DD0 File Offset: 0x000D4FD0
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

	// Token: 0x06003D96 RID: 15766 RVA: 0x000D6E48 File Offset: 0x000D5048
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

	// Token: 0x06003D97 RID: 15767 RVA: 0x000D6E96 File Offset: 0x000D5096
	public static Dictionary<EquipmentType, EquipmentObj> GetEquipmentDict(EquipmentCategoryType category)
	{
		return SaveManager.EquipmentSaveData.GetEquipmentDict(category);
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x000D6EA4 File Offset: 0x000D50A4
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

	// Token: 0x04002DF0 RID: 11760
	private const string EQUIPMENT_MANAGER = "EquipmentManager";

	// Token: 0x04002DF1 RID: 11761
	private static bool m_isDisposed;

	// Token: 0x04002DF2 RID: 11762
	private static bool m_isInitialized;

	// Token: 0x04002DF3 RID: 11763
	private EquipmentPurchasedLevelChangeEventArgs m_purchasedLevelChangeArgs;

	// Token: 0x04002DF4 RID: 11764
	private EquipmentFoundLevelChangeEventArgs m_foundLevelChangeArgs;

	// Token: 0x04002DF5 RID: 11765
	private EquipmentFoundStateChangeEventArgs m_onFoundStateChangeArgs;

	// Token: 0x04002DF6 RID: 11766
	private EquippedChangeEventArgs m_onEquippedChangeArgs;

	// Token: 0x04002DF7 RID: 11767
	private static EquipmentManager m_equipmentManager = null;

	// Token: 0x04002DF8 RID: 11768
	private static List<EquipmentType> m_equipTypesUsedHelper = new List<EquipmentType>();
}
