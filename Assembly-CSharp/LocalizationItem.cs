using System;
using TMPro;
using UnityEngine;

// Token: 0x02000779 RID: 1913
public class LocalizationItem : MonoBehaviour, ILocalizable
{
	// Token: 0x17001594 RID: 5524
	// (get) Token: 0x06003A30 RID: 14896 RVA: 0x0001FF56 File Offset: 0x0001E156
	public string LocID
	{
		get
		{
			return this.m_locID;
		}
	}

	// Token: 0x17001595 RID: 5525
	// (get) Token: 0x06003A31 RID: 14897 RVA: 0x0001FF5E File Offset: 0x0001E15E
	public TMP_Text TextObj
	{
		get
		{
			return this.m_textObj;
		}
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x0001FF66 File Offset: 0x0001E166
	protected virtual string GetLocString(string locID, bool isFemale)
	{
		return LocalizationManager.GetString(locID, isFemale, false);
	}

	// Token: 0x17001596 RID: 5526
	// (get) Token: 0x06003A33 RID: 14899 RVA: 0x0001FF70 File Offset: 0x0001E170
	// (set) Token: 0x06003A34 RID: 14900 RVA: 0x0001FF78 File Offset: 0x0001E178
	public StringGenderType StringGenderType
	{
		get
		{
			return this.m_stringGenderType;
		}
		set
		{
			this.m_stringGenderType = value;
		}
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x000ED760 File Offset: 0x000EB960
	private void OnEnable()
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		if (!this.m_textLoaded || this.m_currentLanguageType != LocalizationManager.CurrentLanguageType)
		{
			this.RefreshText(null, null);
		}
		if (!this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x0001FF81 File Offset: 0x0001E181
	private void OnDisable()
	{
		if (!this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x0001FF98 File Offset: 0x0001E198
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.Initialize();
		if (this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
	private void OnDestroy()
	{
		if (this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x0001FFDF File Offset: 0x0001E1DF
	private void Initialize()
	{
		this.m_textObj = base.GetComponent<TMP_Text>();
		if (this.m_textObj)
		{
			this.m_isInitialized = true;
			this.RefreshText(null, null);
			return;
		}
		Debug.Log("Could not find TMP_Text object on LocalizationItem GameObject.");
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x00020014 File Offset: 0x0001E214
	public void SetString(string locID)
	{
		this.m_locID = locID;
		this.RefreshText(null, null);
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x000ED7B0 File Offset: 0x000EB9B0
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_textLoaded = false;
		if (!this.m_isInitialized)
		{
			return;
		}
		if (!LocalizationManager.IsInitialized)
		{
			return;
		}
		bool isFemale = SaveManager.PlayerSaveData.CurrentCharacter.IsFemale;
		if (this.m_stringGenderType == StringGenderType.Male)
		{
			isFemale = false;
		}
		else if (this.m_stringGenderType == StringGenderType.Female)
		{
			isFemale = true;
		}
		if (!string.IsNullOrEmpty(this.m_locID))
		{
			string locString = this.GetLocString(this.m_locID, isFemale);
			this.m_textObj.text = locString;
		}
		this.m_currentLanguageType = LocalizationManager.CurrentLanguageType;
		this.m_textLoaded = true;
	}

	// Token: 0x04002E61 RID: 11873
	[SerializeField]
	[ReadOnlyOnPlay]
	private string m_locID;

	// Token: 0x04002E62 RID: 11874
	[SerializeField]
	private StringGenderType m_stringGenderType;

	// Token: 0x04002E63 RID: 11875
	[SerializeField]
	private bool m_alwaysRefreshText;

	// Token: 0x04002E64 RID: 11876
	protected TMP_Text m_textObj;

	// Token: 0x04002E65 RID: 11877
	private bool m_isInitialized;

	// Token: 0x04002E66 RID: 11878
	private bool m_textLoaded;

	// Token: 0x04002E67 RID: 11879
	private LanguageType m_currentLanguageType;

	// Token: 0x04002E68 RID: 11880
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
