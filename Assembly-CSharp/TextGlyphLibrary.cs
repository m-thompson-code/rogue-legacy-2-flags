using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000253 RID: 595
[CreateAssetMenu(menuName = "Custom/Libraries/Text Glyph Library")]
public class TextGlyphLibrary : ScriptableObject
{
	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06001782 RID: 6018 RVA: 0x00049333 File Offset: 0x00047533
	public static Dictionary<string, string> TextGlyphDict
	{
		get
		{
			return TextGlyphLibrary.Instance.m_textGlyphDict;
		}
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06001783 RID: 6019 RVA: 0x0004933F File Offset: 0x0004753F
	// (set) Token: 0x06001784 RID: 6020 RVA: 0x0004934B File Offset: 0x0004754B
	public static TMP_SpriteAsset[] SpriteAssetArray
	{
		get
		{
			return TextGlyphLibrary.Instance.m_spriteAssetArray;
		}
		set
		{
			TextGlyphLibrary.Instance.m_spriteAssetArray = value;
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06001785 RID: 6021 RVA: 0x00049358 File Offset: 0x00047558
	public static TextGlyphLibrary Instance
	{
		get
		{
			if (TextGlyphLibrary.m_instance == null)
			{
				TextGlyphLibrary.m_instance = CDGResources.Load<TextGlyphLibrary>("Scriptable Objects/Libraries/TextGlyphLibrary", "", true);
				if (Application.isPlaying && !TextGlyphLibrary.m_isInitialized)
				{
					TextGlyphLibrary.m_instance.Initialize();
				}
			}
			return TextGlyphLibrary.m_instance;
		}
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x000493A4 File Offset: 0x000475A4
	private void Initialize()
	{
		this.m_textGlyphDict = new Dictionary<string, string>();
		foreach (TMP_SpriteAsset tmp_SpriteAsset in this.m_spriteAssetArray)
		{
			if (!(tmp_SpriteAsset == null))
			{
				foreach (TMP_SpriteCharacter tmp_SpriteCharacter in tmp_SpriteAsset.spriteCharacterTable)
				{
					if (tmp_SpriteCharacter != null)
					{
						if (this.m_textGlyphDict.ContainsKey(tmp_SpriteCharacter.name))
						{
							throw new Exception("Text Glyph Library has two sprites with the same |" + tmp_SpriteCharacter.name + "| on different spritesheets.  All sprites must have distinct names.");
						}
						this.m_textGlyphDict.Add(tmp_SpriteCharacter.name, tmp_SpriteAsset.spriteSheet.name);
					}
				}
			}
		}
		TextGlyphLibrary.m_isInitialized = true;
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x00049480 File Offset: 0x00047680
	public static string GetTextGlyphSpritesheet(string entryName)
	{
		if (TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName))
		{
			return TextGlyphLibrary.Instance.m_textGlyphDict[entryName];
		}
		return null;
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000494A6 File Offset: 0x000476A6
	public static string GetTextGlyphRichTextString(string entryName)
	{
		if (TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName))
		{
			return string.Format("<sprite=\"{0}\" name=\"{1}\">", TextGlyphLibrary.Instance.m_textGlyphDict[entryName], entryName);
		}
		return null;
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000494D7 File Offset: 0x000476D7
	public static bool ContainsTextGlyph(string entryName)
	{
		return TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName);
	}

	// Token: 0x04001710 RID: 5904
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TextGlyphLibrary";

	// Token: 0x04001711 RID: 5905
	private const string TMP_SPRITE_RICHTEXT_STRING_FORMAT = "<sprite=\"{0}\" name=\"{1}\">";

	// Token: 0x04001712 RID: 5906
	[SerializeField]
	[HideInInspector]
	private TMP_SpriteAsset[] m_spriteAssetArray;

	// Token: 0x04001713 RID: 5907
	private Dictionary<string, string> m_textGlyphDict;

	// Token: 0x04001714 RID: 5908
	private static bool m_isInitialized;

	// Token: 0x04001715 RID: 5909
	private static TextGlyphLibrary m_instance;
}
