using System;
using TMPro;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class SoulShopOmniUIIncrementResourceButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x060023D2 RID: 9170 RVA: 0x00074D66 File Offset: 0x00072F66
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x060023D3 RID: 9171 RVA: 0x00074D6E File Offset: 0x00072F6E
	// (set) Token: 0x060023D4 RID: 9172 RVA: 0x00074D76 File Offset: 0x00072F76
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x060023D5 RID: 9173 RVA: 0x00074D7F File Offset: 0x00072F7F
	// (set) Token: 0x060023D6 RID: 9174 RVA: 0x00074D87 File Offset: 0x00072F87
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023D7 RID: 9175 RVA: 0x00074D90 File Offset: 0x00072F90
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Equipping);
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x00074DC0 File Offset: 0x00072FC0
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

	// Token: 0x060023D9 RID: 9177 RVA: 0x00074FE0 File Offset: 0x000731E0
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

	// Token: 0x04001E6D RID: 7789
	public static int OreTransferAmount;

	// Token: 0x04001E6E RID: 7790
	public static int AetherTransferAmount;

	// Token: 0x04001E6F RID: 7791
	public static int SoulTransferLevel;

	// Token: 0x04001E70 RID: 7792
	[SerializeField]
	private TMP_Text m_resourceText;

	// Token: 0x04001E71 RID: 7793
	[SerializeField]
	private bool m_soulSwap;

	// Token: 0x04001E72 RID: 7794
	[SerializeField]
	[ConditionalHide("m_soulSwap", true, Inverse = true)]
	private bool m_aetherToOre;

	// Token: 0x04001E73 RID: 7795
	[SerializeField]
	private bool m_isDecrement;

	// Token: 0x04001E74 RID: 7796
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
