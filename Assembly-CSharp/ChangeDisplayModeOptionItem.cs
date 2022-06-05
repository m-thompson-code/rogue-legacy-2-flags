using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class ChangeDisplayModeOptionItem : SelectionListOptionItem
{
	// Token: 0x06002328 RID: 9000 RVA: 0x000AC828 File Offset: 0x000AAA28
	public override void Initialize()
	{
		FullScreenMode[] array = Enum.GetValues(typeof(FullScreenMode)) as FullScreenMode[];
		this.m_screenModeArray = new FullScreenMode[array.Length - 1];
		this.m_selectionLocIDArray = new string[array.Length - 1];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != FullScreenMode.MaximizedWindow)
			{
				string text = null;
				switch (array[i])
				{
				case FullScreenMode.ExclusiveFullScreen:
					text = "LOC_ID_GRAPHICS_SETTING_FULLSCREEN_1";
					break;
				case FullScreenMode.FullScreenWindow:
					text = "LOC_ID_GRAPHICS_SETTING_FULLSCREEN_BORDERLESS_1";
					break;
				case FullScreenMode.Windowed:
					text = "LOC_ID_GRAPHICS_SETTING_WINDOW_1";
					break;
				}
				this.m_selectionLocIDArray[num] = text;
				this.m_screenModeArray[num] = array[i];
				num++;
			}
		}
		base.Initialize();
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000AC8D4 File Offset: 0x000AAAD4
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = this.m_screenModeArray.IndexOf(Screen.fullScreenMode);
			if (this.m_selectedIndex == -1)
			{
				this.m_selectedIndex = 0;
			}
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x00012E7B File Offset: 0x0001107B
	public override void InvokeValueChange()
	{
		Debug.Log("Change screen mod to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000AC928 File Offset: 0x000AAB28
	public override void ConfirmOptionChange()
	{
		GameResolutionManager.SetResolution(GameResolutionManager.Resolution.x, GameResolutionManager.Resolution.y, this.m_screenModeArray[this.m_selectedIndex], true);
	}

	// Token: 0x04001F86 RID: 8070
	private FullScreenMode[] m_screenModeArray;
}
