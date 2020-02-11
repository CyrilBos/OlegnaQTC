using Skills;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class TextInstantiator : MonoBehaviour
    {
        [SerializeField]
        private GameObject damageTextPrefab, skillMissedPrefab;

        private Canvas _canvas;

        private Camera _mainCamera;

        private void Awake()
        {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            _mainCamera = Camera.main;
            Character.OnDamageTaken += CreateDamageText;
            CooldownSkill.TargetDodged += CreateMissText;
        }

        private void OnDisable()
        {
            Character.OnDamageTaken -= CreateDamageText;
            CooldownSkill.TargetDodged += CreateMissText;
        }

        private void CreateDamageText(int damage, Vector3 position)
        {
            GameObject damageText = Instantiate(damageTextPrefab);

            SetRandomTextPositionAround(position, damageText);

            Text text = damageText.GetComponentInChildren<Text>();
            text.text = damage.ToString();
            // damageText.transform.position = screenPosition;
        }

        private void SetRandomTextPositionAround(Vector3 position, GameObject text)
        {
            Vector2 screenPosition = _mainCamera.WorldToScreenPoint((Vector2) position);
            screenPosition.x += Random.Range(-50, +50);
            screenPosition.y += 64;

            text.transform.SetParent(_canvas.transform, false);
            text.transform.position = screenPosition;
        }

        private void CreateMissText(object sender, SkillMissedEventArgs e)
        {
            var missText = Instantiate(skillMissedPrefab);
            
            SetRandomTextPositionAround(e.Position, missText);

        }
    }
}
