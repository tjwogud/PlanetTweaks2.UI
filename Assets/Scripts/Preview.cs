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

        [SerializeField] private Image ring;
        [SerializeField] private GameObject samurai;
        [SerializeField] private GameObject emoji;

        // private bool disableTailCustom = true;
        private bool disableRingCustom = true;

        public void SetColor(Keys key, Color color)
        {
            switch (key)
            {
                case Keys.PlanetColor:
                    if (color.a == 0)
                    {
                        redPlanet.gameObject.SetActive(false);
                        bluePlanet.gameObject.SetActive(false);
                        customPlanet.gameObject.SetActive(false);
                        goldPlanet.gameObject.SetActive(false);
                        rainbow.enabled = false;
                        if (color == Colors.redColor)
                        {
                            redPlanet.gameObject.SetActive(true);
                            if (disableRingCustom)
                                SetRingColor(Color.red);
                            return;
                        }
                        else if (color == Colors.blueColor)
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
                    // welp... i am not able to put a particle in unity, that's way toooo hard for me.
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

        private void SetPlanetColor(Color color)
        {
            customPlanet.color = color;
            if (disableRingCustom)
                SetRingColor(color);
        }

        private void SetRingColor(Color color)
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
    }
}