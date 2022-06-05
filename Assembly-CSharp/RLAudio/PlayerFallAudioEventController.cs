using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000906 RID: 2310
	public class PlayerFallAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06004BC9 RID: 19401 RVA: 0x0011087F File Offset: 0x0010EA7F
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

		// Token: 0x06004BCA RID: 19402 RVA: 0x001108A8 File Offset: 0x0010EAA8
		private void Awake()
		{
			this.m_corgiController = base.GetComponent<CorgiController_RL>();
			this.m_corgiController.CorgiStartFallingRelay.AddListener(new Action<CorgiController_RL>(this.OnPlayerStartFalling), false);
			this.m_corgiController.OnCorgiLandedEnterRelay.AddListener(new Action<CorgiController_RL>(this.OnPlayerLanded), false);
			this.m_fallingEventInstance = AudioUtility.GetEventInstance(this.m_fallingEventPath, base.transform);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00110914 File Offset: 0x0010EB14
		private void OnDestroy()
		{
			if (this.m_fallingEventInstance.isValid())
			{
				this.m_fallingEventInstance.release();
			}
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x0011092F File Offset: 0x0010EB2F
		private void OnPlayerStartFalling(CorgiController_RL corgiController)
		{
			this.m_isFalling = true;
			AudioManager.PlayAttached(this, this.m_fallingEventInstance, base.gameObject);
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x0011094A File Offset: 0x0010EB4A
		private void OnPlayerLanded(CorgiController_RL corgiController)
		{
			this.m_isFalling = false;
			AudioManager.Stop(this.m_fallingEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00110960 File Offset: 0x0010EB60
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

		// Token: 0x04003FDA RID: 16346
		[SerializeField]
		[EventRef]
		private string m_fallingEventPath;

		// Token: 0x04003FDB RID: 16347
		private CorgiController_RL m_corgiController;

		// Token: 0x04003FDC RID: 16348
		private EventInstance m_fallingEventInstance;

		// Token: 0x04003FDD RID: 16349
		private bool m_isFalling;

		// Token: 0x04003FDE RID: 16350
		private string m_description = string.Empty;

		// Token: 0x04003FDF RID: 16351
		private const string FALL_SPEED_PARAMETER = "fallSpeed";
	}
}
