using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class Preview : PTBase
    {
        [SerializeField] private CanvasGroup planets;
        [SerializeField] private Image redPlanet;
        [SerializeField] private Image bluePlanet;
        [SerializeField] private Image customPlanet;
        [SerializeField] private Image goldPlanet;
        [SerializeField] private Rainbow rainbow;
        [SerializeField] private Image image;

        [SerializeField] private Image ring;
        [SerializeField] private GameObject samurai;
        [SerializeField] private GameObject emoji;

        private PlanetImage ptImage;

        // private bool disableTailCustom = true;
        private bool disableRingCustom = true;

        public void SetColor(Keys key, Color color)
        {
            switch (key)
            {
                case Keys.PlanetColor:
                    if (color == Color.red || color == Color.blue || Colors.IsSpecial(color))
                    {
                        redPlanet.gameObject.SetActive(false);
                        bluePlanet.gameObject.SetActive(false);
                        customPlanet.gameObject.SetActive(false);
                        goldPlanet.gameObject.SetActive(false);
                        rainbow.enabled = false;
                        if (color == Color.red)
                        {
                            redPlanet.gameObject.SetActive(true);
                            if (disableRingCustom)
                                SetRingColor(Color.red);
                            return;
                        }
                        else if (color == Color.blue)
                        {
                            bluePlanet.gameObject.SetActive(true);
                            if (disableRingCustom)
                                SetRingColor(Color.blue);
                            return;
                        }
                        else if (color == Colors.goldColor)
                        {
                            disableRingCustom = true;
                            goldPlanet.gameObject.SetActive(true);
                            SetRingColor(Colors.realGoldColor);
                        }
                        else if (color == Colors.rainbowColor)
                        {
                            disableRingCustom = true;
                            customPlanet.gameObject.SetActive(true);
                            rainbow.enabled = true;
                        }
                        else if (color == Colors.overseerColor)
                        {
                            disableRingCustom = true;
                            customPlanet.gameObject.SetActive(true);
                            SetPlanetColor(Colors.realOverseerColor);
                        }
                        return;
                    }
                    redPlanet.gameObject.SetActive(false);
                    bluePlanet.gameObject.SetActive(false);
                    customPlanet.gameObject.SetActive(true);
                    goldPlanet.gameObject.SetActive(false);
                    rainbow.enabled = false;
                    SetPlanetColor(color);
                    break;
                case Keys.TailColor:
                    // welp... i don't know about particles in unity, that's way too hard for me...
                    break;
                case Keys.RingColor:
                    if (color == Colors.disableColor)
                    {
                        disableRingCustom = true;
                        SetRingColor(redPlanet.gameObject.activeSelf
                            ? Color.red
                            : (bluePlanet.gameObject.activeSelf
                            ? Color.blue
                            : (customPlanet.gameObject.activeSelf
                            ? customPlanet.color
                            : goldPlanet.gameObject.activeSelf
                            ? Colors.realGoldColor
                            : throw new Exception("what?"))));
                        return;
                    }
                    disableRingCustom = false;
                    SetRingColor(color);
                    break;
            }
        }

        public void SetPlanetColor(Color color)
        {
            customPlanet.color = color;
            if (disableRingCustom)
                SetRingColor(color);
        }

        public void SetRingColor(Color color)
        {
            ring.color = color.WithAlpha(ring.color.a);
        }

        public void SetAlpha(Keys key, float alpha)
        {
            switch (key)
            {
                case Keys.PlanetAlpha:
                    planets.alpha = alpha;
                    break;
                case Keys.TailAlpha:
                    // welp...
                    break;
                case Keys.RingAlpha:
                    ring.color = ring.color.WithAlpha(alpha);
                    break;
            }
        }

        public void SetSamurai(bool value)
        {
            samurai.SetActive(value);
        }

        public void SetEmoji(bool value)
        {
            emoji.SetActive(value);
        }

        public void SetImage(PlanetImage image)
        {
            ptImage = image;
            this.image.gameObject.SetActive(image != null);
        }

        public void SetImagePosition(Vector2 position)
        {
            image.GetComponent<RectTransform>().anchoredPosition = position;
        }

        public void SetImageSize(Vector2 size)
        {
            image.GetComponent<RectTransform>().sizeDelta = size;
        }

        private void LateUpdate()
        {
            if (ptImage == null)
                return;
            var current = ptImage.GetImage();
            if (image.sprite != current)
                image.sprite = current;
        }
    }
}