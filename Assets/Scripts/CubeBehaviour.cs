using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CubeBehaviour : MonoBehaviour
{
    private enum ActiveState {
        Active = 0,
        Inactive = 1,
    };
    public Material activeMaterial;
    public Material inactiveMaterial;
    private ActiveState state = ActiveState.Inactive;

    public Cube data;

    // Start is called before the first frame update
    void Start()
    {
        SetMaterial();
        SetTag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive( bool active ) {
        this.state = active ? ActiveState.Active : ActiveState.Inactive;
        SetMaterial();
        SetTag();
    }

    private void SetMaterial() {
        GetComponent<MeshRenderer>().material = IsActive() ? activeMaterial : inactiveMaterial;
    }

    private void SetTag() {
        this.tag = IsActive() ? "activeCube" : "inactiveCube";
    }

    public bool IsActive() {
        return state == ActiveState.Active;
    }
}
