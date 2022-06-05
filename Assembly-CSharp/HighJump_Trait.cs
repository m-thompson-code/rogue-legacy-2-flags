using System;
using System.Collections;

// Token: 0x02000345 RID: 837
public class HighJump_Trait : BaseTrait
{
	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x0600202D RID: 8237 RVA: 0x000664B5 File Offset: 0x000646B5
	public override TraitType TraitType
	{
		get
		{
			return TraitType.HighJump;
		}
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000664BC File Offset: 0x000646BC
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().CharacterJump.JumpHeightMultiplier = this.m_jumpHeightMultiplier;
		yield break;
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000664CB File Offset: 0x000646CB
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterJump.JumpHeightMultiplier = 1f;
		}
	}

	// Token: 0x04001C53 RID: 7251
	private float m_jumpHeightMultiplier = 1.5f;
}
