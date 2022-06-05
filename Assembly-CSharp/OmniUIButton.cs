using System;
using System.Collections;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000656 RID: 1622
public abstract class OmniUIButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerExitHandler
{
	// Token: 0x1700131C RID: 4892
	// (get) Token: 0x06003170 RID: 12656 RVA: 0x0001B225 File Offset: 0x00019425
	// (set) Token: 0x06003171 RID: 12657 RVA: 0x0001B22D File Offset: 0x0001942D
	public bool Interactable { get; set; }

	// Token: 0x1700131D RID: 4893
	// (get) Token: 0x06003172 RID: 12658 RVA: 0x0001B236 File Offset: 0x00019436
	// (set) Token: 0x06003173 RID: 12659 RVA: 0x0001B23E File Offset: 0x0001943E
	public int ButtonIndex { get; set; }

	// Token: 0x1700131E RID: 4894
	// (get) Token: 0x06003174 RID: 12660 RVA: 0x0001B247 File Offset: 0x00019447
	// (set) Token: 0x06003175 RID: 12661 RVA: 0x0001B24F File Offset: 0x0001944F
	public OmniUIButtonHandler OnSelectEvent { get; set; }

	// Token: 0x1700131F RID: 4895
	// (get) Token: 0x06003176 RID: 12662 RVA: 0x0001B258 File Offset: 0x00019458
	// (set) Token: 0x06003177 RID: 12663 RVA: 0x0001B260 File Offset: 0x00019460
	public OmniUIButtonHandler OnClickEvent { get; set; }

	// Token: 0x17001320 RID: 4896
	// (get) Token: 0x06003178 RID: 12664
	public abstract EventArgs ButtonEventArgs { get; }

	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x06003179 RID: 12665 RVA: 0x0001B269 File Offset: 0x00019469
	public IRelayLink<OmniUIButton> OnConfirmPressedRelay
	{
		get
		{
			return this.m_onConfirmPressedRelay.link;
		}
	}

	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x0600317A RID: 12666 RVA: 0x0001B276 File Offset: 0x00019476
	// (set) Token: 0x0600317B RID: 12667 RVA: 0x0001B27E File Offset: 0x0001947E
	public virtual bool IsButtonActive { get; protected set; } = true;

	// Token: 0x0600317C RID: 12668
	public abstract void UpdateState();

	// Token: 0x0600317D RID: 12669 RVA: 0x0001B287 File Offset: 0x00019487
	protected virtual void Awake()
	{
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x0600317E RID: 12670
	protected abstract void InitializeButtonEventArgs();

	// Token: 0x0600317F RID: 12671 RVA: 0x0001B29A File Offset: 0x0001949A
	public virtual void OnConfirmButtonPressed()
	{
		if (base.gameObject.activeSelf)
		{
			BaseOmniUIEntry.StaticSelectedButtonIndex = this.ButtonIndex;
		}
		this.m_onConfirmPressedRelay.Dispatch(this);
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x000D3860 File Offset: 0x000D1A60
	protected virtual void RunOnConfirmPressedAnimation()
	{
		Vector3 localScale = this.m_storedScale * 0.9f;
		base.transform.localScale = localScale;
		TweenManager.TweenTo_UnscaledTime(base.transform, 0.05f, new EaseDelegate(Ease.None), new object[]
		{
			"localScale.x",
			this.m_storedScale.x,
			"localScale.y",
			this.m_storedScale.y
		});
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x0001B2C0 File Offset: 0x000194C0
	protected virtual IEnumerator ShakeAnimCoroutine()
	{
		int shakeCount = 0;
		float z = 5f;
		float shakeDelay = 0.05f;
		float shakeTime = Time.unscaledTime + shakeDelay;
		Vector3 shakeEuler = base.transform.localEulerAngles;
		shakeEuler.z = z;
		base.transform.localEulerAngles = shakeEuler;
		while (shakeCount < 3)
		{
			if (Time.unscaledTime >= shakeTime)
			{
				shakeEuler.z *= -1f;
				base.transform.localEulerAngles = shakeEuler;
				shakeTime = Time.unscaledTime + shakeDelay;
				int num = shakeCount;
				shakeCount = num + 1;
			}
			yield return null;
		}
		shakeEuler.z = 0f;
		base.transform.localEulerAngles = shakeEuler;
		yield break;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x000D38E4 File Offset: 0x000D1AE4
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.m_selectedSprite && !this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(true);
		}
		if (this.m_deselectedSprite && this.m_deselectedSprite.gameObject.activeSelf)
		{
			this.m_deselectedSprite.gameObject.SetActive(false);
		}
		if (this.OnSelectEvent != null)
		{
			this.OnSelectEvent(this);
		}
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x000D3984 File Offset: 0x000D1B84
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.m_selectedSprite && this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(false);
		}
		if (this.m_deselectedSprite && !this.m_deselectedSprite.gameObject.activeSelf)
		{
			this.m_deselectedSprite.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x0001B2CF File Offset: 0x000194CF
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (!this.IsButtonActive)
		{
			return;
		}
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Mouse)
		{
			return;
		}
		this.OnSelect(null);
		BaseOmniUIEntry.StaticSelectedButtonIndex = this.ButtonIndex;
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x0001B2FE File Offset: 0x000194FE
	public virtual void OnPointerExit(PointerEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (!this.IsButtonActive)
		{
			return;
		}
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Mouse)
		{
			return;
		}
		this.OnDeselect(null);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x0001B322 File Offset: 0x00019522
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (!this.IsButtonActive)
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Mouse)
		{
			return;
		}
		this.OnConfirmButtonPressed();
		if (this.OnClickEvent != null)
		{
			this.OnClickEvent(this);
		}
	}

	// Token: 0x0400284A RID: 10314
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x0400284B RID: 10315
	[SerializeField]
	protected Image m_deselectedSprite;

	// Token: 0x0400284C RID: 10316
	private Vector3 m_storedScale;

	// Token: 0x04002851 RID: 10321
	protected Relay<OmniUIButton> m_onConfirmPressedRelay = new Relay<OmniUIButton>();
}
