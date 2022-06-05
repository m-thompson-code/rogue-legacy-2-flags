using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E64 RID: 3684
	public class EyeballMinibossAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700213E RID: 8510
		// (get) Token: 0x060067F6 RID: 26614 RVA: 0x00039824 File Offset: 0x00037A24
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

		// Token: 0x060067F7 RID: 26615 RVA: 0x00039845 File Offset: 0x00037A45
		private void Start()
		{
			this.m_enemyController = base.GetComponent<EnemyController>();
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x0017E878 File Offset: 0x0017CA78
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

		// Token: 0x0400546C RID: 21612
		[SerializeField]
		[Tooltip("This is the max move speed per frame. This is used to determine a normalised 'how fast is the Eye moving?' value which is then used as a parameter in its movement audio.")]
		private float m_maxSpeed = 1f;

		// Token: 0x0400546D RID: 21613
		[SerializeField]
		private StudioEventEmitter m_movementEventEmitter;

		// Token: 0x0400546E RID: 21614
		[SerializeField]
		private StudioEventEmitter m_movementStopEventEmitter;

		// Token: 0x0400546F RID: 21615
		private string m_description;

		// Token: 0x04005470 RID: 21616
		private EnemyController m_enemyController;

		// Token: 0x04005471 RID: 21617
		private Vector2 m_previousHeading;

		// Token: 0x04005472 RID: 21618
		private Vector2 m_currentHeading;

		// Token: 0x04005473 RID: 21619
		private const string MOVE_SPEED_PARAMETER = "moveSpeed";
	}
}
