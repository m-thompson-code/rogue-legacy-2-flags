using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

// Token: 0x0200042A RID: 1066
public class VirtualCameraShakeEffect : BaseEffect
{
	// Token: 0x06002745 RID: 10053 RVA: 0x00082CAC File Offset: 0x00080EAC
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x00082CC8 File Offset: 0x00080EC8
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		if (CameraController.CinemachineBrain && !CameraController.CinemachineBrain.ActiveVirtualCamera.IsNativeNull())
		{
			this.m_activeVcam = (CameraController.CinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera);
			if (this.m_activeVcam)
			{
				this.m_activeNoise = this.m_activeVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
				this.m_activeConfiner = this.m_activeVcam.GetComponent<CinemachineConfiner_RL>();
			}
		}
		if (this.m_activeVcam && this.m_activeNoise && this.m_activeConfiner)
		{
			base.StartCoroutine(this.PlayTimedCameraShake(duration));
			return;
		}
		this.PlayComplete();
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x00082D7A File Offset: 0x00080F7A
	private IEnumerator PlayTimedCameraShake(float duration)
	{
		this.ShakeVirtualCamera(this.m_shakeSpeed, this.m_shakeAmplitude);
		RumbleManager.StartRumble(true, true, 0.5f, duration, true);
		if (duration > 0f)
		{
			this.m_waitYield.CreateNew(duration, true);
			yield return this.m_waitYield;
			this.Stop(EffectStopType.Gracefully);
		}
		else
		{
			yield return null;
			if (base.SourceAnimator && duration == -1f)
			{
				int nameHash = base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash;
				while (base.SourceAnimator && base.SourceAnimator.gameObject.activeInHierarchy)
				{
					if (base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash != nameHash)
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				while (base.Source && base.Source.gameObject.activeInHierarchy)
				{
					yield return null;
				}
			}
			this.Stop(EffectStopType.Gracefully);
		}
		yield break;
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x00082D90 File Offset: 0x00080F90
	public override void Stop(EffectStopType stopType)
	{
		base.StopAllCoroutines();
		this.ShakeVirtualCamera(0f, 0f);
		this.m_activeVcam = null;
		RumbleManager.StopRumble(true, true);
		this.PlayComplete();
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x00082DBC File Offset: 0x00080FBC
	private void ShakeVirtualCamera(float shakeSpeed, float shakeSize)
	{
		if (this.m_activeNoise != null)
		{
			this.m_activeNoise.m_FrequencyGain = shakeSpeed;
			this.m_activeNoise.m_AmplitudeGain = shakeSize;
		}
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x00082DE4 File Offset: 0x00080FE4
	protected override void OnDisable()
	{
		this.ShakeVirtualCamera(0f, 0f);
		RumbleManager.StopRumble(true, true);
		base.OnDisable();
	}

	// Token: 0x040020F3 RID: 8435
	private CinemachineVirtualCamera m_activeVcam;

	// Token: 0x040020F4 RID: 8436
	private CinemachineConfiner_RL m_activeConfiner;

	// Token: 0x040020F5 RID: 8437
	private CinemachineBasicMultiChannelPerlin m_activeNoise;

	// Token: 0x040020F6 RID: 8438
	[SerializeField]
	private float m_shakeSpeed;

	// Token: 0x040020F7 RID: 8439
	[SerializeField]
	private float m_shakeAmplitude;

	// Token: 0x040020F8 RID: 8440
	private WaitRL_Yield m_waitYield;
}
