using UnityEngine;
using System.Collections;
using System.Text;

public class PanelController : MonoBehaviour {
	
	UILabel _text,_header;
	string header;
	StringBuilder text;
	public GameObject owner;
	
	// Use this for initialization
	void Start () {
		_text=transform.Find("Text").GetComponent<UILabel>();
		_header=transform.Find("Header").GetComponent<UILabel>();
		text=new StringBuilder();
	}
	
	// Update is called once per frame
	void Update () {
		if(owner!=null){
			header=owner.name;
			text.Remove(0,text.Length);
			Rigidbody r=owner.rigidbody;
			if(r!=null)
			{
				text.AppendLine(@"mass: "+r.mass);
				text.AppendLine(@"position: "+r.position);
				text.AppendLine(@"velocity: "+r.velocity);
				text.AppendLine(@"rotation: "+r.rotation.eulerAngles);
				text.AppendLine(@"drag: "+r.drag);
			}
		}
		_text.text=text.ToString();
		_header.text=header;
	}
}
