using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class SpawnPositionController : MonoBehaviour
{
	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x06001DCD RID: 7629 RVA: 0x00061F83 File Offset: 0x00060183
	public SpawnPositionObj[] SpawnPositionArray
	{
		get
		{
			return this.m_spawnPositionArray;
		}
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x00061F8B File Offset: 0x0006018B
	public bool HasSpawnPosition(int index)
	{
		return index < this.m_spawnPositionArray.Length;
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x00061F98 File Offset: 0x00060198
	public Vector3 GetSpawnPosition(int index)
	{
		if (index < this.m_spawnPositionArray.Length)
		{
			return this.m_spawnPositionArray[index].gameObject.transform.position;
		}
		Debug.Log("<color=yellow>Spawn Position index: " + index.ToString() + " out of array bounds.</color>");
		return Vector3.zero;
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x00061FE8 File Offset: 0x000601E8
	public Vector3 GetLocalSpawnPosition(int index)
	{
		if (index < this.m_spawnPositionArray.Length)
		{
			return this.m_spawnPositionArray[index].gameObject.transform.localPosition;
		}
		Debug.Log("<color=yellow>Spawn Position index: " + index.ToString() + " out of array bounds.</color>");
		return Vector3.zero;
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00062038 File Offset: 0x00060238
	private void Awake()
	{
		SpawnPositionObj[] componentsInChildren = base.GetComponentsInChildren<SpawnPositionObj>(true);
		this.m_spawnPositionArray = new SpawnPositionObj[componentsInChildren.Length];
		foreach (SpawnPositionObj spawnPositionObj in componentsInChildren)
		{
			int index = (int)spawnPositionObj.Index;
			if (this.m_spawnPositionArray[index] != null)
			{
				string name = base.name;
				if (!SpawnPositionController.m_warningIssuedTracker.Contains(name))
				{
					SpawnPositionController.m_warningIssuedTracker.Add(name);
					Debug.Log(string.Concat(new string[]
					{
						"<color=red>ERROR: Duplicate spawn position object index found. Name: <b>",
						name,
						"</b> - Index: <b>",
						index.ToString(),
						"</b></color>"
					}));
				}
			}
			else
			{
				this.m_spawnPositionArray[index] = spawnPositionObj;
				Vector3 localPosition = spawnPositionObj.transform.localPosition;
				localPosition.z = 0f;
				spawnPositionObj.transform.localPosition = localPosition;
				spawnPositionObj.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04001B7E RID: 7038
	private SpawnPositionObj[] m_spawnPositionArray;

	// Token: 0x04001B7F RID: 7039
	private static List<string> m_warningIssuedTracker = new List<string>();
}
