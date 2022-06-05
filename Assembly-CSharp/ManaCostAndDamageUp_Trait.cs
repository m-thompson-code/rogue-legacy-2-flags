using System;
using System.Collections;

// Token: 0x020005B6 RID: 1462
public class ManaCostAndDamageUp_Trait : BaseTrait
{
	// Token: 0x17001229 RID: 4649
	// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x000191BB File Offset: 0x000173BB
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ManaCostAndDamageUp;
		}
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x000191C2 File Offset: 0x000173C2
	public IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().InitializeMagicMods();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityHUD, this, null);
		yield break;
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x000191D1 File Offset: 0x000173D1
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeMagicMods();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityHUD, this, null);
		}
	}
}
