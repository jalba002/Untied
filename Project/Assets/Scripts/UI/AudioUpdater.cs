using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace com.kpg.ggj2022
{
    public class AudioUpdater : MonoBehaviour
    {
        public AudioMixer m_Mixer;

        Slider _slider;

        public TextMeshProUGUI valueDisplay;

        string variableName { get; set; }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(UpdateVisualValue);
            variableName = this.gameObject.name + "Volume";
        }

        public void SetLevel(float sliderValue)
        {
            m_Mixer.SetFloat(variableName, CalculateLogarithmicValue(sliderValue));
        }

        public float GetLevel()
        {
            m_Mixer.GetFloat(variableName, out float value);
            return value;
        }

        float CalculateLogarithmicValue(float value)
        {
            return Mathf.Log10(value) * 20f;
        }

        private void UpdateVisualValue(float value)
        {
            if (valueDisplay == null) return;
            valueDisplay.text = ((int)(value * 100)).ToString(CultureInfo.CurrentUICulture);
        }

        private void OnEnable()
        {
            UpdateSlidersAndValues(GetLevel());
        }

        private void UpdateSlidersAndValues(float value)
        {
            float transformedValue = Mathf.Pow(10, value / 20f);
            UpdateVisualValue(transformedValue);
            _slider.value = transformedValue;
        }
    }
}
