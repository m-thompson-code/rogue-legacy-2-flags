using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class WanderingMusicianController : BaseSpecialPropController
{
	// Token: 0x06002AE4 RID: 10980 RVA: 0x00091487 File Offset: 0x0008F687
	protected override void Awake()
	{
		this.m_waitUntilSpawnControllerReady = new WaitUntil(() => WanderingMusicianSpawnController.IsInitialized);
		base.Awake();
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000914BC File Offset: 0x0008F6BC
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

	// Token: 0x06002AE6 RID: 10982 RVA: 0x00091524 File Offset: 0x0008F724
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x00091526 File Offset: 0x0008F726
	private void OnEnable()
	{
		base.StartCoroutine(this.CheckForEnable());
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x00091535 File Offset: 0x0008F735
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

	// Token: 0x06002AE9 RID: 10985 RVA: 0x00091544 File Offset: 0x0008F744
	protected override void OnDisable()
	{
		base.OnDisable();
		bool specialMusicIsPlaying = this.m_specialMusicIsPlaying;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x00091553 File Offset: 0x0008F753
	protected override void InitializePooledPropOnEnter()
	{
	}

	// Token: 0x04002304 RID: 8964
	private bool m_specialMusicIsPlaying;

	// Token: 0x04002305 RID: 8965
	private WaitUntil m_waitUntilSpawnControllerReady;
}
