using System;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class GlossaryStatusEffectEntry : GlossaryEntry
{
	// Token: 0x06002F60 RID: 12128 RVA: 0x000CA2DC File Offset: 0x000C84DC
	public override void Initialize()
	{
		this.m_icon.sprite = IconLibrary.GetStatusEffectSprite(StatusEffectType_RL.GetStatusBarType(this.m_statusEffectType));
		string locID;
		if (StatusEffect_EV.STATUS_EFFECT_TITLE_LOC_IDS.TryGetValue(this.m_statusEffectType, out locID))
		{
			this.m_titleText.text = LocalizationManager.GetString(locID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_titleText.text = this.m_statusEffectType.ToString() + " - MISSING TITLE LOCID";
		}
		string locID2;
		if (StatusEffect_EV.STATUS_EFFECT_DESC_LOC_IDS.TryGetValue(this.m_statusEffectType, out locID2))
		{
			this.m_descriptionText.text = LocalizationManager.GetString(locID2, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		this.m_descriptionText.text = this.m_statusEffectType.ToString() + " - MISSING DESC LOCID";
	}

	// Token: 0x040026C5 RID: 9925
	[SerializeField]
	private StatusEffectType m_statusEffectType;
}
