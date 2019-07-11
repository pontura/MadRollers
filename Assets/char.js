#pragma strict

var speed : float;


function Start () {

}

function Update () {
 transform.position.z += speed * Time.deltaTime;
}