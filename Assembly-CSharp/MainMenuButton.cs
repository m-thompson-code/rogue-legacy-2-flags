using System;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200060A RID: 1546
public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x170012A5 RID: 4773
	// (get) Token: 0x06002F85 RID: 12165 RVA: 0x0001A016 File Offset: 0x00018216
	// (set) Token: 0x06002F86 RID: 12166 RVA: 0x0001A01E File Offset: 0x0001821E
	public MainMenuButtonSelectedHandler MenuButtonSelected { get; set; }

	// Token: 0x170012A6 RID: 4774
	// (get) Token: 0x06002F87 RID: 12167 RVA: 0x0001A027 File Offset: 0x00018227
	// (set) Token: 0x06002F88 RID: 12168 RVA: 0x0001A02F File Offset: 0x0001822F
	public MainMenuButtonSelectedHandler MenuButtonActivated { get; set; }

	// Token: 0x170012A7 RID: 4775
	// (get) Token: 0x06002F89 RID: 12169 RVA: 0x0001A038 File Offset: 0x00018238
	public MainMenuButton.MainMenuSelectionType SelectorType
	{
		get
		{
			return this.m_selectorType;
		}
	}

	// Token: 0x170012A8 RID: 4776
	// (get) Token: 0x06002F8A RID: 12170 RVA: 0x0001A040 File Offset: 0x00018240
	// (set) Token: 0x06002F8B RID: 12171 RVA: 0x0001A048 File Offset: 0x00018248
	public bool Interactable { get; set; }

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06002F8C RID: 12172 RVA: 0x0001A051 File Offset: 0x00018251
	// (set) Token: 0x06002F8D RID: 12173 RVA: 0x0001A059 File Offset: 0x00018259
	public int Index { get; private set; }

	// Token: 0x170012AA RID: 4778
	// (get) Token: 0x06002F8E RID: 12174 RVA: 0x0001A062 File Offset: 0x00018262
	// (set) Token: 0x06002F8F RID: 12175 RVA: 0x0001A06A File Offset: 0x0001826A
	public TMP_Text Text { get; private set; }

	// Token: 0x06002F90 RID: 12176 RVA: 0x0001A073 File Offset: 0x00018273
	private void Awake()
	{
		this.Text = base.GetComponentInChildren<TMP_Text>();
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x0001A081 File Offset: 0x00018281
	public void Initialize(MainMenuWindowController mainMenuWindow, int index)
	{
		this.m_selectedIndicator.SetActive(false);
		this.Index = index;
		this.m_mainMenuWindow = mainMenuWindow;
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x0001A09D File Offset: 0x0001829D
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x0001A0AE File Offset: 0x000182AE
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x0001A0B7 File Offset: 0x000182B7
	public virtual void OnSelect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(true);
		if (this.MenuButtonSelected != null)
		{
			this.MenuButtonSelected(this);
		}
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x0001A0D9 File Offset: 0x000182D9
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(false);
	}

	// Token: 0x06002F96 RID: 12182 RVA: 0x000CB128 File Offset: 0x000C9328
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

	// Token: 0x040026F0 RID: 9968
	[SerializeField]
	private MainMenuButton.MainMenuSelectionType m_selectorType;

	// Token: 0x040026F1 RID: 9969
	[SerializeField]
	protected GameObject m_selectedIndicator;

	// Token: 0x040026F2 RID: 9970
	private MainMenuWindowController m_mainMenuWindow;

	// Token: 0x0200060B RID: 1547
	public enum MainMenuSelectionType
	{
		// Token: 0x040026F9 RID: 9977
		None,
		// Token: 0x040026FA RID: 9978
		Start,
		// Token: 0x040026FB RID: 9979
		SelectProfile,
		// Token: 0x040026FC RID: 9980
		Options,
		// Token: 0x040026FD RID: 9981
		Credits,
		// Token: 0x040026FE RID: 9982
		Quit,
		// Token: 0x040026FF RID: 9983
		ViewNews
	}
}
