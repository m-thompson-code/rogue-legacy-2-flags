using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class CreditsController : MonoBehaviour
{
	// Token: 0x060013C1 RID: 5057 RVA: 0x0003BF80 File Offset: 0x0003A180
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

	// Token: 0x060013C2 RID: 5058 RVA: 0x0003C080 File Offset: 0x0003A280
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

	// Token: 0x060013C3 RID: 5059 RVA: 0x0003C140 File Offset: 0x0003A340
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

	// Token: 0x060013C4 RID: 5060 RVA: 0x0003C1BC File Offset: 0x0003A3BC
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.2f);
		MusicManager.PlayMusic(SongID.Credits_ASITP, false, false);
		RewiredMapController.SetMap(GameInputMode.Window);
		yield break;
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x0003C1C4 File Offset: 0x0003A3C4
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

	// Token: 0x060013C6 RID: 5062 RVA: 0x0003C21A File Offset: 0x0003A41A
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

	// Token: 0x060013C7 RID: 5063 RVA: 0x0003C24A File Offset: 0x0003A44A
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

	// Token: 0x060013C8 RID: 5064 RVA: 0x0003C25C File Offset: 0x0003A45C
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

	// Token: 0x060013C9 RID: 5065 RVA: 0x0003C328 File Offset: 0x0003A528
	private void ConfirmSkipCredits()
	{
		base.StopAllCoroutines();
		this.m_scrollingComplete = true;
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.LoadNextScene(default(InputActionEventData));
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0003C360 File Offset: 0x0003A560
	private void CancelConfirmMenuSelection()
	{
		this.m_confirmMenuActive = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		RewiredMapController.SetMap(GameInputMode.Window);
		RewiredMapController.SetMapEnabled(GameInputMode.Window, true);
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x0003C380 File Offset: 0x0003A580
	private void InitializeLook(ClassType classType, bool useLeft)
	{
		CharacterData characterData = useLeft ? this.m_leftCharData : this.m_rightCharData;
		PlayerLookController lookObj = useLeft ? this.m_leftPlayer : this.m_rightPlayer;
		CharacterCreator.GenerateClass(classType, characterData);
		LookCreator.InitializeClassLook(characterData.ClassType, lookObj);
		LookCreator.InitializeCharacterLook(characterData, lookObj, true);
		LookCreator.DisableAllWeaponGeo(lookObj);
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x0003C3D2 File Offset: 0x0003A5D2
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

	// Token: 0x060013CD RID: 5069 RVA: 0x0003C3EF File Offset: 0x0003A5EF
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

	// Token: 0x060013CE RID: 5070 RVA: 0x0003C3FE File Offset: 0x0003A5FE
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

	// Token: 0x060013CF RID: 5071 RVA: 0x0003C40D File Offset: 0x0003A60D
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

	// Token: 0x060013D0 RID: 5072 RVA: 0x0003C41C File Offset: 0x0003A61C
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

	// Token: 0x060013D1 RID: 5073 RVA: 0x0003C42B File Offset: 0x0003A62B
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

	// Token: 0x060013D2 RID: 5074 RVA: 0x0003C43A File Offset: 0x0003A63A
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

	// Token: 0x060013D3 RID: 5075 RVA: 0x0003C449 File Offset: 0x0003A649
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

	// Token: 0x060013D4 RID: 5076 RVA: 0x0003C458 File Offset: 0x0003A658
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

	// Token: 0x060013D5 RID: 5077 RVA: 0x0003C467 File Offset: 0x0003A667
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

	// Token: 0x060013D6 RID: 5078 RVA: 0x0003C476 File Offset: 0x0003A676
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

	// Token: 0x060013D7 RID: 5079 RVA: 0x0003C485 File Offset: 0x0003A685
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

	// Token: 0x060013D8 RID: 5080 RVA: 0x0003C494 File Offset: 0x0003A694
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

	// Token: 0x04001396 RID: 5014
	public static bool IsEndingScroll;

	// Token: 0x04001397 RID: 5015
	private const float SCROLL_SPEED = 150f;

	// Token: 0x04001398 RID: 5016
	[SerializeField]
	private RectTransform m_creditsPanel;

	// Token: 0x04001399 RID: 5017
	[SerializeField]
	private GameObject m_finalLogo;

	// Token: 0x0400139A RID: 5018
	[SerializeField]
	private GameObject m_centrePoint;

	// Token: 0x0400139B RID: 5019
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x0400139C RID: 5020
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x0400139D RID: 5021
	[Header("Animations")]
	[SerializeField]
	private PlayerLookController m_leftPlayer;

	// Token: 0x0400139E RID: 5022
	[SerializeField]
	private PlayerLookController m_rightPlayer;

	// Token: 0x0400139F RID: 5023
	[SerializeField]
	private Animator m_pizzaGirl;

	// Token: 0x040013A0 RID: 5024
	[SerializeField]
	private CanvasGroup m_leftOverlay;

	// Token: 0x040013A1 RID: 5025
	[SerializeField]
	private CanvasGroup m_rightOverlay;

	// Token: 0x040013A2 RID: 5026
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x040013A3 RID: 5027
	private Action<InputActionEventData> m_loadNextScene;

	// Token: 0x040013A4 RID: 5028
	private Action<InputActionEventData> m_onCancelPressed;

	// Token: 0x040013A5 RID: 5029
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040013A6 RID: 5030
	private Action m_confirmSkipCredits;

	// Token: 0x040013A7 RID: 5031
	private Vector3 m_leftPlayerStartingPos;

	// Token: 0x040013A8 RID: 5032
	private Vector3 m_rightPlayerStartingPos;

	// Token: 0x040013A9 RID: 5033
	private CharacterData m_leftCharData;

	// Token: 0x040013AA RID: 5034
	private CharacterData m_rightCharData;

	// Token: 0x040013AB RID: 5035
	private bool m_scrollingComplete;

	// Token: 0x040013AC RID: 5036
	private int[] m_animArray;

	// Token: 0x040013AD RID: 5037
	private int m_animIndex;

	// Token: 0x040013AE RID: 5038
	private bool m_confirmMenuActive;

	// Token: 0x040013AF RID: 5039
	private float m_confirmMenuDelay;
}
