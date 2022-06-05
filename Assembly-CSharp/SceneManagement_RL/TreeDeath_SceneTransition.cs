using System;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E37 RID: 3639
	public class TreeDeath_SceneTransition : HestiaDeath_SceneTransition
	{
		// Token: 0x170020E1 RID: 8417
		// (get) Token: 0x0600667E RID: 26238 RVA: 0x00038651 File Offset: 0x00036851
		protected override string AmbienceSFXName
		{
			get
			{
				return "event:/Cut_Scenes/sfx_openingCutscene_blackScreen";
			}
		}

		// Token: 0x170020E2 RID: 8418
		// (get) Token: 0x0600667F RID: 26239 RVA: 0x0002855A File Offset: 0x0002675A
		public override TransitionID ID
		{
			get
			{
				return TransitionID.TreeDeath;
			}
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x0017B1AC File Offset: 0x001793AC
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
