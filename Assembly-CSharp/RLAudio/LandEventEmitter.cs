using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F8 RID: 2296
	public class LandEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x0010F058 File Offset: 0x0010D258
		private void InitializeImpactParameter()
		{
			if (this.m_useSurfaceParameter)
			{
				this.m_impactParameterID = AnimBehaviourEventEmitterUtility.GetParameterID(this.m_eventDescription, "Impact");
			}
			this.m_isImpactParameterInitialized = true;
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x0010F080 File Offset: 0x0010D280
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

		// Token: 0x06004B61 RID: 19297 RVA: 0x0010F128 File Offset: 0x0010D328
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

		// Token: 0x04003F61 RID: 16225
		private const string IMPACT_PARAMETER_NAME = "Impact";

		// Token: 0x04003F62 RID: 16226
		private bool m_isImpactParameterInitialized;

		// Token: 0x04003F63 RID: 16227
		private PARAMETER_ID m_impactParameterID;

		// Token: 0x04003F64 RID: 16228
		private bool m_hasSearchedForCorgiController;

		// Token: 0x04003F65 RID: 16229
		private CorgiController_RL m_corgiController;
	}
}
