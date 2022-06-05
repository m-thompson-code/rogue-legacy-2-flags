using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000668 RID: 1640
public class SoulShopOmniUIToggleButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x06003205 RID: 12805 RVA: 0x0001B704 File Offset: 0x00019904
	// (set) Token: 0x06003206 RID: 12806 RVA: 0x0001B70C File Offset: 0x0001990C
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x06003207 RID: 12807 RVA: 0x0001B715 File Offset: 0x00019915
	// (set) Token: 0x06003208 RID: 12808 RVA: 0x0001B71D File Offset: 0x0001991D
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x17001359 RID: 4953
	// (get) Token: 0x06003209 RID: 12809 RVA: 0x0001B726 File Offset: 0x00019926
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x0001B72E File Offset: 0x0001992E
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Equipping);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Equipping);
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x000D561C File Offset: 0x000D381C
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (!soulShopObj.IsNativeNull() && soulShopObj.CurrentOwnedLevel > 0)
		{
			if (soulShopObj.CurrentEquippedLevel > 0)
			{
				soulShopObj.SetEquippedLevel(0, false, true);
			}
			else
			{
				soulShopObj.SetEquippedLevel(1, false, true);
			}
			this.UpdateState();
		}
		else
		{
			base.StartCoroutine(this.ShakeAnimCoroutine());
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000D5694 File Offset: 0x000D3894
	public override void UpdateState()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (soulShopObj.IsNativeNull() || soulShopObj.CurrentOwnedLevel <= 0 || !this.ParentEntry.IsToggle)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (soulShopObj.CurrentEquippedLevel > 0)
		{
			this.SetToggleOn(true);
		}
		else
		{
			this.SetToggleOn(false);
		}
		this.IsButtonActive = true;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x000D572C File Offset: 0x000D392C
	private void SetToggleOn(bool toggleOn)
	{
		if (toggleOn)
		{
			if (!this.m_sliderOnSprite.gameObject.activeSelf)
			{
				this.m_sliderOnSprite.gameObject.SetActive(true);
			}
			if (this.m_sliderOffSprite.gameObject.activeSelf)
			{
				this.m_sliderOffSprite.gameObject.SetActive(false);
			}
			Vector3 localPosition = this.m_notchGameObj.transform.localPosition;
			localPosition.x = 45f;
			this.m_notchGameObj.transform.localPosition = localPosition;
			return;
		}
		if (this.m_sliderOnSprite.gameObject.activeSelf)
		{
			this.m_sliderOnSprite.gameObject.SetActive(false);
		}
		if (!this.m_sliderOffSprite.gameObject.activeInHierarchy)
		{
			this.m_sliderOffSprite.gameObject.SetActive(true);
		}
		Vector3 localPosition2 = this.m_notchGameObj.transform.localPosition;
		localPosition2.x = 0f;
		this.m_notchGameObj.transform.localPosition = localPosition2;
	}

	// Token: 0x040028B7 RID: 10423
	[SerializeField]
	private Image m_sliderOnSprite;

	// Token: 0x040028B8 RID: 10424
	[SerializeField]
	private Image m_sliderOffSprite;

	// Token: 0x040028B9 RID: 10425
	[SerializeField]
	private GameObject m_notchGameObj;

	// Token: 0x040028BC RID: 10428
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x040028BD RID: 10429
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
