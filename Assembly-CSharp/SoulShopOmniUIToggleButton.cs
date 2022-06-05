using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003CF RID: 975
public class SoulShopOmniUIToggleButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000755E2 File Offset: 0x000737E2
	// (set) Token: 0x060023E8 RID: 9192 RVA: 0x000755EA File Offset: 0x000737EA
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x060023E9 RID: 9193 RVA: 0x000755F3 File Offset: 0x000737F3
	// (set) Token: 0x060023EA RID: 9194 RVA: 0x000755FB File Offset: 0x000737FB
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x060023EB RID: 9195 RVA: 0x00075604 File Offset: 0x00073804
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x0007560C File Offset: 0x0007380C
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Equipping);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Equipping);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x0007563C File Offset: 0x0007383C
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

	// Token: 0x060023EE RID: 9198 RVA: 0x000756B4 File Offset: 0x000738B4
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

	// Token: 0x060023EF RID: 9199 RVA: 0x0007574C File Offset: 0x0007394C
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

	// Token: 0x04001E79 RID: 7801
	[SerializeField]
	private Image m_sliderOnSprite;

	// Token: 0x04001E7A RID: 7802
	[SerializeField]
	private Image m_sliderOffSprite;

	// Token: 0x04001E7B RID: 7803
	[SerializeField]
	private GameObject m_notchGameObj;

	// Token: 0x04001E7E RID: 7806
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001E7F RID: 7807
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
