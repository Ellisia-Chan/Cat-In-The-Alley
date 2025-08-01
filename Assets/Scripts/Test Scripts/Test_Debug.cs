using TMPro;
using UnityEngine;

namespace CatInTheAlley.TestScripts {
    public class Test_Debug : MonoBehaviour {
        [Header("Debug FPS")]
        [SerializeField] private GameObject FPSObject;
        [SerializeField] private TextMeshProUGUI FPSText;
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private int vSyncCount = 0;
        [SerializeField] private int refreshRate = 1;

        private float timer;
        private float frameCount;

        private void Start() {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncCount;
        }

        private void Update() {
            DEBUG_FPS();
        }

        private void DEBUG_FPS() {
            timer += Time.deltaTime;

            frameCount++;
            if (timer >= refreshRate) {
                int fps = Mathf.RoundToInt(frameCount / timer);
                FPSText.text = $"{fps}";
                timer = 0f;
                frameCount = 0;
            }
        }
    }
}