using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UE = UnityEngine;

public class Explorer : IDisposable
{
	public int ExplorerId;
	public string NickName;
	public BoltEntity entity;
	public BoltConnection connection;
	
	public IExplorerState State => entity.GetState<IExplorerState>();
	bool IsServer => connection == null;

	public void Kill()
	{
		if (!entity) return;
		State.Dead = true;
	}

	void Spawn()
	{
		State.Id = ExplorerId;
		Debug.LogWarning(ServerGameManager.instance.boardManager.board);
		Debug.LogWarning(State.Id);
		ExplorerSlate_SO thisSlate = ServerGameManager.instance.boardManager.board.currEquipments.GetExplorerSlate(State.Id);
		
		State.Stats.Speed = thisSlate.Speed.defaultVal;
		State.Stats.Might = thisSlate.Might.defaultVal;
		State.Stats.Knowledge = thisSlate.Knowledge.defaultVal;
		State.Stats.Sanity = thisSlate.Sanity.defaultVal;

		State.NickName = NickName;
		State.Dead = false;

		entity.transform.position = RandomSpawn();
	}

	public void Dispose()
	{
		// destroy
		if (!entity) return;
		BoltNetwork.Destroy(entity.gameObject);
	}

	public void InstantiateExplorer()
	{
		entity = BoltNetwork.Instantiate(BoltPrefabs.Explorer);
		if (!entity) return;
		Spawn();
		
		if (IsServer)
		{
			entity.TakeControl();
		}
		else
		{
			entity.AssignControl(connection);
		}
	}
		
	Vector3 RandomSpawn()
	{ 
		float x = UE.Random.Range(-4f, +4f); 
		float z = UE.Random.Range(-4f, +4f);
		return new Vector3(x, 1f, z); 
	}
}
	