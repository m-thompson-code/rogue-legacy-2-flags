using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public abstract class BaseShop : MonoBehaviour, IDisplaySpeechBubble, IRootObj
{
	// Token: 0x170018A4 RID: 6308
	// (get) Token: 0x0600454A RID: 17738 RVA: 0x000260C6 File Offset: 0x000242C6
	public virtual bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x170018A5 RID: 6309
	// (get) Token: 0x0600454B RID: 17739 RVA: 0x000047A7 File Offset: 0x000029A7
	public virtual SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x0600454C RID: 17740
	protected abstract bool HasEventDialogue();

	// Token: 0x0600454D RID: 17741 RVA: 0x000260CE File Offset: 0x000242CE
	protected virtual void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_npcController = base.GetComponentInChildren<NPCController>();
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnEnable()
	{
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnDisable()
	{
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x000260E8 File Offset: 0x000242E8
	public virtual void OpenShop()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(false);
		}
		base.StartCoroutine(this.OpenShopCoroutine());
	}

	// Token: 0x06004551 RID: 17745
	protected abstract void OpenShop_Internal();

	// Token: 0x06004552 RID: 17746 RVA: 0x001109DC File Offset: 0x0010EBDC
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

	// Token: 0x06004553 RID: 17747 RVA: 0x00026110 File Offset: 0x00024310
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

	// Token: 0x06004554 RID: 17748 RVA: 0x0002611F File Offset: 0x0002431F
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

	// Token: 0x06004556 RID: 17750 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400359C RID: 13724
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400359D RID: 13725
	[SerializeField]
	private StudioEventEmitter m_greetingAudioEmitter;

	// Token: 0x0400359E RID: 13726
	protected Interactable m_interactable;

	// Token: 0x0400359F RID: 13727
	protected NPCController m_npcController;
}
