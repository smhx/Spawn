using System;

[System.Serializable]
public enum BrickTypes {
	Normal1 = 0,
    Normal2,
    SlowDown,
    Freeze,
    Shield,
    DoubleMoney,
    DoubleScore,
    ExtraLife,
    MultiBall,
    Bomb
};

[System.Serializable]
public enum AbilityTypes
{
    Null1 = 0,
    Null2,
    SlowDown,
    Freeze,
    Shield,
    DoubleMoney,
    DoubleScore,
    ExtraLife,
    Multiball,
    Bomb
};

[System.Serializable]
public enum PaddleTypes {
	White = 0,
    Red,
    Orange,
    Yellow,
    YellowGreen,
    Green,
    Blue,
    Purple,
    Pink,
    Black
}

[System.Serializable]
public enum BallTypes {
    White = 0,
    Red,
    Orange,
    Yellow,
    YellowGreen,
    Green,
    Blue,
    Purple,
    Pink,
    Black
}

[System.Serializable]
public enum BackgroundImageTypes {
    Grey = 0,
    Red,
    LightBrown,
    Beige,
    LimeGreen,
    Green,
    DarkGreen,
    LightTurquoise,
    DarkTurquoise,
    SkyBlue,
    NavyBlue,
    DarkPurple,
    Lavender,
    Pink,
    LightGrey,
    Black
}

[System.Serializable]
public enum BackgroundCircleTypes {
    GreyEmpty = 0,
    RedEmpty,
    LightBrownEmpty,
    BeigeEmpty,
    LimeGreenEmpty,
    GreenEmpty,
    DarkGreenEmpty,
    LightTurquoiseEmpty,
    DarkTurquoiseEmpty,
    SkyBlueEmpty,
    NavyBlueEmpty,
    DarkPurpleEmpty,
    LavenderEmpty,
    PinkEmpty,
    LightGreyEmpty,
    BlackEmpty,

    GreyFilled,
    RedFilled,
    LightBrownFilled,
    BeigeFilled,
    LimeGreenFilled,
    GreenFilled,
    DarkGreenFilled,
    LightTurquoiseFilled,
    DarkTurquoiseFilled,
    SkyBlueFilled,
    NavyBlueFilled,
    DarkPurpleFilled,
    LavenderFilled,
    PinkFilled,
    LightGreyFilled,
    BlackFilled
}

[System.Serializable]
public class Background {

    public BackgroundImageTypes image;

    public BackgroundCircleTypes circle;

    public Background(BackgroundImageTypes bit, BackgroundCircleTypes bct) {
        image = bit;
        circle = bct;
    }
}