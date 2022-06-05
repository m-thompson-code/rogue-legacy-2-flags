using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000809 RID: 2057
public class EggplantFoodPropController : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x1700170A RID: 5898
	// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000230A4 File Offset: 0x000212A4
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.EggplantFood) <= 0;
		}
	}

	// Token: 0x1700170B RID: 5899
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x00004A8D File Offset: 0x00002C8D
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x06003F68 RID: 16232 RVA: 0x000FD9F8 File Offset: 0x000FBBF8
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

	// Token: 0x06003F69 RID: 16233 RVA: 0x000230BB File Offset: 0x000212BB
	private void GiveFood()
	{
		base.StartCoroutine(this.GiveFoodCoroutine());
	}

	// Token: 0x06003F6A RID: 16234 RVA: 0x000230CA File Offset: 0x000212CA
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
