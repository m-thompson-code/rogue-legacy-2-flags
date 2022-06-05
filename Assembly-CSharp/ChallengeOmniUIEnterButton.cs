using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class ChallengeOmniUIEnterButton : OmniUIButton, IChallengeOmniUIButton
{
	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06002272 RID: 8818 RVA: 0x0006FCA6 File Offset: 0x0006DEA6
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E4A RID: 3658
	// (get) Token: 0x06002273 RID: 8819 RVA: 0x0006FCAE File Offset: 0x0006DEAE
	// (set) Token: 0x06002274 RID: 8820 RVA: 0x0006FCB6 File Offset: 0x0006DEB6
	public ChallengeType ChallengeType { get; set; }

	// Token: 0x06002275 RID: 8821 RVA: 0x0006FCBF File Offset: 0x0006DEBF
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new ChallengeOmniUIDescriptionEventArgs(this.ChallengeType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.ChallengeType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x0006FCEE File Offset: 0x0006DEEE
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		this.RunOnConfirmPressedAnimation();
		this.EnterChallenge();
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x0006FD0C File Offset: 0x0006DF0C
	public override void UpdateState()
	{
		this.m_deselectedSprite.SetAlpha(1f);
		if (ChallengeManager.GetFoundState(this.ChallengeType) > FoundState.NotFound)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.IsButtonActive = true;
			return;
		}
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		this.IsButtonActive = false;
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x0006FD79 File Offset: 0x0006DF79
	private void EnterChallenge()
	{
		WindowManager.SetWindowIsOpen(WindowID.ChallengeNPC, false);
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ChallengeNPC_EnterChallenge, this, this.ButtonEventArgs);
	}

	// Token: 0x04001DBE RID: 7614
	[SerializeField]
	private TMP_Text m_enterText;

	// Token: 0x04001DBF RID: 7615
	private ChallengeOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001DC0 RID: 7616
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
