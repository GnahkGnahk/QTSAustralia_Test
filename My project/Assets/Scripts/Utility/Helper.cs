using UnityEngine;

public class Helper {
    //====================================================================== TAG
    public const string TAG_OBSTACLE = "Obstacle";
    public const string TAG_PLAYER = "Player";


    //====================================================================== LAYER
    public const int LAYER_PLATFORM = 9;



    //====================================================================== ANIMATOR
    public const string ANIMATOR_VELOCITY = "Velocity";

    public const string ANIMATOR_JUMP_FROM_IDLE = "JumpFromIdle";
    public const string ANIMATOR_JUMP_FROM_MOVING = "JumpFromMoving";

    public const string ANIMATOR_IDLE = "Idle";
    public const string ANIMATOR_STANDARD_WALK = "StandardWalk";
    public const string ANIMATOR_RUN = "Run";

    public const string ANIMATOR_FALL_FLAT = "FallFlat";
}

public enum CameraType {
    TOP_DOWN,
    TOP_DOWN2,
}
public enum ObstacleType {
    A_TYPE,
    B_TYPE,
}
