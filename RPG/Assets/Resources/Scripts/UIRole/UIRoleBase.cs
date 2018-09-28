using UnityEngine;
using UnityEngine.UI;

public class UIRoleBase : MonoBehaviour {
    protected int roleID;

    protected Direction m_dir;
    public RoleState m_state;

    protected Animator animator;
    protected Image img;
    public int line, col;
    protected float pos_x, pos_y;
    public virtual void SetUIRole(int id, int line, int col, float pos_x, float pos_y)
    {
        gameObject.name = "role_" + id;
        roleID = id;
        this.line = line;
        this.col = col;
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        img = transform.Find("img").GetComponent<Image>();
        animator = transform.GetComponent<Animator>();
        m_dir = Direction.None;
        m_state = RoleState.None;
    }

    public virtual void StopMove() { }

    public virtual bool CanClimb(Direction dir)
    {
        return false;
    }


    public virtual bool CanWalk(Direction dir)
    {
        return false;
    }

    public virtual void WalkOneTile(int line, int col)
    {
        this.line += line;
        this.col += col;
        pos_x += col * GameSceneManager.m_instance.tileWidth;
        pos_y -= line * GameSceneManager.m_instance.tileWidth;
    }

    public float GetRoleTilePosX()
    {
        return pos_x;
    }
    public float GetRoleTilePosY()
    {
        return pos_y;
    }
}

public enum RoleState
{
    None = 0,
    Walking = 1,
    Jumping = 2,
    Climbing = 3,
}
