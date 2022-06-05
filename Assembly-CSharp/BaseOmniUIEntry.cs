using System;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000620 RID: 1568
public abstract class BaseOmniUIEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
{
	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x06003027 RID: 12327
	public abstract bool IsEntryActive { get; }

	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x06003028 RID: 12328
	public abstract EventArgs EntryEventArgs { get; }

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x06003029 RID: 12329 RVA: 0x0001A69B File Offset: 0x0001889B
	// (set) Token: 0x0600302A RID: 12330 RVA: 0x000CD92C File Offset: 0x000CBB2C
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

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x0600302B RID: 12331 RVA: 0x000CD968 File Offset: 0x000CBB68
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

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x0600302C RID: 12332 RVA: 0x0001A6A3 File Offset: 0x000188A3
	// (set) Token: 0x0600302D RID: 12333 RVA: 0x0001A6AB File Offset: 0x000188AB
	public int EntryIndex { get; protected set; }

	// Token: 0x0600302E RID: 12334 RVA: 0x000CD9B0 File Offset: 0x000CBBB0
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

	// Token: 0x0600302F RID: 12335 RVA: 0x000CDA00 File Offset: 0x000CBC00
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

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x06003030 RID: 12336 RVA: 0x0001A6B4 File Offset: 0x000188B4
	public IOmniUIWindowController WindowController
	{
		get
		{
			return this.m_windowController;
		}
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000CDA50 File Offset: 0x000CBC50
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

	// Token: 0x06003032 RID: 12338 RVA: 0x0001A6BC File Offset: 0x000188BC
	public virtual void SetEntryIndex(int index)
	{
		this.EntryIndex = index;
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x0001A6C5 File Offset: 0x000188C5
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

	// Token: 0x06003034 RID: 12340
	public abstract void UpdateActive();

	// Token: 0x06003035 RID: 12341 RVA: 0x000CDADC File Offset: 0x000CBCDC
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

	// Token: 0x06003036 RID: 12342 RVA: 0x000CDCBC File Offset: 0x000CBEBC
	public virtual void DeselectAllButtons()
	{
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			buttonArray[i].OnDeselect(null);
		}
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x000CDCE8 File Offset: 0x000CBEE8
	public void SelectRightMostButton()
	{
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.SelectButton(true);
		}
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x000CDD10 File Offset: 0x000CBF10
	public void SelectLeftMostButton()
	{
		for (int i = 0; i < this.m_buttonArray.Length; i++)
		{
			this.SelectButton(false);
		}
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x000CDD38 File Offset: 0x000CBF38
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

	// Token: 0x0600303A RID: 12346 RVA: 0x000CDE54 File Offset: 0x000CC054
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

	// Token: 0x0600303B RID: 12347 RVA: 0x000CDF68 File Offset: 0x000CC168
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

	// Token: 0x0600303C RID: 12348 RVA: 0x0001A704 File Offset: 0x00018904
	private void PlayConfirmPressedSFX(OmniUIButton button)
	{
		if (this.m_windowController.SelectOptionEvent != null)
		{
			this.m_windowController.SelectOptionEvent.Invoke();
		}
	}

	// Token: 0x0600303D RID: 12349 RVA: 0x0001A723 File Offset: 0x00018923
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

	// Token: 0x0600303E RID: 12350 RVA: 0x0001A748 File Offset: 0x00018948
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

	// Token: 0x0600303F RID: 12351 RVA: 0x000CDFC0 File Offset: 0x000CC1C0
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

	// Token: 0x0400278F RID: 10127
	public static int StaticSelectedButtonIndex;

	// Token: 0x04002790 RID: 10128
	[SerializeField]
	protected Image m_icon;

	// Token: 0x04002791 RID: 10129
	[SerializeField]
	protected GameObject m_iconGO;

	// Token: 0x04002792 RID: 10130
	[SerializeField]
	protected GameObject m_inactiveIconGO;

	// Token: 0x04002793 RID: 10131
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04002794 RID: 10132
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x04002795 RID: 10133
	[SerializeField]
	protected Image m_newSymbol;

	// Token: 0x04002796 RID: 10134
	protected IOmniUIWindowController m_windowController;

	// Token: 0x04002797 RID: 10135
	protected OmniUIButton[] m_buttonArray;

	// Token: 0x04002798 RID: 10136
	protected int m_localSelectedButtonIndex;

	// Token: 0x04002799 RID: 10137
	private bool m_interactable;
}
