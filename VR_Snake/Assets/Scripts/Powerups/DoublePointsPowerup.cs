using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointsPowerup : Powerup<VRMovement>
{
    VRMovement Player;
    public override void Initialize(VRMovement _player)
    {
        LifeTimeCurrent = LifeTimeDefault;
        this.Player = _player;
        AffectPlayer(false);
        isInitialized = true;
    }
    protected override void AffectPlayer(bool reverse)
    {
        if (Player != null)
        {
            if (!reverse)
            {
                Player.doublePoints = true; ;
            }
            else
            {
                Player.doublePoints = false;
            }
        }
    }
}

