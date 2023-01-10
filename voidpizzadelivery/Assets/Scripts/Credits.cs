using System.Collections;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    private TextMeshProUGUI credits;
    void Awake()
    {
        credits = GetComponent<TextMeshProUGUI>();
        StartCoroutine(RollCredits());
    }
    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(6f);

        credits.fontSize = 28f;
        credits.text = "Developed by: YAJI";

        yield return new WaitForSeconds(3f);

        credits.text = "";
        yield return new WaitForSeconds(0.5f);

        credits.text = "Sprites by: YAJI";
        
        yield return new WaitForSeconds(3f);

        credits.text = "";

        yield return new WaitForSeconds(0.5f);

        credits.fontSize = 20f;

        credits.text = "\"Sewer Soundscape, A.wav\" by InspectorJ (www.jshaw.co.uk) of Freesound.org";

        yield return new WaitForSeconds(3f);

        credits.text = "";

        yield return new WaitForSeconds(0.5f);

        credits.text = "\"MUDDY AND WET FOOTSTEPS.wav\" by theneedle.tv of Freesound.org";

        yield return new WaitForSeconds(3f);

        credits.text = "";

        yield return new WaitForSeconds(0.5f);

        credits.text = "\"Ripping Apart Carcass.wav\" by ProductionNow of Freesound.org";

        yield return new WaitForSeconds(3f);

        credits.text = "";

        yield return new WaitForSeconds(0.5f);

        credits.fontSize = 26f;
        credits.text = "The rest of the SFX regrettably made by: YAJI";

        yield return new WaitForSeconds(4f);

        credits.text = "";

        yield return new WaitForSeconds(0.5f);

        credits.fontSize = 28f;
        credits.text = "THANK YOU FOR PLAYING!!";

        yield return new WaitForSeconds(3f);
        
        credits.text = "";
        Application.Quit();
    }
}
