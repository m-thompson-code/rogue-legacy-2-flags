using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x020008F9 RID: 2297
	public class LocalTeleporterEventEmitterManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x0010F174 File Offset: 0x0010D374
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x0010F17C File Offset: 0x0010D37C
		private void Awake()
		{
			this.m_teleporter.OnEnterEvent.AddListener(new UnityAction(this.OnPlayerEnter));
			this.m_teleporter.OnExitEvent.AddListener(new UnityAction(this.OnPlayerExit));
			this.m_teleporter.OnHitLeyCornerEvent.AddListener(new UnityAction<Vector2>(this.OnPlayerHitCorner));
			this.m_travelEventInstance = AudioUtility.GetEventInstance(this.m_travelAudioPath, base.transform);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x0010F1F4 File Offset: 0x0010D3F4
		private void OnDestroy()
		{
			if (this.m_travelEventInstance.isValid())
			{
				this.m_travelEventInstance.release();
			}
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0010F20F File Offset: 0x0010D40F
		private void OnPlayerEnter()
		{
			AudioManager.PlayOneShot(this, this.m_enterAudioPath, this.m_teleporter.transform.position);
			base.StartCoroutine(this.UpdateTravelAudioParameters());
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x0010F23A File Offset: 0x0010D43A
		private IEnumerator CalculateTeletubeLength()
		{
			while (LocalTeleporterController.LeylinePoints == null || LocalTeleporterController.LeylinePoints.Count == 0)
			{
				yield return null;
			}
			for (int i = 0; i < LocalTeleporterController.LeylinePoints.Count - 1; i++)
			{
				Vector2 vector = LocalTeleporterController.LeylinePoints[i + 1] - LocalTeleporterController.LeylinePoints[i];
				this.m_teletubeLength += vector.magnitude;
			}
			yield break;
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x0010F249 File Offset: 0x0010D449
		private IEnumerator UpdateTravelAudioParameters()
		{
			if (this.m_teletubeLength == -1f)
			{
				yield return this.CalculateTeletubeLength();
			}
			yield return this.m_delayBeforeSettingParameters;
			AudioManager.PlayAttached(this, this.m_travelEventInstance, PlayerManager.GetPlayerController().gameObject);
			this.m_travelEventInstance.setParameterByName("teletube_speed", 1f, false);
			float totalDistanceTravelled = 0f;
			Vector2 previousPlayerPositionInTube = PlayerManager.GetPlayerController().transform.position;
			for (;;)
			{
				Vector2 vector = PlayerManager.GetPlayerController().transform.position;
				Vector2 vector2 = vector - previousPlayerPositionInTube;
				previousPlayerPositionInTube = vector;
				float magnitude = vector2.magnitude;
				totalDistanceTravelled += magnitude;
				float value = totalDistanceTravelled / this.m_teletubeLength;
				if (this.m_teleporter.CurrentCornerIndex == this.m_teleporter.CornerCount && (this.m_teleporter.LeyLines.transform.TransformPoint(LocalTeleporterController.LeylinePoints[LocalTeleporterController.LeylinePoints.Count - 1]) - vector).sqrMagnitude < 10f)
				{
					break;
				}
				this.m_travelEventInstance.setParameterByName("teletube_progress", value, false);
				yield return null;
			}
			this.m_travelEventInstance.setParameterByName("teletube_speed", 0f, false);
			yield break;
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x0010F258 File Offset: 0x0010D458
		private void OnPlayerExit()
		{
			base.StopAllCoroutines();
			AudioManager.PlayOneShot(this, this.m_exitAudioPath, this.m_teleporter.TeleporterLocation.transform.position);
			AudioManager.Stop(this.m_travelEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x0010F28D File Offset: 0x0010D48D
		private void OnPlayerHitCorner(Vector2 coordinates)
		{
			AudioManager.PlayOneShot(this, this.m_cornerHitAudioPath, coordinates);
		}

		// Token: 0x04003F66 RID: 16230
		[SerializeField]
		private LocalTeleporterController m_teleporter;

		// Token: 0x04003F67 RID: 16231
		[SerializeField]
		[EventRef]
		private string m_enterAudioPath;

		// Token: 0x04003F68 RID: 16232
		[SerializeField]
		[EventRef]
		private string m_exitAudioPath;

		// Token: 0x04003F69 RID: 16233
		[SerializeField]
		[EventRef]
		private string m_cornerHitAudioPath;

		// Token: 0x04003F6A RID: 16234
		[SerializeField]
		[EventRef]
		private string m_travelAudioPath;

		// Token: 0x04003F6B RID: 16235
		private EventInstance m_travelEventInstance;

		// Token: 0x04003F6C RID: 16236
		private float m_teletubeLength = -1f;

		// Token: 0x04003F6D RID: 16237
		private WaitForSeconds m_delayBeforeSettingParameters = new WaitForSeconds(0.25f);

		// Token: 0x04003F6E RID: 16238
		private const string TELETUBE_PROGRESS_PARAMETER = "teletube_progress";

		// Token: 0x04003F6F RID: 16239
		private const string TELETUBE_SPEED_PARAMETER = "teletube_speed";

		// Token: 0x04003F70 RID: 16240
		private const float TELETUBE_END_POINT_SQUARE_MAGNITUDE = 10f;
	}
}
