using UnityEngine;

public class Utils
{
    public static bool MatchesLayer(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }    
}