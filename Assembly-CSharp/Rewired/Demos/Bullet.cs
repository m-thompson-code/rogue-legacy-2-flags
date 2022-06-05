using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x0200094A RID: 2378
	[AddComponentMenu("")]
	public class Bullet : MonoBehaviour
	{
		// Token: 0x06005094 RID: 20628 RVA: 0x0011C8E0 File Offset: 0x0011AAE0
		private void Start()
		{
			if (this.lifeTime > 0f)
			{
				this.deathTime = Time.time + this.lifeTime;
				this.die = true;
			}
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x0011C908 File Offset: 0x0011AB08
		private void Update()
		{
			if (this.die && Time.time >= this.deathTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040042E0 RID: 17120
		public float lifeTime = 3f;

		// Token: 0x040042E1 RID: 17121
		private bool die;

		// Token: 0x040042E2 RID: 17122
		private float deathTime;
	}
}
