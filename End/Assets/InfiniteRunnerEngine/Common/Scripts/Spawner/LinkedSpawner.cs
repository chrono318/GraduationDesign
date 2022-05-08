using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Spawns and positions/resizes objects based on the distance traveled 
	/// </summary>
	[RequireComponent (typeof (ObjectPooler))]
	public class LinkedSpawner : Spawner 
	{
		

		/// the x distance we spawn the player at
		public float SpawnDistanceFromPlayer;

	    [Space(10)]
	    [Header("Activity")]

	    protected LinkedSpawnedObject _lastLinkedSpawnedObject;
	    protected Transform _lastSpawnedTransform;
		protected float _nextSpawnDistance;


	    /// <summary>
	    /// Triggered at the start of the level
	    /// </summary>
	    protected virtual void Start () 
		{
			/// we get the object pooler component
			_objectPooler = GetComponent<ObjectPooler> ();	
	        
	        //FirstSpawn();
	        	
		}

	    protected virtual void FirstSpawn()
	    {
	        /// we define the initial spaw position
			Vector2 spawnPosition = transform.position;
	        spawnPosition.x = transform.position.x + _nextSpawnDistance;
			spawnPosition.y += transform.position.y;
			LinkedSpawn(spawnPosition);	
	    }

	    /// <summary>
	    /// Triggered every frame
	    /// </summary>
	    protected virtual void FixedUpdate () 
		{
	        if (OnlySpawnWhileGameInProgress)
	        {
	            if (GameManager.Instance.Status != GameManager.GameStatus.GameInProgress)
	            {
	                _lastSpawnedTransform = null;
					_lastLinkedSpawnedObject = null;
	                return ;
	            }
	        }


	        if ((_lastSpawnedTransform== null) || (!_lastSpawnedTransform.gameObject.activeInHierarchy))
			{
	            FirstSpawn();
	        }

	        /// if we've reached the next spawn position, we spawn a new object
	        if (transform.position.x - _lastSpawnedTransform.position.x >= _nextSpawnDistance) 
			{
				/// we reposition the object
				Vector3 spawnPosition = _lastSpawnedTransform.transform.position+_lastSpawnedTransform.GetComponent<LinkedSpawnedObject>().Out;	
				LinkedSpawn(spawnPosition);					
			}		
		}
		
		/// <summary>
		/// Spawns an object at the specified position and determines the next spawn position
		/// </summary>
		/// <param name="spawnPosition">Spawn position.</param>
		protected virtual void LinkedSpawn(Vector3 spawnPosition)
		{
			
			GameObject spawnedObject = Spawn(spawnPosition);

			LinkedSpawnedObject spawnedLinkedSpawnedObject = spawnedObject.GetComponent<LinkedSpawnedObject>();


			if (spawnedObject==null)
			{
				if (_lastSpawnedTransform==null)
				{
					_lastSpawnedTransform = this.transform;
				}
				_nextSpawnDistance = 0f ;
			}
			else
			{
				if (_lastLinkedSpawnedObject!=null)
				{
					// we reposition the linked spawned object to have its In match the previously spawned object's Out.
					//Vector3 newPosition = spawnedObject.In;
					spawnedObject.transform.position = _lastSpawnedTransform.position + _lastLinkedSpawnedObject.Out - spawnedLinkedSpawnedObject.In;
				}
		
				_lastSpawnedTransform = spawnedObject.transform;
				_lastLinkedSpawnedObject=spawnedLinkedSpawnedObject;
				// we define the next spawn position based on the size of the current object and the specified gaps		
				_nextSpawnDistance = 0f;
			}
		}
	}
}