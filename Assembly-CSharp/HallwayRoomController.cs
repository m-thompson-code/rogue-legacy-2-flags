using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000503 RID: 1283
public class HallwayRoomController : BaseSpecialRoomController
{
	// Token: 0x06002FEB RID: 12267 RVA: 0x000A3FD7 File Offset: 0x000A21D7
	protected override void Awake()
	{
		base.Awake();
		this.m_displayNextMemory = new Action(this.DisplayNextMemory);
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x000A3FF4 File Offset: 0x000A21F4
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		base.StartCoroutine(this.FlipPlayer());
		base.StartCoroutine(this.FixTunnelLayer());
		foreach (PropSpawnController propSpawnController in this.m_memoryProps)
		{
			propSpawnController.PropInstance.Pivot.gameObject.SetActive(false);
			propSpawnController.PropInstance.GetComponent<StudioEventEmitter>().Stop();
		}
		this.m_memoryIndex = 0;
		this.SetMemoriesEnabled();
		base.StartCoroutine(this.StartEarthShiftChallengeCoroutine());
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x000A407A File Offset: 0x000A227A
	private IEnumerator FixTunnelLayer()
	{
		yield return null;
		SpriteRenderer componentInChildren = this.m_closetTunnelSpawner.Tunnel.GetComponentInChildren<SpriteRenderer>();
		componentInChildren.gameObject.layer = 24;
		Vector3 localPosition = componentInChildren.transform.localPosition;
		localPosition.z = 5f;
		componentInChildren.transform.localPosition = localPosition;
		yield break;
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x000A4089 File Offset: 0x000A2289
	private IEnumerator StartEarthShiftChallengeCoroutine()
	{
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		bool readingMemory = WindowManager.GetIsWindowOpen(WindowID.Dialogue);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		if (readingMemory)
		{
			delay = Time.time + 0.5f;
			while (Time.time < delay)
			{
				yield return null;
			}
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_pizzaGirl_greeting", CameraController.GameCamera.transform.position);
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_TELLING_PLAYER_TO_FOLLOW_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		yield return null;
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ResetGlobalTimer, null, null);
		GlobalTimerHUDController.ReverseTimer = true;
		GlobalTimerHUDController.ReverseStartTime = 600f;
		GlobalTimerHUDController.TrackNegativeTimeAchievement = true;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StartGlobalTimer, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayGlobalTimer, null, null);
		yield break;
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x000A4094 File Offset: 0x000A2294
	private void SetMemoriesEnabled()
	{
		if (BurdenManager.IsBurdenActive(BurdenType.FinalBossUp) && !this.m_memoryProps.IsNativeNull() && this.m_memoryProps.Length != 0)
		{
			for (int i = 0; i <= this.m_memoryIndex; i++)
			{
				this.SetMemoryActive(i);
			}
		}
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x000A40DC File Offset: 0x000A22DC
	private void SetMemoryActive(int index)
	{
		Prop propInstance = this.m_memoryProps[index].PropInstance;
		propInstance.Pivot.gameObject.SetActive(true);
		propInstance.GetComponent<StudioEventEmitter>().Play();
		if (index == this.m_memoryIndex)
		{
			propInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.AddListener(this.m_displayNextMemory, false);
		}
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x000A4134 File Offset: 0x000A2334
	private void DisplayNextMemory()
	{
		this.m_memoryProps[this.m_memoryIndex].PropInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.RemoveListener(this.m_displayNextMemory);
		if (this.m_memoryIndex < this.m_memoryProps.Length - 1)
		{
			this.m_memoryIndex++;
			this.SetMemoryActive(this.m_memoryIndex);
		}
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x000A4195 File Offset: 0x000A2395
	private IEnumerator FlipPlayer()
	{
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (!playerController.IsFacingRight)
		{
			playerController.CharacterCorgi.Flip(true, true);
		}
		yield break;
	}

	// Token: 0x04002633 RID: 9779
	[SerializeField]
	private TunnelSpawnController m_closetTunnelSpawner;

	// Token: 0x04002634 RID: 9780
	[SerializeField]
	private PropSpawnController[] m_memoryProps;

	// Token: 0x04002635 RID: 9781
	private Action m_displayNextMemory;

	// Token: 0x04002636 RID: 9782
	private int m_memoryIndex;
}
