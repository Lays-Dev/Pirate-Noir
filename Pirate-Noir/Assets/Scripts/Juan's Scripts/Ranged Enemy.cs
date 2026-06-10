using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemy : Enemy
{
    public Transform projectile;

    #region Attack Code
    /*public override IEnumerator AttackPlayer()
    {
        
    }*/



    #endregion

    #region Strafe Code
    public override IEnumerator StrafeLeft()
    {
        // in this version of the code, the enemy does two fast dashes in the direction of the strafe while shooting a projectile.
        agent.speed = speed2;

        for (int i = 0; i < 2; i++)
        {
            agent.ResetPath(); // stop the enemy from moving towards the player, this will probably change in later versions
            Vector3 enPosition = transform.position;
            Vector3 playerPosition = Player.position;
            
            var OffsetPlayer = enPosition - playerPosition; // get the direction from the enemy to the player
            var StrafeDirection = Vector3.Cross(OffsetPlayer, Vector3.up);

            float strafeDashTime = 0.3f; // determines how long the dash is.
            Debug.Log(StrafeDirection);

            while(strafeDashTime > 0)
            {
                agent.velocity = StrafeDirection.normalized * speed2; // set the velocity to the left of the player, this will probably change in later versions

                //Debug.Log(agent.velocity); // for testing purposes, to make sure the velocity is correct
            
                //look at player code
                lookPos = playerPosition - enPosition; // keeps the enemy looking at the player
                lookPos.y = 0; // we aren't trying to make the enemy go up
                rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                strafeDashTime -= Time.deltaTime;
                yield return null;
            }

            agent.velocity = Vector3.zero; // stop the enemy after the dash is done
            //AttackPlayer();
            yield return new WaitForSeconds(0.2f); 

        }


        IsDoingAction = false;
        ImmediateAction();
    }

    public override IEnumerator StrafeRight()
    {   

    
        agent.speed = speed2;
        for (int i = 0; i < 2; i++)
        {
            agent.ResetPath();
            Vector3 enPosition = transform.position;
            Vector3 playerPosition = Player.position;
            
            var OffsetPlayer = playerPosition - enPosition; // get the direction from the enemy to the player
            var StrafeDirection = Vector3.Cross(OffsetPlayer, Vector3.up);

            float strafeDashTime = 0.3f; // determines how long the dash is.
            Debug.Log(StrafeDirection);

            while(strafeDashTime > 0)
            {
                agent.velocity = StrafeDirection.normalized * speed2; // set the velocity to the left of the player, this will probably change in later versions

                //Debug.Log(agent.velocity); // for testing purposes, to make sure the velocity is correct
            
                //look at player code
                lookPos = playerPosition - enPosition; // keeps the enemy looking at the player
                lookPos.y = 0; // we aren't trying to make the enemy go up
                rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                strafeDashTime -= Time.deltaTime;
                yield return null;
            }

            agent.velocity = Vector3.zero; // stop the enemy after the dash is done
            //AttackPlayer();
            yield return new WaitForSeconds(0.2f); // wait for the next frame before continuing the loop

        }

        IsDoingAction = false;
        ImmediateAction();
    }










    #endregion
}
