using UnityEngine;
using System.Collections;
using System.Text;

public class PanelController : MonoBehaviour {
	
	UILabel _text,_header;
	string header;
	StringBuilder text;
	public GameObject owner;
	//GameObject link;
	[SerializeField]
	LineRenderer line;
	// Use this for initialization
	void Start () {
		_text=transform.Find("Text").GetComponent<UILabel>();
		_header=transform.Find("Header").GetComponent<UILabel>();
		text=new StringBuilder();
		line=transform.Find("Link").GetComponent<LineRenderer>();
		//line.enabled=false;
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
//			line.enabled=true;
			line.SetPosition(0,line.transform.position);
			line.SetPosition(1,owner.transform.position);
		}
		_text.text=text.ToString();
		//_text.text="123";
		_header.text=header;
		//c=	transform.FindChild("Window").GetComponent<UISlicedSprite>().color;
	}



}
