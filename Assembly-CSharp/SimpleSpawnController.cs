using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A63 RID: 2659
public class SimpleSpawnController : MonoBehaviour, IRoomConsumer, ISimpleSpawnController, ISpawnController
{
	// Token: 0x17001BB1 RID: 7089
	// (get) Token: 0x0600505C RID: 20572 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001BB2 RID: 7090
	// (get) Token: 0x0600505D RID: 20573 RVA: 0x0002BDD1 File Offset: 0x00029FD1
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x17001BB3 RID: 7091
	// (get) Token: 0x0600505E RID: 20574 RVA: 0x0002BDD9 File Offset: 0x00029FD9
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001BB4 RID: 7092
	// (get) Token: 0x0600505F RID: 20575 RVA: 0x0002BDF6 File Offset: 0x00029FF6
	// (set) Token: 0x06005060 RID: 20576 RVA: 0x0002BE19 File Offset: 0x0002A019
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			if (!this.m_hasCheckedForSpawnLogicController)
			{
				this.m_hasCheckedForSpawnLogicController = true;
				this.m_spawnLogic = base.GetComponent<SpawnLogicController>();
			}
			return this.m_spawnLogic;
		}
		private set
		{
			this.m_spawnLogic = value;
		}
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x0002BE22 File Offset: 0x0002A022
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06005062 RID: 20578 RVA: 0x0002BE2B File Offset: 0x0002A02B
	public bool Spawn()
	{
		if (this.ShouldSpawn)
		{
			this.DoIsSpawned();
			return true;
		}
		this.DoIsNotSpawned();
		return false;
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x00132748 File Offset: 0x00130948
	protected virtual void DoIsSpawned()
	{
		if (this.m_instantiatePrefab)
		{
			if (this.m_prefab)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_prefab, base.transform.parent);
				gameObject.transform.position = base.transform.position;
				List<IRoomConsumer> roomConsumerListHelper_STATIC = SimpleSpawnController.m_roomConsumerListHelper_STATIC;
				roomConsumerListHelper_STATIC.Clear();
				gameObject.GetComponentsInChildren<IRoomConsumer>(roomConsumerListHelper_STATIC);
				using (List<IRoomConsumer>.Enumerator enumerator = roomConsumerListHelper_STATIC.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IRoomConsumer roomConsumer = enumerator.Current;
						roomConsumer.SetRoom(this.Room);
					}
					goto IL_9E;
				}
			}
			Debug.LogFormat("<color=red>m_prefab Field is null on ({0})</color>", new object[]
			{
				this
			});
			IL_9E:
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x0001C0A5 File Offset: 0x0001A2A5
	protected virtual void DoIsNotSpawned()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005067 RID: 20583 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003CE0 RID: 15584
	public static List<IRoomConsumer> m_roomConsumerListHelper_STATIC = new List<IRoomConsumer>();

	// Token: 0x04003CE1 RID: 15585
	[SerializeField]
	protected bool m_instantiatePrefab;

	// Token: 0x04003CE2 RID: 15586
	[SerializeField]
	protected GameObject m_prefab;

	// Token: 0x04003CE3 RID: 15587
	private BaseRoom m_room;

	// Token: 0x04003CE4 RID: 15588
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04003CE5 RID: 15589
	private bool m_hasCheckedForSpawnLogicController;
}
