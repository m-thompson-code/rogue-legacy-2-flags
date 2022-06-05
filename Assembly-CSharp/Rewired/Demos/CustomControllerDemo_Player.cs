using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000ED9 RID: 3801
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class CustomControllerDemo_Player : MonoBehaviour
	{
		// Token: 0x17002405 RID: 9221
		// (get) Token: 0x06006E05 RID: 28165 RVA: 0x0003C700 File Offset: 0x0003A900
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

		// Token: 0x06006E06 RID: 28166 RVA: 0x0003C726 File Offset: 0x0003A926
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06006E07 RID: 28167 RVA: 0x001894A0 File Offset: 0x001876A0
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

		// Token: 0x04005870 RID: 22640
		public int playerId;

		// Token: 0x04005871 RID: 22641
		public float speed = 1f;

		// Token: 0x04005872 RID: 22642
		public float bulletSpeed = 20f;

		// Token: 0x04005873 RID: 22643
		public GameObject bulletPrefab;

		// Token: 0x04005874 RID: 22644
		private Player _player;

		// Token: 0x04005875 RID: 22645
		private CharacterController cc;
	}
}
