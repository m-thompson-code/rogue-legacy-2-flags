using System;
using TMPro;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class GeneticistOmniUIChangeOddsButton : OmniUIButton, IGeneticistOmniUIButton
{
	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x060030D8 RID: 12504 RVA: 0x0001ACB1 File Offset: 0x00018EB1
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012F0 RID: 4848
	// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0001ACB9 File Offset: 0x00018EB9
	// (set) Token: 0x060030DA RID: 12506 RVA: 0x0001ACC1 File Offset: 0x00018EC1
	public TraitType TraitType { get; set; }

	// Token: 0x060030DB RID: 12507 RVA: 0x0001ACCA File Offset: 0x00018ECA
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new GeneticistOmniUIDescriptionEventArgs(this.TraitType, this.m_isRightArrow);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.TraitType, this.m_isRightArrow);
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000D1DD0 File Offset: 0x000CFFD0
	public override void UpdateState()
	{
		TraitSpawnOdds traitSpawnOdds = TraitManager.GetTraitSpawnOdds(this.TraitType);
		int traitUpgradeLevel = TraitManager.GetTraitUpgradeLevel(this.TraitType);
		if (this.m_isRightArrow)
		{
			if (traitSpawnOdds < (TraitSpawnOdds)traitUpgradeLevel)
			{
				this.m_deselectedSprite.SetAlpha(1f);
				this.IsButtonActive = true;
			}
			else
			{
				this.m_deselectedSprite.SetAlpha(0.25f);
				this.IsButtonActive = false;
			}
		}
		else if (traitSpawnOdds > TraitSpawnOdds.Common)
		{
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
		}
		else
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
		}
		switch (traitSpawnOdds)
		{
		case TraitSpawnOdds.Common:
			this.m_oddsText.text = "[Common]";
			return;
		case TraitSpawnOdds.Rare:
			this.m_oddsText.text = "[Rare]";
			return;
		case TraitSpawnOdds.VeryRare:
			this.m_oddsText.text = "[Very Rare]";
			return;
		case TraitSpawnOdds.Never:
			this.m_oddsText.text = "[Never]";
			return;
		default:
			return;
		}
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x000D1EC4 File Offset: 0x000D00C4
	public override void OnConfirmButtonPressed()
	{
		if (this.m_isRightArrow)
		{
			TraitManager.SetTraitSpawnOdds(this.TraitType, 1, true);
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			this.RunOnConfirmPressedAnimation();
		}
		else
		{
			TraitManager.SetTraitSpawnOdds(this.TraitType, -1, true);
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			this.RunOnConfirmPressedAnimation();
		}
		base.OnConfirmButtonPressed();
	}

	// Token: 0x04002802 RID: 10242
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04002803 RID: 10243
	[SerializeField]
	private TMP_Text m_oddsText;

	// Token: 0x04002804 RID: 10244
	private GeneticistOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
