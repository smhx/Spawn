using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/FreezeAbility")]
public class FreezeAbility : Ability
{
    BrickSpawner bs;
    GameSettings gs;
    Timer timer;

    protected override void SetUp()
    {
        bs = FindObjectOfType<BrickSpawner>();
        gs = GameObject.FindWithTag("GameSettings").GetComponent<GameSettings>();
		if (gs.currentGameMode == GameModes.TIMED) {
			timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
			if (timer == null)
				Debug.LogError ("FreezeAbility::SetUp timer is null");
//			timer.Freeze();
		}
    }

    public override void ReTriggerAbility() {}


    public override void TriggerAbility()
    {
        bs.Freeze();
		foreach (var brick in ObjectPooler.SharedInstance.GetAllActiveGameObjects ("Brick"))
            brick.GetComponent<BrickMovement>().Freeze();
        if (gs.currentGameMode == GameModes.TIMED) 
            timer.Freeze();
    }

    public override void EndAbility()
    {
        bs.Unfreeze();
		foreach (var brick in ObjectPooler.SharedInstance.GetAllActiveGameObjects ("Brick") )
            brick.GetComponent<BrickMovement>().Unfreeze();
        if (gs.currentGameMode == GameModes.TIMED)
            timer.Unfreeze();
    }
}
