using System;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class TempTutorialSign : MonoBehaviour
{
	// Token: 0x0600327F RID: 12927 RVA: 0x000AAE62 File Offset: 0x000A9062
	public void ActivateTutorialSign()
	{
		bool isInstantiated = PlayerManager.IsInstantiated;
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x000AAE6C File Offset: 0x000A906C
	private void DisplaySwordTutorial()
	{
		string speaker = "Sword Tutorial - Demo";
		string text = "[Attack] - Slash.  **You can move while swinging.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000AAE94 File Offset: 0x000A9094
	private void DisplaySaberTutorial()
	{
		string speaker = "Saber Tutorial - Demo";
		string text = "(Ground) [Attack] - Lunge</LB>(Air) [Attack] - Cut. **Cuts can combo into Lunges if timed correctly.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000AAEBC File Offset: 0x000A90BC
	private void DisplayAxeTutorial()
	{
		string speaker = "Axe Tutorial - Demo";
		string text = "(Ground) [Attack] - Crescent Strike</LB>(Air) [Attack] - Lunar Orbit into (Landing) Lunar Landing</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x000AAEE4 File Offset: 0x000A90E4
	private void DisplayBowTutorial()
	{
		string speaker = "Bow Tutorial - Demo";
		string text = "[Attack] (Hold) - Draw.  Release to fire. Requires Ammo</LB>(Air) [MoveVertical-]+[Jump] - Spinkick **kicking stuff restores ammo.";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000AAF0C File Offset: 0x000A910C
	private void DisplayMageTutorial()
	{
		string speaker = "Mage Tutorial";
		string text = "You are a master of magic.  No weapon, but you regenerate mana automatically.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}
}
