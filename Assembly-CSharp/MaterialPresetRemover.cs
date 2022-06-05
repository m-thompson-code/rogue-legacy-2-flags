using System;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class MaterialPresetRemover : MonoBehaviour
{
	// Token: 0x06002AA5 RID: 10917 RVA: 0x000906D8 File Offset: 0x0008E8D8
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

	// Token: 0x06002AA6 RID: 10918 RVA: 0x0009072F File Offset: 0x0008E92F
	private void OnEnable()
	{
		this.OnLanguageChanged(null, null);
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x0009073C File Offset: 0x0008E93C
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

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000907BC File Offset: 0x0008E9BC
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, new Action<MonoBehaviour, EventArgs>(this.OnLanguageChanged));
	}

	// Token: 0x040022E7 RID: 8935
	[SerializeField]
	private LanguageType[] m_languagesToRemove;

	// Token: 0x040022E8 RID: 8936
	private TMP_Text m_tmpText;

	// Token: 0x040022E9 RID: 8937
	private Material m_cachedMaterialPreset;

	// Token: 0x040022EA RID: 8938
	private bool m_materialPresetRemoved;
}
