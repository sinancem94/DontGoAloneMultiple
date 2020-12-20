using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdpKit;
using UdpKit.Platform.Photon;
using Bolt.Matchmaking;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "BuildingBlocks")]
public class ServerCallbacks : Bolt.GlobalEventListener
{
	private bool _gameStarted;
	
	private int _idIncrementer;
	private int _limit = 0;

	#region Utils

	private bool ReachedLimit => _idIncrementer >= _limit;

	void StartGameIfLimit()
	{
		if (ReachedLimit)
		{
			ServerGameManager.instance.StartGame();
			_gameStarted = true;
		}
	}

	#endregion
	
	#region Scene

	public override void SceneLoadLocalBegin(string scene, Bolt.IProtocolToken token)
	{
		_limit = BoltMatchmaking.CurrentSession.ConnectionsMax;
		_gameStarted = false;
		
		//Debug.LogError("SceneLoadLocalBegin server");
	}
	
	
	public override void SceneLoadLocalDone(string scene, Bolt.IProtocolToken token)
	{
		//Debug.LogError("SceneLoadLocalDone server");

		ServerGameManager.Instantiate();
		ServerGameManager.instance.serverPlayer =  new Explorer();
		
		ServerGameManager.instance.serverPlayer.ExplorerId = _idIncrementer;
		ServerGameManager.instance.serverPlayer.NickName = "SERVER";
		
		ServerGameManager.instance.serverPlayer.InstantiateExplorer();
		ServerGameManager.instance.boardManager.AddExplorer(ServerGameManager.instance.serverPlayer);

		_idIncrementer++;
		
		StartGameIfLimit();
	}

	
	public override void SceneLoadRemoteDone(BoltConnection connection, Bolt.IProtocolToken token)
	{
		// instantiate explorer
		connection.GetExplorer().InstantiateExplorer();
		ServerGameManager.instance.boardManager.AddExplorer(connection.GetExplorer());
		
		StartGameIfLimit();
	}
	
	#endregion



	#region Connection
	
	public override void Connected(BoltConnection connection)
	{ 
		BoltConsole.Write("Connected", Color.red);
		
		//new explorer connected room. create new one 
		connection.UserData =  new Explorer();
		connection.GetExplorer().connection = connection;
		connection.GetExplorer().ExplorerId = _idIncrementer;
		connection.GetExplorer().NickName = "CLIENT:" + connection.RemoteEndPoint.Port;

		_idIncrementer++;
		
		connection.SetStreamBandwidth(1024 * 1024);
	}
	
	public override void Disconnected(BoltConnection connection)
	{ 
		BoltConsole.Write("Disconnected", Color.red);
		base.Disconnected(connection);
		ServerGameManager.instance.boardManager.RemoveExplorer((Explorer)connection.UserData);
	}

	public override void ConnectRequest(UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
	{ 
		BoltConsole.Write("ConnectRequest", Color.red);
   
		if (token != null)
		{ 
			BoltConsole.Write("Token Received", Color.red);
		}
		
		if(_gameStarted)
			BoltNetwork.Refuse(endpoint);
            
		BoltNetwork.Accept(endpoint);
	}
	
	public override void ConnectRefused(UdpEndPoint endpoint, Bolt.IProtocolToken token)
	{ 
		BoltConsole.Write("ConnectRefused", Color.red);
		base.ConnectRefused(endpoint, token);
	}
   
	public override void ConnectFailed(UdpEndPoint endpoint, Bolt.IProtocolToken token)
	{ 
		BoltConsole.Write("ConnectFailed", Color.red);
		base.ConnectFailed(endpoint, token);
	}
   
	public override void ConnectAttempt(UdpEndPoint endpoint, Bolt.IProtocolToken token)
	{ 
		BoltConsole.Write("ConnectAttempt", Color.red);
		base.ConnectAttempt(endpoint, token);
	}


	#endregion

	

   
   
	
	
}
