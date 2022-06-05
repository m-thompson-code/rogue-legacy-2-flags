using System;
using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020005F2 RID: 1522
public class ConfirmMenu_Button : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x17001287 RID: 4743
	// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x000199DB File Offset: 0x00017BDB
	// (set) Token: 0x06002EE3 RID: 12003 RVA: 0x000199E3 File Offset: 0x00017BE3
	public bool DisableClick { get; set; }

	// Token: 0x17001288 RID: 4744
	// (get) Token: 0x06002EE4 RID: 12004 RVA: 0x000199EC File Offset: 0x00017BEC
	// (set) Token: 0x06002EE5 RID: 12005 RVA: 0x000199F4 File Offset: 0x00017BF4
	public int Index { get; private set; }

	// Token: 0x06002EE6 RID: 12006 RVA: 0x000C8D8C File Offset: 0x000C6F8C
	public void Initialize(int index, Action<ConfirmMenu_Button> onPointerEnterAction, Action onPointerExitAction, Action<ConfirmMenu_Button> onPointerClickAction)
	{
		this.Index = index;
		this.m_onPointerEnterAction = onPointerEnterAction;
		this.m_onPointerExitAction = onPointerExitAction;
		this.m_onPointerClickAction = onPointerClickAction;
		this.m_selectImageCanvasGroup.gameObject.SetActive(false);
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x000199FD File Offset: 0x00017BFD
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (RewiredOnStartupController.CurrentActiveControllerType == ControllerType.Joystick)
		{
			return;
		}
		this.OnSelect(null);
		if (this.m_onPointerEnterAction != null)
		{
			this.m_onPointerEnterAction(this);
		}
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x00002FCA File Offset: 0x000011CA
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x00019A23 File Offset: 0x00017C23
	public void OnPointerClick(PointerEventData eventData)
	{
		if (RewiredOnStartupController.CurrentActiveControllerType == ControllerType.Joystick)
		{
			return;
		}
		if (this.DisableClick)
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (this.m_onPointerClickAction != null)
		{
			this.m_onPointerClickAction(this);
		}
		this.ButtonPressed();
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x00019A5A File Offset: 0x00017C5A
	public void OnSelect(BaseEventData eventData)
	{
		if (!this.m_selected)
		{
			this.m_selected = true;
			this.m_selectImageCanvasGroup.gameObject.SetActive(true);
			this.m_selectStartTime = Time.unscaledTime;
		}
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x00019A87 File Offset: 0x00017C87
	public void OnDeselect(BaseEventData eventData)
	{
		this.m_selected = false;
		this.m_selectImageCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x00019AA1 File Offset: 0x00017CA1
	public void ButtonPressed()
	{
		this.OnDeselect(null);
		this.RunOnConfirmPressedAnimation();
		base.StartCoroutine(this.OnClickCoroutine());
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x00019ABD File Offset: 0x00017CBD
	private IEnumerator OnClickCoroutine()
	{
		yield return null;
		if (this.m_onClickAction != null)
		{
			this.m_onClickAction();
		}
		yield break;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x00019ACC File Offset: 0x00017CCC
	public void SetOnClickAction(Action action)
	{
		this.m_onClickAction = action;
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x00019AD5 File Offset: 0x00017CD5
	public void SetButtonText(string text, bool isLocID)
	{
		if (isLocID)
		{
			this.m_text.text = LocalizationManager.GetString(text, false, false);
			this.m_refreshTextLocID = text;
			return;
		}
		this.m_text.text = text;
		this.m_refreshTextLocID = null;
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000C8DD8 File Offset: 0x000C6FD8
	protected virtual void RunOnConfirmPressedAnimation()
	{
		Vector3 storedScale = this.m_storedScale;
		storedScale.x = ((storedScale.x > 0f) ? 0.9f : -0.9f);
		storedScale.y = ((storedScale.y > 0f) ? 0.9f : -0.9f);
		base.transform.localScale = storedScale;
		TweenManager.TweenTo_UnscaledTime(base.transform, 0.05f, new EaseDelegate(Ease.None), new object[]
		{
			"localScale.x",
			(storedScale.x > 0f) ? 1 : -1,
			"localScale.y",
			(storedScale.y > 0f) ? 1 : -1
		});
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x00019B08 File Offset: 0x00017D08
	private void Update()
	{
		if (this.m_selected)
		{
			this.m_selectImageCanvasGroup.alpha = Mathf.Abs(Mathf.Cos((Time.unscaledTime - this.m_selectStartTime) * 3f));
		}
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000C8EA0 File Offset: 0x000C70A0
	public void ResetValues()
	{
		this.m_selectImageCanvasGroup.gameObject.SetActive(false);
		this.m_selected = false;
		this.m_onClickAction = null;
		this.m_text.text = "Button";
		base.gameObject.SetActive(false);
		if (this.m_storedScale != Vector3.zero)
		{
			base.transform.localScale = this.m_storedScale;
		}
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x00019B39 File Offset: 0x00017D39
	public void ForceRefreshText()
	{
		if (!string.IsNullOrEmpty(this.m_refreshTextLocID))
		{
			this.m_text.text = LocalizationManager.GetString(this.m_refreshTextLocID, false, false);
		}
	}

	// Token: 0x04002652 RID: 9810
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04002653 RID: 9811
	[SerializeField]
	private CanvasGroup m_selectImageCanvasGroup;

	// Token: 0x04002654 RID: 9812
	private Action m_onClickAction;

	// Token: 0x04002655 RID: 9813
	private Action m_onPointerExitAction;

	// Token: 0x04002656 RID: 9814
	private Action<ConfirmMenu_Button> m_onPointerEnterAction;

	// Token: 0x04002657 RID: 9815
	private Action<ConfirmMenu_Button> m_onPointerClickAction;

	// Token: 0x04002658 RID: 9816
	private Vector3 m_storedScale;

	// Token: 0x04002659 RID: 9817
	private bool m_selected;

	// Token: 0x0400265A RID: 9818
	private float m_selectStartTime;

	// Token: 0x0400265B RID: 9819
	private string m_refreshTextLocID;
}
