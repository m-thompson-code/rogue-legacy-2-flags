using System;
using FMODUnity;
using RL_Windows;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E93 RID: 3731
	public class SpecialItemWindowAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002180 RID: 8576
		// (get) Token: 0x0600693E RID: 26942 RVA: 0x0003A657 File Offset: 0x00038857
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

		// Token: 0x0600693F RID: 26943 RVA: 0x00181B34 File Offset: 0x0017FD34
		private void Awake()
		{
			SpecialItemDropWindowController component = base.gameObject.GetComponent<SpecialItemDropWindowController>();
			component.WindowOpenedRelay.AddListener(new Action<SpecialItemType>(this.OnWindowOpened), false);
			component.WindowClosedRelay.AddListener(new Action<SpecialItemType>(this.OnWindowClosed), false);
			component.ItemRevealedRelay.AddListener(new Action<SpecialItemType>(this.OnItemRevealed), false);
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x00181B98 File Offset: 0x0017FD98
		private void OnWindowOpened(SpecialItemType itemID)
		{
			string path = string.Empty;
			if (itemID <= SpecialItemType.Rune)
			{
				if (itemID <= SpecialItemType.Gold)
				{
					if (itemID != SpecialItemType.None && itemID != SpecialItemType.Gold)
					{
					}
				}
				else if (itemID != SpecialItemType.Stat)
				{
					if (itemID != SpecialItemType.Blueprint)
					{
						if (itemID == SpecialItemType.Rune)
						{
							path = this.m_runeWindowOpenedPath;
						}
					}
					else
					{
						path = this.m_blueprintWindowOpenedPath;
					}
				}
			}
			else if (itemID <= SpecialItemType.Relic)
			{
				if (itemID != SpecialItemType.Weapon && itemID != SpecialItemType.Ore && itemID != SpecialItemType.Relic)
				{
				}
			}
			else if (itemID != SpecialItemType.Heirloom && itemID != SpecialItemType.Ability && itemID != SpecialItemType.Challenge)
			{
			}
			AudioManager.PlayOneShot(this, path, default(Vector3));
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x00181C18 File Offset: 0x0017FE18
		private void OnItemRevealed(SpecialItemType itemID)
		{
			string path = string.Empty;
			if (itemID <= SpecialItemType.Rune)
			{
				if (itemID <= SpecialItemType.Gold)
				{
					if (itemID != SpecialItemType.None && itemID != SpecialItemType.Gold)
					{
					}
				}
				else if (itemID != SpecialItemType.Stat)
				{
					if (itemID != SpecialItemType.Blueprint)
					{
						if (itemID == SpecialItemType.Rune)
						{
							path = this.m_runeRevealedPath;
						}
					}
					else
					{
						path = this.m_blueprintRevealedPath;
					}
				}
			}
			else if (itemID <= SpecialItemType.Relic)
			{
				if (itemID != SpecialItemType.Weapon && itemID != SpecialItemType.Ore && itemID != SpecialItemType.Relic)
				{
				}
			}
			else if (itemID != SpecialItemType.Heirloom && itemID != SpecialItemType.Ability && itemID != SpecialItemType.Challenge)
			{
			}
			AudioManager.PlayOneShot(this, path, default(Vector3));
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x00181C98 File Offset: 0x0017FE98
		private void OnWindowClosed(SpecialItemType itemID)
		{
			string path = string.Empty;
			if (itemID <= SpecialItemType.Rune)
			{
				if (itemID <= SpecialItemType.Gold)
				{
					if (itemID != SpecialItemType.None && itemID != SpecialItemType.Gold)
					{
					}
				}
				else if (itemID != SpecialItemType.Stat)
				{
					if (itemID != SpecialItemType.Blueprint)
					{
						if (itemID == SpecialItemType.Rune)
						{
							path = this.m_runeWindowClosedPath;
						}
					}
					else
					{
						path = this.m_blueprintWindowClosedPath;
					}
				}
			}
			else if (itemID <= SpecialItemType.Relic)
			{
				if (itemID != SpecialItemType.Weapon && itemID != SpecialItemType.Ore && itemID != SpecialItemType.Relic)
				{
				}
			}
			else if (itemID != SpecialItemType.Heirloom && itemID != SpecialItemType.Ability && itemID != SpecialItemType.Challenge)
			{
			}
			AudioManager.PlayOneShot(this, path, default(Vector3));
		}

		// Token: 0x04005595 RID: 21909
		[SerializeField]
		[EventRef]
		private string m_runeWindowOpenedPath;

		// Token: 0x04005596 RID: 21910
		[SerializeField]
		[EventRef]
		private string m_runeWindowClosedPath;

		// Token: 0x04005597 RID: 21911
		[SerializeField]
		[EventRef]
		private string m_runeRevealedPath;

		// Token: 0x04005598 RID: 21912
		[SerializeField]
		[EventRef]
		private string m_blueprintWindowOpenedPath;

		// Token: 0x04005599 RID: 21913
		[SerializeField]
		[EventRef]
		private string m_blueprintWindowClosedPath;

		// Token: 0x0400559A RID: 21914
		[SerializeField]
		[EventRef]
		private string m_blueprintRevealedPath;

		// Token: 0x0400559B RID: 21915
		private string m_description;
	}
}
