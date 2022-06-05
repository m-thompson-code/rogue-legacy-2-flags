using System;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class ChangeDisplayModeOptionItem : SelectionListOptionItem
{
	// Token: 0x06001939 RID: 6457 RVA: 0x0004F168 File Offset: 0x0004D368
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

	// Token: 0x0600193A RID: 6458 RVA: 0x0004F214 File Offset: 0x0004D414
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

	// Token: 0x0600193B RID: 6459 RVA: 0x0004F267 File Offset: 0x0004D467
	public override void InvokeValueChange()
	{
		Debug.Log("Change screen mod to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x0004F280 File Offset: 0x0004D480
	public override void ConfirmOptionChange()
	{
		GameResolutionManager.SetResolution(GameResolutionManager.Resolution.x, GameResolutionManager.Resolution.y, this.m_screenModeArray[this.m_selectedIndex], true);
	}

	// Token: 0x04001839 RID: 6201
	private FullScreenMode[] m_screenModeArray;
}
