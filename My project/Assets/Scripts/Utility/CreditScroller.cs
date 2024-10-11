using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditScroller : MonoBehaviour {
    public Text creditText;
    public float scrollSpeed = 50f;
    public float resetPositionY = -200f;

    private void Start()
    {
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        Vector3 startPosition = creditText.transform.position;

        while (true)
        {
            creditText.transform.position += scrollSpeed * Time.deltaTime * Vector3.up;

            if (creditText.transform.position.y > Screen.height + 200)
            {
                creditText.transform.position = new Vector3(startPosition.x, resetPositionY, startPosition.z);
            }

            yield return null;
        }
    }
}
