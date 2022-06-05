using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class GainDownStrike_Trait : BaseTrait
{
	// Token: 0x1700120E RID: 4622
	// (get) Token: 0x06002D43 RID: 11587 RVA: 0x0001737E File Offset: 0x0001557E
	public override TraitType TraitType
	{
		get
		{
			return TraitType.GainDownStrike;
		}
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x00018FDF File Offset: 0x000171DF
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().CharacterDownStrike.SpinKickInstead = false;
		yield break;
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x00018FE7 File Offset: 0x000171E7
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.SpinKickInstead = true;
		}
	}
}
