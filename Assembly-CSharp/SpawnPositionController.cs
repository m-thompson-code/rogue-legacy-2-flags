using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class SpawnPositionController : MonoBehaviour
{
	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06002912 RID: 10514 RVA: 0x00017321 File Offset: 0x00015521
	public SpawnPositionObj[] SpawnPositionArray
	{
		get
		{
			return this.m_spawnPositionArray;
		}
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x00017329 File Offset: 0x00015529
	public bool HasSpawnPosition(int index)
	{
		return index < this.m_spawnPositionArray.Length;
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000BF6A0 File Offset: 0x000BD8A0
	public Vector3 GetSpawnPosition(int index)
	{
		if (index < this.m_spawnPositionArray.Length)
		{
			return this.m_spawnPositionArray[index].gameObject.transform.position;
		}
		Debug.Log("<color=yellow>Spawn Position index: " + index.ToString() + " out of array bounds.</color>");
		return Vector3.zero;
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000BF6F0 File Offset: 0x000BD8F0
	public Vector3 GetLocalSpawnPosition(int index)
	{
		if (index < this.m_spawnPositionArray.Length)
		{
			return this.m_spawnPositionArray[index].gameObject.transform.localPosition;
		}
		Debug.Log("<color=yellow>Spawn Position index: " + index.ToString() + " out of array bounds.</color>");
		return Vector3.zero;
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000BF740 File Offset: 0x000BD940
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

	// Token: 0x040023C9 RID: 9161
	private SpawnPositionObj[] m_spawnPositionArray;

	// Token: 0x040023CA RID: 9162
	private static List<string> m_warningIssuedTracker = new List<string>();
}
