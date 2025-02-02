﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : EnemyController
{  
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!hasPickedUpOrb && game.GetComponent<GameController>().orbs.Count == 0 /*&& health <= getInitialHealth()*/)
        {
            rotateTowardsPlayer();

            moveTowardsPlayer();
        }
        else if (!hasPickedUpOrb)
        {
            GameObject mostVal = findNearestMostValuableOrb();

            if (mostVal == null)
            {
                // Do nothing
            }
            else
            {
                rotateTowardsNearestOrb(mostVal);

                moveTowardsNearestOrb();
            }
        }
        else
        {
            rotateAwayFromPlayer();

            moveAwayFromPlayer();

            speed *= 1.2f;
        }
    }
}
