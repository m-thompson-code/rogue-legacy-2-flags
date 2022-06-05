using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E6F RID: 3695
	public class LandEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06006836 RID: 26678 RVA: 0x00039AA9 File Offset: 0x00037CA9
		private void InitializeImpactParameter()
		{
			if (this.m_useSurfaceParameter)
			{
				this.m_impactParameterID = AnimBehaviourEventEmitterUtility.GetParameterID(this.m_eventDescription, "Impact");
			}
			this.m_isImpactParameterInitialized = true;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x0017F134 File Offset: 0x0017D334
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (!this.m_isImpactParameterInitialized)
			{
				this.InitializeImpactParameter();
			}
			if (!this.m_hasSearchedForCorgiController && this.m_corgiController == null)
			{
				this.m_hasSearchedForCorgiController = true;
				this.m_corgiController = animator.GetComponent<CorgiController_RL>();
				if (this.m_corgiController == null)
				{
					Debug.LogFormat("<color=red>| {0} | Missing CorgiController_RL component on ({1}). If you see this message, please add a bug report to Pivotal.</color>", new object[]
					{
						this,
						animator.name
					});
				}
			}
			if (this.m_corgiController != null)
			{
				this.m_eventInstance.setParameterByID(this.m_impactParameterID, this.GetFallSpeedAsAPercentageOfTerminalVelocity(), false);
				base.Play(animator);
			}
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x0017F1DC File Offset: 0x0017D3DC
		private float GetFallSpeedAsAPercentageOfTerminalVelocity()
		{
			float num = Mathf.Abs(this.m_corgiController.Velocity.y);
			float result = 1f;
			if (num < Mathf.Abs(Global_EV.TerminalVelocity))
			{
				result = num / Mathf.Abs(Global_EV.TerminalVelocity);
			}
			return result;
		}

		// Token: 0x040054A5 RID: 21669
		private const string IMPACT_PARAMETER_NAME = "Impact";

		// Token: 0x040054A6 RID: 21670
		private bool m_isImpactParameterInitialized;

		// Token: 0x040054A7 RID: 21671
		private PARAMETER_ID m_impactParameterID;

		// Token: 0x040054A8 RID: 21672
		private bool m_hasSearchedForCorgiController;

		// Token: 0x040054A9 RID: 21673
		private CorgiController_RL m_corgiController;
	}
}
