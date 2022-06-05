using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200082A RID: 2090
public class SwapAbilityRoomPropController : DualChoicePropController, ILocalizable
{
	// Token: 0x1700174D RID: 5965
	// (get) Token: 0x06004075 RID: 16501 RVA: 0x0002396A File Offset: 0x00021B6A
	public AbilityType AbilityType
	{
		get
		{
			return this.m_abilityType;
		}
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x00023972 File Offset: 0x00021B72
	protected override void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		base.Awake();
	}

	// Token: 0x06004077 RID: 16503 RVA: 0x00102EA8 File Offset: 0x001010A8
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

	// Token: 0x06004078 RID: 16504 RVA: 0x00102F70 File Offset: 0x00101170
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

	// Token: 0x06004079 RID: 16505 RVA: 0x00103080 File Offset: 0x00101280
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

	// Token: 0x0600407A RID: 16506 RVA: 0x001031C8 File Offset: 0x001013C8
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

	// Token: 0x0600407B RID: 16507 RVA: 0x000237FE File Offset: 0x000219FE
	public static int CalculateHealthCost(float cost, PlayerController playerController, bool applySkillReduc)
	{
		if (applySkillReduc)
		{
			cost = Mathf.Clamp(cost - SkillTreeLogicHelper.GetRelicCostMod(), 0f, 1f);
		}
		return Mathf.RoundToInt((float)playerController.ClassModdedMaxHealth * (1f + playerController.TraitMaxHealthMod) * cost);
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x0002399E File Offset: 0x00021B9E
	public void ChooseLeftAbility()
	{
		base.StartCoroutine(this.ChooseAbility(this.AbilityType));
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x00002FCA File Offset: 0x000011CA
	public void ChooseRightAbility()
	{
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x000239B3 File Offset: 0x00021BB3
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

	// Token: 0x0600407F RID: 16511 RVA: 0x000239C9 File Offset: 0x00021BC9
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

	// Token: 0x06004080 RID: 16512 RVA: 0x00103258 File Offset: 0x00101458
	private AbilityType GetCurrentPlayerAbility(CastAbilityType castAbilityType)
	{
		BaseAbility_RL ability = PlayerManager.GetPlayerController().CastAbility.GetAbility(castAbilityType, false);
		if (ability)
		{
			return ability.AbilityType;
		}
		return AbilityType.None;
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x000239F0 File Offset: 0x00021BF0
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

	// Token: 0x06004082 RID: 16514 RVA: 0x00023A15 File Offset: 0x00021C15
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_closedSign.gameObject.SetActive(true);
		if (firstTimeDisabled)
		{
			base.StartCoroutine(this.ClosedSignPopupCoroutine());
		}
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00023A3F File Offset: 0x00021C3F
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

	// Token: 0x06004084 RID: 16516 RVA: 0x00023A4E File Offset: 0x00021C4E
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x00023A5D File Offset: 0x00021C5D
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x00023A72 File Offset: 0x00021C72
	public void RefreshText(object sender, EventArgs args)
	{
		this.InitializeTextBox();
	}

	// Token: 0x04003276 RID: 12918
	[SerializeField]
	private CastAbilityType m_castAbilityTypeToSwap;

	// Token: 0x04003277 RID: 12919
	[SerializeField]
	private GameObject m_closedSign;

	// Token: 0x04003278 RID: 12920
	private AbilityType m_abilityType;

	// Token: 0x04003279 RID: 12921
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400327A RID: 12922
	private int m_totalRoomRolls;

	// Token: 0x0400327B RID: 12923
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400327C RID: 12924
	private static List<AbilityType> m_potentialAbilityList = new List<AbilityType>();
}
