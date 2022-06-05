using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x0200069F RID: 1695
public class LocalizationManager : MonoBehaviour
{
	// Token: 0x17001553 RID: 5459
	// (get) Token: 0x06003DEC RID: 15852 RVA: 0x000D87CB File Offset: 0x000D69CB
	public static LanguageType[] AvailableLanguages
	{
		get
		{
			return LocalizationManager.Instance.m_availableLanguages;
		}
	}

	// Token: 0x17001554 RID: 5460
	// (get) Token: 0x06003DED RID: 15853 RVA: 0x000D87D7 File Offset: 0x000D69D7
	public static int NumberOfLanguages
	{
		get
		{
			return LocalizationManager.Instance.m_availableLanguages.Length;
		}
	}

	// Token: 0x17001555 RID: 5461
	// (get) Token: 0x06003DEE RID: 15854 RVA: 0x000D87E5 File Offset: 0x000D69E5
	public static int MaleDictSize
	{
		get
		{
			return LocalizationManager.Instance.m_maleLocDict.Count;
		}
	}

	// Token: 0x17001556 RID: 5462
	// (get) Token: 0x06003DEF RID: 15855 RVA: 0x000D87F6 File Offset: 0x000D69F6
	public static int FemaleDictSize
	{
		get
		{
			return LocalizationManager.Instance.m_femaleLocDict.Count;
		}
	}

	// Token: 0x17001557 RID: 5463
	// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x000D8807 File Offset: 0x000D6A07
	public static bool IsInitialized
	{
		get
		{
			return LocalizationManager.m_isInitialized;
		}
	}

	// Token: 0x17001558 RID: 5464
	// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x000D880E File Offset: 0x000D6A0E
	public static string[] MaleNameArray
	{
		get
		{
			return LocalizationManager.Instance.m_maleNameArray;
		}
	}

	// Token: 0x17001559 RID: 5465
	// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x000D881A File Offset: 0x000D6A1A
	public static string[] FemaleNameArray
	{
		get
		{
			return LocalizationManager.Instance.m_femaleNameArray;
		}
	}

	// Token: 0x1700155A RID: 5466
	// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x000D8826 File Offset: 0x000D6A26
	public static LanguageType CurrentLanguageType
	{
		get
		{
			return LocalizationManager.Instance.m_currentLanguageType;
		}
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x000D8834 File Offset: 0x000D6A34
	public static string GetLanguageLocID(LanguageType languageType)
	{
		string result;
		if (LocalizationManager.Instance.m_languageLocStringDict.TryGetValue(languageType, out result))
		{
			return result;
		}
		return LocalizationManager.Instance.m_languageLocStringDict[LanguageType.English];
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x000D8867 File Offset: 0x000D6A67
	public static CultureInfo GetCurrentCultureInfo()
	{
		return LocalizationManager.GetCultureInfo(LocalizationManager.CurrentLanguageType);
	}

	// Token: 0x06003DF6 RID: 15862 RVA: 0x000D8874 File Offset: 0x000D6A74
	public static CultureInfo GetCultureInfo(LanguageType languageType)
	{
		CultureInfo result;
		if (LocalizationManager.Instance.m_cultureInfoDict.TryGetValue(languageType, out result))
		{
			return result;
		}
		return SaveManager.CultureInfo;
	}

	// Token: 0x1700155B RID: 5467
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x000D889C File Offset: 0x000D6A9C
	public static LocalizationManager Instance
	{
		get
		{
			if (!LocalizationManager.m_manager)
			{
				LocalizationManager.m_manager = CDGHelper.FindStaticInstance<LocalizationManager>(false);
			}
			return LocalizationManager.m_manager;
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x000D88BC File Offset: 0x000D6ABC
	private void Awake()
	{
		if (Application.isPlaying && !LocalizationManager.IsInitialized)
		{
			this.m_availableLanguages = new LanguageType[this.m_languageDict.Count];
			Dictionary<LanguageType, TextAsset>.KeyCollection keys = this.m_languageDict.Keys;
			int num = 0;
			foreach (LanguageType languageType in keys)
			{
				this.m_availableLanguages[num] = languageType;
				num++;
			}
			LocalizationManager.LoadLanguage(SaveManager.ConfigData.Language, false);
			this.LoadNames();
			this.InitializeCultureInfo();
			GlobalTextChangedStaticListener.Initialize();
			this.m_langChangedEventArgs = new LanguageChangedEventArgs(LocalizationManager.CurrentLanguageType);
			LocalizationManager.m_isInitialized = true;
		}
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x000D8980 File Offset: 0x000D6B80
	private void InitializeCultureInfo()
	{
		this.m_cultureInfoDict = new Dictionary<LanguageType, CultureInfo>();
		this.m_cultureInfoDict.Add(LanguageType.English, CultureInfo.GetCultureInfo("en-US"));
		this.m_cultureInfoDict.Add(LanguageType.French, CultureInfo.GetCultureInfo("fr-FR"));
		this.m_cultureInfoDict.Add(LanguageType.German, CultureInfo.GetCultureInfo("de-DE"));
		this.m_cultureInfoDict.Add(LanguageType.Spanish, CultureInfo.GetCultureInfo("es-ES"));
		this.m_cultureInfoDict.Add(LanguageType.Italian, CultureInfo.GetCultureInfo("it-IT"));
		this.m_cultureInfoDict.Add(LanguageType.Portuguese_BR, CultureInfo.GetCultureInfo("pt-BR"));
		this.m_cultureInfoDict.Add(LanguageType.Turkish, CultureInfo.GetCultureInfo("tr-TR"));
		this.m_cultureInfoDict.Add(LanguageType.Russian, CultureInfo.GetCultureInfo("ru-RU"));
		this.m_cultureInfoDict.Add(LanguageType.Korean, CultureInfo.GetCultureInfo("ko-KR"));
		this.m_cultureInfoDict.Add(LanguageType.Chinese_Simp, CultureInfo.GetCultureInfo("zh-CN"));
		this.m_cultureInfoDict.Add(LanguageType.Chinese_Trad, CultureInfo.GetCultureInfo("zh-TW"));
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x000D8A8C File Offset: 0x000D6C8C
	private void ChangeLanguage_Internal(LanguageType languageType, bool broadcastEvent)
	{
		if (LocalizationManager.CurrentLanguageType != languageType)
		{
			LocalizationManager.LoadLanguage(languageType, broadcastEvent);
		}
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x000D8A9D File Offset: 0x000D6C9D
	private bool IsValidName(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && !name.Contains('\\') && (name.Length < 2 || name[0] != '/' || name[1] != '/');
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x000D8AD8 File Offset: 0x000D6CD8
	private void LoadOrCreateCustomNames(bool male)
	{
		string text = male ? "male" : "female";
		Debug.Log("Checking for custom " + text + " names");
		string text2 = Path.Combine(Application.persistentDataPath, "CustomData");
		if (!Directory.Exists(text2))
		{
			try
			{
				Directory.CreateDirectory(text2);
				Debug.Log("<color=green>Created CustomData directory</color>");
			}
			catch (Exception ex)
			{
				string str = "<color=red>CustomData directory does not exist and could not be created.</color> ";
				Exception ex2 = ex;
				Debug.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
		}
		text2 = Path.Combine(text2, StoreAPIManager.GetPlatformDirectoryName());
		if (!Directory.Exists(text2))
		{
			try
			{
				Directory.CreateDirectory(text2);
				Debug.Log("<color=green>Created CustomData/Platform subdirectory</color>");
			}
			catch (Exception ex3)
			{
				string str2 = "<color=red>CustomData/Platform subdirectory does not exist and could not be created.</color> ";
				Exception ex4 = ex3;
				Debug.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
				return;
			}
		}
		string path = Path.Combine(text2, male ? "MaleNameList.txt" : "FemaleNameList.txt");
		if (File.Exists(path))
		{
			try
			{
				string[] array = File.ReadAllLines(path);
				List<string> list = new List<string>();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text3 = array2[i].Trim();
					if (this.IsValidName(text3) && !list.Contains(text3))
					{
						list.Add(text3);
					}
				}
				if (list.Count < 5)
				{
					return;
				}
				if (male)
				{
					this.m_maleNameArray = list.ToArray();
					LocalizationManager.m_usingMaleNameOverrideList = true;
				}
				else
				{
					this.m_femaleNameArray = list.ToArray();
					LocalizationManager.m_usingFemaleNameOverrideList = true;
				}
				Debug.Log("Loaded custom " + text + " heir names");
				return;
			}
			catch (Exception ex5)
			{
				string str3 = "<color=red>Failed to load ";
				string str4 = text;
				string str5 = " heir names from file.</color> ";
				Exception ex6 = ex5;
				Debug.Log(str3 + str4 + str5 + ((ex6 != null) ? ex6.ToString() : null));
				return;
			}
		}
		try
		{
			using (StreamWriter streamWriter = File.CreateText(path))
			{
				streamWriter.WriteLine("// Add your own " + text + " heir names in Rogue Legacy 2 here.");
				for (int j = 0; j < LocalizationManager.NAME_FILE_INSTRUCTIONS.Length; j++)
				{
					streamWriter.WriteLine(LocalizationManager.NAME_FILE_INSTRUCTIONS[j]);
				}
			}
			Debug.Log("<color=green>Successfully created custom " + text + " heir name file</color>");
		}
		catch (Exception ex7)
		{
			string str6 = "<color=red>Failed to create custom ";
			string str7 = text;
			string str8 = " heir name file.</color> ";
			Exception ex8 = ex7;
			Debug.Log(str6 + str7 + str8 + ((ex8 != null) ? ex8.ToString() : null));
		}
	}

	// Token: 0x06003DFD RID: 15869 RVA: 0x000D8D60 File Offset: 0x000D6F60
	private void LoadNames()
	{
		this.LoadOrCreateCustomNames(true);
		this.LoadOrCreateCustomNames(false);
	}

	// Token: 0x06003DFE RID: 15870 RVA: 0x000D8D70 File Offset: 0x000D6F70
	public static void LoadLanguage(LanguageType languageType, bool broadcastEvent)
	{
		if (LocalizationManager.Instance.m_maleLocDict == null)
		{
			LocalizationManager.Instance.m_maleLocDict = new Dictionary<string, string>();
		}
		else
		{
			LocalizationManager.Instance.m_maleLocDict.Clear();
		}
		if (LocalizationManager.Instance.m_femaleLocDict == null)
		{
			LocalizationManager.Instance.m_femaleLocDict = new Dictionary<string, string>();
		}
		else
		{
			LocalizationManager.Instance.m_femaleLocDict.Clear();
		}
		MonoBehaviour.print("Loading Rogue Legacy 2: " + System_EV.GetVersionString());
		MonoBehaviour.print("Loading language: " + languageType.ToString());
		TextAsset textAsset = null;
		if (!LocalizationManager.Instance.m_languageDict.TryGetValue(languageType, out textAsset))
		{
			Debug.Log("<color=red>Could not load language type: " + languageType.ToString() + ". Language file does not exist.  Loading English language file.</color>");
			textAsset = LocalizationManager.Instance.m_languageDict[LanguageType.English];
			languageType = LanguageType.English;
		}
		if (!LocalizationManager.m_usingMaleNameOverrideList)
		{
			if (LocalizationManager.Instance.m_defaultMaleNamesDict.ContainsKey(languageType))
			{
				LocalizationManager.Instance.m_maleNameArray = LocalizationManager.Instance.m_defaultMaleNamesDict[languageType].text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.RemoveEmptyEntries);
			}
			else
			{
				LocalizationManager.Instance.m_maleNameArray = LocalizationManager.Instance.m_defaultMaleNamesDict[LanguageType.English].text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.RemoveEmptyEntries);
			}
		}
		if (!LocalizationManager.m_usingFemaleNameOverrideList)
		{
			if (LocalizationManager.Instance.m_defaultFemaleNamesDict.ContainsKey(languageType))
			{
				LocalizationManager.Instance.m_femaleNameArray = LocalizationManager.Instance.m_defaultFemaleNamesDict[languageType].text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.RemoveEmptyEntries);
			}
			else
			{
				LocalizationManager.Instance.m_femaleNameArray = LocalizationManager.Instance.m_defaultFemaleNamesDict[LanguageType.English].text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.RemoveEmptyEntries);
			}
		}
		foreach (string text in textAsset.text.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.RemoveEmptyEntries))
		{
			if (!string.IsNullOrWhiteSpace(text) && text[0] != '\\' && text[0] != '/' && text[1] != '/')
			{
				string[] array2 = text.Split(new char[]
				{
					'\t'
				});
				int num = array2.Length;
				string text2 = array2[0];
				if (!string.IsNullOrWhiteSpace(text2))
				{
					string b = text2;
					text2 = text2.Trim();
					if (text2 != b)
					{
						Debug.Log("<color=yellow>WARNING: White spacing found before or after locstring: " + text2 + ".  The white spacing has been removed.</color>");
					}
					string text3 = array2[1];
					string text4 = null;
					if (num > 2)
					{
						text4 = array2[2];
					}
					bool flag = false;
					if (string.IsNullOrWhiteSpace(text3))
					{
						flag = true;
						text3 = text2;
					}
					if (string.IsNullOrWhiteSpace(text4))
					{
						text4 = text3;
						if (flag)
						{
							text4 += "-F";
						}
					}
					if (LocalizationManager.Instance.m_maleLocDict.ContainsKey(text2))
					{
						LocalizationManager.Instance.m_maleLocDict[text2] = text3;
						LocalizationManager.Instance.m_femaleLocDict[text2] = text4;
					}
					else
					{
						LocalizationManager.Instance.m_maleLocDict.Add(text2, text3);
						LocalizationManager.Instance.m_femaleLocDict.Add(text2, text4);
					}
				}
			}
		}
		foreach (LocalizationManager.PlatformLocIDReplacement platformLocIDReplacement in LocalizationManager.Instance.m_platformLocIDReplacementArray)
		{
			if (LocalizationManager.Instance.m_maleLocDict.ContainsKey(platformLocIDReplacement.LocIDToReplace))
			{
				string replacementLocID = platformLocIDReplacement.GetReplacementLocID();
				if (LocalizationManager.Instance.m_maleLocDict.ContainsKey(replacementLocID))
				{
					LocalizationManager.Instance.m_maleLocDict[platformLocIDReplacement.LocIDToReplace] = LocalizationManager.Instance.m_maleLocDict[replacementLocID];
				}
				else
				{
					Debug.Log(string.Concat(new string[]
					{
						"Failed to replace male LocID: ",
						platformLocIDReplacement.LocIDToReplace,
						". Replacement LocID: ",
						replacementLocID,
						" not found."
					}));
				}
				if (LocalizationManager.Instance.m_femaleLocDict.ContainsKey(replacementLocID))
				{
					LocalizationManager.Instance.m_femaleLocDict[platformLocIDReplacement.LocIDToReplace] = LocalizationManager.Instance.m_femaleLocDict[replacementLocID];
				}
				else
				{
					Debug.Log(string.Concat(new string[]
					{
						"Failed to replace female LocID: ",
						platformLocIDReplacement.LocIDToReplace,
						". Replacement LocID: ",
						replacementLocID,
						" not found."
					}));
				}
			}
			else
			{
				Debug.Log("Failed to replace LocID: " + platformLocIDReplacement.LocIDToReplace + ". LocID not found.");
			}
		}
		LocalizationManager.Instance.m_currentLanguageType = languageType;
		if (broadcastEvent)
		{
			LocalizationManager.Instance.m_langChangedEventArgs.Initialize(LocalizationManager.CurrentLanguageType);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.LanguageChanged, LocalizationManager.Instance, LocalizationManager.Instance.m_langChangedEventArgs);
		}
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x000D9247 File Offset: 0x000D7447
	public static bool ContainsLocID(string locID, bool isFemale)
	{
		if (!isFemale)
		{
			return LocalizationManager.Instance.m_maleLocDict.ContainsKey(locID);
		}
		return LocalizationManager.Instance.m_femaleLocDict.ContainsKey(locID);
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x000D9270 File Offset: 0x000D7470
	public static string GetString(string locID, bool isFemale, out bool isFormatString, bool disableWordScramble = false)
	{
		isFormatString = false;
		string @string = LocalizationManager.GetString(locID, isFemale, disableWordScramble);
		if (@string.Contains('{') && @string.Contains('}'))
		{
			isFormatString = true;
		}
		return @string;
	}

	// Token: 0x06003E01 RID: 15873 RVA: 0x000D92A4 File Offset: 0x000D74A4
	public static string GetString(string locID, bool isFemale, bool disableWordScramble = false)
	{
		if (string.IsNullOrWhiteSpace(locID))
		{
			return "";
		}
		string text = "";
		Dictionary<string, string> dictionary = LocalizationManager.Instance.m_maleLocDict;
		if (isFemale)
		{
			dictionary = LocalizationManager.Instance.m_femaleLocDict;
		}
		if (!dictionary.TryGetValue(locID, out text))
		{
			return locID;
		}
		text = text.Replace("</LB>", "\n");
		int num = text.IndexOf('\n');
		while (num != -1 && num < text.Length - 1)
		{
			int num2 = 0;
			while (num + num2 + 1 < text.Length && text[num + num2 + 1] == ' ')
			{
				num2++;
			}
			if (num2 > 0)
			{
				text = text.Remove(num + 1, num2);
			}
			num = text.IndexOf('\n', num + 1);
		}
		int num3 = text.IndexOf('\r');
		while (num3 != -1 && num3 < text.Length - 1)
		{
			int num4 = 0;
			while (num3 + num4 + 1 < text.Length && text[num3 + num4 + 1] == ' ')
			{
				num4++;
			}
			if (num4 > 0)
			{
				text = text.Remove(num3, num4 + 1);
			}
			num3 = text.IndexOf('\r');
		}
		return text;
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x000D93BC File Offset: 0x000D75BC
	public static string GetLocalizedPlayerName(CharacterData charData)
	{
		bool isFemale = charData.IsFemale;
		string name = charData.Name;
		string arg = LocalizationManager.GetString("LOC_ID_ROYAL_TITLE_GENDER_1", isFemale, false);
		if ((charData.TraitOne == TraitType.Disposition || charData.TraitTwo == TraitType.Disposition) && charData.Disposition_ID == 2)
		{
			arg = "";
		}
		string text = string.Format(LocalizationManager.GetString("LOC_ID_FORMATTER_TITLE_1", isFemale, false), arg, name);
		if (charData.DuplicateNameCount > 0)
		{
			text = text + " " + CDGHelper.ToRoman(charData.DuplicateNameCount + 1);
		}
		return text;
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x000D943E File Offset: 0x000D763E
	public static void ChangeLanguage(LanguageType languageType, bool broadcastEvent)
	{
		LocalizationManager.Instance.ChangeLanguage_Internal(languageType, broadcastEvent);
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x000D944C File Offset: 0x000D764C
	public static void ForceRefreshAllTextGlyphs()
	{
		foreach (TextGlyphConverter textGlyphConverter in UnityEngine.Object.FindObjectsOfType<TextGlyphConverter>())
		{
			if (textGlyphConverter && textGlyphConverter.IsInitialized)
			{
				textGlyphConverter.UpdateText(true);
			}
		}
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x000D9488 File Offset: 0x000D7688
	public static string GetFormatterGenderForcedString(string formatter, out bool isFemale)
	{
		isFemale = false;
		if (formatter.Contains("[M]"))
		{
			isFemale = false;
			formatter = formatter.Replace("[M]", "");
			formatter = formatter.Trim();
		}
		else if (formatter.Contains("[F]"))
		{
			isFemale = true;
			formatter = formatter.Replace("[F]", "");
			formatter = formatter.Trim();
		}
		return formatter;
	}

	// Token: 0x04002E26 RID: 11814
	private const string LINE_BREAK = "</LB>";

	// Token: 0x04002E27 RID: 11815
	private const string FORMATTER_FORCE_M = "[M]";

	// Token: 0x04002E28 RID: 11816
	private const string FORMATTER_FORCE_F = "[F]";

	// Token: 0x04002E29 RID: 11817
	private static readonly string[] NAME_FILE_INSTRUCTIONS = new string[]
	{
		"// There must be at least 5 names on this list, otherwise the game will use its default name list.",
		"// Each name must be separated by a carriage return.",
		"// Any unsupported characters will appear as tofu (□) in the game.",
		"// When saving this file, make sure its encoding is UTF-8.",
		"// Use Notepad++ for best results!"
	};

	// Token: 0x04002E2A RID: 11818
	private Dictionary<LanguageType, string> m_languageLocStringDict = new Dictionary<LanguageType, string>
	{
		{
			LanguageType.English,
			"LOC_ID_GAME_SETTING_LANGUAGE_ENGLISH_1"
		},
		{
			LanguageType.French,
			"LOC_ID_GAME_SETTING_LANGUAGE_FRENCH_1"
		},
		{
			LanguageType.German,
			"LOC_ID_GAME_SETTING_LANGUAGE_GERMAN_1"
		},
		{
			LanguageType.Italian,
			"LOC_ID_GAME_SETTING_LANGUAGE_ITALIAN_1"
		},
		{
			LanguageType.Portuguese_BR,
			"LOC_ID_GAME_SETTING_LANGUAGE_BRAZILIAN_PORTUGUESE_1"
		},
		{
			LanguageType.Spanish,
			"LOC_ID_GAME_SETTING_LANGUAGE_SPANISH_1"
		},
		{
			LanguageType.Russian,
			"LOC_ID_GAME_SETTING_LANGUAGE_RUSSIAN_1"
		},
		{
			LanguageType.Korean,
			"LOC_ID_GAME_SETTING_LANGUAGE_KOREAN_1"
		},
		{
			LanguageType.Turkish,
			"LOC_ID_GAME_SETTING_LANGUAGE_TURKISH_1"
		},
		{
			LanguageType.Chinese_Simp,
			"LOC_ID_GAME_SETTING_LANGUAGE_CHINESE_SIMPLIFIED_1"
		},
		{
			LanguageType.Chinese_Trad,
			"LOC_ID_GAME_SETTING_LANGUAGE_CHINESE_TRADITIONAL_1"
		}
	};

	// Token: 0x04002E2B RID: 11819
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_languageDict;

	// Token: 0x04002E2C RID: 11820
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_defaultMaleNamesDict;

	// Token: 0x04002E2D RID: 11821
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_defaultFemaleNamesDict;

	// Token: 0x04002E2E RID: 11822
	[SerializeField]
	private LocalizationManager.PlatformLocIDReplacement[] m_platformLocIDReplacementArray;

	// Token: 0x04002E2F RID: 11823
	private LanguageType m_currentLanguageType;

	// Token: 0x04002E30 RID: 11824
	private Dictionary<string, string> m_maleLocDict;

	// Token: 0x04002E31 RID: 11825
	private Dictionary<string, string> m_femaleLocDict;

	// Token: 0x04002E32 RID: 11826
	private LanguageType[] m_availableLanguages;

	// Token: 0x04002E33 RID: 11827
	private LanguageChangedEventArgs m_langChangedEventArgs;

	// Token: 0x04002E34 RID: 11828
	private static bool m_isInitialized;

	// Token: 0x04002E35 RID: 11829
	private static bool m_usingMaleNameOverrideList;

	// Token: 0x04002E36 RID: 11830
	private static bool m_usingFemaleNameOverrideList;

	// Token: 0x04002E37 RID: 11831
	private string[] m_maleNameArray;

	// Token: 0x04002E38 RID: 11832
	private string[] m_femaleNameArray;

	// Token: 0x04002E39 RID: 11833
	private Dictionary<LanguageType, CultureInfo> m_cultureInfoDict;

	// Token: 0x04002E3A RID: 11834
	private static LocalizationManager m_manager = null;

	// Token: 0x02000E10 RID: 3600
	[Serializable]
	public struct PlatformLocIDReplacement
	{
		// Token: 0x06006B4A RID: 27466 RVA: 0x001912C1 File Offset: 0x0018F4C1
		public string GetReplacementLocID()
		{
			return this.LocIDToReplace;
		}

		// Token: 0x040056A1 RID: 22177
		public string LocIDToReplace;

		// Token: 0x040056A2 RID: 22178
		public string XboxLocID;

		// Token: 0x040056A3 RID: 22179
		public string PlayStationLocID;

		// Token: 0x040056A4 RID: 22180
		public string NintendoSwitchLocID;
	}
}
