using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

// Token: 0x020006EF RID: 1775
public class VirtualCameraShakeEffect : BaseEffect
{
	// Token: 0x06003642 RID: 13890 RVA: 0x0001DCCC File Offset: 0x0001BECC
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x000E39A4 File Offset: 0x000E1BA4
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

	// Token: 0x06003644 RID: 13892 RVA: 0x0001DCE5 File Offset: 0x0001BEE5
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

	// Token: 0x06003645 RID: 13893 RVA: 0x0001DCFB File Offset: 0x0001BEFB
	public override void Stop(EffectStopType stopType)
	{
		base.StopAllCoroutines();
		this.ShakeVirtualCamera(0f, 0f);
		this.m_activeVcam = null;
		RumbleManager.StopRumble(true, true);
		this.PlayComplete();
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x0001DD27 File Offset: 0x0001BF27
	private void ShakeVirtualCamera(float shakeSpeed, float shakeSize)
	{
		if (this.m_activeNoise != null)
		{
			this.m_activeNoise.m_FrequencyGain = shakeSpeed;
			this.m_activeNoise.m_AmplitudeGain = shakeSize;
		}
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x0001DD4F File Offset: 0x0001BF4F
	protected override void OnDisable()
	{
		this.ShakeVirtualCamera(0f, 0f);
		RumbleManager.StopRumble(true, true);
		base.OnDisable();
	}

	// Token: 0x04002C11 RID: 11281
	private CinemachineVirtualCamera m_activeVcam;

	// Token: 0x04002C12 RID: 11282
	private CinemachineConfiner_RL m_activeConfiner;

	// Token: 0x04002C13 RID: 11283
	private CinemachineBasicMultiChannelPerlin m_activeNoise;

	// Token: 0x04002C14 RID: 11284
	[SerializeField]
	private float m_shakeSpeed;

	// Token: 0x04002C15 RID: 11285
	[SerializeField]
	private float m_shakeAmplitude;

	// Token: 0x04002C16 RID: 11286
	private WaitRL_Yield m_waitYield;
}
