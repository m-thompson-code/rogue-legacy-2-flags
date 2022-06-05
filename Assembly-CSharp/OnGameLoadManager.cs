using System;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x02000CCB RID: 3275
public class OnGameLoadManager
{
	// Token: 0x06005D7E RID: 23934 RVA: 0x0015E354 File Offset: 0x0015C554
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void Run()
	{
		LanguageType languageType = OnGameLoadManager.GetLanguageType(CultureInfo.CurrentCulture);
		Debug.Log("DEFAULT LANGUAGE: " + languageType.ToString());
		CultureInfo cultureInfo = new CultureInfo("en-US", false);
		cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
		CultureInfo.CurrentCulture = cultureInfo;
		SaveManager.SetCultureInfo(cultureInfo);
		SaveManager.ConfigData.Language = languageType;
		SaveFileSystem.Initialize();
		SaveFileSystem.MountSaveDirectory();
		Time.fixedDeltaTime = 0.016666668f;
		Time.maximumDeltaTime = 0.033333335f;
		Application.targetFrameRate = 120;
		if (File.Exists(SaveManager.GetConfigPath()))
		{
			SaveManager.LoadConfigFile();
		}
		if (Application.isEditor)
		{
			GameResolutionManager.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed, false);
			return;
		}
		OnGameLoadManager.SetScreenModeAndResolution();
		GameResolutionManager.SetVsyncEnable(SaveManager.ConfigData.EnableVsync);
		Application.targetFrameRate = SaveManager.ConfigData.FPSLimit;
		Debug.Log("<color=yellow>Setting target FPS to " + Application.targetFrameRate.ToString() + ".</color>");
		Application.focusChanged -= OnGameLoadManager.OnFocusChanged;
		Application.focusChanged += OnGameLoadManager.OnFocusChanged;
		OnGameLoadManager.ConfineMouseToGameWindow(!SaveManager.ConfigData.DisableCursorConfine);
	}

	// Token: 0x06005D7F RID: 23935 RVA: 0x0015E480 File Offset: 0x0015C680
	private static LanguageType GetLanguageType(CultureInfo cultureInfo)
	{
		if (cultureInfo.TwoLetterISOLanguageName == "en")
		{
			return LanguageType.English;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "fr")
		{
			return LanguageType.French;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "de")
		{
			return LanguageType.German;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "ru")
		{
			return LanguageType.Russian;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "es")
		{
			return LanguageType.Spanish;
		}
		if (cultureInfo.Name == "pt-BR")
		{
			return LanguageType.Portuguese_BR;
		}
		if (cultureInfo.Name == "zh-Hans")
		{
			return LanguageType.Chinese_Simp;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "ko")
		{
			return LanguageType.Korean;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "tr")
		{
			return LanguageType.Turkish;
		}
		if (cultureInfo.Name == "zh-Hant")
		{
			return LanguageType.Chinese_Trad;
		}
		if (cultureInfo.TwoLetterISOLanguageName == "it")
		{
			return LanguageType.Italian;
		}
		return LanguageType.English;
	}

	// Token: 0x06005D80 RID: 23936 RVA: 0x000336FE File Offset: 0x000318FE
	private static void OnFocusChanged(bool isFocused)
	{
		if (isFocused)
		{
			OnGameLoadManager.ConfineMouseToGameWindow(!SaveManager.ConfigData.DisableCursorConfine);
			return;
		}
		OnGameLoadManager.ConfineMouseToGameWindow(false);
	}

	// Token: 0x06005D81 RID: 23937 RVA: 0x0003371C File Offset: 0x0003191C
	public static void ConfineMouseToGameWindow(bool isConfined)
	{
		if (GameManager.IsGameManagerInstantiated && GameManager.IsGamePaused)
		{
			isConfined = false;
		}
		if (isConfined)
		{
			Cursor.lockState = CursorLockMode.Confined;
			return;
		}
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x06005D82 RID: 23938 RVA: 0x0015E56C File Offset: 0x0015C76C
	private static void SetScreenModeAndResolution()
	{
		int screenWidth = SaveManager.ConfigData.ScreenWidth;
		int screenHeight = SaveManager.ConfigData.ScreenHeight;
		bool flag = false;
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].width == screenWidth && resolutions[i].height == screenHeight)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Resolution currentResolution = Screen.currentResolution;
			Debug.LogFormat("Resolution Config is requesting an unsupported resolution {0}x{1}, forcing full-res: {2}x{3}", new object[]
			{
				SaveManager.ConfigData.ScreenWidth,
				SaveManager.ConfigData.ScreenHeight,
				currentResolution.width,
				currentResolution.height
			});
			SaveManager.ConfigData.ScreenWidth = currentResolution.width;
			SaveManager.ConfigData.ScreenHeight = currentResolution.height;
			screenWidth = SaveManager.ConfigData.ScreenWidth;
			screenHeight = SaveManager.ConfigData.ScreenHeight;
		}
		FullScreenMode screenMode = (FullScreenMode)SaveManager.ConfigData.ScreenMode;
		GameResolutionManager.SetResolution(screenWidth, screenHeight, screenMode, false);
	}
}
