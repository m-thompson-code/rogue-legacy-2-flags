using System;
using System.Collections;
using System.Collections.Generic;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B30 RID: 2864
public class ItemDropManager : MonoBehaviour
{
	// Token: 0x06005693 RID: 22163 RVA: 0x0002F0EA File Offset: 0x0002D2EA
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06005694 RID: 22164 RVA: 0x0002F121 File Offset: 0x0002D321
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (ItemDropManager.IsInitialized && ItemDropManager.m_itemDropManager)
		{
			ItemDropManager.DisableAllItemDrops();
		}
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x0002F13B File Offset: 0x0002D33B
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		SoulDrop.FakeSoulCounter_STATIC = 0;
		MinimapHUDController.UpdateTextOnly = true;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, null);
		MinimapHUDController.UpdateTextOnly = false;
		base.StopAllCoroutines();
	}

	// Token: 0x06005696 RID: 22166 RVA: 0x0002F15E File Offset: 0x0002D35E
	private void OnDestroy()
	{
		ItemDropManager.IsInitialized = false;
		ItemDropManager.m_itemDropManager = null;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06005697 RID: 22167 RVA: 0x00148048 File Offset: 0x00146248
	private void Initialize()
	{
		this.m_itemDropDict = new Dictionary<ItemDropType, GenericPool_RL<BaseItemDrop>>();
		foreach (ItemDropEntry itemDropEntry in ItemDropLibrary.ItemDropEntryList)
		{
			BaseItemDrop itemDropPrefab = itemDropEntry.ItemDropPrefab;
			if (this.m_itemDropDict.ContainsKey(itemDropPrefab.ItemDropType))
			{
				throw new Exception("Item Drop Type: " + itemDropPrefab.ItemDropType.ToString() + " already found in Item Drop Library.  Duplicates not allowed.");
			}
			GenericPool_RL<BaseItemDrop> genericPool_RL = new GenericPool_RL<BaseItemDrop>();
			genericPool_RL.Initialize(itemDropPrefab, itemDropEntry.ItemDropPoolSize, false, true);
			this.m_itemDropDict.Add(itemDropPrefab.ItemDropType, genericPool_RL);
		}
		this.m_goldDropTypes = new List<ItemDropType>();
		foreach (object obj in Enum.GetValues(typeof(ItemDropType)))
		{
			ItemDropType itemDropType = (ItemDropType)obj;
			if (Economy_EV.GetItemDropValue(itemDropType, true) > 0)
			{
				this.m_goldDropTypes.Add(itemDropType);
			}
		}
		this.m_goldDropTypes.Sort(new Comparison<ItemDropType>(this.GoldValueSort));
		ItemDropManager.IsInitialized = true;
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x0002F189 File Offset: 0x0002D389
	private int GoldValueSort(ItemDropType obj1, ItemDropType obj2)
	{
		if (obj1 == obj2)
		{
			return 0;
		}
		if (Economy_EV.GetItemDropValue(obj1, false) < Economy_EV.GetItemDropValue(obj2, false))
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06005699 RID: 22169 RVA: 0x001481A0 File Offset: 0x001463A0
	private bool CanDropGold(bool fromChest)
	{
		float architectGoldMod = NPC_EV.GetArchitectGoldMod(-1);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.NoGoldXPBonus).Level;
		bool flag = !TraitManager.IsTraitActive(TraitType.BonusChestGold) || fromChest;
		return architectGoldMod > 0f && level <= 0 && flag;
	}

	// Token: 0x0600569A RID: 22170 RVA: 0x001481E8 File Offset: 0x001463E8
	private void Internal_DropItem(ItemDropType itemDropType, int amount, Vector3 position, bool useLargeSpurt, bool forceMagnetize, bool fromChest, bool dropOneWithFullAmount)
	{
		if (Economy_EV.GetItemDropValue(itemDropType, true) > 0 && !this.CanDropGold(fromChest))
		{
			return;
		}
		if (itemDropType == ItemDropType.HealthDrop)
		{
			if (TraitManager.IsTraitActive(TraitType.MushroomGrow))
			{
				itemDropType = ItemDropType.MushroomDrop;
			}
			else if (Economy_EV.CanDropPizza() && UnityEngine.Random.Range(0f, 1f) < 0.1f)
			{
				itemDropType = ItemDropType.PizzaDrop;
			}
			if (HolidayLookController.IsHoliday(HolidayType.Halloween))
			{
				itemDropType = ItemDropType.CandyDrop;
			}
			else if (HolidayLookController.IsHoliday(HolidayType.Christmas))
			{
				itemDropType = ItemDropType.CookieDrop;
			}
		}
		else if (itemDropType == ItemDropType.ManaDrop && HolidayLookController.IsHoliday(HolidayType.Christmas))
		{
			itemDropType = ItemDropType.MilkManaDrop;
		}
		this.DropItemRelay.Dispatch(itemDropType, position);
		GenericPool_RL<BaseItemDrop> genericPool_RL = null;
		if (!this.m_itemDropDict.TryGetValue(itemDropType, out genericPool_RL))
		{
			throw new Exception("Item Drop: " + itemDropType.ToString() + " cannot be found in ItemDropManager.  Please make sure the prefab is added to the Item Drop Library prefab.");
		}
		int num = -1;
		int num2 = 0;
		int num3;
		if (dropOneWithFullAmount)
		{
			num3 = 1;
			num = amount;
		}
		else
		{
			int itemDropValue = Economy_EV.GetItemDropValue(itemDropType, false);
			num3 = ((itemDropValue > 0) ? (amount / itemDropValue) : 1);
			if (num3 > 10)
			{
				num3 = 10;
				num = (int)((float)amount / (float)num3);
			}
			int num4 = (num != -1) ? num : itemDropValue;
			num2 = amount - num3 * num4;
		}
		for (int i = 0; i < num3; i++)
		{
			BaseItemDrop freeObj = genericPool_RL.GetFreeObj();
			EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = true;
			freeObj.gameObject.SetActive(true);
			freeObj.ValueOverride = num;
			if (num2 > 0 && i == num3 - 1)
			{
				freeObj.ValueOverride += num2;
			}
			freeObj.transform.position = new Vector3(position.x, position.y, freeObj.transform.position.z);
			if (freeObj.CorgiController.IsInitialized)
			{
				freeObj.CorgiController.SetRaysParameters();
				freeObj.CorgiController.ResetState();
				freeObj.CorgiController.State.IsCollidingBelow = false;
			}
			float num5 = UnityEngine.Random.Range(Economy_EV.ITEM_DROP_REGULAR_SPURT_MINMAX_X.x, Economy_EV.ITEM_DROP_REGULAR_SPURT_MINMAX_X.y);
			float y = UnityEngine.Random.Range(Economy_EV.ITEM_DROP_REGULAR_SPURT_MINMAX_Y.x, Economy_EV.ITEM_DROP_REGULAR_SPURT_MINMAX_Y.y);
			if (useLargeSpurt)
			{
				num5 = UnityEngine.Random.Range(Economy_EV.ITEM_DROP_LARGE_SPURT_MINMAX_X.x, Economy_EV.ITEM_DROP_LARGE_SPURT_MINMAX_X.y);
				y = UnityEngine.Random.Range(Economy_EV.ITEM_DROP_LARGE_SPURT_MINMAX_Y.x, Economy_EV.ITEM_DROP_LARGE_SPURT_MINMAX_Y.y);
			}
			num5 *= (float)CDGHelper.RandomPlusMinus();
			Vector2 force = new Vector2(num5, y);
			freeObj.CorgiController.SetForce(force);
			if (forceMagnetize)
			{
				freeObj.ForceMagnetizeOnGrounded();
			}
			freeObj.OnSpawnCollectCollisionCheck();
			EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = false;
			bool activeSelf = freeObj.gameObject.activeSelf;
		}
	}

	// Token: 0x0600569B RID: 22171 RVA: 0x0014846C File Offset: 0x0014666C
	private void Internal_DropSpecialItem(ISpecialItemDrop specialItemDrop, bool displayItemDropWindow)
	{
		IBlueprintDrop blueprintDrop = specialItemDrop as IBlueprintDrop;
		if (blueprintDrop != null)
		{
			if (EquipmentManager.GetFoundState(blueprintDrop.CategoryType, blueprintDrop.EquipmentType) == FoundState.NotFound)
			{
				EquipmentManager.SetFoundState(blueprintDrop.CategoryType, blueprintDrop.EquipmentType, FoundState.FoundButNotViewed, false, true);
			}
			else
			{
				EquipmentManager.SetUpgradeBlueprintsFound(blueprintDrop.CategoryType, blueprintDrop.EquipmentType, 1, true, true);
			}
		}
		IRuneDrop runeDrop = specialItemDrop as IRuneDrop;
		if (runeDrop != null)
		{
			RuneManager.SetUpgradeBlueprintsFound(runeDrop.RuneType, 1, true, true);
		}
		IRelicDrop relicDrop = specialItemDrop as IRelicDrop;
		if (relicDrop != null)
		{
			RelicType relicType = relicDrop.RelicType;
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
			if (relicDrop.RelicModType == RelicModType.DoubleRelic)
			{
				relic.SetLevel(2, true, true);
			}
			else
			{
				relic.SetLevel(1, true, true);
			}
		}
		IAbilityDrop abilityDrop = specialItemDrop as IAbilityDrop;
		if (abilityDrop != null)
		{
			AbilityType abilityType = abilityDrop.AbilityType;
			CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
			switch (abilityDrop.CastAbilityType)
			{
			case CastAbilityType.Weapon:
				currentCharacter.Weapon = abilityType;
				break;
			case CastAbilityType.Spell:
				currentCharacter.Spell = abilityType;
				SaveManager.PlayerSaveData.SetSpellSeenState(currentCharacter.Spell, true);
				break;
			case CastAbilityType.Talent:
				currentCharacter.Talent = abilityType;
				if (currentCharacter.ClassType == ClassType.MagicWandClass)
				{
					SaveManager.PlayerSaveData.SetSpellSeenState(currentCharacter.Talent, true);
				}
				break;
			}
			SaveManager.PlayerSaveData.CurrentCharacter = currentCharacter;
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.CharacterClass.SetAbility(abilityDrop.CastAbilityType, abilityType, true);
			if (playerController.LookController.IsClassLookInitialized)
			{
				playerController.LookController.InitializeEquipmentLook(SaveManager.PlayerSaveData.CurrentCharacter);
			}
		}
		IChallengeDrop challengeDrop = specialItemDrop as IChallengeDrop;
		if (challengeDrop != null)
		{
			ChallengeManager.SetUpgradeBlueprintsFound(challengeDrop.ChallengeType, 1, true, true);
		}
		if (displayItemDropWindow)
		{
			if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
			{
				WindowManager.LoadWindow(WindowID.SpecialItemDrop);
			}
			(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(specialItemDrop);
			if (!WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
			{
				WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
			}
		}
	}

	// Token: 0x0600569C RID: 22172 RVA: 0x00148648 File Offset: 0x00146848
	private void Internal_DropGold(int amount, Vector3 position, bool largeSpurt, bool forceMagnetize, bool fromChest)
	{
		if (!this.CanDropGold(fromChest))
		{
			return;
		}
		int num = amount;
		if (TraitManager.IsTraitActive(TraitType.BonusChestGold))
		{
			float num2 = UnityEngine.Random.Range(Trait_EV.BONUS_CHEST_GOLD_DIE_ROLL.x, Trait_EV.BONUS_CHEST_GOLD_DIE_ROLL.y);
			float num3 = Trait_EV.BONUS_CHEST_GOLD_DIE_MOD * num2;
			num = Mathf.RoundToInt((float)num * num3);
			string text = string.Format(LocalizationManager.GetString("LOC_ID_GOLD_UI_GOLD_PERCENT_POPUP_1", false, false), (int)(num3 * 100f));
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.GoldCollected, text, PlayerManager.GetPlayerController(), true, true);
		}
		int num4 = num;
		int num5 = 0;
		int num6 = 1;
		List<ItemDropManager.GoldDropData> list = new List<ItemDropManager.GoldDropData>();
		if (num <= Economy_EV.GetItemDropValue(ItemDropType.Coin, false))
		{
			this.Internal_DropItem(ItemDropType.Coin, Economy_EV.GetItemDropValue(ItemDropType.Coin, false), position, largeSpurt, forceMagnetize, fromChest, true);
			return;
		}
		for (int i = this.m_goldDropTypes.Count - 1; i >= 0; i--)
		{
			ItemDropType itemDropType = this.m_goldDropTypes[i];
			int itemDropValue = Economy_EV.GetItemDropValue(itemDropType, false);
			int num7 = (int)((float)num4 / (float)itemDropValue);
			int num8 = num7 * itemDropValue;
			num4 -= num8;
			int num9 = -1;
			if (num7 > 10)
			{
				num7 = 10;
				num9 = num8 / num7;
			}
			for (int j = 0; j < num7; j++)
			{
				int amount2 = (num9 > 0) ? num9 : itemDropValue;
				list.Add(new ItemDropManager.GoldDropData(itemDropType, amount2));
			}
			num5 += num7;
		}
		float num10 = (float)num5 * 0.02f;
		if (num10 > 1.5f)
		{
			num6 = Mathf.CeilToInt(num10 / 1.5f);
		}
		if (num6 <= 0)
		{
			throw new Exception("ERROR: numDropsPerDelay must be greater than 0. Double check code.");
		}
		base.StartCoroutine(this.DropGoldCoroutine(num5, num6, position, list, largeSpurt, forceMagnetize, fromChest));
	}

	// Token: 0x0600569D RID: 22173 RVA: 0x001487E4 File Offset: 0x001469E4
	private IEnumerator DropGoldCoroutine(int totalItemDrops, int numDropsPerDelay, Vector2 position, List<ItemDropManager.GoldDropData> allItemsDroppedList, bool largeSpurt, bool forceMagnetize, bool fromChest)
	{
		int dropCounter = 0;
		this.DispatchDropGoldRelay(allItemsDroppedList, position);
		while (totalItemDrops > 0)
		{
			for (int i = 0; i < numDropsPerDelay; i++)
			{
				ItemDropType itemDropType;
				int amount;
				if (dropCounter < allItemsDroppedList.Count)
				{
					itemDropType = allItemsDroppedList[dropCounter].Type;
					amount = allItemsDroppedList[dropCounter].Amount;
				}
				else
				{
					itemDropType = ItemDropType.Coin;
					amount = Economy_EV.GetItemDropValue(itemDropType, false);
				}
				this.Internal_DropItem(itemDropType, amount, position, largeSpurt, forceMagnetize, fromChest, true);
				int num = dropCounter;
				dropCounter = num + 1;
				num = totalItemDrops;
				totalItemDrops = num - 1;
				if (totalItemDrops <= 0)
				{
					break;
				}
			}
			yield return this.m_coinDropWaitYield;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600569E RID: 22174 RVA: 0x00148834 File Offset: 0x00146A34
	private void DispatchDropGoldRelay(List<ItemDropManager.GoldDropData> allItemsDroppedList, Vector2 position)
	{
		foreach (ItemDropType itemDropType in ItemDropType_RL.Types)
		{
			int num = 0;
			for (int j = 0; j < allItemsDroppedList.Count; j++)
			{
				if (allItemsDroppedList[j].Type == itemDropType)
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.DropGoldRelay.Dispatch(itemDropType, position, num);
			}
		}
	}

	// Token: 0x17001D2C RID: 7468
	// (get) Token: 0x0600569F RID: 22175 RVA: 0x0002F1A4 File Offset: 0x0002D3A4
	private static ItemDropManager Instance
	{
		get
		{
			if (!ItemDropManager.m_itemDropManager)
			{
				ItemDropManager.m_itemDropManager = CDGHelper.FindStaticInstance<ItemDropManager>(false);
			}
			return ItemDropManager.m_itemDropManager;
		}
	}

	// Token: 0x17001D2D RID: 7469
	// (get) Token: 0x060056A0 RID: 22176 RVA: 0x00148898 File Offset: 0x00146A98
	public static bool HasActiveItemDrops
	{
		get
		{
			bool flag = TraitManager.IsTraitActive(TraitType.NoMeat);
			foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
			{
				if (((keyValuePair.Key != ItemDropType.HealthDrop && keyValuePair.Key != ItemDropType.PizzaDrop) || !flag) && keyValuePair.Value.HasActiveObjects)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060056A1 RID: 22177 RVA: 0x00148924 File Offset: 0x00146B24
	public static bool HasActiveItemDropsOfType(ItemDropType itemDropType)
	{
		foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
		{
			if (keyValuePair.Key == itemDropType && keyValuePair.Value.HasActiveObjects)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17001D2E RID: 7470
	// (get) Token: 0x060056A2 RID: 22178 RVA: 0x0002F1C2 File Offset: 0x0002D3C2
	// (set) Token: 0x060056A3 RID: 22179 RVA: 0x0002F1C9 File Offset: 0x0002D3C9
	public static bool IsInitialized { get; private set; }

	// Token: 0x060056A4 RID: 22180 RVA: 0x0002F1D1 File Offset: 0x0002D3D1
	public static void DropGold(int amount, Vector3 position, bool largeSpurt, bool forceMagnetize = false, bool fromChest = false)
	{
		ItemDropManager.Instance.Internal_DropGold(amount, position, largeSpurt, forceMagnetize, fromChest);
	}

	// Token: 0x060056A5 RID: 22181 RVA: 0x0002F1E3 File Offset: 0x0002D3E3
	public static void DropItem(ItemDropType itemDropType, int amount, Vector3 position, bool largeSpurt, bool forceMagnetize = false, bool fromChest = false)
	{
		ItemDropManager.Instance.Internal_DropItem(itemDropType, amount, position, largeSpurt, forceMagnetize, fromChest, false);
	}

	// Token: 0x060056A6 RID: 22182 RVA: 0x0002F1F8 File Offset: 0x0002D3F8
	public static void DropSpecialItem(ISpecialItemDrop specialItemDrop, bool displayItemDropWindow = true)
	{
		ItemDropManager.Instance.Internal_DropSpecialItem(specialItemDrop, displayItemDropWindow);
	}

	// Token: 0x060056A7 RID: 22183 RVA: 0x00148994 File Offset: 0x00146B94
	public static void DisableAllItemDrops()
	{
		foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x060056A8 RID: 22184 RVA: 0x0002F206 File Offset: 0x0002D406
	public static void Reset()
	{
		ItemDropManager.DisableAllItemDrops();
	}

	// Token: 0x060056A9 RID: 22185 RVA: 0x001489F4 File Offset: 0x00146BF4
	public static void DestroyPools()
	{
		foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
		{
			keyValuePair.Value.DestroyPool();
		}
		ItemDropManager.Instance.m_itemDropDict.Clear();
	}

	// Token: 0x04004002 RID: 16386
	private const string ITEMDROPMANAGER_NAME = "ItemDropManager";

	// Token: 0x04004003 RID: 16387
	private const string RESOURCE_PATH = "Prefabs/Managers/ItemDropManager";

	// Token: 0x04004004 RID: 16388
	private const float DELAY_BETWEEN_GOLD_DROPS = 0.02f;

	// Token: 0x04004005 RID: 16389
	private const float MAX_GOLD_DROP_ANIM_DURATION = 1.5f;

	// Token: 0x04004006 RID: 16390
	public Relay<ItemDropType, Vector2, int> DropGoldRelay = new Relay<ItemDropType, Vector2, int>();

	// Token: 0x04004007 RID: 16391
	public Relay<ItemDropType, Vector2> DropItemRelay = new Relay<ItemDropType, Vector2>();

	// Token: 0x04004008 RID: 16392
	private Dictionary<ItemDropType, GenericPool_RL<BaseItemDrop>> m_itemDropDict;

	// Token: 0x04004009 RID: 16393
	private List<ItemDropType> m_goldDropTypes;

	// Token: 0x0400400A RID: 16394
	private WaitForSeconds m_coinDropWaitYield = new WaitForSeconds(0.02f);

	// Token: 0x0400400B RID: 16395
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x0400400C RID: 16396
	private static ItemDropManager m_itemDropManager;

	// Token: 0x02000B31 RID: 2865
	private struct GoldDropData
	{
		// Token: 0x060056AC RID: 22188 RVA: 0x0002F23B File Offset: 0x0002D43B
		public GoldDropData(ItemDropType type, int amount)
		{
			this.Type = type;
			this.Amount = amount;
		}

		// Token: 0x0400400E RID: 16398
		public ItemDropType Type;

		// Token: 0x0400400F RID: 16399
		public int Amount;
	}
}
