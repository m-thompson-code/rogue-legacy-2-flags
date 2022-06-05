using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200062F RID: 1583
public class ChallengeOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06003092 RID: 12434 RVA: 0x0001AA1A File Offset: 0x00018C1A
	// (set) Token: 0x06003093 RID: 12435 RVA: 0x0001AA22 File Offset: 0x00018C22
	public ChallengeType ChallengeType { get; protected set; }

	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06003094 RID: 12436 RVA: 0x0001AA2B File Offset: 0x00018C2B
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

	// Token: 0x170012E0 RID: 4832
	// (get) Token: 0x06003095 RID: 12437 RVA: 0x0001AA61 File Offset: 0x00018C61
	public override bool IsEntryActive
	{
		get
		{
			return this.m_isEntryActive;
		}
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x000D0A58 File Offset: 0x000CEC58
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

	// Token: 0x06003097 RID: 12439 RVA: 0x000D0A9C File Offset: 0x000CEC9C
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

	// Token: 0x06003098 RID: 12440 RVA: 0x000D0AFC File Offset: 0x000CECFC
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

	// Token: 0x06003099 RID: 12441 RVA: 0x0001AA69 File Offset: 0x00018C69
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

	// Token: 0x040027DB RID: 10203
	[SerializeField]
	private Image m_frame;

	// Token: 0x040027DD RID: 10205
	private bool m_isEntryActive = true;

	// Token: 0x040027DE RID: 10206
	private ChallengeOmniUIDescriptionEventArgs m_eventArgs;
}
