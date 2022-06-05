using System;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class ReturnToMainMenuOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023F6 RID: 9206 RVA: 0x000AE5A8 File Offset: 0x000AC7A8
	public override void ActivateOption()
	{
		base.ActivateOption();
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (!currentPlayerRoom.IsNativeNull() && currentPlayerRoom.SaveController.CanSaveRoom)
			{
				RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(currentPlayerRoom.BiomeType, currentPlayerRoom.BiomeControllerIndex);
				currentPlayerRoom.SaveController.SaveRoomState(roomSaveData, true);
			}
		}
		if (ChallengeManager.IsInChallenge)
		{
			ChallengeManager.SetActiveChallenge(ChallengeType.None);
			ChallengeManager.RestoreCharacter(false);
			SaveManager.DisableSaving = false;
		}
		if (SceneLoader_RL.CurrentScene != SceneLoadingUtility.GetSceneName(SceneID.Tutorial))
		{
			SaveManager.PlayerSaveData.SecondsPlayed += (uint)GameTimer.TotalSessionAccumulatedTime;
			GameTimer.ClearSessionAccumulatedTime();
			SaveManager.SaveAllCurrentProfileGameData(SavingType.FileOnly, true, true);
		}
		Debug.Log("Returning to Main Menu...");
		SceneLoader_RL.LoadScene(SceneID.MainMenu, TransitionID.FadeToBlackNoLoading);
	}
}
