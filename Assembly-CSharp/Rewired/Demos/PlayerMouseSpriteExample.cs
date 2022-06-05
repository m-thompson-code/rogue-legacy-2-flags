using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000943 RID: 2371
	[AddComponentMenu("")]
	public class PlayerMouseSpriteExample : MonoBehaviour
	{
		// Token: 0x06005061 RID: 20577 RVA: 0x0011B88C File Offset: 0x00119A8C
		private void Awake()
		{
			this.pointer = UnityEngine.Object.Instantiate<GameObject>(this.pointerPrefab);
			this.pointer.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			if (this.hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			this.mouse = PlayerMouse.Factory.Create();
			this.mouse.playerId = this.playerId;
			this.mouse.xAxis.actionName = this.horizontalAction;
			this.mouse.yAxis.actionName = this.verticalAction;
			this.mouse.wheel.yAxis.actionName = this.wheelAction;
			this.mouse.leftButton.actionName = this.leftButtonAction;
			this.mouse.rightButton.actionName = this.rightButtonAction;
			this.mouse.middleButton.actionName = this.middleButtonAction;
			this.mouse.pointerSpeed = 1f;
			this.mouse.wheel.yAxis.repeatRate = 5f;
			this.mouse.screenPosition = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
			this.mouse.ScreenPositionChangedEvent += this.OnScreenPositionChanged;
			this.OnScreenPositionChanged(this.mouse.screenPosition);
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0011BA00 File Offset: 0x00119C00
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.pointer.transform.Rotate(Vector3.forward, this.mouse.wheel.yAxis.value * 20f);
			if (this.mouse.leftButton.justPressed)
			{
				this.CreateClickEffect(new Color(0f, 1f, 0f, 1f));
			}
			if (this.mouse.rightButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 0f, 0f, 1f));
			}
			if (this.mouse.middleButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 1f, 0f, 1f));
			}
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0011BAD8 File Offset: 0x00119CD8
		private void OnDestroy()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.mouse.ScreenPositionChangedEvent -= this.OnScreenPositionChanged;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x0011BAFC File Offset: 0x00119CFC
		private void CreateClickEffect(Color color)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.clickEffectPrefab);
			gameObject.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(this.mouse.screenPosition.x, this.mouse.screenPosition.y, this.distanceFromCamera));
			gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
			UnityEngine.Object.Destroy(gameObject, 0.5f);
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x0011BB8C File Offset: 0x00119D8C
		private void OnScreenPositionChanged(Vector2 position)
		{
			Vector3 position2 = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, this.distanceFromCamera));
			this.pointer.transform.position = position2;
		}

		// Token: 0x040042BA RID: 17082
		[Tooltip("The Player that will control the mouse")]
		public int playerId;

		// Token: 0x040042BB RID: 17083
		[Tooltip("The Rewired Action used for the mouse horizontal axis.")]
		public string horizontalAction = "MouseX";

		// Token: 0x040042BC RID: 17084
		[Tooltip("The Rewired Action used for the mouse vertical axis.")]
		public string verticalAction = "MouseY";

		// Token: 0x040042BD RID: 17085
		[Tooltip("The Rewired Action used for the mouse wheel axis.")]
		public string wheelAction = "MouseWheel";

		// Token: 0x040042BE RID: 17086
		[Tooltip("The Rewired Action used for the mouse left button.")]
		public string leftButtonAction = "MouseLeftButton";

		// Token: 0x040042BF RID: 17087
		[Tooltip("The Rewired Action used for the mouse right button.")]
		public string rightButtonAction = "MouseRightButton";

		// Token: 0x040042C0 RID: 17088
		[Tooltip("The Rewired Action used for the mouse middle button.")]
		public string middleButtonAction = "MouseMiddleButton";

		// Token: 0x040042C1 RID: 17089
		[Tooltip("The distance from the camera that the pointer will be drawn.")]
		public float distanceFromCamera = 1f;

		// Token: 0x040042C2 RID: 17090
		[Tooltip("The scale of the sprite pointer.")]
		public float spriteScale = 0.05f;

		// Token: 0x040042C3 RID: 17091
		[Tooltip("The pointer prefab.")]
		public GameObject pointerPrefab;

		// Token: 0x040042C4 RID: 17092
		[Tooltip("The click effect prefab.")]
		public GameObject clickEffectPrefab;

		// Token: 0x040042C5 RID: 17093
		[Tooltip("Should the hardware pointer be hidden?")]
		public bool hideHardwarePointer = true;

		// Token: 0x040042C6 RID: 17094
		[NonSerialized]
		private GameObject pointer;

		// Token: 0x040042C7 RID: 17095
		[NonSerialized]
		private PlayerMouse mouse;
	}
}
