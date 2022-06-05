using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class SimpleSpawnController : MonoBehaviour, IRoomConsumer, ISimpleSpawnController, ISpawnController
{
	// Token: 0x1700144A RID: 5194
	// (get) Token: 0x0600397D RID: 14717 RVA: 0x000C4199 File Offset: 0x000C2399
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x1700144B RID: 5195
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x000C41A1 File Offset: 0x000C23A1
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x1700144C RID: 5196
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x000C41A9 File Offset: 0x000C23A9
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x1700144D RID: 5197
	// (get) Token: 0x06003980 RID: 14720 RVA: 0x000C41C6 File Offset: 0x000C23C6
	// (set) Token: 0x06003981 RID: 14721 RVA: 0x000C41E9 File Offset: 0x000C23E9
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

	// Token: 0x06003982 RID: 14722 RVA: 0x000C41F2 File Offset: 0x000C23F2
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x000C41FB File Offset: 0x000C23FB
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

	// Token: 0x06003984 RID: 14724 RVA: 0x000C4214 File Offset: 0x000C2414
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

	// Token: 0x06003985 RID: 14725 RVA: 0x000C42F8 File Offset: 0x000C24F8
	protected virtual void DoIsNotSpawned()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x000C431A File Offset: 0x000C251A
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002C4E RID: 11342
	public static List<IRoomConsumer> m_roomConsumerListHelper_STATIC = new List<IRoomConsumer>();

	// Token: 0x04002C4F RID: 11343
	[SerializeField]
	protected bool m_instantiatePrefab;

	// Token: 0x04002C50 RID: 11344
	[SerializeField]
	protected GameObject m_prefab;

	// Token: 0x04002C51 RID: 11345
	private BaseRoom m_room;

	// Token: 0x04002C52 RID: 11346
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04002C53 RID: 11347
	private bool m_hasCheckedForSpawnLogicController;
}
