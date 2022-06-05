using System;
using System.Collections;

// Token: 0x020005BE RID: 1470
public class NoHealthBar_Trait : BaseTrait
{
	// Token: 0x17001233 RID: 4659
	// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000054AD File Offset: 0x000036AD
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NoHealthBar;
		}
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x0001923F File Offset: 0x0001743F
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHealthChange, this, new HealthChangeEventArgs(playerController, playerController.CurrentHealth, (float)playerController.ActualMaxHealth));
		yield break;
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x000C7444 File Offset: 0x000C5644
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHealthChange, this, new HealthChangeEventArgs(playerController, playerController.CurrentHealth, (float)playerController.ActualMaxHealth));
		}
	}
}
