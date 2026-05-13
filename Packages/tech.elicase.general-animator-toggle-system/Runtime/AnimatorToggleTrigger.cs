
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimatorToggleTrigger : UdonSharpBehaviour
{
    public AnimatorToggleController[] controllers;

    // 进入碰撞体
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        // 只响应本地玩家
        if (!player.isLocal) return;

        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.Open();
            }
        }
    }

    // 离开碰撞体
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        // 只响应本地玩家
        if (!player.isLocal) return;

        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.Close();
            }
        }
    }
}
