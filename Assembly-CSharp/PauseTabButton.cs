using System;
using RL_Windows;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200066D RID: 1645
public class PauseTabButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler
{
	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x0600322D RID: 12845 RVA: 0x0001B88F File Offset: 0x00019A8F
	// (set) Token: 0x0600322E RID: 12846 RVA: 0x0001B897 File Offset: 0x00019A97
	public PauseTabSelectedHandler OnSelectEvent { get; set; }

	// Token: 0x17001360 RID: 4960
	// (get) Token: 0x0600322F RID: 12847 RVA: 0x0001B8A0 File Offset: 0x00019AA0
	// (set) Token: 0x06003230 RID: 12848 RVA: 0x0001B8A8 File Offset: 0x00019AA8
	public WindowID WindowToDisplay
	{
		get
		{
			return this.m_windowToDisplay;
		}
		set
		{
			this.m_windowToDisplay = value;
		}
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x000D5F8C File Offset: 0x000D418C
	public void OnSelect(BaseEventData eventData)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.ConfirmMenu) || WindowManager.GetIsWindowOpen(WindowID.ConfirmMenuBig))
		{
			return;
		}
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			PauseWindowController pauseWindowController = WindowManager.GetWindowController(WindowID.Pause) as PauseWindowController;
			if (pauseWindowController)
			{
				if (!pauseWindowController.AreControlsEnabled)
				{
					return;
				}
				if (this.m_windowToDisplay == WindowID.Quest && !pauseWindowController.QuestInsightFound)
				{
					return;
				}
			}
			if (this.OnSelectEvent != null)
			{
				this.OnSelectEvent(this);
			}
		}
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x0001B8B1 File Offset: 0x00019AB1
	public void OnPointerClick(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x040028D1 RID: 10449
	[SerializeField]
	private WindowID m_windowToDisplay;
}
