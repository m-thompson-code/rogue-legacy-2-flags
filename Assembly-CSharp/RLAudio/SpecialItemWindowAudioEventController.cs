using System;
using FMODUnity;
using RL_Windows;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000916 RID: 2326
	public class SpecialItemWindowAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06004C43 RID: 19523 RVA: 0x00111E34 File Offset: 0x00110034
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

		// Token: 0x06004C44 RID: 19524 RVA: 0x00111E58 File Offset: 0x00110058
		private void Awake()
		{
			SpecialItemDropWindowController component = base.gameObject.GetComponent<SpecialItemDropWindowController>();
			component.WindowOpenedRelay.AddListener(new Action<SpecialItemType>(this.OnWindowOpened), false);
			component.WindowClosedRelay.AddListener(new Action<SpecialItemType>(this.OnWindowClosed), false);
			component.ItemRevealedRelay.AddListener(new Action<SpecialItemType>(this.OnItemRevealed), false);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x00111EBC File Offset: 0x001100BC
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

		// Token: 0x06004C46 RID: 19526 RVA: 0x00111F3C File Offset: 0x0011013C
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

		// Token: 0x06004C47 RID: 19527 RVA: 0x00111FBC File Offset: 0x001101BC
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

		// Token: 0x04004035 RID: 16437
		[SerializeField]
		[EventRef]
		private string m_runeWindowOpenedPath;

		// Token: 0x04004036 RID: 16438
		[SerializeField]
		[EventRef]
		private string m_runeWindowClosedPath;

		// Token: 0x04004037 RID: 16439
		[SerializeField]
		[EventRef]
		private string m_runeRevealedPath;

		// Token: 0x04004038 RID: 16440
		[SerializeField]
		[EventRef]
		private string m_blueprintWindowOpenedPath;

		// Token: 0x04004039 RID: 16441
		[SerializeField]
		[EventRef]
		private string m_blueprintWindowClosedPath;

		// Token: 0x0400403A RID: 16442
		[SerializeField]
		[EventRef]
		private string m_blueprintRevealedPath;

		// Token: 0x0400403B RID: 16443
		private string m_description;
	}
}
