using System;
using TMPro;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class ChallengeOmniUIEquipButton : OmniUIButton, IChallengeOmniUIButton
{
	// Token: 0x170012E2 RID: 4834
	// (get) Token: 0x0600309D RID: 12445 RVA: 0x0001AAB1 File Offset: 0x00018CB1
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012E3 RID: 4835
	// (get) Token: 0x0600309E RID: 12446 RVA: 0x0001AAB9 File Offset: 0x00018CB9
	// (set) Token: 0x0600309F RID: 12447 RVA: 0x0001AAC1 File Offset: 0x00018CC1
	public ChallengeType ChallengeType { get; set; }

	// Token: 0x060030A0 RID: 12448 RVA: 0x000D0CC4 File Offset: 0x000CEEC4
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = this.m_isRightArrow ? OmniUIButtonType.Equipping : OmniUIButtonType.Unequipping;
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new ChallengeOmniUIDescriptionEventArgs(this.ChallengeType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.ChallengeType, buttonType);
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000D0D0C File Offset: 0x000CEF0C
	public override void UpdateState()
	{
		ChallengeManager.GetChallenge(this.ChallengeType);
		int challengeEquippedLevel = ChallengeManager.GetChallengeEquippedLevel(this.ChallengeType);
		int upgradeBlueprintsFound = ChallengeManager.GetUpgradeBlueprintsFound(this.ChallengeType, false);
		if (upgradeBlueprintsFound > 0)
		{
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
			this.m_levelText.text = challengeEquippedLevel.ToString() + "/" + upgradeBlueprintsFound.ToString();
			return;
		}
		this.m_deselectedSprite.SetAlpha(0f);
		this.IsButtonActive = false;
		this.m_levelText.text = "";
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000D0DA4 File Offset: 0x000CEFA4
	public override void OnConfirmButtonPressed()
	{
		if (this.m_isRightArrow)
		{
			if (ChallengeManager.CanEquip(this.ChallengeType, true))
			{
				if (ChallengeManager.SetChallengeEquippedLevel(this.ChallengeType, 1, true, true))
				{
					this.InitializeButtonEventArgs();
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
				}
			}
			else
			{
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
		else if (ChallengeManager.SetChallengeEquippedLevel(this.ChallengeType, -1, true, true))
		{
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		this.RunOnConfirmPressedAnimation();
		base.OnConfirmButtonPressed();
	}

	// Token: 0x040027DF RID: 10207
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x040027E0 RID: 10208
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x040027E1 RID: 10209
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040027E2 RID: 10210
	private ChallengeOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
