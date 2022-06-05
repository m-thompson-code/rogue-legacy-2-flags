using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class AboveGroundSkillTreeShop : MonoBehaviour, IRootObj, IAudioEventEmitter
{
	// Token: 0x1700122D RID: 4653
	// (get) Token: 0x06003146 RID: 12614 RVA: 0x000A6D43 File Offset: 0x000A4F43
	// (set) Token: 0x06003147 RID: 12615 RVA: 0x000A6D4B File Offset: 0x000A4F4B
	public bool IsSkillTreeOpen { get; private set; }

	// Token: 0x1700122E RID: 4654
	// (get) Token: 0x06003148 RID: 12616 RVA: 0x000A6D54 File Offset: 0x000A4F54
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x000A6D5C File Offset: 0x000A4F5C
	private void Awake()
	{
		this.m_skillTreeWindowClosed = new Action<object, EventArgs>(this.SkillTreeWindowClosed);
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x000A6D70 File Offset: 0x000A4F70
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x000A6D7F File Offset: 0x000A4F7F
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x000A6D8E File Offset: 0x000A4F8E
	public void OpenSkillTree()
	{
		if (!this.IsSkillTreeOpen)
		{
			this.IsSkillTreeOpen = true;
			base.StartCoroutine(this.OpenSkillTreeCoroutine());
		}
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x000A6DAC File Offset: 0x000A4FAC
	private IEnumerator OpenSkillTreeCoroutine()
	{
		yield return this.MovePlayerToPosition(this.m_playerEnterPositionObj.transform.position);
		if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
		{
			WindowManager.LoadWindow(WindowID.SkillTree);
		}
		AudioManager.PlayOneShot(this, "event:/UI/FrontEnd/ui_fe_docks_transition_toSkilltree", default(Vector3));
		WindowManager.SetWindowIsOpen(WindowID.SkillTree, true, TransitionID.QuickSwipe);
		yield break;
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x000A6DBB File Offset: 0x000A4FBB
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		base.StartCoroutine(this.CloseSkillTreeCoroutine());
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x000A6DCA File Offset: 0x000A4FCA
	private IEnumerator CloseSkillTreeCoroutine()
	{
		yield return null;
		PlayerManager.GetPlayerController().transform.position = this.m_playerEnterPositionObj.transform.position;
		PlayerManager.GetPlayerController().ControllerCorgi.SetRaysParameters();
		PlayerManager.GetPlayerController().ControllerCorgi.ResetState();
		yield return this.MovePlayerToPosition(this.m_playerExitPositionObj.transform.position);
		this.IsSkillTreeOpen = false;
		yield break;
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x000A6DD9 File Offset: 0x000A4FD9
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000A6DF0 File Offset: 0x000A4FF0
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040026DF RID: 9951
	[SerializeField]
	private GameObject m_playerEnterPositionObj;

	// Token: 0x040026E0 RID: 9952
	[SerializeField]
	private GameObject m_playerExitPositionObj;

	// Token: 0x040026E1 RID: 9953
	private Action<object, EventArgs> m_skillTreeWindowClosed;
}
