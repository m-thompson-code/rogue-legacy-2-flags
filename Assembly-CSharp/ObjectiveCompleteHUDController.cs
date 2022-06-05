using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Token: 0x0200038F RID: 911
public class ObjectiveCompleteHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x060021F1 RID: 8689 RVA: 0x0006BA40 File Offset: 0x00069C40
	private void Awake()
	{
		this.m_displayHUD = new Action<MonoBehaviour, EventArgs>(this.DisplayHUD);
		this.m_hideHUD = new Action<MonoBehaviour, EventArgs>(this.HideHUD);
		this.m_onPause = new Action<MonoBehaviour, EventArgs>(this.OnPause);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.DisplayObjectiveCompleteHUD, this.m_displayHUD);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_hideHUD);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_hideHUD);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GamePauseStateChange, this.m_onPause);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_canvasGroup.alpha = 0f;
		this.m_canvasGO.SetActive(false);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x0006BB08 File Offset: 0x00069D08
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.DisplayObjectiveCompleteHUD, this.m_displayHUD);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_hideHUD);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_hideHUD);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GamePauseStateChange, this.m_onPause);
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x0006BB5A File Offset: 0x00069D5A
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		this.HideHUD(null, null);
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x0006BB64 File Offset: 0x00069D64
	private void OnPause(object sender, EventArgs args)
	{
		GamePauseStateChangeEventArgs gamePauseStateChangeEventArgs = args as GamePauseStateChangeEventArgs;
		if (gamePauseStateChangeEventArgs != null)
		{
			if (gamePauseStateChangeEventArgs.IsPaused)
			{
				this.m_storedAlpha = this.m_canvasGroup.alpha;
				this.m_canvasGroup.alpha = 0f;
				if (!this.m_tweenIn.IsNativeNull())
				{
					this.m_tweenIn.SetPaused(true);
				}
				if (!this.m_tweenOut.IsNativeNull())
				{
					this.m_tweenOut.SetPaused(true);
					return;
				}
			}
			else
			{
				this.m_canvasGroup.alpha = this.m_storedAlpha;
				if (!this.m_tweenIn.IsNativeNull())
				{
					this.m_tweenIn.SetPaused(false);
				}
				if (!this.m_tweenOut.IsNativeNull())
				{
					this.m_tweenOut.SetPaused(false);
				}
			}
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x0006BC20 File Offset: 0x00069E20
	private void HideHUD(object sender, EventArgs args)
	{
		base.StopAllCoroutines();
		this.m_canvasGroup.alpha = 0f;
		if (!this.m_canvasGO.activeSelf)
		{
			this.m_canvasGO.SetActive(false);
		}
		this.m_storedAlpha = 0f;
		if (this.m_tweenIn != null)
		{
			this.m_tweenIn.StopTweenWithConditionChecks(false, this.m_canvasGroup, null);
		}
		if (this.m_tweenOut != null)
		{
			this.m_tweenOut.StopTweenWithConditionChecks(false, this.m_canvasGroup, null);
		}
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x0006BCAC File Offset: 0x00069EAC
	private void DisplayHUD(MonoBehaviour sender, EventArgs args)
	{
		this.HideHUD(null, null);
		ObjectiveCompleteHUDEventArgs objectiveCompleteHUDEventArgs = args as ObjectiveCompleteHUDEventArgs;
		this.m_refreshTextEventArgs = objectiveCompleteHUDEventArgs;
		this.UpdateObjectiveCompleteText(objectiveCompleteHUDEventArgs, true);
		this.m_canvasGO.SetActive(true);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.DisplayHUDCoroutine(objectiveCompleteHUDEventArgs.DisplayDuration));
		}
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x0006BD04 File Offset: 0x00069F04
	private void UpdateObjectiveCompleteText(ObjectiveCompleteHUDEventArgs objectiveArgs, bool runEvent)
	{
		this.m_playerNameText.gameObject.SetActive(false);
		this.m_titleText.gameObject.SetActive(true);
		this.m_subTitleText.gameObject.SetActive(true);
		this.m_descriptionText.gameObject.SetActive(true);
		this.m_descriptionSpacer.SetActive(false);
		switch (objectiveArgs.HUDType)
		{
		case ObjectiveCompleteHUDType.Boss:
			this.m_subTitleText.gameObject.SetActive(false);
			this.m_descriptionText.gameObject.SetActive(false);
			this.m_descriptionSpacer.SetActive(true);
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_BOSS_DEFEATED_TITLE_1", false, false);
			if (this.m_bossCompleteUnityEvent != null && runEvent)
			{
				this.m_bossCompleteUnityEvent.Invoke();
			}
			break;
		case ObjectiveCompleteHUDType.Heirloom:
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_HEIRLOOM_GAINED_TITLE_1", false, false);
			this.m_subTitleText.gameObject.SetActive(false);
			this.m_descriptionText.gameObject.SetActive(false);
			this.m_descriptionSpacer.SetActive(true);
			if (this.m_heirloomCompleteUnityEvent != null && runEvent)
			{
				this.m_heirloomCompleteUnityEvent.Invoke();
			}
			break;
		case ObjectiveCompleteHUDType.Insight:
		{
			InsightObjectiveCompleteHUDEventArgs insightObjectiveCompleteHUDEventArgs = objectiveArgs as InsightObjectiveCompleteHUDEventArgs;
			if (Insight_EV.LocIDTable.ContainsKey(insightObjectiveCompleteHUDEventArgs.InsightType))
			{
				InsightLocIDEntry insightLocIDEntry = Insight_EV.LocIDTable[insightObjectiveCompleteHUDEventArgs.InsightType];
				string locID;
				string locID2;
				if (insightObjectiveCompleteHUDEventArgs.Discovered)
				{
					locID = "LOC_ID_BIG_TEXT_UI_INSIGHT_DISCOVERY_TITLE_1";
					locID2 = "LOC_ID_BIG_TEXT_UI_INSIGHT_DISCLAIMER_1";
				}
				else
				{
					locID = "LOC_ID_BIG_TEXT_UI_INSIGHT_RESOLVED_TITLE_1";
					locID2 = insightLocIDEntry.ResolvedTextLocID;
				}
				this.m_titleText.text = LocalizationManager.GetString(locID, false, false);
				this.m_subTitleText.text = LocalizationManager.GetString(insightLocIDEntry.TitleLocID, false, false);
				this.m_descriptionText.text = LocalizationManager.GetString(locID2, false, false);
			}
			else
			{
				Debug.Log("<color=red>Insight: " + insightObjectiveCompleteHUDEventArgs.InsightType.ToString() + " could not be found in Insight_EV.LocIDTable.</color>");
			}
			if (this.m_insightCompleteUnityEvent != null && runEvent)
			{
				this.m_insightCompleteUnityEvent.Invoke();
			}
			break;
		}
		case ObjectiveCompleteHUDType.Traits:
		{
			this.m_playerNameText.gameObject.SetActive(true);
			CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
			bool isFemale = SaveManager.PlayerSaveData.CurrentCharacter.IsFemale;
			string localizedPlayerName = LocalizationManager.GetLocalizedPlayerName(currentCharacter);
			string @string = LocalizationManager.GetString(ClassLibrary.GetClassData(currentCharacter.ClassType).PassiveData.Title, isFemale, false);
			TraitData traitData = TraitLibrary.GetTraitData(currentCharacter.TraitOne);
			string text = "";
			string text2 = "";
			if (traitData != null)
			{
				if (currentCharacter.TraitOne == TraitType.Disposition)
				{
					text = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), isFemale, false);
					text2 = LocalizationManager.GetString(Gay_Trait.GetDispositionLocID(currentCharacter, true), isFemale, false);
				}
				else if (currentCharacter.TraitOne == TraitType.Antique)
				{
					RelicData relicData = (currentCharacter.AntiqueOneOwned != RelicType.None) ? RelicLibrary.GetRelicData(currentCharacter.AntiqueOneOwned) : null;
					text = ((relicData != null) ? LocalizationManager.GetString(relicData.Title, false, false) : "NULL RELIC");
					bool flag;
					text2 = LocalizationManager.GetString((relicData != null) ? relicData.Description : ("NULL ANTIQUE DESCRIPTION(" + currentCharacter.AntiqueOneOwned.ToString() + ")"), isFemale, out flag, false);
					if (flag)
					{
						float value;
						float relicFormatString = Relic_EV.GetRelicFormatString(currentCharacter.AntiqueOneOwned, 1, out value);
						text2 = string.Format(text2, relicFormatString.ToCIString(), value.ToCIString());
					}
				}
				else
				{
					text = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), isFemale, false);
					text2 = LocalizationManager.GetString(traitData.Description_2, isFemale, false);
				}
			}
			TraitData traitData2 = TraitLibrary.GetTraitData(currentCharacter.TraitTwo);
			string text3 = "";
			string text4 = "";
			if (traitData2 != null)
			{
				if (currentCharacter.TraitTwo == TraitType.Disposition)
				{
					text3 = LocalizationManager.GetString(traitData2.GetTraitTitleLocID(), isFemale, false);
					text4 = LocalizationManager.GetString(Gay_Trait.GetDispositionLocID(currentCharacter, true), isFemale, false);
				}
				else if (currentCharacter.TraitTwo == TraitType.Antique)
				{
					RelicData relicData2 = (currentCharacter.AntiqueTwoOwned != RelicType.None) ? RelicLibrary.GetRelicData(currentCharacter.AntiqueTwoOwned) : null;
					text3 = ((relicData2 != null) ? LocalizationManager.GetString(relicData2.Title, false, false) : "NULL RELIC");
					bool flag2;
					text4 = LocalizationManager.GetString((relicData2 != null) ? relicData2.Description : ("NULL ANTIQUE DESCRIPTION(" + currentCharacter.AntiqueTwoOwned.ToString() + ")"), isFemale, out flag2, false);
					if (flag2)
					{
						float value2;
						float relicFormatString2 = Relic_EV.GetRelicFormatString(currentCharacter.AntiqueTwoOwned, 1, out value2);
						text4 = string.Format(text4, relicFormatString2.ToCIString(), value2.ToCIString());
					}
				}
				else
				{
					text3 = LocalizationManager.GetString(traitData2.GetTraitTitleLocID(), isFemale, false);
					text4 = LocalizationManager.GetString(traitData2.Description_2, isFemale, false);
				}
			}
			this.m_playerNameText.text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_FULL_PLAYER_CLASS_FORMATTER_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), localizedPlayerName, @string);
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text3))
			{
				this.m_titleText.text = text + " + " + text3;
			}
			else
			{
				this.m_titleText.text = text + text3;
			}
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text4))
			{
				this.m_subTitleText.text = text2 + " + " + text4;
			}
			else
			{
				this.m_subTitleText.text = text2 + text4;
			}
			this.m_descriptionText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_VISIT_PAUSE_TEXT_1", false, false);
			break;
		}
		case ObjectiveCompleteHUDType.PizzaGirl:
			this.m_subTitleText.gameObject.SetActive(false);
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_PORTAL_UNLOCKED_TITLE_1", false, false);
			this.m_descriptionText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_PORTAL_UNLOCKED_TEXT_1", false, false);
			if (this.m_bossCompleteUnityEvent != null && runEvent)
			{
				this.m_bossCompleteUnityEvent.Invoke();
			}
			break;
		case ObjectiveCompleteHUDType.Scar:
		{
			ScarObjectiveCompleteHUDEventArgs scarObjectiveCompleteHUDEventArgs = objectiveArgs as ScarObjectiveCompleteHUDEventArgs;
			this.m_descriptionText.gameObject.SetActive(false);
			this.m_descriptionSpacer.SetActive(true);
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_SCAR_DISCOVERED_TITLE_1", false, false);
			this.m_subTitleText.text = LocalizationManager.GetString(ChallengeLibrary.GetChallengeData(scarObjectiveCompleteHUDEventArgs.ChallengeType).Title, false, false);
			if (this.m_insightCompleteUnityEvent != null && runEvent)
			{
				this.m_insightCompleteUnityEvent.Invoke();
			}
			break;
		}
		case ObjectiveCompleteHUDType.WorldComplete:
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_NG_UI_WORLD_COMPLETE_1", false, false);
			this.m_subTitleText.gameObject.SetActive(false);
			if (SaveManager.PlayerSaveData.HighestNGPlusBeaten == -1)
			{
				this.m_descriptionText.text = LocalizationManager.GetString("LOC_ID_NG_UI_NGPLUS_UNLOCKED_1", false, false);
			}
			else if (SaveManager.PlayerSaveData.NewGamePlusLevel > SaveManager.PlayerSaveData.HighestNGPlusBeaten)
			{
				this.m_descriptionText.text = LocalizationManager.GetString("LOC_ID_NG_UI_NEW_THREAD_1", false, false);
			}
			else
			{
				this.m_descriptionText.gameObject.SetActive(false);
				this.m_descriptionSpacer.SetActive(true);
			}
			break;
		case ObjectiveCompleteHUDType.LQA:
		{
			LQAObjectiveCompleteHUDEventArgs lqaobjectiveCompleteHUDEventArgs = objectiveArgs as LQAObjectiveCompleteHUDEventArgs;
			if (lqaobjectiveCompleteHUDEventArgs.DisplayPlayer)
			{
				this.m_playerNameText.gameObject.SetActive(true);
			}
			if (string.IsNullOrEmpty(lqaobjectiveCompleteHUDEventArgs.TitleTextOverride))
			{
				this.m_titleText.gameObject.SetActive(false);
			}
			else
			{
				this.m_titleText.text = LocalizationManager.GetString(lqaobjectiveCompleteHUDEventArgs.TitleTextOverride, false, false);
			}
			if (string.IsNullOrEmpty(lqaobjectiveCompleteHUDEventArgs.SubTitleTextOverride))
			{
				this.m_subTitleText.gameObject.SetActive(false);
			}
			else
			{
				this.m_subTitleText.text = LocalizationManager.GetString(lqaobjectiveCompleteHUDEventArgs.SubTitleTextOverride, false, false);
			}
			if (string.IsNullOrEmpty(lqaobjectiveCompleteHUDEventArgs.DescriptionTextOverride))
			{
				this.m_descriptionText.gameObject.SetActive(false);
			}
			else
			{
				this.m_descriptionText.text = LocalizationManager.GetString(lqaobjectiveCompleteHUDEventArgs.DescriptionTextOverride, false, false);
			}
			break;
		}
		}
		if (objectiveArgs.HUDType != ObjectiveCompleteHUDType.LQA)
		{
			if (!string.IsNullOrEmpty(objectiveArgs.TitleTextOverride))
			{
				this.m_titleText.text = objectiveArgs.TitleTextOverride;
			}
			if (!string.IsNullOrEmpty(objectiveArgs.SubTitleTextOverride))
			{
				this.m_subTitleText.text = objectiveArgs.SubTitleTextOverride;
			}
			if (!string.IsNullOrEmpty(objectiveArgs.DescriptionTextOverride))
			{
				this.m_descriptionText.text = objectiveArgs.DescriptionTextOverride;
			}
		}
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x0006C56B File Offset: 0x0006A76B
	private IEnumerator DisplayHUDCoroutine(float duration)
	{
		float num = 1f;
		float fadeOutTime = 1f;
		duration -= num + fadeOutTime;
		this.m_tweenIn = TweenManager.TweenTo(this.m_canvasGroup, num, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		yield return this.m_tweenIn.TweenCoroutine;
		if (duration > 0f)
		{
			this.m_waitYield.CreateNew(duration, false);
			yield return this.m_waitYield;
		}
		this.m_tweenOut = TweenManager.TweenTo(this.m_canvasGroup, fadeOutTime, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return this.m_tweenOut.TweenCoroutine;
		this.m_canvasGO.SetActive(false);
		yield break;
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x0006C581 File Offset: 0x0006A781
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x0006C590 File Offset: 0x0006A790
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x0006C59F File Offset: 0x0006A79F
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_refreshTextEventArgs != null)
		{
			this.UpdateObjectiveCompleteText(this.m_refreshTextEventArgs, false);
		}
	}

	// Token: 0x04001D6D RID: 7533
	[SerializeField]
	private TMP_Text m_playerNameText;

	// Token: 0x04001D6E RID: 7534
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001D6F RID: 7535
	[SerializeField]
	private TMP_Text m_subTitleText;

	// Token: 0x04001D70 RID: 7536
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x04001D71 RID: 7537
	[SerializeField]
	[Tooltip("Spacer used purely for keeping the spacing correct when disabling the description text.")]
	private GameObject m_descriptionSpacer;

	// Token: 0x04001D72 RID: 7538
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001D73 RID: 7539
	[SerializeField]
	private GameObject m_canvasGO;

	// Token: 0x04001D74 RID: 7540
	[SerializeField]
	private UnityEvent m_bossCompleteUnityEvent;

	// Token: 0x04001D75 RID: 7541
	[SerializeField]
	private UnityEvent m_insightCompleteUnityEvent;

	// Token: 0x04001D76 RID: 7542
	[SerializeField]
	private UnityEvent m_heirloomCompleteUnityEvent;

	// Token: 0x04001D77 RID: 7543
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001D78 RID: 7544
	private Tween m_tweenIn;

	// Token: 0x04001D79 RID: 7545
	private Tween m_tweenOut;

	// Token: 0x04001D7A RID: 7546
	private float m_storedAlpha;

	// Token: 0x04001D7B RID: 7547
	private ObjectiveCompleteHUDEventArgs m_refreshTextEventArgs;

	// Token: 0x04001D7C RID: 7548
	private Action<MonoBehaviour, EventArgs> m_displayHUD;

	// Token: 0x04001D7D RID: 7549
	private Action<MonoBehaviour, EventArgs> m_hideHUD;

	// Token: 0x04001D7E RID: 7550
	private Action<MonoBehaviour, EventArgs> m_onPause;

	// Token: 0x04001D7F RID: 7551
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
