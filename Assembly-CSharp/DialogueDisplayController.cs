using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class DialogueDisplayController : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x0600144A RID: 5194 RVA: 0x0003DB50 File Offset: 0x0003BD50
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x0600144B RID: 5195 RVA: 0x0003DB53 File Offset: 0x0003BD53
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x0003DB56 File Offset: 0x0003BD56
	protected virtual void Awake()
	{
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_stopDialogue = new Action(this.StopDialogue);
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x0003DB77 File Offset: 0x0003BD77
	public void DisplayDialogue()
	{
		base.StartCoroutine(this.DisplayDialogueCoroutine());
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x0003DB86 File Offset: 0x0003BD86
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

	// Token: 0x0600144F RID: 5199 RVA: 0x0003DB98 File Offset: 0x0003BD98
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

	// Token: 0x06001450 RID: 5200 RVA: 0x0003DD07 File Offset: 0x0003BF07
	protected virtual void StopDialogue()
	{
		if (this.m_npcController)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x0003DD23 File Offset: 0x0003BF23
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

	// Token: 0x04001416 RID: 5142
	[SerializeField]
	protected bool m_useLocID = true;

	// Token: 0x04001417 RID: 5143
	[SerializeField]
	protected string m_dialogue;

	// Token: 0x04001418 RID: 5144
	[SerializeField]
	protected string m_repeatedDialogue;

	// Token: 0x04001419 RID: 5145
	[SerializeField]
	protected string m_speaker;

	// Token: 0x0400141A RID: 5146
	[SerializeField]
	protected PlayerSaveFlag m_spokenFlag;

	// Token: 0x0400141B RID: 5147
	[SerializeField]
	protected DialogueWindowStyle m_dialogueWindowStyle;

	// Token: 0x0400141C RID: 5148
	[SerializeField]
	protected GameObject m_playerPositionObj;

	// Token: 0x0400141D RID: 5149
	protected NPCController m_npcController;

	// Token: 0x0400141E RID: 5150
	private Action m_stopDialogue;
}
