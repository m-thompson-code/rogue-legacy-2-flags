using System;
using UnityEngine;

// Token: 0x020002BA RID: 698
public class RelicTester : MonoBehaviour
{
	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06001BCC RID: 7116 RVA: 0x00059B42 File Offset: 0x00057D42
	// (set) Token: 0x06001BCD RID: 7117 RVA: 0x00059B4A File Offset: 0x00057D4A
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

	// Token: 0x06001BCE RID: 7118 RVA: 0x00059B53 File Offset: 0x00057D53
	private void OnEnable()
	{
		if (this.m_addOnStart)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, new Action<MonoBehaviour, EventArgs>(this.AddRelicHandler));
		}
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x00059B70 File Offset: 0x00057D70
	private void OnDisable()
	{
		if (this.m_addOnStart)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, new Action<MonoBehaviour, EventArgs>(this.AddRelicHandler));
		}
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00059B8D File Offset: 0x00057D8D
	private void AddRelicHandler(MonoBehaviour sender, EventArgs args)
	{
		this.AddRelic();
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x00059B98 File Offset: 0x00057D98
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

	// Token: 0x06001BD2 RID: 7122 RVA: 0x00059BD0 File Offset: 0x00057DD0
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

	// Token: 0x0400196E RID: 6510
	[SerializeField]
	private RelicType m_relicToAdd;

	// Token: 0x0400196F RID: 6511
	[SerializeField]
	private bool m_addOnStart = true;

	// Token: 0x04001970 RID: 6512
	[SerializeField]
	private int m_relicLevel = 1;
}
