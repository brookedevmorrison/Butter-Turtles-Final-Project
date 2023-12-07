using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Morrison, Brooke and Melendrez, Servando
/// 12/6/23
/// This script tilts the arena back and forth 
/// </summary>
public class ArenaMovement : MonoBehaviour
{
    //controls the speed of the angle tilting
    [SerializeField] float rotationSpeed = 20f;


    // Update is called once per frame
    void Update()
    {
        //forces the arena to tilt 15 degrees and tilt back 15 degrees
        transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * rotationSpeed, 15) - 15);
    }
}
