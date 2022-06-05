using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE8 RID: 3816
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressStartToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x1700240B RID: 9227
		// (get) Token: 0x06006E69 RID: 28265 RVA: 0x0003CB68 File Offset: 0x0003AD68
		private Player player
		{
			get
			{
				return PressStartToJoinExample_Assigner.GetRewiredPlayer(this.gamePlayerId);
			}
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x0003CB75 File Offset: 0x0003AD75
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x0003CB83 File Offset: 0x0003AD83
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

		// Token: 0x06006E6C RID: 28268 RVA: 0x0018AE0C File Offset: 0x0018900C
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06006E6D RID: 28269 RVA: 0x0018AE68 File Offset: 0x00189068
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

		// Token: 0x040058BE RID: 22718
		public int gamePlayerId;

		// Token: 0x040058BF RID: 22719
		public float moveSpeed = 3f;

		// Token: 0x040058C0 RID: 22720
		public float bulletSpeed = 15f;

		// Token: 0x040058C1 RID: 22721
		public GameObject bulletPrefab;

		// Token: 0x040058C2 RID: 22722
		private CharacterController cc;

		// Token: 0x040058C3 RID: 22723
		private Vector3 moveVector;

		// Token: 0x040058C4 RID: 22724
		private bool fire;
	}
}
