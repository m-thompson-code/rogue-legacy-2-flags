using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x02000B39 RID: 2873
public class LocalizationManager : MonoBehaviour
{
	// Token: 0x17001D3B RID: 7483
	// (get) Token: 0x060056DD RID: 22237 RVA: 0x0002F3EA File Offset: 0x0002D5EA
	public static LanguageType[] AvailableLanguages
	{
		get
		{
			return LocalizationManager.Instance.m_availableLanguages;
		}
	}

	// Token: 0x17001D3C RID: 7484
	// (get) Token: 0x060056DE RID: 22238 RVA: 0x0002F3F6 File Offset: 0x0002D5F6
	public static int NumberOfLanguages
	{
		get
		{
			return LocalizationManager.Instance.m_availableLanguages.Length;
		}
	}

	// Token: 0x17001D3D RID: 7485
	// (get) Token: 0x060056DF RID: 22239 RVA: 0x0002F404 File Offset: 0x0002D604
	public static int MaleDictSize
	{
		get
		{
			return LocalizationManager.Instance.m_maleLocDict.Count;
		}
	}

	// Token: 0x17001D3E RID: 7486
	// (get) Token: 0x060056E0 RID: 22240 RVA: 0x0002F415 File Offset: 0x0002D615
	public static int FemaleDictSize
	{
		get
		{
			return LocalizationManager.Instance.m_femaleLocDict.Count;
		}
	}

	// Token: 0x17001D3F RID: 7487
	// (get) Token: 0x060056E1 RID: 22241 RVA: 0x0002F426 File Offset: 0x0002D626
	public static bool IsInitialized
	{
		get
		{
			return LocalizationManager.m_isInitialized;
		}
	}

	// Token: 0x17001D40 RID: 7488
	// (get) Token: 0x060056E2 RID: 22242 RVA: 0x0002F42D File Offset: 0x0002D62D
	public static string[] MaleNameArray
	{
		get
		{
			return LocalizationManager.Instance.m_maleNameArray;
		}
	}

	// Token: 0x17001D41 RID: 7489
	// (get) Token: 0x060056E3 RID: 22243 RVA: 0x0002F439 File Offset: 0x0002D639
	public static string[] FemaleNameArray
	{
		get
		{
			return LocalizationManager.Instance.m_femaleNameArray;
		}
	}

	// Token: 0x17001D42 RID: 7490
	// (get) Token: 0x060056E4 RID: 22244 RVA: 0x0002F445 File Offset: 0x0002D645
	public static LanguageType CurrentLanguageType
	{
		get
		{
			return LocalizationManager.Instance.m_currentLanguageType;
		}
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x001491B0 File Offset: 0x001473B0
	public static string GetLanguageLocID(LanguageType languageType)
	{
		string result;
		if (LocalizationManager.Instance.m_languageLocStringDict.TryGetValue(languageType, out result))
		{
			return result;
		}
		return LocalizationManager.Instance.m_languageLocStringDict[LanguageType.English];
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x0002F451 File Offset: 0x0002D651
	public static CultureInfo GetCurrentCultureInfo()
	{
		return LocalizationManager.GetCultureInfo(LocalizationManager.CurrentLanguageType);
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x001491E4 File Offset: 0x001473E4
	public static CultureInfo GetCultureInfo(LanguageType languageType)
	{
		CultureInfo result;
		if (LocalizationManager.Instance.m_cultureInfoDict.TryGetValue(languageType, out result))
		{
			return result;
		}
		return SaveManager.CultureInfo;
	}

	// Token: 0x17001D43 RID: 7491
	// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0002F45D File Offset: 0x0002D65D
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

	// Token: 0x060056E9 RID: 22249 RVA: 0x0014920C File Offset: 0x0014740C
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

	// Token: 0x060056EA RID: 22250 RVA: 0x001492D0 File Offset: 0x001474D0
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

	// Token: 0x060056EB RID: 22251 RVA: 0x0002F47B File Offset: 0x0002D67B
	private void ChangeLanguage_Internal(LanguageType languageType, bool broadcastEvent)
	{
		if (LocalizationManager.CurrentLanguageType != languageType)
		{
			LocalizationManager.LoadLanguage(languageType, broadcastEvent);
		}
	}

	// Token: 0x060056EC RID: 22252 RVA: 0x0002F48C File Offset: 0x0002D68C
	private bool IsValidName(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && !name.Contains('\\') && (name.Length < 2 || name[0] != '/' || name[1] != '/');
	}

	// Token: 0x060056ED RID: 22253 RVA: 0x001493DC File Offset: 0x001475DC
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

	// Token: 0x060056EE RID: 22254 RVA: 0x0002F4C6 File Offset: 0x0002D6C6
	private void LoadNames()
	{
		this.LoadOrCreateCustomNames(true);
		this.LoadOrCreateCustomNames(false);
	}

	// Token: 0x060056EF RID: 22255 RVA: 0x00149664 File Offset: 0x00147864
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

	// Token: 0x060056F0 RID: 22256 RVA: 0x0002F4D6 File Offset: 0x0002D6D6
	public static bool ContainsLocID(string locID, bool isFemale)
	{
		if (!isFemale)
		{
			return LocalizationManager.Instance.m_maleLocDict.ContainsKey(locID);
		}
		return LocalizationManager.Instance.m_femaleLocDict.ContainsKey(locID);
	}

	// Token: 0x060056F1 RID: 22257 RVA: 0x00149B3C File Offset: 0x00147D3C
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

	// Token: 0x060056F2 RID: 22258 RVA: 0x00149B70 File Offset: 0x00147D70
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

	// Token: 0x060056F3 RID: 22259 RVA: 0x00149C88 File Offset: 0x00147E88
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

	// Token: 0x060056F4 RID: 22260 RVA: 0x0002F4FC File Offset: 0x0002D6FC
	public static void ChangeLanguage(LanguageType languageType, bool broadcastEvent)
	{
		LocalizationManager.Instance.ChangeLanguage_Internal(languageType, broadcastEvent);
	}

	// Token: 0x060056F5 RID: 22261 RVA: 0x00149D0C File Offset: 0x00147F0C
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

	// Token: 0x060056F6 RID: 22262 RVA: 0x00149D48 File Offset: 0x00147F48
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

	// Token: 0x0400403F RID: 16447
	private const string LINE_BREAK = "</LB>";

	// Token: 0x04004040 RID: 16448
	private const string FORMATTER_FORCE_M = "[M]";

	// Token: 0x04004041 RID: 16449
	private const string FORMATTER_FORCE_F = "[F]";

	// Token: 0x04004042 RID: 16450
	private static readonly string[] NAME_FILE_INSTRUCTIONS = new string[]
	{
		"// There must be at least 5 names on this list, otherwise the game will use its default name list.",
		"// Each name must be separated by a carriage return.",
		"// Any unsupported characters will appear as tofu (□) in the game.",
		"// When saving this file, make sure its encoding is UTF-8.",
		"// Use Notepad++ for best results!"
	};

	// Token: 0x04004043 RID: 16451
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

	// Token: 0x04004044 RID: 16452
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_languageDict;

	// Token: 0x04004045 RID: 16453
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_defaultMaleNamesDict;

	// Token: 0x04004046 RID: 16454
	[SerializeField]
	private LanguageTypeTextAssetDictionary m_defaultFemaleNamesDict;

	// Token: 0x04004047 RID: 16455
	[SerializeField]
	private LocalizationManager.PlatformLocIDReplacement[] m_platformLocIDReplacementArray;

	// Token: 0x04004048 RID: 16456
	private LanguageType m_currentLanguageType;

	// Token: 0x04004049 RID: 16457
	private Dictionary<string, string> m_maleLocDict;

	// Token: 0x0400404A RID: 16458
	private Dictionary<string, string> m_femaleLocDict;

	// Token: 0x0400404B RID: 16459
	private LanguageType[] m_availableLanguages;

	// Token: 0x0400404C RID: 16460
	private LanguageChangedEventArgs m_langChangedEventArgs;

	// Token: 0x0400404D RID: 16461
	private static bool m_isInitialized;

	// Token: 0x0400404E RID: 16462
	private static bool m_usingMaleNameOverrideList;

	// Token: 0x0400404F RID: 16463
	private static bool m_usingFemaleNameOverrideList;

	// Token: 0x04004050 RID: 16464
	private string[] m_maleNameArray;

	// Token: 0x04004051 RID: 16465
	private string[] m_femaleNameArray;

	// Token: 0x04004052 RID: 16466
	private Dictionary<LanguageType, CultureInfo> m_cultureInfoDict;

	// Token: 0x04004053 RID: 16467
	private static LocalizationManager m_manager = null;

	// Token: 0x02000B3A RID: 2874
	[Serializable]
	public struct PlatformLocIDReplacement
	{
		// Token: 0x060056F9 RID: 22265 RVA: 0x0002F545 File Offset: 0x0002D745
		public string GetReplacementLocID()
		{
			return this.LocIDToReplace;
		}

		// Token: 0x04004054 RID: 16468
		public string LocIDToReplace;

		// Token: 0x04004055 RID: 16469
		public string XboxLocID;

		// Token: 0x04004056 RID: 16470
		public string PlayStationLocID;

		// Token: 0x04004057 RID: 16471
		public string NintendoSwitchLocID;
	}
}
