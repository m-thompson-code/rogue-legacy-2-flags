using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public abstract class BaseShop : MonoBehaviour, IDisplaySpeechBubble, IRootObj
{
	// Token: 0x17001233 RID: 4659
	// (get) Token: 0x06003172 RID: 12658 RVA: 0x000A75DE File Offset: 0x000A57DE
	public virtual bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17001234 RID: 4660
	// (get) Token: 0x06003173 RID: 12659 RVA: 0x000A75E6 File Offset: 0x000A57E6
	public virtual SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x06003174 RID: 12660
	protected abstract bool HasEventDialogue();

	// Token: 0x06003175 RID: 12661 RVA: 0x000A75E9 File Offset: 0x000A57E9
	protected virtual void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_npcController = base.GetComponentInChildren<NPCController>();
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x000A7603 File Offset: 0x000A5803
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000A7605 File Offset: 0x000A5805
	protected virtual void OnDisable()
	{
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000A7607 File Offset: 0x000A5807
	public virtual void OpenShop()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(false);
		}
		base.StartCoroutine(this.OpenShopCoroutine());
	}

	// Token: 0x06003179 RID: 12665
	protected abstract void OpenShop_Internal();

	// Token: 0x0600317A RID: 12666 RVA: 0x000A7630 File Offset: 0x000A5830
	public virtual void CloseShop()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
		if (this.m_npcController && this.m_npcController.CurrentState != NPCState.Idle)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x000A767E File Offset: 0x000A587E
	protected virtual IEnumerator OpenShopCoroutine()
	{
		if (this.m_greetingAudioEmitter)
		{
			this.m_greetingAudioEmitter.Play();
		}
		if (this.m_npcController)
		{
			this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		}
		if (this.m_playerPositionObj)
		{
			yield return this.MovePlayerToPos();
		}
		this.OpenShop_Internal();
		yield break;
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x000A768D File Offset: 0x000A588D
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

	// Token: 0x0600317E RID: 12670 RVA: 0x000A76A4 File Offset: 0x000A58A4
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040026FD RID: 9981
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040026FE RID: 9982
	[SerializeField]
	private StudioEventEmitter m_greetingAudioEmitter;

	// Token: 0x040026FF RID: 9983
	protected Interactable m_interactable;

	// Token: 0x04002700 RID: 9984
	protected NPCController m_npcController;
}
