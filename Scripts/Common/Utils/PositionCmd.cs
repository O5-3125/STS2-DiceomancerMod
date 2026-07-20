using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Diceomancer.Scripts.Common.Utils;

public static class PositionCmd
{
    public static async Task MoveForward(Creature creature, float distance)
    {
        Tween positionTween = null;

        // 计算该生物需要移动的距离
        // const float moveDistance = 100;

        // 获取该生物的节点
        NCreature currentCreatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
        if (currentCreatureNode != null)
        {
            // 第一次创建Tween对象
            positionTween ??= NCombatRoom.Instance.CreateTween()
                .SetParallel()
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);

            // 在0.25秒内平滑移动到新位置
            positionTween.TweenProperty(
                currentCreatureNode,
                "global_position:x",
                currentCreatureNode.GlobalPosition.X + distance,
                0.25
            );
        }

        // 如果有任何生物需要移动，等待动画完成
        if (positionTween != null)
        {
            await positionTween.AwaitFinished(NCombatRoom.Instance);
        }
    }


    public static async Task MoveBackward(Creature creature, float distance)
    {
        Tween positionTween = null;

        // 计算该生物需要移动的距离
        // const float moveDistance = 100;

        // 获取该生物的节点
        NCreature currentCreatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
        if (currentCreatureNode != null)
        {
            // 第一次创建Tween对象
            positionTween ??= NCombatRoom.Instance.CreateTween()
                .SetParallel()
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);

            // 在0.25秒内平滑移动到新位置
            positionTween.TweenProperty(
                currentCreatureNode,
                "global_position:x",
                currentCreatureNode.GlobalPosition.X - distance,
                0.25
            );
        }

        // 如果有任何生物需要移动，等待动画完成
        if (positionTween != null)
        {
            await positionTween.AwaitFinished(NCombatRoom.Instance);
        }
    }


    public static async Task MoveUp(Creature creature, float distance)
    {
        Tween positionTween = null;

        // 计算该生物需要移动的距离
        // const float moveDistance = 100;

        // 获取该生物的节点
        NCreature currentCreatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
        if (currentCreatureNode != null)
        {
            // 第一次创建Tween对象
            positionTween ??= NCombatRoom.Instance.CreateTween()
                .SetParallel()
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);

            // 在0.25秒内平滑移动到新位置
            positionTween.TweenProperty(
                currentCreatureNode,
                "global_position:y",
                currentCreatureNode.GlobalPosition.Y - distance,
                0.25
            );
        }

        // 如果有任何生物需要移动，等待动画完成
        if (positionTween != null)
        {
            await positionTween.AwaitFinished(NCombatRoom.Instance);
        }
    }

    public static async Task MoveDown(Creature creature, float distance)
    {
        Tween positionTween = null;

        // 计算该生物需要移动的距离
        // const float moveDistance = 100;

        // 获取该生物的节点
        NCreature currentCreatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
        if (currentCreatureNode != null)
        {
            // 第一次创建Tween对象
            positionTween ??= NCombatRoom.Instance.CreateTween()
                .SetParallel()
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);

            // 在0.25秒内平滑移动到新位置
            positionTween.TweenProperty(
                currentCreatureNode,
                "global_position:y",
                currentCreatureNode.GlobalPosition.Y + distance,
                0.25
            );
        }

        // 如果有任何生物需要移动，等待动画完成
        if (positionTween != null)
        {
            await positionTween.AwaitFinished(NCombatRoom.Instance);
        }
    }
}