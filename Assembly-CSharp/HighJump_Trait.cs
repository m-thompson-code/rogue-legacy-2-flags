using System;
using System.Collections;

// Token: 0x020005A7 RID: 1447
public class HighJump_Trait : BaseTrait
{
	// Token: 0x17001216 RID: 4630
	// (get) Token: 0x06002D60 RID: 11616 RVA: 0x00017A47 File Offset: 0x00015C47
	public override TraitType TraitType
	{
		get
		{
			return TraitType.HighJump;
		}
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x00019088 File Offset: 0x00017288
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().CharacterJump.JumpHeightMultiplier = this.m_jumpHeightMultiplier;
		yield break;
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x00019097 File Offset: 0x00017297
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterJump.JumpHeightMultiplier = 1f;
		}
	}

	// Token: 0x040025BE RID: 9662
	private float m_jumpHeightMultiplier = 1.5f;
}
