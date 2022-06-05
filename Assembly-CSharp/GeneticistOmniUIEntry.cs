using System;
using UnityEngine.EventSystems;

// Token: 0x020003AC RID: 940
public class GeneticistOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x060022CA RID: 8906 RVA: 0x0007162A File Offset: 0x0006F82A
	// (set) Token: 0x060022CB RID: 8907 RVA: 0x00071632 File Offset: 0x0006F832
	public TraitType TraitType { get; protected set; }

	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x060022CC RID: 8908 RVA: 0x0007163B File Offset: 0x0006F83B
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new GeneticistOmniUIDescriptionEventArgs(this.TraitType, true);
			}
			else
			{
				this.m_eventArgs.Initialize(this.TraitType, true);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x060022CD RID: 8909 RVA: 0x00071671 File Offset: 0x0006F871
	public override bool IsEntryActive
	{
		get
		{
			return TraitManager.GetTraitFoundState(this.TraitType) != FoundState.NotFound;
		}
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x00071688 File Offset: 0x0006F888
	public void Initialize(TraitType traitType, GeneticistOmniUIWindowController windowController)
	{
		this.TraitType = traitType;
		this.Initialize(windowController);
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((IGeneticistOmniUIButton)buttonArray[i]).TraitType = this.TraitType;
		}
		string @string = LocalizationManager.GetString(TraitLibrary.GetTraitData(this.TraitType).GetTraitTitleLocID(), false, false);
		this.m_titleText.text = @string;
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000716EF File Offset: 0x0006F8EF
	public override void UpdateActive()
	{
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000716F4 File Offset: 0x0006F8F4
	public override void UpdateState()
	{
		if (TraitManager.GetTraitFoundState(this.TraitType) == FoundState.FoundButNotViewed)
		{
			if (!this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(true);
			}
		}
		else if (this.m_newSymbol.gameObject.activeSelf)
		{
			this.m_newSymbol.gameObject.SetActive(false);
		}
		base.UpdateState();
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x0007175E File Offset: 0x0006F95E
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		if (TraitManager.GetTraitFoundState(this.TraitType) == FoundState.FoundButNotViewed)
		{
			TraitManager.SetTraitFoundState(this.TraitType, FoundState.FoundAndViewed);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		base.OnSelect(eventData);
	}

	// Token: 0x04001DE7 RID: 7655
	private GeneticistOmniUIDescriptionEventArgs m_eventArgs;
}
