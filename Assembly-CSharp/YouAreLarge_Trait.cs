using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class YouAreLarge_Trait : BaseTrait
{
	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000180F5 File Offset: 0x000162F5
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreLarge;
		}
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x00019632 File Offset: 0x00017832
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		GameObject player = PlayerManager.GetPlayer();
		Vector3 localScale = player.transform.localScale;
		localScale.x = 2.1f;
		localScale.y = 2.1f;
		player.transform.localScale = localScale;
		PlayerManager.GetPlayerController().ControllerCorgi.InitializeRays();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerScaleChanged, null, null);
		yield break;
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x000C7F64 File Offset: 0x000C6164
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			GameObject player = PlayerManager.GetPlayer();
			Vector3 localScale = player.transform.localScale;
			localScale.x = 1.4f;
			localScale.y = 1.4f;
			player.transform.localScale = localScale;
			PlayerManager.GetPlayerController().ControllerCorgi.InitializeRays();
			if (!GameUtility.IsApplicationQuitting)
			{
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerScaleChanged, null, null);
			}
		}
	}
}
