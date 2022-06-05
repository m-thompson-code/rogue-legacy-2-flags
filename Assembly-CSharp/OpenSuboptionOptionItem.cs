using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class OpenSuboptionOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x060023C6 RID: 9158 RVA: 0x00013B34 File Offset: 0x00011D34
	// (set) Token: 0x060023C7 RID: 9159 RVA: 0x00013B3C File Offset: 0x00011D3C
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

	// Token: 0x060023C8 RID: 9160 RVA: 0x00013B45 File Offset: 0x00011D45
	public override void ActivateOption()
	{
		base.ActivateOption();
		WindowManager.SetWindowIsOpen(WindowID.Suboptions, true);
		(WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController).ChangeSuboptionDisplayed(this.m_suboptionType);
	}

	// Token: 0x04001FC4 RID: 8132
	[SerializeField]
	private SuboptionType m_suboptionType;
}
