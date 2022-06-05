using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public class GardenHubTownController : BaseSpecialRoomController
{
	// Token: 0x06004661 RID: 18017 RVA: 0x00113BBC File Offset: 0x00111DBC
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		base.StartCoroutine(this.FlipPlayer());
		if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
		{
			WindowManager.LoadWindow(WindowID.SkillTree);
		}
		WindowController windowController = WindowManager.GetWindowController(WindowID.SkillTree);
		if (windowController)
		{
			SkillTreeWindowController skillTreeWindowController = windowController as SkillTreeWindowController;
			GameObject gameObject = skillTreeWindowController.SkillTreeCastleParentObj.transform.GetChild(0).gameObject;
			gameObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.SetParent(this.m_castleParentObj.transform, false);
			gameObject.SetLayerRecursively(26, false);
			EffectManager.AddAnimatorToDisableList(skillTreeWindowController.CastleAnimator);
			skillTreeWindowController.ForceUpdateSkillTreeAnimatorParams();
			skillTreeWindowController.CastleAnimator.SetBool("StartingStairs", true);
			skillTreeWindowController.CastleAnimator.SetBool("Entrance_1", true);
			skillTreeWindowController.CastleAnimator.Update(1f);
			EffectManager.RemoveAnimatorFromDisableList(skillTreeWindowController.CastleAnimator);
		}
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x00113CB0 File Offset: 0x00111EB0
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerExitRoom(sender, eventArgs);
		if (this.m_castleParentObj.transform.childCount > 0)
		{
			WindowController windowController = WindowManager.GetWindowController(WindowID.SkillTree);
			GameObject gameObject = this.m_castleParentObj.transform.GetChild(0).gameObject;
			SkillTreeWindowController skillTreeWindowController = windowController as SkillTreeWindowController;
			gameObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.SetParent(skillTreeWindowController.SkillTreeCastleParentObj.transform, false);
			gameObject.SetLayerRecursively(5, false);
		}
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x00026AB3 File Offset: 0x00024CB3
	private IEnumerator FlipPlayer()
	{
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsFacingRight)
		{
			playerController.CharacterCorgi.Flip(true, true);
		}
		yield break;
	}

	// Token: 0x0400364A RID: 13898
	[SerializeField]
	private GameObject m_castleParentObj;
}
