using System;
using UnityEngine;

// Token: 0x02000693 RID: 1683
public class Background : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x0600336C RID: 13164 RVA: 0x0001C2E0 File Offset: 0x0001A4E0
	// (set) Token: 0x0600336D RID: 13165 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x0600336E RID: 13166 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsAwakeCalled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x0600336F RID: 13167 RVA: 0x0001C2F1 File Offset: 0x0001A4F1
	// (set) Token: 0x06003370 RID: 13168 RVA: 0x0001C2F9 File Offset: 0x0001A4F9
	public Vector2Int Size
	{
		get
		{
			return this.m_size;
		}
		private set
		{
			this.m_size = value;
		}
	}

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x06003371 RID: 13169 RVA: 0x0001C302 File Offset: 0x0001A502
	public bool IsTileable
	{
		get
		{
			return this.m_isTileable;
		}
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x000DB368 File Offset: 0x000D9568
	private void OnValidate()
	{
		if (this.Size.x < 1 || this.Size.y < 1)
		{
			this.Size = Vector2Int.one;
		}
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x000DB3A4 File Offset: 0x000D95A4
	private void Start()
	{
		CameraLayerController component = base.GetComponent<CameraLayerController>();
		if (component)
		{
			component.SetLayerAndSubLayer();
		}
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x00002FCA File Offset: 0x000011CA
	public void ResetValues()
	{
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x0001BE85 File Offset: 0x0001A085
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040029EC RID: 10732
	[SerializeField]
	private Vector2Int m_size = Vector2Int.one;

	// Token: 0x040029ED RID: 10733
	[SerializeField]
	private bool m_isTileable = true;
}
