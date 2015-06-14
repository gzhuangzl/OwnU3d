using UnityEngine;
using System.Collections;
using Framework;

namespace Game{

	public class Test : MonoBehaviour {
		private int count = 0;
		// Use this for initialization
		void Start () {
			EventDispatcher.Instance.AddListener(1,t);
		}
		
		// Update is called once per frame
		void Update () {
			EventDispatcher.Instance.Dispatcher(1,new object());
			if(count++ > 10){
				EventDispatcher.Instance.RemoveListener(1,t);
			}
		}
		
		private void t(int eventType,object eventData){
			Debug.Log(eventType + ":" + eventData);
		}
	}
}

