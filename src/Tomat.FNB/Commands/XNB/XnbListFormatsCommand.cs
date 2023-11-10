﻿using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace Tomat.FNB.Commands.XNB;

[Command("xnb list-formats", Description = "Lists the known formats that can be extracted from XNB files")]
public class XnbListFormatsCommand : ICommand {
    public ValueTask ExecuteAsync(IConsole console) {
        throw new System.NotImplementedException();
    }
}
