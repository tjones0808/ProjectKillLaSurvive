using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] float speed;

    public Transform Reticule;

    Transform crossTop;
    Transform crossBottom;
    Transform crossLeft;
    Transform crossRight;

    float reticuleStartPoint;
    private void Start()
    {
        crossTop = Reticule.FindChild("Cross/Top").transform;
        crossBottom = Reticule.FindChild("Cross/Bottom").transform;
        crossLeft = Reticule.FindChild("Cross/Left").transform;
        crossRight = Reticule.FindChild("Cross/Right").transform;

        reticuleStartPoint = crossTop.localPosition.y;
    }

    void SetVisibility(bool value)
    {
        Reticule.gameObject.SetActive(value);
    }

    private void Update()
    {
        SetVisibility(false);
        if (GameManager.Instance.InputController.Fire2)
        {
            SetVisibility(true);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            Reticule.transform.position = Vector3.Lerp(Reticule.transform.position, screenPosition, speed * Time.deltaTime);
        }
       
    }

    public void ApplyScale(float scale)
    {
        crossTop.localPosition = new Vector3(0, reticuleStartPoint + scale, 0);
        crossBottom.localPosition = new Vector3(0, -reticuleStartPoint - scale, 0);
        crossLeft.localPosition = new Vector3(-reticuleStartPoint - scale, 0, 0);
        crossRight.localPosition = new Vector3(reticuleStartPoint + scale, 0, 0);
    }
}
