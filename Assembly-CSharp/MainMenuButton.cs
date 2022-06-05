using System;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000382 RID: 898
public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x06002191 RID: 8593 RVA: 0x0006A2EE File Offset: 0x000684EE
	// (set) Token: 0x06002192 RID: 8594 RVA: 0x0006A2F6 File Offset: 0x000684F6
	public MainMenuButtonSelectedHandler MenuButtonSelected { get; set; }

	// Token: 0x17000E1F RID: 3615
	// (get) Token: 0x06002193 RID: 8595 RVA: 0x0006A2FF File Offset: 0x000684FF
	// (set) Token: 0x06002194 RID: 8596 RVA: 0x0006A307 File Offset: 0x00068507
	public MainMenuButtonSelectedHandler MenuButtonActivated { get; set; }

	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x06002195 RID: 8597 RVA: 0x0006A310 File Offset: 0x00068510
	public MainMenuButton.MainMenuSelectionType SelectorType
	{
		get
		{
			return this.m_selectorType;
		}
	}

	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x06002196 RID: 8598 RVA: 0x0006A318 File Offset: 0x00068518
	// (set) Token: 0x06002197 RID: 8599 RVA: 0x0006A320 File Offset: 0x00068520
	public bool Interactable { get; set; }

	// Token: 0x17000E22 RID: 3618
	// (get) Token: 0x06002198 RID: 8600 RVA: 0x0006A329 File Offset: 0x00068529
	// (set) Token: 0x06002199 RID: 8601 RVA: 0x0006A331 File Offset: 0x00068531
	public int Index { get; private set; }

	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x0600219A RID: 8602 RVA: 0x0006A33A File Offset: 0x0006853A
	// (set) Token: 0x0600219B RID: 8603 RVA: 0x0006A342 File Offset: 0x00068542
	public TMP_Text Text { get; private set; }

	// Token: 0x0600219C RID: 8604 RVA: 0x0006A34B File Offset: 0x0006854B
	private void Awake()
	{
		this.Text = base.GetComponentInChildren<TMP_Text>();
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x0006A359 File Offset: 0x00068559
	public void Initialize(MainMenuWindowController mainMenuWindow, int index)
	{
		this.m_selectedIndicator.SetActive(false);
		this.Index = index;
		this.m_mainMenuWindow = mainMenuWindow;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x0006A375 File Offset: 0x00068575
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x0006A386 File Offset: 0x00068586
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x0006A38F File Offset: 0x0006858F
	public virtual void OnSelect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(true);
		if (this.MenuButtonSelected != null)
		{
			this.MenuButtonSelected(this);
		}
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x0006A3B1 File Offset: 0x000685B1
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(false);
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x0006A3C0 File Offset: 0x000685C0
	public void ExecuteButton()
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.MenuButtonActivated != null)
		{
			this.MenuButtonActivated(this);
		}
		switch (this.m_selectorType)
		{
		case MainMenuButton.MainMenuSelectionType.Start:
			this.m_mainMenuWindow.LoadGame();
			return;
		case MainMenuButton.MainMenuSelectionType.SelectProfile:
			WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
			WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, true);
			return;
		case MainMenuButton.MainMenuSelectionType.Options:
			WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
			WindowManager.SetWindowIsOpen(WindowID.Options, true);
			return;
		case MainMenuButton.MainMenuSelectionType.Credits:
			CreditsController.IsEndingScroll = false;
			SceneLoader_RL.LoadScene(SceneID.Credits, TransitionID.FadeToBlackNoLoading);
			return;
		case MainMenuButton.MainMenuSelectionType.Quit:
			Application.Quit(10);
			return;
		case MainMenuButton.MainMenuSelectionType.ViewNews:
			this.m_mainMenuWindow.SetEnablePatchNotes(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x04001D0F RID: 7439
	[SerializeField]
	private MainMenuButton.MainMenuSelectionType m_selectorType;

	// Token: 0x04001D10 RID: 7440
	[SerializeField]
	protected GameObject m_selectedIndicator;

	// Token: 0x04001D11 RID: 7441
	private MainMenuWindowController m_mainMenuWindow;

	// Token: 0x02000C01 RID: 3073
	public enum MainMenuSelectionType
	{
		// Token: 0x04004E76 RID: 20086
		None,
		// Token: 0x04004E77 RID: 20087
		Start,
		// Token: 0x04004E78 RID: 20088
		SelectProfile,
		// Token: 0x04004E79 RID: 20089
		Options,
		// Token: 0x04004E7A RID: 20090
		Credits,
		// Token: 0x04004E7B RID: 20091
		Quit,
		// Token: 0x04004E7C RID: 20092
		ViewNews
	}
}
