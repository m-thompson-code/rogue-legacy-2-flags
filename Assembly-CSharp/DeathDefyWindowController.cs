using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200094D RID: 2381
public class DeathDefyWindowController : WindowController
{
	// Token: 0x17001952 RID: 6482
	// (get) Token: 0x06004866 RID: 18534 RVA: 0x00027C87 File Offset: 0x00025E87
	// (set) Token: 0x06004867 RID: 18535 RVA: 0x00027C8F File Offset: 0x00025E8F
	public DeathDefiedType DeathDefiedType { get; set; }

	// Token: 0x17001953 RID: 6483
	// (get) Token: 0x06004868 RID: 18536 RVA: 0x00007B8D File Offset: 0x00005D8D
	public override WindowID ID
	{
		get
		{
			return WindowID.DeathDefy;
		}
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x00027C98 File Offset: 0x00025E98
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, true);
		this.m_hudArgs = new PlayerHUDVisibilityEventArgs(0.5f);
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x00027CBB File Offset: 0x00025EBB
	protected override void OnOpen()
	{
		CameraController.CinemachineBrain.enabled = false;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, this.m_hudArgs);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x00027CF4 File Offset: 0x00025EF4
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

	// Token: 0x0600486C RID: 18540 RVA: 0x0011875C File Offset: 0x0011695C
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

	// Token: 0x0600486D RID: 18541 RVA: 0x00027D03 File Offset: 0x00025F03
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

	// Token: 0x0600486E RID: 18542 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnFocus()
	{
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x001187F0 File Offset: 0x001169F0
	protected override void OnClose()
	{
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		CameraController.CinemachineBrain.enabled = true;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, this.m_hudArgs);
		this.m_windowCanvas.gameObject.SetActive(false);
		CameraController.SoloCam.gameObject.SetActive(false);
	}

	// Token: 0x04003768 RID: 14184
	[SerializeField]
	private Image m_banner;

	// Token: 0x04003769 RID: 14185
	[SerializeField]
	private CanvasGroup m_bannerCanvasGroup;

	// Token: 0x0400376A RID: 14186
	[SerializeField]
	private CanvasGroup m_spotlightCanvasGroup;

	// Token: 0x0400376B RID: 14187
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x0400376C RID: 14188
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x0400376D RID: 14189
	[SerializeField]
	private StudioEventEmitter m_snapshotEventEmitter;

	// Token: 0x0400376E RID: 14190
	[SerializeField]
	private StudioEventEmitter m_deathLoopEventEmitter;

	// Token: 0x0400376F RID: 14191
	[SerializeField]
	private StudioEventEmitter m_reverseLoopEventEmitter;

	// Token: 0x04003770 RID: 14192
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003771 RID: 14193
	private PlayerHUDVisibilityEventArgs m_hudArgs;

	// Token: 0x04003772 RID: 14194
	private List<Transform> m_visualsTransformList = new List<Transform>();

	// Token: 0x04003773 RID: 14195
	private List<LayerMask> m_visualsLayerMaskList = new List<LayerMask>();
}
