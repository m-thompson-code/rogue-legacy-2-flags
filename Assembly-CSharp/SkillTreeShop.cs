using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200091F RID: 2335
public class SkillTreeShop : MonoBehaviour, IRootObj, IAudioEventEmitter
{
	// Token: 0x17001906 RID: 6406
	// (get) Token: 0x060046E6 RID: 18150 RVA: 0x00026E64 File Offset: 0x00025064
	public GameObject HubTownCastleParentObj
	{
		get
		{
			return this.m_castleParentObj;
		}
	}

	// Token: 0x17001907 RID: 6407
	// (get) Token: 0x060046E7 RID: 18151 RVA: 0x00026E6C File Offset: 0x0002506C
	public bool IsSkillTreeOpen
	{
		get
		{
			return this.m_isSkillTreeOpen;
		}
	}

	// Token: 0x17001908 RID: 6408
	// (get) Token: 0x060046E8 RID: 18152 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x00026E74 File Offset: 0x00025074
	private void Awake()
	{
		this.m_skillTreeWindowClosed = new Action<object, EventArgs>(this.SkillTreeWindowClosed);
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x00026E88 File Offset: 0x00025088
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x00026E97 File Offset: 0x00025097
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x00026EA6 File Offset: 0x000250A6
	private void Start()
	{
		this.m_playerEnterPositionObj.SetActive(false);
		this.m_playerExitPositionObj.SetActive(false);
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x00114DF0 File Offset: 0x00112FF0
	public void OpenSkillTree(bool animate)
	{
		if (!this.m_isSkillTreeOpen)
		{
			this.m_isSkillTreeOpen = true;
			if (animate)
			{
				base.StartCoroutine(this.OpenSkillTreeCoroutine());
				return;
			}
			PlayerManager.GetPlayerController().gameObject.transform.position = this.m_playerEnterPositionObj.transform.position;
			AudioManager.PlayOneShot(this, "event:/UI/FrontEnd/ui_fe_castle_open", default(Vector3));
			WindowManager.SetWindowIsOpen(WindowID.SkillTree, true);
			this.EnableSkillTreeCastle(false);
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x00026EC0 File Offset: 0x000250C0
	private IEnumerator OpenSkillTreeCoroutine()
	{
		yield return this.MovePlayerToPosition(this.m_playerEnterPositionObj.transform.position);
		AudioManager.PlayOneShot(this, "event:/UI/FrontEnd/ui_fe_docks_transition_toSkilltree", default(Vector3));
		WindowManager.SetWindowIsOpen(WindowID.SkillTree, true, TransitionID.QuickSwipe);
		while (WindowManager.ActiveWindow == null)
		{
			yield return null;
		}
		this.EnableSkillTreeCastle(false);
		yield break;
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x00026ECF File Offset: 0x000250CF
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x00114E64 File Offset: 0x00113064
	private void EnableSkillTreeCastle(bool enableInHubTown)
	{
		if (!enableInHubTown)
		{
			if (this.HubTownCastleParentObj.transform.childCount > 0)
			{
				if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
				{
					WindowManager.LoadWindow(WindowID.SkillTree);
				}
				WindowController windowController = WindowManager.GetWindowController(WindowID.SkillTree);
				GameObject gameObject = this.HubTownCastleParentObj.transform.GetChild(0).gameObject;
				SkillTreeWindowController skillTreeWindowController = windowController as SkillTreeWindowController;
				gameObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.SetParent(skillTreeWindowController.SkillTreeCastleParentObj.transform, false);
				gameObject.SetLayerRecursively(5, false);
				return;
			}
		}
		else
		{
			WindowController windowController2 = WindowManager.GetWindowController(WindowID.SkillTree);
			if (windowController2)
			{
				GameObject gameObject2 = (windowController2 as SkillTreeWindowController).SkillTreeCastleParentObj.transform.GetChild(0).gameObject;
				gameObject2.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.transform.SetParent(this.HubTownCastleParentObj.transform, false);
				gameObject2.SetLayerRecursively(26, false);
			}
		}
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x00026EDE File Offset: 0x000250DE
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		this.EnableSkillTreeCastle(true);
		base.StartCoroutine(this.CloseSkillTreeCoroutine());
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x00026EF4 File Offset: 0x000250F4
	private IEnumerator CloseSkillTreeCoroutine()
	{
		yield return null;
		yield return this.MovePlayerToPosition(this.m_playerExitPositionObj.transform.position);
		this.m_isSkillTreeOpen = false;
		yield break;
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003691 RID: 13969
	[SerializeField]
	private GameObject m_playerEnterPositionObj;

	// Token: 0x04003692 RID: 13970
	[SerializeField]
	private GameObject m_playerExitPositionObj;

	// Token: 0x04003693 RID: 13971
	[SerializeField]
	private GameObject m_castleParentObj;

	// Token: 0x04003694 RID: 13972
	private bool m_isSkillTreeOpen;

	// Token: 0x04003695 RID: 13973
	private Action<object, EventArgs> m_skillTreeWindowClosed;
}
