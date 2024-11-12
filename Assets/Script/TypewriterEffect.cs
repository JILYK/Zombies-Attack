using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public string fullText ; // Полный текст, который нужно вывести
    public float typingDuration = 3f; // Время, за которое текст будет полностью написан

    private void OnEnable()
    {
       
            StartCoroutine(TypeText());
        
    }

    private IEnumerator TypeText()
    {
        gameObject.GetComponent<Text>().text = ""; // Очищаем текст перед началом
        float typingInterval = typingDuration / fullText.Length; // Интервал между появлением символов

        for (int i = 0; i < fullText.Length; i++)
        {
            gameObject.GetComponent<Text>().text += fullText[i]; // Добавляем следующий символ к тексту
            yield return new WaitForSeconds(typingInterval); // Ждем перед добавлением следующего символа
        }
    }
}