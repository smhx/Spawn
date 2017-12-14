using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerItems {

	public int money = 0;

	public List<BrickTypes> unlockedBricks;
	public List<PaddleTypes> unlockedPaddles;
    public List<BallTypes> unlockedBalls;
    public List<BackgroundImageTypes> unlockedBackgroundImages;
    public List<Background> unlockedBackgrounds;

    public PaddleTypes currentPaddle;
    public BallTypes currentBall;
    public Background currentBackground;

    public Dictionary<AbilityTypes, int> abilityLevels;

    public PlayerItems() {
		money = 0;

        abilityLevels = new Dictionary<AbilityTypes, int> {
            { AbilityTypes.Null1, 1 },
            { AbilityTypes.Null2, 1 },
            { AbilityTypes.SlowDown, 0 },
            { AbilityTypes.Freeze, 0 },
            { AbilityTypes.Shield, 0 },
            { AbilityTypes.DoubleMoney, 0 },
            { AbilityTypes.DoubleScore, 0 },
            { AbilityTypes.ExtraLife, 0 },
            { AbilityTypes.Multiball, 0 },
            { AbilityTypes.Bomb, 0 }
        }; //level=0 means that it hasn't been bought in the store yet

		unlockedBricks = new List<BrickTypes> {
			BrickTypes.Normal1,
            BrickTypes.Normal2
		};

        unlockedPaddles = new List<PaddleTypes> {
			PaddleTypes.Green
		};

		currentPaddle = PaddleTypes.Green;

        unlockedBalls = new List<BallTypes> {
            BallTypes.White
        };

        currentBall = BallTypes.White;

        unlockedBackgroundImages = new List<BackgroundImageTypes> {
            BackgroundImageTypes.Grey
        };

		currentBackground = new Background(BackgroundImageTypes.Grey, BackgroundCircleTypes.SkyBlueEmpty);

        unlockedBackgrounds = new List<Background>() { currentBackground };
    }
}