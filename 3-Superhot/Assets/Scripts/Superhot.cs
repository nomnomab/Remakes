using System.Collections;
using Attributes;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(Rigidbody))]
public class Superhot: CustomBehaviour {
    [Require] private Rigidbody _rBody;
    [Require] private RigidbodyFirstPersonController _controller;

    private bool _usingAction;
    private Coroutine _coroutine;

    private void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        const float limit = 0.001f;
        bool isZero = x < limit && x > -limit && y < limit && y > -limit && !_usingAction;
        bool falling = _rBody.velocity.y > limit || _rBody.velocity.y < -limit;

        if (isZero && !falling) {
            _rBody.isKinematic = true;
            _rBody.velocity = Vector3.zero;
        }
        else {
            _rBody.isKinematic = false;
        }

        float time = !isZero || falling ? 1f : 0.03f;
        float lerpTime = !isZero || falling ? 0.05f : 0.5f;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);
    }

    public void EnableAction(float time) {
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(WaitForAction(time));
    }

    private IEnumerator WaitForAction(float time) {
        _usingAction = true;
        yield return new WaitForSeconds(time);
        _usingAction = false;
    }
}