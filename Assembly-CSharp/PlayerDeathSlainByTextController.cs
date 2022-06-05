using System;
using GameEventTracking;
using TMPro;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class PlayerDeathSlainByTextController : MonoBehaviour
{
	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x0600242F RID: 9263 RVA: 0x000778C0 File Offset: 0x00075AC0
	public TMP_Text Text
	{
		get
		{
			return this.m_slainByText;
		}
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x000778C8 File Offset: 0x00075AC8
	public void UpdateMessage(bool isVictory)
	{
		string slainBy = GameEventTrackerManager.EnemyEventTracker.GetSlainBy();
		string localizedPlayerName = LocalizationManager.GetLocalizedPlayerName(SaveManager.PlayerSaveData.CurrentCharacter);
		string playerKiller = GameEventTrackerManager.EnemyEventTracker.GetPlayerKiller();
		if (!isVictory)
		{
			string text = string.Format(slainBy, localizedPlayerName, playerKiller);
			this.m_slainByText.text = text;
			return;
		}
		string @string = LocalizationManager.GetString("LOC_ID_ENDING_VICTORY_DEATH_SLAIN_TEXT_OVERRIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		this.m_slainByText.text = string.Format(@string, localizedPlayerName);
	}

	// Token: 0x04001EA7 RID: 7847
	[SerializeField]
	private TMP_Text m_slainByText;
}
