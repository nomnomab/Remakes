using System;
using UnityEngine;

public class Gun: MonoBehaviour {
    [SerializeField] private Superhot _superhot;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _force = 1;
    [SerializeField] private bool _isPlayer;

    private void Update() {
        if (Input.GetMouseButtonUp(0) && _isPlayer) {
            ShootOrigin();
            
            _superhot.EnableAction(0.1f);
        }
    }

    public void ShootOrigin() {
        Vector3 forward = Camera.main.transform.forward;
        ShootPosition(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f)) + forward * 2);
    }

    public void ShootPosition(Vector3 origin) {
        GameObject instance = Instantiate(_bullet, origin, transform.rotation);
        Rigidbody rBody = instance.GetComponent<Rigidbody>();
        rBody.AddForce(instance.transform.forward * _force, ForceMode.Acceleration);
    }
}