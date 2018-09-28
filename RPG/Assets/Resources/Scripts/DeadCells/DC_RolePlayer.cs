
using UnityEngine;

public class DC_RolePlayer : UIRoleBase
{
    
    public override void SetUIRole(int id, int line, int col, float pos_x, float pos_y)
    {
        base.SetUIRole(id, line, col, pos_x, pos_y);
    }

    public RoleState GetRoleState()
    {
        return m_state;
    }

    public void SetRoleState(RoleState state)
    {
        m_state = state;
    }

    private MapTile next_tile;

    void StartMove()
    {
        next_tile = null;
    }

    public override void StopMove()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, pos_y, 0);
        m_state = RoleState.None;
        animator.Play("idle");
    }

    public override void WalkOneTile(int line, int col)
    {
        this.line += line;
        this.col += col;
        pos_x += col * DC_GameManager.m_instance.tileWidth;
        pos_y -= line * DC_GameManager.m_instance.tileWidth;
    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }

    public override bool CanClimb(Direction dir)
    {

        StartMove();
        switch (dir)
        {
            case Direction.Up:
                next_tile = DC_GameManager.m_instance.GetTile(line - 1, col);
                if (DC_GameManager.m_instance.GetTile(line, col).id == 0)  //在梯子上
                {
                    if (m_state != RoleState.Jumping)
                    {
                        m_state = RoleState.Climbing;
                        animator.Play("walk_up");
                    }
                    return (next_tile.id != 2 || transform.localPosition.y < pos_y);
                }
                else  //尝试往梯子上爬
                {
                    return (next_tile.id == 0 || transform.localPosition.y < pos_y);
                }
            case Direction.Down:
                next_tile = DC_GameManager.m_instance.GetTile(line + 1, col);
                if (DC_GameManager.m_instance.GetTile(line, col).id == 0)  //在梯子上
                {
                    if (m_state != RoleState.Jumping)
                    {
                        m_state = RoleState.Climbing;
                        animator.Play("walk_up");
                    }
                    return (next_tile.id != 2 || transform.localPosition.y > pos_y);
                }
                else  //尝试往梯子上爬
                {
                    return (next_tile.id == 0 || transform.localPosition.y > pos_y);
                }
            case Direction.Left:
                next_tile = DC_GameManager.m_instance.GetTile(line, col - 1);
                return (next_tile.id == 0 || transform.localPosition.x > pos_x);
            case Direction.Right:
                next_tile = DC_GameManager.m_instance.GetTile(line, col + 1);
                return (next_tile.id == 0 || transform.localPosition.x < pos_x);
            default:
                return false;
        }
    }

    public override bool CanWalk(Direction dir)
    {
        
        StartMove();

        switch (dir)
        {
            case Direction.Up:
                next_tile = DC_GameManager.m_instance.GetTile(line - 1, col);
                return (next_tile.id == 1 || transform.localPosition.y < pos_y);
            case Direction.Down:
                next_tile = DC_GameManager.m_instance.GetTile(line + 1, col);
                return (next_tile.id == 1 || transform.localPosition.y > pos_y);
            case Direction.Left:
                if (m_state != RoleState.Jumping)
                    m_state = RoleState.Walking;
                animator.Play("walk_left");
                next_tile = DC_GameManager.m_instance.GetTile(line, col - 1);
                return (next_tile.id == 1 || transform.localPosition.x > pos_x);
            case Direction.Right:
                if (m_state != RoleState.Jumping)
                    m_state = RoleState.Walking;
                animator.Play("walk_right");
                next_tile = DC_GameManager.m_instance.GetTile(line, col + 1);
                return (next_tile.id == 1 || transform.localPosition.x < pos_x);
            default:
                return false;
        }

    }
}
