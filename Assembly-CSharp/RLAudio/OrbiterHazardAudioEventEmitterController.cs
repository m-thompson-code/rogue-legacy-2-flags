using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E7F RID: 3711
	public class OrbiterHazardAudioEventEmitterController : MonoBehaviour
	{
		// Token: 0x060068AD RID: 26797 RVA: 0x00039F32 File Offset: 0x00038132
		private void Awake()
		{
			this.m_orbiter = base.GetComponent<Orbiter_Hazard>();
			this.m_swingAngle = 90f;
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x00039F4B File Offset: 0x0003814B
		private void OnEnable()
		{
			this.m_hasSwingAudioPlayed = false;
			this.m_hasExpansionCompleteAudioPlayed = false;
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x00039F5B File Offset: 0x0003815B
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

		// Token: 0x060068B0 RID: 26800 RVA: 0x00180710 File Offset: 0x0017E910
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

		// Token: 0x060068B1 RID: 26801 RVA: 0x00180790 File Offset: 0x0017E990
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

		// Token: 0x060068B2 RID: 26802 RVA: 0x001807F4 File Offset: 0x0017E9F4
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

		// Token: 0x0400551B RID: 21787
		[SerializeField]
		private StudioEventEmitter m_expansionAudioEventEmitter;

		// Token: 0x0400551C RID: 21788
		[SerializeField]
		private StudioEventEmitter m_expansionCompleteAudioEventEmitter;

		// Token: 0x0400551D RID: 21789
		[SerializeField]
		private StudioEventEmitter m_swingAudioEventEmitter;

		// Token: 0x0400551E RID: 21790
		[SerializeField]
		private StudioEventEmitter m_orbiterAudioEventEmitter;

		// Token: 0x0400551F RID: 21791
		[SerializeField]
		private float m_maxSquareMagnitude = 100f;

		// Token: 0x04005520 RID: 21792
		private const string EXPANSION_PARAMETER_NAME = "chainLength";

		// Token: 0x04005521 RID: 21793
		private const int FULL_ROTATION_VALUE = 350;

		// Token: 0x04005522 RID: 21794
		private const string ORBITER_PROXIMITY_PARAMETER_NAME = "orbiterProximity";

		// Token: 0x04005523 RID: 21795
		private Orbiter_Hazard m_orbiter;

		// Token: 0x04005524 RID: 21796
		private float m_swingAngle;

		// Token: 0x04005525 RID: 21797
		private bool m_hasSwingAudioPlayed;

		// Token: 0x04005526 RID: 21798
		private bool m_hasExpansionCompleteAudioPlayed;
	}
}
