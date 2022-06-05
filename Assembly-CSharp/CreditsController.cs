using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class CreditsController : MonoBehaviour
{
	// Token: 0x06001C72 RID: 7282 RVA: 0x000990C0 File Offset: 0x000972C0
	private void Awake()
	{
		this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
		this.m_loadNextScene = new Action<InputActionEventData>(this.LoadNextScene);
		this.m_onCancelPressed = new Action<InputActionEventData>(this.OnCancelPressed);
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmSkipCredits = new Action(this.ConfirmSkipCredits);
		this.m_leftCharData = new CharacterData();
		this.m_rightCharData = new CharacterData();
		this.m_rightPlayer.transform.SetLocalScaleX(Mathf.Abs(this.m_rightPlayer.transform.localScale.x) * -1f);
		this.m_pizzaGirl.gameObject.SetActive(false);
		LookCreator.DisableAllWeaponGeo(this.m_leftPlayer);
		LookCreator.DisableAllWeaponGeo(this.m_rightPlayer);
		this.m_animArray = new int[11];
		for (int i = 0; i < this.m_animArray.Length; i++)
		{
			this.m_animArray[i] = i;
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x000991C0 File Offset: 0x000973C0
	private void OnEnable()
	{
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = true;
		this.m_canvas.worldCamera = CameraController.UICamera;
		this.m_fastForwardObj.SetActive(false);
		RewiredMapController.SetMap(GameInputMode.Window);
		Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
		Rewired_RL.Player.AddInputEventDelegate(this.m_loadNextScene, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		Rewired_RL.Player.AddInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		base.StartCoroutine(this.CreditRoll());
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x00099280 File Offset: 0x00097480
	private void OnDisable()
	{
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = false;
		CreditsController.IsEndingScroll = false;
		if (!GameManager.IsApplicationClosing)
		{
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_loadNextScene, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			Rewired_RL.Player.RemoveInputEventDelegate(this.m_onCancelPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0000EB6B File Offset: 0x0000CD6B
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.2f);
		MusicManager.PlayMusic(SongID.Credits_ASITP, false, false);
		RewiredMapController.SetMap(GameInputMode.Window);
		yield break;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000992FC File Offset: 0x000974FC
	private void ToggleSpeedUp(InputActionEventData eventData)
	{
		if (this.m_confirmMenuActive)
		{
			return;
		}
		if (!this.m_scrollingComplete)
		{
			if (!eventData.GetButtonUp())
			{
				RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 10f);
				this.m_fastForwardObj.SetActive(true);
				return;
			}
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
			this.m_fastForwardObj.SetActive(false);
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x0000EB73 File Offset: 0x0000CD73
	private void LoadNextScene(InputActionEventData eventData)
	{
		if (this.m_confirmMenuActive)
		{
			return;
		}
		if (this.m_scrollingComplete)
		{
			if (!CreditsController.IsEndingScroll)
			{
				WorldBuilder.FirstBiomeOverride = BiomeType.None;
				SceneLoader_RL.LoadScene(SceneID.MainMenu, TransitionID.FadeToBlackNoLoading);
				return;
			}
			SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.FadeToBlackWithLoading);
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x0000EBA3 File Offset: 0x0000CDA3
	private IEnumerator CreditRoll()
	{
		this.m_confirmMenuDelay = Time.time + 2.5f;
		this.m_scrollingComplete = false;
		Vector3 pos = this.m_creditsPanel.transform.localPosition;
		while (this.m_finalLogo.transform.position.y < this.m_centrePoint.transform.position.y)
		{
			float num = Time.deltaTime * 150f;
			pos.y += num;
			this.m_creditsPanel.localPosition = pos;
			yield return null;
		}
		float endDelay = Time.time + 3f;
		while (Time.time < endDelay)
		{
			yield return null;
		}
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
		this.m_scrollingComplete = true;
		yield break;
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x00099354 File Offset: 0x00097554
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
		confirmMenuWindowController.SetTitleText("LOC_ID_SYSTEM_MESSAGE_SKIP_CREDITS_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_SYSTEM_MESSAGE_SKIP_CREDITS_TEXT_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		confirmMenuWindowController.SetStartingSelectedButton(1);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmSkipCredits);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00099420 File Offset: 0x00097620
	private void ConfirmSkipCredits()
	{
		base.StopAllCoroutines();
		this.m_scrollingComplete = true;
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.LoadNextScene(default(InputActionEventData));
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x0000EBB2 File Offset: 0x0000CDB2
	private void CancelConfirmMenuSelection()
	{
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		RewiredMapController.SetMap(GameInputMode.Window);
		RewiredMapController.SetMapEnabled(GameInputMode.Window, true);
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00099458 File Offset: 0x00097658
	private void InitializeLook(ClassType classType, bool useLeft)
	{
		CharacterData characterData = useLeft ? this.m_leftCharData : this.m_rightCharData;
		PlayerLookController lookObj = useLeft ? this.m_leftPlayer : this.m_rightPlayer;
		CharacterCreator.GenerateClass(classType, characterData);
		LookCreator.InitializeClassLook(characterData.ClassType, lookObj);
		LookCreator.InitializeCharacterLook(characterData, lookObj, true);
		LookCreator.DisableAllWeaponGeo(lookObj);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x0000EBD2 File Offset: 0x0000CDD2
	private IEnumerator PlayAnimation(Animator animator, int layerIndex, params string[] states)
	{
		foreach (string stateName in states)
		{
			animator.Play(stateName, layerIndex);
			yield return null;
			while (animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1f)
			{
				yield return null;
			}
		}
		string[] array = null;
		yield break;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x0000EBEF File Offset: 0x0000CDEF
	private IEnumerator StartPlayerAnimations()
	{
		CDGHelper.Shuffle<int>(this.m_animArray);
		this.m_animIndex = 0;
		float delay = UnityEngine.Random.Range(8f, 12f);
		delay += Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayerStartingPos = this.m_leftPlayer.transform.position;
		this.m_rightPlayerStartingPos = this.m_rightPlayer.transform.position;
		while (this.m_finalLogo.transform.position.y < this.m_centrePoint.transform.position.y)
		{
			switch (this.m_animArray[this.m_animIndex])
			{
			case 0:
				yield return this.BowAnimation();
				break;
			case 1:
				yield return this.TwirlAnimation();
				break;
			case 2:
				yield return this.TwirlUpAnimation();
				break;
			case 3:
				yield return this.FireballAnimation();
				break;
			case 4:
				yield return this.RandomDeathAnimation();
				break;
			case 5:
				yield return this.RetireAnimation();
				break;
			case 6:
				yield return this.LookDistanceAnimation();
				break;
			case 7:
				yield return this.KickDownAnimation();
				break;
			case 8:
				yield return this.BowFartAnimation();
				break;
			case 9:
				yield return this.BoxingAnimation();
				break;
			case 10:
				yield return this.EatPizza();
				break;
			}
			this.m_animIndex++;
			delay = (float)UnityEngine.Random.Range(5, 8);
			delay += Time.time;
			while (Time.time < delay)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x0000EBFE File Offset: 0x0000CDFE
	private IEnumerator BowAnimation()
	{
		this.InitializeLook(ClassType.SwordClass, true);
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_leftPlayer.Animator.Play("Idle");
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.Play("Bow");
		delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x0000EC0D File Offset: 0x0000CE0D
	private IEnumerator TwirlAnimation()
	{
		this.InitializeLook(ClassType.LuteClass, false);
		this.m_leftPlayer.transform.position = new Vector3(-10000f, 0f, this.m_leftPlayerStartingPos.z);
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 0f;
		this.m_rightOverlay.alpha = 1f;
		this.m_rightPlayer.Animator.SetBool("Bounce", true);
		this.m_rightPlayer.Animator.Play("Bounce");
		this.m_rightPlayer.Animator.speed = 0.5f;
		TweenManager.TweenBy(this.m_rightPlayer.transform, 3f, new EaseDelegate(Ease.None), new object[]
		{
			"position.x",
			-25
		});
		TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_rightPlayer.Animator.SetBool("Bounce", false);
		this.m_rightPlayer.Animator.speed = 1f;
		yield break;
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x0000EC1C File Offset: 0x0000CE1C
	private IEnumerator TwirlUpAnimation()
	{
		this.InitializeLook(ClassType.LuteClass, false);
		this.m_leftPlayer.transform.position = new Vector3(-10000f, 0f, this.m_leftPlayerStartingPos.z);
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_rightPlayer.transform.SetPositionY(this.m_rightPlayerStartingPos.y - 10f);
		this.m_leftOverlay.alpha = 0f;
		this.m_rightOverlay.alpha = 1f;
		this.m_rightPlayer.Animator.SetBool("Bounce", true);
		this.m_rightPlayer.Animator.Play("Bounce");
		this.m_rightPlayer.Animator.speed = 0.5f;
		TweenManager.TweenBy(this.m_rightPlayer.transform, 3f, new EaseDelegate(Ease.None), new object[]
		{
			"position.y",
			25
		});
		TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_rightPlayer.Animator.SetBool("Bounce", false);
		this.m_rightPlayer.Animator.speed = 1f;
		yield break;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x0000EC2B File Offset: 0x0000CE2B
	private IEnumerator FireballAnimation()
	{
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.Play("SpellCast_Tell_Intro");
		yield return null;
		while (this.m_leftPlayer.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.Play("SpellCast_Tell_Hold");
		delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.Play("SpellCast_Attack_Intro");
		yield return null;
		while (this.m_leftPlayer.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.Play("SpellCast_Attack_Hold");
		yield return null;
		while (this.m_leftPlayer.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
		{
			yield return null;
		}
		yield return null;
		this.m_leftPlayer.Animator.speed = 0f;
		delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 position = this.m_leftPlayer.transform.position;
		position.x += 1.5f;
		position.y += 1f;
		EffectManager.PlayEffect(this.m_leftPlayer.gameObject, null, "Fart_Effect", position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).gameObject.SetLayerRecursively(5, false);
		delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.speed = 1f;
		this.m_leftPlayer.Animator.Play("Empty", 1);
		this.m_leftPlayer.Animator.Play("Death_6");
		delay = Time.time + 2.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x0000EC3A File Offset: 0x0000CE3A
	private IEnumerator RandomDeathAnimation()
	{
		this.InitializeLook(ClassType.AxeClass, false);
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		float num = (float)UnityEngine.Random.Range(0, 12);
		this.m_rightPlayer.Animator.Play("Death_" + num.ToString());
		delay = Time.time + 3f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x0000EC49 File Offset: 0x0000CE49
	private IEnumerator RetireAnimation()
	{
		this.InitializeLook(ClassType.AxeClass, false);
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_rightPlayer.Animator.SetBool("Retire", true);
		this.m_rightPlayer.Animator.Play("Player_Retire");
		delay = Time.time + 3f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_rightPlayer.Animator.SetBool("Retire", false);
		yield break;
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x0000EC58 File Offset: 0x0000CE58
	private IEnumerator LookDistanceAnimation()
	{
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_leftPlayer.Animator.SetBool("LookDistance", true);
		this.m_leftPlayer.Animator.Play("Player_LookDistance_Intro");
		delay = Time.time + 3f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_leftPlayer.Animator.SetBool("LookDistance", false);
		yield break;
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x0000EC67 File Offset: 0x0000CE67
	private IEnumerator KickDownAnimation()
	{
		this.m_rightPlayer.transform.position = new Vector3(-10000f, 0f, this.m_rightPlayerStartingPos.z);
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_leftPlayer.transform.SetPositionY(this.m_leftPlayerStartingPos.y + 20f);
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		this.m_leftPlayer.Animator.SetBool("Kick_Down", true);
		this.m_leftPlayer.Animator.Play("Kick_Down");
		TweenManager.TweenBy(this.m_leftPlayer.transform, 3f, new EaseDelegate(Ease.None), new object[]
		{
			"position.y",
			-40
		});
		TweenManager.TweenTo(this.m_leftOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_leftPlayer.Animator.SetBool("Kick_Down", false);
		yield break;
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x0000EC76 File Offset: 0x0000CE76
	private IEnumerator BowFartAnimation()
	{
		this.InitializeLook(ClassType.SwordClass, true);
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.Animator.Play("Idle");
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_rightPlayer.Animator.Play("Bow");
		delay = Time.time + 0.8f;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 position = this.m_rightPlayer.transform.position;
		position.x += 1f;
		position.y += 0.5f;
		BaseEffect baseEffect = EffectManager.PlayEffect(this.m_rightPlayer.gameObject, null, "Fart_Effect", position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.gameObject.SetLayerRecursively(5, false);
		baseEffect.transform.SetLocalScaleX(baseEffect.transform.localScale.x * -1f);
		TweenManager.TweenBy(this.m_rightPlayer.transform, 0.5f, new EaseDelegate(Ease.Quint.EaseOut), new object[]
		{
			"position.x",
			-1
		});
		delay = Time.time + 1.2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x0000EC85 File Offset: 0x0000CE85
	private IEnumerator BoxingAnimation()
	{
		this.m_leftPlayer.transform.position = this.m_leftPlayerStartingPos;
		this.m_rightPlayer.transform.position = this.m_rightPlayerStartingPos;
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_rightPlayer.Animator.speed = 2f;
		yield return this.PlayAnimation(this.m_rightPlayer.Animator, 1, new string[]
		{
			"Punch1_Tell_Intro",
			"Punch1_Tell_Hold",
			"Punch1_Attack_Intro",
			"Punch1_Attack_Hold",
			"Punch1_Exit"
		});
		yield return this.PlayAnimation(this.m_rightPlayer.Animator, 1, new string[]
		{
			"Punch1_Tell_Intro",
			"Punch1_Tell_Hold",
			"Punch1_Attack_Intro",
			"Punch1_Attack_Hold",
			"Punch1_Exit"
		});
		yield return this.PlayAnimation(this.m_rightPlayer.Animator, 1, new string[]
		{
			"Punch2_Tell_Intro",
			"Punch2_Tell_Hold",
			"Punch2_Attack_Intro",
			"Punch2_Attack_Hold",
			"Punch2_Exit"
		});
		yield return this.PlayAnimation(this.m_rightPlayer.Animator, 1, new string[]
		{
			"Punch3_Tell_Intro",
			"Punch3_Tell_Hold",
			"Punch3_Attack_Intro",
			"Punch3_Attack_Hold",
			"Punch3_Exit"
		});
		yield return this.PlayAnimation(this.m_rightPlayer.Animator, 1, new string[]
		{
			"Uppercut_Tell_Intro",
			"Uppercut_Tell_Hold",
			"Uppercut_Attack_Intro",
			"Uppercut_Attack_Hold",
			"Uppercut_Exit"
		});
		this.m_rightPlayer.Animator.speed = 0f;
		delay = Time.time + 1.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_rightPlayer.Animator.speed = 1f;
		this.m_rightPlayer.Animator.Play("Empty", 1);
		yield break;
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x0000EC94 File Offset: 0x0000CE94
	private IEnumerator EatPizza()
	{
		this.m_rightPlayer.gameObject.SetActive(false);
		this.m_leftPlayer.gameObject.SetActive(false);
		this.m_pizzaGirl.gameObject.SetActive(true);
		this.m_leftOverlay.alpha = 1f;
		this.m_rightOverlay.alpha = 1f;
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_pizzaGirl.Play("EatPizza");
		delay = Time.time + 4f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_rightOverlay, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_rightPlayer.gameObject.SetActive(true);
		this.m_leftPlayer.gameObject.SetActive(true);
		this.m_pizzaGirl.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040019F2 RID: 6642
	public static bool IsEndingScroll;

	// Token: 0x040019F3 RID: 6643
	private const float SCROLL_SPEED = 150f;

	// Token: 0x040019F4 RID: 6644
	[SerializeField]
	private RectTransform m_creditsPanel;

	// Token: 0x040019F5 RID: 6645
	[SerializeField]
	private GameObject m_finalLogo;

	// Token: 0x040019F6 RID: 6646
	[SerializeField]
	private GameObject m_centrePoint;

	// Token: 0x040019F7 RID: 6647
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x040019F8 RID: 6648
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x040019F9 RID: 6649
	[Header("Animations")]
	[SerializeField]
	private PlayerLookController m_leftPlayer;

	// Token: 0x040019FA RID: 6650
	[SerializeField]
	private PlayerLookController m_rightPlayer;

	// Token: 0x040019FB RID: 6651
	[SerializeField]
	private Animator m_pizzaGirl;

	// Token: 0x040019FC RID: 6652
	[SerializeField]
	private CanvasGroup m_leftOverlay;

	// Token: 0x040019FD RID: 6653
	[SerializeField]
	private CanvasGroup m_rightOverlay;

	// Token: 0x040019FE RID: 6654
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x040019FF RID: 6655
	private Action<InputActionEventData> m_loadNextScene;

	// Token: 0x04001A00 RID: 6656
	private Action<InputActionEventData> m_onCancelPressed;

	// Token: 0x04001A01 RID: 6657
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001A02 RID: 6658
	private Action m_confirmSkipCredits;

	// Token: 0x04001A03 RID: 6659
	private Vector3 m_leftPlayerStartingPos;

	// Token: 0x04001A04 RID: 6660
	private Vector3 m_rightPlayerStartingPos;

	// Token: 0x04001A05 RID: 6661
	private CharacterData m_leftCharData;

	// Token: 0x04001A06 RID: 6662
	private CharacterData m_rightCharData;

	// Token: 0x04001A07 RID: 6663
	private bool m_scrollingComplete;

	// Token: 0x04001A08 RID: 6664
	private int[] m_animArray;

	// Token: 0x04001A09 RID: 6665
	private int m_animIndex;

	// Token: 0x04001A0A RID: 6666
	private bool m_confirmMenuActive;

	// Token: 0x04001A0B RID: 6667
	private float m_confirmMenuDelay;
}
