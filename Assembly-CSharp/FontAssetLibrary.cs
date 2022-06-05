using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200022E RID: 558
[CreateAssetMenu(menuName = "Custom/Libraries/FontAsset Library")]
public class FontAssetLibrary : ScriptableObject
{
	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x060016B4 RID: 5812 RVA: 0x00046D00 File Offset: 0x00044F00
	private static FontAssetLibrary Instance
	{
		get
		{
			if (!FontAssetLibrary.m_instance)
			{
				if (Application.isPlaying)
				{
					FontAssetLibrary.m_instance = CDGResources.Load<FontAssetLibrary>("Scriptable Objects/Libraries/FontAssetLibrary", "", true);
				}
				if (FontAssetLibrary.m_instance)
				{
					FontAssetLibrary.m_instance.Initialize();
				}
			}
			return FontAssetLibrary.m_instance;
		}
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x00046D50 File Offset: 0x00044F50
	private void Initialize()
	{
		this.m_fontAssetDict = new Dictionary<TMP_FontAsset, FontAssetLibrary.FontAssetEntry>();
		foreach (FontAssetLibrary.FontAssetEntry fontAssetEntry in this.m_fontAssetArray)
		{
			if (!this.m_fontAssetDict.ContainsKey(fontAssetEntry.DefaultFontAsset))
			{
				this.m_fontAssetDict.Add(fontAssetEntry.DefaultFontAsset, fontAssetEntry);
			}
			else
			{
				Debug.Log("<color=yellow>WARNING: Duplicate font ASSET entry found. Ignoring entry: " + fontAssetEntry.DefaultFontAsset.name);
			}
		}
		this.m_fontMaterialDict = new Dictionary<Material, FontAssetLibrary.FontMaterialEntry>();
		foreach (FontAssetLibrary.FontMaterialEntry fontMaterialEntry in this.m_fontMaterialArray)
		{
			if (!this.m_fontMaterialDict.ContainsKey(fontMaterialEntry.DefaultFontMaterial))
			{
				this.m_fontMaterialDict.Add(fontMaterialEntry.DefaultFontMaterial, fontMaterialEntry);
			}
			else
			{
				Debug.Log("<color=yellow>WARNING: Duplicate font MATERIAL entry found. Ignoring entry: " + fontMaterialEntry.DefaultFontMaterial.name);
			}
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x00046E2C File Offset: 0x0004502C
	public static TMP_FontAsset GetFontAsset(TMP_FontAsset fontAsset, LanguageType language)
	{
		FontAssetLibrary.FontAssetEntry fontAssetEntry;
		TMP_FontAsset result;
		if (FontAssetLibrary.m_instance.m_fontAssetDict.TryGetValue(fontAsset, out fontAssetEntry) && fontAssetEntry.FontAssetTable.TryGetValue(language, out result))
		{
			return result;
		}
		return fontAsset;
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x00046E60 File Offset: 0x00045060
	public static Material GetFontMaterial(Material material, LanguageType language)
	{
		FontAssetLibrary.FontMaterialEntry fontMaterialEntry;
		Material result;
		if (FontAssetLibrary.m_instance.m_fontMaterialDict.TryGetValue(material, out fontMaterialEntry) && fontMaterialEntry.FontMaterialTable.TryGetValue(language, out result))
		{
			return result;
		}
		return material;
	}

	// Token: 0x040015F7 RID: 5623
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/FontAssetLibrary";

	// Token: 0x040015F8 RID: 5624
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/FontAssetLibrary.asset";

	// Token: 0x040015F9 RID: 5625
	[Header("Font Asset Replacement")]
	[SerializeField]
	private FontAssetLibrary.FontAssetEntry[] m_fontAssetArray;

	// Token: 0x040015FA RID: 5626
	[Header("Font Material Preset Replacement")]
	[Space(10f)]
	[SerializeField]
	private FontAssetLibrary.FontMaterialEntry[] m_fontMaterialArray;

	// Token: 0x040015FB RID: 5627
	private Dictionary<TMP_FontAsset, FontAssetLibrary.FontAssetEntry> m_fontAssetDict;

	// Token: 0x040015FC RID: 5628
	private Dictionary<Material, FontAssetLibrary.FontMaterialEntry> m_fontMaterialDict;

	// Token: 0x040015FD RID: 5629
	private static FontAssetLibrary m_instance;

	// Token: 0x02000B34 RID: 2868
	[Serializable]
	private class FontAssetEntry
	{
		// Token: 0x17001E60 RID: 7776
		// (get) Token: 0x06005C1A RID: 23578 RVA: 0x0015BE7F File Offset: 0x0015A07F
		public TMP_FontAsset DefaultFontAsset
		{
			get
			{
				return this.m_defaultFontAsset;
			}
		}

		// Token: 0x17001E61 RID: 7777
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x0015BE87 File Offset: 0x0015A087
		public LanguageTypeFontAssetDictionary FontAssetTable
		{
			get
			{
				return this.m_fontAssetTable;
			}
		}

		// Token: 0x04004BA9 RID: 19369
		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;

		// Token: 0x04004BAA RID: 19370
		[SerializeField]
		private LanguageTypeFontAssetDictionary m_fontAssetTable;
	}

	// Token: 0x02000B35 RID: 2869
	[Serializable]
	private class FontMaterialEntry
	{
		// Token: 0x17001E62 RID: 7778
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x0015BE97 File Offset: 0x0015A097
		public Material DefaultFontMaterial
		{
			get
			{
				return this.m_defaultFontMaterial;
			}
		}

		// Token: 0x17001E63 RID: 7779
		// (get) Token: 0x06005C1E RID: 23582 RVA: 0x0015BE9F File Offset: 0x0015A09F
		public LanguageTypeFontMaterialDictionary FontMaterialTable
		{
			get
			{
				return this.m_fontMaterialTable;
			}
		}

		// Token: 0x04004BAB RID: 19371
		[SerializeField]
		private Material m_defaultFontMaterial;

		// Token: 0x04004BAC RID: 19372
		[SerializeField]
		private LanguageTypeFontMaterialDictionary m_fontMaterialTable;
	}
}
