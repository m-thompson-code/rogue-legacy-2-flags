using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public class HallwayRoomController : BaseSpecialRoomController
{
	// Token: 0x0600426B RID: 17003 RVA: 0x00024C60 File Offset: 0x00022E60
	protected override void Awake()
	{
		base.Awake();
		this.m_displayNextMemory = new Action(this.DisplayNextMemory);
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x0010AA94 File Offset: 0x00108C94
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

	// Token: 0x0600426D RID: 17005 RVA: 0x00024C7A File Offset: 0x00022E7A
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

	// Token: 0x0600426E RID: 17006 RVA: 0x00024C89 File Offset: 0x00022E89
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

	// Token: 0x0600426F RID: 17007 RVA: 0x0010AB1C File Offset: 0x00108D1C
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

	// Token: 0x06004270 RID: 17008 RVA: 0x0010AB64 File Offset: 0x00108D64
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

	// Token: 0x06004271 RID: 17009 RVA: 0x0010ABBC File Offset: 0x00108DBC
	private void DisplayNextMemory()
	{
		this.m_memoryProps[this.m_memoryIndex].PropInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.RemoveListener(this.m_displayNextMemory);
		if (this.m_memoryIndex < this.m_memoryProps.Length - 1)
		{
			this.m_memoryIndex++;
			this.SetMemoryActive(this.m_memoryIndex);
		}
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x00024C91 File Offset: 0x00022E91
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

	// Token: 0x040033F7 RID: 13303
	[SerializeField]
	private TunnelSpawnController m_closetTunnelSpawner;

	// Token: 0x040033F8 RID: 13304
	[SerializeField]
	private PropSpawnController[] m_memoryProps;

	// Token: 0x040033F9 RID: 13305
	private Action m_displayNextMemory;

	// Token: 0x040033FA RID: 13306
	private int m_memoryIndex;
}
