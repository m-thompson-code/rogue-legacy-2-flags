using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class GeneticistOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<GeneticistOmniUIDescriptionEventArgs, GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType>
{
	// Token: 0x060030DF RID: 12511 RVA: 0x0001AD03 File Offset: 0x00018F03
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == GeneticistOmniUIDescriptionBoxEntry.GeneticistOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
		}
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000D1F44 File Offset: 0x000D0144
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

	// Token: 0x0200063F RID: 1599
	public enum GeneticistOmniUIDescriptionBoxType
	{
		// Token: 0x04002807 RID: 10247
		None,
		// Token: 0x04002808 RID: 10248
		Title,
		// Token: 0x04002809 RID: 10249
		Description,
		// Token: 0x0400280A RID: 10250
		Stat
	}
}
