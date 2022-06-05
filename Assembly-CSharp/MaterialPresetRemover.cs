using System;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public class MaterialPresetRemover : MonoBehaviour
{
	// Token: 0x06003AD0 RID: 15056 RVA: 0x000F21E4 File Offset: 0x000F03E4
	private void Awake()
	{
		this.m_tmpText = base.GetComponent<TMP_Text>();
		if (this.m_tmpText)
		{
			this.m_cachedMaterialPreset = this.m_tmpText.fontSharedMaterial;
			if (this.m_cachedMaterialPreset)
			{
				Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, new Action<MonoBehaviour, EventArgs>(this.OnLanguageChanged));
			}
		}
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0002045D File Offset: 0x0001E65D
	private void OnEnable()
	{
		this.OnLanguageChanged(null, null);
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x000F223C File Offset: 0x000F043C
	private void OnLanguageChanged(object sender, EventArgs args)
	{
		if (!LocalizationManager.IsInitialized)
		{
			return;
		}
		if (!this.m_tmpText)
		{
			return;
		}
		if (this.m_languagesToRemove.Contains(LocalizationManager.CurrentLanguageType))
		{
			if (!this.m_materialPresetRemoved)
			{
				this.m_tmpText.fontSharedMaterial = this.m_tmpText.font.material;
				this.m_materialPresetRemoved = true;
				return;
			}
		}
		else if (this.m_materialPresetRemoved)
		{
			this.m_tmpText.fontSharedMaterial = this.m_cachedMaterialPreset;
			this.m_materialPresetRemoved = false;
		}
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x00020467 File Offset: 0x0001E667
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, new Action<MonoBehaviour, EventArgs>(this.OnLanguageChanged));
	}

	// Token: 0x04002EE7 RID: 12007
	[SerializeField]
	private LanguageType[] m_languagesToRemove;

	// Token: 0x04002EE8 RID: 12008
	private TMP_Text m_tmpText;

	// Token: 0x04002EE9 RID: 12009
	private Material m_cachedMaterialPreset;

	// Token: 0x04002EEA RID: 12010
	private bool m_materialPresetRemoved;
}
