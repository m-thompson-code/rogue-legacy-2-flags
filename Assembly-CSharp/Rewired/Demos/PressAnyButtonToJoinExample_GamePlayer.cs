using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE5 RID: 3813
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressAnyButtonToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x1700240A RID: 9226
		// (get) Token: 0x06006E5C RID: 28252 RVA: 0x0003CACA File Offset: 0x0003ACCA
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

		// Token: 0x06006E5D RID: 28253 RVA: 0x0003CAE5 File Offset: 0x0003ACE5
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x0003CAF3 File Offset: 0x0003ACF3
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

		// Token: 0x06006E5F RID: 28255 RVA: 0x0018AB78 File Offset: 0x00188D78
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x0018ABD4 File Offset: 0x00188DD4
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

		// Token: 0x040058B1 RID: 22705
		public int playerId;

		// Token: 0x040058B2 RID: 22706
		public float moveSpeed = 3f;

		// Token: 0x040058B3 RID: 22707
		public float bulletSpeed = 15f;

		// Token: 0x040058B4 RID: 22708
		public GameObject bulletPrefab;

		// Token: 0x040058B5 RID: 22709
		private CharacterController cc;

		// Token: 0x040058B6 RID: 22710
		private Vector3 moveVector;

		// Token: 0x040058B7 RID: 22711
		private bool fire;
	}
}
