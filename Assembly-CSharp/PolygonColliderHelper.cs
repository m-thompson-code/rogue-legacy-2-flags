using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class PolygonColliderHelper : MonoBehaviour
{
	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06001B72 RID: 7026 RVA: 0x00057CE6 File Offset: 0x00055EE6
	// (set) Token: 0x06001B73 RID: 7027 RVA: 0x00057CF2 File Offset: 0x00055EF2
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

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00057CFF File Offset: 0x00055EFF
	// (set) Token: 0x06001B75 RID: 7029 RVA: 0x00057D06 File Offset: 0x00055F06
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

	// Token: 0x06001B76 RID: 7030 RVA: 0x00057D0E File Offset: 0x00055F0E
	private void Awake()
	{
		if (PolygonColliderHelper.Instance == null)
		{
			PolygonColliderHelper.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x00057D30 File Offset: 0x00055F30
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

	// Token: 0x06001B78 RID: 7032 RVA: 0x00057D84 File Offset: 0x00055F84
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

	// Token: 0x04001924 RID: 6436
	[SerializeField]
	private CompositeCollider2D m_compositeCollider;

	// Token: 0x04001925 RID: 6437
	[SerializeField]
	private PolygonCollider2D[] m_debugPolygonColliders;

	// Token: 0x04001926 RID: 6438
	private static List<PolygonCollider2D> m_colliders;

	// Token: 0x04001927 RID: 6439
	private static PolygonColliderHelper m_instance;
}
