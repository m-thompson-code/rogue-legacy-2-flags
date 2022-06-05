using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200055B RID: 1371
public class SkillTreeShop : MonoBehaviour, IRootObj, IAudioEventEmitter
{
	// Token: 0x17001259 RID: 4697
	// (get) Token: 0x06003259 RID: 12889 RVA: 0x000AA8EF File Offset: 0x000A8AEF
	public GameObject HubTownCastleParentObj
	{
		get
		{
			return this.m_castleParentObj;
		}
	}

	// Token: 0x1700125A RID: 4698
	// (get) Token: 0x0600325A RID: 12890 RVA: 0x000AA8F7 File Offset: 0x000A8AF7
	public bool IsSkillTreeOpen
	{
		get
		{
			return this.m_isSkillTreeOpen;
		}
	}

	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x0600325B RID: 12891 RVA: 0x000AA8FF File Offset: 0x000A8AFF
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x000AA907 File Offset: 0x000A8B07
	private void Awake()
	{
		this.m_skillTreeWindowClosed = new Action<object, EventArgs>(this.SkillTreeWindowClosed);
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x000AA91B File Offset: 0x000A8B1B
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x000AA92A File Offset: 0x000A8B2A
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x000AA939 File Offset: 0x000A8B39
	private void Start()
	{
		this.m_playerEnterPositionObj.SetActive(false);
		this.m_playerExitPositionObj.SetActive(false);
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x000AA954 File Offset: 0x000A8B54
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

	// Token: 0x06003261 RID: 12897 RVA: 0x000AA9C7 File Offset: 0x000A8BC7
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

	// Token: 0x06003262 RID: 12898 RVA: 0x000AA9D6 File Offset: 0x000A8BD6
	private IEnumerator MovePlayerToPosition(Vector3 playerPos)
	{
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(playerPos, true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x000AA9E8 File Offset: 0x000A8BE8
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

	// Token: 0x06003264 RID: 12900 RVA: 0x000AAAFD File Offset: 0x000A8CFD
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		this.EnableSkillTreeCastle(true);
		base.StartCoroutine(this.CloseSkillTreeCoroutine());
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x000AAB13 File Offset: 0x000A8D13
	private IEnumerator CloseSkillTreeCoroutine()
	{
		yield return null;
		yield return this.MovePlayerToPosition(this.m_playerExitPositionObj.transform.position);
		this.m_isSkillTreeOpen = false;
		yield break;
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x000AAB2A File Offset: 0x000A8D2A
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400278A RID: 10122
	[SerializeField]
	private GameObject m_playerEnterPositionObj;

	// Token: 0x0400278B RID: 10123
	[SerializeField]
	private GameObject m_playerExitPositionObj;

	// Token: 0x0400278C RID: 10124
	[SerializeField]
	private GameObject m_castleParentObj;

	// Token: 0x0400278D RID: 10125
	private bool m_isSkillTreeOpen;

	// Token: 0x0400278E RID: 10126
	private Action<object, EventArgs> m_skillTreeWindowClosed;
}
