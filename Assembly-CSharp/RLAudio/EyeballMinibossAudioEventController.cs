using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008EF RID: 2287
	public class EyeballMinibossAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0010E683 File Offset: 0x0010C883
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_description))
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x0010E6A4 File Offset: 0x0010C8A4
		private void Start()
		{
			this.m_enemyController = base.GetComponent<EnemyController>();
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x0010E6B4 File Offset: 0x0010C8B4
		private void Update()
		{
			if (!this.m_enemyController)
			{
				return;
			}
			this.m_currentHeading.x = this.m_enemyController.HeadingX;
			this.m_currentHeading.y = this.m_enemyController.HeadingY;
			if (this.m_previousHeading != this.m_currentHeading)
			{
				if (!this.m_movementEventEmitter.IsPlaying())
				{
					this.m_movementEventEmitter.Play();
				}
				float value = Mathf.Clamp((this.m_currentHeading - this.m_previousHeading).magnitude, 0f, this.m_maxSpeed) / this.m_maxSpeed;
				this.m_movementEventEmitter.SetParameter("moveSpeed", value, false);
				this.m_previousHeading = this.m_currentHeading;
				return;
			}
			if (this.m_movementEventEmitter.IsPlaying())
			{
				this.m_movementEventEmitter.Stop();
				this.m_movementStopEventEmitter.Play();
			}
		}

		// Token: 0x04003F32 RID: 16178
		[SerializeField]
		[Tooltip("This is the max move speed per frame. This is used to determine a normalised 'how fast is the Eye moving?' value which is then used as a parameter in its movement audio.")]
		private float m_maxSpeed = 1f;

		// Token: 0x04003F33 RID: 16179
		[SerializeField]
		private StudioEventEmitter m_movementEventEmitter;

		// Token: 0x04003F34 RID: 16180
		[SerializeField]
		private StudioEventEmitter m_movementStopEventEmitter;

		// Token: 0x04003F35 RID: 16181
		private string m_description;

		// Token: 0x04003F36 RID: 16182
		private EnemyController m_enemyController;

		// Token: 0x04003F37 RID: 16183
		private Vector2 m_previousHeading;

		// Token: 0x04003F38 RID: 16184
		private Vector2 m_currentHeading;

		// Token: 0x04003F39 RID: 16185
		private const string MOVE_SPEED_PARAMETER = "moveSpeed";
	}
}
