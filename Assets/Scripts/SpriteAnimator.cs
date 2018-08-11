using UnityEngine;

public class SpriteAnimator : MonoBehaviour {
    public Sprite[] frames;
    public int frame;

    public bool playing = true;

    public float frameRate = 60;

    public AnimationRestartMode mode;

    private bool isLoopingBack = false;
    private float frameTime = 0F;

    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();

        Reload();
    }

    public void Update()
    {
        if (!playing)
            return;

        float spf = 1 / frameRate;

        frameTime += Time.deltaTime;

        if (frameTime > spf)
        {
            frameTime = 0;
            AdvanceAnimation();
        }
    }

    private void AdvanceAnimation()
    {
        if (mode == AnimationRestartMode.LOOPBACK)
        {
            if (isLoopingBack)
            {
                frame--;
                if(frame == 0)
                {
                    isLoopingBack = false;
                }
            }
            else
            {
                frame++;
                if (frame >= frames.Length)
                {
                    isLoopingBack = true;
                    frame--;
                }
            }
        }
        else
        {
            frame++;
            if (frame >= frames.Length)
                if (mode == AnimationRestartMode.STOP)
                    playing = false;
                else
                    frame = 0;
        }
        Reload();
    }

    private void Reload()
    {
        render.sprite = frames[frame];
    }

    public enum AnimationRestartMode
    {
        RESET,
        LOOPBACK,
        STOP
    }
}
