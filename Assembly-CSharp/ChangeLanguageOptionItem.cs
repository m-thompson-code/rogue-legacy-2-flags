using System;
using UnityEngine;

// Token: 0x0200027E RID: 638
public class ChangeLanguageOptionItem : SelectionListOptionItem
{
	// Token: 0x06001948 RID: 6472 RVA: 0x0004F4D0 File Offset: 0x0004D6D0
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = Mathf.Max(0, LocalizationManager.AvailableLanguages.IndexOf(SaveManager.ConfigData.Language));
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x0004F520 File Offset: 0x0004D720
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[LocalizationManager.AvailableLanguages.Length];
		for (int i = 0; i < this.m_selectionLocIDArray.Length; i++)
		{
			LanguageType languageType = LocalizationManager.AvailableLanguages[i];
			this.m_selectionLocIDArray[i] = LocalizationManager.GetLanguageLocID(languageType);
		}
		this.m_selectedIndex = Mathf.Max(0, LocalizationManager.AvailableLanguages.IndexOf(SaveManager.ConfigData.Language));
		base.Initialize();
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x0004F58E File Offset: 0x0004D78E
	public override void InvokeValueChange()
	{
		Debug.Log("Changed language to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x0004F5A5 File Offset: 0x0004D7A5
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.Language = LocalizationManager.AvailableLanguages[this.m_selectedIndex];
		LocalizationManager.ChangeLanguage(SaveManager.ConfigData.Language, true);
	}
}
