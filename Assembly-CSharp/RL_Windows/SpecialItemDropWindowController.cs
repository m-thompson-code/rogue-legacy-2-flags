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
	// Token: 0x020008BB RID: 2235
	public class SpecialItemDropWindowController : WindowController, ILocalizable
	{
		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06004908 RID: 18696 RVA: 0x001069CB File Offset: 0x00104BCB
		public override WindowID ID
		{
			get
			{
				return WindowID.SpecialItemDrop;
			}
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x001069CF File Offset: 0x00104BCF
		private void Awake()
		{
			this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x001069F6 File Offset: 0x00104BF6
		public override void Initialize()
		{
			base.Initialize();
			this.m_specialItemDropArray = new ISpecialItemDrop[5];
			this.m_waitYield = new WaitRL_Yield(0f, true);
			this.m_displayNextYield = new WaitUntil(() => this.m_displayNextEntry);
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x00106A32 File Offset: 0x00104C32
		public void AddSpecialItemDrop(ISpecialItemDrop specialItemDrop)
		{
			if (specialItemDrop != null)
			{
				this.m_specialItemDropArray[this.m_numActiveEntries] = specialItemDrop;
				this.m_numActiveEntries++;
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00106A54 File Offset: 0x00104C54
		private void ClearSpecialItemEntryArray()
		{
			this.m_numActiveEntries = 0;
			for (int i = 0; i < 5; i++)
			{
				this.m_specialItemDropArray[i] = null;
			}
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00106A80 File Offset: 0x00104C80
		protected override void OnFocus()
		{
			if (ReInput.isReady && base.RewiredPlayer != null)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x00106ACC File Offset: 0x00104CCC
		protected override void OnLostFocus()
		{
			if (ReInput.isReady && base.RewiredPlayer != null)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			}
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x00106B18 File Offset: 0x00104D18
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

		// Token: 0x06004910 RID: 18704 RVA: 0x00106B48 File Offset: 0x00104D48
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

		// Token: 0x06004911 RID: 18705 RVA: 0x00106BC4 File Offset: 0x00104DC4
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

		// Token: 0x06004912 RID: 18706 RVA: 0x00106C22 File Offset: 0x00104E22
		private void Update()
		{
			this.m_textBGCanvasGroup.alpha = this.m_descriptionText.alpha;
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x00106C3A File Offset: 0x00104E3A
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

		// Token: 0x06004914 RID: 18708 RVA: 0x00106C49 File Offset: 0x00104E49
		private IEnumerator FadeOutAnimationCoroutine()
		{
			this.m_animator.SetTrigger("Exit");
			this.m_waitYield.CreateNew(0.25f, true);
			yield return this.m_waitYield;
			yield break;
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x00106C58 File Offset: 0x00104E58
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

		// Token: 0x06004916 RID: 18710 RVA: 0x00106C70 File Offset: 0x00104E70
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

		// Token: 0x06004917 RID: 18711 RVA: 0x001072C0 File Offset: 0x001054C0
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

		// Token: 0x06004918 RID: 18712 RVA: 0x001072D0 File Offset: 0x001054D0
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

		// Token: 0x06004919 RID: 18713 RVA: 0x00107334 File Offset: 0x00105534
		public void RefreshText(object sender, EventArgs args)
		{
			if (this.m_currentEntryIndex >= 0 && this.m_currentEntryIndex < this.m_specialItemDropArray.Length)
			{
				ISpecialItemDrop specialItem = this.m_specialItemDropArray[this.m_currentEntryIndex];
				this.InitializeSpecialItemEntry(specialItem);
			}
		}

		// Token: 0x04003D9E RID: 15774
		private const int ITEM_DROP_ENTRY_POOLSIZE = 5;

		// Token: 0x04003D9F RID: 15775
		[SerializeField]
		private CanvasGroup m_bgCanvasGroup;

		// Token: 0x04003DA0 RID: 15776
		[SerializeField]
		private CanvasGroup m_textBGCanvasGroup;

		// Token: 0x04003DA1 RID: 15777
		[Header("Text Objects")]
		[SerializeField]
		private TMP_Text m_itemFoundText;

		// Token: 0x04003DA2 RID: 15778
		[SerializeField]
		private TMP_Text m_itemNameText;

		// Token: 0x04003DA3 RID: 15779
		[SerializeField]
		private TMP_Text m_descriptionText;

		// Token: 0x04003DA4 RID: 15780
		[Header("Icons")]
		[SerializeField]
		private Image m_iconImage;

		// Token: 0x04003DA5 RID: 15781
		[SerializeField]
		private Image m_iconFrameImage;

		// Token: 0x04003DA6 RID: 15782
		[SerializeField]
		private Sprite m_genericIconFrameSprite;

		// Token: 0x04003DA7 RID: 15783
		[SerializeField]
		private Sprite m_runeIconFrameSprite;

		// Token: 0x04003DA8 RID: 15784
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04003DA9 RID: 15785
		private ISpecialItemDrop[] m_specialItemDropArray;

		// Token: 0x04003DAA RID: 15786
		private int m_currentEntryIndex;

		// Token: 0x04003DAB RID: 15787
		private int m_numActiveEntries;

		// Token: 0x04003DAC RID: 15788
		private bool m_displayNextEntry;

		// Token: 0x04003DAD RID: 15789
		private WaitRL_Yield m_waitYield;

		// Token: 0x04003DAE RID: 15790
		private WaitUntil m_displayNextYield;

		// Token: 0x04003DAF RID: 15791
		private Action<MonoBehaviour, EventArgs> m_refreshText;

		// Token: 0x04003DB0 RID: 15792
		public Relay<SpecialItemType> WindowOpenedRelay = new Relay<SpecialItemType>();

		// Token: 0x04003DB1 RID: 15793
		public Relay<SpecialItemType> WindowClosedRelay = new Relay<SpecialItemType>();

		// Token: 0x04003DB2 RID: 15794
		public Relay<SpecialItemType> ItemRevealedRelay = new Relay<SpecialItemType>();

		// Token: 0x04003DB3 RID: 15795
		private Action<InputActionEventData> m_onConfirmButtonDown;
	}
}
