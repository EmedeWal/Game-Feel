using UnityEngine;

namespace ShatterStep
{
    public class CursorManager : MonoBehaviour
    {
        [Header("REFERENCE")]
        [SerializeField] private Texture2D _cursor;

        [Header("SETTINGS")]
        [SerializeField] private Vector2 _offset = new(25, 25);

        private void Start()
        {
            Cursor.SetCursor(_cursor, _offset, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            DontDestroyOnLoad(gameObject);
        }
    }
}