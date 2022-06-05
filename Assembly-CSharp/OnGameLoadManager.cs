using System;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x02000803 RID: 2051
public class OnGameLoadManager
{
	// Token: 0x060043F5 RID: 17397 RVA: 0x000F064C File Offset: 0x000EE84C
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

	// Token: 0x060043F6 RID: 17398 RVA: 0x000F0778 File Offset: 0x000EE978
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

	// Token: 0x060043F7 RID: 17399 RVA: 0x000F0864 File Offset: 0x000EEA64
	private static void OnFocusChanged(bool isFocused)
	{
		if (isFocused)
		{
			OnGameLoadManager.ConfineMouseToGameWindow(!SaveManager.ConfigData.DisableCursorConfine);
			return;
		}
		OnGameLoadManager.ConfineMouseToGameWindow(false);
	}

	// Token: 0x060043F8 RID: 17400 RVA: 0x000F0882 File Offset: 0x000EEA82
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

	// Token: 0x060043F9 RID: 17401 RVA: 0x000F08A8 File Offset: 0x000EEAA8
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
