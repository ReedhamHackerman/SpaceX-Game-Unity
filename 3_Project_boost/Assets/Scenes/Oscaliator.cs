using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Oscaliator : MonoBehaviour
{
    
    [SerializeField] Vector3 MomentVector = new Vector3(5f,0f,0f);
    [SerializeField] float period = 2f;
    // range in how much it should go in that particular direction 
    float Movementfactor;
    Vector3 startingPos;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    //set movement factor and should be done in terms of sin vector 
    void Update()
    {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2; // arournd 2 * pie
        float rawsinwave = Mathf.Sin(cycles * tau);
        Movementfactor = rawsinwave / 2f + 0.5f ;
        Vector3 offset = Movementfactor * MomentVector;
        transform.position = offset + startingPos;
    }
}
