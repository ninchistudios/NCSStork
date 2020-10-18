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
            Debug.Print("NCS: In NCSStorkCampaignBehavior.OnGameLoaded");
            Dictionary<Hero, int> heirs = Hero.MainHero.Clan.GetHeirApparents();
            Debug.Print("NCS: Heirs: " + heirs.Count);
            foreach (KeyValuePair<Hero, int> heir in heirs) {
                Debug.Print("NCS: heir: " + heir.Key.Name);
            }
            List<Hero> children = Hero.MainHero.Children;
            Debug.Print("NCS: Children: " + children.Count);
            foreach (Hero child in children) {
                Debug.Print("NCS: child: " + child.Name);
            }
            
        }

    }

}