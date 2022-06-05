using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020003BB RID: 955
public class NewGamePlusOmniUIEquipButton : OmniUIButton, INewGamePlusOmniUIButton
{
	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06002344 RID: 9028 RVA: 0x0007306E File Offset: 0x0007126E
	// (set) Token: 0x06002345 RID: 9029 RVA: 0x00073075 File Offset: 0x00071275
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

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06002346 RID: 9030 RVA: 0x0007307D File Offset: 0x0007127D
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x06002347 RID: 9031 RVA: 0x00073085 File Offset: 0x00071285
	// (set) Token: 0x06002348 RID: 9032 RVA: 0x0007308D File Offset: 0x0007128D
	public BurdenType BurdenType { get; set; }

	// Token: 0x06002349 RID: 9033 RVA: 0x00073096 File Offset: 0x00071296
	public override void OnSelect(BaseEventData eventData)
	{
		if (this.BurdenType != BurdenType.None || (WindowManager.GetWindowController(WindowID.NewGamePlusNPC) as NewGamePlusOmniUIWindowController).InTimelineEntry)
		{
			base.OnSelect(eventData);
		}
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000730BC File Offset: 0x000712BC
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

	// Token: 0x0600234B RID: 9035 RVA: 0x00073104 File Offset: 0x00071304
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

	// Token: 0x0600234C RID: 9036 RVA: 0x000731CD File Offset: 0x000713CD
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

	// Token: 0x0600234D RID: 9037 RVA: 0x000731F4 File Offset: 0x000713F4
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

	// Token: 0x0600234E RID: 9038 RVA: 0x000732D0 File Offset: 0x000714D0
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

	// Token: 0x04001E0C RID: 7692
	[SerializeField]
	private bool m_isRightArrow;

	// Token: 0x04001E0D RID: 7693
	[SerializeField]
	private GameObject m_buttonGO;

	// Token: 0x04001E0E RID: 7694
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001E0F RID: 7695
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
