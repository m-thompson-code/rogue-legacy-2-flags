using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
[CreateAssetMenu(menuName = "Custom/Libraries/FontAsset Library")]
public class FontAssetLibrary : ScriptableObject
{
	// Token: 0x17000E5A RID: 3674
	// (get) Token: 0x06002061 RID: 8289 RVA: 0x000A4E14 File Offset: 0x000A3014
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

	// Token: 0x06002062 RID: 8290 RVA: 0x000A4E64 File Offset: 0x000A3064
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

	// Token: 0x06002063 RID: 8291 RVA: 0x000A4F40 File Offset: 0x000A3140
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

	// Token: 0x06002064 RID: 8292 RVA: 0x000A4F74 File Offset: 0x000A3174
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

	// Token: 0x04001D0B RID: 7435
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/FontAssetLibrary";

	// Token: 0x04001D0C RID: 7436
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/FontAssetLibrary.asset";

	// Token: 0x04001D0D RID: 7437
	[Header("Font Asset Replacement")]
	[SerializeField]
	private FontAssetLibrary.FontAssetEntry[] m_fontAssetArray;

	// Token: 0x04001D0E RID: 7438
	[Header("Font Material Preset Replacement")]
	[Space(10f)]
	[SerializeField]
	private FontAssetLibrary.FontMaterialEntry[] m_fontMaterialArray;

	// Token: 0x04001D0F RID: 7439
	private Dictionary<TMP_FontAsset, FontAssetLibrary.FontAssetEntry> m_fontAssetDict;

	// Token: 0x04001D10 RID: 7440
	private Dictionary<Material, FontAssetLibrary.FontMaterialEntry> m_fontMaterialDict;

	// Token: 0x04001D11 RID: 7441
	private static FontAssetLibrary m_instance;

	// Token: 0x020003EA RID: 1002
	[Serializable]
	private class FontAssetEntry
	{
		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x000112EB File Offset: 0x0000F4EB
		public TMP_FontAsset DefaultFontAsset
		{
			get
			{
				return this.m_defaultFontAsset;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000112F3 File Offset: 0x0000F4F3
		public LanguageTypeFontAssetDictionary FontAssetTable
		{
			get
			{
				return this.m_fontAssetTable;
			}
		}

		// Token: 0x04001D12 RID: 7442
		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;

		// Token: 0x04001D13 RID: 7443
		[SerializeField]
		private LanguageTypeFontAssetDictionary m_fontAssetTable;
	}

	// Token: 0x020003EB RID: 1003
	[Serializable]
	private class FontMaterialEntry
	{
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000112FB File Offset: 0x0000F4FB
		public Material DefaultFontMaterial
		{
			get
			{
				return this.m_defaultFontMaterial;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x00011303 File Offset: 0x0000F503
		public LanguageTypeFontMaterialDictionary FontMaterialTable
		{
			get
			{
				return this.m_fontMaterialTable;
			}
		}

		// Token: 0x04001D14 RID: 7444
		[SerializeField]
		private Material m_defaultFontMaterial;

		// Token: 0x04001D15 RID: 7445
		[SerializeField]
		private LanguageTypeFontMaterialDictionary m_fontMaterialTable;
	}
}
