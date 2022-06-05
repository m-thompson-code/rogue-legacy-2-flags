using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015E RID: 350
public static class PersistentCoroutine_AIExtension
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x00023905 File Offset: 0x00021B05
	public static Coroutine RunPersistentCoroutine(this BaseAIScript aiScript, IEnumerator coroutine)
	{
		return aiScript.EnemyController.StartCoroutine(coroutine);
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00023913 File Offset: 0x00021B13
	public static void StopPersistentCoroutine(this BaseAIScript aiScript, IEnumerator coroutine)
	{
		if (coroutine != null)
		{
			aiScript.EnemyController.StopCoroutine(coroutine);
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00023924 File Offset: 0x00021B24
	public static void StopPersistentCoroutine(this BaseAIScript aiScript, Coroutine coroutine)
	{
		if (coroutine != null)
		{
			aiScript.EnemyController.StopCoroutine(coroutine);
		}
	}
}
