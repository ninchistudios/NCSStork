﻿using System;
using System.Xml;
using Bannerlord.UIExtenderEx;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs;
using Bannerlord.UIExtenderEx.ViewModels;
using SandBox.Quests.QuestBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using Path = System.IO.Path;

namespace NCSStork {

    public class Main : MBSubModuleBase {

        private UIExtender _extender;

        protected override void OnSubModuleLoad() {
            Debug.Print("NCSStork entered OnSubModuleLoad");
            base.OnSubModuleLoad();

            _extender = new UIExtender("NCSStork");
            _extender.Register(typeof(Main).Assembly);
            _extender.Enable();
            Debug.Print("NCSStork enabled UIExtender");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter) {
            base.OnGameStart(game, gameStarter);
            if (game.GameType is Campaign) {
                CampaignGameStarter campaignStarter = (CampaignGameStarter) gameStarter;
                campaignStarter.AddBehavior(new NCSStorkCampaignBehavior());
            }
        }

    }

    [PrefabExtension("MapBar", "descendant::ListPanel[@Id='BottomInfoBar']/Children")]
    public class MapBarPatch : PrefabExtensionInsertPatch {

        public sealed override string Id => "NCSStorkButton";
        public override int Position => PositionLast;
        private XmlDocument XmlDocument { get; } = new XmlDocument();
        public override XmlDocument GetPrefabExtension() => XmlDocument;

        public MapBarPatch() {

            Debug.Print("NCSStork entered MapBarPatch, loading XML: " + XmlPathHelper.GetXmlPath(Id));

            using (XmlReader reader = XmlReader.Create(XmlPathHelper.GetXmlPath(Id),
                new XmlReaderSettings {IgnoreComments = true, IgnoreWhitespace = true})) {
                XmlDocument.Load(reader);
                Debug.Print("NCSStork loaded MapBarPatch");
            }

            Debug.Assert(XmlDocument.HasChildNodes, $"Failed to parse extension ({Id}) XML!");
        }

    }

    [ViewModelMixin]
    public class MapInfoMixin : BaseViewModelMixin<MapInfoVM> {

        private int _childrenAmount = -1;
        private string _storkTooltip = "Children";
        private readonly MapInfoVM _viewModel;

        [DataSourceProperty]
        public BasicTooltipViewModel NCSStorkChildrenAmountHint => new BasicTooltipViewModel(() => _storkTooltip);

        // [DataSourceProperty] public string NCSStorkChildrenAmount => "" + _childrenAmount;
        [DataSourceProperty] 
        public string NCSStorkChildrenAmount {
            get => "" + this._childrenAmount;
            
        }

        public MapInfoMixin(MapInfoVM vm) : base(vm) {
            Debug.Print("NCS: In MapInfoMixin constructor");
            if (ViewModel != null) {
                _viewModel = ViewModel;
            } else {
                throw new NullReferenceException("MapInfoVM ViewModel is null");
            }
        }

        public override void OnRefresh() {
            var childcount = Hero.MainHero.Children.Count;
            if (childcount != _childrenAmount) {
                Debug.Print("NCS: updating child count");
                _childrenAmount = childcount;
                _viewModel.OnPropertyChanged(nameof(NCSStorkChildrenAmount));
            }
        }

    }

    internal static class XmlPathHelper {

        public static string GetXmlPath(string id) => Path.Combine(Utilities.GetBasePath(), "Modules", "NCSStork",
            "GUI", "PrefabExtensions", $"{id}.xml");

    }

}