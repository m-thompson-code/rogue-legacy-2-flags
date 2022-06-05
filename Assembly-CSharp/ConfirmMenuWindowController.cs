using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000574 RID: 1396
public class ConfirmMenuWindowController : WindowController, ILocalizable
{
	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06003341 RID: 13121 RVA: 0x000AD6CB File Offset: 0x000AB8CB
	protected bool InputDelayed
	{
		get
		{
			return Time.unscaledTime < this.m_buttonDelayTime;
		}
	}

	// Token: 0x1700127D RID: 4733
	// (get) Token: 0x06003342 RID: 13122 RVA: 0x000AD6DA File Offset: 0x000AB8DA
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

	// Token: 0x06003343 RID: 13123 RVA: 0x000AD6EC File Offset: 0x000AB8EC
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
		this.m_onCancelPressed = new Action<InputActionEventData>(this.OnCancelPressed);
		this.m_onVerticalPressed = new Action<InputActionEventData>(this.OnVerticalPressed);
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x000AD744 File Offset: 0x000AB944
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

	// Token: 0x06003345 RID: 13125 RVA: 0x000AD7EA File Offset: 0x000AB9EA
	private void OnPointerExit()
	{
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x000AD7EC File Offset: 0x000AB9EC
	private void OnPointerEnter(ConfirmMenu_Button button)
	{
		this.SetSelectedButton(button.Index);
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x000AD7FA File Offset: 0x000AB9FA
	private void PlayClickSFX(ConfirmMenu_Button button)
	{
		if (this.m_selectOptionEvent != null)
		{
			this.m_selectOptionEvent.Invoke();
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x000AD810 File Offset: 0x000ABA10
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

	// Token: 0x06003349 RID: 13129 RVA: 0x000AD8CC File Offset: 0x000ABACC
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x000AD8D4 File Offset: 0x000ABAD4
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x000AD8DC File Offset: 0x000ABADC
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

	// Token: 0x0600334C RID: 13132 RVA: 0x000AD93A File Offset: 0x000ABB3A
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

	// Token: 0x0600334D RID: 13133 RVA: 0x000AD949 File Offset: 0x000ABB49
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

	// Token: 0x0600334E RID: 13134 RVA: 0x000AD958 File Offset: 0x000ABB58
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

	// Token: 0x0600334F RID: 13135 RVA: 0x000ADA1B File Offset: 0x000ABC1B
	public void SetStartingSelectedButton(int startingButtonIndex)
	{
		this.m_startingButtonIndex = startingButtonIndex;
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x000ADA24 File Offset: 0x000ABC24
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

	// Token: 0x06003351 RID: 13137 RVA: 0x000ADAF3 File Offset: 0x000ABCF3
	public void SetButtonDelayTime(float delayTime)
	{
		this.m_buttonDelayTime = delayTime;
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x000ADAFC File Offset: 0x000ABCFC
	public ConfirmMenu_Button GetButtonAtIndex(int index)
	{
		if (index < 0 || index >= this.m_activeButtonList.Count)
		{
			Debug.Log("<color=red>ConfirmMenuWindowController.GetButtonAtIndex(int index) - Cannot get button at index: " + index.ToString() + ". Index is out of bounds. Please call SetNumberOfButtons() first to turn on the required number of buttons.</color>");
			return null;
		}
		return this.m_activeButtonList[index];
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x000ADB3C File Offset: 0x000ABD3C
	private void SetBGHeight(float height)
	{
		Vector2 sizeDelta = this.m_confirmBoxBGRectTransform.sizeDelta;
		sizeDelta.y = height;
		this.m_confirmBoxBGRectTransform.sizeDelta = sizeDelta;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_confirmBoxBGRectTransform);
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x000ADB74 File Offset: 0x000ABD74
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

	// Token: 0x06003355 RID: 13141 RVA: 0x000ADBA7 File Offset: 0x000ABDA7
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

	// Token: 0x06003356 RID: 13142 RVA: 0x000ADBDA File Offset: 0x000ABDDA
	public void SetOnCancelAction(Action cancelAction)
	{
		this.m_onCancelAction = cancelAction;
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000ADBE4 File Offset: 0x000ABDE4
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

	// Token: 0x06003358 RID: 13144 RVA: 0x000ADC48 File Offset: 0x000ABE48
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

	// Token: 0x06003359 RID: 13145 RVA: 0x000ADCAE File Offset: 0x000ABEAE
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

	// Token: 0x0600335A RID: 13146 RVA: 0x000ADCCC File Offset: 0x000ABECC
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

	// Token: 0x0600335B RID: 13147 RVA: 0x000ADD50 File Offset: 0x000ABF50
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

	// Token: 0x0600335C RID: 13148 RVA: 0x000ADDC8 File Offset: 0x000ABFC8
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

	// Token: 0x0600335D RID: 13149 RVA: 0x000ADE40 File Offset: 0x000AC040
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

	// Token: 0x04002805 RID: 10245
	private const float DEFAULT_ONE_BUTTON_BG_HEIGHT = 765f;

	// Token: 0x04002806 RID: 10246
	private const float DEFAULT_ONE_BUTTON_BIG_BG_HEIGHT = 970f;

	// Token: 0x04002807 RID: 10247
	public const float BUTTON_HEIGHT = 100f;

	// Token: 0x04002808 RID: 10248
	[SerializeField]
	private RectTransform m_confirmBoxBGRectTransform;

	// Token: 0x04002809 RID: 10249
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x0400280A RID: 10250
	[SerializeField]
	private Image m_backgroundImage;

	// Token: 0x0400280B RID: 10251
	[SerializeField]
	private Material m_backgroundHackMaterial;

	// Token: 0x0400280C RID: 10252
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x0400280D RID: 10253
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x0400280E RID: 10254
	[SerializeField]
	private GameObject m_confirmMenuBox;

	// Token: 0x0400280F RID: 10255
	[SerializeField]
	private TMP_Text m_buttonDelayCountdownText;

	// Token: 0x04002810 RID: 10256
	[SerializeField]
	private bool m_isBigWindow;

	// Token: 0x04002811 RID: 10257
	[Space(10f)]
	[SerializeField]
	private UnityEvent m_windowOpenedEvent;

	// Token: 0x04002812 RID: 10258
	[SerializeField]
	private UnityEvent m_windowClosedEvent;

	// Token: 0x04002813 RID: 10259
	[SerializeField]
	private UnityEvent m_changeSelectedOptionEvent;

	// Token: 0x04002814 RID: 10260
	[SerializeField]
	private UnityEvent m_selectOptionEvent;

	// Token: 0x04002815 RID: 10261
	private ConfirmMenu_Button[] m_buttonArray;

	// Token: 0x04002816 RID: 10262
	private List<ConfirmMenu_Button> m_activeButtonList;

	// Token: 0x04002817 RID: 10263
	private int m_selectedButtonIndex;

	// Token: 0x04002818 RID: 10264
	private Action m_onCancelAction;

	// Token: 0x04002819 RID: 10265
	private Vector3 m_storedBoxScale;

	// Token: 0x0400281A RID: 10266
	private int m_startingButtonIndex;

	// Token: 0x0400281B RID: 10267
	private bool m_playerMATHackApplied;

	// Token: 0x0400281C RID: 10268
	private float m_buttonDelayTime;

	// Token: 0x0400281D RID: 10269
	private string m_refreshTitleTextLocID;

	// Token: 0x0400281E RID: 10270
	private string m_refreshDescriptionTextLocID;

	// Token: 0x0400281F RID: 10271
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002820 RID: 10272
	private Action<InputActionEventData> m_onConfirmPressed;

	// Token: 0x04002821 RID: 10273
	private Action<InputActionEventData> m_onCancelPressed;

	// Token: 0x04002822 RID: 10274
	private Action<InputActionEventData> m_onVerticalPressed;
}
