using System;
using UnityEngine.EventSystems;

// Token: 0x02000640 RID: 1600
public class GeneticistOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x060030E2 RID: 12514 RVA: 0x0001AD2D File Offset: 0x00018F2D
	// (set) Token: 0x060030E3 RID: 12515 RVA: 0x0001AD35 File Offset: 0x00018F35
	public TraitType TraitType { get; protected set; }

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x060030E4 RID: 12516 RVA: 0x0001AD3E File Offset: 0x00018F3E
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

	// Token: 0x170012F3 RID: 4851
	// (get) Token: 0x060030E5 RID: 12517 RVA: 0x0001AD74 File Offset: 0x00018F74
	public override bool IsEntryActive
	{
		get
		{
			return TraitManager.GetTraitFoundState(this.TraitType) != FoundState.NotFound;
		}
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x000D1FA0 File Offset: 0x000D01A0
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

	// Token: 0x060030E7 RID: 12519 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void UpdateActive()
	{
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x000D2008 File Offset: 0x000D0208
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

	// Token: 0x060030E9 RID: 12521 RVA: 0x0001AD88 File Offset: 0x00018F88
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

	// Token: 0x0400280C RID: 10252
	private GeneticistOmniUIDescriptionEventArgs m_eventArgs;
}
