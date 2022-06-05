using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class IradInitial_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition, IAudioEventEmitter
{
	// Token: 0x06001D45 RID: 7493 RVA: 0x000608A4 File Offset: 0x0005EAA4
	protected override void Awake()
	{
		base.Awake();
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000608CD File Offset: 0x0005EACD
	public void SetDistortInPosition(GameObject obj)
	{
		this.m_distortInPosObj = obj;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000608D6 File Offset: 0x0005EAD6
	public void SetDistortOutPosition(GameObject obj)
	{
		this.m_distortOutPosObj = obj;
	}

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x06001D48 RID: 7496 RVA: 0x000608DF File Offset: 0x0005EADF
	public override TransitionID ID
	{
		get
		{
			return TransitionID.IradInitial;
		}
	}

	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x06001D49 RID: 7497 RVA: 0x000608E3 File Offset: 0x0005EAE3
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

	// Token: 0x06001D4A RID: 7498 RVA: 0x00060909 File Offset: 0x0005EB09
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x00060911 File Offset: 0x0005EB11
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

	// Token: 0x06001D4C RID: 7500 RVA: 0x00060920 File Offset: 0x0005EB20
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

	// Token: 0x06001D4D RID: 7501 RVA: 0x0006092F File Offset: 0x0005EB2F
	private void OnDisable()
	{
		this.m_distortInPosObj = null;
		this.m_distortOutPosObj = null;
	}

	// Token: 0x04001B41 RID: 6977
	[SerializeField]
	private float m_distortDuration = 1f;

	// Token: 0x04001B42 RID: 6978
	[SerializeField]
	[EventRef]
	private string m_transitionInAudioEventPath;

	// Token: 0x04001B43 RID: 6979
	[SerializeField]
	[EventRef]
	private string m_transitionOutAudioEventPath;

	// Token: 0x04001B44 RID: 6980
	private GameObject m_distortInPosObj;

	// Token: 0x04001B45 RID: 6981
	private GameObject m_distortOutPosObj;

	// Token: 0x04001B46 RID: 6982
	private HeirloomWarp_Effect m_warpEffect;

	// Token: 0x04001B47 RID: 6983
	private string m_description = string.Empty;

	// Token: 0x04001B48 RID: 6984
	private float m_storedFallMultiplier = 1f;

	// Token: 0x04001B49 RID: 6985
	private WaitRL_Yield m_waitYield;
}
