using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class YouAreSmall_Trait : BaseTrait
{
	// Token: 0x17001274 RID: 4724
	// (get) Token: 0x06002E83 RID: 11907 RVA: 0x0000452B File Offset: 0x0000272B
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreSmall;
		}
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x0001965D File Offset: 0x0001785D
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		GameObject player = PlayerManager.GetPlayer();
		Vector3 localScale = player.transform.localScale;
		localScale.x = 0.77f;
		localScale.y = 0.77f;
		player.transform.localScale = localScale;
		PlayerManager.GetPlayerController().ControllerCorgi.InitializeRays();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerScaleChanged, null, null);
		yield break;
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x000C7F64 File Offset: 0x000C6164
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
