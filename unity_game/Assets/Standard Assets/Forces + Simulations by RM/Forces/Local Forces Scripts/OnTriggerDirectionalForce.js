@script AddComponentMenu ("Forces/Local/OnTrigger Directional Force")

var ForceSpeed: float = 1;
var Cloths: Cloth[] = null;
var TriggerDrag: float = 0;
var SpaceDrag: float = 0;

function OnTriggerStay (other : Collider) { 
        if(other.rigidbody==null)
        return;
var MyObjectsForce : Vector3  = gameObject.transform.TransformDirection (0, 1, 0);      
//GameObjects
other.rigidbody.AddForce (MyObjectsForce * ForceSpeed);
other.rigidbody.drag = TriggerDrag;
}

function OnTriggerExit (other : Collider) { 
if(other.rigidbody==null)
        return;
//GameObjects
other.rigidbody.drag = SpaceDrag;

}
