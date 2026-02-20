using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class ColorPicker : PTBase
    {
        [SerializeField] private Image SV;
        [SerializeField] private Image H;

        private Texture2D sv_texture;
        private Texture2D h_texture;

        [SerializeField] private Picker sv_picker;
        [SerializeField] private Picker h_picker;

        [SerializeField] private Image R;
        [SerializeField] private Image G;
        [SerializeField] private Image B;

        private Texture2D r_texture;
        private Texture2D g_texture;
        private Texture2D b_texture;

        [SerializeField] private Picker r_picker;
        [SerializeField] private Picker g_picker;
        [SerializeField] private Picker b_picker;

        [SerializeField] private TMP_InputField r_input;
        [SerializeField] private TMP_InputField g_input;
        [SerializeField] private TMP_InputField b_input;

        [SerializeField] private TMP_InputField hex_input;

        [SerializeField] private Image current;
        [SerializeField] private Button apply;

        private Color color;
        private bool used_hsv;
        private float[] preserve_hsv;

        private UnityAction<Color> callback;

        public void Show(UnityAction<Color> callback)
        {
            gameObject.SetActive(true);

            color = Color.white;
            used_hsv = false;

            this.callback = callback;

            UpdateColor(color);
        }

        private void Awake()
        {
            sv_texture = new Texture2D(200, 200);
            h_texture = new Texture2D(200, 1);

            var colors = new Color32[200];
            for (int i = 0; i < 200; i++)
                colors[i] = Color.HSVToRGB(i / 200f, 1, 1);
            h_texture.SetPixels32(colors);
            h_texture.Apply();

            SV.sprite = Sprite.Create(sv_texture, new(0, 0, 200, 200), Vector2.zero);
            H.sprite = Sprite.Create(h_texture, new(0, 0, 200, 1), Vector2.zero);

            r_texture = new Texture2D(140, 1);
            g_texture = new Texture2D(140, 1);
            b_texture = new Texture2D(140, 1);

            R.sprite = Sprite.Create(r_texture, new(0, 0, 140, 1), Vector2.zero);
            G.sprite = Sprite.Create(g_texture, new(0, 0, 140, 1), Vector2.zero);
            B.sprite = Sprite.Create(b_texture, new(0, 0, 140, 1), Vector2.zero);

            sv_picker.OnChange += sv =>
            {
                if (used_hsv)
                    UpdateColor(preserve_hsv[0], sv.x, sv.y);
                else
                {
                    Color.RGBToHSV(color, out float h, out _, out _);
                    UpdateColor(h, sv.x, sv.y);
                }
            };
            h_picker.OnChange += h =>
            {
                if (used_hsv)
                    UpdateColor(h.x, preserve_hsv[1], preserve_hsv[2]);
                else
                {
                    Color.RGBToHSV(color, out _, out float s, out float v);
                    UpdateColor(h.x, s, v);
                }
            };

            r_picker.OnChange += r => UpdateColor(new(r.x, color.g, color.b));
            g_picker.OnChange += g => UpdateColor(new(color.r, g.x, color.b));
            b_picker.OnChange += b => UpdateColor(new(color.r, color.g, b.x));

            r_input.onEndEdit.AddListener(_ =>
            {
                if (r_input.text == null
                || r_input.text.Length == 0
                || !int.TryParse(r_input.text, out int value))
                {
                    r_input.text = ((int)(color.r * 255)).ToString();
                    return;
                }
                value = Mathf.Clamp(value, 0, 255);
                r_input.text = value.ToString();
                UpdateColor(new(value / 255f, color.g, color.b));
            });
            g_input.onEndEdit.AddListener(_ =>
            {
                if (g_input.text == null
                || g_input.text.Length == 0
                || !int.TryParse(g_input.text, out int value))
                {
                    g_input.text = ((int)(color.g * 255)).ToString();
                    return;
                }
                value = Mathf.Clamp(value, 0, 255);
                g_input.text = value.ToString();
                UpdateColor(new(color.g, value / 255f, color.b));
            });
            b_input.onEndEdit.AddListener(_ =>
            {
                if (b_input.text == null
                || b_input.text.Length == 0
                || !int.TryParse(b_input.text, out int value))
                {
                    b_input.text = ((int)(color.b * 255)).ToString();
                    return;
                }
                value = Mathf.Clamp(value, 0, 255);
                b_input.text = value.ToString();
                UpdateColor(new(color.r, color.g, value / 255f));
            });

            hex_input.onEndEdit.AddListener(_ =>
            {
                if (hex_input.text == null
                || hex_input.text.Length != 6
                || !int.TryParse(hex_input.text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int value))
                {
                    hex_input.text = ToHex(color);
                    return;
                }
                UpdateColor(new(((value >> 16) & 255) / 255f, ((value >> 8) & 255) / 255f, (value & 255) / 255f));
            });

            apply.onClick.AddListener(() =>
            {
                callback?.Invoke(color);
                gameObject.SetActive(false);
            });

            UpdateColor(Color.white);
        }

        private void UpdateColor(Color color)
        {
            used_hsv = false;

            Color.RGBToHSV(color, out float h, out float s, out float v);
            UpdateColor(color, h, s, v);
        }

        private void UpdateColor(float h, float s, float v)
        {
            used_hsv = true;
            preserve_hsv = new float[] { h, s, v };

            UpdateColor(Color.HSVToRGB(h, s, v), h, s, v);
        }

        private void UpdateColor(Color color, float h, float s, float v)
        {
            this.color = color;

            var colors = new Color32[200 * 200];
            for (int i = 0; i < 200; i++)
                for (int j = 0; j < 200; j++)
                    colors[i * 200 + j] = Color.HSVToRGB(h, j / 200f, i / 200f);
            sv_texture.SetPixels32(colors);
            sv_texture.Apply();

            colors = new Color32[140];
            for (int i = 0; i < 140; i++)
                colors[i] = new Color(i / 140f, color.g, color.b);
            r_texture.SetPixels32(colors);
            r_texture.Apply();

            colors = new Color32[140];
            for (int i = 0; i < 140; i++)
                colors[i] = new Color(color.r, i / 140f, color.b);
            g_texture.SetPixels32(colors);
            g_texture.Apply();

            colors = new Color32[140];
            for (int i = 0; i < 140; i++)
                colors[i] = new Color(color.r, color.g, i / 140f);
            b_texture.SetPixels32(colors);
            b_texture.Apply();

            sv_picker.Value = new(s, v);
            h_picker.Value = new(h, 0);

            r_picker.Value = new(color.r, 0);
            g_picker.Value = new(color.g, 0);
            b_picker.Value = new(color.b, 0);

            r_input.text = ((int)(color.r * 255)).ToString();
            g_input.text = ((int)(color.g * 255)).ToString();
            b_input.text = ((int)(color.b * 255)).ToString();

            hex_input.text = ToHex(color);

            current.color = color;
        }

        private static string ToHex(Color color)
        {
            return
                ((byte)(color.r * 255)).ToString("X2") +
                ((byte)(color.g * 255)).ToString("X2") +
                ((byte)(color.b * 255)).ToString("X2");
        }
    }
}