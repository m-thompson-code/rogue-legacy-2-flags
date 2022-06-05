using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class PolygonColliderHelper : MonoBehaviour
{
	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x0600260E RID: 9742 RVA: 0x0001524D File Offset: 0x0001344D
	// (set) Token: 0x0600260F RID: 9743 RVA: 0x00015259 File Offset: 0x00013459
	public static CompositeCollider2D CompositeCollider
	{
		get
		{
			return PolygonColliderHelper.Instance.m_compositeCollider;
		}
		set
		{
			PolygonColliderHelper.Instance.m_compositeCollider = value;
		}
	}

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x06002610 RID: 9744 RVA: 0x00015266 File Offset: 0x00013466
	// (set) Token: 0x06002611 RID: 9745 RVA: 0x0001526D File Offset: 0x0001346D
	private static PolygonColliderHelper Instance
	{
		get
		{
			return PolygonColliderHelper.m_instance;
		}
		set
		{
			PolygonColliderHelper.m_instance = value;
		}
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x00015275 File Offset: 0x00013475
	private void Awake()
	{
		if (PolygonColliderHelper.Instance == null)
		{
			PolygonColliderHelper.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x000B4A1C File Offset: 0x000B2C1C
	private void OnDestroy()
	{
		PolygonColliderHelper.m_instance = null;
		if (PolygonColliderHelper.m_colliders != null)
		{
			for (int i = PolygonColliderHelper.m_colliders.Count - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(PolygonColliderHelper.m_colliders[i]);
			}
			PolygonColliderHelper.m_colliders.Clear();
			PolygonColliderHelper.m_colliders = null;
		}
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x000B4A70 File Offset: 0x000B2C70
	public static void UpdateCompositeCollider(PolygonCollider2D[] collider2Ds)
	{
		if (PolygonColliderHelper.m_colliders == null)
		{
			PolygonColliderHelper.m_colliders = new List<PolygonCollider2D>();
		}
		if (PolygonColliderHelper.m_colliders.Count < collider2Ds.Length)
		{
			int num = collider2Ds.Length - PolygonColliderHelper.m_colliders.Count;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = new GameObject("Collider");
				gameObject.transform.SetParent(PolygonColliderHelper.Instance.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.AddComponent<PolygonCollider2D>();
				PolygonCollider2D component = gameObject.GetComponent<PolygonCollider2D>();
				component.usedByComposite = true;
				component.isTrigger = true;
				PolygonColliderHelper.m_colliders.Add(component);
				gameObject.gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < collider2Ds.Length; j++)
		{
			PolygonColliderHelper.m_colliders[j].transform.localPosition = collider2Ds[j].transform.localPosition;
			PolygonColliderHelper.m_colliders[j].gameObject.SetActive(true);
			PolygonColliderHelper.m_colliders[j].SetPath(0, collider2Ds[j].points);
		}
		PolygonColliderHelper.Instance.m_compositeCollider.GenerateGeometry();
		for (int k = 0; k < PolygonColliderHelper.m_colliders.Count; k++)
		{
			PolygonColliderHelper.m_colliders[k].gameObject.SetActive(false);
		}
	}

	// Token: 0x04002100 RID: 8448
	[SerializeField]
	private CompositeCollider2D m_compositeCollider;

	// Token: 0x04002101 RID: 8449
	[SerializeField]
	private PolygonCollider2D[] m_debugPolygonColliders;

	// Token: 0x04002102 RID: 8450
	private static List<PolygonCollider2D> m_colliders;

	// Token: 0x04002103 RID: 8451
	private static PolygonColliderHelper m_instance;
}
