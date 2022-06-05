using System;
using System.Collections;

// Token: 0x02000282 RID: 642
public static class ChangeAnimationState_AIExtension
{
	// Token: 0x0600128F RID: 4751 RVA: 0x00009733 File Offset: 0x00007933
	public static IEnumerator ChangeAnimationState(this BaseAIScript aiScript, string animParamName)
	{
		aiScript.Animator.Play(animParamName, aiScript.DefaultAnimationLayer);
		yield break;
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00009749 File Offset: 0x00007949
	public static IEnumerator ChangeAnimationState(this BaseAIScript aiScript, int animParamID)
	{
		aiScript.Animator.Play(animParamID, aiScript.DefaultAnimationLayer);
		yield break;
	}
}
