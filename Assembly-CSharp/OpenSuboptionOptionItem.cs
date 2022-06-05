using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class OpenSuboptionOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000511B1 File Offset: 0x0004F3B1
	// (set) Token: 0x060019D8 RID: 6616 RVA: 0x000511B9 File Offset: 0x0004F3B9
	public SuboptionType SuboptionType
	{
		get
		{
			return this.m_suboptionType;
		}
		set
		{
			this.m_suboptionType = value;
		}
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000511C2 File Offset: 0x0004F3C2
	public override void ActivateOption()
	{
		base.ActivateOption();
		WindowManager.SetWindowIsOpen(WindowID.Suboptions, true);
		(WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController).ChangeSuboptionDisplayed(this.m_suboptionType);
	}

	// Token: 0x04001873 RID: 6259
	[SerializeField]
	private SuboptionType m_suboptionType;
}
