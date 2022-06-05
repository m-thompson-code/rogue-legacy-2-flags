using System;
using TMPro;
using UnityEngine;

// Token: 0x02000664 RID: 1636
public class SoulShopOmniUIEquipButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17001350 RID: 4944
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x0001B645 File Offset: 0x00019845
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001351 RID: 4945
	// (get) Token: 0x060031E8 RID: 12776 RVA: 0x0001B64D File Offset: 0x0001984D
	// (set) Token: 0x060031E9 RID: 12777 RVA: 0x0001B655 File Offset: 0x00019855
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17001352 RID: 4946
	// (get) Token: 0x060031EA RID: 12778 RVA: 0x0001B65E File Offset: 0x0001985E
	// (set) Token: 0x060031EB RID: 12779 RVA: 0x0001B666 File Offset: 0x00019866
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060031EC RID: 12780 RVA: 0x000D4CE8 File Offset: 0x000D2EE8
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = this.m_isRightArrow ? OmniUIButtonType.Equipping : OmniUIButtonType.Unequipping;
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, buttonType);
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x000D4D30 File Offset: 0x000D2F30
	public override void UpdateState()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (soulShopObj.IsNativeNull())
		{
			return;
		}
		int currentEquippedLevel = soulShopObj.CurrentEquippedLevel;
		int currentOwnedLevel = soulShopObj.CurrentOwnedLevel;
		this.IsButtonActive = true;
		this.m_levelText.text = currentEquippedLevel.ToString() + "/" + currentOwnedLevel.ToString();
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x000D4D90 File Offset: 0x000D2F90
	public override void OnConfirmButtonPressed()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (soulShopObj.IsNativeNull())
		{
			return;
		}
		if (this.m_isRightArrow)
		{
			if (soulShopObj.CurrentEquippedLevel < soulShopObj.CurrentOwnedLevel)
			{
				soulShopObj.SetEquippedLevel(1, true, true);
				this.InitializeButtonEventArgs();
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			}
			else
			{
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
		else if (soulShopObj.CurrentEquippedLevel > 0)
		{
			soulShopObj.SetEquippedLevel(-1, true, true);
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		else
		{
			base.StartCoroutine(this.ShakeAnimCoroutine());
		}
		this.RunOnConfirmPressedAnimation();
		base.OnConfirmButtonPressed();
	}

	// Token: 0x040028A5 RID: 10405
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x040028A6 RID: 10406
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x040028A7 RID: 10407
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040028A8 RID: 10408
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
