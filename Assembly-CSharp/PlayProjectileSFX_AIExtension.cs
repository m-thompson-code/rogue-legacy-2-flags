using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200028B RID: 651
public static class PlayProjectileSFX_AIExtension
{
	// Token: 0x060012A8 RID: 4776 RVA: 0x00082520 File Offset: 0x00080720
	public static void PlayProjectileSFX(this BaseAIScript aiScript, string projectileName, bool useSingleShotAudio = true)
	{
		Vector2 position = aiScript.transform.parent.transform.position;
		string text = ProjectileAudioLibrary.GetMultiShotAudioPath(projectileName);
		if (useSingleShotAudio)
		{
			text = ProjectileAudioLibrary.GetSingleShotAudioPath(projectileName);
		}
		if (text != string.Empty)
		{
			AudioManager.PlayDelayedOneShot(null, text, position, -1f);
		}
	}
}
