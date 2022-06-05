using System;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008CD RID: 2253
	public class TreeDeath_SceneTransition : HestiaDeath_SceneTransition
	{
		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x0010A4A9 File Offset: 0x001086A9
		protected override string AmbienceSFXName
		{
			get
			{
				return "event:/Cut_Scenes/sfx_openingCutscene_blackScreen";
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x0010A4B0 File Offset: 0x001086B0
		public override TransitionID ID
		{
			get
			{
				return TransitionID.TreeDeath;
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0010A4B4 File Offset: 0x001086B4
		protected override void InitializeText()
		{
			int num = Mathf.Min((int)SaveManager.PlayerSaveData.TreeCutsceneDisplayCount, Ending_EV.TREE_CUTSCENE_DIALOGUE_1_LOCID.Length - 1);
			this.m_text1.text = LocalizationManager.GetString(Ending_EV.TREE_CUTSCENE_DIALOGUE_1_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_text2.text = LocalizationManager.GetString(Ending_EV.TREE_CUTSCENE_DIALOGUE_2_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Tree_DeathCutscene, false);
			PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
			playerSaveData.TreeCutsceneDisplayCount += 1;
		}
	}
}
