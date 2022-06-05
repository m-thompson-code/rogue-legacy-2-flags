using System;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000392 RID: 914
public abstract class BaseOmniUIEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
{
	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x06002215 RID: 8725
	public abstract bool IsEntryActive { get; }

	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x06002216 RID: 8726
	public abstract EventArgs EntryEventArgs { get; }

	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x06002217 RID: 8727 RVA: 0x0006C9F4 File Offset: 0x0006ABF4
	// (set) Token: 0x06002218 RID: 8728 RVA: 0x0006C9FC File Offset: 0x0006ABFC
	public bool Interactable
	{
		get
		{
			return this.m_interactable;
		}
		set
		{
			if (this.m_buttonArray != null)
			{
				OmniUIButton[] buttonArray = this.m_buttonArray;
				for (int i = 0; i < buttonArray.Length; i++)
				{
					buttonArray[i].Interactable = value;
				}
			}
			this.m_interactable = value;
		}
	}

	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06002219 RID: 8729 RVA: 0x0006CA38 File Offset: 0x0006AC38
	public bool HasActiveButtons
	{
		get
		{
			if (this.m_buttonArray.Length != 0)
			{
				foreach (OmniUIButton omniUIButton in this.m_buttonArray)
				{
					if (omniUIButton.gameObject.activeInHierarchy && omniUIButton.IsButtonActive)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x0600221A RID: 8730 RVA: 0x0006CA7F File Offset: 0x0006AC7F
	// (set) Token: 0x0600221B RID: 8731 RVA: 0x0006CA87 File Offset: 0x0006AC87
	public int EntryIndex { get; protected set; }

	// Token: 0x0600221C RID: 8732 RVA: 0x0006CA90 File Offset: 0x0006AC90
	public int GetFirstActiveButtonIndex()
	{
		if (this.HasActiveButtons)
		{
			for (int i = 0; i < this.m_buttonArray.Length; i++)
			{
				if (this.m_buttonArray[i].gameObject.activeInHierarchy && this.m_buttonArray[i].IsButtonActive)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x0006CAE0 File Offset: 0x0006ACE0
	public int GetLastActiveButtonIndex()
	{
		if (this.HasActiveButtons)
		{
			for (int i = this.m_buttonArray.Length - 1; i >= 0; i--)
			{
				if (this.m_buttonArray[i].gameObject.activeInHierarchy && this.m_buttonArray[i].IsButtonActive)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x0600221E RID: 8734 RVA: 0x0006CB30 File Offset: 0x0006AD30
	public IOmniUIWindowController WindowController
	{
		get
		{
			return this.m_windowController;
		}
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x0006CB38 File Offset: 0x0006AD38
	public virtual void Initialize(IOmniUIWindowController windowController)
	{
		this.m_windowController = windowController;
		this.m_buttonArray = base.GetComponentsInChildren<OmniUIButton>();
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.m_buttonArray[i].ButtonIndex = i;
			OmniUIButton omniUIButton = this.m_buttonArray[i];
			omniUIButton.OnSelectEvent = (OmniUIButtonHandler)Delegate.Combine(omniUIButton.OnSelectEvent, new OmniUIButtonHandler(this.OnButtonSelected));
			this.m_buttonArray[i].OnConfirmPressedRelay.AddListener(new Action<OmniUIButton>(this.PlayConfirmPressedSFX), false);
		}
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x0006CBC2 File Offset: 0x0006ADC2
	public virtual void SetEntryIndex(int index)
	{
		this.EntryIndex = index;
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x0006CBCB File Offset: 0x0006ADCB
	private void OnButtonSelected(OmniUIButton button)
	{
		if (this.m_localSelectedButtonIndex != button.ButtonIndex)
		{
			if (this.m_localSelectedButtonIndex < this.m_buttonArray.Length)
			{
				this.m_buttonArray[this.m_localSelectedButtonIndex].OnDeselect(null);
			}
			this.m_localSelectedButtonIndex = button.ButtonIndex;
		}
	}

	// Token: 0x06002222 RID: 8738
	public abstract void UpdateActive();

	// Token: 0x06002223 RID: 8739 RVA: 0x0006CC0C File Offset: 0x0006AE0C
	public virtual void UpdateState()
	{
		if (!this.IsEntryActive)
		{
			this.m_titleText.text = "???";
		}
		if (!this.IsEntryActive)
		{
			if (this.m_iconGO && this.m_iconGO.activeSelf)
			{
				this.m_iconGO.SetActive(false);
			}
			if (this.m_inactiveIconGO && !this.m_inactiveIconGO.activeSelf)
			{
				this.m_inactiveIconGO.SetActive(true);
			}
		}
		else
		{
			if (this.m_iconGO && !this.m_iconGO.activeSelf)
			{
				this.m_iconGO.SetActive(true);
			}
			if (this.m_inactiveIconGO && this.m_inactiveIconGO.activeSelf)
			{
				this.m_inactiveIconGO.SetActive(false);
			}
		}
		if (this.m_buttonArray == null || this.m_buttonArray.Length == 0)
		{
			return;
		}
		foreach (OmniUIButton omniUIButton in this.m_buttonArray)
		{
			GameObject gameObject = omniUIButton.gameObject;
			if (omniUIButton.transform.parent.gameObject != base.gameObject)
			{
				gameObject = omniUIButton.transform.parent.gameObject;
			}
			if (!this.IsEntryActive)
			{
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
			else
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
				omniUIButton.UpdateState();
			}
		}
		if (this.m_windowController.SelectedEntryIndex == this.EntryIndex)
		{
			OmniUIButton omniUIButton2 = this.m_buttonArray[this.m_localSelectedButtonIndex];
			if (!this.m_buttonArray[this.m_localSelectedButtonIndex].IsButtonActive)
			{
				if (this.GetFirstActiveButtonIndex() < this.m_localSelectedButtonIndex)
				{
					this.SelectButton(false);
				}
				else if (this.GetLastActiveButtonIndex() > this.m_localSelectedButtonIndex)
				{
					this.SelectButton(true);
				}
			}
			if (omniUIButton2 == this.m_buttonArray[this.m_localSelectedButtonIndex] && !omniUIButton2.IsButtonActive)
			{
				omniUIButton2.OnDeselect(null);
			}
		}
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x0006CDEC File Offset: 0x0006AFEC
	public virtual void DeselectAllButtons()
	{
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			buttonArray[i].OnDeselect(null);
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x0006CE18 File Offset: 0x0006B018
	public void SelectRightMostButton()
	{
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.SelectButton(true);
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x0006CE40 File Offset: 0x0006B040
	public void SelectLeftMostButton()
	{
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.SelectButton(false);
		}
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x0006CE68 File Offset: 0x0006B068
	public void SelectButton(bool selectRight)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (!this.HasActiveButtons)
		{
			return;
		}
		if (!this.IsEntryActive)
		{
			return;
		}
		int firstActiveButtonIndex = this.GetFirstActiveButtonIndex();
		int lastActiveButtonIndex = this.GetLastActiveButtonIndex();
		int localSelectedButtonIndex = this.m_localSelectedButtonIndex;
		if (selectRight)
		{
			while (BaseOmniUIEntry.StaticSelectedButtonIndex < lastActiveButtonIndex)
			{
				BaseOmniUIEntry.StaticSelectedButtonIndex++;
				if (this.m_buttonArray[BaseOmniUIEntry.StaticSelectedButtonIndex].IsButtonActive)
				{
					break;
				}
			}
		}
		else
		{
			while (BaseOmniUIEntry.StaticSelectedButtonIndex > firstActiveButtonIndex)
			{
				BaseOmniUIEntry.StaticSelectedButtonIndex--;
				if (this.m_buttonArray[BaseOmniUIEntry.StaticSelectedButtonIndex].IsButtonActive)
				{
					break;
				}
			}
		}
		if (selectRight)
		{
			while (this.m_localSelectedButtonIndex < lastActiveButtonIndex)
			{
				this.m_localSelectedButtonIndex++;
				if (this.m_buttonArray[this.m_localSelectedButtonIndex].IsButtonActive)
				{
					break;
				}
			}
		}
		else
		{
			while (this.m_localSelectedButtonIndex > firstActiveButtonIndex)
			{
				this.m_localSelectedButtonIndex--;
				if (this.m_buttonArray[this.m_localSelectedButtonIndex].IsButtonActive)
				{
					break;
				}
			}
		}
		if (localSelectedButtonIndex != this.m_localSelectedButtonIndex)
		{
			this.m_buttonArray[this.m_localSelectedButtonIndex].OnSelect(null);
			this.m_buttonArray[localSelectedButtonIndex].OnDeselect(null);
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x0006CF84 File Offset: 0x0006B184
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (!this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(true);
		}
		if (this.HasActiveButtons)
		{
			this.m_localSelectedButtonIndex = BaseOmniUIEntry.StaticSelectedButtonIndex;
			int firstActiveButtonIndex = this.GetFirstActiveButtonIndex();
			int lastActiveButtonIndex = this.GetLastActiveButtonIndex();
			if (this.m_localSelectedButtonIndex >= lastActiveButtonIndex)
			{
				this.m_localSelectedButtonIndex = lastActiveButtonIndex;
			}
			if (this.m_localSelectedButtonIndex < firstActiveButtonIndex)
			{
				this.m_localSelectedButtonIndex = firstActiveButtonIndex;
			}
			while (this.m_localSelectedButtonIndex > firstActiveButtonIndex && this.m_localSelectedButtonIndex < lastActiveButtonIndex && !this.m_buttonArray[this.m_localSelectedButtonIndex].IsButtonActive)
			{
				if (this.m_buttonArray[this.m_localSelectedButtonIndex - 1].IsButtonActive)
				{
					this.m_localSelectedButtonIndex--;
				}
				else
				{
					this.m_localSelectedButtonIndex++;
				}
			}
			this.m_buttonArray[this.m_localSelectedButtonIndex].OnSelect(null);
		}
		if (!this.IsEntryActive)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, null);
			return;
		}
		if (!this.HasActiveButtons)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.EntryEventArgs);
		}
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x0006D098 File Offset: 0x0006B298
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(false);
		}
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			buttonArray[i].OnDeselect(null);
		}
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x0006D0EF File Offset: 0x0006B2EF
	private void PlayConfirmPressedSFX(OmniUIButton button)
	{
		if (this.m_windowController.SelectOptionEvent != null)
		{
			this.m_windowController.SelectOptionEvent.Invoke();
		}
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x0006D10E File Offset: 0x0006B30E
	public virtual void OnConfirmButtonPressed()
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.HasActiveButtons)
		{
			this.m_buttonArray[this.m_localSelectedButtonIndex].OnConfirmButtonPressed();
		}
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x0006D133 File Offset: 0x0006B333
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Mouse)
		{
			return;
		}
		if (!this.Interactable)
		{
			return;
		}
		this.m_windowController.SetSelectedEntryIndex(this.EntryIndex, true, true);
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x0006D15C File Offset: 0x0006B35C
	private void OnDestroy()
	{
		if (this.m_buttonArray != null)
		{
			foreach (OmniUIButton omniUIButton in this.m_buttonArray)
			{
				omniUIButton.OnSelectEvent = (OmniUIButtonHandler)Delegate.Remove(omniUIButton.OnSelectEvent, new OmniUIButtonHandler(this.OnButtonSelected));
				omniUIButton.OnConfirmPressedRelay.RemoveListener(new Action<OmniUIButton>(this.PlayConfirmPressedSFX));
			}
		}
	}

	// Token: 0x04001D94 RID: 7572
	public static int StaticSelectedButtonIndex;

	// Token: 0x04001D95 RID: 7573
	[SerializeField]
	protected Image m_icon;

	// Token: 0x04001D96 RID: 7574
	[SerializeField]
	protected GameObject m_iconGO;

	// Token: 0x04001D97 RID: 7575
	[SerializeField]
	protected GameObject m_inactiveIconGO;

	// Token: 0x04001D98 RID: 7576
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04001D99 RID: 7577
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x04001D9A RID: 7578
	[SerializeField]
	protected Image m_newSymbol;

	// Token: 0x04001D9B RID: 7579
	protected IOmniUIWindowController m_windowController;

	// Token: 0x04001D9C RID: 7580
	protected OmniUIButton[] m_buttonArray;

	// Token: 0x04001D9D RID: 7581
	protected int m_localSelectedButtonIndex;

	// Token: 0x04001D9E RID: 7582
	private bool m_interactable;
}
