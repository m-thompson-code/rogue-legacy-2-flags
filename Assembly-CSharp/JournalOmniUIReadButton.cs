using System;
using Rewired;

// Token: 0x020003B1 RID: 945
public class JournalOmniUIReadButton : OmniUIButton, IJournalOmniUIButton
{
	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x060022F0 RID: 8944 RVA: 0x00071D05 File Offset: 0x0006FF05
	public override bool IsButtonActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x060022F1 RID: 8945 RVA: 0x00071D08 File Offset: 0x0006FF08
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x060022F2 RID: 8946 RVA: 0x00071D10 File Offset: 0x0006FF10
	// (set) Token: 0x060022F3 RID: 8947 RVA: 0x00071D18 File Offset: 0x0006FF18
	public int EntryIndex { get; set; }

	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x060022F4 RID: 8948 RVA: 0x00071D21 File Offset: 0x0006FF21
	// (set) Token: 0x060022F5 RID: 8949 RVA: 0x00071D29 File Offset: 0x0006FF29
	public int JournalIndex { get; set; }

	// Token: 0x17000E6E RID: 3694
	// (get) Token: 0x060022F6 RID: 8950 RVA: 0x00071D32 File Offset: 0x0006FF32
	// (set) Token: 0x060022F7 RID: 8951 RVA: 0x00071D3A File Offset: 0x0006FF3A
	public JournalType JournalType { get; set; }

	// Token: 0x17000E6F RID: 3695
	// (get) Token: 0x060022F8 RID: 8952 RVA: 0x00071D43 File Offset: 0x0006FF43
	// (set) Token: 0x060022F9 RID: 8953 RVA: 0x00071D4B File Offset: 0x0006FF4B
	public JournalOmniUIWindowController JournalWindowController { get; set; }

	// Token: 0x060022FA RID: 8954 RVA: 0x00071D54 File Offset: 0x0006FF54
	protected override void Awake()
	{
		this.m_onCancelButtonPressed = new Action<InputActionEventData>(this.OnCancelButtonPressed);
		base.Awake();
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x00071D70 File Offset: 0x0006FF70
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new JournalOmniUIDescriptionEventArgs(this.EntryIndex, this.JournalWindowController.HighlightedCategory, this.JournalIndex, this.JournalType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.EntryIndex, this.JournalWindowController.HighlightedCategory, this.JournalIndex, this.JournalType);
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x00071DD8 File Offset: 0x0006FFD8
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

	// Token: 0x060022FD RID: 8957 RVA: 0x00071E58 File Offset: 0x00070058
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

	// Token: 0x060022FE RID: 8958 RVA: 0x00071EEE File Offset: 0x000700EE
	public override void UpdateState()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04001DF1 RID: 7665
	private JournalOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001DF2 RID: 7666
	private Action<InputActionEventData> m_onCancelButtonPressed;
}
