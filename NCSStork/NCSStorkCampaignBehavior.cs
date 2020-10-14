using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace NCSStork {

    public class NCSStorkCampaignBehavior : CampaignBehaviorBase {

        public override void RegisterEvents() {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener((object) this, new Action<CampaignGameStarter>(this.OnGameLoaded));
        }

        public override void SyncData(IDataStore dataStore) {
        }
        
        private void OnGameLoaded(CampaignGameStarter gameSystemInitializer) {
            Debug.Print("In NCSStorkCampaignBehavior.OnGameLoaded");
            Dictionary<Hero, int> heirs = Hero.MainHero.Clan.GetHeirApparents();
            Debug.Print("Heirs: " + heirs.Count);
        }

    }

}