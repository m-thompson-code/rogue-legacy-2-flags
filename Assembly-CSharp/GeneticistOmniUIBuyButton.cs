using System;
using TMPro;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class GeneticistOmniUIBuyButton : OmniUIButton, IGeneticistOmniUIButton
{
	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x060030CF RID: 12495 RVA: 0x0001AC69 File Offset: 0x00018E69
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012ED RID: 4845
	// (get) Token: 0x060030D0 RID: 12496 RVA: 0x0001AC71 File Offset: 0x00018E71
	// (set) Token: 0x060030D1 RID: 12497 RVA: 0x0001AC79 File Offset: 0x00018E79
	public TraitType TraitType { get; set; }

	// Token: 0x060030D2 RID: 12498 RVA: 0x0001AC82 File Offset: 0x00018E82
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new GeneticistOmniUIDescriptionEventArgs(this.TraitType, true);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.TraitType, true);
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x000D1C7C File Offset: 0x000CFE7C
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

	// Token: 0x060030D4 RID: 12500 RVA: 0x000D1D28 File Offset: 0x000CFF28
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

	// Token: 0x040027FE RID: 10238
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x040027FF RID: 10239
	private bool m_buttonIsUpgrade;

	// Token: 0x04002800 RID: 10240
	private GeneticistOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
