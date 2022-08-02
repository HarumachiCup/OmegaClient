using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StateMover : MonoBehaviour
{
    public Vector2 PositionWhenIdle;
    public float OpacityWhenIdle = 1;
    public Vector2 PositionWhenPlay;
    public float OpacityWhenPlay = 1;
    public float range;
    public bool inState = false;
    public enum States
    {
        IdleToPlay = 0,
        AnyToMappool = 1,
    }
    public States StateToWatch;

    private void Update()
    {
        if (StateToWatch == States.IdleToPlay)
        {
            if (IPC.state != "Playing" && IPC.state != "WaitingForClients")
            {
                inState = false;
            }
            else
            {
                inState = true;
            }
        }
        else if(StateToWatch == States.AnyToMappool)
        {
            // what do we do if we add another screen and we're on neither play nor mappool? probably just hide all, but im not sure that's possible? will have to look into that at a later date
            if(Global.CurrentScreen == Global.Screens.Play)
            {
                inState = false;
            }
            else if (Global.CurrentScreen == Global.Screens.Mappool)
            {
                inState = true;
            }
        }

        if (!inState)
        {
            if(range > 0)
            {
                range -= (Time.deltaTime * 2);
            }
        }
        else
        {
            if(range < 1)
            {
                range += (Time.deltaTime * 2);
            }
        }
        range = Mathf.Clamp(range, 0, 1);
        if(this.GetComponent<CanvasGroup>() == null)
        {
            this.gameObject.AddComponent<CanvasGroup>();
        }
        CanvasGroup cg = this.GetComponent<CanvasGroup>();
        cg.alpha = hMath.Remap(range, 0, 1, OpacityWhenIdle, OpacityWhenPlay);
        this.transform.localPosition = Vector2.Lerp(PositionWhenIdle, PositionWhenPlay, LerpCurves.SoftestInSoftOut01(range));
    }
}

public static class hMath
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}


public static class LerpCurves
{
    public static float SofterInSofterOut01(float valueFrom0to1) { return (-Mathf.Cos(valueFrom0to1 * Mathf.PI)) * 0.5F + 0.5F; }    //    ( cos(x*pi)*0.5+0.5)
    public static float SofterInSofterOut10(float valueFrom0to1) { return 1F - (-Mathf.Cos(valueFrom0to1 * Mathf.PI)) * 0.5F + 0.5F; }    //   1-( cos(x*pi)*0.5+0.5)
    public static float SoftestInSoftOut01(float valueFrom0to1) { return Mathf.Pow((-Mathf.Cos(valueFrom0to1 * Mathf.PI)) * 0.5F + 0.5F, 2F); }    //     (-cos(x*pi)*0.5+0.5)^2
    public static float SoftestInSoftOut10(float valueFrom0to1) { return 1F - Mathf.Pow((-Mathf.Cos(valueFrom0to1 * Mathf.PI)) * 0.5F + 0.5F, 2F); }    //    1-(-cos(x*pi)*0.5+0.5)^2

    //pretty much the same as SofterInSofterOut
    public static float SmoothStep01(float valueFrom0to1) { return valueFrom0to1 * valueFrom0to1 * (3 - 2 * valueFrom0to1); }    //     (x*x * (3 - 2*x))
    public static float SmoothStep10(float valueFrom0to1) { return 1F - valueFrom0to1 * valueFrom0to1 * (3 - 2 * valueFrom0to1); }    //    1-(x*x * (3 - 2*x))

    //Symmetric, SoftestInSoftOut is not
    public static float SmootherStep01(float valueFrom0to1) { return valueFrom0to1 * valueFrom0to1 * valueFrom0to1 * (valueFrom0to1 * (6F * valueFrom0to1 - 15F) + 10F); }    //     x*x*x * (x* (6*x - 15) + 10)
    public static float SmootherStep10(float valueFrom0to1) { return 1F - valueFrom0to1 * valueFrom0to1 * valueFrom0to1 * (valueFrom0to1 * (6F * valueFrom0to1 - 15F) + 10F); }    //    1-x*x*x * (x* (6*x - 15) + 10)

    public static float Linear01(float valueFrom0to1) { return valueFrom0to1; }    //        x

    public static float SoftInHardOut01(float valueFrom0to1) { return 1 - Mathf.Pow(valueFrom0to1, 3); }    //        x^3
    public static float SoftInHardOut10(float valueFrom0to1) { return 1 - Mathf.Pow(valueFrom0to1, 3); }    //    1-   x^3
    public static float HardInSoftOut01(float valueFrom0to1) { return Mathf.Pow(1 - valueFrom0to1, 3); }    //     (1-x)^3
    public static float HardInSoftOut10(float valueFrom0to1) { return Mathf.Pow(1 - valueFrom0to1, 3); }    //    1-(1-x)^3
}