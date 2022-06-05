using System;
using System.Collections;

// Token: 0x02000353 RID: 851
public class NoHealthBar_Trait : BaseTrait
{
	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x0600205A RID: 8282 RVA: 0x00066759 File Offset: 0x00064959
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NoHealthBar;
		}
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x0006675D File Offset: 0x0006495D
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

	// Token: 0x0600205C RID: 8284 RVA: 0x0006676C File Offset: 0x0006496C
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHealthChange, this, new HealthChangeEventArgs(playerController, playerController.CurrentHealth, (float)playerController.ActualMaxHealth));
		}
	}
}
