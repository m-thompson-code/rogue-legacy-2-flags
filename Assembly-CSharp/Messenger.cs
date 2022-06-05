using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000A40 RID: 2624
public class Messenger<BaseClassName, EventIDs> : MonoBehaviour where BaseClassName : MonoBehaviour
{
	// Token: 0x17001B46 RID: 6982
	// (get) Token: 0x06004F1D RID: 20253 RVA: 0x0002B202 File Offset: 0x00029402
	// (set) Token: 0x06004F1E RID: 20254 RVA: 0x0002B209 File Offset: 0x00029409
	protected static BaseClassName Instance { get; set; }

	// Token: 0x17001B47 RID: 6983
	// (get) Token: 0x06004F1F RID: 20255 RVA: 0x0002B211 File Offset: 0x00029411
	// (set) Token: 0x06004F20 RID: 20256 RVA: 0x0002B218 File Offset: 0x00029418
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

	// Token: 0x06004F21 RID: 20257 RVA: 0x0002B220 File Offset: 0x00029420
	protected static BaseClassName CreateInstance()
	{
		BaseClassName baseClassName = CDGHelper.FindStaticInstance<BaseClassName>(true);
		UnityEngine.Object.DontDestroyOnLoad(baseClassName);
		return baseClassName;
	}

	// Token: 0x06004F22 RID: 20258 RVA: 0x0012F1E8 File Offset: 0x0012D3E8
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

	// Token: 0x06004F23 RID: 20259 RVA: 0x0002B233 File Offset: 0x00029433
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

	// Token: 0x06004F24 RID: 20260 RVA: 0x0012F274 File Offset: 0x0012D474
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

	// Token: 0x04003C1B RID: 15387
	protected static Dictionary<EventIDs, Relay<MonoBehaviour, EventArgs>> m_eventTable = new Dictionary<EventIDs, Relay<MonoBehaviour, EventArgs>>();

	// Token: 0x04003C1C RID: 15388
	private static BaseClassName m_instance;
}
