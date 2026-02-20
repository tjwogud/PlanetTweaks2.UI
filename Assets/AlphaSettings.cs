using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlanetTweaks2.UI
{
    public class AlphaSettings : PTBase
    {
        [SerializeField] private Picker picker;
        [SerializeField] private TMP_InputField input;
        public UnityAction<float> SetAlpha { get; private set; }

        public void Init(UnityAction<float> setAlpha)
        {
            SetAlpha = setAlpha;
        }

        public void SetValue(float value)
        {
            picker.Value = new(value, 0);
            input.text = ((int)(value * 100)).ToString();
        }

        private void Awake()
        {
            picker.OnChange += value =>
            {
                SetAlpha(value.x);
                input.text = ((int)(value.x * 100)).ToString();
            };
            input.onEndEdit.AddListener(_ =>
            {
                if (input.text == null
                || input.text.Length == 0
                || !int.TryParse(input.text, out int value))
                {
                    input.text = ((int)(picker.Value.x * 100)).ToString();
                    return;
                }
                value = Mathf.Clamp(value, 0, 100);
                input.text = value.ToString();
                SetAlpha(value / 100f);
                picker.Value = new(value / 100f, 0);
            });
        }
    }
}