using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class RelicTester : MonoBehaviour
{
	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x06002680 RID: 9856 RVA: 0x00015784 File Offset: 0x00013984
	// (set) Token: 0x06002681 RID: 9857 RVA: 0x0001578C File Offset: 0x0001398C
	public RelicType RelicToAdd
	{
		get
		{
			return this.m_relicToAdd;
		}
		set
		{
			this.m_relicToAdd = value;
		}
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x00015795 File Offset: 0x00013995
	private void OnEnable()
	{
		if (this.m_addOnStart)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, new Action<MonoBehaviour, EventArgs>(this.AddRelicHandler));
		}
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000157B2 File Offset: 0x000139B2
	private void OnDisable()
	{
		if (this.m_addOnStart)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, new Action<MonoBehaviour, EventArgs>(this.AddRelicHandler));
		}
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000157CF File Offset: 0x000139CF
	private void AddRelicHandler(MonoBehaviour sender, EventArgs args)
	{
		this.AddRelic();
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000B6850 File Offset: 0x000B4A50
	public void AddRelic()
	{
		if (this.m_relicToAdd == RelicType.None)
		{
			return;
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(this.m_relicToAdd);
		if (relic != null)
		{
			relic.SetLevel(this.m_relicLevel, false, true);
		}
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000B6888 File Offset: 0x000B4A88
	public void RemoveRelic()
	{
		if (this.m_relicToAdd == RelicType.None)
		{
			return;
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(this.m_relicToAdd);
		if (relic != null)
		{
			relic.SetLevel(0, false, true);
		}
	}

	// Token: 0x0400215B RID: 8539
	[SerializeField]
	private RelicType m_relicToAdd;

	// Token: 0x0400215C RID: 8540
	[SerializeField]
	private bool m_addOnStart = true;

	// Token: 0x0400215D RID: 8541
	[SerializeField]
	private int m_relicLevel = 1;
}
