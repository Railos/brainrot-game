using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D rb { get; set; }

    bool isFacingRight { get; set; }

    void MoveEnemy(Vector2 velocity);

    void CheckForLeftOrRightFacing(Vector2 velocity);
}
