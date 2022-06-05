using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008EB RID: 2283
	public class DancingBossAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x06004AEC RID: 19180 RVA: 0x0010D344 File Offset: 0x0010B544
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

		// Token: 0x06004AED RID: 19181 RVA: 0x0010D36C File Offset: 0x0010B56C
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

		// Token: 0x06004AEE RID: 19182 RVA: 0x0010D457 File Offset: 0x0010B657
		private void Start()
		{
			base.StartCoroutine(this.WaitUntilIntroIsComplete());
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0010D468 File Offset: 0x0010B668
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

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0010D570 File Offset: 0x0010B770
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

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0010D671 File Offset: 0x0010B871
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

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0010D680 File Offset: 0x0010B880
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

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0010D7D6 File Offset: 0x0010B9D6
		private void OnSpreadIntroAnimStart()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spreadAttackIntroAudioPath, this.m_enemyController.gameObject);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0010D7EF File Offset: 0x0010B9EF
		private void OnSpreadIntroAnimComplete()
		{
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0010D7F4 File Offset: 0x0010B9F4
		private void OnSpreadAttackAnimStart()
		{
			if (!this.m_spreadAttackEventInstance.isValid())
			{
				this.m_spreadAttackEventInstance = AudioUtility.GetEventInstance(this.m_spreadAttackStartAudioPath, this.m_enemyController.transform);
			}
			AudioManager.PlayAttached(this, this.m_spreadAttackEventInstance, this.m_enemyController.gameObject);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0010D841 File Offset: 0x0010BA41
		private void OnSpreadAttackAnimComplete()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spreadAttackCompleteAudioPath, this.m_enemyController.gameObject);
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0010D85A File Offset: 0x0010BA5A
		private void OnSpreadAttackStart(Vector3 summonPosition)
		{
			this.m_spreadAttackSummonPosition = summonPosition;
			if (!this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				this.m_spreadAttackSummoningAudioEventInstance = AudioUtility.GetEventInstance(this.m_spreadAttackSpawnPortalsAudioPath, base.transform);
			}
			AudioManager.Play(this, this.m_spreadAttackSummoningAudioEventInstance, summonPosition);
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x0010D894 File Offset: 0x0010BA94
		private void OnSpreadAttackComplete()
		{
			if (this.m_spreadAttackSummoningAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_spreadAttackSummoningAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			AudioManager.PlayOneShot(this, this.m_spreadAttackDespawnPortalsAudioPath, this.m_spreadAttackSummonPosition);
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0010D8C1 File Offset: 0x0010BAC1
		private void OnSummonShotsTellStart()
		{
			AudioManager.PlayOneShot(this, this.m_summonTellStartAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0010D8DF File Offset: 0x0010BADF
		private void OnSummonShotsTellComplete()
		{
			AudioManager.PlayOneShot(this, this.m_summonTellCompleteAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0010D900 File Offset: 0x0010BB00
		private void OnSummonShotsAttackStart()
		{
			if (!this.m_summonAttackAudioEventInstance.isValid())
			{
				this.m_summonAttackAudioEventInstance = AudioUtility.GetEventInstance(this.m_summonAttackStartAudioPath, base.transform);
			}
			AudioManager.Play(this, this.m_summonAttackAudioEventInstance, this.m_enemyController.transform.position);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0010D94D File Offset: 0x0010BB4D
		private void OnSummonShotsAttackComplete()
		{
			if (this.m_summonAttackAudioEventInstance.isValid())
			{
				AudioManager.Stop(this.m_summonAttackAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			AudioManager.PlayOneShot(this, this.m_summonAttackCompleteAudioPath, this.m_enemyController.transform.position);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x0010D984 File Offset: 0x0010BB84
		private void OnSummonShotsPortalOpened(Vector2 position)
		{
			base.StartCoroutine(this.PlaySummonShotsPortalAudio(position));
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x0010D994 File Offset: 0x0010BB94
		private IEnumerator PlaySummonShotsPortalAudio(Vector2 position)
		{
			AudioManager.PlayDelayedOneShot(this, this.m_summonSpawnPortalAudioPath, position, -1f);
			yield return this.m_waitUntilSummonPortalDespawnStart;
			AudioManager.PlayDelayedOneShot(this, this.m_summonDespawnPortalAudioPath, position, -1f);
			yield break;
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x0010D9AA File Offset: 0x0010BBAA
		private void OnWaveFired(DancingBossWaveFiredEventArgs waveFiredEventArgs)
		{
			base.StartCoroutine(this.PlayWaveFiredAudio(waveFiredEventArgs));
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x0010D9BA File Offset: 0x0010BBBA
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

		// Token: 0x06004B01 RID: 19201 RVA: 0x0010D9D0 File Offset: 0x0010BBD0
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

		// Token: 0x06004B02 RID: 19202 RVA: 0x0010DAAC File Offset: 0x0010BCAC
		private void OnSentrySummonStart()
		{
			AudioManager.PlayOneShot(this, this.m_summonSentriesStartAudioPath, default(Vector3));
			this.m_animator.SetInteger("Phase", 1);
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x0010DAE0 File Offset: 0x0010BCE0
		private void OnSentrySummonComplete()
		{
			AudioManager.PlayOneShot(this, this.m_summonSentriesCompleteAudioPath, default(Vector3));
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x0010DB02 File Offset: 0x0010BD02
		private void OnBossDefeated()
		{
			if (this.m_bossRoomController)
			{
				this.m_bossRoomController.BossDefeatedRelay.RemoveListener(this.m_onBossDefeated);
			}
			this.StopAudioEventInstances();
			AudioManager.PlayOneShotAttached(this, this.m_hoopMovementEndAudioPath, base.gameObject);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x0010DB40 File Offset: 0x0010BD40
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

		// Token: 0x06004B06 RID: 19206 RVA: 0x0010DBE3 File Offset: 0x0010BDE3
		private void Update()
		{
			if (!this.m_isIntroComplete)
			{
				return;
			}
			this.UpdateHoopAudio();
			this.UpdateFlyingAudio();
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x0010DBFC File Offset: 0x0010BDFC
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

		// Token: 0x06004B08 RID: 19208 RVA: 0x0010DD58 File Offset: 0x0010BF58
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

		// Token: 0x06004B09 RID: 19209 RVA: 0x0010DDD4 File Offset: 0x0010BFD4
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

		// Token: 0x06004B0A RID: 19210 RVA: 0x0010DE48 File Offset: 0x0010C048
		private bool GetIsAnimatorStateTag(string tag)
		{
			return this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
		}

		// Token: 0x04003ED6 RID: 16086
		[SerializeField]
		[EventRef]
		private string m_hoopMovementAudioPath = string.Empty;

		// Token: 0x04003ED7 RID: 16087
		[SerializeField]
		[EventRef]
		private string m_hoopMovementEndAudioPath = string.Empty;

		// Token: 0x04003ED8 RID: 16088
		[SerializeField]
		[EventRef]
		private string m_flyingAudioPath = string.Empty;

		// Token: 0x04003ED9 RID: 16089
		[SerializeField]
		[EventRef]
		private string m_stopFlyingAudioPath = string.Empty;

		// Token: 0x04003EDA RID: 16090
		[SerializeField]
		[EventRef]
		private string m_changeDirectionWhileFlyingAudioPath = string.Empty;

		// Token: 0x04003EDB RID: 16091
		[SerializeField]
		[Range(1f, 100f)]
		private float m_maxFlyingSpeed = 5f;

		// Token: 0x04003EDC RID: 16092
		[SerializeField]
		[EventRef]
		private string m_summonSentriesStartAudioPath = string.Empty;

		// Token: 0x04003EDD RID: 16093
		[SerializeField]
		[EventRef]
		private string m_summonSentriesCompleteAudioPath = string.Empty;

		// Token: 0x04003EDE RID: 16094
		[SerializeField]
		[EventRef]
		private string m_verticalWaveShootAudioPath = string.Empty;

		// Token: 0x04003EDF RID: 16095
		[SerializeField]
		[EventRef]
		private string m_verticalWaveSpawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE0 RID: 16096
		[SerializeField]
		[EventRef]
		private string m_verticalWaveDespawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE1 RID: 16097
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveShootAudioPath = string.Empty;

		// Token: 0x04003EE2 RID: 16098
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveSpawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE3 RID: 16099
		[SerializeField]
		[EventRef]
		private string m_horizontalWaveDespawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE4 RID: 16100
		[SerializeField]
		[EventRef]
		private string m_spreadAttackSpawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE5 RID: 16101
		[SerializeField]
		[EventRef]
		private string m_spreadAttackIntroAudioPath = string.Empty;

		// Token: 0x04003EE6 RID: 16102
		[SerializeField]
		[EventRef]
		private string m_spreadAttackStartAudioPath = string.Empty;

		// Token: 0x04003EE7 RID: 16103
		[SerializeField]
		[EventRef]
		private string m_spreadAttackCompleteAudioPath = string.Empty;

		// Token: 0x04003EE8 RID: 16104
		[SerializeField]
		[EventRef]
		private string m_spreadAttackDespawnPortalsAudioPath = string.Empty;

		// Token: 0x04003EE9 RID: 16105
		[SerializeField]
		[EventRef]
		private string m_summonTellStartAudioPath = string.Empty;

		// Token: 0x04003EEA RID: 16106
		[SerializeField]
		[EventRef]
		private string m_summonTellCompleteAudioPath = string.Empty;

		// Token: 0x04003EEB RID: 16107
		[SerializeField]
		[EventRef]
		private string m_summonAttackStartAudioPath = string.Empty;

		// Token: 0x04003EEC RID: 16108
		[SerializeField]
		[EventRef]
		private string m_summonAttackCompleteAudioPath = string.Empty;

		// Token: 0x04003EED RID: 16109
		[SerializeField]
		[EventRef]
		private string m_summonSpawnPortalAudioPath = string.Empty;

		// Token: 0x04003EEE RID: 16110
		[SerializeField]
		[EventRef]
		private string m_summonDespawnPortalAudioPath = string.Empty;

		// Token: 0x04003EEF RID: 16111
		private Animator m_animator;

		// Token: 0x04003EF0 RID: 16112
		private BossRoomController m_bossRoomController;

		// Token: 0x04003EF1 RID: 16113
		private EnemyController m_enemyController;

		// Token: 0x04003EF2 RID: 16114
		private string m_description = string.Empty;

		// Token: 0x04003EF3 RID: 16115
		private DancingBossAudioController.AttackStateID m_previousAttackState;

		// Token: 0x04003EF4 RID: 16116
		private const string HOOP_PARAMETER = "dancingBoss_idleSpeed";

		// Token: 0x04003EF5 RID: 16117
		private const string FLYING_SPEED_PARAMETER = "danceBoss_flySpeed";

		// Token: 0x04003EF6 RID: 16118
		private const int INITIAL_WAVE_AUDIO_SOURCE_COUNT = 4;

		// Token: 0x04003EF7 RID: 16119
		private const string PHASE_ANIMATOR_PARAMETER = "Phase";

		// Token: 0x04003EF8 RID: 16120
		private DancingBossAudioController.FlyingStateID m_currentFlyingState;

		// Token: 0x04003EF9 RID: 16121
		private Vector2 m_previousFlightDirection;

		// Token: 0x04003EFA RID: 16122
		private bool m_isIntroComplete;

		// Token: 0x04003EFB RID: 16123
		private List<GameObject> m_waveShootLoopAudioSources;

		// Token: 0x04003EFC RID: 16124
		private WaitForSeconds m_waitUntilSummonPortalDespawnStart = new WaitForSeconds(1.2f);

		// Token: 0x04003EFD RID: 16125
		private Vector3 m_spreadAttackSummonPosition;

		// Token: 0x04003EFE RID: 16126
		private EventInstance m_hoopAudioEventInstance;

		// Token: 0x04003EFF RID: 16127
		private EventInstance m_flyingAudioEventInstance;

		// Token: 0x04003F00 RID: 16128
		private EventInstance m_spreadAttackSummoningAudioEventInstance;

		// Token: 0x04003F01 RID: 16129
		private EventInstance m_summonAttackAudioEventInstance;

		// Token: 0x04003F02 RID: 16130
		private EventInstance m_spreadAttackEventInstance;

		// Token: 0x04003F03 RID: 16131
		private EventInstance m_waveAttackEventInstance;

		// Token: 0x04003F04 RID: 16132
		private EventInstance m_summoningPortalsEventInstance;

		// Token: 0x04003F05 RID: 16133
		private Action m_onBossDefeated;

		// Token: 0x04003F06 RID: 16134
		private Action m_onSentrySummonStart;

		// Token: 0x04003F07 RID: 16135
		private Action m_onSentrySummonComplete;

		// Token: 0x04003F08 RID: 16136
		private Action<DancingBossWaveFiredEventArgs> m_onWaveFired;

		// Token: 0x04003F09 RID: 16137
		private Action<Vector3> m_onSpreadAttackStart;

		// Token: 0x04003F0A RID: 16138
		private Action m_onSpreadAttackComplete;

		// Token: 0x04003F0B RID: 16139
		private Action m_onSummonShotsTellStart;

		// Token: 0x04003F0C RID: 16140
		private Action m_onSummonShotsTellComplete;

		// Token: 0x04003F0D RID: 16141
		private Action m_onSummonShotsAttackStart;

		// Token: 0x04003F0E RID: 16142
		private Action m_onSummonShotsAttackComplete;

		// Token: 0x04003F0F RID: 16143
		private Action<Vector2> m_onSummonShotsPortalOpened;

		// Token: 0x02000EE1 RID: 3809
		private enum AttackStateID
		{
			// Token: 0x04005993 RID: 22931
			None,
			// Token: 0x04005994 RID: 22932
			TellIntro,
			// Token: 0x04005995 RID: 22933
			TellHold,
			// Token: 0x04005996 RID: 22934
			AttackIntro,
			// Token: 0x04005997 RID: 22935
			AttackHold,
			// Token: 0x04005998 RID: 22936
			AttackExit
		}

		// Token: 0x02000EE2 RID: 3810
		private enum FlyingStateID
		{
			// Token: 0x0400599A RID: 22938
			Stopped,
			// Token: 0x0400599B RID: 22939
			Flying
		}
	}
}
