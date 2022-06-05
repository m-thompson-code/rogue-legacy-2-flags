using System;
using TMPro;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class EnchantressOmniUIEquipButton : OmniUIButton, IEnchantressOmniUIButton
{
	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x060022AD RID: 8877 RVA: 0x00070F07 File Offset: 0x0006F107
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x060022AE RID: 8878 RVA: 0x00070F0F File Offset: 0x0006F10F
	// (set) Token: 0x060022AF RID: 8879 RVA: 0x00070F17 File Offset: 0x0006F117
	public RuneType RuneType { get; set; }

	// Token: 0x060022B0 RID: 8880 RVA: 0x00070F20 File Offset: 0x0006F120
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = this.m_isRightArrow ? OmniUIButtonType.Equipping : OmniUIButtonType.Unequipping;
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new EnchantressOmniUIDescriptionEventArgs(this.RuneType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.RuneType, buttonType);
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x00070F68 File Offset: 0x0006F168
	public override void UpdateState()
	{
		RuneManager.GetRune(this.RuneType);
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(this.RuneType);
		int upgradeLevel = RuneManager.GetUpgradeLevel(this.RuneType);
		if (upgradeLevel > 0)
		{
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
		}
		else
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
		}
		this.m_levelText.text = runeEquippedLevel.ToString() + "/" + upgradeLevel.ToString();
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x00070FF0 File Offset: 0x0006F1F0
	public override void OnConfirmButtonPressed()
	{
		if (this.m_isRightArrow)
		{
			if (RuneManager.CanEquip(this.RuneType, true))
			{
				if (RuneManager.SetRuneEquippedLevel(this.RuneType, 1, true, true))
				{
					this.InitializeButtonEventArgs();
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
				}
			}
			else
			{
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
		else if (RuneManager.SetRuneEquippedLevel(this.RuneType, -1, true, true))
		{
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

	// Token: 0x04001DD9 RID: 7641
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04001DDA RID: 7642
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x04001DDB RID: 7643
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001DDC RID: 7644
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
