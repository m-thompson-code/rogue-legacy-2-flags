using System;
using RL_Windows;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020003D3 RID: 979
public class PauseTabButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler
{
	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x06002409 RID: 9225 RVA: 0x00076024 File Offset: 0x00074224
	// (set) Token: 0x0600240A RID: 9226 RVA: 0x0007602C File Offset: 0x0007422C
	public PauseTabSelectedHandler OnSelectEvent { get; set; }

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x0600240B RID: 9227 RVA: 0x00076035 File Offset: 0x00074235
	// (set) Token: 0x0600240C RID: 9228 RVA: 0x0007603D File Offset: 0x0007423D
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

	// Token: 0x0600240D RID: 9229 RVA: 0x00076048 File Offset: 0x00074248
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

	// Token: 0x0600240E RID: 9230 RVA: 0x000760B4 File Offset: 0x000742B4
	public void OnPointerClick(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x04001E8F RID: 7823
	[SerializeField]
	private WindowID m_windowToDisplay;
}
