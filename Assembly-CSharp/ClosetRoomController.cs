using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class ClosetRoomController : BaseSpecialRoomController
{
	// Token: 0x060041B2 RID: 16818 RVA: 0x00108700 File Offset: 0x00106900
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		base.StartCoroutine(this.FixTunnelLayer());
		if (!GlobalTimerHUDController.IsRunning)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ResetGlobalTimer, null, null);
			GlobalTimerHUDController.ReverseTimer = true;
			GlobalTimerHUDController.ReverseStartTime = 600f;
			GlobalTimerHUDController.TrackNegativeTimeAchievement = true;
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StartGlobalTimer, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayGlobalTimer, null, null);
		}
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x00024631 File Offset: 0x00022831
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!PlayerManager.GetPlayerController().IsFacingRight)
		{
			PlayerManager.GetPlayerController().CharacterCorgi.Flip(false, false);
		}
		base.OnPlayerExitRoom(sender, eventArgs);
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x00024658 File Offset: 0x00022858
	private IEnumerator FixTunnelLayer()
	{
		yield return null;
		SpriteRenderer componentInChildren = this.m_closetTunnelSpawner.Tunnel.GetComponentInChildren<SpriteRenderer>();
		componentInChildren.gameObject.layer = 24;
		Vector3 localPosition = componentInChildren.transform.localPosition;
		localPosition.z = 5f;
		componentInChildren.transform.localPosition = localPosition;
		yield break;
	}

	// Token: 0x0400336C RID: 13164
	[SerializeField]
	private TunnelSpawnController m_closetTunnelSpawner;
}
