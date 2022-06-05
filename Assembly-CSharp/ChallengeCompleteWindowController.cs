using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000573 RID: 1395
public class ChallengeCompleteWindowController : WindowController, ILocalizable
{
	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06003333 RID: 13107 RVA: 0x000AD18B File Offset: 0x000AB38B
	public override WindowID ID
	{
		get
		{
			return WindowID.ChallengeComplete;
		}
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x000AD18F File Offset: 0x000AB38F
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x000AD1B8 File Offset: 0x000AB3B8
	public override void Initialize()
	{
		this.m_canvasGroup = this.m_windowCanvas.GetComponent<CanvasGroup>();
		base.Initialize();
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x000AD208 File Offset: 0x000AB408
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.UpdateChallengeScoreAndRank();
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_playerModel.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		this.m_playerModel.Animator.SetBool("Victory", true);
		this.m_playerModel.Animator.Play("Victory", 0, 1f);
		this.m_biomeLightController.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x000AD298 File Offset: 0x000AB498
	private void UpdateChallengeScoreAndRank()
	{
		ChallengeType challengeType = ChallengeManager.ActiveChallenge.ChallengeType;
		ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(challengeType, true);
		ChallengeManager.UpdateChallengeScore(challengeType, SaveManager.PlayerSaveData.CurrentCharacter.ClassType, Mathf.RoundToInt(ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.FinalScore)), GlobalTimerHUDController.ElapsedTime, ChallengeManager.GetChallengeEquippedLevel(challengeType) > 0);
		this.m_empathiesUnlockedGO.SetActive(false);
		this.m_trophyAward.gameObject.SetActive(false);
		ChallengeTrophyRank challengeTrophyRank2 = ChallengeManager.GetChallengeTrophyRank(challengeType, false);
		if (challengeTrophyRank2 != challengeTrophyRank)
		{
			this.m_trophyAward.gameObject.SetActive(true);
			switch (challengeTrophyRank2)
			{
			case ChallengeTrophyRank.Bronze:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_BRONZE_REWARD_1", false, false);
				this.m_trophyImage.sprite = IconLibrary.GetChallengeIcon(challengeType, ChallengeLibrary.ChallengeIconEntryType.Bronze);
				break;
			case ChallengeTrophyRank.Silver:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_SILVER_REWARD_1", false, false);
				this.m_trophyImage.sprite = IconLibrary.GetChallengeIcon(challengeType, ChallengeLibrary.ChallengeIconEntryType.Silver);
				break;
			case ChallengeTrophyRank.Gold:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_GOLD_REWARD_1", false, false);
				this.m_trophyImage.sprite = IconLibrary.GetChallengeIcon(challengeType, ChallengeLibrary.ChallengeIconEntryType.Gold);
				if (challengeType != ChallengeType.Tutorial && challengeType != ChallengeType.TutorialPurified)
				{
					this.m_empathiesUnlockedGO.SetActive(true);
				}
				break;
			}
		}
		ChallengeTrophyRank challengeTrophyRank3 = challengeTrophyRank2;
		if (challengeTrophyRank2 == challengeTrophyRank && challengeTrophyRank2 < ChallengeTrophyRank.Gold)
		{
			challengeTrophyRank3 = challengeTrophyRank2 + 1;
		}
		if (challengeTrophyRank2 == ChallengeTrophyRank.Gold || challengeTrophyRank == ChallengeTrophyRank.Gold)
		{
			ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
			ChallengeManager.SetUpgradeBlueprintsFound(challengeType, challenge.MaxLevel, false, true);
		}
		this.UpdateAllStats(challengeTrophyRank3);
		this.m_refreshTextNewRank = challengeTrophyRank2;
		this.m_refreshTextGoalRank = challengeTrophyRank3;
		if (challengeType == ChallengeType.NightmareKhidr)
		{
			StoreAPIManager.GiveAchievement(AchievementType.NightmareKhidr, StoreType.All);
		}
		if (challengeType == ChallengeType.TutorialPurified)
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllScarsBronze, StoreType.All);
		}
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x000AD439 File Offset: 0x000AB639
	private IEnumerator OnOpenCoroutine()
	{
		this.m_canvasGroup.alpha = 0f;
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x000AD448 File Offset: 0x000AB648
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
		this.m_biomeLightController.SetActive(false);
		this.m_trophyAward.gameObject.SetActive(false);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x000AD485 File Offset: 0x000AB685
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x000AD4B7 File Offset: 0x000AB6B7
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x000AD4E9 File Offset: 0x000AB6E9
	private void OnCancelButtonDown(InputActionEventData data)
	{
		WindowManager.SetWindowIsOpen(WindowID.ChallengeComplete, false);
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x000AD4F4 File Offset: 0x000AB6F4
	private void UpdateAllStats(ChallengeTrophyRank goalRank)
	{
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		this.m_bannerName.text = LocalizationManager.GetLocalizedPlayerName(currentCharacter);
		if (ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.TutorialPurified)
		{
			this.m_challengeTitle.text = LocalizationManager.GetString(ChallengeManager.GetChallenge(ChallengeType.Tutorial).ChallengeData.Title, false, false);
		}
		else
		{
			this.m_challengeTitle.text = LocalizationManager.GetString(ChallengeManager.ActiveChallenge.ChallengeData.Title, false, false);
		}
		this.m_classTitle.text = string.Format(LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_CLASS_TITLE_1", currentCharacter.IsFemale, false), LocalizationManager.GetString(ClassLibrary.GetClassData(currentCharacter.ClassType).PassiveData.Title, currentCharacter.IsFemale, false));
		ChallengeCompleteStatsEntry[] statsEntries = this.m_statsEntries;
		for (int i = 0; i < statsEntries.Length; i++)
		{
			statsEntries[i].UpdateStat(goalRank);
		}
		this.m_ratingEntry.UpdateStat(goalRank);
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x000AD5E0 File Offset: 0x000AB7E0
	private void Update()
	{
		if (this.m_windowCanvas.gameObject.activeSelf)
		{
			float num = -5f * Time.unscaledDeltaTime;
			this.m_ray.transform.SetLocalEulerZ(this.m_ray.transform.localEulerAngles.z + num);
		}
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x000AD634 File Offset: 0x000AB834
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_refreshTextNewRank != ChallengeTrophyRank.None)
		{
			switch (this.m_refreshTextNewRank)
			{
			case ChallengeTrophyRank.Bronze:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_BRONZE_REWARD_1", false, false);
				break;
			case ChallengeTrophyRank.Silver:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_SILVER_REWARD_1", false, false);
				break;
			case ChallengeTrophyRank.Gold:
				this.m_trophyAward.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_GOLD_REWARD_1", false, false);
				break;
			}
		}
		if (this.m_refreshTextGoalRank != ChallengeTrophyRank.None)
		{
			this.UpdateAllStats(this.m_refreshTextGoalRank);
		}
	}

	// Token: 0x040027F5 RID: 10229
	[SerializeField]
	private Image m_ray;

	// Token: 0x040027F6 RID: 10230
	[SerializeField]
	private GameObject m_biomeLightController;

	// Token: 0x040027F7 RID: 10231
	[SerializeField]
	private TMP_Text m_bannerName;

	// Token: 0x040027F8 RID: 10232
	[SerializeField]
	private TMP_Text m_challengeTitle;

	// Token: 0x040027F9 RID: 10233
	[SerializeField]
	private TMP_Text m_classTitle;

	// Token: 0x040027FA RID: 10234
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x040027FB RID: 10235
	[SerializeField]
	private ChallengeCompleteStatsEntry[] m_statsEntries;

	// Token: 0x040027FC RID: 10236
	[SerializeField]
	private ChallengeCompleteStatsEntry m_ratingEntry;

	// Token: 0x040027FD RID: 10237
	[SerializeField]
	private TMP_Text m_trophyAward;

	// Token: 0x040027FE RID: 10238
	[SerializeField]
	private Image m_trophyImage;

	// Token: 0x040027FF RID: 10239
	[SerializeField]
	private GameObject m_empathiesUnlockedGO;

	// Token: 0x04002800 RID: 10240
	private CanvasGroup m_canvasGroup;

	// Token: 0x04002801 RID: 10241
	private ChallengeTrophyRank m_refreshTextNewRank;

	// Token: 0x04002802 RID: 10242
	private ChallengeTrophyRank m_refreshTextGoalRank;

	// Token: 0x04002803 RID: 10243
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002804 RID: 10244
	private Action<InputActionEventData> m_onCancelButtonDown;
}
