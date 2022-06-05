using System;
using System.Collections;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200091C RID: 2332
public class ParadeTransition : MonoBehaviour, IRootObj
{
	// Token: 0x060046D8 RID: 18136 RVA: 0x00026E0E File Offset: 0x0002500E
	public void TransitionToParade()
	{
		base.StartCoroutine(this.TransitionToParadeCoroutine());
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x00026E1D File Offset: 0x0002501D
	private IEnumerator TransitionToParadeCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerManager.GetPlayerController().StopActiveAbilities(true);
		ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(TransitionID.FadeToBlackNoLoading) as ISceneLoadingTransition;
		transition.GameObject.SetActive(true);
		yield return null;
		yield return transition.TransitionIn();
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterParade, null, null);
		WindowManager.SetWindowIsOpen(WindowID.PlayerDeath, true);
		yield return null;
		yield return transition.TransitionOut();
		transition.GameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400368A RID: 13962
	[SerializeField]
	private GameObject m_playerEnterPositionObj;
}
