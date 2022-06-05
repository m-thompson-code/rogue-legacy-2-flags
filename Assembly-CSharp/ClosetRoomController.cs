using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F7 RID: 1271
public class ClosetRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F8B RID: 12171 RVA: 0x000A2D38 File Offset: 0x000A0F38
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

	// Token: 0x06002F8C RID: 12172 RVA: 0x000A2D92 File Offset: 0x000A0F92
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!PlayerManager.GetPlayerController().IsFacingRight)
		{
			PlayerManager.GetPlayerController().CharacterCorgi.Flip(false, false);
		}
		base.OnPlayerExitRoom(sender, eventArgs);
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x000A2DB9 File Offset: 0x000A0FB9
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

	// Token: 0x040025E5 RID: 9701
	[SerializeField]
	private TunnelSpawnController m_closetTunnelSpawner;
}
