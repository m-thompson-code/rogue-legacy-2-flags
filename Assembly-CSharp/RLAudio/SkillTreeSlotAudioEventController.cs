using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E91 RID: 3729
	public class SkillTreeSlotAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700217E RID: 8574
		// (get) Token: 0x06006930 RID: 26928 RVA: 0x0003A5D7 File Offset: 0x000387D7
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

		// Token: 0x06006931 RID: 26929 RVA: 0x001818F4 File Offset: 0x0017FAF4
		private void Awake()
		{
			SkillTreeSlot component = base.GetComponent<SkillTreeSlot>();
			component.SelectedRelay.AddListener(new Action(this.OnSlotSelected), false);
			component.FullyUpgradedRelay.AddListener(new Action(this.OnFullyUpgraded), false);
			component.UpgradedRelay.AddListener(new Action(this.OnUpgraded), false);
			component.UpgradeFailedRelay.AddListener(new Action(this.OnUpgradeFailed), false);
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x0018196C File Offset: 0x0017FB6C
		private void OnUpgradeFailed()
		{
			AudioManager.PlayOneShot(this, this.m_upgradeFailedPath, default(Vector3));
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x00181990 File Offset: 0x0017FB90
		private void OnUpgraded()
		{
			AudioManager.PlayOneShot(this, this.m_upgradedPath, default(Vector3));
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x001819B4 File Offset: 0x0017FBB4
		private void OnFullyUpgraded()
		{
			AudioManager.PlayOneShot(this, this.m_fullyUpgradedPath, default(Vector3));
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x001819D8 File Offset: 0x0017FBD8
		private void OnSlotSelected()
		{
			AudioManager.PlayOneShot(this, this.m_selectedPath, default(Vector3));
		}

		// Token: 0x04005587 RID: 21895
		[SerializeField]
		[EventRef]
		private string m_selectedPath;

		// Token: 0x04005588 RID: 21896
		[SerializeField]
		[EventRef]
		private string m_upgradedPath;

		// Token: 0x04005589 RID: 21897
		[SerializeField]
		[EventRef]
		private string m_fullyUpgradedPath;

		// Token: 0x0400558A RID: 21898
		[SerializeField]
		[EventRef]
		private string m_upgradeFailedPath;

		// Token: 0x0400558B RID: 21899
		private string m_description;
	}
}
