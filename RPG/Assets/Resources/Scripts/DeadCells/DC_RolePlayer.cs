
using UnityEngine;

public class DC_RolePlayer : UIRoleBase
{

    private Direction last_dir;
    public override void SetUIRole(int id, int line, int col, float pos_x, float pos_y)
    {
        base.SetUIRole(id, line, col, pos_x, pos_y);
        last_dir = Direction.None;
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
        m_state = RoleState.None;
        m_dir = Direction.None;
        animator.Play("idle");
    }

    public override void WalkOneTile(int line, int col)
    {
        this.line += line;
        this.col += col;
        pos_x += col * DC_GameManager.m_instance.tileWidth;
        pos_y -= line * DC_GameManager.m_instance.tileWidth;
    }

    public override bool CanWalk(Direction dir)
    {
        
        StartMove();

        switch (dir)
        {
            case Direction.Up:
                next_tile = DC_GameManager.m_instance.GetTile(line - 1, col);
                return ((next_tile.id == 1 && next_tile.npc == 0) || transform.localPosition.y < pos_y);
            case Direction.Down:
                next_tile = DC_GameManager.m_instance.GetTile(line + 1, col);
                return ((next_tile.id == 1 && next_tile.npc == 0) || transform.localPosition.y > pos_y);
            case Direction.Left:
                if (m_state != RoleState.Jumping)
                    m_state = RoleState.Walking;
                last_dir = Direction.Left;
                //if (m_dir != Direction.Left)
                //{
                    m_dir = Direction.Left;
                    animator.Play("walk_left");
                //}
                next_tile = DC_GameManager.m_instance.GetTile(line, col - 1);
                return ((next_tile.id == 1) || transform.localPosition.x > pos_x);
            case Direction.Right:
                if (m_state != RoleState.Jumping)
                    m_state = RoleState.Walking;
                last_dir = Direction.Right;
                //if (m_dir != Direction.Right)
                //{
                    m_dir = Direction.Right;
                    animator.Play("walk_right");
                //}
                next_tile = DC_GameManager.m_instance.GetTile(line, col + 1);
                return ((next_tile.id == 1) || transform.localPosition.x < pos_x);
            default:
                return false;
        }

    }
}
