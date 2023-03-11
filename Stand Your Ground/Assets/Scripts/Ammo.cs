
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public AmmoCrateData ammoCrateData;
    public AnimationCurve curve;
    public float scale = 1f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // animate the crate
        transform.position += transform.up * curve.Evaluate(Mathf.Repeat(Time.time, 1f)) * Time.deltaTime * scale;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Player") {
            hit.gameObject.GetComponent<PlayerController>().AddAmmo(ammoCrateData.ammoAmount);
            Destroy(gameObject);
        }
    }
}
