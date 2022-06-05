using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EDF RID: 3807
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class EightPlayersExample_Player : MonoBehaviour
	{
		// Token: 0x06006E2C RID: 28204 RVA: 0x0003C8DF File Offset: 0x0003AADF
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06006E2D RID: 28205 RVA: 0x0003C8ED File Offset: 0x0003AAED
		private void Initialize()
		{
			this.player = ReInput.players.GetPlayer(this.playerId);
			this.initialized = true;
		}

		// Token: 0x06006E2E RID: 28206 RVA: 0x0003C90C File Offset: 0x0003AB0C
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x06006E2F RID: 28207 RVA: 0x00189DDC File Offset: 0x00187FDC
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06006E30 RID: 28208 RVA: 0x00189E38 File Offset: 0x00188038
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

		// Token: 0x0400588D RID: 22669
		public int playerId;

		// Token: 0x0400588E RID: 22670
		public float moveSpeed = 3f;

		// Token: 0x0400588F RID: 22671
		public float bulletSpeed = 15f;

		// Token: 0x04005890 RID: 22672
		public GameObject bulletPrefab;

		// Token: 0x04005891 RID: 22673
		private Player player;

		// Token: 0x04005892 RID: 22674
		private CharacterController cc;

		// Token: 0x04005893 RID: 22675
		private Vector3 moveVector;

		// Token: 0x04005894 RID: 22676
		private bool fire;

		// Token: 0x04005895 RID: 22677
		[NonSerialized]
		private bool initialized;
	}
}
