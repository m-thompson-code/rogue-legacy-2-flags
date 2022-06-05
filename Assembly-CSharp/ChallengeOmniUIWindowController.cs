using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000585 RID: 1413
public class ChallengeOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, ChallengeOmniUIEntry>
{
	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x060034C0 RID: 13504 RVA: 0x000B51A4 File Offset: 0x000B33A4
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x060034C1 RID: 13505 RVA: 0x000B51A7 File Offset: 0x000B33A7
	public override WindowID ID
	{
		get
		{
			return WindowID.ChallengeNPC;
		}
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000B51AB File Offset: 0x000B33AB
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x000B51AD File Offset: 0x000B33AD
	protected override void Awake()
	{
		base.Awake();
		this.m_onUpdateDescription = new Action<object, EventArgs>(this.OnUpdateDescription);
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000B51C8 File Offset: 0x000B33C8
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		Array challenge_ORDER = Challenge_EV.CHALLENGE_ORDER;
		base.EntryArray = new ChallengeOmniUIEntry[challenge_ORDER.Length];
		int num = 0;
		foreach (object obj in challenge_ORDER)
		{
			ChallengeType challengeType = (ChallengeType)obj;
			if (challengeType != ChallengeType.None && challengeType != ChallengeType.TutorialPurified)
			{
				base.EntryArray[num] = UnityEngine.Object.Instantiate<ChallengeOmniUIEntry>(this.m_entryPrefab);
				base.EntryArray[num].transform.SetParent(base.EntryLayoutGroup.transform);
				base.EntryArray[num].transform.localScale = Vector3.one;
				base.EntryArray[num].Initialize(challengeType, this);
				base.EntryArray[num].SetEntryIndex(num);
				num++;
			}
		}
		this.m_scoreboardEntries = this.m_scoreboardGO.GetComponentsInChildren<ChallengeOmniUIScoreboardEntry>(true);
		this.m_descriptionGO.SetActive(true);
		this.m_controlsGO.SetActive(true);
		this.m_scoreboardGO.SetActive(false);
		base.ResetTextbox.SetActive(false);
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000B530C File Offset: 0x000B350C
	protected override void UpdateScrollArrows(float scrollAmount)
	{
		base.UpdateScrollArrows(scrollAmount);
		if (base.TopScrollArrow.activeSelf || base.BottomScrollArrow.activeSelf)
		{
			float num = (float)base.ActiveEntryArray.Length * this.m_entryHeight;
			float num2 = Mathf.Clamp(1f - base.ScrollBar.value, 0f, 1f);
			float height = base.ContentViewport.rect.height;
			float num3 = height;
			float num4 = num3 - height;
			if (num2 > 0f)
			{
				num3 = height + (num - height) * num2;
				num4 = num3 - height;
			}
			int num5 = Mathf.CeilToInt(num4 / this.m_entryHeight);
			int num6 = Mathf.FloorToInt(num3 / this.m_entryHeight);
			if (base.TopScrollArrow.activeSelf)
			{
				bool flag = false;
				for (int i = 0; i < num5; i++)
				{
					if (ChallengeManager.GetFoundState(base.ActiveEntryArray[i].ChallengeType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
				else
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag2 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					if (ChallengeManager.GetFoundState(base.ActiveEntryArray[j].ChallengeType) == FoundState.FoundButNotViewed)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
					return;
				}
				this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
				this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
			}
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000B54DE File Offset: 0x000B36DE
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x000B54F4 File Offset: 0x000B36F4
	protected override void OnOpen()
	{
		base.OnOpen();
		this.SetScoreboardActive(false, false);
		this.SetGameObjectActive(base.ResetTextbox, false);
		if (ChallengeManager.GetTotalTrophiesEarnedOfRankOrHigher(ChallengeTrophyRank.Bronze, true) >= ChallengeManager.GetTotalTrophyCount())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllScarsBronze, StoreType.All);
		}
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateDescription, this.m_onUpdateDescription);
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x000B5543 File Offset: 0x000B3743
	protected override void OnClose()
	{
		base.OnClose();
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ResetBaseValues();
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ChallengeNPC_Menu_Closed, this, null);
		SaveManager.PlayerSaveData.UpdateCachedData();
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateDescription, this.m_onUpdateDescription);
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x000B557C File Offset: 0x000B377C
	private void OnUpdateDescription(object sender, EventArgs args)
	{
		this.SetScoreboardActive(false, false);
		ChallengeOmniUIDescriptionEventArgs challengeOmniUIDescriptionEventArgs = args as ChallengeOmniUIDescriptionEventArgs;
		if (challengeOmniUIDescriptionEventArgs != null)
		{
			ChallengeObj challenge = ChallengeManager.GetChallenge(challengeOmniUIDescriptionEventArgs.ChallengeType);
			if (challenge != null)
			{
				if (challenge.FoundState >= FoundState.FoundButNotViewed && challenge.ScoringType == ChallengeScoringType.Battle && challengeOmniUIDescriptionEventArgs.ChallengeType != ChallengeType.Tutorial && challengeOmniUIDescriptionEventArgs.ChallengeType != ChallengeType.TutorialPurified)
				{
					this.SetGameObjectActive(base.ResetTextbox, true);
					return;
				}
				this.SetGameObjectActive(base.ResetTextbox, false);
			}
		}
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x000B55EC File Offset: 0x000B37EC
	protected override void OnYButtonJustPressed()
	{
		base.OnYButtonJustPressed();
		ChallengeObj challenge = ChallengeManager.GetChallenge(base.ActiveEntryArray[base.SelectedEntryIndex].ChallengeType);
		if (challenge != null)
		{
			FoundState foundState = challenge.FoundState;
			if (base.ResetTextbox.activeSelf && foundState != FoundState.NotFound)
			{
				this.SetScoreboardActive(!this.m_scoreboardActive, true);
			}
		}
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x000B5644 File Offset: 0x000B3844
	private void SetScoreboardActive(bool active, bool playAudio)
	{
		if (this.m_scoreboardActive != active)
		{
			if (active)
			{
				ChallengeType challengeType = base.ActiveEntryArray[base.SelectedEntryIndex].ChallengeType;
				ChallengeOmniUIScoreboardEntry[] scoreboardEntries = this.m_scoreboardEntries;
				for (int i = 0; i < scoreboardEntries.Length; i++)
				{
					scoreboardEntries[i].UpdateEntry(challengeType);
				}
			}
			if (playAudio)
			{
				AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_menu_tab_over", default(Vector3));
			}
			this.SetGameObjectActive(this.m_descriptionGO, !active);
			this.SetGameObjectActive(this.m_controlsGO, !active);
			this.SetGameObjectActive(this.m_scoreboardGO, active);
			this.m_scoreboardActive = active;
		}
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x000B56D9 File Offset: 0x000B38D9
	public void ForceRefresh()
	{
		if (base.SelectedEntryIndex < base.ActiveEntryArray.Length)
		{
			base.ActiveEntryArray[base.SelectedEntryIndex].OnSelect(null);
		}
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x000B56FE File Offset: 0x000B38FE
	protected override void OnLBButtonJustPressed()
	{
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x000B5700 File Offset: 0x000B3900
	protected override void OnRBButtonJustPressed()
	{
	}

	// Token: 0x0400293C RID: 10556
	[SerializeField]
	private GameObject m_descriptionGO;

	// Token: 0x0400293D RID: 10557
	[SerializeField]
	private GameObject m_controlsGO;

	// Token: 0x0400293E RID: 10558
	[SerializeField]
	private GameObject m_scoreboardGO;

	// Token: 0x0400293F RID: 10559
	private bool m_scoreboardActive;

	// Token: 0x04002940 RID: 10560
	private ChallengeOmniUIScoreboardEntry[] m_scoreboardEntries;

	// Token: 0x04002941 RID: 10561
	private Action<object, EventArgs> m_onUpdateDescription;
}
