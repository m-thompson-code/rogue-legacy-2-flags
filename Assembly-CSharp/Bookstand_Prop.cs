using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A18 RID: 2584
public class Bookstand_Prop : Prop
{
	// Token: 0x06004DF3 RID: 19955 RVA: 0x0012C9E0 File Offset: 0x0012ABE0
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

	// Token: 0x04003AE5 RID: 15077
	[SerializeField]
	private SpriteRenderer m_legs;

	// Token: 0x04003AE6 RID: 15078
	private const int MAX_RAYCAST_DISTANCE = 18;

	// Token: 0x04003AE7 RID: 15079
	private List<RaycastHit2D> m_hitInfo = new List<RaycastHit2D>(2);
}
