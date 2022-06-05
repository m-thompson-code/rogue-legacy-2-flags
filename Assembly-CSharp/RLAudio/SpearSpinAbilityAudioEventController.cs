using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000915 RID: 2325
	public class SpearSpinAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x00111C9E File Offset: 0x0010FE9E
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

		// Token: 0x06004C3D RID: 19517 RVA: 0x00111CC4 File Offset: 0x0010FEC4
		private void Awake()
		{
			SpearSpin_Ability component = base.GetComponent<SpearSpin_Ability>();
			component.BeginCastingRelay.AddListener(new Action(this.OnSpearSpinStart), false);
			component.HitProjectileRelay.AddListener(new Action(this.OnProjectileHit), false);
			component.SpinCompleteRelay.AddListener(new Action<bool>(this.OnSpinComplete), false);
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x00111D24 File Offset: 0x0010FF24
		private void OnDestroy()
		{
			SpearSpin_Ability component = base.GetComponent<SpearSpin_Ability>();
			component.BeginCastingRelay.RemoveListener(new Action(this.OnSpearSpinStart));
			component.HitProjectileRelay.RemoveListener(new Action(this.OnProjectileHit));
			component.SpinCompleteRelay.RemoveListener(new Action<bool>(this.OnSpinComplete));
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x00111D80 File Offset: 0x0010FF80
		private void OnProjectileHit()
		{
			float time = Time.time;
			string path = this.m_singleProjectileHitAudioPath;
			if (this.m_timeOfProjectileLastHit - time <= this.m_timeBetweenProjectileHits)
			{
				path = this.m_multipleProjectilesHitAudioPath;
			}
			AudioManager.PlayOneShotAttached(this, path, base.gameObject);
			this.m_timeOfProjectileLastHit = time;
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x00111DC5 File Offset: 0x0010FFC5
		private void OnSpearSpinStart()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spinStartAudioPath, base.gameObject);
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x00111DDC File Offset: 0x0010FFDC
		private void OnSpinComplete(bool hitProjectileWhileActive)
		{
			if (this.m_playSuccessFailAudio)
			{
				string path = this.m_abilityFailAudioPath;
				if (hitProjectileWhileActive)
				{
					path = this.m_abilitySuccessAudioPath;
				}
				AudioManager.PlayOneShotAttached(this, path, base.gameObject);
			}
		}

		// Token: 0x0400402C RID: 16428
		[SerializeField]
		[EventRef]
		private string m_spinStartAudioPath;

		// Token: 0x0400402D RID: 16429
		[SerializeField]
		[EventRef]
		private string m_singleProjectileHitAudioPath;

		// Token: 0x0400402E RID: 16430
		[SerializeField]
		[EventRef]
		private string m_multipleProjectilesHitAudioPath;

		// Token: 0x0400402F RID: 16431
		[SerializeField]
		[EventRef]
		private string m_abilitySuccessAudioPath;

		// Token: 0x04004030 RID: 16432
		[SerializeField]
		[EventRef]
		private string m_abilityFailAudioPath;

		// Token: 0x04004031 RID: 16433
		[SerializeField]
		private bool m_playSuccessFailAudio = true;

		// Token: 0x04004032 RID: 16434
		[SerializeField]
		private float m_timeBetweenProjectileHits = 0.1f;

		// Token: 0x04004033 RID: 16435
		private float m_timeOfProjectileLastHit;

		// Token: 0x04004034 RID: 16436
		private string m_description = string.Empty;
	}
}
