using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine _coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.OnTakeDamage += Flash;
    }

    private void Flash()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f);
        _coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
            yield return null;
        }
    }
}
