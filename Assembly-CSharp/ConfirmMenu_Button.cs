using System;
using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000372 RID: 882
public class ConfirmMenu_Button : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x06002113 RID: 8467 RVA: 0x00067E7A File Offset: 0x0006607A
	// (set) Token: 0x06002114 RID: 8468 RVA: 0x00067E82 File Offset: 0x00066082
	public bool DisableClick { get; set; }

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x06002115 RID: 8469 RVA: 0x00067E8B File Offset: 0x0006608B
	// (set) Token: 0x06002116 RID: 8470 RVA: 0x00067E93 File Offset: 0x00066093
	public int Index { get; private set; }

	// Token: 0x06002117 RID: 8471 RVA: 0x00067E9C File Offset: 0x0006609C
	public void Initialize(int index, Action<ConfirmMenu_Button> onPointerEnterAction, Action onPointerExitAction, Action<ConfirmMenu_Button> onPointerClickAction)
	{
		this.Index = index;
		this.m_onPointerEnterAction = onPointerEnterAction;
		this.m_onPointerExitAction = onPointerExitAction;
		this.m_onPointerClickAction = onPointerClickAction;
		this.m_selectImageCanvasGroup.gameObject.SetActive(false);
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x00067EE8 File Offset: 0x000660E8
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

	// Token: 0x06002119 RID: 8473 RVA: 0x00067F0E File Offset: 0x0006610E
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x00067F10 File Offset: 0x00066110
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

	// Token: 0x0600211B RID: 8475 RVA: 0x00067F47 File Offset: 0x00066147
	public void OnSelect(BaseEventData eventData)
	{
		if (!this.m_selected)
		{
			this.m_selected = true;
			this.m_selectImageCanvasGroup.gameObject.SetActive(true);
			this.m_selectStartTime = Time.unscaledTime;
		}
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x00067F74 File Offset: 0x00066174
	public void OnDeselect(BaseEventData eventData)
	{
		this.m_selected = false;
		this.m_selectImageCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x00067F8E File Offset: 0x0006618E
	public void ButtonPressed()
	{
		this.OnDeselect(null);
		this.RunOnConfirmPressedAnimation();
		base.StartCoroutine(this.OnClickCoroutine());
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x00067FAA File Offset: 0x000661AA
	private IEnumerator OnClickCoroutine()
	{
		yield return null;
		if (this.m_onClickAction != null)
		{
			this.m_onClickAction();
		}
		yield break;
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x00067FB9 File Offset: 0x000661B9
	public void SetOnClickAction(Action action)
	{
		this.m_onClickAction = action;
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x00067FC2 File Offset: 0x000661C2
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

	// Token: 0x06002121 RID: 8481 RVA: 0x00067FF8 File Offset: 0x000661F8
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

	// Token: 0x06002122 RID: 8482 RVA: 0x000680BD File Offset: 0x000662BD
	private void Update()
	{
		if (this.m_selected)
		{
			this.m_selectImageCanvasGroup.alpha = Mathf.Abs(Mathf.Cos((Time.unscaledTime - this.m_selectStartTime) * 3f));
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000680F0 File Offset: 0x000662F0
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

	// Token: 0x06002124 RID: 8484 RVA: 0x0006815B File Offset: 0x0006635B
	public void ForceRefreshText()
	{
		if (!string.IsNullOrEmpty(this.m_refreshTextLocID))
		{
			this.m_text.text = LocalizationManager.GetString(this.m_refreshTextLocID, false, false);
		}
	}

	// Token: 0x04001C9A RID: 7322
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001C9B RID: 7323
	[SerializeField]
	private CanvasGroup m_selectImageCanvasGroup;

	// Token: 0x04001C9C RID: 7324
	private Action m_onClickAction;

	// Token: 0x04001C9D RID: 7325
	private Action m_onPointerExitAction;

	// Token: 0x04001C9E RID: 7326
	private Action<ConfirmMenu_Button> m_onPointerEnterAction;

	// Token: 0x04001C9F RID: 7327
	private Action<ConfirmMenu_Button> m_onPointerClickAction;

	// Token: 0x04001CA0 RID: 7328
	private Vector3 m_storedScale;

	// Token: 0x04001CA1 RID: 7329
	private bool m_selected;

	// Token: 0x04001CA2 RID: 7330
	private float m_selectStartTime;

	// Token: 0x04001CA3 RID: 7331
	private string m_refreshTextLocID;
}
