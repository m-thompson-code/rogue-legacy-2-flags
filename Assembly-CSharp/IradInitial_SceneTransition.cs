using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
public class IradInitial_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition, IAudioEventEmitter
{
	// Token: 0x06002824 RID: 10276 RVA: 0x000168EC File Offset: 0x00014AEC
	protected override void Awake()
	{
		base.Awake();
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x00016915 File Offset: 0x00014B15
	public void SetDistortInPosition(GameObject obj)
	{
		this.m_distortInPosObj = obj;
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x0001691E File Offset: 0x00014B1E
	public void SetDistortOutPosition(GameObject obj)
	{
		this.m_distortOutPosObj = obj;
	}

	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06002827 RID: 10279 RVA: 0x00004A89 File Offset: 0x00002C89
	public override TransitionID ID
	{
		get
		{
			return TransitionID.IradInitial;
		}
	}

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06002828 RID: 10280 RVA: 0x00016927 File Offset: 0x00014B27
	public string Description
	{
		get
		{
			if (this.m_description == string.Empty)
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x0001694D File Offset: 0x00014B4D
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x00016955 File Offset: 0x00014B55
	public IEnumerator TransitionIn()
	{
		RewiredMapController.SetIsInCutscene(true);
		if (!this.m_distortInPosObj)
		{
			EnemySpawnController[] enemySpawnControllers = PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers;
			for (int i = 0; i < enemySpawnControllers.Length; i++)
			{
				EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
				if (enemyInstance && enemyInstance.EnemyRank == EnemyRank.Miniboss && enemyInstance.EnemyType == EnemyType.EyeballBoss_Middle)
				{
					this.m_distortInPosObj = enemyInstance.gameObject;
					break;
				}
			}
			if (!this.m_distortInPosObj)
			{
				this.m_distortInPosObj = PlayerManager.GetPlayerController().gameObject;
			}
		}
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_storedFallMultiplier = playerController.FallMultiplierOverride;
		playerController.FallMultiplierOverride = 0.1f;
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(this.m_distortInPosObj.transform.position);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = true;
		TweenManager.TweenTo(this.m_warpEffect, 1.5f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"DistortionAmount",
			0.025f
		});
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		Vector2 vector2 = this.m_distortInPosObj.transform.position - playerController.transform.position;
		vector2 *= 0.75f;
		TweenManager.TweenBy(playerController.transform, 1f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.x",
			vector2.x,
			"localPosition.y",
			vector2.y
		});
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		AudioManager.PlayOneShot(this, this.m_transitionInAudioEventPath, this.m_distortInPosObj.transform.position);
		yield return TweenManager.TweenTo(this.m_warpEffect, this.m_distortDuration, new EaseDelegate(Ease.Back.EaseIn), new object[]
		{
			"DistortionAmount",
			1
		}).TweenCoroutine;
		this.m_warpEffect.DistortionAmount = 1f;
		yield return null;
		RewiredMapController.SetIsInCutscene(false);
		yield break;
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x00016964 File Offset: 0x00014B64
	public IEnumerator TransitionOut()
	{
		PlayerManager.GetPlayerController().FallMultiplierOverride = this.m_storedFallMultiplier;
		if (!this.m_distortOutPosObj)
		{
			EnemySpawnController[] enemySpawnControllers = PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers;
			for (int i = 0; i < enemySpawnControllers.Length; i++)
			{
				EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
				if (enemyInstance && enemyInstance.EnemyRank == EnemyRank.Miniboss && enemyInstance.EnemyType == EnemyType.EyeballBoss_Middle)
				{
					this.m_distortOutPosObj = enemyInstance.gameObject;
					break;
				}
			}
			if (!this.m_distortOutPosObj)
			{
				this.m_distortOutPosObj = PlayerManager.GetPlayerController().gameObject;
			}
		}
		AudioManager.PlayOneShot(this, this.m_transitionOutAudioEventPath, this.m_distortInPosObj.transform.position);
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(this.m_distortOutPosObj.transform.position);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 1f;
		yield return TweenManager.TweenTo(this.m_warpEffect, this.m_distortDuration, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"DistortionAmount",
			0
		}).TweenCoroutine;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = false;
		yield break;
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x00016973 File Offset: 0x00014B73
	private void OnDisable()
	{
		this.m_distortInPosObj = null;
		this.m_distortOutPosObj = null;
	}

	// Token: 0x04002356 RID: 9046
	[SerializeField]
	private float m_distortDuration = 1f;

	// Token: 0x04002357 RID: 9047
	[SerializeField]
	[EventRef]
	private string m_transitionInAudioEventPath;

	// Token: 0x04002358 RID: 9048
	[SerializeField]
	[EventRef]
	private string m_transitionOutAudioEventPath;

	// Token: 0x04002359 RID: 9049
	private GameObject m_distortInPosObj;

	// Token: 0x0400235A RID: 9050
	private GameObject m_distortOutPosObj;

	// Token: 0x0400235B RID: 9051
	private HeirloomWarp_Effect m_warpEffect;

	// Token: 0x0400235C RID: 9052
	private string m_description = string.Empty;

	// Token: 0x0400235D RID: 9053
	private float m_storedFallMultiplier = 1f;

	// Token: 0x0400235E RID: 9054
	private WaitRL_Yield m_waitYield;
}
