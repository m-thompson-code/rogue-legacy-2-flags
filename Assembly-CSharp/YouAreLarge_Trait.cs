using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class YouAreLarge_Trait : BaseTrait
{
	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x060020BF RID: 8383 RVA: 0x00066F8F File Offset: 0x0006518F
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreLarge;
		}
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x00066F93 File Offset: 0x00065193
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

	// Token: 0x060020C1 RID: 8385 RVA: 0x00066F9C File Offset: 0x0006519C
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
