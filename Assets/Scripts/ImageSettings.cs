using Ookii.Dialogs;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class ImageSettings : PTBase
    {
        [SerializeField] private Button fileSelect;
        [SerializeField] private Button disable;
        [SerializeField] private TMP_InputField x;
        [SerializeField] private TMP_InputField y;
        [SerializeField] private TMP_InputField w;
        [SerializeField] private TMP_InputField h;
        [SerializeField] private Button fixRotation;

        private float _x;
        private float _y;
        private float _w = 100;
        private float _h = 100;

        private VistaOpenFileDialog dialog;

        private void Awake()
        {
            dialog = new VistaOpenFileDialog();
            dialog.Filter = "Image File|*.png;*.jpg;*.jpeg;*.webp;*.bmp;*.gif;*.apng|All Files|*.*";
            dialog.Title = "Select Image";

            fileSelect.onClick.AddListener(() =>
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.FileName;
                    var ext = Path.GetExtension(path);
                    var isAnim = ext == ".gif" || ext == ".apng";
                    var image = PlanetImage.Create(path);
                    if (image == null) return; // should not
                    UI.SetValue(Keys.Image, image);
                    UI.preview.SetImage(image);
                    disable.gameObject.SetActive(true);
                }
            });

            disable.onClick.AddListener(() =>
            {
                UI.SetValue(Keys.Image, null);
                UI.preview.SetImage(null);
                disable.gameObject.SetActive(false);
            });

            x.onEndEdit.AddListener(_ =>
            {
                if (!float.TryParse(x.text, out float value))
                {
                    x.text = _x.ToString();
                    return;
                }
                _x = value;
                UI.SetValue(Keys.ImagePosition, new Vector2(_x, _y));
                UI.preview.SetImagePosition(new(_x, _y));
            });

            y.onEndEdit.AddListener(_ =>
            {
                if (!float.TryParse(y.text, out float value))
                {
                    y.text = _y.ToString();
                    return;
                }
                _y = value;
                UI.SetValue(Keys.ImagePosition, new Vector2(_x, _y));
                UI.preview.SetImagePosition(new(_x, _y));
            });

            w.onEndEdit.AddListener(_ =>
            {
                if (!float.TryParse(w.text, out float value))
                {
                    w.text = _w.ToString();
                    return;
                }
                _w = value;
                UI.SetValue(Keys.ImageSize, new Vector2(_w, _h));
                UI.preview.SetImageSize(new(_w, _h));
            });

            h.onEndEdit.AddListener(_ =>
            {
                if (!float.TryParse(h.text, out float value))
                {
                    h.text = _h.ToString();
                    return;
                }
                _h = value;
                UI.SetValue(Keys.ImageSize, new Vector2(_w, _h));
                UI.preview.SetImageSize(new(_w, _h));
            });

            fixRotation.onClick.AddListener(() =>
            {
                var text = fixRotation.GetComponentInChildren<TMP_Text>();
                var enable = text.text == "Off";
                text.text = enable ? "On" : "Off";
                UI.SetValue(Keys.ImageFixRotation, enable);
            });
        }

        public void UpdateValue()
        {
            var image = (PlanetImage)UI.GetValue(Keys.Image);
            disable.gameObject.SetActive(image != null);

            var position = (Vector2)UI.GetValue(Keys.ImagePosition);
            x.text = position.x.ToString();
            _x = position.x;
            y.text = position.y.ToString();
            _y = position.y;

            var size = (Vector2)UI.GetValue(Keys.ImageSize);
            w.text = size.x.ToString();
            _w = size.x;
            h.text = size.y.ToString();
            _h = size.y;

            var fixRot = (bool)UI.GetValue(Keys.ImageFixRotation);
            fixRotation.GetComponentInChildren<TMP_Text>().text = fixRot ? "On" : "Off";
        }
    }
}
