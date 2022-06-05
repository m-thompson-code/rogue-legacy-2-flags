using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E98 RID: 3736
	public class SurfaceAudioController : MonoBehaviour
	{
		// Token: 0x17002185 RID: 8581
		// (get) Token: 0x06006954 RID: 26964 RVA: 0x0003A6F1 File Offset: 0x000388F1
		// (set) Token: 0x06006955 RID: 26965 RVA: 0x0003A6F9 File Offset: 0x000388F9
		public EventHandler<float> StandingOnParameterChangeEvent { get; set; }

		// Token: 0x17002186 RID: 8582
		// (get) Token: 0x06006956 RID: 26966 RVA: 0x0003A702 File Offset: 0x00038902
		// (set) Token: 0x06006957 RID: 26967 RVA: 0x0003A70A File Offset: 0x0003890A
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

		// Token: 0x17002187 RID: 8583
		// (get) Token: 0x06006958 RID: 26968 RVA: 0x0003A713 File Offset: 0x00038913
		// (set) Token: 0x06006959 RID: 26969 RVA: 0x0003A71B File Offset: 0x0003891B
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

		// Token: 0x17002188 RID: 8584
		// (get) Token: 0x0600695A RID: 26970 RVA: 0x0003A724 File Offset: 0x00038924
		// (set) Token: 0x0600695B RID: 26971 RVA: 0x0003A72C File Offset: 0x0003892C
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

		// Token: 0x0600695C RID: 26972 RVA: 0x00182124 File Offset: 0x00180324
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

		// Token: 0x0600695D RID: 26973 RVA: 0x00182180 File Offset: 0x00180380
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

		// Token: 0x0600695E RID: 26974 RVA: 0x001821E8 File Offset: 0x001803E8
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

		// Token: 0x040055C1 RID: 21953
		private CorgiController_RL m_corgiController;

		// Token: 0x040055C2 RID: 21954
		private SurfaceType m_standingOnSurfaceType = SurfaceType.None;

		// Token: 0x040055C3 RID: 21955
		private SurfaceType m_surfaceOverride = SurfaceType.None;

		// Token: 0x040055C4 RID: 21956
		private float m_surfaceTypeParameter;
	}
}
