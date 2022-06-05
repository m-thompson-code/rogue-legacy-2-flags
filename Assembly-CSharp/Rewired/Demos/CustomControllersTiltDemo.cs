using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x0200093B RID: 2363
	[AddComponentMenu("")]
	public class CustomControllersTiltDemo : MonoBehaviour
	{
		// Token: 0x0600501F RID: 20511 RVA: 0x0011A590 File Offset: 0x00118790
		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			this.player = ReInput.players.GetPlayer(0);
			ReInput.InputSourceUpdateEvent += this.OnInputUpdate;
			this.controller = (CustomController)this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x0011A5E8 File Offset: 0x001187E8
		private void Update()
		{
			if (this.target == null)
			{
				return;
			}
			Vector3 a = Vector3.zero;
			a.y = this.player.GetAxis("Tilt Vertical");
			a.x = this.player.GetAxis("Tilt Horizontal");
			if (a.sqrMagnitude > 1f)
			{
				a.Normalize();
			}
			a *= Time.deltaTime;
			this.target.Translate(a * this.speed);
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x0011A670 File Offset: 0x00118870
		private void OnInputUpdate()
		{
			Vector3 acceleration = Input.acceleration;
			this.controller.SetAxisValue(0, acceleration.x);
			this.controller.SetAxisValue(1, acceleration.y);
			this.controller.SetAxisValue(2, acceleration.z);
		}

		// Token: 0x04004281 RID: 17025
		public Transform target;

		// Token: 0x04004282 RID: 17026
		public float speed = 10f;

		// Token: 0x04004283 RID: 17027
		private CustomController controller;

		// Token: 0x04004284 RID: 17028
		private Player player;
	}
}
