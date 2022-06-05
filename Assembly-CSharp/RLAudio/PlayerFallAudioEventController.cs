using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E82 RID: 3714
	public class PlayerFallAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002160 RID: 8544
		// (get) Token: 0x060068BE RID: 26814 RVA: 0x00039FB3 File Offset: 0x000381B3
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

		// Token: 0x060068BF RID: 26815 RVA: 0x00180BCC File Offset: 0x0017EDCC
		private void Awake()
		{
			this.m_corgiController = base.GetComponent<CorgiController_RL>();
			this.m_corgiController.CorgiStartFallingRelay.AddListener(new Action<CorgiController_RL>(this.OnPlayerStartFalling), false);
			this.m_corgiController.OnCorgiLandedEnterRelay.AddListener(new Action<CorgiController_RL>(this.OnPlayerLanded), false);
			this.m_fallingEventInstance = AudioUtility.GetEventInstance(this.m_fallingEventPath, base.transform);
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x00039FD9 File Offset: 0x000381D9
		private void OnDestroy()
		{
			if (this.m_fallingEventInstance.isValid())
			{
				this.m_fallingEventInstance.release();
			}
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x00039FF4 File Offset: 0x000381F4
		private void OnPlayerStartFalling(CorgiController_RL corgiController)
		{
			this.m_isFalling = true;
			AudioManager.PlayAttached(this, this.m_fallingEventInstance, base.gameObject);
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x0003A00F File Offset: 0x0003820F
		private void OnPlayerLanded(CorgiController_RL corgiController)
		{
			this.m_isFalling = false;
			AudioManager.Stop(this.m_fallingEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x00180C38 File Offset: 0x0017EE38
		private void Update()
		{
			if (this.m_corgiController == null || !this.m_isFalling)
			{
				return;
			}
			if (this.m_fallingEventInstance.isValid())
			{
				float num = Mathf.Abs(this.m_corgiController.Velocity.y);
				float num2 = Mathf.Abs(Global_EV.TerminalVelocity);
				float value = num / num2;
				value = Mathf.Clamp(value, 0f, 1f);
				this.m_fallingEventInstance.setParameterByName("fallSpeed", value, false);
			}
		}

		// Token: 0x04005537 RID: 21815
		[SerializeField]
		[EventRef]
		private string m_fallingEventPath;

		// Token: 0x04005538 RID: 21816
		private CorgiController_RL m_corgiController;

		// Token: 0x04005539 RID: 21817
		private EventInstance m_fallingEventInstance;

		// Token: 0x0400553A RID: 21818
		private bool m_isFalling;

		// Token: 0x0400553B RID: 21819
		private string m_description = string.Empty;

		// Token: 0x0400553C RID: 21820
		private const string FALL_SPEED_PARAMETER = "fallSpeed";
	}
}
