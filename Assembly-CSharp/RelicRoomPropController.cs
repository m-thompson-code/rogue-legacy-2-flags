using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class RelicRoomPropController : DualChoicePropController, ILocalizable
{
	// Token: 0x17001743 RID: 5955
	// (get) Token: 0x06004048 RID: 16456 RVA: 0x000237BE File Offset: 0x000219BE
	public RelicType LeftRelicType
	{
		get
		{
			return (RelicType)this.m_relicTypes.x;
		}
	}

	// Token: 0x17001744 RID: 5956
	// (get) Token: 0x06004049 RID: 16457 RVA: 0x000237CB File Offset: 0x000219CB
	public RelicType RightRelicType
	{
		get
		{
			return (RelicType)this.m_relicTypes.y;
		}
	}

	// Token: 0x17001745 RID: 5957
	// (get) Token: 0x0600404A RID: 16458 RVA: 0x000237D8 File Offset: 0x000219D8
	public SpriteRenderer LeftIconTwin
	{
		get
		{
			return this.m_leftIconTwin;
		}
	}

	// Token: 0x17001746 RID: 5958
	// (get) Token: 0x0600404B RID: 16459 RVA: 0x000237E0 File Offset: 0x000219E0
	public SpriteRenderer RightIconTwin
	{
		get
		{
			return this.m_rightIconTwin;
		}
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00101DAC File Offset: 0x000FFFAC
	protected override void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		base.Awake();
		this.m_leftIconStartingPos = base.LeftIcon.transform.localPosition;
		this.m_rightIconStartingPos = base.RightIcon.transform.localPosition;
		this.m_leftIconStartingScale = base.LeftIcon.transform.localScale;
		this.m_rightIconStartingScale = base.RightIcon.transform.localScale;
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x00101E3C File Offset: 0x0010003C
	protected override void InitializePooledPropOnEnter()
	{
		base.InitializePooledPropOnEnter();
		this.m_totalRoomRolls = 1;
		int totalRoomRolls;
		if (int.TryParse(base.GetRoomMiscData("TotalRelicRolls"), NumberStyles.Any, SaveManager.CultureInfo, out totalRoomRolls))
		{
			this.m_totalRoomRolls = totalRoomRolls;
		}
		RelicRoomPropController.m_exclusionList.Clear();
		if (ChallengeManager.IsInChallenge)
		{
			RelicRoomPropController.m_exclusionList.AddRange(Challenge_EV.RELIC_EXCLUSION_ARRAY);
		}
		this.RollRelics(this.m_totalRoomRolls, false, true);
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x00101EAC File Offset: 0x001000AC
	public void RollRelics(int numRolls, bool addToTotalRoomRolls, bool rollMods = true)
	{
		if (this.m_leftTwinSpinCoroutine != null)
		{
			base.StopCoroutine(this.m_leftTwinSpinCoroutine);
		}
		if (this.m_rightTwinSpinCoroutine != null)
		{
			base.StopCoroutine(this.m_rightTwinSpinCoroutine);
		}
		if (addToTotalRoomRolls)
		{
			this.m_totalRoomRolls += numRolls;
			base.SetRoomMiscData("TotalRelicRolls", this.m_totalRoomRolls.ToString());
		}
		Prop component = base.GetComponent<Prop>();
		RelicPropTypeOverride relicPropTypeOverride = null;
		if (component && component.PropSpawnController)
		{
			relicPropTypeOverride = component.PropSpawnController.gameObject.GetComponent<RelicPropTypeOverride>();
		}
		for (int i = 0; i < numRolls; i++)
		{
			bool flag = false;
			bool flag2 = false;
			this.m_relicTypes.x = 0;
			this.m_relicTypes.y = 0;
			this.m_relicModTypes.x = 0;
			this.m_relicModTypes.y = 0;
			this.m_replacedRelic1 = RelicType.None;
			this.m_replacedRelic2 = RelicType.None;
			if (relicPropTypeOverride)
			{
				if (relicPropTypeOverride.Relic1Override != RelicType.None)
				{
					this.m_relicTypes.x = (int)relicPropTypeOverride.Relic1Override;
					flag = true;
				}
				if (relicPropTypeOverride.Relic2Override != RelicType.None)
				{
					this.m_relicTypes.y = (int)relicPropTypeOverride.Relic2Override;
					flag2 = true;
				}
				if ((relicPropTypeOverride.Relic1Override == RelicType.Lily1 || relicPropTypeOverride.Relic1Override == RelicType.Lily2 || relicPropTypeOverride.Relic1Override == RelicType.Lily3) && SaveManager.PlayerSaveData.GetInsightState(InsightType.ForestBoss_DoorOpened) >= InsightState.ResolvedButNotViewed)
				{
					flag = false;
				}
				if ((relicPropTypeOverride.Relic2Override == RelicType.Lily1 || relicPropTypeOverride.Relic2Override == RelicType.Lily2 || relicPropTypeOverride.Relic2Override == RelicType.Lily3) && SaveManager.PlayerSaveData.GetInsightState(InsightType.ForestBoss_DoorOpened) >= InsightState.ResolvedButNotViewed)
				{
					flag2 = false;
				}
				if ((relicPropTypeOverride.Relic1Override == RelicType.DragonKeyWhite && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened)) || (relicPropTypeOverride.Relic1Override == RelicType.DragonKeyBlack && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened)))
				{
					flag = false;
				}
				if ((relicPropTypeOverride.Relic2Override == RelicType.DragonKeyWhite && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened)) || (relicPropTypeOverride.Relic2Override == RelicType.DragonKeyBlack && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened)))
				{
					flag2 = false;
				}
				if (Global_EV.RELIC_ROOM_TEST_RELICS.x != 0)
				{
					this.m_relicTypes.x = Global_EV.RELIC_ROOM_TEST_RELICS.x;
					flag = true;
				}
				if (Global_EV.RELIC_ROOM_TEST_RELICS.y != 0)
				{
					this.m_relicTypes.y = Global_EV.RELIC_ROOM_TEST_RELICS.y;
					flag2 = true;
				}
			}
			RngID rngIDToUse = RngID.SpecialProps_RoomSeed;
			if (GameUtility.IsInLevelEditor)
			{
				rngIDToUse = RngID.None;
			}
			if (!flag)
			{
				this.m_relicTypes.x = (int)RelicLibrary.GetRandomRelic(rngIDToUse, false, RelicRoomPropController.m_exclusionList);
			}
			if (!flag2)
			{
				while (this.m_relicTypes.y == this.m_relicTypes.x || this.m_relicTypes.y == 0)
				{
					Debug.Log("Duplicate relic or not relic found.  Attempting to roll again...");
					this.m_relicTypes.y = (int)RelicLibrary.GetRandomRelic(rngIDToUse, false, RelicRoomPropController.m_exclusionList);
				}
			}
			if (!RelicRoomPropController.m_exclusionList.Contains((RelicType)this.m_relicTypes.x))
			{
				RelicRoomPropController.m_exclusionList.Add((RelicType)this.m_relicTypes.x);
			}
			if (!RelicRoomPropController.m_exclusionList.Contains((RelicType)this.m_relicTypes.y) && base.RightIcon.gameObject.activeSelf)
			{
				RelicRoomPropController.m_exclusionList.Add((RelicType)this.m_relicTypes.y);
			}
			if (rollMods)
			{
				if (!flag)
				{
					this.RollRelicMod(true, rngIDToUse);
				}
				if (!flag2)
				{
					this.RollRelicMod(false, rngIDToUse);
				}
			}
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic((RelicType)this.m_relicTypes.x);
			RelicData relicData = RelicLibrary.GetRelicData((RelicType)this.m_relicTypes.x);
			if (relic != null && relic.Level >= relicData.MaxStack)
			{
				this.m_replacedRelic1 = (RelicType)this.m_relicTypes.x;
				this.m_relicTypes.x = 600;
				this.m_relicModTypes.x = 0;
			}
			RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic((RelicType)this.m_relicTypes.y);
			RelicData relicData2 = RelicLibrary.GetRelicData((RelicType)this.m_relicTypes.y);
			if (relic2 != null && relic2.Level >= relicData2.MaxStack)
			{
				this.m_replacedRelic2 = (RelicType)this.m_relicTypes.y;
				this.m_relicTypes.y = 600;
				this.m_relicModTypes.y = 0;
			}
		}
		this.InitializeTextBox(true);
		this.InitializeTextBox(false);
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x001022EC File Offset: 0x001004EC
	private void RollRelicMod(bool leftSide, RngID rngIDToUse)
	{
		if (leftSide)
		{
			this.m_relicModTypes.x = 0;
		}
		else
		{
			this.m_relicModTypes.y = 0;
		}
		RelicType relicType = leftSide ? this.LeftRelicType : this.RightRelicType;
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
		if (relicType != RelicType.None)
		{
			RelicModType relicModType = RelicModType.None;
			if (TraitManager.IsTraitActive(TraitType.TwinRelics))
			{
				relicModType = RelicModType.DoubleRelic;
			}
			else
			{
				float num;
				if (rngIDToUse == RngID.None)
				{
					num = UnityEngine.Random.Range(0f, 1f);
				}
				else
				{
					num = RNGManager.GetRandomNumber(rngIDToUse, "RelicRoomPropController.RollRelicMods()", 0f, 1f);
				}
				if (num <= 0.2f)
				{
					int num2;
					if (rngIDToUse == RngID.None)
					{
						num2 = UnityEngine.Random.Range(1, RelicModType_RL.TypeArray.Length);
					}
					else
					{
						num2 = RNGManager.GetRandomNumber(rngIDToUse, "RelicRoomPropController.RollRelicMods()", 0, RelicModType_RL.TypeArray.Length);
					}
					relicModType = RelicModType_RL.TypeArray[num2];
				}
			}
			if (relicModType == RelicModType.DoubleRelic && relicData && relicData.MaxStack - relic.Level <= 1)
			{
				relicModType = RelicModType.None;
			}
			if (leftSide)
			{
				this.m_relicModTypes.x = (int)relicModType;
				return;
			}
			this.m_relicModTypes.y = (int)relicModType;
		}
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x001023F8 File Offset: 0x001005F8
	public void InitializeTextBox(bool leftSide)
	{
		PlayerManager.GetPlayerController();
		RelicType relicType;
		GenericInfoTextBox genericInfoTextBox;
		SpriteRenderer spriteRenderer;
		SpriteRenderer spriteRenderer2;
		RelicModType relicModType;
		SpriteRenderer spriteRenderer3;
		Vector3 localPosition;
		Vector3 localScale;
		if (leftSide)
		{
			relicType = this.LeftRelicType;
			genericInfoTextBox = base.LeftInfoTextBox;
			spriteRenderer = base.LeftIcon;
			spriteRenderer2 = this.m_leftIconTwin;
			relicModType = (RelicModType)this.m_relicModTypes.x;
			spriteRenderer3 = this.m_leftBeam;
			localPosition = this.m_leftIconStartingPos;
			localScale = this.m_leftIconStartingScale;
		}
		else
		{
			relicType = this.RightRelicType;
			genericInfoTextBox = base.RightInfoTextBox;
			spriteRenderer = base.RightIcon;
			spriteRenderer2 = this.m_rightIconTwin;
			relicModType = (RelicModType)this.m_relicModTypes.y;
			spriteRenderer3 = this.m_rightBeam;
			localPosition = this.m_rightIconStartingPos;
			localScale = this.m_rightIconStartingScale;
		}
		spriteRenderer.transform.localPosition = localPosition;
		spriteRenderer.transform.localScale = localScale;
		spriteRenderer2.gameObject.SetActive(false);
		if (spriteRenderer3)
		{
			if (relicModType != RelicModType.None)
			{
				spriteRenderer3.color = Color.yellow;
			}
			else
			{
				spriteRenderer3.color = Color.white;
			}
		}
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		if (relicData)
		{
			genericInfoTextBox.HeaderText.text = LocalizationManager.GetString(relicData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			if (relicModType == RelicModType.DoubleRelic)
			{
				spriteRenderer2.gameObject.SetActive(true);
				spriteRenderer2.sprite = IconLibrary.GetRelicSprite(relicType, false);
				if (leftSide)
				{
					if (this.m_leftTwinSpinCoroutine != null)
					{
						base.StopCoroutine(this.m_leftTwinSpinCoroutine);
					}
					this.m_leftTwinSpinCoroutine = base.StartCoroutine(this.TwinRelicAnimCoroutine(leftSide));
				}
				else
				{
					if (this.m_rightTwinSpinCoroutine != null)
					{
						base.StopCoroutine(this.m_rightTwinSpinCoroutine);
					}
					this.m_rightTwinSpinCoroutine = base.StartCoroutine(this.TwinRelicAnimCoroutine(leftSide));
				}
				genericInfoTextBox.HeaderText.text = string.Format(LocalizationManager.GetString("LOC_ID_RELIC_TITLE_APPEND_DoubleRelic_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), genericInfoTextBox.HeaderText.text);
			}
			if (relicType == RelicType.GoldDeathCurse)
			{
				int num = (int)((float)SaveManager.PlayerSaveData.GoldCollected * 1.35f);
				genericInfoTextBox.SubHeaderText.text = string.Format(LocalizationManager.GetString("LOC_ID_RELIC_COST_DEATH_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num);
			}
			else
			{
				float costAmount = relicData.CostAmount;
				float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
				int num2 = Mathf.RoundToInt(Mathf.Clamp(costAmount - relicCostMod, 0f, float.MaxValue) * 100f);
				if (relicModType == RelicModType.DoubleRelic)
				{
					num2 *= 2;
				}
				genericInfoTextBox.SubHeaderText.text = string.Format(LocalizationManager.GetString("LOC_ID_RELIC_UI_RESOLVE_COST_1", false, false), num2);
				bool flag;
				string text = LocalizationManager.GetString(relicData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, out flag, false);
				if (flag)
				{
					int num3 = SaveManager.PlayerSaveData.GetRelic(relicType).Level;
					if (relicModType == RelicModType.DoubleRelic)
					{
						num3++;
					}
					float value;
					float relicFormatString = Relic_EV.GetRelicFormatString(relicType, num3 + 1, out value);
					text = string.Format(text, relicFormatString.ToCIString(), value.ToCIString());
				}
				if (!SaveManager.PlayerSaveData.GetRelic(relicType).WasSeen && !ChallengeManager.IsInChallenge)
				{
					text = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_RELIC_1", false, false);
				}
				TMP_Text subHeaderText = genericInfoTextBox.SubHeaderText;
				subHeaderText.text = subHeaderText.text + "\n" + text;
			}
			genericInfoTextBox.DescriptionText.text = LocalizationManager.GetString(relicData.Description02, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			spriteRenderer.sprite = IconLibrary.GetRelicSprite(relicType, false);
			spriteRenderer.color = Color.white;
		}
	}

	// Token: 0x06004051 RID: 16465 RVA: 0x000237E8 File Offset: 0x000219E8
	private IEnumerator TwinRelicAnimCoroutine(bool leftSide)
	{
		GameObject icon;
		GameObject iconTwin;
		Vector3 startingPos;
		float startingScale;
		if (leftSide)
		{
			icon = base.LeftIcon.gameObject;
			iconTwin = this.m_leftIconTwin.gameObject;
			startingPos = this.m_leftIconStartingPos;
			startingScale = this.m_leftIconStartingScale.x;
		}
		else
		{
			icon = base.RightIcon.gameObject;
			iconTwin = this.m_rightIconTwin.gameObject;
			startingPos = this.m_rightIconStartingPos;
			startingScale = this.m_rightIconStartingScale.x;
		}
		for (;;)
		{
			float num = Mathf.Sin(Time.timeSinceLevelLoad * 1.5f) / 1.5f;
			float xPos = startingPos.x + num;
			icon.transform.SetLocalPositionX(xPos);
			float xPos2 = startingPos.x - num;
			iconTwin.transform.SetLocalPositionX(xPos2);
			float zPos;
			float num2 = zPos = Mathf.Cos(Time.timeSinceLevelLoad * 1.5f) / 1.5f;
			icon.transform.SetLocalPositionZ(zPos);
			float zPos2 = -num2;
			iconTwin.transform.SetLocalPositionZ(zPos2);
			float num3 = num2 * 0.5f;
			float num4 = startingScale - num3;
			icon.transform.localScale = new Vector3(num4, num4, num4);
			float num5 = startingScale + num3;
			iconTwin.transform.localScale = new Vector3(num5, num5, num5);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x000237FE File Offset: 0x000219FE
	public static int CalculateHealthCost(float cost, PlayerController playerController, bool applySkillReduc)
	{
		if (applySkillReduc)
		{
			cost = Mathf.Clamp(cost - SkillTreeLogicHelper.GetRelicCostMod(), 0f, 1f);
		}
		return Mathf.RoundToInt((float)playerController.ClassModdedMaxHealth * (1f + playerController.TraitMaxHealthMod) * cost);
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x00102768 File Offset: 0x00100968
	private string GetHealthCostString(RelicData relicData)
	{
		PlayerManager.GetPlayerController();
		RelicCostType costType = relicData.CostType;
		if (costType == RelicCostType.MaxHealthDamage || costType != RelicCostType.MaxHealthPermanent)
		{
			return LocalizationManager.GetString("LOC_ID_RELIC_COST_DAMAGE_HEALTH_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		return LocalizationManager.GetString("LOC_ID_RELIC_COST_MAX_HEALTH_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x00023836 File Offset: 0x00021A36
	public void ChooseLeftRelic()
	{
		base.StartCoroutine(this.ChooseRelic(this.LeftRelicType, (RelicModType)this.m_relicModTypes.x));
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x00023856 File Offset: 0x00021A56
	public void ChooseRightRelic()
	{
		base.StartCoroutine(this.ChooseRelic(this.RightRelicType, (RelicModType)this.m_relicModTypes.y));
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x00023876 File Offset: 0x00021A76
	private IEnumerator ChooseRelic(RelicType relicType, RelicModType relicModType)
	{
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterHitResponse.StopInvincibleTime();
		int previousMaxHP = playerController.ActualMaxHealth;
		RewiredMapController.SetCurrentMapEnabled(false);
		playerController.Animator.SetBool("Victory", true);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		RelicDrop relicDrop = new RelicDrop(relicType, relicModType);
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(relicDrop);
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		playerController.Animator.SetBool("Victory", false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (relicType == RelicType.GoldDeathCurse)
		{
			SaveManager.PlayerSaveData.GoldCollected = (int)((float)SaveManager.PlayerSaveData.GoldCollected * 1.35f);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
			RewiredMapController.SetCurrentMapEnabled(true);
			playerController.DisableArmor = true;
			playerController.CharacterHitResponse.StartHitResponse(playerController.gameObject, playerController, 2.1474836E+09f, true, true);
			playerController.DisableArmor = false;
		}
		else
		{
			playerController.InitializeHealthMods();
			playerController.InitializeManaMods();
		}
		if (relicType != RelicType.GoldDeathCurse)
		{
			ItemDropManager.DropSpecialItem(relicDrop, false);
			RewiredMapController.SetCurrentMapEnabled(true);
			float costAmount = relicData.CostAmount;
			float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
			int num = Mathf.RoundToInt(Mathf.Clamp(costAmount - relicCostMod, 0f, float.MaxValue) * 100f);
			if (relicModType == RelicModType.DoubleRelic)
			{
				num *= 2;
			}
			int num2 = previousMaxHP - playerController.ActualMaxHealth;
			if (num > 0)
			{
				string text = string.Format(LocalizationManager.GetString("LOC_ID_RELIC_UI_RESOLVE_LOST_1", false, false), -num);
				if (num2 > 0)
				{
					text = text + "\n" + string.Format(LocalizationManager.GetString("LOC_ID_RELIC_UI_HP_LOST_1", false, false), -num2);
				}
				TextPopupManager.DisplayTextDefaultPos(TextPopupType.OutOfAmmo, text, playerController, true, true);
			}
			this.DisableProp(true);
		}
		else
		{
			this.DisableProp(false);
		}
		this.PropComplete();
		yield break;
	}

	// Token: 0x06004057 RID: 16471 RVA: 0x001027C0 File Offset: 0x001009C0
	protected override void DisableProp(bool firstTimeDisabled)
	{
		if (firstTimeDisabled)
		{
			base.Animator.SetBool("Used", true);
		}
		else
		{
			base.Animator.SetBool("InstantlyUsed", true);
		}
		base.DisableProp(firstTimeDisabled);
		this.m_leftIconTwin.gameObject.SetActive(false);
		this.m_rightIconTwin.gameObject.SetActive(false);
	}

	// Token: 0x06004058 RID: 16472 RVA: 0x00023893 File Offset: 0x00021A93
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06004059 RID: 16473 RVA: 0x000238A2 File Offset: 0x00021AA2
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x0600405A RID: 16474 RVA: 0x000238B7 File Offset: 0x00021AB7
	public void RefreshText(object sender, EventArgs args)
	{
		this.InitializeTextBox(true);
		this.InitializeTextBox(false);
	}

	// Token: 0x0400324C RID: 12876
	[SerializeField]
	private SpriteRenderer m_leftBeam;

	// Token: 0x0400324D RID: 12877
	[SerializeField]
	private SpriteRenderer m_rightBeam;

	// Token: 0x0400324E RID: 12878
	[SerializeField]
	private SpriteRenderer m_leftIconTwin;

	// Token: 0x0400324F RID: 12879
	[SerializeField]
	private SpriteRenderer m_rightIconTwin;

	// Token: 0x04003250 RID: 12880
	private Vector2Int m_relicTypes;

	// Token: 0x04003251 RID: 12881
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003252 RID: 12882
	private int m_totalRoomRolls;

	// Token: 0x04003253 RID: 12883
	private Vector2Int m_relicModTypes;

	// Token: 0x04003254 RID: 12884
	private Vector3 m_leftIconStartingPos;

	// Token: 0x04003255 RID: 12885
	private Vector3 m_rightIconStartingPos;

	// Token: 0x04003256 RID: 12886
	private Vector3 m_leftIconStartingScale;

	// Token: 0x04003257 RID: 12887
	private Vector3 m_rightIconStartingScale;

	// Token: 0x04003258 RID: 12888
	private Coroutine m_leftTwinSpinCoroutine;

	// Token: 0x04003259 RID: 12889
	private Coroutine m_rightTwinSpinCoroutine;

	// Token: 0x0400325A RID: 12890
	private RelicType m_replacedRelic1;

	// Token: 0x0400325B RID: 12891
	private RelicType m_replacedRelic2;

	// Token: 0x0400325C RID: 12892
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400325D RID: 12893
	private static List<RelicType> m_exclusionList = new List<RelicType>();
}
