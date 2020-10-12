using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace NCSStork {

    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("NCSMenuOption", new TextObject("NCS Menu", null), 9990, () =>
            {
                InformationManager.DisplayMessage(new InformationMessage("NCS Menu TODO"));
            }, false));
        }
    }

}