using System;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class ObjectReferenceFinder : MonoBehaviour
{
	// Token: 0x060018AF RID: 6319 RVA: 0x0004D588 File Offset: 0x0004B788
	public GameObject GetObject(string ID)
	{
		GameObject result;
		if (this.m_objectReferenceTable.TryGetValue(ID, out result))
		{
			return result;
		}
		Debug.Log("Object reference '" + ID + "' could not be found.  Are you sure this object was added to the Object Reference Finder component?");
		return null;
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x0004D5C0 File Offset: 0x0004B7C0
	public T GetObject<T>(string ID, bool searchAllChildren = false, bool includeInactive = false) where T : UnityEngine.Object
	{
		GameObject @object = this.GetObject(ID);
		if (!@object)
		{
			return default(T);
		}
		if (!searchAllChildren)
		{
			return @object.GetComponent<T>();
		}
		return @object.GetComponentInChildren<T>(includeInactive);
	}

	// Token: 0x040017F8 RID: 6136
	[SerializeField]
	private StringGameObjectDictionary m_objectReferenceTable;
}
