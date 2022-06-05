using System;

// Token: 0x02000286 RID: 646
public static class FaceAwayFromPlayer_AIExtension
{
	// Token: 0x0600129F RID: 4767 RVA: 0x000822A0 File Offset: 0x000804A0
	public static void FaceAwayFromTarget(this BaseAIScript aiScript)
	{
		if (aiScript.Target && ((!aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x < aiScript.gameObject.transform.parent.position.x) || (aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x > aiScript.gameObject.transform.parent.position.x)))
		{
			aiScript.EnemyController.CharacterCorgi.Flip(false, false);
		}
	}
}
