using System.Collections;
using UnityEngine;
using TMPro;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject menaceMan;
    [SerializeField] private GameObject tutorial;

    [SerializeField] private float disappearTime = 5f;
    private TextMeshProUGUI tutorialText;

    private void Awake() {
        tutorialText = tutorial.GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerExit(Collider other) {
        //this code is really funny lmao \/ \/ \/
        Destroy(gameObject.transform.GetChild(0).gameObject);
        tutorial.SetActive(true);
        StartCoroutine(CountdownAndChange());
    }

    IEnumerator CountdownAndChange()
    {
        yield return new WaitForSeconds(disappearTime);

        tutorialText.text = "it can't see.";
        tutorialText.fontSize = 50;

        yield return new WaitForSeconds(disappearTime/1.5f);

        tutorialText.text = "it hears.";

        yield return new WaitForSeconds(disappearTime/1.5f);

        tutorialText.fontSize = 40;
        tutorialText.text = "find all 8 slices.";

        yield return new WaitForSeconds(disappearTime/2f);

        Destroy(tutorial);

        menaceMan.GetComponent<EnemyAI>().enabled = true;
    }
}