using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C60 RID: 3168
public static class CDGExtensions
{
	// Token: 0x06005B5F RID: 23391 RVA: 0x00159D20 File Offset: 0x00157F20
	public static Transform FindDeep(this Transform aParent, string aName)
	{
		Transform transform = aParent.Find(aName);
		if (transform != null)
		{
			return transform;
		}
		foreach (object obj in aParent)
		{
			transform = ((Transform)obj).FindDeep(aName);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x00159D98 File Offset: 0x00157F98
	public static void FindAllTransforms(this Transform parent, List<Transform> outputList)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			outputList.Add(child);
			child.FindAllTransforms(outputList);
		}
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x00159DCC File Offset: 0x00157FCC
	public static string FullHierarchyPath(this GameObject gameObject)
	{
		string text = gameObject.name;
		Transform parent = gameObject.transform.parent;
		while (parent != null)
		{
			text = parent.name + "/" + text;
			parent = parent.parent;
		}
		return text;
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x00159E14 File Offset: 0x00158014
	public static GameObject FindObjectReference(this GameObject gameObject, string id)
	{
		ObjectReferenceFinder component = gameObject.GetComponent<ObjectReferenceFinder>();
		if (component)
		{
			return component.GetObject(id);
		}
		return null;
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x00159E3C File Offset: 0x0015803C
	public static T FindObjectReference<T>(this GameObject gameObject, string id, bool searchAllChildren = false, bool includeInactive = false) where T : UnityEngine.Object
	{
		ObjectReferenceFinder component = gameObject.GetComponent<ObjectReferenceFinder>();
		if (component)
		{
			return component.GetObject<T>(id, searchAllChildren, includeInactive);
		}
		return default(T);
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x00159E6C File Offset: 0x0015806C
	public static bool Intersects(this Rect r1, Rect r2, out Rect area)
	{
		area = default(Rect);
		if (r2.Overlaps(r1))
		{
			float num = Mathf.Min(r1.xMax, r2.xMax);
			float num2 = Mathf.Max(r1.xMin, r2.xMin);
			float num3 = Mathf.Min(r1.yMax, r2.yMax);
			float num4 = Mathf.Max(r1.yMin, r2.yMin);
			area.x = Mathf.Min(num, num2);
			area.y = Mathf.Min(num3, num4);
			area.width = Mathf.Max(0f, num - num2);
			area.height = Mathf.Max(0f, num3 - num4);
			return true;
		}
		return false;
	}

	// Token: 0x06005B65 RID: 23397 RVA: 0x000321F9 File Offset: 0x000303F9
	public static GameObject GetRoot(this Component obj, bool includeInactive = false)
	{
		return obj.gameObject.GetRoot(includeInactive);
	}

	// Token: 0x06005B66 RID: 23398 RVA: 0x00159F20 File Offset: 0x00158120
	public static GameObject GetRoot(this GameObject obj, bool includeInactive = false)
	{
		if (obj.transform.parent == null)
		{
			return obj;
		}
		GameObject result = obj;
		IRootObj componentInParent = obj.GetComponentInParent<IRootObj>(includeInactive);
		if (componentInParent != null)
		{
			result = componentInParent.gameObject;
		}
		return result;
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x00032207 File Offset: 0x00030407
	public static bool Contains(this LayerMask mask, int layer)
	{
		return (mask.value & 1 << layer) > 0;
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x0003221A File Offset: 0x0003041A
	public static bool Contains(this LayerMask mask, GameObject gameobject)
	{
		return (mask.value & 1 << gameobject.layer) > 0;
	}

	// Token: 0x06005B69 RID: 23401 RVA: 0x00159F58 File Offset: 0x00158158
	public static void SetLayerRecursively(this GameObject gameObject, LayerMask layer, bool includeInactive = false)
	{
		CDGExtensions.m_recursiveLayersHelper.Clear();
		gameObject.GetComponentsInChildren<Transform>(includeInactive, CDGExtensions.m_recursiveLayersHelper);
		foreach (Transform transform in CDGExtensions.m_recursiveLayersHelper)
		{
			transform.gameObject.layer = layer;
		}
	}

	// Token: 0x06005B6A RID: 23402 RVA: 0x00159FC8 File Offset: 0x001581C8
	public static void SetAlpha(this Image image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x00159FC8 File Offset: 0x001581C8
	public static void SetAlpha(this RawImage image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x06005B6C RID: 23404 RVA: 0x00159FEC File Offset: 0x001581EC
	public static void SetAlpha(this SpriteRenderer image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x0015A010 File Offset: 0x00158210
	public static T[] InsertAt<T>(this T[] source, int index, T element)
	{
		T[] array = new T[source.Length + 1];
		for (int i = 0; i < source.Length; i++)
		{
			if (i < index)
			{
				array[i] = source[i];
			}
			else if (i >= index)
			{
				array[i + 1] = source[i];
			}
		}
		array[index] = element;
		return array;
	}

	// Token: 0x06005B6E RID: 23406 RVA: 0x0015A068 File Offset: 0x00158268
	public static T[] RemoveAt<T>(this T[] source, int index)
	{
		T[] array = new T[source.Length - 1];
		if (index > 0)
		{
			Array.Copy(source, 0, array, 0, index);
		}
		if (index < source.Length - 1)
		{
			Array.Copy(source, index + 1, array, index, source.Length - index - 1);
		}
		return array;
	}

	// Token: 0x06005B6F RID: 23407 RVA: 0x0015A0AC File Offset: 0x001582AC
	public static T[] Remove<T>(this T[] source, T element)
	{
		int num = source.IndexOf(element);
		if (num > -1)
		{
			source = source.RemoveAt(num);
		}
		return source;
	}

	// Token: 0x06005B70 RID: 23408 RVA: 0x0015A0D0 File Offset: 0x001582D0
	public static T[] Add<T>(this T[] source, T element)
	{
		T[] array = new T[source.Length + 1];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = source[i];
		}
		array[array.Length - 1] = element;
		return array;
	}

	// Token: 0x06005B71 RID: 23409 RVA: 0x0015A114 File Offset: 0x00158314
	public static int IndexOf<T>(this T[] source, T element)
	{
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i].Equals(element))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06005B72 RID: 23410 RVA: 0x00032232 File Offset: 0x00030432
	public static bool IsNativeNull(this object obj)
	{
		return obj == null || obj.Equals(null);
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x00032240 File Offset: 0x00030440
	public static string ToCIString(this float value)
	{
		return value.ToString(LocalizationManager.GetCurrentCultureInfo());
	}

	// Token: 0x04004BF3 RID: 19443
	private static List<Transform> m_recursiveLayersHelper = new List<Transform>();
}
