using System;
using TMPro;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class LocalizationItem : MonoBehaviour, ILocalizable
{
	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06002A1A RID: 10778 RVA: 0x0008BAFA File Offset: 0x00089CFA
	public string LocID
	{
		get
		{
			return this.m_locID;
		}
	}

	// Token: 0x1700105C RID: 4188
	// (get) Token: 0x06002A1B RID: 10779 RVA: 0x0008BB02 File Offset: 0x00089D02
	public TMP_Text TextObj
	{
		get
		{
			return this.m_textObj;
		}
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x0008BB0A File Offset: 0x00089D0A
	protected virtual string GetLocString(string locID, bool isFemale)
	{
		return LocalizationManager.GetString(locID, isFemale, false);
	}

	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06002A1D RID: 10781 RVA: 0x0008BB14 File Offset: 0x00089D14
	// (set) Token: 0x06002A1E RID: 10782 RVA: 0x0008BB1C File Offset: 0x00089D1C
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

	// Token: 0x06002A1F RID: 10783 RVA: 0x0008BB28 File Offset: 0x00089D28
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

	// Token: 0x06002A20 RID: 10784 RVA: 0x0008BB75 File Offset: 0x00089D75
	private void OnDisable()
	{
		if (!this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x0008BB8C File Offset: 0x00089D8C
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.Initialize();
		if (this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x0008BBBC File Offset: 0x00089DBC
	private void OnDestroy()
	{
		if (this.m_alwaysRefreshText)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		}
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x0008BBD3 File Offset: 0x00089DD3
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

	// Token: 0x06002A24 RID: 10788 RVA: 0x0008BC08 File Offset: 0x00089E08
	public void SetString(string locID)
	{
		this.m_locID = locID;
		this.RefreshText(null, null);
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x0008BC1C File Offset: 0x00089E1C
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

	// Token: 0x04002277 RID: 8823
	[SerializeField]
	[ReadOnlyOnPlay]
	private string m_locID;

	// Token: 0x04002278 RID: 8824
	[SerializeField]
	private StringGenderType m_stringGenderType;

	// Token: 0x04002279 RID: 8825
	[SerializeField]
	private bool m_alwaysRefreshText;

	// Token: 0x0400227A RID: 8826
	protected TMP_Text m_textObj;

	// Token: 0x0400227B RID: 8827
	private bool m_isInitialized;

	// Token: 0x0400227C RID: 8828
	private bool m_textLoaded;

	// Token: 0x0400227D RID: 8829
	private LanguageType m_currentLanguageType;

	// Token: 0x0400227E RID: 8830
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
