using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : Powerup<VRMovement>
{
    VRMovement Player;
    public override void Initialize(VRMovement _player)
    {
        LifeTimeCurrent = 2.0f;
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
                Player.isFlying = true;
            }
            else
            {
                Player.isFlying = false;
            }
        }
    }
}
