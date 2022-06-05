using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000970 RID: 2416
	[AddComponentMenu("Corgi Engine/Environment/Character Hztal Mvt Override")]
	public class CharacterHorizontalMovementOverride : MonoBehaviour
	{
		// Token: 0x0600520B RID: 21003 RVA: 0x00122CF0 File Offset: 0x00120EF0
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

		// Token: 0x0600520C RID: 21004 RVA: 0x00122D28 File Offset: 0x00120F28
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			CharacterHorizontalMovement component = collider.GetComponent<CharacterHorizontalMovement>();
			if (component == null)
			{
				return;
			}
			component.MovementSpeed = this._previousMovementSpeed;
		}

		// Token: 0x0400440E RID: 17422
		public float MovementSpeed = 8f;

		// Token: 0x0400440F RID: 17423
		protected float _previousMovementSpeed;
	}
}
