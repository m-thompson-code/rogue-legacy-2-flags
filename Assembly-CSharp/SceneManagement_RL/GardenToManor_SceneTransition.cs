using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008C5 RID: 2245
	public class GardenToManor_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x060049AC RID: 18860 RVA: 0x00109CE2 File Offset: 0x00107EE2
		public override TransitionID ID
		{
			get
			{
				return TransitionID.GardenToManor;
			}
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00109CE8 File Offset: 0x00107EE8
		protected override void Awake()
		{
			base.Awake();
			this.m_waitRL = new WaitRL_Yield(0f, true);
			this.m_canvasGroup.alpha = 0f;
			this.m_swipeAnimator = base.GetComponent<Animator>();
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
			this.m_skyObj.SetLayerRecursively(23, true);
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00109D56 File Offset: 0x00107F56
		public IEnumerator TransitionIn()
		{
			RewiredMapController.SetIsInCutscene(true);
			this.m_canvasGroup.alpha = 0f;
			this.m_playerModel.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
			this.m_playerModel.gameObject.SetActive(false);
			this.m_playerModel.transform.localScale *= 0.5f;
			this.m_playerModel.gameObject.SetLayerRecursively(5, true);
			this.m_biomeLightController.gameObject.SetActive(false);
			this.m_skyObj.SetActive(false);
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			TweenManager.StopAllTweens(false);
			yield break;
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x00109D65 File Offset: 0x00107F65
		public IEnumerator TransitionOut()
		{
			this.m_playerModel.gameObject.SetActive(true);
			this.m_biomeLightController.gameObject.SetActive(true);
			if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
			{
				WindowManager.LoadWindow(WindowID.SkillTree);
			}
			(WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController).EnableGardenTransitionState(true);
			WindowManager.SetWindowIsOpen(WindowID.SkillTree, true);
			this.m_waitRL.CreateNew(0.5f, true);
			yield return this.m_waitRL;
			Vector3 vector = CameraController.UICamera.transform.position;
			vector.z = 0f;
			vector += new Vector3(-7.4f, -6.5f, 0f);
			this.m_playerModel.transform.position = vector;
			Vector3 position = CameraController.UICamera.transform.position;
			position.z = 0f;
			this.m_skyObj.transform.position = position;
			this.m_skyObj.SetActive(true);
			Animator castleAnimator = (WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController).CastleAnimator;
			EffectManager.AddAnimatorToDisableList(castleAnimator);
			castleAnimator.SetBool("StartingStairs", true);
			castleAnimator.SetBool("Entrance_1", true);
			castleAnimator.Update(1f);
			castleAnimator.Update(1f);
			EffectManager.RemoveAnimatorFromDisableList(castleAnimator);
			AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_ending_stairwayClimb", CameraController.GameCamera.transform.position);
			this.m_playerModel.Animator.Play("WalkUpStairs", 0, 0f);
			TweenManager.TweenBy(this.m_playerModel.transform, 4f, new EaseDelegate(Ease.None), new object[]
			{
				"localPosition.y",
				1.5f
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			this.m_waitRL.CreateNew(3f, true);
			yield return this.m_waitRL;
			AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_ending_enterCastle", CameraController.GameCamera.transform.position);
			this.m_swipeAnimator.SetBool("Covered", true);
			yield return null;
			AnimatorStateInfo currentAnimatorStateInfo = this.m_swipeAnimator.GetCurrentAnimatorStateInfo(0);
			float waitTime = currentAnimatorStateInfo.length * Mathf.Abs(currentAnimatorStateInfo.speed) * currentAnimatorStateInfo.speedMultiplier;
			this.m_waitRL.CreateNew(waitTime, true);
			yield return this.m_waitRL;
			WindowManager.SetWindowIsOpen(WindowID.SkillTree, false);
			(WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController).EnableGardenTransitionState(false);
			this.m_playerModel.gameObject.SetActive(false);
			this.m_biomeLightController.gameObject.SetActive(false);
			this.m_skyObj.SetActive(false);
			yield return null;
			yield return null;
			this.m_swipeAnimator.SetBool("Covered", false);
			yield return null;
			currentAnimatorStateInfo = this.m_swipeAnimator.GetCurrentAnimatorStateInfo(0);
			waitTime = currentAnimatorStateInfo.length * Mathf.Abs(currentAnimatorStateInfo.speed) * currentAnimatorStateInfo.speedMultiplier;
			this.m_waitRL.CreateNew(waitTime, true);
			yield return this.m_waitRL;
			RewiredMapController.SetIsInCutscene(false);
			yield break;
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00109D74 File Offset: 0x00107F74
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04003E00 RID: 15872
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x04003E01 RID: 15873
		[SerializeField]
		private CanvasGroup m_canvasGroup;

		// Token: 0x04003E02 RID: 15874
		[SerializeField]
		private PlayerLookController m_playerModel;

		// Token: 0x04003E03 RID: 15875
		[SerializeField]
		private GameObject m_biomeLightController;

		// Token: 0x04003E04 RID: 15876
		[SerializeField]
		private GameObject m_skyObj;

		// Token: 0x04003E05 RID: 15877
		private Animator m_swipeAnimator;

		// Token: 0x04003E06 RID: 15878
		private WaitRL_Yield m_waitRL;
	}
}
