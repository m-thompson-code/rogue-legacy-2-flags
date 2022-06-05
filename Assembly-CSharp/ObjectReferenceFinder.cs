using System;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class ObjectReferenceFinder : MonoBehaviour
{
	// Token: 0x0600229E RID: 8862 RVA: 0x000AB2BC File Offset: 0x000A94BC
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

	// Token: 0x0600229F RID: 8863 RVA: 0x000AB2F4 File Offset: 0x000A94F4
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

	// Token: 0x04001F3D RID: 7997
	[SerializeField]
	private StringGameObjectDictionary m_objectReferenceTable;
}
