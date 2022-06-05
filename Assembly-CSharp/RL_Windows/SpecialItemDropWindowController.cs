using System;
using System.Collections;
using Rewired;
using RLAudio;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000DF6 RID: 3574
	public class SpecialItemDropWindowController : WindowController, ILocalizable
	{
		// Token: 0x17002069 RID: 8297
		// (get) Token: 0x060064A0 RID: 25760 RVA: 0x00004A89 File Offset: 0x00002C89
		public override WindowID ID
		{
			get
			{
				return WindowID.SpecialItemDrop;
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x00037879 File Offset: 0x00035A79
		private void Awake()
		{
			this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x000378A0 File Offset: 0x00035AA0
		public override void Initialize()
		{
			base.Initialize();
			this.m_specialItemDropArray = new ISpecialItemDrop[5];
			this.m_waitYield = new WaitRL_Yield(0f, true);
			this.m_displayNextYield = new WaitUntil(() => this.m_displayNextEntry);
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x000378DC File Offset: 0x00035ADC
		public void AddSpecialItemDrop(ISpecialItemDrop specialItemDrop)
		{
			if (specialItemDrop != null)
			{
				this.m_specialItemDropArray[this.m_numActiveEntries] = specialItemDrop;
				this.m_numActiveEntries++;
			}
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x00175B70 File Offset: 0x00173D70
		private void ClearSpecialItemEntryArray()
		{
			this.m_numActiveEntries = 0;
			for (int i = 0; i < 5; i++)
			{
				this.m_specialItemDropArray[i] = null;
			}
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x00175B9C File Offset: 0x00173D9C
		protected override void OnFocus()
		{
			if (ReInput.isReady && base.RewiredPlayer != null)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x00175BE8 File Offset: 0x00173DE8
		protected override void OnLostFocus()
		{
			if (ReInput.isReady && base.RewiredPlayer != null)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x060064A7 RID: 25767 RVA: 0x000378FD File Offset: 0x00035AFD
		private void OnConfirmButtonDown(InputActionEventData obj)
		{
			if (this.m_currentEntryIndex >= this.m_numActiveEntries - 1)
			{
				RewiredMapController.SetCurrentMapEnabled(false);
				base.StartCoroutine(this.OnExitCoroutine());
				return;
			}
			this.m_displayNextEntry = true;
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x00175C34 File Offset: 0x00173E34
		protected override void OnOpen()
		{
			AudioManager.SetEnemySFXPaused(true);
			AudioManager.SetPlayerSFXPaused(true);
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
			AudioManager.PlayOneShot(null, "event:/Stingers/sting_chestItem", CameraController.GameCamera.transform.position);
			this.SetAnimatorToIdle();
			this.m_windowCanvas.gameObject.SetActive(true);
			this.m_currentEntryIndex = 0;
			base.StopAllCoroutines();
			if (this.m_numActiveEntries > 0)
			{
				base.StartCoroutine(this.RunOnEnterAnimation());
			}
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x00175CB0 File Offset: 0x00173EB0
		protected override void OnClose()
		{
			AudioManager.SetEnemySFXPaused(false);
			AudioManager.SetPlayerSFXPaused(false);
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
			this.m_windowCanvas.gameObject.SetActive(false);
			this.ClearSpecialItemEntryArray();
			SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Stage, SavingType.FileOnly, true, null);
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Equipment, SavingType.FileOnly, true, null);
			saveBatch.End();
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x0003792A File Offset: 0x00035B2A
		private void Update()
		{
			this.m_textBGCanvasGroup.alpha = this.m_descriptionText.alpha;
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x00037942 File Offset: 0x00035B42
		private IEnumerator RunOnEnterAnimation()
		{
			this.m_bgCanvasGroup.alpha = 0f;
			TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 0.25f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"alpha",
				0.75f
			});
			RewiredMapController.SetCurrentMapEnabled(true);
			while (this.m_currentEntryIndex < this.m_numActiveEntries)
			{
				ISpecialItemDrop specialItem = this.m_specialItemDropArray[this.m_currentEntryIndex];
				yield return this.DisplayEntryCoroutine(specialItem);
				yield return this.m_displayNextYield;
				yield return this.FadeOutAnimationCoroutine();
				this.m_displayNextEntry = false;
				this.m_currentEntryIndex++;
			}
			yield break;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x00037951 File Offset: 0x00035B51
		private IEnumerator FadeOutAnimationCoroutine()
		{
			this.m_animator.SetTrigger("Exit");
			this.m_waitYield.CreateNew(0.25f, true);
			yield return this.m_waitYield;
			yield break;
		}

		// Token: 0x060064AD RID: 25773 RVA: 0x00037960 File Offset: 0x00035B60
		private IEnumerator DisplayEntryCoroutine(ISpecialItemDrop specialItem)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			this.InitializeSpecialItemEntry(specialItem);
			this.m_waitYield.CreateNew(2.5f, true);
			yield return this.m_waitYield;
			this.ItemRevealedRelay.Dispatch(specialItem.SpecialItemType);
			RewiredMapController.SetCurrentMapEnabled(true);
			yield break;
		}

		// Token: 0x060064AE RID: 25774 RVA: 0x00175D10 File Offset: 0x00173F10
		private void InitializeSpecialItemEntry(ISpecialItemDrop specialItem)
		{
			this.m_iconFrameImage.gameObject.SetActive(true);
			this.m_iconFrameImage.gameObject.transform.localScale = Vector3.one;
			this.m_iconImage.transform.localScale = new Vector3(1f, 1f, 1f);
			SpecialItemType specialItemType = specialItem.SpecialItemType;
			string text;
			string text2;
			string text3;
			if (specialItemType <= SpecialItemType.Relic)
			{
				if (specialItemType == SpecialItemType.Blueprint)
				{
					IBlueprintDrop blueprintDrop = specialItem as IBlueprintDrop;
					EquipmentObj equipment = EquipmentManager.GetEquipment(blueprintDrop.CategoryType, blueprintDrop.EquipmentType);
					EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(blueprintDrop.CategoryType, blueprintDrop.EquipmentType);
					text = LocalizationManager.GetString("LOC_ID_BLUEPRINT_UI_BLUEPRINT_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					if (equipment.UpgradeBlueprintsFound > 0)
					{
						text2 = Equipment_EV.GetFormattedEquipmentName(equipment.CategoryType, equipment.EquipmentType);
						text2 = text2 + " +" + equipment.UpgradeBlueprintsFound.ToString();
					}
					else
					{
						text2 = Equipment_EV.GetFormattedEquipmentName(equipment.CategoryType, equipment.EquipmentType);
					}
					text3 = LocalizationManager.GetString(equipmentData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_iconFrameImage.sprite = this.m_genericIconFrameSprite;
					this.m_iconImage.sprite = IconLibrary.GetEquipmentIcon(blueprintDrop.CategoryType, blueprintDrop.EquipmentType);
					this.m_animator.SetTrigger("Blueprint");
					goto IL_61F;
				}
				if (specialItemType == SpecialItemType.Rune)
				{
					IRuneDrop runeDrop = specialItem as IRuneDrop;
					RuneData runeData = RuneLibrary.GetRuneData(runeDrop.RuneType);
					text = LocalizationManager.GetString("LOC_ID_RUNE_UI_RUNE_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text2 = LocalizationManager.GetString(runeData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text3 = LocalizationManager.GetString(runeData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_iconFrameImage.sprite = this.m_runeIconFrameSprite;
					this.m_iconImage.sprite = IconLibrary.GetRuneIcon(runeDrop.RuneType);
					this.m_animator.SetTrigger("Rune");
					goto IL_61F;
				}
				if (specialItemType == SpecialItemType.Relic)
				{
					IRelicDrop relicDrop = specialItem as IRelicDrop;
					RelicData relicData = RelicLibrary.GetRelicData(relicDrop.RelicType);
					text = LocalizationManager.GetString("LOC_ID_RELIC_UI_RELIC_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					if (relicDrop.RelicModType == RelicModType.DoubleRelic)
					{
						text += " <size=70%>X</size><size=130%>2</size>";
					}
					text2 = LocalizationManager.GetString(relicData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					bool flag;
					text3 = LocalizationManager.GetString(relicData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, out flag, false);
					if (flag)
					{
						int num = SaveManager.PlayerSaveData.GetRelic(relicDrop.RelicType).Level;
						if (relicDrop.RelicModType == RelicModType.DoubleRelic)
						{
							num++;
						}
						float value;
						float relicFormatString = Relic_EV.GetRelicFormatString(relicDrop.RelicType, num + 1, out value);
						text3 = string.Format(text3, relicFormatString.ToCIString(), value.ToCIString());
					}
					this.m_iconFrameImage.gameObject.SetActive(false);
					this.m_iconImage.sprite = IconLibrary.GetRelicSprite(relicDrop.RelicType, true);
					this.m_iconImage.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
					this.m_animator.SetTrigger("Item");
					goto IL_61F;
				}
			}
			else
			{
				if (specialItemType == SpecialItemType.Heirloom)
				{
					IHeirloomDrop heirloomDrop = specialItem as IHeirloomDrop;
					HeirloomData heirloomData = HeirloomLibrary.GetHeirloomData(heirloomDrop.HeirloomType);
					text = LocalizationManager.GetString("LOC_ID_HEIRLOOM_UI_HEIRLOOM_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text2 = LocalizationManager.GetString(heirloomData.TitleLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text3 = LocalizationManager.GetString(heirloomData.DescriptionLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_iconFrameImage.gameObject.SetActive(false);
					this.m_iconImage.sprite = IconLibrary.GetHeirloomSprite(heirloomDrop.HeirloomType);
					this.m_iconImage.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
					this.m_animator.SetTrigger("Item");
					goto IL_61F;
				}
				if (specialItemType == SpecialItemType.Ability)
				{
					IAbilityDrop abilityDrop = specialItem as IAbilityDrop;
					AbilityData abilityData = AbilityLibrary.GetAbility(abilityDrop.AbilityType).AbilityData;
					text = LocalizationManager.GetString("LOC_ID_RELIC_UI_SWAP_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text2 = LocalizationManager.GetString(abilityData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text3 = LocalizationManager.GetString(abilityData.Description, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_iconFrameImage.gameObject.SetActive(false);
					this.m_iconImage.sprite = IconLibrary.GetLargeAbilityIcon(abilityDrop.AbilityType, true);
					this.m_iconImage.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
					this.m_animator.SetTrigger("Item");
					goto IL_61F;
				}
				if (specialItemType == SpecialItemType.Challenge)
				{
					IChallengeDrop challengeDrop = specialItem as IChallengeDrop;
					ChallengeData challengeData = ChallengeLibrary.GetChallengeData(challengeDrop.ChallengeType);
					text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_CHALLENGE_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text2 = LocalizationManager.GetString(challengeData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					text3 = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_CHALLENGE_FOUND_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_iconFrameImage.sprite = this.m_genericIconFrameSprite;
					this.m_iconImage.sprite = IconLibrary.GetChallengeIcon(challengeDrop.ChallengeType, ChallengeLibrary.ChallengeIconEntryType.Challenge);
					this.m_animator.SetTrigger("Challenge");
					goto IL_61F;
				}
			}
			text = LocalizationManager.GetString("LOC_ID_DEFAULT_ITEM_UI_DEFAULT_FOUND_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			text2 = LocalizationManager.GetString("LOC_ID_CHEST_UI_DEFAULT_ITEM_NAME_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			text3 = LocalizationManager.GetString("LOC_ID_CHEST_UI_DEFAULT_DESCRIPTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_iconFrameImage.sprite = this.m_genericIconFrameSprite;
			this.m_animator.SetTrigger("Item");
			IL_61F:
			this.m_itemFoundText.text = text;
			this.m_itemNameText.text = text2;
			this.m_descriptionText.text = text3;
		}

		// Token: 0x060064AF RID: 25775 RVA: 0x00037976 File Offset: 0x00035B76
		private IEnumerator OnExitCoroutine()
		{
			TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			RewiredMapController.SetCurrentMapEnabled(false);
			this.m_animator.SetTrigger("Exit");
			this.m_waitYield.CreateNew(0.25f, true);
			yield return this.m_waitYield;
			RewiredMapController.SetCurrentMapEnabled(true);
			WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, false);
			yield break;
		}

		// Token: 0x060064B0 RID: 25776 RVA: 0x00176360 File Offset: 0x00174560
		private void SetAnimatorToIdle()
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in this.m_animator.parameters)
			{
				AnimatorControllerParameterType type = animatorControllerParameter.type;
				if (type != AnimatorControllerParameterType.Bool)
				{
					if (type == AnimatorControllerParameterType.Trigger)
					{
						this.m_animator.ResetTrigger(animatorControllerParameter.name);
					}
				}
				else
				{
					this.m_animator.SetBool(animatorControllerParameter.name, false);
				}
			}
		}

		// Token: 0x060064B1 RID: 25777 RVA: 0x001763C4 File Offset: 0x001745C4
		public void RefreshText(object sender, EventArgs args)
		{
			if (this.m_currentEntryIndex >= 0 && this.m_currentEntryIndex < this.m_specialItemDropArray.Length)
			{
				ISpecialItemDrop specialItem = this.m_specialItemDropArray[this.m_currentEntryIndex];
				this.InitializeSpecialItemEntry(specialItem);
			}
		}

		// Token: 0x04005207 RID: 20999
		private const int ITEM_DROP_ENTRY_POOLSIZE = 5;

		// Token: 0x04005208 RID: 21000
		[SerializeField]
		private CanvasGroup m_bgCanvasGroup;

		// Token: 0x04005209 RID: 21001
		[SerializeField]
		private CanvasGroup m_textBGCanvasGroup;

		// Token: 0x0400520A RID: 21002
		[Header("Text Objects")]
		[SerializeField]
		private TMP_Text m_itemFoundText;

		// Token: 0x0400520B RID: 21003
		[SerializeField]
		private TMP_Text m_itemNameText;

		// Token: 0x0400520C RID: 21004
		[SerializeField]
		private TMP_Text m_descriptionText;

		// Token: 0x0400520D RID: 21005
		[Header("Icons")]
		[SerializeField]
		private Image m_iconImage;

		// Token: 0x0400520E RID: 21006
		[SerializeField]
		private Image m_iconFrameImage;

		// Token: 0x0400520F RID: 21007
		[SerializeField]
		private Sprite m_genericIconFrameSprite;

		// Token: 0x04005210 RID: 21008
		[SerializeField]
		private Sprite m_runeIconFrameSprite;

		// Token: 0x04005211 RID: 21009
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04005212 RID: 21010
		private ISpecialItemDrop[] m_specialItemDropArray;

		// Token: 0x04005213 RID: 21011
		private int m_currentEntryIndex;

		// Token: 0x04005214 RID: 21012
		private int m_numActiveEntries;

		// Token: 0x04005215 RID: 21013
		private bool m_displayNextEntry;

		// Token: 0x04005216 RID: 21014
		private WaitRL_Yield m_waitYield;

		// Token: 0x04005217 RID: 21015
		private WaitUntil m_displayNextYield;

		// Token: 0x04005218 RID: 21016
		private Action<MonoBehaviour, EventArgs> m_refreshText;

		// Token: 0x04005219 RID: 21017
		public Relay<SpecialItemType> WindowOpenedRelay = new Relay<SpecialItemType>();

		// Token: 0x0400521A RID: 21018
		public Relay<SpecialItemType> WindowClosedRelay = new Relay<SpecialItemType>();

		// Token: 0x0400521B RID: 21019
		public Relay<SpecialItemType> ItemRevealedRelay = new Relay<SpecialItemType>();

		// Token: 0x0400521C RID: 21020
		private Action<InputActionEventData> m_onConfirmButtonDown;
	}
}
