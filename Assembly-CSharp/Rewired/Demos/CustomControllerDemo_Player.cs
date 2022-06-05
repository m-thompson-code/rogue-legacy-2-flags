using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x0200093D RID: 2365
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class CustomControllerDemo_Player : MonoBehaviour
	{
		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600502E RID: 20526 RVA: 0x0011A9A6 File Offset: 0x00118BA6
		private Player player
		{
			get
			{
				if (this._player == null)
				{
					this._player = ReInput.players.GetPlayer(this.playerId);
				}
				return this._player;
			}
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x0011A9CC File Offset: 0x00118BCC
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x0011A9DC File Offset: 0x00118BDC
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Vector2 a = new Vector2(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical"));
			this.cc.Move(a * this.speed * Time.deltaTime);
			if (this.player.GetButtonDown("Fire"))
			{
				Vector3 b = Vector3.Scale(new Vector3(1f, 0f, 0f), base.transform.right);
				UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + b, Quaternion.identity).GetComponent<Rigidbody>().velocity = new Vector3(this.bulletSpeed * base.transform.right.x, 0f, 0f);
			}
			if (this.player.GetButtonDown("Change Color"))
			{
				Renderer component = base.GetComponent<Renderer>();
				Material material = component.material;
				material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
				component.material = material;
			}
		}

		// Token: 0x04004290 RID: 17040
		public int playerId;

		// Token: 0x04004291 RID: 17041
		public float speed = 1f;

		// Token: 0x04004292 RID: 17042
		public float bulletSpeed = 20f;

		// Token: 0x04004293 RID: 17043
		public GameObject bulletPrefab;

		// Token: 0x04004294 RID: 17044
		private Player _player;

		// Token: 0x04004295 RID: 17045
		private CharacterController cc;
	}
}
