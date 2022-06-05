using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000941 RID: 2369
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class EightPlayersExample_Player : MonoBehaviour
	{
		// Token: 0x06005052 RID: 20562 RVA: 0x0011B4BF File Offset: 0x001196BF
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0011B4CD File Offset: 0x001196CD
		private void Initialize()
		{
			this.player = ReInput.players.GetPlayer(this.playerId);
			this.initialized = true;
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0011B4EC File Offset: 0x001196EC
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

		// Token: 0x06005055 RID: 20565 RVA: 0x0011B510 File Offset: 0x00119710
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0011B56C File Offset: 0x0011976C
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

		// Token: 0x040042AA RID: 17066
		public int playerId;

		// Token: 0x040042AB RID: 17067
		public float moveSpeed = 3f;

		// Token: 0x040042AC RID: 17068
		public float bulletSpeed = 15f;

		// Token: 0x040042AD RID: 17069
		public GameObject bulletPrefab;

		// Token: 0x040042AE RID: 17070
		private Player player;

		// Token: 0x040042AF RID: 17071
		private CharacterController cc;

		// Token: 0x040042B0 RID: 17072
		private Vector3 moveVector;

		// Token: 0x040042B1 RID: 17073
		private bool fire;

		// Token: 0x040042B2 RID: 17074
		[NonSerialized]
		private bool initialized;
	}
}
