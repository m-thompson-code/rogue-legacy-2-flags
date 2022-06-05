using System;
using TMPro;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class EnchantressOmniUIEquipButton : OmniUIButton, IEnchantressOmniUIButton
{
	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x060030C5 RID: 12485 RVA: 0x0001AC48 File Offset: 0x00018E48
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x060030C6 RID: 12486 RVA: 0x0001AC50 File Offset: 0x00018E50
	// (set) Token: 0x060030C7 RID: 12487 RVA: 0x0001AC58 File Offset: 0x00018E58
	public RuneType RuneType { get; set; }

	// Token: 0x060030C8 RID: 12488 RVA: 0x000D1974 File Offset: 0x000CFB74
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

	// Token: 0x060030C9 RID: 12489 RVA: 0x000D19BC File Offset: 0x000CFBBC
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

	// Token: 0x060030CA RID: 12490 RVA: 0x000D1A44 File Offset: 0x000CFC44
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

	// Token: 0x040027F9 RID: 10233
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x040027FA RID: 10234
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x040027FB RID: 10235
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040027FC RID: 10236
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
