using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagsVisibilityHandler : MonoBehaviour
{
    private Image[] flags;

    private void Awake()
    {
        WaveSystem.RequestFlagPositions += SetFlagPos;


        flags = GetComponentsInChildren<Image>();

        for (int i = 0; i < flags.Length; i++)
        {
            flags[i].enabled = false;
        }

        WaveSystem.RequestFlagStatus += SetupFlags;
    }

    private void SetupFlags(bool[] flagStatus)
    {
        for (int i = 0; i<flags.Length ; i++)
        {
            flags[i].enabled = flagStatus[i];
        }
    }

    void SetFlagPos(List<float> origPoses)
    {
        int x = 0;
        for (int i = 0; i < flags.Length; i++)
        {
            if(flags[i].enabled == true)
            {
                //konvertovat 0-1 na souradnice X
                //float pos = (origPoses[x] - (-2.93f)) / (3.36f - (-2.93f));
                float pos = (6.29f / 1) * origPoses[x] - 2.93f;
                flags[i].rectTransform.anchoredPosition = new Vector2(pos, flags[i].rectTransform.anchoredPosition.y);
                x++;
            }
        }
    }
}
