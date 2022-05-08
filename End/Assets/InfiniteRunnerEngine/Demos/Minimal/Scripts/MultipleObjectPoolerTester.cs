using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	
	public class MultipleObjectPoolerTester : MonoBehaviour 
	{
		[InspectorButtonAttribute("DisableClouds")]
		public bool DisableCloudsBtn;
		[InspectorButtonAttribute("EnableClouds")]
		public bool EnableCloudsBtn;
		[InspectorButtonAttribute("ResetCounters")]
		public bool ResetCountersBtn;

		protected MultipleObjectPooler[] _objectPoolersList;

		void Start () 
		{
			_objectPoolersList = FindObjectsOfType (typeof(MultipleObjectPooler)) as MultipleObjectPooler[];
		}

		public virtual void DisableClouds()
		{
			foreach (MultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.EnableObjects ("Cloud1", false);
			}
		}

		public virtual void EnableClouds()
		{
			foreach (MultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.EnableObjects ("Cloud1", true);
			}
		}

		public virtual void ResetCounters()
		{
			foreach (MultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.ResetCurrentIndex ();
			}
		}




	}
}
