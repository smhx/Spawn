using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public AbilityTypes type;
    
	public Sprite sprite;

    public int maxLevel;

    //these lists should be length maxLevel+1

    public List<int> cost; //cost[i] is the money needed to upgrade from level i-1 to i

    public List<int> probabilityWeight;

    public List<float> duration;

    public List<string> description; //description[i] describes the upgrade from level i to i+1, or "Fully Upgraded!" if i = maxLevel
    
    Store store;
    GameSceneManager gameManager;

    
    public void Initialize()
    {
        if (store == null)
            store = GameObject.FindWithTag("Store").GetComponent<Store>();
        if (gameManager == null)
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameSceneManager>();
        SetUp();
    }
		
    protected abstract void SetUp ();

	public abstract void TriggerAbility ();

	public abstract void EndAbility ();

	public abstract void ReTriggerAbility ();

    public int level {
        get {
            if (store == null)
                store = GameObject.FindWithTag("Store").GetComponent<Store>();
            return store.AbilityLevel(type);
        }
    }
}

