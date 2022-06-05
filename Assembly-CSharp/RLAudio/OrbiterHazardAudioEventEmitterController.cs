using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000903 RID: 2307
	public class OrbiterHazardAudioEventEmitterController : MonoBehaviour
	{
		// Token: 0x06004BB8 RID: 19384 RVA: 0x00110341 File Offset: 0x0010E541
		private void Awake()
		{
			this.m_orbiter = base.GetComponent<Orbiter_Hazard>();
			this.m_swingAngle = 90f;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0011035A File Offset: 0x0010E55A
		private void OnEnable()
		{
			this.m_hasSwingAudioPlayed = false;
			this.m_hasExpansionCompleteAudioPlayed = false;
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0011036A File Offset: 0x0010E56A
		private void Update()
		{
			if (this.m_orbiter == null)
			{
				this.m_orbiter = base.GetComponent<Orbiter_Hazard>();
			}
			this.PlayExpansionAudio();
			this.PlaySwingAudio();
			this.PlayOrbiterProximityAudio();
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00110398 File Offset: 0x0010E598
		private void PlayOrbiterProximityAudio()
		{
			if (!PlayerManager.IsInstantiated)
			{
				return;
			}
			Vector2 a = PlayerManager.GetPlayerController().transform.position;
			Vector2 b = this.m_orbiter.OrbiterBall.transform.position;
			float value = (a - b).sqrMagnitude / this.m_maxSquareMagnitude;
			value = Mathf.Clamp(value, 0f, 1f);
			this.m_orbiterAudioEventEmitter.SetParameter("orbiterProximity", value, false);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00110418 File Offset: 0x0010E618
		private void PlayExpansionAudio()
		{
			if (this.m_orbiter.ExpansionPercent < 1f)
			{
				this.m_expansionAudioEventEmitter.SetParameter("chainLength", this.m_orbiter.ExpansionPercent, false);
				return;
			}
			this.m_expansionAudioEventEmitter.Stop();
			if (!this.m_hasExpansionCompleteAudioPlayed)
			{
				this.m_hasExpansionCompleteAudioPlayed = true;
				this.m_expansionCompleteAudioEventEmitter.Play();
			}
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x0011047C File Offset: 0x0010E67C
		private void PlaySwingAudio()
		{
			float num = this.m_orbiter.CurrentRotation - this.m_orbiter.InitialRotation;
			if (this.m_hasSwingAudioPlayed)
			{
				if (num <= 350f)
				{
					this.m_hasSwingAudioPlayed = false;
					return;
				}
			}
			else if (num > 350f)
			{
				this.m_hasSwingAudioPlayed = true;
				this.m_swingAudioEventEmitter.Play();
			}
		}

		// Token: 0x04003FBE RID: 16318
		[SerializeField]
		private StudioEventEmitter m_expansionAudioEventEmitter;

		// Token: 0x04003FBF RID: 16319
		[SerializeField]
		private StudioEventEmitter m_expansionCompleteAudioEventEmitter;

		// Token: 0x04003FC0 RID: 16320
		[SerializeField]
		private StudioEventEmitter m_swingAudioEventEmitter;

		// Token: 0x04003FC1 RID: 16321
		[SerializeField]
		private StudioEventEmitter m_orbiterAudioEventEmitter;

		// Token: 0x04003FC2 RID: 16322
		[SerializeField]
		private float m_maxSquareMagnitude = 100f;

		// Token: 0x04003FC3 RID: 16323
		private const string EXPANSION_PARAMETER_NAME = "chainLength";

		// Token: 0x04003FC4 RID: 16324
		private const int FULL_ROTATION_VALUE = 350;

		// Token: 0x04003FC5 RID: 16325
		private const string ORBITER_PROXIMITY_PARAMETER_NAME = "orbiterProximity";

		// Token: 0x04003FC6 RID: 16326
		private Orbiter_Hazard m_orbiter;

		// Token: 0x04003FC7 RID: 16327
		private float m_swingAngle;

		// Token: 0x04003FC8 RID: 16328
		private bool m_hasSwingAudioPlayed;

		// Token: 0x04003FC9 RID: 16329
		private bool m_hasExpansionCompleteAudioPlayed;
	}
}
