using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
public class SwapAbilityRoomPropController : DualChoicePropController, ILocalizable
{
	// Token: 0x1700118C RID: 4492
	// (get) Token: 0x06002EBB RID: 11963 RVA: 0x0009F398 File Offset: 0x0009D598
	public AbilityType AbilityType
	{
		get
		{
			return this.m_abilityType;
		}
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x0009F3A0 File Offset: 0x0009D5A0
	protected override void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		base.Awake();
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x0009F3CC File Offset: 0x0009D5CC
	protected override void InitializePooledPropOnEnter()
	{
		base.InitializePooledPropOnEnter();
		base.RightInfoTextBox.gameObject.SetActive(false);
		base.RightIcon.gameObject.SetActive(false);
		this.m_closedSign.gameObject.SetActive(false);
		this.m_totalRoomRolls = 1;
		int totalRoomRolls;
		if (int.TryParse(base.GetRoomMiscData("TotalAbilityRolls"), NumberStyles.Any, SaveManager.CultureInfo, out totalRoomRolls))
		{
			this.m_totalRoomRolls = totalRoomRolls;
		}
		SwapAbilityRoomPropController.m_potentialAbilityList.Clear();
		AbilityType[] abilityArray = this.GetAbilityArray(this.m_castAbilityTypeToSwap);
		for (int i = 0; i < abilityArray.Length; i++)
		{
			if (abilityArray[i] != AbilityType.None && AbilityLibrary.GetAbility(abilityArray[i]))
			{
				SwapAbilityRoomPropController.m_potentialAbilityList.Add(abilityArray[i]);
			}
		}
		this.RollAbilities(this.m_totalRoomRolls, false);
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x0009F494 File Offset: 0x0009D694
	public void RollAbilities(int numRolls, bool addToTotalRoomRolls)
	{
		if (addToTotalRoomRolls)
		{
			this.m_totalRoomRolls += numRolls;
			base.SetRoomMiscData("TotalAbilityRolls", this.m_totalRoomRolls.ToString());
		}
		for (int i = 0; i < numRolls; i++)
		{
			AbilityType currentPlayerAbility = this.GetCurrentPlayerAbility(CastAbilityType.Weapon);
			AbilityType currentPlayerAbility2 = this.GetCurrentPlayerAbility(CastAbilityType.Spell);
			AbilityType currentPlayerAbility3 = this.GetCurrentPlayerAbility(CastAbilityType.Talent);
			this.m_abilityType = AbilityType.None;
			while (this.m_abilityType == AbilityType.None)
			{
				if (SwapAbilityRoomPropController.m_potentialAbilityList.Count == 0)
				{
					this.m_abilityType = AbilityType.SwordWeapon;
					break;
				}
				int randomNumber = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "AbilitySwapRoomPropController.RollAbilities()", 0, SwapAbilityRoomPropController.m_potentialAbilityList.Count);
				AbilityType abilityType = SwapAbilityRoomPropController.m_potentialAbilityList[randomNumber];
				for (int j = 0; j < SwapAbilityRoomPropController.m_potentialAbilityList.Count; j++)
				{
					if (SwapAbilityRoomPropController.m_potentialAbilityList[j] == abilityType)
					{
						SwapAbilityRoomPropController.m_potentialAbilityList.RemoveAt(j);
						j--;
					}
				}
				if (abilityType != currentPlayerAbility && abilityType != currentPlayerAbility2 && abilityType != currentPlayerAbility3)
				{
					this.m_abilityType = abilityType;
				}
			}
		}
		this.InitializeTextBox();
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x0009F5A4 File Offset: 0x0009D7A4
	public void InitializeTextBox()
	{
		PlayerManager.GetPlayerController();
		AbilityType abilityType = this.AbilityType;
		GenericInfoTextBox leftInfoTextBox = base.LeftInfoTextBox;
		SpriteRenderer leftIcon = base.LeftIcon;
		BaseAbility_RL ability = AbilityLibrary.GetAbility(abilityType);
		if (!ability)
		{
			throw new Exception("Invalid ability selected. AbilityType: " + abilityType.ToString());
		}
		AbilityData abilityData = ability.AbilityData;
		if (abilityData)
		{
			leftInfoTextBox.HeaderText.text = LocalizationManager.GetString(abilityData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			string costString = this.GetCostString(this.m_castAbilityTypeToSwap);
			leftInfoTextBox.SubHeaderText.text = costString;
			string @string = LocalizationManager.GetString(abilityData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			if (this.m_castAbilityTypeToSwap == CastAbilityType.Spell && !SaveManager.PlayerSaveData.GetSpellSeenState(abilityType))
			{
				@string = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_ABILITY_1", false, false);
			}
			TMP_Text subHeaderText = leftInfoTextBox.SubHeaderText;
			subHeaderText.text = subHeaderText.text + "\n" + @string;
			leftInfoTextBox.DescriptionText.text = "";
			leftInfoTextBox.SubHeaderText.GetComponent<TextGlyphConverter>().UpdateText(true);
			leftInfoTextBox.DescriptionText.GetComponent<TextGlyphConverter>().UpdateText(true);
			leftIcon.sprite = IconLibrary.GetAbilityIcon(abilityType, false);
		}
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x0009F6EC File Offset: 0x0009D8EC
	private string GetCostString(CastAbilityType castAbilityType)
	{
		string locID = null;
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			locID = "LOC_ID_RELIC_COST_WEAPON_SWAP_1";
			break;
		case CastAbilityType.Spell:
			locID = "LOC_ID_RELIC_COST_SPELL_SWAP_1";
			break;
		case CastAbilityType.Talent:
			locID = "LOC_ID_RELIC_COST_TALENT_SWAP_1";
			break;
		}
		float costAmount = RelicLibrary.GetRelicData(this.GetSwapRelic(castAbilityType)).CostAmount;
		float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
		int num = Mathf.RoundToInt(Mathf.Clamp(costAmount - relicCostMod, 0f, float.MaxValue) * 100f);
		return string.Format(LocalizationManager.GetString(locID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num);
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x0009F77A File Offset: 0x0009D97A
	public static int CalculateHealthCost(float cost, PlayerController playerController, bool applySkillReduc)
	{
		if (applySkillReduc)
		{
			cost = Mathf.Clamp(cost - SkillTreeLogicHelper.GetRelicCostMod(), 0f, 1f);
		}
		return Mathf.RoundToInt((float)playerController.ClassModdedMaxHealth * (1f + playerController.TraitMaxHealthMod) * cost);
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x0009F7B2 File Offset: 0x0009D9B2
	public void ChooseLeftAbility()
	{
		base.StartCoroutine(this.ChooseAbility(this.AbilityType));
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x0009F7C7 File Offset: 0x0009D9C7
	public void ChooseRightAbility()
	{
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x0009F7C9 File Offset: 0x0009D9C9
	private IEnumerator ChooseAbility(AbilityType abilityType)
	{
		base.LeftIcon.gameObject.SetActive(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		RewiredMapController.SetCurrentMapEnabled(false);
		playerController.Animator.SetBool("Victory", true);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		if (this.m_castAbilityTypeToSwap == CastAbilityType.Weapon && TraitManager.IsTraitActive(TraitType.CantAttack))
		{
			if (SaveManager.PlayerSaveData.CurrentCharacter.TraitOne == TraitType.CantAttack)
			{
				SaveManager.PlayerSaveData.CurrentCharacter.TraitOne = TraitType.CanNowAttack;
				TraitManager.SetTraitSeenState(TraitType.CanNowAttack, TraitSeenState.SeenTwice, true);
			}
			else if (SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo == TraitType.CantAttack)
			{
				SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo = TraitType.CanNowAttack;
				TraitManager.SetTraitSeenState(TraitType.CanNowAttack, TraitSeenState.SeenTwice, true);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, this, new TraitChangedEventArgs(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne, SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo));
		}
		AbilityDrop abilityDrop = new AbilityDrop(abilityType, this.m_castAbilityTypeToSwap);
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(abilityDrop);
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		playerController.Animator.SetBool("Victory", false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		ItemDropManager.DropSpecialItem(abilityDrop, false);
		SaveManager.PlayerSaveData.GetRelic(this.GetSwapRelic(this.m_castAbilityTypeToSwap)).SetLevel(1, true, true);
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponSwap).Level > 0 && SaveManager.PlayerSaveData.GetRelic(RelicType.TalentSwap).Level > 0 && SaveManager.PlayerSaveData.GetRelic(RelicType.SpellSwap).Level > 0)
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllBlessings, StoreType.All);
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		this.DisableProp(true);
		this.PropComplete();
		yield break;
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x0009F7DF File Offset: 0x0009D9DF
	private AbilityType[] GetAbilityArray(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		case CastAbilityType.Spell:
			return AbilityType_RL.ValidSpellTypeArray;
		case CastAbilityType.Talent:
			return AbilityType_RL.ValidTalentTypeArray;
		}
		return AbilityType_RL.ValidWeaponTypeArray;
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x0009F808 File Offset: 0x0009DA08
	private AbilityType GetCurrentPlayerAbility(CastAbilityType castAbilityType)
	{
		BaseAbility_RL ability = PlayerManager.GetPlayerController().CastAbility.GetAbility(castAbilityType, false);
		if (ability)
		{
			return ability.AbilityType;
		}
		return AbilityType.None;
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x0009F837 File Offset: 0x0009DA37
	private RelicType GetSwapRelic(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		default:
			return RelicType.WeaponSwap;
		case CastAbilityType.Spell:
			return RelicType.SpellSwap;
		case CastAbilityType.Talent:
			return RelicType.TalentSwap;
		}
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x0009F85C File Offset: 0x0009DA5C
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_closedSign.gameObject.SetActive(true);
		if (firstTimeDisabled)
		{
			base.StartCoroutine(this.ClosedSignPopupCoroutine());
		}
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x0009F886 File Offset: 0x0009DA86
	private IEnumerator ClosedSignPopupCoroutine()
	{
		Vector3 localPosition = this.m_closedSign.transform.localPosition;
		localPosition.y -= 2f;
		this.m_closedSign.transform.localPosition = localPosition;
		TweenManager.TweenBy(this.m_closedSign.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localPosition.y",
			2
		});
		float delayTime = Time.time + 0.2f;
		while (Time.time < delayTime)
		{
			yield return null;
		}
		EffectManager.PlayEffect(this.m_closedSign.gameObject, null, "LandedDust_Effect", this.m_closedSign.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		yield break;
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x0009F895 File Offset: 0x0009DA95
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x0009F8A4 File Offset: 0x0009DAA4
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x0009F8B9 File Offset: 0x0009DAB9
	public void RefreshText(object sender, EventArgs args)
	{
		this.InitializeTextBox();
	}

	// Token: 0x04002546 RID: 9542
	[SerializeField]
	private CastAbilityType m_castAbilityTypeToSwap;

	// Token: 0x04002547 RID: 9543
	[SerializeField]
	private GameObject m_closedSign;

	// Token: 0x04002548 RID: 9544
	private AbilityType m_abilityType;

	// Token: 0x04002549 RID: 9545
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400254A RID: 9546
	private int m_totalRoomRolls;

	// Token: 0x0400254B RID: 9547
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400254C RID: 9548
	private static List<AbilityType> m_potentialAbilityList = new List<AbilityType>();
}
