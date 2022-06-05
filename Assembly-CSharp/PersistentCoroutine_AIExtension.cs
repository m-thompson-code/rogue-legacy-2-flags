using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028A RID: 650
public static class PersistentCoroutine_AIExtension
{
	// Token: 0x060012A5 RID: 4773 RVA: 0x0000978D File Offset: 0x0000798D
	public static Coroutine RunPersistentCoroutine(this BaseAIScript aiScript, IEnumerator coroutine)
	{
		return aiScript.EnemyController.StartCoroutine(coroutine);
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0000979B File Offset: 0x0000799B
	public static void StopPersistentCoroutine(this BaseAIScript aiScript, IEnumerator coroutine)
	{
		if (coroutine != null)
		{
			aiScript.EnemyController.StopCoroutine(coroutine);
		}
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x000097AC File Offset: 0x000079AC
	public static void StopPersistentCoroutine(this BaseAIScript aiScript, Coroutine coroutine)
	{
		if (coroutine != null)
		{
			aiScript.EnemyController.StopCoroutine(coroutine);
		}
	}
}
