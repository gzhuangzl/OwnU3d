using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Framework;
using System.Runtime.Serialization;
using System;

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
//			Log.WriteLog(Application.dataPath);
//			Log.WriteWarning(GUID.GetNumber());
//			Log.WriteError(GUID.GetString());
//			List<int> l = new List<int>(1);
//			l.Add(1);
//			l.Add(2);
//			Debug.Log(l.Count);
//			Debug.Log(eventType + ":" + eventData);
//			if(count == 10){
//				string s = Serializetion.ToBase64String(new tt());
//				Debug.Log(s);
//				tt go = Serializetion.FromBase64String<tt>(s);
//				Debug.Log(go);
//			}
			
//			LocalStorage.SetObject("aq",new tt());
//			
//			Debug.Log(LocalStorage.GetObject<tt>("aq"));
//			
//			LocalStorage.DeleteKey("aq");
//			
//			LocalStorage.SetFloat("aa",1.33f);
//			Debug.Log(LocalStorage.GetFloat("aa"));

//			P p = ObjectPool.GetObject<P>();
//			ObjectPool.DisposeObject(p);
//			
//			Debug.Log(BitConverter.ToString(Guid.NewGuid().ToByteArray()));
		}
	}
	[System.Serializable]
	public class tt{
		public int x = 23;
		public string y = "dsfjasd";
		
		public int getX(){
			return x;
		}
		
		public string getY(){
			return y;
		}
	}
	
	public class P:IReusable{
		public void Reset(){
		
		}
	}
}

