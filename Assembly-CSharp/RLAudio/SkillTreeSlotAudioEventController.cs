using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000914 RID: 2324
	public class SkillTreeSlotAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x00111B6C File Offset: 0x0010FD6C
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

		// Token: 0x06004C36 RID: 19510 RVA: 0x00111B90 File Offset: 0x0010FD90
		private void Awake()
		{
			SkillTreeSlot component = base.GetComponent<SkillTreeSlot>();
			component.SelectedRelay.AddListener(new Action(this.OnSlotSelected), false);
			component.FullyUpgradedRelay.AddListener(new Action(this.OnFullyUpgraded), false);
			component.UpgradedRelay.AddListener(new Action(this.OnUpgraded), false);
			component.UpgradeFailedRelay.AddListener(new Action(this.OnUpgradeFailed), false);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00111C08 File Offset: 0x0010FE08
		private void OnUpgradeFailed()
		{
			AudioManager.PlayOneShot(this, this.m_upgradeFailedPath, default(Vector3));
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00111C2C File Offset: 0x0010FE2C
		private void OnUpgraded()
		{
			AudioManager.PlayOneShot(this, this.m_upgradedPath, default(Vector3));
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x00111C50 File Offset: 0x0010FE50
		private void OnFullyUpgraded()
		{
			AudioManager.PlayOneShot(this, this.m_fullyUpgradedPath, default(Vector3));
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x00111C74 File Offset: 0x0010FE74
		private void OnSlotSelected()
		{
			AudioManager.PlayOneShot(this, this.m_selectedPath, default(Vector3));
		}

		// Token: 0x04004027 RID: 16423
		[SerializeField]
		[EventRef]
		private string m_selectedPath;

		// Token: 0x04004028 RID: 16424
		[SerializeField]
		[EventRef]
		private string m_upgradedPath;

		// Token: 0x04004029 RID: 16425
		[SerializeField]
		[EventRef]
		private string m_fullyUpgradedPath;

		// Token: 0x0400402A RID: 16426
		[SerializeField]
		[EventRef]
		private string m_upgradeFailedPath;

		// Token: 0x0400402B RID: 16427
		private string m_description;
	}
}
