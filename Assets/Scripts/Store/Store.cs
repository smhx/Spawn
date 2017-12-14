using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Store : Setting<PlayerItems> {
	public static Dictionary<BrickTypes, Brick> bricks;
	public static Dictionary<PaddleTypes, Paddle> paddles;
    public static Dictionary<BallTypes, Ball> balls;
    public static Dictionary<BackgroundImageTypes, BackgroundImage> backgroundImages;
    public static Dictionary<BackgroundCircleTypes, BackgroundCircle> backgroundCircles;
    public static Dictionary<AbilityTypes, Ability> abilities;

	[SerializeField] List<Brick> brickList;
	[SerializeField] List<Paddle> paddleList;
	[SerializeField] List<Ball> ballList;
	[SerializeField] List<BackgroundImage> biList;
	[SerializeField] List<BackgroundCircle> bcList;
	[SerializeField] List<Ability> abilityList;

    void Awake() {
//		ResetAllData ();
        bricks = new Dictionary<BrickTypes, Brick> ();
		paddles = new Dictionary<PaddleTypes, Paddle> ();
        balls = new Dictionary<BallTypes, Ball>();
        backgroundImages = new Dictionary<BackgroundImageTypes, BackgroundImage>();
        backgroundCircles = new Dictionary<BackgroundCircleTypes, BackgroundCircle>();
        abilities = new Dictionary<AbilityTypes, Ability>();

        foreach (Brick brick in brickList) {
			bricks.Add(brick.brickType, brick);
		}
		foreach (Paddle paddle in paddleList) {
			paddles.Add(paddle.paddleType, paddle);
		}
        foreach (Ball ball in ballList) {
			balls.Add(ball.ballType, ball);
		}
        foreach (BackgroundImage bi in biList) {
            backgroundImages.Add(bi.backgroundImageType, bi);
        }
        foreach (BackgroundCircle bc in bcList) {
            backgroundCircles.Add(bc.backgroundCircleType, bc);
        }
        foreach (Ability ability in abilityList) {
            abilities.Add(ability.type, ability);
        }
    }

	public List<BrickTypes> GetActiveBricks() {
		List<BrickTypes> bts = new List<BrickTypes> ();
		foreach (Brick b in brickList) {
            if (AbilityLevel(b.brickAbility) > 0)
			    bts.Add(b.brickType);
		}
		return bts;
	}

	public Paddle GetActivePaddle() {
		return paddles[data.currentPaddle];
	}
    
    public Ball GetActiveBall() {
		return balls[data.currentBall];
	}

    public Background GetActiveBackground() {
        return data.currentBackground;
    }

	public void IncrementMoneyBy(int m) {
        data.money += m;
//        data.money = 1000; //Use this to manually set money
        SendDataChangedEvent ();
        Save ();
	}

	public bool CanBuy(PaddleTypes pt) {
		if (IsUnlocked (pt))
			return false;
		Paddle p =  paddles[pt];
		if (data.money >= p.price)
			return true;
		return false;
	}

	public bool CanBuy(BallTypes bt) {
		if (IsUnlocked (bt))
			return false;
		Ball b = balls[bt];
		if (data.money >= b.price)
			return true;
		return false;
	}

    public bool CanBuy(BackgroundImageTypes bit) {
        if (IsUnlocked(bit))
            return false;
		BackgroundImage bi = backgroundImages[bit];
        if (data.money >= bi.price)
            return true;
		return false;
	}

    public bool CanBuy(Background b) {
        if (IsUnlocked(b))
            return false;
        if (IsUnlocked(b.image) && data.money >= backgroundCircles[b.circle].price)
            return true;
		return false;
	}

	public void Buy(PaddleTypes pt) {
		if (!CanBuy(pt)) {
			return;
		}
		int price = paddles [pt].price;
		data.money -= price;
		data.unlockedPaddles.Add (pt);
		data.currentPaddle = pt;
		SendDataChangedEvent ();
		Save ();
	}
    
	public void Buy(BallTypes bt) {
		if (!CanBuy(bt)) {
			return;
		}
		int price = balls[bt].price;
		data.money -= price;
		data.unlockedBalls.Add(bt);
		data.currentBall = bt;
		SendDataChangedEvent ();
		Save ();
    }

    public void Buy(BackgroundImageTypes bit) {
		if (!CanBuy(bit)) {
			return;
		}
		int price = backgroundImages[bit].price;
		data.money -= price;
		data.unlockedBackgroundImages.Add(bit);
        SendDataChangedEvent ();
		Save ();
	}

    public void Buy(Background b) {
        if (!CanBuy(b)) {
			Debug.LogWarning("ERROR: you don't have enough to buy in Store::Buy");
			return;
		}
        int price = backgroundCircles[b.circle].price;
        data.money -= price;
        data.unlockedBackgrounds.Add(b);
        data.currentBackground = b;
        SendDataChangedEvent();
        Save();
    }

	public bool IsActive(PaddleTypes pt) {
		return data.currentPaddle == pt;
	}
    
	public bool IsActive(BallTypes bt) {
		return data.currentBall == bt;
	}

    public bool IsActive(Background b) {
        return data.currentBackground.image == b.image && data.currentBackground.circle == b.circle;
    }

	public void SetActive(PaddleTypes pt) {
		data.currentPaddle = pt;
		SendDataChangedEvent ();
		Save ();
	}

    public void SetActive(BallTypes bt) {
		data.currentBall = bt;
		SendDataChangedEvent ();
		Save ();
	}

    public void SetActive(Background b) {
        data.currentBackground = b;
        SendDataChangedEvent();
        Save();
    }

	public bool IsUnlocked(BrickTypes bt) {
		return data.unlockedBricks.Contains (bt);
	}

	public bool IsUnlocked(PaddleTypes pt) {
		return data.unlockedPaddles.Contains (pt);
	}
    
	public bool IsUnlocked(BallTypes bt) {
		return data.unlockedBalls.Contains (bt);
	}

    public bool IsUnlocked(BackgroundImageTypes bit) {
        return data.unlockedBackgroundImages.Contains (bit);
	}

    public bool IsUnlocked(Background b) {
        return data.unlockedBackgrounds.Any(n => n.image == b.image && n.circle == b.circle);
	}

    //ability functions
    public int AbilityLevel(AbilityTypes at) { return data.abilityLevels[at]; }

    public bool CanUpgrade(AbilityTypes at) {
        int level = AbilityLevel(at);
        if (level == abilities[at].maxLevel)
            return false;
        int price = abilities[at].cost[level + 1];
        if (data.money >= price)
            return true;
        else
            return false;
    }

    public void UpgradeAbility(AbilityTypes at) {
        if (!CanUpgrade(at)) {
			Debug.LogWarning("Store::UpgradeAbility: You don't have enough money to upgrade ability "+at.ToString ());
            return;
        }
        int level = AbilityLevel(at);
        int price = abilities[at].cost[level + 1];
        ++data.abilityLevels[at];
        data.money -= price;
        SendDataChangedEvent();
        Save();
    }
}