using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200039D RID: 925
public class ChallengeOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x0600227A RID: 8826 RVA: 0x0006FDAB File Offset: 0x0006DFAB
	// (set) Token: 0x0600227B RID: 8827 RVA: 0x0006FDB3 File Offset: 0x0006DFB3
	public ChallengeType ChallengeType { get; protected set; }

	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x0600227C RID: 8828 RVA: 0x0006FDBC File Offset: 0x0006DFBC
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new ChallengeOmniUIDescriptionEventArgs(this.ChallengeType, OmniUIButtonType.Purchasing);
			}
			else
			{
				this.m_eventArgs.Initialize(this.ChallengeType, OmniUIButtonType.Purchasing);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x17000E4D RID: 3661
	// (get) Token: 0x0600227D RID: 8829 RVA: 0x0006FDF2 File Offset: 0x0006DFF2
	public override bool IsEntryActive
	{
		get
		{
			return this.m_isEntryActive;
		}
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x0006FDFC File Offset: 0x0006DFFC
	public void Initialize(ChallengeType challengeType, ChallengeOmniUIWindowController windowController)
	{
		this.ChallengeType = challengeType;
		this.Initialize(windowController);
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((IChallengeOmniUIButton)buttonArray[i]).ChallengeType = this.ChallengeType;
		}
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x0006FE40 File Offset: 0x0006E040
	public override void UpdateActive()
	{
		ChallengeData challengeData = ChallengeLibrary.GetChallengeData(this.ChallengeType);
		if (challengeData == null || challengeData.Disabled)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x0006FEA0 File Offset: 0x0006E0A0
	public override void UpdateState()
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(this.ChallengeType);
		FoundState foundState = challenge.FoundState;
		if (foundState == FoundState.FoundButNotViewed)
		{
			if (!this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(true);
			}
		}
		else if (this.m_newSymbol.gameObject.activeSelf)
		{
			this.m_newSymbol.gameObject.SetActive(false);
		}
		if (foundState > FoundState.NotFound)
		{
			this.m_icon.sprite = IconLibrary.GetChallengeIcon(this.ChallengeType, ChallengeLibrary.ChallengeIconEntryType.Challenge);
		}
		else
		{
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
		if (foundState == FoundState.NotFound)
		{
			this.m_isEntryActive = false;
		}
		base.UpdateState();
		if (foundState == FoundState.NotFound)
		{
			this.m_isEntryActive = true;
		}
		if (foundState == FoundState.NotFound)
		{
			if (Challenge_EV.ScarBossRequirementTable.ContainsKey(this.ChallengeType) && SaveManager.PlayerSaveData.GetFlag(Challenge_EV.ScarBossRequirementTable[this.ChallengeType]))
			{
				this.m_titleText.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_BOSS_REQUIREMENT_HINT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				return;
			}
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_CHALLENGE_BOSS_REQUIREMENT_LOCKED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		else
		{
			this.m_titleText.text = LocalizationManager.GetString(challenge.ChallengeData.Title, false, false);
			ChallengeType challengeType = this.ChallengeType;
			if (challengeType == ChallengeType.Tutorial)
			{
				challengeType = ChallengeType.TutorialPurified;
			}
			switch (ChallengeManager.GetChallengeTrophyRank(challengeType, true))
			{
			case ChallengeTrophyRank.Bronze:
				this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Bronze);
				return;
			case ChallengeTrophyRank.Silver:
				this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Silver);
				return;
			case ChallengeTrophyRank.Gold:
				this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Gold);
				return;
			default:
				this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Default);
				return;
			}
		}
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x00070067 File Offset: 0x0006E267
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		if (ChallengeManager.GetFoundState(this.ChallengeType) == FoundState.FoundButNotViewed)
		{
			ChallengeManager.SetFoundState(this.ChallengeType, FoundState.FoundAndViewed, false, true);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		base.OnSelect(eventData);
	}

	// Token: 0x04001DC2 RID: 7618
	[SerializeField]
	private Image m_frame;

	// Token: 0x04001DC4 RID: 7620
	private bool m_isEntryActive = true;

	// Token: 0x04001DC5 RID: 7621
	private ChallengeOmniUIDescriptionEventArgs m_eventArgs;
}
