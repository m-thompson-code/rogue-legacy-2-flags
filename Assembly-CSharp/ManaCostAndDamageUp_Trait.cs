using System;
using System.Collections;

// Token: 0x0200034D RID: 845
public class ManaCostAndDamageUp_Trait : BaseTrait
{
	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x0600204A RID: 8266 RVA: 0x000666B6 File Offset: 0x000648B6
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ManaCostAndDamageUp;
		}
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000666BD File Offset: 0x000648BD
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

	// Token: 0x0600204C RID: 8268 RVA: 0x000666CC File Offset: 0x000648CC
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeMagicMods();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityHUD, this, null);
		}
	}
}
