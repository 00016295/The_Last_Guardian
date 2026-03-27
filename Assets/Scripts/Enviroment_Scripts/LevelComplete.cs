using UnityEngine;
using TMPro;
using System.Collections;

public class LevelComplete : MonoBehaviour
{
    public GameObject resultsPanel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI hitsText;
    // Добавьте остальные поля...

    public void ShowResults(float time, int hits)
    {
        resultsPanel.SetActive(true);
        StartCoroutine(DisplayRoutine(time, hits));
    }

    IEnumerator DisplayRoutine(float time, int hits)
    {
        // 1. Показываем время
        timeText.text = FormatTime(time);
        // Можно добавить звук "тык"
        yield return new WaitForSeconds(0.5f);

        // 2. Эффект накрутки хитов
        int currentHits = 0;
        while (currentHits < hits)
        {
            currentHits += Mathf.CeilToInt(hits * Time.deltaTime * 2);
            hitsText.text = Mathf.Min(currentHits, hits).ToString();
            yield return null;
        }
    }

    string FormatTime(float time)
    {
        // Логика форматирования в 00:00.00
        return string.Format("{0:00}:{1:00}.{2:00}", Mathf.Floor(time / 60), time % 60, (time * 100) % 100);
    }
}