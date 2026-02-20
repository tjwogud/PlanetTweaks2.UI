using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PlanetTweaks2.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Picker : PTBase, IPointerDownHandler
    {
        [SerializeField] private bool horizontal = true;
        [SerializeField] private bool vertical = true;
        [SerializeField] private RectTransform picker;

        private bool dragging;
        private Vector3 prevMouse;

        private Vector2 size;

        private Vector2 value;
        public Vector2 Value
        {
            get => value;
            set
            {
                this.value = value;

                var x = horizontal ? value.x * size.x : 0;
                var y = vertical ? value.y * size.y : 0;
                picker.anchoredPosition = new Vector2(x, y);
            }
        }

        public event UnityAction<Vector2> OnChange;

        private void Awake()
        {
            size = GetComponent<RectTransform>().sizeDelta;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            dragging = true;
            prevMouse = new(0, 0, 1);
        }

        private void Update()
        {
            if (!dragging)
                return;
            if (!Input.GetMouseButton(0))
            {
                dragging = false;
                return;
            }
            var mouse = Input.mousePosition;
            if (prevMouse == mouse)
                return;
            prevMouse = mouse;
            var local = transform.InverseTransformPoint(mouse);

            var x = horizontal ? Mathf.Clamp(local.x, 0, size.x) : 0;
            var y = vertical ? Mathf.Clamp(local.y, 0, size.y) : 0;
            picker.anchoredPosition = new Vector2(x, y);

            value = new(x / size.x, y / size.y);
            OnChange?.Invoke(value);
        }
    }
}