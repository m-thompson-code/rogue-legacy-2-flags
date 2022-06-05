using System;

// Token: 0x0200015B RID: 347
public static class FacePlayer_AIExtension
{
	// Token: 0x06000BC9 RID: 3017 RVA: 0x00023740 File Offset: 0x00021940
	public static void FaceTarget(this BaseAIScript aiScript)
	{
		if (aiScript.Target && ((!aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x > aiScript.gameObject.transform.parent.position.x) || (aiScript.EnemyController.CharacterCorgi.IsFacingRight && aiScript.Target.transform.position.x < aiScript.gameObject.transform.parent.position.x)))
		{
			aiScript.EnemyController.CharacterCorgi.Flip(false, false);
		}
	}
}
