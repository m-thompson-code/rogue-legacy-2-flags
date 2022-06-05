using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class EggplantFoodPropController : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x17001169 RID: 4457
	// (get) Token: 0x06002E0C RID: 11788 RVA: 0x0009B5E7 File Offset: 0x000997E7
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.EggplantFood) <= 0;
		}
	}

	// Token: 0x1700116A RID: 4458
	// (get) Token: 0x06002E0D RID: 11789 RVA: 0x0009B5FE File Offset: 0x000997FE
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x0009B604 File Offset: 0x00099804
	public void TriggerEggplantFoodDialogue()
	{
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_EGGPLANT_TEXT_HEIRLOOM_BOX_TITLE_1", "LOC_ID_EGGPLANT_TEXT_HEIRLOOM_BOX_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.EggplantFood) <= 0)
		{
			DialogueManager.AddDialogueCompleteEndHandler(new Action(this.GiveFood));
		}
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x0009B668 File Offset: 0x00099868
	private void GiveFood()
	{
		base.StartCoroutine(this.GiveFoodCoroutine());
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x0009B677 File Offset: 0x00099877
	private IEnumerator GiveFoodCoroutine()
	{
		RewiredMapController.SetIsInCutscene(true);
		SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.EggplantFood, 1, false, true);
		SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Animator.SetBool("Victory", true);
		delay = Time.time + 0.75f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		HeirloomDrop heirloomDrop = new HeirloomDrop(HeirloomType.EggplantFood);
		(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(heirloomDrop);
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		playerController.Animator.SetBool("Victory", false);
		delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		RewiredMapController.SetIsInCutscene(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}
}
