using System;
using System.Collections;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020003BF RID: 959
public abstract class OmniUIButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerExitHandler
{
	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x06002358 RID: 9048 RVA: 0x0007344B File Offset: 0x0007164B
	// (set) Token: 0x06002359 RID: 9049 RVA: 0x00073453 File Offset: 0x00071653
	public bool Interactable { get; set; }

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x0600235A RID: 9050 RVA: 0x0007345C File Offset: 0x0007165C
	// (set) Token: 0x0600235B RID: 9051 RVA: 0x00073464 File Offset: 0x00071664
	public int ButtonIndex { get; set; }

	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x0600235C RID: 9052 RVA: 0x0007346D File Offset: 0x0007166D
	// (set) Token: 0x0600235D RID: 9053 RVA: 0x00073475 File Offset: 0x00071675
	public OmniUIButtonHandler OnSelectEvent { get; set; }

	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x0600235E RID: 9054 RVA: 0x0007347E File Offset: 0x0007167E
	// (set) Token: 0x0600235F RID: 9055 RVA: 0x00073486 File Offset: 0x00071686
	public OmniUIButtonHandler OnClickEvent { get; set; }

	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x06002360 RID: 9056
	public abstract EventArgs ButtonEventArgs { get; }

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06002361 RID: 9057 RVA: 0x0007348F File Offset: 0x0007168F
	public IRelayLink<OmniUIButton> OnConfirmPressedRelay
	{
		get
		{
			return this.m_onConfirmPressedRelay.link;
		}
	}

	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06002362 RID: 9058 RVA: 0x0007349C File Offset: 0x0007169C
	// (set) Token: 0x06002363 RID: 9059 RVA: 0x000734A4 File Offset: 0x000716A4
	public virtual bool IsButtonActive { get; protected set; } = true;

	// Token: 0x06002364 RID: 9060
	public abstract void UpdateState();

	// Token: 0x06002365 RID: 9061 RVA: 0x000734AD File Offset: 0x000716AD
	protected virtual void Awake()
	{
		this.m_storedScale = base.transform.localScale;
	}

	// Token: 0x06002366 RID: 9062
	protected abstract void InitializeButtonEventArgs();

	// Token: 0x06002367 RID: 9063 RVA: 0x000734C0 File Offset: 0x000716C0
	public virtual void OnConfirmButtonPressed()
	{
		if (base.gameObject.activeSelf)
		{
			BaseOmniUIEntry.StaticSelectedButtonIndex = this.ButtonIndex;
		}
		this.m_onConfirmPressedRelay.Dispatch(this);
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000734E8 File Offset: 0x000716E8
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

	// Token: 0x06002369 RID: 9065 RVA: 0x0007356B File Offset: 0x0007176B
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

	// Token: 0x0600236A RID: 9066 RVA: 0x0007357C File Offset: 0x0007177C
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

	// Token: 0x0600236B RID: 9067 RVA: 0x0007361C File Offset: 0x0007181C
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

	// Token: 0x0600236C RID: 9068 RVA: 0x00073692 File Offset: 0x00071892
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

	// Token: 0x0600236D RID: 9069 RVA: 0x000736C1 File Offset: 0x000718C1
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

	// Token: 0x0600236E RID: 9070 RVA: 0x000736E5 File Offset: 0x000718E5
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

	// Token: 0x04001E18 RID: 7704
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x04001E19 RID: 7705
	[SerializeField]
	protected Image m_deselectedSprite;

	// Token: 0x04001E1A RID: 7706
	private Vector3 m_storedScale;

	// Token: 0x04001E1F RID: 7711
	protected Relay<OmniUIButton> m_onConfirmPressedRelay = new Relay<OmniUIButton>();
}
