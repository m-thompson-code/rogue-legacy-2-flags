using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000157 RID: 343
public static class AIMessage_AIExtension
{
	// Token: 0x06000BC0 RID: 3008 RVA: 0x00023473 File Offset: 0x00021673
	public static IEnumerator ToDo(this BaseAIScript aiScript, string message, float waitTime = 0f)
	{
		bool isEditor = Application.isEditor;
		yield break;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0002347B File Offset: 0x0002167B
	public static IEnumerator DisplayMessage(this BaseAIScript aiScript, string message, float waitTime = 0f)
	{
		aiScript.DisplayMessage(message);
		if (waitTime < 0f)
		{
			waitTime = 0f;
		}
		if (AIMessage_AIExtension.m_waitForSecondsTable == null)
		{
			AIMessage_AIExtension.m_waitForSecondsTable = new Dictionary<float, WaitForSeconds>();
		}
		if (waitTime > 0f)
		{
			if (!AIMessage_AIExtension.m_waitForSecondsTable.ContainsKey(waitTime))
			{
				AIMessage_AIExtension.m_waitForSecondsTable.Add(waitTime, new WaitForSeconds(waitTime));
			}
			yield return AIMessage_AIExtension.m_waitForSecondsTable[waitTime];
		}
		yield break;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00023498 File Offset: 0x00021698
	public static void ToDo(this BaseAIScript aiScript, string message)
	{
		bool isEditor = Application.isEditor;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000234A0 File Offset: 0x000216A0
	public static void DisplayMessage(this BaseAIScript aiScript, string message)
	{
		Vector2 absPos = new Vector2(aiScript.EnemyController.Midpoint.x, aiScript.EnemyController.VisualBounds.max.y) + new Vector2(0f, 1f);
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.EquipmentOreCollected, message, absPos, null, TextAlignmentOptions.Center);
	}

	// Token: 0x0400102D RID: 4141
	private static Dictionary<float, WaitForSeconds> m_waitForSecondsTable;
}
