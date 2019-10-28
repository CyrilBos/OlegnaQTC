using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject damageTextPrefab;

    private Canvas canvas;

    private Camera mainCamera;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        mainCamera = Camera.main;
        Character.OnDamageTaken += CreateDamageText;
    }

    private void OnDisable()
    {
        Character.OnDamageTaken -= CreateDamageText;
    }

    public void CreateDamageText(int damage, Vector3 position)
    {
        GameObject damageText = Instantiate(damageTextPrefab, mainCamera.WorldToScreenPoint(position), Quaternion.identity);
        Text text = damageText.GetComponent<Text>();
        text.text = damage.ToString();

        Vector2 screenPosition = mainCamera.WorldToScreenPoint((Vector2)position);
        screenPosition.x += Random.Range(-50, +50);
        screenPosition.y += 128;

        damageText.transform.SetParent(canvas.transform, false);
        damageText.transform.position = screenPosition;
    }
}
