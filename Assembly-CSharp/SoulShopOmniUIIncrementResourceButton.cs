using System;
using TMPro;
using UnityEngine;

// Token: 0x02000665 RID: 1637
public class SoulShopOmniUIIncrementResourceButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17001353 RID: 4947
	// (get) Token: 0x060031F0 RID: 12784 RVA: 0x0001B66F File Offset: 0x0001986F
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001354 RID: 4948
	// (get) Token: 0x060031F1 RID: 12785 RVA: 0x0001B677 File Offset: 0x00019877
	// (set) Token: 0x060031F2 RID: 12786 RVA: 0x0001B67F File Offset: 0x0001987F
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17001355 RID: 4949
	// (get) Token: 0x060031F3 RID: 12787 RVA: 0x0001B688 File Offset: 0x00019888
	// (set) Token: 0x060031F4 RID: 12788 RVA: 0x0001B690 File Offset: 0x00019890
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060031F5 RID: 12789 RVA: 0x0001B699 File Offset: 0x00019899
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Equipping);
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000D4E50 File Offset: 0x000D3050
	public override void UpdateState()
	{
		if (this.m_soulSwap)
		{
			int soulSwapCost = Souls_EV.GetSoulSwapCost(SoulShopOmniUIIncrementResourceButton.SoulTransferLevel);
			int num = SoulShopOmniUIIncrementResourceButton.SoulTransferLevel * 150;
			if ((this.m_isDecrement && soulSwapCost <= 0) || (!this.m_isDecrement && SoulShopOmniUIIncrementResourceButton.SoulTransferLevel >= Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS.Length - 1))
			{
				this.m_deselectedSprite.SetAlpha(0.25f);
				this.IsButtonActive = false;
			}
			else
			{
				this.m_deselectedSprite.SetAlpha(1f);
				this.IsButtonActive = true;
			}
			string text = soulSwapCost.ToString();
			if (text.Length > 6)
			{
				text = Mathf.RoundToInt((float)soulSwapCost / 1000f).ToString() + "K";
			}
			string text2 = num.ToString();
			if (text2.Length > 6)
			{
				text2 = Mathf.RoundToInt((float)num / 1000f).ToString() + "K";
			}
			this.m_resourceText.text = string.Format("{0} : {1}", text, text2);
			return;
		}
		int num2 = SaveManager.PlayerSaveData.EquipmentOreCollected;
		int num3 = SoulShopOmniUIIncrementResourceButton.OreTransferAmount;
		if (this.m_aetherToOre)
		{
			num2 = SaveManager.PlayerSaveData.RuneOreCollected;
			num3 = SoulShopOmniUIIncrementResourceButton.AetherTransferAmount;
		}
		int num4 = Mathf.RoundToInt((float)(num3 / 2));
		if ((this.m_isDecrement && num3 <= 0) || (!this.m_isDecrement && num3 >= num2) || (!this.m_isDecrement && num2 - num3 < 500))
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
		}
		else
		{
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
		}
		string text3 = num3.ToString();
		if (text3.Length > 6)
		{
			text3 = Mathf.RoundToInt((float)num3 / 1000f).ToString() + "K";
		}
		string text4 = num4.ToString();
		if (text4.Length > 6)
		{
			text4 = Mathf.RoundToInt((float)num4 / 1000f).ToString() + "K";
		}
		this.m_resourceText.text = string.Format("{0} : {1}", text3, text4);
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000D5070 File Offset: 0x000D3270
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		if (!this.m_isDecrement)
		{
			if (this.m_soulSwap)
			{
				SoulShopOmniUIIncrementResourceButton.SoulTransferLevel++;
				if (SoulShopOmniUIIncrementResourceButton.SoulTransferLevel > Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS.Length - 1)
				{
					SoulShopOmniUIIncrementResourceButton.SoulTransferLevel = Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS.Length - 1;
				}
			}
			else if (!this.m_aetherToOre)
			{
				SoulShopOmniUIIncrementResourceButton.OreTransferAmount += 500;
			}
			else
			{
				SoulShopOmniUIIncrementResourceButton.AetherTransferAmount += 500;
			}
		}
		else if (this.m_soulSwap)
		{
			SoulShopOmniUIIncrementResourceButton.SoulTransferLevel--;
			if (SoulShopOmniUIIncrementResourceButton.SoulTransferLevel < 0)
			{
				SoulShopOmniUIIncrementResourceButton.SoulTransferLevel = 0;
			}
		}
		else if (!this.m_aetherToOre)
		{
			SoulShopOmniUIIncrementResourceButton.OreTransferAmount -= 500;
		}
		else
		{
			SoulShopOmniUIIncrementResourceButton.AetherTransferAmount -= 500;
		}
		this.RunOnConfirmPressedAnimation();
		this.ParentEntry.UpdateState();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
	}

	// Token: 0x040028AB RID: 10411
	public static int OreTransferAmount;

	// Token: 0x040028AC RID: 10412
	public static int AetherTransferAmount;

	// Token: 0x040028AD RID: 10413
	public static int SoulTransferLevel;

	// Token: 0x040028AE RID: 10414
	[SerializeField]
	private TMP_Text m_resourceText;

	// Token: 0x040028AF RID: 10415
	[SerializeField]
	private bool m_soulSwap;

	// Token: 0x040028B0 RID: 10416
	[SerializeField]
	[ConditionalHide("m_soulSwap", true, Inverse = true)]
	private bool m_aetherToOre;

	// Token: 0x040028B1 RID: 10417
	[SerializeField]
	private bool m_isDecrement;

	// Token: 0x040028B2 RID: 10418
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
