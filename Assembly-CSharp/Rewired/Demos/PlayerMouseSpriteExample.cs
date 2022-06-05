using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE1 RID: 3809
	[AddComponentMenu("")]
	public class PlayerMouseSpriteExample : MonoBehaviour
	{
		// Token: 0x06006E3B RID: 28219 RVA: 0x0018A0CC File Offset: 0x001882CC
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

		// Token: 0x06006E3C RID: 28220 RVA: 0x0018A240 File Offset: 0x00188440
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

		// Token: 0x06006E3D RID: 28221 RVA: 0x0003C9AB File Offset: 0x0003ABAB
		private void OnDestroy()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.mouse.ScreenPositionChangedEvent -= this.OnScreenPositionChanged;
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x0018A318 File Offset: 0x00188518
		private void CreateClickEffect(Color color)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.clickEffectPrefab);
			gameObject.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(this.mouse.screenPosition.x, this.mouse.screenPosition.y, this.distanceFromCamera));
			gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
			UnityEngine.Object.Destroy(gameObject, 0.5f);
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x0018A3A8 File Offset: 0x001885A8
		private void OnScreenPositionChanged(Vector2 position)
		{
			Vector3 position2 = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, this.distanceFromCamera));
			this.pointer.transform.position = position2;
		}

		// Token: 0x0400589D RID: 22685
		[Tooltip("The Player that will control the mouse")]
		public int playerId;

		// Token: 0x0400589E RID: 22686
		[Tooltip("The Rewired Action used for the mouse horizontal axis.")]
		public string horizontalAction = "MouseX";

		// Token: 0x0400589F RID: 22687
		[Tooltip("The Rewired Action used for the mouse vertical axis.")]
		public string verticalAction = "MouseY";

		// Token: 0x040058A0 RID: 22688
		[Tooltip("The Rewired Action used for the mouse wheel axis.")]
		public string wheelAction = "MouseWheel";

		// Token: 0x040058A1 RID: 22689
		[Tooltip("The Rewired Action used for the mouse left button.")]
		public string leftButtonAction = "MouseLeftButton";

		// Token: 0x040058A2 RID: 22690
		[Tooltip("The Rewired Action used for the mouse right button.")]
		public string rightButtonAction = "MouseRightButton";

		// Token: 0x040058A3 RID: 22691
		[Tooltip("The Rewired Action used for the mouse middle button.")]
		public string middleButtonAction = "MouseMiddleButton";

		// Token: 0x040058A4 RID: 22692
		[Tooltip("The distance from the camera that the pointer will be drawn.")]
		public float distanceFromCamera = 1f;

		// Token: 0x040058A5 RID: 22693
		[Tooltip("The scale of the sprite pointer.")]
		public float spriteScale = 0.05f;

		// Token: 0x040058A6 RID: 22694
		[Tooltip("The pointer prefab.")]
		public GameObject pointerPrefab;

		// Token: 0x040058A7 RID: 22695
		[Tooltip("The click effect prefab.")]
		public GameObject clickEffectPrefab;

		// Token: 0x040058A8 RID: 22696
		[Tooltip("Should the hardware pointer be hidden?")]
		public bool hideHardwarePointer = true;

		// Token: 0x040058A9 RID: 22697
		[NonSerialized]
		private GameObject pointer;

		// Token: 0x040058AA RID: 22698
		[NonSerialized]
		private PlayerMouse mouse;
	}
}
