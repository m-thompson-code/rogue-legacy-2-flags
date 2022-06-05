using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200091A RID: 2330
	public class SurfaceAudioController : MonoBehaviour
	{
		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x00112365 File Offset: 0x00110565
		// (set) Token: 0x06004C54 RID: 19540 RVA: 0x0011236D File Offset: 0x0011056D
		public EventHandler<float> StandingOnParameterChangeEvent { get; set; }

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x00112376 File Offset: 0x00110576
		// (set) Token: 0x06004C56 RID: 19542 RVA: 0x0011237E File Offset: 0x0011057E
		public float SurfaceTypeParameter
		{
			get
			{
				return this.m_surfaceTypeParameter;
			}
			private set
			{
				this.m_surfaceTypeParameter = value;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x06004C57 RID: 19543 RVA: 0x00112387 File Offset: 0x00110587
		// (set) Token: 0x06004C58 RID: 19544 RVA: 0x0011238F File Offset: 0x0011058F
		public SurfaceType StandingOnSurfaceType
		{
			get
			{
				return this.m_standingOnSurfaceType;
			}
			private set
			{
				this.m_standingOnSurfaceType = value;
			}
		}

		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x00112398 File Offset: 0x00110598
		// (set) Token: 0x06004C5A RID: 19546 RVA: 0x001123A0 File Offset: 0x001105A0
		public SurfaceType SurfaceOverride
		{
			get
			{
				return this.m_surfaceOverride;
			}
			private set
			{
				this.m_surfaceOverride = value;
			}
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x001123AC File Offset: 0x001105AC
		private void Awake()
		{
			this.m_corgiController = base.GetComponent<CorgiController_RL>();
			if (this.m_corgiController != null)
			{
				this.m_corgiController.StandingOnChangeRelay.AddListener(new Action<object, GameObject>(this.OnStandingOnChange), false);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Missing Corgi Controller Component. If you see this message, please add a bug report to Pivotal</color>", new object[]
			{
				this
			});
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x00112408 File Offset: 0x00110608
		private void OnStandingOnChange(object sender, GameObject standingOn)
		{
			if (this.SurfaceOverride == SurfaceType.None)
			{
				this.StandingOnSurfaceType = SurfaceType.Default;
				Surface componentInParent = standingOn.GetComponentInParent<Surface>();
				if (componentInParent != null)
				{
					this.StandingOnSurfaceType = componentInParent.SurfaceType;
				}
				this.SurfaceTypeParameter = SurfaceAudioManager.GetSurfaceParameter(this.StandingOnSurfaceType);
				if (this.StandingOnParameterChangeEvent != null)
				{
					this.StandingOnParameterChangeEvent(this, this.SurfaceTypeParameter);
				}
			}
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x00112470 File Offset: 0x00110670
		public void SetSurfaceOverride(SurfaceType surfaceType)
		{
			if (this.SurfaceOverride != surfaceType)
			{
				this.SurfaceOverride = surfaceType;
				if (surfaceType != SurfaceType.None)
				{
					this.SurfaceTypeParameter = SurfaceAudioManager.GetSurfaceParameter(this.SurfaceOverride);
				}
				else
				{
					this.SurfaceTypeParameter = SurfaceAudioManager.GetSurfaceParameter(this.StandingOnSurfaceType);
				}
				if (this.StandingOnParameterChangeEvent != null)
				{
					this.StandingOnParameterChangeEvent(this, this.SurfaceTypeParameter);
				}
			}
		}

		// Token: 0x04004059 RID: 16473
		private CorgiController_RL m_corgiController;

		// Token: 0x0400405A RID: 16474
		private SurfaceType m_standingOnSurfaceType = SurfaceType.None;

		// Token: 0x0400405B RID: 16475
		private SurfaceType m_surfaceOverride = SurfaceType.None;

		// Token: 0x0400405C RID: 16476
		private float m_surfaceTypeParameter;
	}
}
