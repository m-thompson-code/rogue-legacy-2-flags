using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F25 RID: 3877
	[AddComponentMenu("Corgi Engine/Environment/Character Hztal Mvt Override")]
	public class CharacterHorizontalMovementOverride : MonoBehaviour
	{
		// Token: 0x0600701E RID: 28702 RVA: 0x001909CC File Offset: 0x0018EBCC
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			CharacterHorizontalMovement component = collider.GetComponent<CharacterHorizontalMovement>();
			if (component == null)
			{
				return;
			}
			this._previousMovementSpeed = component.MovementSpeed;
			component.MovementSpeed = this.MovementSpeed;
		}

		// Token: 0x0600701F RID: 28703 RVA: 0x00190A04 File Offset: 0x0018EC04
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			CharacterHorizontalMovement component = collider.GetComponent<CharacterHorizontalMovement>();
			if (component == null)
			{
				return;
			}
			component.MovementSpeed = this._previousMovementSpeed;
		}

		// Token: 0x04005A54 RID: 23124
		public float MovementSpeed = 8f;

		// Token: 0x04005A55 RID: 23125
		protected float _previousMovementSpeed;
	}
}
