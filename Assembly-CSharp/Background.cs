using System;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class Background : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06002512 RID: 9490 RVA: 0x0007B1FC File Offset: 0x000793FC
	// (set) Token: 0x06002513 RID: 9491 RVA: 0x0007B204 File Offset: 0x00079404
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x06002514 RID: 9492 RVA: 0x0007B20D File Offset: 0x0007940D
	public bool IsAwakeCalled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000EED RID: 3821
	// (get) Token: 0x06002515 RID: 9493 RVA: 0x0007B210 File Offset: 0x00079410
	// (set) Token: 0x06002516 RID: 9494 RVA: 0x0007B218 File Offset: 0x00079418
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

	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06002517 RID: 9495 RVA: 0x0007B221 File Offset: 0x00079421
	public bool IsTileable
	{
		get
		{
			return this.m_isTileable;
		}
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x0007B22C File Offset: 0x0007942C
	private void OnValidate()
	{
		if (this.Size.x < 1 || this.Size.y < 1)
		{
			this.Size = Vector2Int.one;
		}
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x0007B268 File Offset: 0x00079468
	private void Start()
	{
		CameraLayerController component = base.GetComponent<CameraLayerController>();
		if (component)
		{
			component.SetLayerAndSubLayer();
		}
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x0007B28A File Offset: 0x0007948A
	public void ResetValues()
	{
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x0007B28C File Offset: 0x0007948C
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x0007B2AF File Offset: 0x000794AF
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001F5A RID: 8026
	[SerializeField]
	private Vector2Int m_size = Vector2Int.one;

	// Token: 0x04001F5B RID: 8027
	[SerializeField]
	private bool m_isTileable = true;
}
