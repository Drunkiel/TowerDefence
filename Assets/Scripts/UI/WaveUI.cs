using System.Collections;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public TMP_Text waveText;
    public TMP_Text phaseText;
    public TMP_Text mapNameText;
    public TMP_Text timerText;

    public IEnumerator StartTimer(float time)
    {
        for (float i = time; i >= 0; i--)
        {
            timerText.text = $"{i}s to next phase";
            yield return new WaitForSeconds(1);
        }
    }
}
