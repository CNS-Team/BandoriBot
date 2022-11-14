using BandoriBot.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandoriBot.Commands
{
    public class BlacklistCommand : HashCommand<BlacklistF,string>
    {
        public override List<string> Alias => new List<string>
        {
            "/blacklist"
        };

        protected override string Permission => throw new NotImplementedException();

        protected override long GetTarget(string value)
        {
            try
            {
                return long.Parse(value.Split('.')[0]);
            }
            catch
            {
                return 0;
            }
        }

        public override async Task Run(CommandArgs args)
        {
            await base.Run(args);
        }
    }
}
