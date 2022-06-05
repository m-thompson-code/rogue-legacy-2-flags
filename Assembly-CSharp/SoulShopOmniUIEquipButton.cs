using System;
using TMPro;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class SoulShopOmniUIEquipButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x060023C9 RID: 9161 RVA: 0x00074BCC File Offset: 0x00072DCC
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x060023CA RID: 9162 RVA: 0x00074BD4 File Offset: 0x00072DD4
	// (set) Token: 0x060023CB RID: 9163 RVA: 0x00074BDC File Offset: 0x00072DDC
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x060023CC RID: 9164 RVA: 0x00074BE5 File Offset: 0x00072DE5
	// (set) Token: 0x060023CD RID: 9165 RVA: 0x00074BED File Offset: 0x00072DED
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023CE RID: 9166 RVA: 0x00074BF8 File Offset: 0x00072DF8
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

	// Token: 0x060023CF RID: 9167 RVA: 0x00074C40 File Offset: 0x00072E40
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

	// Token: 0x060023D0 RID: 9168 RVA: 0x00074CA0 File Offset: 0x00072EA0
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

	// Token: 0x04001E67 RID: 7783
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04001E68 RID: 7784
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x04001E69 RID: 7785
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001E6A RID: 7786
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
