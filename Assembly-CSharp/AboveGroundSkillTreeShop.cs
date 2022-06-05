using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class AboveGroundSkillTreeShop : MonoBehaviour, IRootObj, IAudioEventEmitter
{
	// Token: 0x17001892 RID: 6290
	// (get) Token: 0x060044FA RID: 17658 RVA: 0x00025E44 File Offset: 0x00024044
	// (set) Token: 0x060044FB RID: 17659 RVA: 0x00025E4C File Offset: 0x0002404C
	public bool IsSkillTreeOpen { get; private set; }

	// Token: 0x17001893 RID: 6291
	// (get) Token: 0x060044FC RID: 17660 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x00025E55 File Offset: 0x00024055
	private void Awake()
	{
		this.m_skillTreeWindowClosed = new Action<object, EventArgs>(this.SkillTreeWindowClosed);
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x00025E69 File Offset: 0x00024069
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x060044FF RID: 17663 RVA: 0x00025E78 File Offset: 0x00024078
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x00025E87 File Offset: 0x00024087
	public void OpenSkillTree()
	{
		if (!this.IsSkillTreeOpen)
		{
			this.IsSkillTreeOpen = true;
			base.StartCoroutine(this.OpenSkillTreeCoroutine());
		}
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x00025EA5 File Offset: 0x000240A5
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

	// Token: 0x06004502 RID: 17666 RVA: 0x00025EB4 File Offset: 0x000240B4
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		base.StartCoroutine(this.CloseSkillTreeCoroutine());
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x00025EC3 File Offset: 0x000240C3
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

	// Token: 0x06004504 RID: 17668 RVA: 0x00025ED2 File Offset: 0x000240D2
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	private GameObject m_playerEnterPositionObj;

	// Token: 0x0400356B RID: 13675
	[SerializeField]
	private GameObject m_playerExitPositionObj;

	// Token: 0x0400356C RID: 13676
	private Action<object, EventArgs> m_skillTreeWindowClosed;
}
