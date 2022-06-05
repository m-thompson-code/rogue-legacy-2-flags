using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class Bookstand_Prop : Prop
{
	// Token: 0x0600376A RID: 14186 RVA: 0x000BDE04 File Offset: 0x000BC004
	public override void Initialize(BaseRoom room, PropSpawnController propSpawnController)
	{
		base.Initialize(room, propSpawnController);
		base.transform.rotation = Quaternion.identity;
		float y = this.m_legs.bounds.max.y;
		Vector2 origin = new Vector2(base.transform.position.x, y);
		int mask = LayerMask.GetMask(new string[]
		{
			"Platform_CollidesWithAll",
			"Platform_CollidesWithEnemy",
			"Platform_CollidesWithPlayer",
			"Platform_OneWay"
		});
		ContactFilter2D contactFilter = default(ContactFilter2D);
		contactFilter.SetLayerMask(mask);
		int num = Physics2D.Raycast(origin, Vector2.down, contactFilter, this.m_hitInfo, 18f);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				if (!this.m_hitInfo[i].collider.gameObject.CompareTag("Untagged"))
				{
					float distance = this.m_hitInfo[i].distance;
					float num2 = 1f;
					if (base.transform.lossyScale.y != 0f)
					{
						num2 = 1f / base.transform.lossyScale.y;
					}
					this.m_legs.size = new Vector2(this.m_legs.size.x, num2 * distance);
					return;
				}
			}
		}
	}

	// Token: 0x04002A93 RID: 10899
	[SerializeField]
	private SpriteRenderer m_legs;

	// Token: 0x04002A94 RID: 10900
	private const int MAX_RAYCAST_DISTANCE = 18;

	// Token: 0x04002A95 RID: 10901
	private List<RaycastHit2D> m_hitInfo = new List<RaycastHit2D>(2);
}
