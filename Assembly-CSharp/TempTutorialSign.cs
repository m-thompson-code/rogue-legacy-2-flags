using System;
using UnityEngine;

// Token: 0x02000929 RID: 2345
public class TempTutorialSign : MonoBehaviour
{
	// Token: 0x06004736 RID: 18230 RVA: 0x00027113 File Offset: 0x00025313
	public void ActivateTutorialSign()
	{
		bool isInstantiated = PlayerManager.IsInstantiated;
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x00115554 File Offset: 0x00113754
	private void DisplaySwordTutorial()
	{
		string speaker = "Sword Tutorial - Demo";
		string text = "[Attack] - Slash.  **You can move while swinging.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x0011557C File Offset: 0x0011377C
	private void DisplaySaberTutorial()
	{
		string speaker = "Saber Tutorial - Demo";
		string text = "(Ground) [Attack] - Lunge</LB>(Air) [Attack] - Cut. **Cuts can combo into Lunges if timed correctly.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x001155A4 File Offset: 0x001137A4
	private void DisplayAxeTutorial()
	{
		string speaker = "Axe Tutorial - Demo";
		string text = "(Ground) [Attack] - Crescent Strike</LB>(Air) [Attack] - Lunar Orbit into (Landing) Lunar Landing</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x001155CC File Offset: 0x001137CC
	private void DisplayBowTutorial()
	{
		string speaker = "Bow Tutorial - Demo";
		string text = "[Attack] (Hold) - Draw.  Release to fire. Requires Ammo</LB>(Air) [MoveVertical-]+[Jump] - Spinkick **kicking stuff restores ammo.";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x001155F4 File Offset: 0x001137F4
	private void DisplayMageTutorial()
	{
		string speaker = "Mage Tutorial";
		string text = "You are a master of magic.  No weapon, but you regenerate mana automatically.</LB>(Air) [MoveVertical-]+[Jump] - Spinkick";
		DialogueManager.AddNonLocDialogue(speaker, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}
}
