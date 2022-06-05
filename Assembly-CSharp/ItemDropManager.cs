using System;
using System.Collections;
using System.Collections.Generic;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200069C RID: 1692
public class ItemDropManager : MonoBehaviour
{
	// Token: 0x06003DC0 RID: 15808 RVA: 0x000D7998 File Offset: 0x000D5B98
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x000D79CF File Offset: 0x000D5BCF
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (ItemDropManager.IsInitialized && ItemDropManager.m_itemDropManager)
		{
			ItemDropManager.DisableAllItemDrops();
		}
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x000D79E9 File Offset: 0x000D5BE9
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		SoulDrop.FakeSoulCounter_STATIC = 0;
		MinimapHUDController.UpdateTextOnly = true;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, null);
		MinimapHUDController.UpdateTextOnly = false;
		base.StopAllCoroutines();
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x000D7A0C File Offset: 0x000D5C0C
	private void OnDestroy()
	{
		ItemDropManager.IsInitialized = false;
		ItemDropManager.m_itemDropManager = null;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x000D7A38 File Offset: 0x000D5C38
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

	// Token: 0x06003DC5 RID: 15813 RVA: 0x000D7B90 File Offset: 0x000D5D90
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

	// Token: 0x06003DC6 RID: 15814 RVA: 0x000D7BAC File Offset: 0x000D5DAC
	private bool CanDropGold(bool fromChest)
	{
		float architectGoldMod = NPC_EV.GetArchitectGoldMod(-1);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.NoGoldXPBonus).Level;
		bool flag = !TraitManager.IsTraitActive(TraitType.BonusChestGold) || fromChest;
		return architectGoldMod > 0f && level <= 0 && flag;
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x000D7BF4 File Offset: 0x000D5DF4
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

	// Token: 0x06003DC8 RID: 15816 RVA: 0x000D7E78 File Offset: 0x000D6078
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

	// Token: 0x06003DC9 RID: 15817 RVA: 0x000D8054 File Offset: 0x000D6254
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

	// Token: 0x06003DCA RID: 15818 RVA: 0x000D81F0 File Offset: 0x000D63F0
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

	// Token: 0x06003DCB RID: 15819 RVA: 0x000D8240 File Offset: 0x000D6440
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

	// Token: 0x1700154C RID: 5452
	// (get) Token: 0x06003DCC RID: 15820 RVA: 0x000D82A2 File Offset: 0x000D64A2
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

	// Token: 0x1700154D RID: 5453
	// (get) Token: 0x06003DCD RID: 15821 RVA: 0x000D82C0 File Offset: 0x000D64C0
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

	// Token: 0x06003DCE RID: 15822 RVA: 0x000D834C File Offset: 0x000D654C
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

	// Token: 0x1700154E RID: 5454
	// (get) Token: 0x06003DCF RID: 15823 RVA: 0x000D83BC File Offset: 0x000D65BC
	// (set) Token: 0x06003DD0 RID: 15824 RVA: 0x000D83C3 File Offset: 0x000D65C3
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003DD1 RID: 15825 RVA: 0x000D83CB File Offset: 0x000D65CB
	public static void DropGold(int amount, Vector3 position, bool largeSpurt, bool forceMagnetize = false, bool fromChest = false)
	{
		ItemDropManager.Instance.Internal_DropGold(amount, position, largeSpurt, forceMagnetize, fromChest);
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x000D83DD File Offset: 0x000D65DD
	public static void DropItem(ItemDropType itemDropType, int amount, Vector3 position, bool largeSpurt, bool forceMagnetize = false, bool fromChest = false)
	{
		ItemDropManager.Instance.Internal_DropItem(itemDropType, amount, position, largeSpurt, forceMagnetize, fromChest, false);
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x000D83F2 File Offset: 0x000D65F2
	public static void DropSpecialItem(ISpecialItemDrop specialItemDrop, bool displayItemDropWindow = true)
	{
		ItemDropManager.Instance.Internal_DropSpecialItem(specialItemDrop, displayItemDropWindow);
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x000D8400 File Offset: 0x000D6600
	public static void DisableAllItemDrops()
	{
		foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x000D8460 File Offset: 0x000D6660
	public static void Reset()
	{
		ItemDropManager.DisableAllItemDrops();
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x000D8468 File Offset: 0x000D6668
	public static void DestroyPools()
	{
		foreach (KeyValuePair<ItemDropType, GenericPool_RL<BaseItemDrop>> keyValuePair in ItemDropManager.Instance.m_itemDropDict)
		{
			keyValuePair.Value.DestroyPool();
		}
		ItemDropManager.Instance.m_itemDropDict.Clear();
	}

	// Token: 0x04002E08 RID: 11784
	private const string ITEMDROPMANAGER_NAME = "ItemDropManager";

	// Token: 0x04002E09 RID: 11785
	private const string RESOURCE_PATH = "Prefabs/Managers/ItemDropManager";

	// Token: 0x04002E0A RID: 11786
	private const float DELAY_BETWEEN_GOLD_DROPS = 0.02f;

	// Token: 0x04002E0B RID: 11787
	private const float MAX_GOLD_DROP_ANIM_DURATION = 1.5f;

	// Token: 0x04002E0C RID: 11788
	public Relay<ItemDropType, Vector2, int> DropGoldRelay = new Relay<ItemDropType, Vector2, int>();

	// Token: 0x04002E0D RID: 11789
	public Relay<ItemDropType, Vector2> DropItemRelay = new Relay<ItemDropType, Vector2>();

	// Token: 0x04002E0E RID: 11790
	private Dictionary<ItemDropType, GenericPool_RL<BaseItemDrop>> m_itemDropDict;

	// Token: 0x04002E0F RID: 11791
	private List<ItemDropType> m_goldDropTypes;

	// Token: 0x04002E10 RID: 11792
	private WaitForSeconds m_coinDropWaitYield = new WaitForSeconds(0.02f);

	// Token: 0x04002E11 RID: 11793
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002E12 RID: 11794
	private static ItemDropManager m_itemDropManager;

	// Token: 0x02000E0A RID: 3594
	private struct GoldDropData
	{
		// Token: 0x06006B2C RID: 27436 RVA: 0x00190C28 File Offset: 0x0018EE28
		public GoldDropData(ItemDropType type, int amount)
		{
			this.Type = type;
			this.Amount = amount;
		}

		// Token: 0x04005682 RID: 22146
		public ItemDropType Type;

		// Token: 0x04005683 RID: 22147
		public int Amount;
	}
}
