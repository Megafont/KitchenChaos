using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong ClientId;
    public int ColorIndex;



    public bool Equals(PlayerData other)
    {
        return ClientId == other.ClientId && ColorIndex == other.ColorIndex;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref ColorIndex);
    }
}
