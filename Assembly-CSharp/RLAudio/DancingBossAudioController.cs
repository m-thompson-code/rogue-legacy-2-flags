using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E5B RID: 3675
	public class DancingBossAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002134 RID: 8500
		// (get) Token: 0x060067A5 RID: 26533 RVA: 0x0003937A File Offset: 0x0003757A
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

		// Token: 0x060067A6 RID: 26534 RVA: 0x0017D580 File Offset: 0x0017B780
		private void Awake()
		{
			this.m_animator = base.GetComponent<Animator>();
			this.m_enemyController = base.GetComponent<EnemyController>();
			this.m_onBossDefeated = new Action(this.OnBossDefeated);
			this.m_onSentrySummonStart = new Action(this.OnSentrySummonStart);
			this.m_onSentrySummonComplete = new Action(this.OnSentrySummonComplete);
			this.m_onWaveFired = new Action<DancingBossWaveFiredEventArgs>(this.OnWaveFired);
			this.m_onSpreadAttackStart = new Action<Vector3>(this.OnSpreadAttackStart);
			this.m_onSpreadAttackComplete = new Action(this.OnSpreadAttackComplete);
			this.m_onSummonShotsTellStart = new Action(this.OnSummonShotsTellStart);
			this.m_onSummonShotsAttackStart = new Action(this.OnSummonShotsAttackStart);
			this.m_onSummonShotsAttackComplete = new Action(this.OnSummonShotsAttackComplete);
			this.m_onSummonShotsTellComplete = new Action(this.OnSummonShotsTellComplete);
			this.m_onSummonShotsPortalOpened = new Action<Vector2>(this.OnSummonShotsPortalOpened);
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x000393A0 File Offset: 0x000375A0
		private void Start()
		{
			base.StartCoroutine(this.WaitUntilIntroIsComplete());
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x0017D66C File Offset: 0x0017B86C
		private void OnDisable()
		{
			this.m_isIntroComplete = false;
			DancingBoss_Basic_AIScript dancingBoss_Basic_AIScript = this.m_enemyController.LogicController.LogicScript as DancingBoss_Basic_AIScript;
			if (this.m_bossRoomController)
			{
				this.m_bossRoomController.BossDefeatedRelay.RemoveListener(this.m_onBossDefeated);
			}
			dancingBoss_Basic_AIScript.SummonSentriesStartRelay.RemoveListener(this.m_onSentrySummonStart);
			dancingBoss_Basic_AIScript.SummonSentriesCompleteRelay.RemoveListener(this.m_onSentrySummonComplete);
			dancingBoss_Basic_AIScript.WaveFiredRelay.RemoveListener(this.m_onWaveFired);
			dancingBoss_Basic_AIScript.SpreadAttackStartRelay.RemoveListener(this.m_onSpreadAttackStart);
			dancingBoss_Basic_AIScript.SpreadAttackCompleteRelay.RemoveListener(this.m_onSpreadAttackComplete);
			dancingBoss_Basic_AIScript.SummonShotsTellStartRelay.RemoveListener(this.m_onSummonShotsTellStart);
			dancingBoss_Basic_AIScript.SummonShotsTellCompleteRelay.RemoveListener(this.m_onSummonShotsTellComplete);
			dancingBoss_Basic_AIScript.SummonShotsAttackStartRelay.RemoveListener(this.m_onSummonShotsAttackStart);
			dancingBoss_Basic_AIScript.SummonShotsAttackCompleteRelay.RemoveListener(this.m_onSummonShotsAttackComplete);
			dancingBoss_Basic_AIScript.SummonShotsPortalOpenedRelay.RemoveListener(this.m_onSummonShotsPortalOpened);
			this.StopAudioEventInstances();
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x0017D774 File Offset: 0x0017B974
		private void OnDestroy()
		{
			if (this.m_waveShootLoopAudioSources != null)
			{
				for (int i = this.m_waveShootLoopAudioSources.Count - 1; i >= 0; i--)
				{
					UnityEngine.Object.Destroy(this.m_waveShootLoopAudioSources[i]);
					this.m_waveShootLoopAudioSources[i] = null;
				}
				this.m_waveShootLoopAudioSources = null;
			}
			if (this.m_hoopAudioEventInstance.isValid())
			{
				this.m_hoopAudioEventInstance.release();
			}
			if (this.m_flyingAudioEventInstance.isValid())
			{
				this.m_flyingAudioEventInstance.release();
			}
			if (this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				this.m_spreadAttackSummoningAudioEventInstance.release();
			}
			if (this.m_summonAttackAudioEventInstance.isValid())
			{
				this.m_summonAttackAudioEventInstance.release();
			}
			if (this.m_spreadAttackEventInstance.isValid())
			{
				this.m_spreadAttackEventInstance.release();
			}
			if (this.m_waveAttackEventInstance.isValid())
			{
				this.m_waveAttackEventInstance.release();
			}
			if (this.m_summoningPortalsEventInstance.isValid())
			{
				this.m_summoningPortalsEventInstance.release();
			}
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x000393AF File Offset: 0x000375AF
		private IEnumerator WaitUntilIntroIsComplete()
		{
			while (this.m_enemyController.Room == null)
			{
				yield return null;
			}
			if (!this.m_bossRoomController)
			{
				this.m_bossRoomController = this.m_enemyController.Room.gameObject.GetComponent<BossRoomController>();
			}
			bool hasEnteredIntroState;
			do
			{
				hasEnteredIntroState = this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Intro");
				yield return null;
			}
			while (!hasEnteredIntroState);
			bool hasEnteredIdleState;
			do
			{
				hasEnteredIdleState = this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
				yield return null;
			}
			while (!hasEnteredIdleState);
			this.OnIntroComplete();
			yield break;
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x0017D878 File Offset: 0x0017BA78
		private void OnIntroComplete()
		{
			this.m_animator.SetInteger("Phase", 0);
			DancingBoss_Basic_AIScript dancingBoss_Basic_AIScript = this.m_enemyController.LogicController.LogicScript as DancingBoss_Basic_AIScript;
			if (this.m_bossRoomController)
			{
				this.m_bossRoomController.BossDefeatedRelay.AddListener(this.m_onBossDefeated, false);
			}
			dancingBoss_Basic_AIScript.SummonSentriesStartRelay.AddListener(this.m_onSentrySummonStart, false);
			dancingBoss_Basic_AIScript.SummonSentriesCompleteRelay.AddListener(this.m_onSentrySummonComplete, false);
			dancingBoss_Basic_AIScript.WaveFiredRelay.AddListener(this.m_onWaveFired, false);
			dancingBoss_Basic_AIScript.SpreadAttackStartRelay.AddListener(this.m_onSpreadAttackStart, false);
			dancingBoss_Basic_AIScript.SpreadAttackCompleteRelay.AddListener(this.m_onSpreadAttackComplete, false);
			dancingBoss_Basic_AIScript.SummonShotsTellStartRelay.AddListener(this.m_onSummonShotsTellStart, false);
			dancingBoss_Basic_AIScript.SummonShotsTellCompleteRelay.AddListener(this.m_onSummonShotsTellComplete, false);
			dancingBoss_Basic_AIScript.SummonShotsAttackStartRelay.AddListener(this.m_onSummonShotsAttackStart, false);
			dancingBoss_Basic_AIScript.SummonShotsAttackCompleteRelay.AddListener(this.m_onSummonShotsAttackComplete, false);
			dancingBoss_Basic_AIScript.SummonShotsPortalOpenedRelay.AddListener(this.m_onSummonShotsPortalOpened, false);
			this.m_hoopAudioEventInstance = AudioUtility.GetEventInstance(this.m_hoopMovementAudioPath, base.transform);
			AudioManager.Play(this, this.m_hoopAudioEventInstance);
			this.m_flyingAudioEventInstance = AudioUtility.GetEventInstance(this.m_flyingAudioPath, base.transform);
			this.m_isIntroComplete = true;
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x000393BE File Offset: 0x000375BE
		private void OnSpreadIntroAnimStart()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spreadAttackIntroAudioPath, this.m_enemyController.gameObject);
		}

		// Token: 0x060067AD RID: 26541 RVA: 0x00002FCA File Offset: 0x000011CA
		private void OnSpreadIntroAnimComplete()
		{
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x0017D9D0 File Offset: 0x0017BBD0
		private void OnSpreadAttackAnimStart()
		{
			if (!this.m_spreadAttackEventInstance.isValid())
			{
				this.m_spreadAttackEventInstance = AudioUtility.GetEventInstance(this.m_spreadAttackStartAudioPath, this.m_enemyController.transform);
			}
			AudioManager.PlayAttached(this, this.m_spreadAttackEventInstance, this.m_enemyController.gameObject);
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x000393D7 File Offset: 0x000375D7
		private void OnSpreadAttackAnimComplete()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spreadAttackCompleteAudioPath, this.m_enemyController.gameObject);
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x000393F0 File Offset: 0x000375F0
		private void OnSpreadAttackStart(Vector3 summonPosition)
		{
			this.m_spreadAttackSummonPosition = summonPosition;
			if (!this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				this.m_spreadAttackSummoningAudioEventInstance = AudioUtility.GetEventInstance(this.m_spreadAttackSpawnPortalsAudioPath, base.transform);
			}
			AudioManager.Play(this, this.m_spreadAttackSummoningAudioEventInstance, summonPosition);
		}

		// Token: 0x060067B1 RID: 26545 RVA: 0x0003942A File Offset: 0x0003762A
		private void OnSpreadAttackComplete()
		{
			if (this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_spreadAttackSummoningAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			AudioManager.PlayOneShot(this, this.m_spreadAttackDespawnPortalsAudioPath, this.m_spreadAttackSummonPosition);
		}

		// Token: 0x060067B2 RID: 26546 RVA: 0x00039457 File Offset: 0x00037657
		private void OnSummonShotsTellStart()
		{
			AudioManager.PlayOneShot(this, this.m_summonTellStartAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x060067B3 RID: 26547 RVA: 0x00039475 File Offset: 0x00037675
		private void OnSummonShotsTellComplete()
		{
			AudioManager.PlayOneShot(this, this.m_summonTellCompleteAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x0017DA20 File Offset: 0x0017BC20
		private void OnSummonShotsAttackStart()
		{
			if (!this.m_summonAttackAudioEventInstance.isValid())
			{
				this.m_summonAttackAudioEventInstance = AudioUtility.GetEventInstance(this.m_summonAttackStartAudioPath, base.transform);
			}
			AudioManager.Play(this, this.m_summonAttackAudioEventInstance, this.m_enemyController.transform.position);
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x00039493 File Offset: 0x00037693
		private void OnSummonShotsAttackComplete()
		{
			if (this.m_summonAttackAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_summonAttackAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			AudioManager.PlayOneShot(this, this.m_summonAttackCompleteAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x000394CA File Offset: 0x000376CA
		private void OnSummonShotsPortalOpened(Vector2 position)
		{
			base.StartCoroutine(this.PlaySummonShotsPortalAudio(position));
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x000394DA File Offset: 0x000376DA
		private IEnumerator PlaySummonShotsPortalAudio(Vector2 position)
		{
			AudioManager.PlayDelayedOneShot(this, this.m_summonSpawnPortalAudioPath, position, -1f);
			yield return this.m_waitUntilSummonPortalDespawnStart;
			AudioManager.PlayDelayedOneShot(this, this.m_summonDespawnPortalAudioPath, position, -1f);
			yield break;
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x000394F0 File Offset: 0x000376F0
		private void OnWaveFired(DancingBossWaveFiredEventArgs waveFiredEventArgs)
		{
			base.StartCoroutine(this.PlayWaveFiredAudio(waveFiredEventArgs));
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x00039500 File Offset: 0x00037700
		private IEnumerator PlayWaveFiredAudio(DancingBossWaveFiredEventArgs waveInfo)
		{
			string shootAudioPath = this.m_verticalWaveShootAudioPath;
			string eventPath = this.m_verticalWaveSpawnPortalsAudioPath;
			string despawnAudioPath = this.m_verticalWaveDespawnPortalsAudioPath;
			if (waveInfo.Side == RoomSide.Top || waveInfo.Side == RoomSide.Bottom)
			{
				shootAudioPath = this.m_horizontalWaveShootAudioPath;
				eventPath = this.m_horizontalWaveSpawnPortalsAudioPath;
				despawnAudioPath = this.m_horizontalWaveDespawnPortalsAudioPath;
			}
			if (!this.m_summoningPortalsEventInstance.isValid())
			{
				this.m_summoningPortalsEventInstance = AudioUtility.GetEventInstance(eventPath, base.transform);
			}
			AudioManager.Play(this, this.m_summoningPortalsEventInstance, waveInfo.Origin);
			yield return this.m_waitUntilSummonPortalDespawnStart;
			AudioManager.Stop(this.m_summoningPortalsEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.PlayOneShot(this, despawnAudioPath, waveInfo.Origin);
			Vector3 direction = Vector2.zero;
			switch (waveInfo.Side)
			{
			case RoomSide.Top:
				direction = Vector2.down;
				break;
			case RoomSide.Bottom:
				direction = Vector2.up;
				break;
			case RoomSide.Left:
				direction = Vector2.right;
				break;
			case RoomSide.Right:
				direction = Vector2.left;
				break;
			}
			GameObject waveShootLoopAudioSource = this.GetWaveShootLoopAudioSource();
			waveShootLoopAudioSource.transform.position = waveInfo.Origin;
			waveShootLoopAudioSource.SetActive(true);
			if (!this.m_waveAttackEventInstance.isValid())
			{
				this.m_waveAttackEventInstance = AudioUtility.GetEventInstance(shootAudioPath, waveShootLoopAudioSource.transform);
			}
			AudioManager.PlayAttached(this, this.m_waveAttackEventInstance, waveShootLoopAudioSource);
			float timeStart = Time.time;
			while (Time.time - timeStart < waveInfo.LifeSpan)
			{
				waveShootLoopAudioSource.transform.position += waveInfo.Speed * direction * Time.deltaTime;
				yield return null;
			}
			AudioManager.Stop(this.m_waveAttackEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			waveShootLoopAudioSource.SetActive(false);
			yield break;
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x0017DA70 File Offset: 0x0017BC70
		private GameObject GetWaveShootLoopAudioSource()
		{
			if (this.m_waveShootLoopAudioSources == null)
			{
				this.m_waveShootLoopAudioSources = new List<GameObject>(4);
				for (int i = 0; i < 4; i++)
				{
					GameObject gameObject = new GameObject("Wave Shoot Loop Audio Source");
					this.m_waveShootLoopAudioSources.Add(gameObject);
					gameObject.transform.SetParent(this.m_enemyController.Room.gameObject.transform);
					gameObject.SetActive(false);
				}
			}
			GameObject gameObject2 = null;
			for (int j = 0; j < 4; j++)
			{
				if (!this.m_waveShootLoopAudioSources[j].activeInHierarchy)
				{
					gameObject2 = this.m_waveShootLoopAudioSources[j];
					break;
				}
			}
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("Wave Shoot Loop Audio Source");
				gameObject2.transform.SetParent(this.m_enemyController.Room.gameObject.transform);
				this.m_waveShootLoopAudioSources.Add(gameObject2);
			}
			return gameObject2;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0017DB4C File Offset: 0x0017BD4C
		private void OnSentrySummonStart()
		{
			AudioManager.PlayOneShot(this, this.m_summonSentriesStartAudioPath, default(Vector3));
			this.m_animator.SetInteger("Phase", 1);
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x0017DB80 File Offset: 0x0017BD80
		private void OnSentrySummonComplete()
		{
			AudioManager.PlayOneShot(this, this.m_summonSentriesCompleteAudioPath, default(Vector3));
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x00039516 File Offset: 0x00037716
		private void OnBossDefeated()
		{
			if (this.m_bossRoomController)
			{
				this.m_bossRoomController.BossDefeatedRelay.RemoveListener(this.m_onBossDefeated);
			}
			this.StopAudioEventInstances();
			AudioManager.PlayOneShotAttached(this, this.m_hoopMovementEndAudioPath, base.gameObject);
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x0017DBA4 File Offset: 0x0017BDA4
		private void StopAudioEventInstances()
		{
			if (this.m_hoopAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_hoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_flyingAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_flyingAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_waveAttackEventInstance.isValid())
			{
				AudioManager.Stop(this.m_waveAttackEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			if (this.m_summoningPortalsEventInstance.isValid())
			{
				AudioManager.Stop(this.m_summoningPortalsEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			if (this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_spreadAttackSummoningAudioEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			if (this.m_summonAttackAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_summonAttackAudioEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x00039554 File Offset: 0x00037754
		private void Update()
		{
			if (!this.m_isIntroComplete)
			{
				return;
			}
			this.UpdateHoopAudio();
			this.UpdateFlyingAudio();
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x0017DC48 File Offset: 0x0017BE48
		private void UpdateFlyingAudio()
		{
			bool flag = this.m_enemyController.Velocity.sqrMagnitude > 0f;
			if (flag && this.m_currentFlyingState == DancingBossAudioController.FlyingStateID.Stopped)
			{
				AudioManager.PlayAttached(this, this.m_flyingAudioEventInstance, base.gameObject);
				this.m_currentFlyingState = DancingBossAudioController.FlyingStateID.Flying;
			}
			else if (!flag && this.m_currentFlyingState == DancingBossAudioController.FlyingStateID.Flying)
			{
				AudioManager.Stop(this.m_flyingAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				AudioManager.PlayOneShotAttached(this, this.m_stopFlyingAudioPath, base.gameObject);
				this.m_currentFlyingState = DancingBossAudioController.FlyingStateID.Stopped;
			}
			if (flag)
			{
				float magnitude = this.m_enemyController.Velocity.magnitude;
				float value = 0f;
				if (magnitude > 0f)
				{
					if (magnitude < this.m_maxFlyingSpeed)
					{
						value = magnitude / this.m_maxFlyingSpeed;
					}
					else
					{
						value = 1f;
					}
				}
				if (this.m_flyingAudioEventInstance.isValid() && AudioUtility.GetHasParameter(this.m_flyingAudioEventInstance, "danceBoss_flySpeed"))
				{
					this.m_flyingAudioEventInstance.setParameterByName("danceBoss_flySpeed", value, false);
				}
			}
			Vector2 normalized = this.m_enemyController.Velocity.normalized;
			if (Mathf.Sign(this.m_previousFlightDirection.x) != Mathf.Sign(normalized.x) || Mathf.Sign(this.m_previousFlightDirection.y) != Mathf.Sign(normalized.y))
			{
				AudioManager.PlayOneShotAttached(this, this.m_changeDirectionWhileFlyingAudioPath, base.gameObject);
				this.m_previousFlightDirection = normalized;
			}
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x0017DDA4 File Offset: 0x0017BFA4
		private void UpdateHoopAudio()
		{
			DancingBossAudioController.AttackStateID attackState = this.GetAttackState();
			if (attackState != this.m_previousAttackState)
			{
				if (attackState == DancingBossAudioController.AttackStateID.AttackHold)
				{
					if (this.m_hoopAudioEventInstance.isValid())
					{
						this.m_hoopAudioEventInstance.setParameterByName("dancingBoss_idleSpeed", 1f, false);
					}
				}
				else if (this.m_previousAttackState == DancingBossAudioController.AttackStateID.AttackHold && this.m_hoopAudioEventInstance.isValid())
				{
					this.m_hoopAudioEventInstance.setParameterByName("dancingBoss_idleSpeed", 0f, false);
				}
				this.m_previousAttackState = attackState;
			}
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0017DE20 File Offset: 0x0017C020
		private DancingBossAudioController.AttackStateID GetAttackState()
		{
			DancingBossAudioController.AttackStateID result = DancingBossAudioController.AttackStateID.None;
			if (this.m_animator != null)
			{
				if (this.GetIsAnimatorStateTag("Tell_Intro"))
				{
					result = DancingBossAudioController.AttackStateID.TellIntro;
				}
				else if (this.GetIsAnimatorStateTag("Tell_Hold"))
				{
					result = DancingBossAudioController.AttackStateID.TellHold;
				}
				else if (this.GetIsAnimatorStateTag("Attack_Intro"))
				{
					result = DancingBossAudioController.AttackStateID.AttackIntro;
				}
				else if (this.GetIsAnimatorStateTag("Attack_Hold"))
				{
					result = DancingBossAudioController.AttackStateID.AttackHold;
				}
				else if (this.GetIsAnimatorStateTag("Attack_Exit"))
				{
					result = DancingBossAudioController.AttackStateID.AttackExit;
				}
			}
			return result;
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x0017DE94 File Offset: 0x0017C094
		private bool GetIsAnimatorStateTag(string tag)
		{
			return this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
		}

		// Token: 0x040053F4 RID: 21492
		[SerializeField]
		[EventRef]
		private string m_hoopMovementAudioPath = string.Empty;

		// Token: 0x040053F5 RID: 21493
		[SerializeField]
		[EventRef]
		private string m_hoopMovementEndAudioPath = string.Empty;

		// Token: 0x040053F6 RID: 21494
		[SerializeField]
		[EventRef]
		private string m_flyingAudioPath = string.Empty;

		// Token: 0x040053F7 RID: 21495
		[SerializeField]
		[EventRef]
		private string m_stopFlyingAudioPath = string.Empty;

		// Token: 0x040053F8 RID: 21496
		[SerializeField]
		[EventRef]
		private string m_changeDirectionWhileFlyingAudioPath = string.Empty;

		// Token: 0x040053F9 RID: 21497
		[SerializeField]
		[Range(1f, 100f)]
		private float m_maxFlyingSpeed = 5f;

		// Token: 0x040053FA RID: 21498
		[SerializeField]
		[EventRef]
		private string m_summonSentriesStartAudioPath = string.Empty;

		// Token: 0x040053FB RID: 21499
		[SerializeField]
		[EventRef]
		private string m_summonSentriesCompleteAudioPath = string.Empty;

		// Token: 0x040053FC RID: 21500
		[SerializeField]
		[EventRef]
		private string m_verticalWaveShootAudioPath = string.Empty;

		// Token: 0x040053FD RID: 21501
		[SerializeField]
		[EventRef]
		private string m_verticalWaveSpawnPortalsAudioPath = string.Empty;

		// Token: 0x040053FE RID: 21502
		[SerializeField]
		[EventRef]
		private string m_verticalWaveDespawnPortalsAudioPath = string.Empty;

		// Token: 0x040053FF RID: 21503
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveShootAudioPath = string.Empty;

		// Token: 0x04005400 RID: 21504
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveSpawnPortalsAudioPath = string.Empty;

		// Token: 0x04005401 RID: 21505
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveDespawnPortalsAudioPath = string.Empty;

		// Token: 0x04005402 RID: 21506
		[SerializeField]
		[EventRef]
		private string m_spreadAttackSpawnPortalsAudioPath = string.Empty;

		// Token: 0x04005403 RID: 21507
		[SerializeField]
		[EventRef]
		private string m_spreadAttackIntroAudioPath = string.Empty;

		// Token: 0x04005404 RID: 21508
		[SerializeField]
		[EventRef]
		private string m_spreadAttackStartAudioPath = string.Empty;

		// Token: 0x04005405 RID: 21509
		[SerializeField]
		[EventRef]
		private string m_spreadAttackCompleteAudioPath = string.Empty;

		// Token: 0x04005406 RID: 21510
		[SerializeField]
		[EventRef]
		private string m_spreadAttackDespawnPortalsAudioPath = string.Empty;

		// Token: 0x04005407 RID: 21511
		[SerializeField]
		[EventRef]
		private string m_summonTellStartAudioPath = string.Empty;

		// Token: 0x04005408 RID: 21512
		[SerializeField]
		[EventRef]
		private string m_summonTellCompleteAudioPath = string.Empty;

		// Token: 0x04005409 RID: 21513
		[SerializeField]
		[EventRef]
		private string m_summonAttackStartAudioPath = string.Empty;

		// Token: 0x0400540A RID: 21514
		[SerializeField]
		[EventRef]
		private string m_summonAttackCompleteAudioPath = string.Empty;

		// Token: 0x0400540B RID: 21515
		[SerializeField]
		[EventRef]
		private string m_summonSpawnPortalAudioPath = string.Empty;

		// Token: 0x0400540C RID: 21516
		[SerializeField]
		[EventRef]
		private string m_summonDespawnPortalAudioPath = string.Empty;

		// Token: 0x0400540D RID: 21517
		private Animator m_animator;

		// Token: 0x0400540E RID: 21518
		private BossRoomController m_bossRoomController;

		// Token: 0x0400540F RID: 21519
		private EnemyController m_enemyController;

		// Token: 0x04005410 RID: 21520
		private string m_description = string.Empty;

		// Token: 0x04005411 RID: 21521
		private DancingBossAudioController.AttackStateID m_previousAttackState;

		// Token: 0x04005412 RID: 21522
		private const string HOOP_PARAMETER = "dancingBoss_idleSpeed";

		// Token: 0x04005413 RID: 21523
		private const string FLYING_SPEED_PARAMETER = "danceBoss_flySpeed";

		// Token: 0x04005414 RID: 21524
		private const int INITIAL_WAVE_AUDIO_SOURCE_COUNT = 4;

		// Token: 0x04005415 RID: 21525
		private const string PHASE_ANIMATOR_PARAMETER = "Phase";

		// Token: 0x04005416 RID: 21526
		private DancingBossAudioController.FlyingStateID m_currentFlyingState;

		// Token: 0x04005417 RID: 21527
		private Vector2 m_previousFlightDirection;

		// Token: 0x04005418 RID: 21528
		private bool m_isIntroComplete;

		// Token: 0x04005419 RID: 21529
		private List<GameObject> m_waveShootLoopAudioSources;

		// Token: 0x0400541A RID: 21530
		private WaitForSeconds m_waitUntilSummonPortalDespawnStart = new WaitForSeconds(1.2f);

		// Token: 0x0400541B RID: 21531
		private Vector3 m_spreadAttackSummonPosition;

		// Token: 0x0400541C RID: 21532
		private EventInstance m_hoopAudioEventInstance;

		// Token: 0x0400541D RID: 21533
		private EventInstance m_flyingAudioEventInstance;

		// Token: 0x0400541E RID: 21534
		private EventInstance m_spreadAttackSummoningAudioEventInstance;

		// Token: 0x0400541F RID: 21535
		private EventInstance m_summonAttackAudioEventInstance;

		// Token: 0x04005420 RID: 21536
		private EventInstance m_spreadAttackEventInstance;

		// Token: 0x04005421 RID: 21537
		private EventInstance m_waveAttackEventInstance;

		// Token: 0x04005422 RID: 21538
		private EventInstance m_summoningPortalsEventInstance;

		// Token: 0x04005423 RID: 21539
		private Action m_onBossDefeated;

		// Token: 0x04005424 RID: 21540
		private Action m_onSentrySummonStart;

		// Token: 0x04005425 RID: 21541
		private Action m_onSentrySummonComplete;

		// Token: 0x04005426 RID: 21542
		private Action<DancingBossWaveFiredEventArgs> m_onWaveFired;

		// Token: 0x04005427 RID: 21543
		private Action<Vector3> m_onSpreadAttackStart;

		// Token: 0x04005428 RID: 21544
		private Action m_onSpreadAttackComplete;

		// Token: 0x04005429 RID: 21545
		private Action m_onSummonShotsTellStart;

		// Token: 0x0400542A RID: 21546
		private Action m_onSummonShotsTellComplete;

		// Token: 0x0400542B RID: 21547
		private Action m_onSummonShotsAttackStart;

		// Token: 0x0400542C RID: 21548
		private Action m_onSummonShotsAttackComplete;

		// Token: 0x0400542D RID: 21549
		private Action<Vector2> m_onSummonShotsPortalOpened;

		// Token: 0x02000E5C RID: 3676
		private enum AttackStateID
		{
			// Token: 0x0400542F RID: 21551
			None,
			// Token: 0x04005430 RID: 21552
			TellIntro,
			// Token: 0x04005431 RID: 21553
			TellHold,
			// Token: 0x04005432 RID: 21554
			AttackIntro,
			// Token: 0x04005433 RID: 21555
			AttackHold,
			// Token: 0x04005434 RID: 21556
			AttackExit
		}

		// Token: 0x02000E5D RID: 3677
		private enum FlyingStateID
		{
			// Token: 0x04005436 RID: 21558
			Stopped,
			// Token: 0x04005437 RID: 21559
			Flying
		}
	}
}
