using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class ChangeLanguageOptionItem : SelectionListOptionItem
{
	// Token: 0x06002337 RID: 9015 RVA: 0x000ACACC File Offset: 0x000AACCC
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = Mathf.Max(0, LocalizationManager.AvailableLanguages.IndexOf(SaveManager.ConfigData.Language));
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000ACB1C File Offset: 0x000AAD1C
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

	// Token: 0x06002339 RID: 9017 RVA: 0x00012F28 File Offset: 0x00011128
	public override void InvokeValueChange()
	{
		Debug.Log("Changed language to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x00012F3F File Offset: 0x0001113F
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.Language = LocalizationManager.AvailableLanguages[this.m_selectedIndex];
		LocalizationManager.ChangeLanguage(SaveManager.ConfigData.Language, true);
	}
}
