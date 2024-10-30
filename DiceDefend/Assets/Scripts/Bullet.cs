using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }
    public void Fire(EnumConst.BulletDirection direction, float bulletSpeed)
    {
        var rb = this.GetComponent<Rigidbody2D>();
        switch (direction) 
        {
            case EnumConst.BulletDirection.Left:
                break;
            case EnumConst.BulletDirection.Right:
                this.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case EnumConst.BulletDirection.Up:
                this.transform.eulerAngles = new Vector3(0, 0, -90);
                break;
            case EnumConst.BulletDirection.Down:
                this.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
        }
        rb.velocity = GetDirectionVector(direction) * bulletSpeed;
    }

    Vector2 GetDirectionVector(EnumConst.BulletDirection direction)
    {
        return direction switch
        {
            EnumConst.BulletDirection.Left => Vector2.left,
            EnumConst.BulletDirection.Right => Vector2.right,
            EnumConst.BulletDirection.Up => Vector2.up,
            EnumConst.BulletDirection.Down => Vector2.down,
            _ => Vector2.right
        };
    }
}
