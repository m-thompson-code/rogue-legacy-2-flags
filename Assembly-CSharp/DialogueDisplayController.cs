using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class DialogueDisplayController : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06001D5F RID: 7519 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06001D60 RID: 7520 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x0000F277 File Offset: 0x0000D477
	protected virtual void Awake()
	{
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_stopDialogue = new Action(this.StopDialogue);
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x0000F298 File Offset: 0x0000D498
	public void DisplayDialogue()
	{
		base.StartCoroutine(this.DisplayDialogueCoroutine());
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x0000F2A7 File Offset: 0x0000D4A7
	protected virtual IEnumerator DisplayDialogueCoroutine()
	{
		if (this.m_npcController)
		{
			this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		}
		if (this.m_playerPositionObj)
		{
			yield return this.MovePlayerToPos();
		}
		this.StartDialogue();
		yield break;
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x0009C75C File Offset: 0x0009A95C
	protected virtual void StartDialogue()
	{
		bool flag = false;
		PlayerSaveFlag playerSaveFlag = this.m_spokenFlag;
		if (playerSaveFlag != PlayerSaveFlag.None && SaveManager.PlayerSaveData.GetFlag(playerSaveFlag))
		{
			flag = true;
		}
		bool flag2 = this.m_useLocID;
		string text = (!flag) ? this.m_dialogue : this.m_repeatedDialogue;
		string text2 = this.m_speaker;
		if (this.m_npcController)
		{
			text2 = NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType);
		}
		Prop component = base.GetComponent<Prop>();
		if (component && component.PropSpawnController)
		{
			DialogueDisplayOverride component2 = component.PropSpawnController.gameObject.GetComponent<DialogueDisplayOverride>();
			if (component2)
			{
				playerSaveFlag = component2.SpokenFlagOverride;
				flag = false;
				if (playerSaveFlag != PlayerSaveFlag.None && SaveManager.PlayerSaveData.GetFlag(playerSaveFlag))
				{
					flag = true;
				}
				flag2 = component2.UseLocIDOverride;
				text = ((!flag) ? component2.DialogueOverride : component2.RepeatedDialogueOverride);
				if (!string.IsNullOrEmpty(component2.SpeakerOverride))
				{
					text2 = component2.SpeakerOverride;
				}
			}
		}
		DialogueManager.StartNewNPCDialogue(this.m_npcController, NPCState.Idle);
		if (flag2)
		{
			DialogueManager.AddDialogue(text2, text, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, this.m_dialogueWindowStyle, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		else
		{
			DialogueManager.AddNonLocDialogue(text2, text, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, this.m_dialogueWindowStyle, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_stopDialogue);
		if (!flag && playerSaveFlag != PlayerSaveFlag.None)
		{
			SaveManager.PlayerSaveData.SetFlag(playerSaveFlag, true);
		}
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
	protected virtual void StopDialogue()
	{
		if (this.m_npcController)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0000F2D2 File Offset: 0x0000D4D2
	protected virtual IEnumerator MovePlayerToPos()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (this.m_playerPositionObj.transform.lossyScale.x > 0f)
		{
			PlayerManager.GetPlayerController().SetFacing(false);
		}
		else
		{
			PlayerManager.GetPlayerController().SetFacing(true);
		}
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x04001AB6 RID: 6838
	[SerializeField]
	protected bool m_useLocID = true;

	// Token: 0x04001AB7 RID: 6839
	[SerializeField]
	protected string m_dialogue;

	// Token: 0x04001AB8 RID: 6840
	[SerializeField]
	protected string m_repeatedDialogue;

	// Token: 0x04001AB9 RID: 6841
	[SerializeField]
	protected string m_speaker;

	// Token: 0x04001ABA RID: 6842
	[SerializeField]
	protected PlayerSaveFlag m_spokenFlag;

	// Token: 0x04001ABB RID: 6843
	[SerializeField]
	protected DialogueWindowStyle m_dialogueWindowStyle;

	// Token: 0x04001ABC RID: 6844
	[SerializeField]
	protected GameObject m_playerPositionObj;

	// Token: 0x04001ABD RID: 6845
	protected NPCController m_npcController;

	// Token: 0x04001ABE RID: 6846
	private Action m_stopDialogue;
}
