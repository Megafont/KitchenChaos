using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong ClientId;
    public int ColorIndex;
    public FixedString64Bytes PlayerName; // This is a fixed string because serializer.SerializeValue (see NetworkSerialize() below) does not support strings.
    public FixedString64Bytes PlayerId;



    public bool Equals(PlayerData other)
    {
        return ClientId == other.ClientId && 
               ColorIndex == other.ColorIndex &&
               PlayerName == other.PlayerName &&
               PlayerId == other.PlayerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref ColorIndex);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref PlayerId);
    }
}
