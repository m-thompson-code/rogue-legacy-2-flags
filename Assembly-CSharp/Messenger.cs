using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class Messenger<BaseClassName, EventIDs> : MonoBehaviour where BaseClassName : MonoBehaviour
{
	// Token: 0x170013EF RID: 5103
	// (get) Token: 0x06003879 RID: 14457 RVA: 0x000C0F60 File Offset: 0x000BF160
	// (set) Token: 0x0600387A RID: 14458 RVA: 0x000C0F67 File Offset: 0x000BF167
	protected static BaseClassName Instance { get; set; }

	// Token: 0x170013F0 RID: 5104
	// (get) Token: 0x0600387B RID: 14459 RVA: 0x000C0F6F File Offset: 0x000BF16F
	// (set) Token: 0x0600387C RID: 14460 RVA: 0x000C0F76 File Offset: 0x000BF176
	public static Dictionary<EventIDs, Relay<MonoBehaviour, EventArgs>> EventTable
	{
		get
		{
			return Messenger<BaseClassName, EventIDs>.m_eventTable;
		}
		protected set
		{
			Messenger<BaseClassName, EventIDs>.m_eventTable = value;
		}
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x000C0F7E File Offset: 0x000BF17E
	protected static BaseClassName CreateInstance()
	{
		BaseClassName baseClassName = CDGHelper.FindStaticInstance<BaseClassName>(true);
		UnityEngine.Object.DontDestroyOnLoad(baseClassName);
		return baseClassName;
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x000C0F94 File Offset: 0x000BF194
	public static void AddListener(EventIDs eventID, Action<MonoBehaviour, EventArgs> listener)
	{
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		if (listener == null)
		{
			return;
		}
		if (!Messenger<BaseClassName, EventIDs>.Instance)
		{
			Messenger<BaseClassName, EventIDs>.Instance = Messenger<BaseClassName, EventIDs>.CreateInstance();
			if (Messenger<BaseClassName, EventIDs>.Instance == null)
			{
				return;
			}
		}
		if (Messenger<BaseClassName, EventIDs>.EventTable.ContainsKey(eventID))
		{
			Messenger<BaseClassName, EventIDs>.EventTable[eventID].AddListener(listener, false);
			return;
		}
		Messenger<BaseClassName, EventIDs>.EventTable.Add(eventID, new Relay<MonoBehaviour, EventArgs>());
		Messenger<BaseClassName, EventIDs>.EventTable[eventID].AddListener(listener, false);
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x000C101F File Offset: 0x000BF21F
	public static void RemoveListener(EventIDs eventID, Action<MonoBehaviour, EventArgs> listener)
	{
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		if (listener == null)
		{
			return;
		}
		if (Messenger<BaseClassName, EventIDs>.EventTable.ContainsKey(eventID))
		{
			Messenger<BaseClassName, EventIDs>.EventTable[eventID].RemoveListener(listener);
		}
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x000C104C File Offset: 0x000BF24C
	public static void Broadcast(EventIDs eventID, MonoBehaviour sender, EventArgs eventArgs)
	{
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		if (!Messenger<BaseClassName, EventIDs>.Instance)
		{
			Messenger<BaseClassName, EventIDs>.Instance = Messenger<BaseClassName, EventIDs>.CreateInstance();
			if (!Messenger<BaseClassName, EventIDs>.Instance)
			{
				Debug.LogFormat("{0}: Failed to create instance of Messenger", new object[]
				{
					Time.frameCount
				});
				return;
			}
		}
		if (Messenger<BaseClassName, EventIDs>.EventTable.ContainsKey(eventID) && Messenger<BaseClassName, EventIDs>.EventTable[eventID] != null)
		{
			Messenger<BaseClassName, EventIDs>.EventTable[eventID].Dispatch(sender, eventArgs);
		}
	}

	// Token: 0x04002BAE RID: 11182
	protected static Dictionary<EventIDs, Relay<MonoBehaviour, EventArgs>> m_eventTable = new Dictionary<EventIDs, Relay<MonoBehaviour, EventArgs>>();

	// Token: 0x04002BAF RID: 11183
	private static BaseClassName m_instance;
}
