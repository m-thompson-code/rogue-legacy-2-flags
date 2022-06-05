using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000576 RID: 1398
public class DeathDefyWindowController : WindowController
{
	// Token: 0x1700127F RID: 4735
	// (get) Token: 0x06003367 RID: 13159 RVA: 0x000ADF70 File Offset: 0x000AC170
	// (set) Token: 0x06003368 RID: 13160 RVA: 0x000ADF78 File Offset: 0x000AC178
	public DeathDefiedType DeathDefiedType { get; set; }

	// Token: 0x17001280 RID: 4736
	// (get) Token: 0x06003369 RID: 13161 RVA: 0x000ADF81 File Offset: 0x000AC181
	public override WindowID ID
	{
		get
		{
			return WindowID.DeathDefy;
		}
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x000ADF85 File Offset: 0x000AC185
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, true);
		this.m_hudArgs = new PlayerHUDVisibilityEventArgs(0.5f);
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x000ADFA8 File Offset: 0x000AC1A8
	protected override void OnOpen()
	{
		CameraController.CinemachineBrain.enabled = false;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, this.m_hudArgs);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x000ADFE1 File Offset: 0x000AC1E1
	private IEnumerator OnOpenCoroutine()
	{
		this.m_snapshotEventEmitter.Play();
		this.m_deathLoopEventEmitter.Play();
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			(TraitManager.GetActiveTrait(TraitType.UpsideDown) as UpsideDown_Trait).ApplyDeathDefy(false);
		}
		PlayerController player = PlayerManager.GetPlayerController();
		Vector3 localScale = this.m_banner.transform.localScale;
		localScale.x = 0f;
		localScale.y = 0f;
		this.m_banner.transform.localScale = localScale;
		RectTransform component = this.m_windowCanvas.GetComponent<RectTransform>();
		Vector3 vector = CameraController.GameCamera.WorldToScreenPoint(player.transform.position);
		vector = new Vector3(vector.x / this.m_windowCanvas.scaleFactor, vector.y / this.m_windowCanvas.scaleFactor, vector.z / this.m_windowCanvas.scaleFactor);
		vector -= new Vector3(component.sizeDelta.x / 2f, component.sizeDelta.y / 2f, 0f);
		this.m_bannerCanvasGroup.alpha = 1f;
		this.m_spotlightCanvasGroup.transform.localPosition = vector;
		this.m_spotlightCanvasGroup.alpha = 0f;
		RewiredMapController.SetCurrentMapEnabled(false);
		player.Animator.SetBool("Stunned", true);
		player.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		player.CharacterHitResponse.AnimateHitEffectsOnUnscaledTime = true;
		this.m_visualsTransformList.Clear();
		this.m_visualsLayerMaskList.Clear();
		player.Visuals.GetComponentsInChildren<Transform>(this.m_visualsTransformList);
		for (int i = 0; i < this.m_visualsTransformList.Count; i++)
		{
			this.m_visualsLayerMaskList.Add(this.m_visualsTransformList[i].gameObject.layer);
		}
		Vector3 position = CameraController.GameCamera.transform.position;
		position.z = CameraController.SoloCam.transform.position.z;
		CameraController.SoloCam.transform.position = position;
		CameraController.SoloCam.gameObject.SetActive(true);
		CameraController.SoloCam.AddToCameraLayer(player.Visuals);
		CameraController.SoloCam.Camera.orthographicSize = CameraController.GameCamera.orthographicSize;
		yield return null;
		this.RevivePlayer(player);
		this.m_waitYield.CreateNew(0.5f, true);
		yield return this.m_waitYield;
		this.m_bgCanvasGroup.alpha = 0f;
		this.m_bgCanvasGroup.gameObject.SetActive(true);
		Vector3 position2 = CameraController.GameCamera.transform.position;
		position2.z = this.m_bgCanvasGroup.transform.position.z;
		this.m_bgCanvasGroup.transform.position = position2;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield return new WaitForSecondsRealtime(0.25f);
		player.Animator.SetBool("Stunned", false);
		int num = Player_EV.PLAYER_DEATH_ANIM_OVERRIDE;
		if (num == -1 && (TraitManager.IsTraitActive(TraitType.Fart) || TraitManager.IsTraitActive(TraitType.SuperFart)))
		{
			num = 9;
		}
		if (SaveManager.PlayerSaveData.IsInHeirloom)
		{
			num = 8;
		}
		if (num > -1 && num < 12)
		{
			player.Animator.SetTrigger("Death" + num.ToString());
		}
		else
		{
			int num2 = UnityEngine.Random.Range(0, 12);
			while (num2 == 9 || num2 == 8)
			{
				num2 = UnityEngine.Random.Range(0, 12);
			}
			player.Animator.SetTrigger("Death" + num2.ToString());
		}
		yield return null;
		while (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(1f, true);
		yield return this.m_waitYield;
		player.SetHealth(player.CurrentHealth, false, true);
		EffectManager.PlayEffect(player.gameObject, player.Animator, "PlayerDeathDefyStart_Effect", player.transform.localPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_waitYield.CreateNew(0.5f, true);
		yield return this.m_waitYield;
		this.m_reverseLoopEventEmitter.Play();
		player.Animator.SetFloat("Ability_Anim_Speed", -3f);
		while (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
		{
			yield return null;
		}
		this.m_reverseLoopEventEmitter.Stop();
		EffectManager.PlayEffect(player.gameObject, player.Animator, "PlayerDeathDefyEnd_Effect", player.transform.localPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		player.Animator.SetBool("DeathDefied", true);
		player.Animator.SetBool("Victory", true);
		player.Animator.SetFloat("Ability_Anim_Speed", 1f);
		TweenManager.TweenTo_UnscaledTime(this.m_spotlightCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_banner.transform, 0.5f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(1f, true);
		yield return this.m_waitYield;
		base.StartCoroutine(this.OnCloseCoroutine());
		yield break;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x000ADFF0 File Offset: 0x000AC1F0
	private void RevivePlayer(PlayerController player)
	{
		switch (this.DeathDefiedType)
		{
		case DeathDefiedType.Death_Dodge:
			player.SetHealth(1f, false, false);
			break;
		case DeathDefiedType.ExtraLife:
		{
			float num = (float)player.ActualMaxHealth * 0.5f;
			num += 0f * player.ActualMagic;
			player.SetHealth(num, false, false);
			break;
		}
		case DeathDefiedType.ExtraLife_Unity:
		{
			float value = (float)player.ActualMaxHealth * 0.5f;
			player.SetHealth(value, false, false);
			break;
		}
		}
		PlayerHUDController.IgnoreHealthChangeEvents = true;
		SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		PlayerHUDController.IgnoreHealthChangeEvents = false;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x000AE081 File Offset: 0x000AC281
	private IEnumerator OnCloseCoroutine()
	{
		PlayerController player = PlayerManager.GetPlayerController();
		player.SetVelocity(0f, 0f, false);
		TweenManager.TweenTo_UnscaledTime(this.m_spotlightCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo_UnscaledTime(this.m_bannerCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		for (int i = 0; i < this.m_visualsTransformList.Count; i++)
		{
			this.m_visualsTransformList[i].gameObject.layer = this.m_visualsLayerMaskList[i];
		}
		player.GetComponentInChildren<CharacterSortController>().ResetCharacterLayers();
		player.Animator.updateMode = AnimatorUpdateMode.Normal;
		player.Animator.SetBool("Victory", false);
		player.Animator.SetBool("DeathDefied", false);
		player.CharacterHitResponse.AnimateHitEffectsOnUnscaledTime = false;
		player.BlinkPulseEffect.UseUnscaledTime = false;
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			(TraitManager.GetActiveTrait(TraitType.UpsideDown) as UpsideDown_Trait).ApplyDeathDefy(true);
		}
		this.m_deathLoopEventEmitter.Stop();
		this.m_snapshotEventEmitter.Stop();
		WindowManager.SetWindowIsOpen(WindowID.DeathDefy, false);
		yield break;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x000AE090 File Offset: 0x000AC290
	protected override void OnFocus()
	{
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x000AE092 File Offset: 0x000AC292
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x000AE094 File Offset: 0x000AC294
	protected override void OnClose()
	{
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		CameraController.CinemachineBrain.enabled = true;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, this.m_hudArgs);
		this.m_windowCanvas.gameObject.SetActive(false);
		CameraController.SoloCam.gameObject.SetActive(false);
	}

	// Token: 0x04002824 RID: 10276
	[SerializeField]
	private Image m_banner;

	// Token: 0x04002825 RID: 10277
	[SerializeField]
	private CanvasGroup m_bannerCanvasGroup;

	// Token: 0x04002826 RID: 10278
	[SerializeField]
	private CanvasGroup m_spotlightCanvasGroup;

	// Token: 0x04002827 RID: 10279
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04002828 RID: 10280
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04002829 RID: 10281
	[SerializeField]
	private StudioEventEmitter m_snapshotEventEmitter;

	// Token: 0x0400282A RID: 10282
	[SerializeField]
	private StudioEventEmitter m_deathLoopEventEmitter;

	// Token: 0x0400282B RID: 10283
	[SerializeField]
	private StudioEventEmitter m_reverseLoopEventEmitter;

	// Token: 0x0400282C RID: 10284
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400282D RID: 10285
	private PlayerHUDVisibilityEventArgs m_hudArgs;

	// Token: 0x0400282E RID: 10286
	private List<Transform> m_visualsTransformList = new List<Transform>();

	// Token: 0x0400282F RID: 10287
	private List<LayerMask> m_visualsLayerMaskList = new List<LayerMask>();
}
