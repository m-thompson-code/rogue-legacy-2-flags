using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000947 RID: 2375
public class ChallengeCompleteWindowController : WindowController, ILocalizable
{
	// Token: 0x17001948 RID: 6472
	// (get) Token: 0x06004820 RID: 18464 RVA: 0x000081DE File Offset: 0x000063DE
	public override WindowID ID
	{
		get
		{
			return WindowID.ChallengeComplete;
		}
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x0002797D File Offset: 0x00025B7D
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x001178B0 File Offset: 0x00115AB0
	public override void Initialize()
	{
		this.m_canvasGroup = this.m_windowCanvas.GetComponent<CanvasGroup>();
		base.Initialize();
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x00117900 File Offset: 0x00115B00
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

	// Token: 0x06004824 RID: 18468 RVA: 0x00117990 File Offset: 0x00115B90
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

	// Token: 0x06004825 RID: 18469 RVA: 0x000279A4 File Offset: 0x00025BA4
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

	// Token: 0x06004826 RID: 18470 RVA: 0x000279B3 File Offset: 0x00025BB3
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
		this.m_biomeLightController.SetActive(false);
		this.m_trophyAward.gameObject.SetActive(false);
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x000279F0 File Offset: 0x00025BF0
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x00027A22 File Offset: 0x00025C22
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x00027A54 File Offset: 0x00025C54
	private void OnCancelButtonDown(InputActionEventData data)
	{
		WindowManager.SetWindowIsOpen(WindowID.ChallengeComplete, false);
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x00117B34 File Offset: 0x00115D34
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

	// Token: 0x0600482B RID: 18475 RVA: 0x00117C20 File Offset: 0x00115E20
	private void Update()
	{
		if (this.m_windowCanvas.gameObject.activeSelf)
		{
			float num = -5f * Time.unscaledDeltaTime;
			this.m_ray.transform.SetLocalEulerZ(this.m_ray.transform.localEulerAngles.z + num);
		}
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x00117C74 File Offset: 0x00115E74
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

	// Token: 0x04003730 RID: 14128
	[SerializeField]
	private Image m_ray;

	// Token: 0x04003731 RID: 14129
	[SerializeField]
	private GameObject m_biomeLightController;

	// Token: 0x04003732 RID: 14130
	[SerializeField]
	private TMP_Text m_bannerName;

	// Token: 0x04003733 RID: 14131
	[SerializeField]
	private TMP_Text m_challengeTitle;

	// Token: 0x04003734 RID: 14132
	[SerializeField]
	private TMP_Text m_classTitle;

	// Token: 0x04003735 RID: 14133
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x04003736 RID: 14134
	[SerializeField]
	private ChallengeCompleteStatsEntry[] m_statsEntries;

	// Token: 0x04003737 RID: 14135
	[SerializeField]
	private ChallengeCompleteStatsEntry m_ratingEntry;

	// Token: 0x04003738 RID: 14136
	[SerializeField]
	private TMP_Text m_trophyAward;

	// Token: 0x04003739 RID: 14137
	[SerializeField]
	private Image m_trophyImage;

	// Token: 0x0400373A RID: 14138
	[SerializeField]
	private GameObject m_empathiesUnlockedGO;

	// Token: 0x0400373B RID: 14139
	private CanvasGroup m_canvasGroup;

	// Token: 0x0400373C RID: 14140
	private ChallengeTrophyRank m_refreshTextNewRank;

	// Token: 0x0400373D RID: 14141
	private ChallengeTrophyRank m_refreshTextGoalRank;

	// Token: 0x0400373E RID: 14142
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400373F RID: 14143
	private Action<InputActionEventData> m_onCancelButtonDown;
}
