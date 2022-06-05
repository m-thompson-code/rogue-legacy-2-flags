using System;
using System.Collections;
using FMOD.Studio;

namespace RLAudio
{
	// Token: 0x020008F1 RID: 2289
	public class HealingTreeSpecialRoomAudioEventEmitterController : DualChoiceSpecialRoomAudioEventEmitterController
	{
		// Token: 0x06004B3D RID: 19261 RVA: 0x0010EB38 File Offset: 0x0010CD38
		protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
		{
			base.OnPlayerEnterRoom(sender, eventArgs);
			if (this.m_ambientSoundOverride == null)
			{
				this.m_ambientSoundOverride = base.GetComponent<AmbientSoundOverride>();
			}
			base.StartCoroutine(this.WaitUntilAmbientSoundIsSet());
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x0010EB69 File Offset: 0x0010CD69
		protected override void OnRoomComplete()
		{
			base.OnRoomComplete();
			this.SetAmbientParameter(AmbientSoundController.GetCurrentEventInstance());
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x0010EB7C File Offset: 0x0010CD7C
		private IEnumerator WaitUntilAmbientSoundIsSet()
		{
			bool isSet = false;
			EventInstance currentEventInstance = default(EventInstance);
			while (!isSet)
			{
				currentEventInstance = AmbientSoundController.GetCurrentEventInstance();
				if (currentEventInstance.isValid())
				{
					EventDescription eventDescription;
					currentEventInstance.getDescription(out eventDescription);
					string text;
					eventDescription.getPath(out text);
					if (text.Contains("amb_treeRoom") || text.Contains("amb_relicRoom"))
					{
						isSet = true;
					}
				}
				yield return null;
			}
			this.SetAmbientParameter(currentEventInstance);
			yield break;
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x0010EB8C File Offset: 0x0010CD8C
		private void SetAmbientParameter(EventInstance currentEventInstance)
		{
			float value = 0f;
			if (this.m_isRoomComplete)
			{
				value = 0.51f;
			}
			if (currentEventInstance.isValid())
			{
				currentEventInstance.setParameterByName("Tree", value, false);
			}
		}

		// Token: 0x04003F4A RID: 16202
		private AmbientSoundOverride m_ambientSoundOverride;
	}
}
