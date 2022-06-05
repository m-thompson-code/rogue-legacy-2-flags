using System;
using Rewired;

// Token: 0x02000646 RID: 1606
public class JournalOmniUIReadButton : OmniUIButton, IJournalOmniUIButton
{
	// Token: 0x170012FD RID: 4861
	// (get) Token: 0x06003108 RID: 12552 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool IsButtonActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012FE RID: 4862
	// (get) Token: 0x06003109 RID: 12553 RVA: 0x0001AEC0 File Offset: 0x000190C0
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x0600310A RID: 12554 RVA: 0x0001AEC8 File Offset: 0x000190C8
	// (set) Token: 0x0600310B RID: 12555 RVA: 0x0001AED0 File Offset: 0x000190D0
	public int EntryIndex { get; set; }

	// Token: 0x17001300 RID: 4864
	// (get) Token: 0x0600310C RID: 12556 RVA: 0x0001AED9 File Offset: 0x000190D9
	// (set) Token: 0x0600310D RID: 12557 RVA: 0x0001AEE1 File Offset: 0x000190E1
	public int JournalIndex { get; set; }

	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x0600310E RID: 12558 RVA: 0x0001AEEA File Offset: 0x000190EA
	// (set) Token: 0x0600310F RID: 12559 RVA: 0x0001AEF2 File Offset: 0x000190F2
	public JournalType JournalType { get; set; }

	// Token: 0x17001302 RID: 4866
	// (get) Token: 0x06003110 RID: 12560 RVA: 0x0001AEFB File Offset: 0x000190FB
	// (set) Token: 0x06003111 RID: 12561 RVA: 0x0001AF03 File Offset: 0x00019103
	public JournalOmniUIWindowController JournalWindowController { get; set; }

	// Token: 0x06003112 RID: 12562 RVA: 0x0001AF0C File Offset: 0x0001910C
	protected override void Awake()
	{
		this.m_onCancelButtonPressed = new Action<InputActionEventData>(this.OnCancelButtonPressed);
		base.Awake();
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000D24C4 File Offset: 0x000D06C4
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new JournalOmniUIDescriptionEventArgs(this.EntryIndex, this.JournalWindowController.HighlightedCategory, this.JournalIndex, this.JournalType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.EntryIndex, this.JournalWindowController.HighlightedCategory, this.JournalIndex, this.JournalType);
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x000D252C File Offset: 0x000D072C
	public override void OnConfirmButtonPressed()
	{
		base.OnConfirmButtonPressed();
		this.JournalWindowController.CommonFields.DescriptionBoxRaycastBlocker.gameObject.SetActive(true);
		this.JournalWindowController.DescriptionBoxScrollInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
		this.JournalWindowController.SetKeyboardEnabled(false);
		this.JournalWindowController.ScrollArrow.SetActive(true);
		this.JournalWindowController.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000D25AC File Offset: 0x000D07AC
	private void OnCancelButtonPressed(InputActionEventData eventData)
	{
		this.JournalWindowController.CommonFields.DescriptionBoxRaycastBlocker.gameObject.SetActive(false);
		this.JournalWindowController.DescriptionBoxScrollInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
		this.JournalWindowController.SetKeyboardEnabled(true);
		this.JournalWindowController.ScrollArrow.SetActive(false);
		this.JournalWindowController.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		if (this.JournalWindowController.SelectOptionEvent != null)
		{
			this.JournalWindowController.SelectOptionEvent.Invoke();
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x0001AF26 File Offset: 0x00019126
	public override void UpdateState()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0400281A RID: 10266
	private JournalOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x0400281B RID: 10267
	private Action<InputActionEventData> m_onCancelButtonPressed;
}
