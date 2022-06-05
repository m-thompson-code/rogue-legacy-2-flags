using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class ChallengeOmniUIEnterButton : OmniUIButton, IChallengeOmniUIButton
{
	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x0600308A RID: 12426 RVA: 0x0001A983 File Offset: 0x00018B83
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x0600308B RID: 12427 RVA: 0x0001A98B File Offset: 0x00018B8B
	// (set) Token: 0x0600308C RID: 12428 RVA: 0x0001A993 File Offset: 0x00018B93
	public ChallengeType ChallengeType { get; set; }

	// Token: 0x0600308D RID: 12429 RVA: 0x0001A99C File Offset: 0x00018B9C
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new ChallengeOmniUIDescriptionEventArgs(this.ChallengeType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.ChallengeType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x0001A9CB File Offset: 0x00018BCB
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

	// Token: 0x0600308F RID: 12431 RVA: 0x000D09E8 File Offset: 0x000CEBE8
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

	// Token: 0x06003090 RID: 12432 RVA: 0x0001A9E8 File Offset: 0x00018BE8
	private void EnterChallenge()
	{
		WindowManager.SetWindowIsOpen(WindowID.ChallengeNPC, false);
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ChallengeNPC_EnterChallenge, this, this.ButtonEventArgs);
	}

	// Token: 0x040027D7 RID: 10199
	[SerializeField]
	private TMP_Text m_enterText;

	// Token: 0x040027D8 RID: 10200
	private ChallengeOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x040027D9 RID: 10201
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
