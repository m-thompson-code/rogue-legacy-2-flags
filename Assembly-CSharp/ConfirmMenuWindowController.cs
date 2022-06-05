using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000949 RID: 2377
public class ConfirmMenuWindowController : WindowController, ILocalizable
{
	// Token: 0x1700194B RID: 6475
	// (get) Token: 0x06004834 RID: 18484 RVA: 0x00027A75 File Offset: 0x00025C75
	protected bool InputDelayed
	{
		get
		{
			return Time.unscaledTime < this.m_buttonDelayTime;
		}
	}

	// Token: 0x1700194C RID: 6476
	// (get) Token: 0x06004835 RID: 18485 RVA: 0x00027A84 File Offset: 0x00025C84
	public override WindowID ID
	{
		get
		{
			if (!this.m_isBigWindow)
			{
				return WindowID.ConfirmMenu;
			}
			return WindowID.ConfirmMenuBig;
		}
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x00117D9C File Offset: 0x00115F9C
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
		this.m_onCancelPressed = new Action<InputActionEventData>(this.OnCancelPressed);
		this.m_onVerticalPressed = new Action<InputActionEventData>(this.OnVerticalPressed);
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x00117DF4 File Offset: 0x00115FF4
	public override void Initialize()
	{
		base.Initialize();
		this.m_storedBoxScale = this.m_confirmMenuBox.transform.localScale;
		if (!this.m_isBigWindow)
		{
			this.SetBGHeight(765f);
		}
		else
		{
			this.SetBGHeight(970f);
		}
		this.m_buttonArray = base.GetComponentsInChildren<ConfirmMenu_Button>(true);
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.m_buttonArray[i].Initialize(i, new Action<ConfirmMenu_Button>(this.OnPointerEnter), new Action(this.OnPointerExit), new Action<ConfirmMenu_Button>(this.PlayClickSFX));
		}
		this.m_activeButtonList = new List<ConfirmMenu_Button>();
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnPointerExit()
	{
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x00027A93 File Offset: 0x00025C93
	private void OnPointerEnter(ConfirmMenu_Button button)
	{
		this.SetSelectedButton(button.Index);
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x00027AA1 File Offset: 0x00025CA1
	private void PlayClickSFX(ConfirmMenu_Button button)
	{
		if (this.m_selectOptionEvent != null)
		{
			this.m_selectOptionEvent.Invoke();
		}
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x00117E9C File Offset: 0x0011609C
	private void SetSelectedButton(int index)
	{
		if (index == this.m_selectedButtonIndex)
		{
			return;
		}
		int selectedButtonIndex = this.m_selectedButtonIndex;
		this.m_selectedButtonIndex = index;
		if (selectedButtonIndex > -1 && selectedButtonIndex < this.m_activeButtonList.Count)
		{
			this.m_activeButtonList[selectedButtonIndex].OnDeselect(null);
		}
		if (index < 0)
		{
			using (List<ConfirmMenu_Button>.Enumerator enumerator = this.m_activeButtonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ConfirmMenu_Button confirmMenu_Button = enumerator.Current;
					confirmMenu_Button.OnDeselect(null);
				}
				goto IL_8B;
			}
		}
		this.m_activeButtonList[this.m_selectedButtonIndex].OnSelect(null);
		IL_8B:
		if (this.m_changeSelectedOptionEvent != null)
		{
			this.m_changeSelectedOptionEvent.Invoke();
		}
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x00027AB6 File Offset: 0x00025CB6
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x00027ABE File Offset: 0x00025CBE
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x00117F58 File Offset: 0x00116158
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_selectedButtonIndex = -1;
		this.SetSelectedButton(this.m_startingButtonIndex);
		base.StartCoroutine(this.RunOpenAnimCoroutine());
		if (this.m_windowOpenedEvent != null)
		{
			this.m_windowOpenedEvent.Invoke();
		}
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x00027AC6 File Offset: 0x00025CC6
	private IEnumerator RunOpenAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		foreach (ConfirmMenu_Button confirmMenu_Button in this.m_activeButtonList)
		{
			confirmMenu_Button.DisableClick = true;
		}
		if (this.m_buttonDelayTime > 0f)
		{
			base.StartCoroutine(this.ConfirmButtonDelayCoroutine());
		}
		else
		{
			this.m_buttonDelayCountdownText.gameObject.SetActive(false);
		}
		this.m_fadeBGCanvasGroup.alpha = 0f;
		TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		Vector3 vector = this.m_storedBoxScale;
		vector -= new Vector3(0.05f, 0.05f, 0f);
		this.m_confirmMenuBox.transform.localScale = vector;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_confirmMenuBox.transform, 0.15f, new EaseDelegate(Ease.Back.EaseOutLarge), new object[]
		{
			"localScale.x",
			this.m_storedBoxScale.x,
			"localScale.y",
			this.m_storedBoxScale.y
		}).TweenCoroutine;
		foreach (ConfirmMenu_Button confirmMenu_Button2 in this.m_activeButtonList)
		{
			confirmMenu_Button2.DisableClick = false;
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x00027AD5 File Offset: 0x00025CD5
	private IEnumerator ConfirmButtonDelayCoroutine()
	{
		foreach (ConfirmMenu_Button confirmMenu_Button in this.m_activeButtonList)
		{
			confirmMenu_Button.gameObject.SetActive(false);
		}
		this.m_buttonDelayCountdownText.gameObject.SetActive(true);
		this.m_buttonDelayTime += Time.unscaledTime;
		while (Time.unscaledTime < this.m_buttonDelayTime)
		{
			int num = (int)(this.m_buttonDelayTime - Time.unscaledTime) + 1;
			this.m_buttonDelayCountdownText.text = "- " + num.ToString() + " -";
			yield return null;
		}
		this.m_buttonDelayCountdownText.gameObject.SetActive(false);
		using (List<ConfirmMenu_Button>.Enumerator enumerator = this.m_activeButtonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ConfirmMenu_Button confirmMenu_Button2 = enumerator.Current;
				confirmMenu_Button2.gameObject.SetActive(true);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x00117FB8 File Offset: 0x001161B8
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		ConfirmMenu_Button[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			buttonArray[i].ResetValues();
		}
		this.m_onCancelAction = null;
		this.SetStartingSelectedButton(0);
		this.m_windowCanvas.gameObject.SetActive(false);
		if (this.m_windowClosedEvent != null)
		{
			this.m_windowClosedEvent.Invoke();
		}
		if (this.m_playerMATHackApplied)
		{
			this.m_backgroundImage.material = null;
			RectTransform component = this.m_backgroundImage.GetComponent<RectTransform>();
			Vector3 anchoredPosition3D = component.anchoredPosition3D;
			anchoredPosition3D.z = 0f;
			component.anchoredPosition3D = anchoredPosition3D;
			this.m_playerMATHackApplied = false;
			base.WindowCanvas.sortingLayerName = "Default";
		}
		this.m_buttonDelayTime = 0f;
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x00027AE4 File Offset: 0x00025CE4
	public void SetStartingSelectedButton(int startingButtonIndex)
	{
		this.m_startingButtonIndex = startingButtonIndex;
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x0011807C File Offset: 0x0011627C
	public void SetNumberOfButtons(int numButtons)
	{
		if (numButtons < 1 || numButtons >= this.m_buttonArray.Length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"<color=red>ConfirmMenuWindowController.SetNumberOfButtons(int numButtons) - Cannot set confirm menu buttons to: ",
				numButtons.ToString(),
				". Max number of buttons allowed is: ",
				(this.m_buttonArray.Length - 1).ToString(),
				". Must also be greater than 0.</color>"
			}));
			return;
		}
		this.m_activeButtonList.Clear();
		for (int i = 0; i < numButtons; i++)
		{
			this.m_buttonArray[i].gameObject.SetActive(true);
			this.m_activeButtonList.Add(this.m_buttonArray[i]);
		}
		float num = 100f * (float)(numButtons - 1);
		if (!this.m_isBigWindow)
		{
			this.SetBGHeight(765f + num);
			return;
		}
		this.SetBGHeight(970f + num);
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x00027AED File Offset: 0x00025CED
	public void SetButtonDelayTime(float delayTime)
	{
		this.m_buttonDelayTime = delayTime;
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x00027AF6 File Offset: 0x00025CF6
	public ConfirmMenu_Button GetButtonAtIndex(int index)
	{
		if (index < 0 || index >= this.m_activeButtonList.Count)
		{
			Debug.Log("<color=red>ConfirmMenuWindowController.GetButtonAtIndex(int index) - Cannot get button at index: " + index.ToString() + ". Index is out of bounds. Please call SetNumberOfButtons() first to turn on the required number of buttons.</color>");
			return null;
		}
		return this.m_activeButtonList[index];
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x0011814C File Offset: 0x0011634C
	private void SetBGHeight(float height)
	{
		Vector2 sizeDelta = this.m_confirmBoxBGRectTransform.sizeDelta;
		sizeDelta.y = height;
		this.m_confirmBoxBGRectTransform.sizeDelta = sizeDelta;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_confirmBoxBGRectTransform);
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x00027B33 File Offset: 0x00025D33
	public void SetTitleText(string text, bool isLocID)
	{
		if (isLocID)
		{
			this.m_titleText.text = LocalizationManager.GetString(text, false, false);
			this.m_refreshTitleTextLocID = text;
			return;
		}
		this.m_titleText.text = text;
		this.m_refreshTitleTextLocID = null;
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x00027B66 File Offset: 0x00025D66
	public void SetDescriptionText(string text, bool isLocID)
	{
		if (isLocID)
		{
			this.m_descriptionText.text = LocalizationManager.GetString(text, false, false);
			this.m_refreshDescriptionTextLocID = text;
			return;
		}
		this.m_descriptionText.text = text;
		this.m_refreshDescriptionTextLocID = null;
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x00027B99 File Offset: 0x00025D99
	public void SetOnCancelAction(Action cancelAction)
	{
		this.m_onCancelAction = cancelAction;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x00118184 File Offset: 0x00116384
	public void ApplyMATHack()
	{
		if (!this.m_playerMATHackApplied)
		{
			this.m_backgroundImage.material = this.m_backgroundHackMaterial;
			RectTransform component = this.m_backgroundImage.GetComponent<RectTransform>();
			Vector3 anchoredPosition3D = component.anchoredPosition3D;
			anchoredPosition3D.z = -50f;
			component.anchoredPosition3D = anchoredPosition3D;
			base.WindowCanvas.sortingLayerName = "Game";
			this.m_playerMATHackApplied = true;
		}
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x001181E8 File Offset: 0x001163E8
	private void OnConfirmPressed(InputActionEventData data)
	{
		if (data.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (this.m_selectedButtonIndex > -1 && this.m_selectedButtonIndex < this.m_activeButtonList.Count)
		{
			if (this.InputDelayed)
			{
				return;
			}
			this.m_activeButtonList[this.m_selectedButtonIndex].ButtonPressed();
			if (this.m_selectOptionEvent != null)
			{
				this.m_selectOptionEvent.Invoke();
			}
		}
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x00027BA2 File Offset: 0x00025DA2
	private void OnCancelPressed(InputActionEventData data)
	{
		if (this.InputDelayed)
		{
			return;
		}
		if (this.m_onCancelAction != null)
		{
			this.m_onCancelAction();
		}
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x00118250 File Offset: 0x00116450
	private void OnVerticalPressed(InputActionEventData data)
	{
		if (this.InputDelayed)
		{
			return;
		}
		int selectedButtonIndex = this.m_selectedButtonIndex;
		int num = selectedButtonIndex;
		if (num == -1)
		{
			num = 0;
		}
		else
		{
			float num2 = data.GetAxis();
			if (num2 == 0f)
			{
				num2 = -data.GetAxisPrev();
			}
			if (num2 < 0f)
			{
				num++;
			}
			else
			{
				num--;
			}
		}
		if (num < 0)
		{
			num = this.m_activeButtonList.Count - 1;
		}
		else if (num >= this.m_activeButtonList.Count)
		{
			num = 0;
		}
		if (num != selectedButtonIndex)
		{
			this.SetSelectedButton(num);
		}
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x001182D4 File Offset: 0x001164D4
	private void AddInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
		}
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x0011834C File Offset: 0x0011654C
	private void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
		}
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x001183C4 File Offset: 0x001165C4
	public void RefreshText(object sender, EventArgs args)
	{
		if (!string.IsNullOrEmpty(this.m_refreshTitleTextLocID))
		{
			this.m_descriptionText.text = LocalizationManager.GetString(this.m_refreshTitleTextLocID, false, false);
		}
		if (!string.IsNullOrEmpty(this.m_refreshDescriptionTextLocID))
		{
			this.m_descriptionText.text = LocalizationManager.GetString(this.m_refreshDescriptionTextLocID, false, false);
		}
		ConfirmMenu_Button[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			buttonArray[i].ForceRefreshText();
		}
	}

	// Token: 0x04003743 RID: 14147
	private const float DEFAULT_ONE_BUTTON_BG_HEIGHT = 765f;

	// Token: 0x04003744 RID: 14148
	private const float DEFAULT_ONE_BUTTON_BIG_BG_HEIGHT = 970f;

	// Token: 0x04003745 RID: 14149
	public const float BUTTON_HEIGHT = 100f;

	// Token: 0x04003746 RID: 14150
	[SerializeField]
	private RectTransform m_confirmBoxBGRectTransform;

	// Token: 0x04003747 RID: 14151
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x04003748 RID: 14152
	[SerializeField]
	private Image m_backgroundImage;

	// Token: 0x04003749 RID: 14153
	[SerializeField]
	private Material m_backgroundHackMaterial;

	// Token: 0x0400374A RID: 14154
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x0400374B RID: 14155
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x0400374C RID: 14156
	[SerializeField]
	private GameObject m_confirmMenuBox;

	// Token: 0x0400374D RID: 14157
	[SerializeField]
	private TMP_Text m_buttonDelayCountdownText;

	// Token: 0x0400374E RID: 14158
	[SerializeField]
	private bool m_isBigWindow;

	// Token: 0x0400374F RID: 14159
	[Space(10f)]
	[SerializeField]
	private UnityEvent m_windowOpenedEvent;

	// Token: 0x04003750 RID: 14160
	[SerializeField]
	private UnityEvent m_windowClosedEvent;

	// Token: 0x04003751 RID: 14161
	[SerializeField]
	private UnityEvent m_changeSelectedOptionEvent;

	// Token: 0x04003752 RID: 14162
	[SerializeField]
	private UnityEvent m_selectOptionEvent;

	// Token: 0x04003753 RID: 14163
	private ConfirmMenu_Button[] m_buttonArray;

	// Token: 0x04003754 RID: 14164
	private List<ConfirmMenu_Button> m_activeButtonList;

	// Token: 0x04003755 RID: 14165
	private int m_selectedButtonIndex;

	// Token: 0x04003756 RID: 14166
	private Action m_onCancelAction;

	// Token: 0x04003757 RID: 14167
	private Vector3 m_storedBoxScale;

	// Token: 0x04003758 RID: 14168
	private int m_startingButtonIndex;

	// Token: 0x04003759 RID: 14169
	private bool m_playerMATHackApplied;

	// Token: 0x0400375A RID: 14170
	private float m_buttonDelayTime;

	// Token: 0x0400375B RID: 14171
	private string m_refreshTitleTextLocID;

	// Token: 0x0400375C RID: 14172
	private string m_refreshDescriptionTextLocID;

	// Token: 0x0400375D RID: 14173
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400375E RID: 14174
	private Action<InputActionEventData> m_onConfirmPressed;

	// Token: 0x0400375F RID: 14175
	private Action<InputActionEventData> m_onCancelPressed;

	// Token: 0x04003760 RID: 14176
	private Action<InputActionEventData> m_onVerticalPressed;
}
