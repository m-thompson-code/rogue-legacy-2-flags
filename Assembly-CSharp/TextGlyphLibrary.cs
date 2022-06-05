using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000411 RID: 1041
[CreateAssetMenu(menuName = "Custom/Libraries/Text Glyph Library")]
public class TextGlyphLibrary : ScriptableObject
{
	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06002136 RID: 8502 RVA: 0x00011A9C File Offset: 0x0000FC9C
	public static Dictionary<string, string> TextGlyphDict
	{
		get
		{
			return TextGlyphLibrary.Instance.m_textGlyphDict;
		}
	}

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06002137 RID: 8503 RVA: 0x00011AA8 File Offset: 0x0000FCA8
	// (set) Token: 0x06002138 RID: 8504 RVA: 0x00011AB4 File Offset: 0x0000FCB4
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

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06002139 RID: 8505 RVA: 0x000A6BD8 File Offset: 0x000A4DD8
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

	// Token: 0x0600213A RID: 8506 RVA: 0x000A6C24 File Offset: 0x000A4E24
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

	// Token: 0x0600213B RID: 8507 RVA: 0x00011AC1 File Offset: 0x0000FCC1
	public static string GetTextGlyphSpritesheet(string entryName)
	{
		if (TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName))
		{
			return TextGlyphLibrary.Instance.m_textGlyphDict[entryName];
		}
		return null;
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x00011AE7 File Offset: 0x0000FCE7
	public static string GetTextGlyphRichTextString(string entryName)
	{
		if (TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName))
		{
			return string.Format("<sprite=\"{0}\" name=\"{1}\">", TextGlyphLibrary.Instance.m_textGlyphDict[entryName], entryName);
		}
		return null;
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x00011B18 File Offset: 0x0000FD18
	public static bool ContainsTextGlyph(string entryName)
	{
		return TextGlyphLibrary.Instance.m_textGlyphDict.ContainsKey(entryName);
	}

	// Token: 0x04001E2A RID: 7722
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TextGlyphLibrary";

	// Token: 0x04001E2B RID: 7723
	private const string TMP_SPRITE_RICHTEXT_STRING_FORMAT = "<sprite=\"{0}\" name=\"{1}\">";

	// Token: 0x04001E2C RID: 7724
	[SerializeField]
	[HideInInspector]
	private TMP_SpriteAsset[] m_spriteAssetArray;

	// Token: 0x04001E2D RID: 7725
	private Dictionary<string, string> m_textGlyphDict;

	// Token: 0x04001E2E RID: 7726
	private static bool m_isInitialized;

	// Token: 0x04001E2F RID: 7727
	private static TextGlyphLibrary m_instance;
}
