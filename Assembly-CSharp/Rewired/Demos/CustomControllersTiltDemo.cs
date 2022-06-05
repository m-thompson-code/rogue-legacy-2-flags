using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000ED7 RID: 3799
	[AddComponentMenu("")]
	public class CustomControllersTiltDemo : MonoBehaviour
	{
		// Token: 0x06006DF6 RID: 28150 RVA: 0x0018910C File Offset: 0x0018730C
		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			this.player = ReInput.players.GetPlayer(0);
			ReInput.InputSourceUpdateEvent += this.OnInputUpdate;
			this.controller = (CustomController)this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x00189164 File Offset: 0x00187364
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

		// Token: 0x06006DF8 RID: 28152 RVA: 0x001891EC File Offset: 0x001873EC
		private void OnInputUpdate()
		{
			Vector3 acceleration = Input.acceleration;
			this.controller.SetAxisValue(0, acceleration.x);
			this.controller.SetAxisValue(1, acceleration.y);
			this.controller.SetAxisValue(2, acceleration.z);
		}

		// Token: 0x04005861 RID: 22625
		public Transform target;

		// Token: 0x04005862 RID: 22626
		public float speed = 10f;

		// Token: 0x04005863 RID: 22627
		private CustomController controller;

		// Token: 0x04005864 RID: 22628
		private Player player;
	}
}
