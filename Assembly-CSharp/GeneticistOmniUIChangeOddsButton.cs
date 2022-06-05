using System;
using TMPro;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class GeneticistOmniUIChangeOddsButton : OmniUIButton, IGeneticistOmniUIButton
{
	// Token: 0x17000E5C RID: 3676
	// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000713D9 File Offset: 0x0006F5D9
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E5D RID: 3677
	// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000713E1 File Offset: 0x0006F5E1
	// (set) Token: 0x060022C2 RID: 8898 RVA: 0x000713E9 File Offset: 0x0006F5E9
	public TraitType TraitType { get; set; }

	// Token: 0x060022C3 RID: 8899 RVA: 0x000713F2 File Offset: 0x0006F5F2
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new GeneticistOmniUIDescriptionEventArgs(this.TraitType, this.m_isRightArrow);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.TraitType, this.m_isRightArrow);
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x0007142C File Offset: 0x0006F62C
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

	// Token: 0x060022C5 RID: 8901 RVA: 0x00071520 File Offset: 0x0006F720
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

	// Token: 0x04001DE2 RID: 7650
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04001DE3 RID: 7651
	[SerializeField]
	private TMP_Text m_oddsText;

	// Token: 0x04001DE4 RID: 7652
	private GeneticistOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
