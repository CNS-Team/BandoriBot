using BandoriBot.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandoriBot.Commands
{
    public class WhitelistCommand : HashCommand<Whitelist, long>
    {
        public override List<string> Alias => new List<string>
        {
            "/whitelist"
        };

        protected override string Permission => throw new NotImplementedException();

        protected override long GetTarget(long value) => value;

        public override async Task Run(CommandArgs args)
        {
            await base.Run(args);
        }
    }
}
