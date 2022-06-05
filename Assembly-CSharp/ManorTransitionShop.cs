using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class ManorTransitionShop : MonoBehaviour, IRootObj
{
	// Token: 0x06003233 RID: 12851 RVA: 0x000AA406 File Offset: 0x000A8606
	public void TransitionToManor()
	{
		base.StartCoroutine(this.TransitionToManorCoroutine());
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x000AA415 File Offset: 0x000A8615
	private IEnumerator TransitionToManorCoroutine()
	{
		yield return this.MovePlayerToPosition(this.m_playerEnterPositionObj.transform.position);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_docks_transition_toSkilltree", default(Vector3));
		SceneLoader_RL.RunTransitionWithLogic(this.EnterManorCoroutine(), TransitionID.GardenToManor, false);
		yield break;
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x000AA424 File Offset: 0x000A8624
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x000AA433 File Offset: 0x000A8633
	private IEnumerator EnterManorCoroutine()
	{
		PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("ManorTunnel", false, false).Tunnel.ForceEnterTunnel(false, null);
		PlayerManager.GetPlayerController().SetFacing(true);
		yield break;
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x000AA443 File Offset: 0x000A8643
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400277A RID: 10106
	[SerializeField]
	private GameObject m_playerEnterPositionObj;
}
