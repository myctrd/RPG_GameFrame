using UnityEngine.UI;
using UnityEngine;

public class UIRolePlayer : UIRoleBase
{
    private GameObject t_interaction;
    private Direction last_dir;
    public override void SetUIRole(int id, int line, int col, float pos_x, float pos_y)
    {
        base.SetUIRole(id, line, col, pos_x, pos_y);
        t_interaction = transform.Find("interaction").gameObject;
        t_interaction.SetActive(false);
        last_dir = Direction.None;
    }

    public override void StopMove()
    {
        m_state = RoleState.None;
        m_dir = Direction.None;
        animator.Play("idle");
        t_interaction.SetActive(next_tile != null && next_tile.npc != 0);
        PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_CurrentCol", col);
        PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_CurrentLine", line);
    }

    void StartMove()
    {
        next_tile = null;
        t_interaction.SetActive(false);
        m_state = RoleState.Walking;
    }

    public int GetInteractiveNPC()
    {
        if(next_tile == null)
        {
            return 0;
        }
        return next_tile.npc;
    }

    private MapTile next_tile;

    public override bool CanWalk(Direction dir)
    {
        if (m_state != RoleState.None && m_state != RoleState.Walking)
        {
            return false;
        }
        StartMove();

        switch (dir)
        {
            case Direction.Up:
                if (m_dir != Direction.None && m_dir != Direction.Up)
                {
                    return false;
                }
                last_dir = Direction.Up;
                if (m_dir != Direction.Up)
                {
                    m_dir = Direction.Up;
                    animator.Play("walk_up");
                }
                next_tile = GameSceneManager.m_instance.GetTile(line - 1, col);
                return ((next_tile.canWalk == 1 && next_tile.npc == 0) || transform.localPosition.y < pos_y);
            case Direction.Down:
                if (m_dir != Direction.None && m_dir != Direction.Down)
                {
                    return false;
                }
                last_dir = Direction.Down;
                if (m_dir != Direction.Down)
                {
                    m_dir = Direction.Down;
                    animator.Play("walk_down");
                }
                next_tile = GameSceneManager.m_instance.GetTile(line + 1, col);
                return ((next_tile.canWalk == 1 && next_tile.npc == 0) || transform.localPosition.y > pos_y);
            case Direction.Left:
                if (m_dir != Direction.None && m_dir != Direction.Left)
                {
                    return false;
                }
                last_dir = Direction.Left;
                if (m_dir != Direction.Left)
                {
                    m_dir = Direction.Left;
                    animator.Play("walk_left");
                }
                next_tile = GameSceneManager.m_instance.GetTile(line, col - 1);
                return ((next_tile.canWalk == 1 && next_tile.npc == 0) || transform.localPosition.x > pos_x);
            case Direction.Right:
                if (m_dir != Direction.None && m_dir != Direction.Right)
                {
                    return false;
                }
                last_dir = Direction.Right;
                if (m_dir != Direction.Right)
                {
                    m_dir = Direction.Right;
                    animator.Play("walk_right");
                }
                next_tile = GameSceneManager.m_instance.GetTile(line, col + 1);
                return ((next_tile.canWalk == 1 && next_tile.npc == 0) || transform.localPosition.x < pos_x);
            default:
                return false;
        }

    }
}
