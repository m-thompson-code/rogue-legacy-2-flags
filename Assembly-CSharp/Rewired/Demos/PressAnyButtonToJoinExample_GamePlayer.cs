using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000947 RID: 2375
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressAnyButtonToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x06005082 RID: 20610 RVA: 0x0011C45D File Offset: 0x0011A65D
		private Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x0011C478 File Offset: 0x0011A678
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x0011C486 File Offset: 0x0011A686
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.player == null)
			{
				return;
			}
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x0011C4A8 File Offset: 0x0011A6A8
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0011C504 File Offset: 0x0011A704
		private void ProcessInput()
		{
			if (this.moveVector.x != 0f || this.moveVector.y != 0f)
			{
				this.cc.Move(this.moveVector * this.moveSpeed * Time.deltaTime);
			}
			if (this.fire)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + base.transform.right, base.transform.rotation).GetComponent<Rigidbody>().AddForce(base.transform.right * this.bulletSpeed, ForceMode.VelocityChange);
			}
		}

		// Token: 0x040042CE RID: 17102
		public int playerId;

		// Token: 0x040042CF RID: 17103
		public float moveSpeed = 3f;

		// Token: 0x040042D0 RID: 17104
		public float bulletSpeed = 15f;

		// Token: 0x040042D1 RID: 17105
		public GameObject bulletPrefab;

		// Token: 0x040042D2 RID: 17106
		private CharacterController cc;

		// Token: 0x040042D3 RID: 17107
		private Vector3 moveVector;

		// Token: 0x040042D4 RID: 17108
		private bool fire;
	}
}
