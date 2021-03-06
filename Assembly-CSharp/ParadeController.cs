using System;
using System.Collections;
using Cinemachine;
using FMOD.Studio;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class ParadeController : MonoBehaviour
{
	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00052369 File Offset: 0x00050569
	public CinemachineVirtualCamera VirtualCamera
	{
		get
		{
			return this.m_vcam;
		}
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x00052374 File Offset: 0x00050574
	private void Awake()
	{
		this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
		this.m_onCancelPressed = new Action<InputActionEventData>(this.OnCancelPressed);
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmSkipParade = new Action(this.ConfirmSkipParade);
		this.m_actionArray = new IParadeAction[this.m_actionOrderArray.Length][];
		for (int i = 0; i < this.m_actionOrderArray.Length; i++)
		{
			ParadeActionController componentInChildren = this.m_actionOrderArray[i].ActionObj.GetComponentInChildren<ParadeActionController>();
			if (componentInChildren)
			{
				this.m_actionArray[i] = new IParadeAction[componentInChildren.transform.childCount];
				for (int j = 0; j < this.m_actionArray[i].Length; j++)
				{
					IParadeAction component = componentInChildren.transform.GetChild(j).gameObject.GetComponent<IParadeAction>();
					if (!component.IsNativeNull())
					{
						this.m_actionArray[i][j] = component;
					}
				}
			}
		}
		Renderer[] componentsInChildren = base.gameObject.FindObjectReference("FinalBossBlack").GetComponentsInChildren<Renderer>();
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		foreach (Renderer renderer in componentsInChildren)
		{
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat("_TextureBlend", 1f);
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
		if (!this.m_fastForwardObj.transform.parent.gameObject.activeSelf)
		{
			this.m_fastForwardObj.transform.parent.gameObject.SetActive(true);
		}
		this.m_fastForwardObj.SetActive(false);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000524FC File Offset: 0x000506FC
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.2f);
		RewiredMapController.SetMap(GameInputMode.Window);
		MusicManager.PlayMusic(SongID.Parade_ASITP_Videri_Enemy_Parade, false, false);
		yield break;
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x00052504 File Offset: 0x00050704
	private void OnEnable()
	{
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = true;
		RewiredMapController.SetMap(GameInputMode.Window);
		Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
		Rewired_RL.Player.AddInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		if (!this.m_ambienceEventInstance.isValid())
		{
			this.m_ambienceEventInstance = AudioUtility.GetEventInstance("event:/Environment/amb_eyeballBoss", CameraController.UICamera.transform);
		}
		if (!this.m_rootMoveEventInstance.isValid())
		{
			this.m_rootMoveEventInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_credits_roots_move_loop", CameraController.UICamera.transform);
		}
		if (this.m_ambienceEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_ambienceEventInstance);
		}
		if (this.m_rootMoveEventInstance.isValid())
		{
			this.m_rootMoveEventInstance.setParameterByName("creditsRootMove", 0f, true);
			AudioManager.Play(null, this.m_rootMoveEventInstance);
		}
		base.StartCoroutine(this.BeginParade());
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00052608 File Offset: 0x00050808
	private void OnDisable()
	{
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = false;
		if (this.m_ambienceEventInstance.isValid())
		{
			AudioManager.Stop(this.m_ambienceEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_rootMoveEventInstance.isValid())
		{
			AudioManager.Stop(this.m_rootMoveEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (!GameManager.IsApplicationClosing)
		{
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x00052699 File Offset: 0x00050899
	private void OnDestroy()
	{
		if (this.m_ambienceEventInstance.isValid())
		{
			this.m_ambienceEventInstance.release();
		}
		if (this.m_rootMoveEventInstance.isValid())
		{
			this.m_rootMoveEventInstance.release();
		}
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x000526D0 File Offset: 0x000508D0
	private void ToggleSpeedUp(InputActionEventData eventData)
	{
		if (this.m_confirmMenuActive)
		{
			return;
		}
		if (!eventData.GetButtonUp())
		{
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 15f);
			this.m_fastForwardObj.SetActive(true);
			return;
		}
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0005271E File Offset: 0x0005091E
	private IEnumerator BeginParade()
	{
		this.m_confirmMenuDelay = Time.time + 4.5f;
		Vector3 localPosition = this.m_startingCamPos.localPosition;
		localPosition.z = this.m_vcam.transform.position.z;
		this.m_vcam.transform.position = localPosition;
		int num;
		for (int i = 0; i < this.m_actionArray.Length; i = num + 1)
		{
			for (int j = 0; j < this.m_actionArray[i].Length; j = num + 1)
			{
				IParadeAction paradeAction = this.m_actionArray[i][j];
				if (!paradeAction.IsNativeNull())
				{
					yield return paradeAction.TriggerAction(this);
				}
				num = j;
			}
			if (this.m_actionOrderArray[i].ExitDelay > 0f)
			{
				float nextActionDelay = Mathf.Min(this.m_actionOrderArray[i].ExitDelay, 3f);
				nextActionDelay += Time.time;
				while (Time.time < nextActionDelay)
				{
					yield return null;
				}
			}
			num = i;
		}
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SeenParade, true);
		float fadeOutDelay = Time.time + 3f;
		while (Time.time < fadeOutDelay)
		{
			yield return null;
		}
		CreditsController.IsEndingScroll = true;
		SceneLoader_RL.LoadScene(SceneID.Credits, TransitionID.FadeToBlackNoLoading);
		yield break;
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x0005272D File Offset: 0x0005092D
	public void SetRootMoveAudioParam(bool moving)
	{
		if (this.m_rootMoveEventInstance.isValid())
		{
			this.m_rootMoveEventInstance.setParameterByName("creditsRootMove", moving ? 1f : 0f, false);
		}
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x00052760 File Offset: 0x00050960
	private void OnCancelPressed(InputActionEventData obj)
	{
		if (Time.time < this.m_confirmMenuDelay)
		{
			return;
		}
		this.m_confirmMenuActive = true;
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SYSTEM_MESSAGE_SKIP_PARADE_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_SYSTEM_MESSAGE_SKIP_PARADE_TEXT_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		confirmMenuWindowController.SetStartingSelectedButton(1);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmSkipParade);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x0005282A File Offset: 0x00050A2A
	private void ConfirmSkipParade()
	{
		base.StopAllCoroutines();
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SeenParade, true);
		CreditsController.IsEndingScroll = true;
		SceneLoader_RL.LoadScene(SceneID.Credits, TransitionID.FadeToBlackNoLoading);
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x0005285F File Offset: 0x00050A5F
	private void CancelConfirmMenuSelection()
	{
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		RewiredMapController.SetMap(GameInputMode.Window);
		RewiredMapController.SetMapEnabled(GameInputMode.Window, true);
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x00052880 File Offset: 0x00050A80
	public void CreateEnemyPortrait(EnemyController enemyController)
	{
		if (!enemyController)
		{
			return;
		}
		Transform transform = enemyController.transform.FindDeep("Visuals");
		if (transform && transform.childCount > 0)
		{
			ParadePortrait component = UnityEngine.Object.Instantiate<GameObject>(this.m_portraitPrefab.gameObject, base.transform).GetComponent<ParadePortrait>();
			component.EnemyType = enemyController.EnemyType;
			component.EnemyRank = enemyController.EnemyRank;
			component.name = enemyController.EnemyType.ToString() + " " + enemyController.EnemyRank.ToString() + " Portrait";
			component.NameText.text = enemyController.EnemyType.ToString() + " " + enemyController.EnemyRank.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.GetChild(0).gameObject, component.transform);
			float scale = EnemyClassLibrary.GetEnemyData(enemyController.EnemyType, enemyController.EnemyRank).Scale;
			float x = gameObject.transform.localScale.x;
			gameObject.transform.localScale = new Vector3(scale, scale, scale) * x;
			Animator component2 = enemyController.GetComponent<Animator>();
			component.Animator.runtimeAnimatorController = component2.runtimeAnimatorController;
			component.Animator.avatar = component2.avatar;
			component.Model = gameObject;
			return;
		}
		Debug.Log(string.Concat(new string[]
		{
			"<color=red>ERROR: Could not create enemy of type: ",
			enemyController.EnemyType.ToString(),
			".",
			enemyController.EnemyRank.ToString(),
			" because Visuals child GO not found.</color>"
		}));
	}

	// Token: 0x0400188A RID: 6282
	[SerializeField]
	private Transform m_startingCamPos;

	// Token: 0x0400188B RID: 6283
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x0400188C RID: 6284
	[SerializeField]
	private CinemachineVirtualCamera m_vcam;

	// Token: 0x0400188D RID: 6285
	[SerializeField]
	private ParadePortrait m_portraitPrefab;

	// Token: 0x0400188E RID: 6286
	[SerializeField]
	private ParadeController.ParadeActionOrderEntry[] m_actionOrderArray;

	// Token: 0x0400188F RID: 6287
	private IParadeAction[][] m_actionArray;

	// Token: 0x04001890 RID: 6288
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x04001891 RID: 6289
	private Action<InputActionEventData> m_onCancelPressed;

	// Token: 0x04001892 RID: 6290
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001893 RID: 6291
	private Action m_confirmSkipParade;

	// Token: 0x04001894 RID: 6292
	private EventInstance m_ambienceEventInstance;

	// Token: 0x04001895 RID: 6293
	private EventInstance m_rootMoveEventInstance;

	// Token: 0x04001896 RID: 6294
	private bool m_confirmMenuActive;

	// Token: 0x04001897 RID: 6295
	private float m_confirmMenuDelay;

	// Token: 0x02000B4A RID: 2890
	[Serializable]
	private class ParadeActionOrderEntry
	{
		// Token: 0x04004BFA RID: 19450
		public GameObject ActionObj;

		// Token: 0x04004BFB RID: 19451
		public float ExitDelay;
	}
}
