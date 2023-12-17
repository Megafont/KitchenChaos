using Unity.Netcode.Components;
using UnityEngine;


// NOTE: The code in this file is from the Unity Netcode documentation:
//       https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/index.html
namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    /// <summary>
    /// A client-authoritative NetworkTransform component.
    ///
    /// Used for syncing a transform with client side changes. This includes host. Pure server as owner isn't supported by this. Please use NetworkTransform
    /// for transforms that'll always be owned by the server.
    /// </summary>
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        /// <summary>
        /// Used to determine who can write to this transform. Owner client only.
        /// This imposes state to the server. This is putting trust on your clients. Make sure no security-sensitive features use this transform.
        /// </summary>
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}