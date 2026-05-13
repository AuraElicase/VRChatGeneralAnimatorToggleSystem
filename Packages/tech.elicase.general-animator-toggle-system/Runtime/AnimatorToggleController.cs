using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimatorToggleController : UdonSharpBehaviour
{
    public Animator[] animators;
    public string param = "IsOpen";

    // 是否全局同步
    public bool isGlobal = true;

    public float triggerCoolDownTime = 0.5f;

    [UdonSynced]
    private bool isOpenSynced = false;

    // 本地运行状态
    private bool isOpen = false;

    private float lastToggleTime;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            RequestSerialization();
        }
    }

    private bool CanTrigger()
    {
        // 防抖
        if (Time.time - lastToggleTime < triggerCoolDownTime)
            return false;

        lastToggleTime = Time.time;
        return true;
    }

    private void EnsureOwnership()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }

    public void Open()
    {
        if (!CanTrigger()) return;

        if (isOpen) return;

        SetState(true);
    }

    public void Close()
    {
        if (!CanTrigger()) return;

        if (!isOpen) return;

        SetState(false);
    }

    public void Toggle()
    {
        if (!CanTrigger()) return;

        SetState(!isOpen);
    }

    private void SetState(bool state)
    {
        isOpen = state;

        // Global 模式
        if (isGlobal)
        {
            EnsureOwnership();

            isOpenSynced = state;

            RequestSerialization();
        }

        ApplyState();
    }

    public override void OnDeserialization()
    {
        // 只有 global 才接受网络同步
        if (!isGlobal) return;

        isOpen = isOpenSynced;

        ApplyState();
    }

    private void Start()
    {
        // 初始同步状态
        if (isGlobal)
        {
            isOpen = isOpenSynced;
        }

        ApplyState();
    }

    private void ApplyState()
    {
        foreach (Animator anim in animators)
        {
            if (anim != null)
            {
                anim.SetBool(param, isOpen);
            }
        }
    }
}