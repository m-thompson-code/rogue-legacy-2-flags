using System;

// Token: 0x02000287 RID: 647
public static class FacePlayer_AIExtension
{
	// Token: 0x060012A0 RID: 4768 RVA: 0x00082358 File Offset: 0x00080558
	public static void FaceTarget(this BaseAIScript aiScript)
	{
		if (aiScript.Target && ((!aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x > aiScript.gameObject.transform.parent.position.x) || (aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x < aiScript.gameObject.transform.parent.position.x)))
		{
			aiScript.EnemyController.CharacterCorgi.Flip(false, false);
		}
	}
}
