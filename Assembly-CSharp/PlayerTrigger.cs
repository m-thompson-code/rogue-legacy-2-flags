using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class PlayerTrigger : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060027EB RID: 10219 RVA: 0x000BC28C File Offset: 0x000BA48C
	// (remove) Token: 0x060027EC RID: 10220 RVA: 0x000BC2C4 File Offset: 0x000BA4C4
	public event EventHandler<EventArgs> PlayerEnter;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060027ED RID: 10221 RVA: 0x000BC2FC File Offset: 0x000BA4FC
	// (remove) Token: 0x060027EE RID: 10222 RVA: 0x000BC334 File Offset: 0x000BA534
	public event EventHandler<EventArgs> PlayerExit;

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x060027EF RID: 10223 RVA: 0x000166FE File Offset: 0x000148FE
	public bool IsTriggerActive
	{
		get
		{
			return this.m_collider != null && this.m_collider.enabled;
		}
	}

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x060027F0 RID: 10224 RVA: 0x0001671B File Offset: 0x0001491B
	// (set) Token: 0x060027F1 RID: 10225 RVA: 0x00016723 File Offset: 0x00014923
	public bool IsPlayerInTrigger { get; private set; }

	// Token: 0x060027F2 RID: 10226 RVA: 0x0001672C File Offset: 0x0001492C
	private IEnumerator Start()
	{
		if (this.m_requireButtonPress)
		{
			if (this.m_pressButtonIndicator == null)
			{
				Debug.LogFormat("<color=yellow>{0}: UI Window Trigger on ({1}) requires a Button Press, but you haven't set a Press Button Indicator</color>", new object[]
				{
					Time.frameCount,
					base.name
				});
			}
			Debug.LogFormat("{0}: ({1}) is waiting for Rewired...", new object[]
			{
				Time.frameCount,
				this
			});
			yield return new WaitUntil(() => ReInput.isReady);
			Debug.LogFormat("{0}: ({1}) finished waiting for Rewired", new object[]
			{
				Time.frameCount,
				this
			});
			ReInput.players.GetPlayer(0).AddInputEventDelegate(new Action<InputActionEventData>(this.OnInteractButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
		yield break;
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x0001673B File Offset: 0x0001493B
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x00016743 File Offset: 0x00014943
	private void OnDestroy()
	{
		if (ReInput.isReady)
		{
			ReInput.players.GetPlayer(0).RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnInteractButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000BC36C File Offset: 0x000BA56C
	public static PlayerTrigger CreateInstance(Vector2 size, Vector2 position, Transform parent = null)
	{
		GameObject gameObject = new GameObject("Exit");
		gameObject.transform.SetParent(parent);
		gameObject.transform.position = position;
		gameObject.layer = LayerMask.NameToLayer("Terrain_Hitbox");
		PlayerTrigger playerTrigger = gameObject.AddComponent<PlayerTrigger>();
		gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
		playerTrigger.m_collider = gameObject.AddComponent<BoxCollider2D>();
		playerTrigger.m_collider.isTrigger = true;
		(playerTrigger.m_collider as BoxCollider2D).size = size;
		return playerTrigger;
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000BC3EC File Offset: 0x000BA5EC
	public void SetTriggerActive(bool isActive)
	{
		if (isActive)
		{
			this.Reset();
		}
		if (this.m_collider == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
			if (this.m_collider == null)
			{
				Debug.LogFormat("<color=red>{0}: No Collider found on GameObject.</color>", new object[]
				{
					Time.frameCount
				});
				return;
			}
		}
		this.m_collider.enabled = isActive;
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x0001676F File Offset: 0x0001496F
	public void Reset()
	{
		this.IsPlayerInTrigger = false;
		if (this.m_pressButtonIndicator != null)
		{
			this.m_pressButtonIndicator.SetActive(false);
		}
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000BC454 File Offset: 0x000BA654
	private void OnInteractButtonDown(InputActionEventData inputActionEventData)
	{
		if (this.m_requireButtonPress && this.IsPlayerInTrigger && !PlayerManager.GetPlayerController().CharacterCorgi.IsFrozen)
		{
			if (this.m_printDebug)
			{
				Debug.LogFormat("{0}: Player pressed interact Button on ({1})", new object[]
				{
					Time.frameCount,
					base.name
				});
			}
			if (this.PlayerEnter != null)
			{
				this.PlayerEnter(this, EventArgs.Empty);
			}
		}
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x00016792 File Offset: 0x00014992
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") || collision.CompareTag("Player_Dodging"))
		{
			this.PlayerEnterTrigger();
		}
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000BC4CC File Offset: 0x000BA6CC
	private void PlayerEnterTrigger()
	{
		if (this.m_printDebug)
		{
			Debug.LogFormat("<color=green>{0}: Player Entered PlayerTrigger</color>", new object[]
			{
				Time.frameCount
			});
		}
		this.IsPlayerInTrigger = true;
		if (this.m_requireButtonPress && this.m_pressButtonIndicator != null)
		{
			this.m_pressButtonIndicator.SetActive(true);
			return;
		}
		if (this.PlayerEnter != null)
		{
			this.PlayerEnter(this, EventArgs.Empty);
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000BC544 File Offset: 0x000BA744
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (this.m_printDebug)
		{
			Debug.LogFormat("<color=purple>{0}: Player Exited PlayerTrigger</color>", new object[]
			{
				Time.frameCount
			});
		}
		if (collision.CompareTag("Player") || collision.CompareTag("Player_Dodging"))
		{
			this.IsPlayerInTrigger = false;
			if (this.m_requireButtonPress && this.m_pressButtonIndicator != null)
			{
				this.m_pressButtonIndicator.SetActive(false);
				return;
			}
			if (this.PlayerExit != null)
			{
				this.PlayerExit(this, EventArgs.Empty);
			}
		}
	}

	// Token: 0x0400232F RID: 9007
	[SerializeField]
	private bool m_requireButtonPress;

	// Token: 0x04002330 RID: 9008
	[SerializeField]
	private GameObject m_pressButtonIndicator;

	// Token: 0x04002331 RID: 9009
	[SerializeField]
	private bool m_printDebug;

	// Token: 0x04002332 RID: 9010
	private Collider2D m_collider;
}
