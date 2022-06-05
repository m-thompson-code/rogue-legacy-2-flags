using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x0200078A RID: 1930
public class WanderingMusicianController : BaseSpecialPropController
{
	// Token: 0x06003B18 RID: 15128 RVA: 0x00020720 File Offset: 0x0001E920
	protected override void Awake()
	{
		this.m_waitUntilSpawnControllerReady = new WaitUntil(() => WanderingMusicianSpawnController.IsInitialized);
		base.Awake();
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x000F2F94 File Offset: 0x000F1194
	public void RunWanderer()
	{
		if (!base.IsPropComplete)
		{
			this.PropComplete();
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddNonLocDialogue("Wanderer", "Hi, I'm the Wanderer!", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			return;
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddNonLocDialogue("Wanderer", "I already told you all I know...", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x00020752 File Offset: 0x0001E952
	private void OnEnable()
	{
		base.StartCoroutine(this.CheckForEnable());
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x00020761 File Offset: 0x0001E961
	private IEnumerator CheckForEnable()
	{
		yield return this.m_waitUntilSpawnControllerReady;
		if (base.Room != null)
		{
			if (WanderingMusicianSpawnController.BiomeControllerIndex == base.Room.BiomeControllerIndex && WanderingMusicianSpawnController.BiomeControllerType == base.Room.AppearanceBiomeType)
			{
				this.m_specialMusicIsPlaying = true;
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x00020770 File Offset: 0x0001E970
	protected override void OnDisable()
	{
		base.OnDisable();
		bool specialMusicIsPlaying = this.m_specialMusicIsPlaying;
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void InitializePooledPropOnEnter()
	{
	}

	// Token: 0x04002F0A RID: 12042
	private bool m_specialMusicIsPlaying;

	// Token: 0x04002F0B RID: 12043
	private WaitUntil m_waitUntilSpawnControllerReady;
}
