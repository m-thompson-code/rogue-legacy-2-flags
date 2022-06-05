using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200097D RID: 2429
public class ChallengeOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, ChallengeOmniUIEntry>
{
	// Token: 0x170019D6 RID: 6614
	// (get) Token: 0x06004A7D RID: 19069 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170019D7 RID: 6615
	// (get) Token: 0x06004A7E RID: 19070 RVA: 0x00028C5C File Offset: 0x00026E5C
	public override WindowID ID
	{
		get
		{
			return WindowID.ChallengeNPC;
		}
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x00028C60 File Offset: 0x00026E60
	protected override void Awake()
	{
		base.Awake();
		this.m_onUpdateDescription = new Action<object, EventArgs>(this.OnUpdateDescription);
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x00121F70 File Offset: 0x00120170
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

	// Token: 0x06004A82 RID: 19074 RVA: 0x001220B4 File Offset: 0x001202B4
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

	// Token: 0x06004A83 RID: 19075 RVA: 0x00028C7A File Offset: 0x00026E7A
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x00122288 File Offset: 0x00120488
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

	// Token: 0x06004A85 RID: 19077 RVA: 0x00028C8F File Offset: 0x00026E8F
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

	// Token: 0x06004A86 RID: 19078 RVA: 0x001222D8 File Offset: 0x001204D8
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

	// Token: 0x06004A87 RID: 19079 RVA: 0x00122348 File Offset: 0x00120548
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

	// Token: 0x06004A88 RID: 19080 RVA: 0x001223A0 File Offset: 0x001205A0
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

	// Token: 0x06004A89 RID: 19081 RVA: 0x00028CC8 File Offset: 0x00026EC8
	public void ForceRefresh()
	{
		if (base.SelectedEntryIndex < base.ActiveEntryArray.Length)
		{
			base.ActiveEntryArray[base.SelectedEntryIndex].OnSelect(null);
		}
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLBButtonJustPressed()
	{
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnRBButtonJustPressed()
	{
	}

	// Token: 0x040038F7 RID: 14583
	[SerializeField]
	private GameObject m_descriptionGO;

	// Token: 0x040038F8 RID: 14584
	[SerializeField]
	private GameObject m_controlsGO;

	// Token: 0x040038F9 RID: 14585
	[SerializeField]
	private GameObject m_scoreboardGO;

	// Token: 0x040038FA RID: 14586
	private bool m_scoreboardActive;

	// Token: 0x040038FB RID: 14587
	private ChallengeOmniUIScoreboardEntry[] m_scoreboardEntries;

	// Token: 0x040038FC RID: 14588
	private Action<object, EventArgs> m_onUpdateDescription;
}
