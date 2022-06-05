using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200015F RID: 351
public static class PlayProjectileSFX_AIExtension
{
	// Token: 0x06000BD1 RID: 3025 RVA: 0x00023938 File Offset: 0x00021B38
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
