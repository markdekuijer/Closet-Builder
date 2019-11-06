//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Sends UnityEvents for basic hand interactions
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class InteractableHoverEvents : MonoBehaviour
	{
        public bool Activated = true;

		public UnityEvent onHandHoverBegin;
		public UnityEvent onHandHoverEnd;
		public UnityEvent onAttachedToHand;
		public UnityEvent onDetachedFromHand;

		//-------------------------------------------------
		private void OnHandHoverBegin()
		{
            if(!Activated) { return; }

			onHandHoverBegin.Invoke();
		}


		//-------------------------------------------------
		private void OnHandHoverEnd()
        {
            if (!Activated) { return; }

            onHandHoverEnd.Invoke();
		}


		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
        {
            if (!Activated) { return; }

            onAttachedToHand.Invoke();
		}


		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
        {
            if (!Activated) { return; }

            onDetachedFromHand.Invoke();
		}
	}
}
