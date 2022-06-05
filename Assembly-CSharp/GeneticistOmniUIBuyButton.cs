using System;
using TMPro;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class GeneticistOmniUIBuyButton : OmniUIButton, IGeneticistOmniUIButton
{
	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x060022B7 RID: 8887 RVA: 0x00071238 File Offset: 0x0006F438
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E5A RID: 3674
	// (get) Token: 0x060022B8 RID: 8888 RVA: 0x00071240 File Offset: 0x0006F440
	// (set) Token: 0x060022B9 RID: 8889 RVA: 0x00071248 File Offset: 0x0006F448
	public TraitType TraitType { get; set; }

	// Token: 0x060022BA RID: 8890 RVA: 0x00071251 File Offset: 0x0006F451
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new GeneticistOmniUIDescriptionEventArgs(this.TraitType, true);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.TraitType, true);
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x00071280 File Offset: 0x0006F480
	public override void OnConfirmButtonPressed()
	{
		base.OnConfirmButtonPressed();
		if (!this.m_buttonIsUpgrade)
		{
			if (TraitManager.CanPurchaseTrait(this.TraitType))
			{
				TraitManager.SetTraitFoundState(this.TraitType, FoundState.Purchased);
				TraitManager.SetTraitSpawnOdds(this.TraitType, 1, false);
				this.InitializeButtonEventArgs();
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			}
		}
		else if (TraitManager.CanUpgradeTrait(this.TraitType))
		{
			TraitManager.SetTraitUpgradeLevel(this.TraitType, 1, true, false);
			TraitManager.SetTraitSpawnOdds(this.TraitType, 1, true);
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x0007132C File Offset: 0x0006F52C
	public override void UpdateState()
	{
		if (TraitManager.GetTraitFoundState(this.TraitType) != FoundState.Purchased)
		{
			this.m_buyText.text = "Buy";
			this.m_buttonIsUpgrade = false;
		}
		else
		{
			this.m_buyText.text = "Upgrade";
			this.m_buttonIsUpgrade = true;
		}
		if (TraitManager.GetTraitUpgradeLevel(this.TraitType) < TraitManager.GetTraitBlueprintLevel(this.TraitType))
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.m_deselectedSprite.SetAlpha(1f);
			return;
		}
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04001DDE RID: 7646
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x04001DDF RID: 7647
	private bool m_buttonIsUpgrade;

	// Token: 0x04001DE0 RID: 7648
	private GeneticistOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
