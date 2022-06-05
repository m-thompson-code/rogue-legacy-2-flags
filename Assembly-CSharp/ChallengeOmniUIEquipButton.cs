using System;
using TMPro;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class ChallengeOmniUIEquipButton : OmniUIButton, IChallengeOmniUIButton
{
	// Token: 0x17000E4F RID: 3663
	// (get) Token: 0x06002285 RID: 8837 RVA: 0x000700AF File Offset: 0x0006E2AF
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E50 RID: 3664
	// (get) Token: 0x06002286 RID: 8838 RVA: 0x000700B7 File Offset: 0x0006E2B7
	// (set) Token: 0x06002287 RID: 8839 RVA: 0x000700BF File Offset: 0x0006E2BF
	public ChallengeType ChallengeType { get; set; }

	// Token: 0x06002288 RID: 8840 RVA: 0x000700C8 File Offset: 0x0006E2C8
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

	// Token: 0x06002289 RID: 8841 RVA: 0x00070110 File Offset: 0x0006E310
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

	// Token: 0x0600228A RID: 8842 RVA: 0x000701A8 File Offset: 0x0006E3A8
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

	// Token: 0x04001DC6 RID: 7622
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x04001DC8 RID: 7624
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001DC9 RID: 7625
	private ChallengeOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
