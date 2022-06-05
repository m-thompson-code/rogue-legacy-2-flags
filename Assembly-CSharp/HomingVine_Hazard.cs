using System;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class HomingVine_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06002898 RID: 10392 RVA: 0x00086253 File Offset: 0x00084453
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x0008625A File Offset: 0x0008445A
	private void OnEnable()
	{
		this.ResetHazard();
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x00086264 File Offset: 0x00084464
	private void Update()
	{
		this.m_currentAngle = Mathf.LerpAngle(this.m_currentAngle, CDGHelper.AngleBetweenPts(this.m_pivot.transform.position, PlayerManager.GetPlayerController().Midpoint), Time.fixedDeltaTime);
		this.m_pivot.transform.rotation = Quaternion.Euler(0f, 0f, this.m_currentAngle - 90f);
		this.m_length += 0.01f;
		this.m_vineSprite.size = new Vector2(this.m_vineSprite.size.x, this.m_length);
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x00086313 File Offset: 0x00084513
	public override void ResetHazard()
	{
		this.m_length = 0f;
		this.m_vineSprite.size = new Vector2(this.m_vineSprite.size.x, 0f);
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x0008634D File Offset: 0x0008454D
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002186 RID: 8582
	[SerializeField]
	private SpriteRenderer m_vineSprite;

	// Token: 0x04002187 RID: 8583
	private float m_length;

	// Token: 0x04002188 RID: 8584
	private float m_currentAngle;
}
