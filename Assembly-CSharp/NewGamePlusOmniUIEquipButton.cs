using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000652 RID: 1618
public class NewGamePlusOmniUIEquipButton : OmniUIButton, INewGamePlusOmniUIButton
{
	// Token: 0x17001318 RID: 4888
	// (get) Token: 0x0600315C RID: 12636 RVA: 0x0001B070 File Offset: 0x00019270
	// (set) Token: 0x0600315D RID: 12637 RVA: 0x0001B1B4 File Offset: 0x000193B4
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
		set
		{
			NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel = value;
		}
	}

	// Token: 0x17001319 RID: 4889
	// (get) Token: 0x0600315E RID: 12638 RVA: 0x0001B1BC File Offset: 0x000193BC
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x1700131A RID: 4890
	// (get) Token: 0x0600315F RID: 12639 RVA: 0x0001B1C4 File Offset: 0x000193C4
	// (set) Token: 0x06003160 RID: 12640 RVA: 0x0001B1CC File Offset: 0x000193CC
	public BurdenType BurdenType { get; set; }

	// Token: 0x06003161 RID: 12641 RVA: 0x0001B1D5 File Offset: 0x000193D5
	public override void OnSelect(BaseEventData eventData)
	{
		if (this.BurdenType != BurdenType.None || (WindowManager.GetWindowController(WindowID.NewGamePlusNPC) as NewGamePlusOmniUIWindowController).InTimelineEntry)
		{
			base.OnSelect(eventData);
		}
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000D350C File Offset: 0x000D170C
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = this.m_isRightArrow ? OmniUIButtonType.Equipping : OmniUIButtonType.Unequipping;
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new NewGamePlusOmniUIDescriptionEventArgs(this.BurdenType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.BurdenType, buttonType);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000D3554 File Offset: 0x000D1754
	public override void UpdateState()
	{
		if (this.BurdenType == BurdenType.None)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.IsButtonActive = true;
			this.m_levelText.text = this.CurrentNewGamePlusLevel.ToString();
			return;
		}
		BurdenObj burden = BurdenManager.GetBurden(this.BurdenType);
		if (burden.IsNativeNull())
		{
			return;
		}
		int currentLevel = burden.CurrentLevel;
		int maxLevel = burden.MaxLevel;
		if (maxLevel > 0)
		{
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
		}
		else
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
		}
		this.m_levelText.text = currentLevel.ToString() + "/" + maxLevel.ToString();
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x0001B1F9 File Offset: 0x000193F9
	public override void OnConfirmButtonPressed()
	{
		if (this.BurdenType == BurdenType.None)
		{
			this.ChangeNGPlusLevel();
		}
		else
		{
			this.ChangeBurdenLevel();
		}
		this.RunOnConfirmPressedAnimation();
		base.OnConfirmButtonPressed();
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x000D3620 File Offset: 0x000D1820
	private void ChangeBurdenLevel()
	{
		BurdenObj burden = BurdenManager.GetBurden(this.BurdenType);
		if (!burden.IsNativeNull())
		{
			if (this.m_isRightArrow)
			{
				if (burden.CurrentLevel < burden.MaxLevel)
				{
					BurdenManager.SetBurdenLevel(this.BurdenType, 1, true, true);
					this.InitializeButtonEventArgs();
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
					return;
				}
				base.StartCoroutine(this.ShakeAnimCoroutine());
				return;
			}
			else
			{
				if (burden.CurrentLevel > 0)
				{
					if (burden.CurrentLevel >= burden.MaxLevel)
					{
						BurdenManager.SetBurdenLevel(this.BurdenType, burden.CurrentLevel - 1, false, true);
					}
					else
					{
						BurdenManager.SetBurdenLevel(this.BurdenType, -1, true, true);
					}
					this.InitializeButtonEventArgs();
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
					return;
				}
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x000D36FC File Offset: 0x000D18FC
	private void ChangeNGPlusLevel()
	{
		int currentNewGamePlusLevel = this.CurrentNewGamePlusLevel;
		if (this.m_isRightArrow && this.CurrentNewGamePlusLevel < 100 && this.CurrentNewGamePlusLevel < SaveManager.PlayerSaveData.HighestNGPlusBeaten + 1)
		{
			int currentNewGamePlusLevel2 = this.CurrentNewGamePlusLevel;
			this.CurrentNewGamePlusLevel = currentNewGamePlusLevel2 + 1;
		}
		else if (!this.m_isRightArrow && this.CurrentNewGamePlusLevel > 0)
		{
			int currentNewGamePlusLevel2 = this.CurrentNewGamePlusLevel;
			this.CurrentNewGamePlusLevel = currentNewGamePlusLevel2 - 1;
		}
		this.CurrentNewGamePlusLevel = Mathf.Clamp(this.CurrentNewGamePlusLevel, 0, 100);
		if (currentNewGamePlusLevel != this.CurrentNewGamePlusLevel)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			return;
		}
		base.StartCoroutine(this.ShakeAnimCoroutine());
	}

	// Token: 0x0400283E RID: 10302
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x0400283F RID: 10303
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x04002840 RID: 10304
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04002841 RID: 10305
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
