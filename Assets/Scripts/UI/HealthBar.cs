using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] Image fillBar;
    Damageable unit;

    // Start is called before the first frame update
    void Start() {
        unit = GetComponentInParent<Damageable>();
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = GameManager.Instance.cameraTransform.rotation;
        fillBar.fillAmount = unit.GetNormalizedHealth();
    }
}
