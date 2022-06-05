using System;
using System.Collections;
using FMOD.Studio;

namespace RLAudio
{
	// Token: 0x02000E67 RID: 3687
	public class HealingTreeSpecialRoomAudioEventEmitterController : DualChoiceSpecialRoomAudioEventEmitterController
	{
		// Token: 0x0600680E RID: 26638 RVA: 0x000398BC File Offset: 0x00037ABC
		protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
		{
			base.OnPlayerEnterRoom(sender, eventArgs);
			if (this.m_ambientSoundOverride == null)
			{
				this.m_ambientSoundOverride = base.GetComponent<AmbientSoundOverride>();
			}
			base.StartCoroutine(this.WaitUntilAmbientSoundIsSet());
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x000398ED File Offset: 0x00037AED
		protected override void OnRoomComplete()
		{
			base.OnRoomComplete();
			this.SetAmbientParameter(AmbientSoundController.GetCurrentEventInstance());
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00039900 File Offset: 0x00037B00
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

		// Token: 0x06006811 RID: 26641 RVA: 0x0017ED38 File Offset: 0x0017CF38
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

		// Token: 0x04005489 RID: 21641
		private AmbientSoundOverride m_ambientSoundOverride;
	}
}
