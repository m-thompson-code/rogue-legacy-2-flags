using System;
using System.Collections;

// Token: 0x02000158 RID: 344
public static class ChangeAnimationState_AIExtension
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x000234FF File Offset: 0x000216FF
	public static IEnumerator ChangeAnimationState(this BaseAIScript aiScript, string animParamName)
	{
		aiScript.Animator.Play(animParamName, aiScript.DefaultAnimationLayer);
		yield break;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00023515 File Offset: 0x00021715
	public static IEnumerator ChangeAnimationState(this BaseAIScript aiScript, int animParamID)
	{
		aiScript.Animator.Play(animParamID, aiScript.DefaultAnimationLayer);
		yield break;
	}
}
