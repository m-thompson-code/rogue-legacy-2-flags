using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200027F RID: 639
public static class AIMessage_AIExtension
{
	// Token: 0x0600127F RID: 4735 RVA: 0x000096D8 File Offset: 0x000078D8
	public static IEnumerator ToDo(this BaseAIScript aiScript, string message, float waitTime = 0f)
	{
		bool isEditor = Application.isEditor;
		yield break;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x000096E0 File Offset: 0x000078E0
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

	// Token: 0x06001281 RID: 4737 RVA: 0x000096FD File Offset: 0x000078FD
	public static void ToDo(this BaseAIScript aiScript, string message)
	{
		bool isEditor = Application.isEditor;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00081F70 File Offset: 0x00080170
	public static void DisplayMessage(this BaseAIScript aiScript, string message)
	{
		Vector2 absPos = new Vector2(aiScript.EnemyController.Midpoint.x, aiScript.EnemyController.VisualBounds.max.y) + new Vector2(0f, 1f);
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.EquipmentOreCollected, message, absPos, null, TextAlignmentOptions.Center);
	}

	// Token: 0x0400151A RID: 5402
	private static Dictionary<float, WaitForSeconds> m_waitForSecondsTable;
}
