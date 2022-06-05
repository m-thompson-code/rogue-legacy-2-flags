using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class GenericPool_RL<T> where T : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00043928 File Offset: 0x00041B28
	public List<T> ObjectList
	{
		get
		{
			return this.m_genericList;
		}
	}

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x060015B7 RID: 5559 RVA: 0x00043930 File Offset: 0x00041B30
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

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x060015B8 RID: 5560 RVA: 0x00043998 File Offset: 0x00041B98
	// (set) Token: 0x060015B9 RID: 5561 RVA: 0x000439A0 File Offset: 0x00041BA0
	public int PoolSize { get; private set; }

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x060015BA RID: 5562 RVA: 0x000439A9 File Offset: 0x00041BA9
	// (set) Token: 0x060015BB RID: 5563 RVA: 0x000439B1 File Offset: 0x00041BB1
	public bool IsPoolingDisabled { get; private set; }

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x060015BC RID: 5564 RVA: 0x000439BA File Offset: 0x00041BBA
	// (set) Token: 0x060015BD RID: 5565 RVA: 0x000439C2 File Offset: 0x00041BC2
	public bool IsInitialized { get; private set; }

	// Token: 0x060015BE RID: 5566 RVA: 0x000439CC File Offset: 0x00041BCC
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

	// Token: 0x060015BF RID: 5567 RVA: 0x00043AAC File Offset: 0x00041CAC
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

	// Token: 0x060015C0 RID: 5568 RVA: 0x00043BA4 File Offset: 0x00041DA4
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

	// Token: 0x060015C1 RID: 5569 RVA: 0x00043C40 File Offset: 0x00041E40
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

	// Token: 0x060015C2 RID: 5570 RVA: 0x00043D14 File Offset: 0x00041F14
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

	// Token: 0x060015C3 RID: 5571 RVA: 0x00043D80 File Offset: 0x00041F80
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

	// Token: 0x060015C4 RID: 5572 RVA: 0x00043E24 File Offset: 0x00042024
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

	// Token: 0x040014E7 RID: 5351
	private T m_sourcePrefab;

	// Token: 0x040014E8 RID: 5352
	private List<T> m_genericList;

	// Token: 0x040014E9 RID: 5353
	private Action<MonoBehaviour, EventArgs> m_reparentAllPooledObjects;

	// Token: 0x040014EA RID: 5354
	private bool m_disableAwakeOnCreation;
}
