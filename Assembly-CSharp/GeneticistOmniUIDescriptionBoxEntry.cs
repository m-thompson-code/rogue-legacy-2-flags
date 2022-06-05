using System;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class GeneticistOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<GeneticistOmniUIDescriptionEventArgs, GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType>
{
	// Token: 0x060022C7 RID: 8903 RVA: 0x000715A5 File Offset: 0x0006F7A5
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
		}
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000715C8 File Offset: 0x0006F7C8
	protected override void DisplayDescriptionBox(GeneticistOmniUIDescriptionEventArgs args)
	{
		TraitData traitData = TraitLibrary.GetTraitData(args.TraitType);
		GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType descriptionType = this.m_descriptionType;
		if (descriptionType == GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), false, false);
			return;
		}
		if (descriptionType != GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType.Description)
		{
			return;
		}
		this.m_titleText.text = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), false, false);
	}

	// Token: 0x02000C0C RID: 3084
	public enum GeneticistOmniUIDescriptionBoxType
	{
		// Token: 0x04004EB5 RID: 20149
		None,
		// Token: 0x04004EB6 RID: 20150
		Title,
		// Token: 0x04004EB7 RID: 20151
		Description,
		// Token: 0x04004EB8 RID: 20152
		Stat
	}
}
