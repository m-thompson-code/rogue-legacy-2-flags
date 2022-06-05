using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000949 RID: 2377
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressStartToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x0011C779 File Offset: 0x0011A979
		private Player player
		{
			get
			{
				return PressStartToJoinExample_Assigner.GetRewiredPlayer(this.gamePlayerId);
			}
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x0011C786 File Offset: 0x0011A986
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x0011C794 File Offset: 0x0011A994
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

		// Token: 0x06005091 RID: 20625 RVA: 0x0011C7B4 File Offset: 0x0011A9B4
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x0011C810 File Offset: 0x0011AA10
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

		// Token: 0x040042D9 RID: 17113
		public int gamePlayerId;

		// Token: 0x040042DA RID: 17114
		public float moveSpeed = 3f;

		// Token: 0x040042DB RID: 17115
		public float bulletSpeed = 15f;

		// Token: 0x040042DC RID: 17116
		public GameObject bulletPrefab;

		// Token: 0x040042DD RID: 17117
		private CharacterController cc;

		// Token: 0x040042DE RID: 17118
		private Vector3 moveVector;

		// Token: 0x040042DF RID: 17119
		private bool fire;
	}
}
