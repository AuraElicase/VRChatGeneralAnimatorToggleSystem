
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimatorToggleButton : UdonSharpBehaviour
{
    public AnimatorToggleController[] controllers;
    public float delay = 10f;
    public bool enableDelayClose = false;

    // for Custom Button
    public override void Interact()
    {
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                if (enableDelayClose)
                {
                    controller.Open();
                    SendCustomEventDelayedSeconds(nameof(DelayedToggle), delay);
                    return;
                }
                controller.Toggle();
            }
        }
    }

    // for LuraSwitch
    public void ChangeStat()
    {
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                if (enableDelayClose)
                {
                    controller.Open();
                    SendCustomEventDelayedSeconds(nameof(DelayedToggle), delay);
                    return;
                }
                controller.Toggle();
            }
        }
    }

    public void DelayedToggle()
    {
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.Close();
            }
        }
    }
}
