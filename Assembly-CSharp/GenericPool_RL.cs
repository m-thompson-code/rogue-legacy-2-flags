using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class GenericPool_RL<T> where T : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x000105C7 File Offset: 0x0000E7C7
	public List<T> ObjectList
	{
		get
		{
			return this.m_genericList;
		}
	}

	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000A1EF0 File Offset: 0x000A00F0
	public bool HasActiveObjects
	{
		get
		{
			if (this.IsInitialized)
			{
				using (List<T>.Enumerator enumerator = this.m_genericList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.isActiveAndEnabled)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}
	}

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000105CF File Offset: 0x0000E7CF
	// (set) Token: 0x06001F2D RID: 7981 RVA: 0x000105D7 File Offset: 0x0000E7D7
	public int PoolSize { get; private set; }

	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x06001F2E RID: 7982 RVA: 0x000105E0 File Offset: 0x0000E7E0
	// (set) Token: 0x06001F2F RID: 7983 RVA: 0x000105E8 File Offset: 0x0000E7E8
	public bool IsPoolingDisabled { get; private set; }

	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000105F1 File Offset: 0x0000E7F1
	// (set) Token: 0x06001F31 RID: 7985 RVA: 0x000105F9 File Offset: 0x0000E7F9
	public bool IsInitialized { get; private set; }

	// Token: 0x06001F32 RID: 7986 RVA: 0x000A1F58 File Offset: 0x000A0158
	public void Initialize(T prefab, int poolSize, bool disablePooling, bool disableAwakeOnCreation = true)
	{
		if (this.IsInitialized)
		{
			string str = "Pool of type: ";
			Type type = prefab.GetType();
			Debug.LogWarning(str + ((type != null) ? type.ToString() : null) + " already initialized.");
			return;
		}
		this.m_genericList = new List<T>();
		this.m_sourcePrefab = prefab;
		this.IsPoolingDisabled = disablePooling;
		this.m_disableAwakeOnCreation = disableAwakeOnCreation;
		if (!DisablePooledObjectManager.DisablePoolObjsHelper_STATIC)
		{
			DisablePooledObjectManager.DisablePoolObjsHelper_STATIC = new GameObject("Disable Pool Objs Helper");
			DisablePooledObjectManager.DisablePoolObjsHelper_STATIC.SetActive(false);
			DisablePooledObjectManager.DisablePoolObjsHelper_STATIC.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(DisablePooledObjectManager.DisablePoolObjsHelper_STATIC);
		}
		if (!disablePooling)
		{
			for (int i = 0; i < poolSize; i++)
			{
				this.CreateNew();
			}
		}
		this.m_reparentAllPooledObjects = new Action<MonoBehaviour, EventArgs>(this.ReparentAllPooledObjects);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.UpdatePools, this.m_reparentAllPooledObjects);
		this.IsInitialized = true;
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x000A2038 File Offset: 0x000A0238
	private T CreateNew()
	{
		T t = default(T);
		if (!this.IsPoolingDisabled)
		{
			if (!this.m_disableAwakeOnCreation)
			{
				t = UnityEngine.Object.Instantiate<T>(this.m_sourcePrefab, null, false);
			}
			else
			{
				t = UnityEngine.Object.Instantiate<T>(this.m_sourcePrefab, DisablePooledObjectManager.DisablePoolObjsHelper_STATIC.transform, false);
				t.gameObject.SetActive(false);
				t.transform.SetParent(null);
			}
			UnityEngine.Object.DontDestroyOnLoad(t);
		}
		else
		{
			t = UnityEngine.Object.Instantiate<T>(this.m_sourcePrefab);
		}
		t.name = this.m_sourcePrefab.name;
		if (!this.IsPoolingDisabled)
		{
			this.m_genericList.Add(t);
			if (t.gameObject.activeSelf)
			{
				t.gameObject.SetActive(false);
			}
			int poolSize = this.PoolSize;
			this.PoolSize = poolSize + 1;
		}
		t.IsFreePoolObj = true;
		return t;
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x000A2130 File Offset: 0x000A0330
	public void ResizePool(int newSize)
	{
		if (this.m_genericList == null)
		{
			Debug.Log("Cannot resize a non-existent pool.  Call Initialize() instead.");
			return;
		}
		this.DisableAll();
		this.ReparentAllPooledObjects(null, null);
		if (newSize == this.PoolSize)
		{
			return;
		}
		int num = newSize - this.PoolSize;
		if (num < 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.CreateNew();
			}
		}
		else
		{
			for (int j = this.PoolSize - 1; j >= newSize; j--)
			{
				UnityEngine.Object.Destroy(this.m_genericList[j].gameObject);
				this.m_genericList.RemoveAt(j);
			}
		}
		this.PoolSize = newSize;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x000A21CC File Offset: 0x000A03CC
	public T GetFreeObj()
	{
		if (!this.IsPoolingDisabled)
		{
			int count = this.m_genericList.Count;
			for (int i = 0; i < count; i++)
			{
				T t = this.m_genericList[i];
				if (!t)
				{
					throw new Exception("A pooled object got destroyed!!!");
				}
				if (t.IsFreePoolObj)
				{
					if (t.transform.parent)
					{
						t.transform.SetParent(null);
						UnityEngine.Object.DontDestroyOnLoad(t);
					}
					t.IsFreePoolObj = false;
					if (t.IsAwakeCalled)
					{
						t.ResetValues();
					}
					return t;
				}
			}
		}
		T t2 = this.CreateNew();
		t2.IsFreePoolObj = false;
		return t2;
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x000A22A0 File Offset: 0x000A04A0
	public bool DisableAll()
	{
		bool result;
		try
		{
			foreach (T t in this.m_genericList)
			{
				DisablePooledObjectManager.DisablePooledObject(t, false);
			}
			result = true;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x000A230C File Offset: 0x000A050C
	private void ReparentAllPooledObjects(object sender, EventArgs args)
	{
		foreach (T t in this.m_genericList)
		{
			if (t.transform.parent)
			{
				t.transform.SetParent(null);
				UnityEngine.Object.DontDestroyOnLoad(t);
			}
			if (t.gameObject.activeSelf)
			{
				t.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x000A23B0 File Offset: 0x000A05B0
	public void DestroyPool()
	{
		foreach (T t in this.m_genericList)
		{
			if (t)
			{
				UnityEngine.Object.Destroy(t.gameObject);
			}
		}
		this.m_genericList.Clear();
		this.m_genericList = null;
		this.m_sourcePrefab = default(T);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdatePools, this.m_reparentAllPooledObjects);
	}

	// Token: 0x04001BCE RID: 7118
	private T m_sourcePrefab;

	// Token: 0x04001BCF RID: 7119
	private List<T> m_genericList;

	// Token: 0x04001BD0 RID: 7120
	private Action<MonoBehaviour, EventArgs> m_reparentAllPooledObjects;

	// Token: 0x04001BD1 RID: 7121
	private bool m_disableAwakeOnCreation;
}
