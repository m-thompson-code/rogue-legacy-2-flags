using System;
using GameEventTracking;
using TMPro;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class PlayerDeathSlainByTextController : MonoBehaviour
{
	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x06003259 RID: 12889 RVA: 0x0001BA11 File Offset: 0x00019C11
	public TMP_Text Text
	{
		get
		{
			return this.m_slainByText;
		}
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000D7970 File Offset: 0x000D5B70
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

	// Token: 0x04002916 RID: 10518
	[SerializeField]
	private TMP_Text m_slainByText;
}
