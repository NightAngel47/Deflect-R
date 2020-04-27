using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour
{
    public Image startArrow;
    public Image helpArrow;
    public Image credArrow;
    public Image quitArrow;
    public Image holder;


    public void startReveal()
    {
        holder = startArrow;
        holder.enabled = true;
    }

    public void helpReveal()
    {
        holder = helpArrow;
        holder.enabled = true;
    }

    public void credReveal()
    {
        holder = credArrow;
        holder.enabled = true;
    }

    public void quitReveal()
    {
        holder = quitArrow;
        holder.enabled = true;
    }

    public void hide()
    {
        holder.enabled = false;
    }
}
