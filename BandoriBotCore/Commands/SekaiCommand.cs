﻿using BandoriBot.Models;
using Newtonsoft.Json.Linq;
using SekaiClient.Datas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BandoriBot.Commands
{
    public class SekaiLineCommand : ICommand
    {
        public List<string> Alias => new List<string> { "sekai线" };

        public void Run(CommandArgs args)
        {
            var track = Utils.GetHttp($"https://bitbucket.org/sekai-world/sekai-event-track/raw/main/event{MasterData.Instance.events.Last().id}.json");
            args.Callback(string.Join('\n', new int[] { 100, 500, 1000, 2000, 5000, 10000, 50000 }.Select(i => $"rank{i} pt={track[$"rank{i}"].Single()["score"]}")));
        }
    }

    public class SekaiCommand : ICommand
    {
        public List<string> Alias => new List<string> { "sekai" };
        private SekaiClient.SekaiClient client;
        private int eventId;

        private async Task ClientReady()
        {
            while (true)
            {
                try
                {
                    client = new SekaiClient.SekaiClient(new SekaiClient.EnvironmentInfo(), false);
                    await client.UpgradeEnvironment();
                    await client.Login(await client.Register());
                    await MasterData.Initialize(client);
                    await client.PassTutorial(true);
                    break;
                }
                catch (Exception e)
                {
                    this.Log(LoggerLevel.Error, e.ToString());
                }
            }
            eventId = MasterData.Instance.events.Last().id;
        }

        public SekaiCommand()
        {
            if (File.Exists("sekai"))
                ClientReady().Wait();
        }

        public void Run(CommandArgs args)
        {
            long arg;
            try
            {
                arg = long.Parse(args.Arg.Trim());
            }
            catch
            {
                return;
            }

            try
            {
                var result = (arg > int.MaxValue ?
                    client.CallUserApi($"/event/{eventId}/ranking?targetUserId={arg}", HttpMethod.Get, null) :
                    client.CallUserApi($"/event/{eventId}/ranking?targetRank={arg}", HttpMethod.Get, null)).Result;
                var rank = result["rankings"]?.SingleOrDefault();

                args.Callback(rank == null ? "找不到玩家" : $"排名为{rank["rank"]}的玩家是`{rank["name"]}`(uid={rank["userId"]})，分数为{rank["score"]}");
            }
            catch (Exception e)
            {
                this.Log(LoggerLevel.Debug, e.ToString());
                ClientReady().Wait();
            }
        }
    }
}
