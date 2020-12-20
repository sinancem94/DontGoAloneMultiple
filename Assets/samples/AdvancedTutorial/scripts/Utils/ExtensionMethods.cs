using UnityEngine;
using System.Collections;
using Bolt.AdvancedTutorial;
public static class ExtensionMethods
{
	public static Player GetPlayer (this BoltConnection connection)
	{
		if (connection == null) { return Player.serverPlayer;
		}

		return (Player)connection.UserData; 
	}
		
	public static Explorer GetExplorer (this BoltConnection connection)
	{
		if (connection == null)
		{
			Debug.LogWarning("GetExplorer Connection null");
			return null;
		} 
		else if (connection.UserData == null)
		{
			Debug.LogWarning("GetExplorer Connection.UserData null");
			return null;
		}
		return (Explorer)connection.UserData;
	}
}

