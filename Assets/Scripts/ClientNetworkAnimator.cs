using System.Collections;
using System.Collections.Generic;

using Unity.Netcode.Components;
using UnityEngine;


/// <summary>
/// A client-authoritative NetworkAnimator component.
/// </summary>
public class ClientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
