using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class ManorTransitionShop : MonoBehaviour, IRootObj
{
	// Token: 0x0600468A RID: 18058 RVA: 0x00026C31 File Offset: 0x00024E31
	public void TransitionToManor()
	{
		base.StartCoroutine(this.TransitionToManorCoroutine());
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x00026C40 File Offset: 0x00024E40
	private IEnumerator TransitionToManorCoroutine()
	{
		yield return this.MovePlayerToPosition(this.m_playerEnterPositionObj.transform.position);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_docks_transition_toSkilltree", default(Vector3));
		SceneLoader_RL.RunTransitionWithLogic(this.EnterManorCoroutine(), TransitionID.GardenToManor, false);
		yield break;
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x00026C4F File Offset: 0x00024E4F
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x00026C5E File Offset: 0x00024E5E
	private IEnumerator EnterManorCoroutine()
	{
		PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("ManorTunnel", false, false).Tunnel.ForceEnterTunnel(false, null);
		PlayerManager.GetPlayerController().SetFacing(true);
		yield break;
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003664 RID: 13924
	[SerializeField]
	private GameObject m_playerEnterPositionObj;
}
