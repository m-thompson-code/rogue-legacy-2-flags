using System;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class HomingVine_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x170014FD RID: 5373
	// (get) Token: 0x06003800 RID: 14336 RVA: 0x00005391 File Offset: 0x00003591
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x0001E5DF File Offset: 0x0001C7DF
	private void OnEnable()
	{
		this.ResetHazard();
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x000E7144 File Offset: 0x000E5344
	private void Update()
	{
		this.m_currentAngle = Mathf.LerpAngle(this.m_currentAngle, CDGHelper.AngleBetweenPts(this.m_pivot.transform.position, PlayerManager.GetPlayerController().Midpoint), Time.fixedDeltaTime);
		this.m_pivot.transform.rotation = Quaternion.Euler(0f, 0f, this.m_currentAngle - 90f);
		this.m_length += 0.01f;
		this.m_vineSprite.size = new Vector2(this.m_vineSprite.size.x, this.m_length);
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x0001EBB7 File Offset: 0x0001CDB7
	public override void ResetHazard()
	{
		this.m_length = 0f;
		this.m_vineSprite.size = new Vector2(this.m_vineSprite.size.x, 0f);
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CF4 RID: 11508
	[SerializeField]
	private SpriteRenderer m_vineSprite;

	// Token: 0x04002CF5 RID: 11509
	private float m_length;

	// Token: 0x04002CF6 RID: 11510
	private float m_currentAngle;
}
