using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x02000E70 RID: 3696
	public class LocalTeleporterEventEmitterManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700214D RID: 8525
		// (get) Token: 0x0600683A RID: 26682 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x0017F220 File Offset: 0x0017D420
		private void Awake()
		{
			this.m_teleporter.OnEnterEvent.AddListener(new UnityAction(this.OnPlayerEnter));
			this.m_teleporter.OnExitEvent.AddListener(new UnityAction(this.OnPlayerExit));
			this.m_teleporter.OnHitLeyCornerEvent.AddListener(new UnityAction<Vector2>(this.OnPlayerHitCorner));
			this.m_travelEventInstance = AudioUtility.GetEventInstance(this.m_travelAudioPath, base.transform);
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x00039AD8 File Offset: 0x00037CD8
		private void OnDestroy()
		{
			if (this.m_travelEventInstance.isValid())
			{
				this.m_travelEventInstance.release();
			}
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x00039AF3 File Offset: 0x00037CF3
		private void OnPlayerEnter()
		{
			AudioManager.PlayOneShot(this, this.m_enterAudioPath, this.m_teleporter.transform.position);
			base.StartCoroutine(this.UpdateTravelAudioParameters());
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x00039B1E File Offset: 0x00037D1E
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

		// Token: 0x0600683F RID: 26687 RVA: 0x00039B2D File Offset: 0x00037D2D
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

		// Token: 0x06006840 RID: 26688 RVA: 0x00039B3C File Offset: 0x00037D3C
		private void OnPlayerExit()
		{
			base.StopAllCoroutines();
			AudioManager.PlayOneShot(this, this.m_exitAudioPath, this.m_teleporter.TeleporterLocation.transform.position);
			AudioManager.Stop(this.m_travelEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06006841 RID: 26689 RVA: 0x00039B71 File Offset: 0x00037D71
		private void OnPlayerHitCorner(Vector2 coordinates)
		{
			AudioManager.PlayOneShot(this, this.m_cornerHitAudioPath, coordinates);
		}

		// Token: 0x040054AA RID: 21674
		[SerializeField]
		private LocalTeleporterController m_teleporter;

		// Token: 0x040054AB RID: 21675
		[SerializeField]
		[EventRef]
		private string m_enterAudioPath;

		// Token: 0x040054AC RID: 21676
		[SerializeField]
		[EventRef]
		private string m_exitAudioPath;

		// Token: 0x040054AD RID: 21677
		[SerializeField]
		[EventRef]
		private string m_cornerHitAudioPath;

		// Token: 0x040054AE RID: 21678
		[SerializeField]
		[EventRef]
		private string m_travelAudioPath;

		// Token: 0x040054AF RID: 21679
		private EventInstance m_travelEventInstance;

		// Token: 0x040054B0 RID: 21680
		private float m_teletubeLength = -1f;

		// Token: 0x040054B1 RID: 21681
		private WaitForSeconds m_delayBeforeSettingParameters = new WaitForSeconds(0.25f);

		// Token: 0x040054B2 RID: 21682
		private const string TELETUBE_PROGRESS_PARAMETER = "teletube_progress";

		// Token: 0x040054B3 RID: 21683
		private const string TELETUBE_SPEED_PARAMETER = "teletube_speed";

		// Token: 0x040054B4 RID: 21684
		private const float TELETUBE_END_POINT_SQUARE_MAGNITUDE = 10f;
	}
}
