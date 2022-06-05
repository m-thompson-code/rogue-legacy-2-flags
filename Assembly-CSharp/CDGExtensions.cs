using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200079D RID: 1949
public static class CDGExtensions
{
	// Token: 0x060041E2 RID: 16866 RVA: 0x000EAD24 File Offset: 0x000E8F24
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

	// Token: 0x060041E3 RID: 16867 RVA: 0x000EAD9C File Offset: 0x000E8F9C
	public static void FindAllTransforms(this Transform parent, List<Transform> outputList)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			outputList.Add(child);
			child.FindAllTransforms(outputList);
		}
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x000EADD0 File Offset: 0x000E8FD0
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

	// Token: 0x060041E5 RID: 16869 RVA: 0x000EAE18 File Offset: 0x000E9018
	public static GameObject FindObjectReference(this GameObject gameObject, string id)
	{
		ObjectReferenceFinder component = gameObject.GetComponent<ObjectReferenceFinder>();
		if (component)
		{
			return component.GetObject(id);
		}
		return null;
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x000EAE40 File Offset: 0x000E9040
	public static T FindObjectReference<T>(this GameObject gameObject, string id, bool searchAllChildren = false, bool includeInactive = false) where T : UnityEngine.Object
	{
		ObjectReferenceFinder component = gameObject.GetComponent<ObjectReferenceFinder>();
		if (component)
		{
			return component.GetObject<T>(id, searchAllChildren, includeInactive);
		}
		return default(T);
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x000EAE70 File Offset: 0x000E9070
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

	// Token: 0x060041E8 RID: 16872 RVA: 0x000EAF24 File Offset: 0x000E9124
	public static GameObject GetRoot(this Component obj, bool includeInactive = false)
	{
		return obj.gameObject.GetRoot(includeInactive);
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x000EAF34 File Offset: 0x000E9134
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

	// Token: 0x060041EA RID: 16874 RVA: 0x000EAF6B File Offset: 0x000E916B
	public static bool Contains(this LayerMask mask, int layer)
	{
		return (mask.value & 1 << layer) > 0;
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x000EAF7E File Offset: 0x000E917E
	public static bool Contains(this LayerMask mask, GameObject gameobject)
	{
		return (mask.value & 1 << gameobject.layer) > 0;
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x000EAF98 File Offset: 0x000E9198
	public static void SetLayerRecursively(this GameObject gameObject, LayerMask layer, bool includeInactive = false)
	{
		CDGExtensions.m_recursiveLayersHelper.Clear();
		gameObject.GetComponentsInChildren<Transform>(includeInactive, CDGExtensions.m_recursiveLayersHelper);
		foreach (Transform transform in CDGExtensions.m_recursiveLayersHelper)
		{
			transform.gameObject.layer = layer;
		}
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x000EB008 File Offset: 0x000E9208
	public static void SetAlpha(this Image image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x000EB02C File Offset: 0x000E922C
	public static void SetAlpha(this RawImage image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x000EB050 File Offset: 0x000E9250
	public static void SetAlpha(this SpriteRenderer image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x000EB074 File Offset: 0x000E9274
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

	// Token: 0x060041F1 RID: 16881 RVA: 0x000EB0CC File Offset: 0x000E92CC
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

	// Token: 0x060041F2 RID: 16882 RVA: 0x000EB110 File Offset: 0x000E9310
	public static T[] Remove<T>(this T[] source, T element)
	{
		int num = source.IndexOf(element);
		if (num > -1)
		{
			source = source.RemoveAt(num);
		}
		return source;
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x000EB134 File Offset: 0x000E9334
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

	// Token: 0x060041F4 RID: 16884 RVA: 0x000EB178 File Offset: 0x000E9378
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

	// Token: 0x060041F5 RID: 16885 RVA: 0x000EB1B2 File Offset: 0x000E93B2
	public static bool IsNativeNull(this object obj)
	{
		return obj == null || obj.Equals(null);
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x000EB1C0 File Offset: 0x000E93C0
	public static string ToCIString(this float value)
	{
		return value.ToString(LocalizationManager.GetCurrentCultureInfo());
	}

	// Token: 0x0400393C RID: 14652
	private static List<Transform> m_recursiveLayersHelper = new List<Transform>();
}
