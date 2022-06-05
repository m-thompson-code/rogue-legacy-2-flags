using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class YouAreSmall_Trait : BaseTrait
{
	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x060020C3 RID: 8387 RVA: 0x0006700B File Offset: 0x0006520B
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreSmall;
		}
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x0006700F File Offset: 0x0006520F
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

	// Token: 0x060020C5 RID: 8389 RVA: 0x00067018 File Offset: 0x00065218
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
