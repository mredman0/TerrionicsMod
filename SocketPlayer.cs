﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
//using Terraria.ModLoader.Newtonsoft.Json;

namespace SocketTest
{
    public class SocketPlayer : ModPlayer
    {
		SocketTest mod;
		Socket socket;
		int count = 0;
		float[] positionArray = {0, 0};
		String output;
		
		public override void Initialize()
        {
			mod = (SocketTest)ModLoader.GetMod("SocketTest");
			socket = mod.socket;
				
			socket.On("spawn item", (data) =>
			{
				int itemID = Convert.ToInt32(data);
				player.QuickSpawnItem(itemID, 1);
			});
				
			if(!mod.playerInitialized)
			{				
				socket.On("chat message", (data) =>
				{
					socket.Emit("socket-connected", "Terraria received message");
					Main.NewText("Server: " + (String)data, 0, 0, 255, false);
				});
				
				mod.playerInitialized = true;
			}

 //               socket.On("spawn-npc", () =>
 //               {
 //                   socket.Emit("socket-connected", "aloha");
 //               });
        }
		
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			new Task(EmitPositionAsync).Start();
		}
		
		private async void EmitPositionAsync()
		{
			count = (count+1)%5;
			if(count == 0)
			{
				positionArray[0] = player.position.X;
				positionArray[1] = player.position.Y;
				String output = JsonConvert.SerializeObject(positionArray);
				socket.Emit("player-position", output);
				//Main.NewText(output);
			}
		}
    }
}
