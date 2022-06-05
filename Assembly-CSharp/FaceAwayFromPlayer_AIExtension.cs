using System;

// Token: 0x0200015A RID: 346
public static class FaceAwayFromPlayer_AIExtension
{
	// Token: 0x06000BC8 RID: 3016 RVA: 0x00023688 File Offset: 0x00021888
	public static void FaceAwayFromTarget(this BaseAIScript aiScript)
	{
		if (aiScript.Target && ((!aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x < aiScript.gameObject.transform.parent.position.x) || (aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x > aiScript.gameObject.transform.parent.position.x)))
		{
			aiScript.EnemyController.CharacterCorgi.Flip(false, false);
		}
	}
}
