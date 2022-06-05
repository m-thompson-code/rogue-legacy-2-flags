using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E92 RID: 3730
	public class SpearSpinAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700217F RID: 8575
		// (get) Token: 0x06006937 RID: 26935 RVA: 0x0003A5F8 File Offset: 0x000387F8
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

		// Token: 0x06006938 RID: 26936 RVA: 0x001819FC File Offset: 0x0017FBFC
		private void Awake()
		{
			SpearSpin_Ability component = base.GetComponent<SpearSpin_Ability>();
			component.BeginCastingRelay.AddListener(new Action(this.OnSpearSpinStart), false);
			component.HitProjectileRelay.AddListener(new Action(this.OnProjectileHit), false);
			component.SpinCompleteRelay.AddListener(new Action<bool>(this.OnSpinComplete), false);
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x00181A5C File Offset: 0x0017FC5C
		private void OnDestroy()
		{
			SpearSpin_Ability component = base.GetComponent<SpearSpin_Ability>();
			component.BeginCastingRelay.RemoveListener(new Action(this.OnSpearSpinStart));
			component.HitProjectileRelay.RemoveListener(new Action(this.OnProjectileHit));
			component.SpinCompleteRelay.RemoveListener(new Action<bool>(this.OnSpinComplete));
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x00181AB8 File Offset: 0x0017FCB8
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

		// Token: 0x0600693B RID: 26939 RVA: 0x0003A61E File Offset: 0x0003881E
		private void OnSpearSpinStart()
		{
			AudioManager.PlayOneShotAttached(this, this.m_spinStartAudioPath, base.gameObject);
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x00181B00 File Offset: 0x0017FD00
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

		// Token: 0x0400558C RID: 21900
		[SerializeField]
		[EventRef]
		private string m_spinStartAudioPath;

		// Token: 0x0400558D RID: 21901
		[SerializeField]
		[EventRef]
		private string m_singleProjectileHitAudioPath;

		// Token: 0x0400558E RID: 21902
		[SerializeField]
		[EventRef]
		private string m_multipleProjectilesHitAudioPath;

		// Token: 0x0400558F RID: 21903
		[SerializeField]
		[EventRef]
		private string m_abilitySuccessAudioPath;

		// Token: 0x04005590 RID: 21904
		[SerializeField]
		[EventRef]
		private string m_abilityFailAudioPath;

		// Token: 0x04005591 RID: 21905
		[SerializeField]
		private bool m_playSuccessFailAudio = true;

		// Token: 0x04005592 RID: 21906
		[SerializeField]
		private float m_timeBetweenProjectileHits = 0.1f;

		// Token: 0x04005593 RID: 21907
		private float m_timeOfProjectileLastHit;

		// Token: 0x04005594 RID: 21908
		private string m_description = string.Empty;
	}
}
