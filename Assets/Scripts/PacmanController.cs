using UnityEngine;

public class PacmanController : MonoBehaviour {
    public Direction direction = Direction.NONE;

    public SpriteAnimator aliveAnimator;
    public SpriteAnimator deadAnimator;

    public float speed = 1.5F;

    private Direction directionBuffer = Direction.NONE;

    private void Awake()
    {
        if(!aliveAnimator)
        {
            Debug.LogError("No alive animator set");
            Destroy(this);
        }
    }

    private void Update()
    {
        if (true && direction != Direction.NONE) // TODO: if alive
            aliveAnimator.playing = true;

        if (Input.GetKey(KeyCode.UpArrow))
            directionBuffer = Direction.UP;
        if (Input.GetKey(KeyCode.DownArrow))
            directionBuffer = Direction.DOWN;
        if (Input.GetKey(KeyCode.LeftArrow))
            directionBuffer = Direction.LEFT;
        if (Input.GetKey(KeyCode.RightArrow))
            directionBuffer = Direction.RIGHT;

        if (CanTurn()) {
            direction = directionBuffer;
        }

        if (direction == Direction.NONE)
            return;

        transform.eulerAngles = new Vector3(0, 0, (int)direction);

        if (CanMove())
            transform.Translate(Vector2.right * Time.deltaTime * speed);
    }

    private bool CanTurn()
    {
        if (directionBuffer == Direction.NONE)
            return false;

        Ray2D ray = CreateRay(doOriginShift: CanMove());
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta);
        return !Physics2D.Raycast(ray.origin, ray.direction, ray.origin == (Vector2)transform.position ? .26F : .51F);
    }

    private bool CanMove()
    {
        if (direction == Direction.NONE)
            return false;

        Ray2D ray = CreateRay(direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        return !Physics2D.Raycast(ray.origin, ray.direction, ray.origin == (Vector2)transform.position ? .26F : .49F);
    }

    private Ray2D CreateRay(Direction dirOverride = Direction.NONE, bool doOriginShift = true)
    {
        Direction dirTo = dirOverride == Direction.NONE ? directionBuffer : dirOverride;

        if (dirTo == Direction.NONE)
            return new Ray2D();

        Vector2 origin = Vector2.zero;
        Vector2 vecDir = Vector2.zero;

        if (doOriginShift)
            switch (direction)
            {
                case Direction.UP:
                    origin = (Vector2)transform.position + new Vector2(0F, -.22F);
                    break;
                case Direction.DOWN:
                    origin = (Vector2)transform.position + new Vector2(0F, .22F);
                    break;
                case Direction.LEFT:
                    origin = (Vector2)transform.position + new Vector2(.22F, 0F);
                    break;
                case Direction.RIGHT:
                    origin = (Vector2)transform.position + new Vector2(-.22F, 0F);
                    break;
                default:
                    origin = transform.position;
                    break;
            }
        else
            origin = transform.position;

        switch (dirTo)
        {
            case Direction.UP:
                vecDir = Vector2.up;
                break;
            case Direction.DOWN:
                vecDir = Vector2.down;
                break;
            case Direction.LEFT:
                vecDir = Vector2.left;
                break;
            case Direction.RIGHT:
                vecDir = Vector2.right;
                break;
            default:
                return new Ray2D();
        }

        return new Ray2D(origin, vecDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dot") || collision.CompareTag("bigDot"))
            Destroy(collision.gameObject);
    }

    public enum Direction
    {
        UP = 90,
        DOWN = -90,
        LEFT = -180,
        RIGHT = 0,
        NONE = -1337
    }
}
