using System.Linq;
using System.Reflection;
using System.Xml;
using Bannerlord.UIExtenderEx;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs;
using Bannerlord.UIExtenderEx.ViewModels;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
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

    }

    [PrefabExtension("MapBar", "descendant::ListPanel[@Id='BottomInfoBar']/Children")]
    public class MapBarPatch : PrefabExtensionInsertPatch {

        public sealed override string Id => "NCSStorkButton";
        public override int Position => PositionLast;
        private XmlDocument XmlDocument { get; } = new XmlDocument();
        public override XmlDocument GetPrefabExtension() => XmlDocument;

        public MapBarPatch() {
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

        private int _childrenAmount = 99;
        private string _storkTooltip = "Test Stork";

        [DataSourceProperty]
        public BasicTooltipViewModel NCSStorkChildrenAmountHint => new BasicTooltipViewModel(() => _storkTooltip);

        [DataSourceProperty] public string NCSStorkChildrenAmount => "" + _childrenAmount;

        public MapInfoMixin(MapInfoVM vm) : base(vm) { }
        
    }

    internal static class XmlPathHelper {

        public static string GetXmlPath(string id) => Path.Combine(Utilities.GetBasePath(), "Modules", "NCSStork",
            "GUI", "PrefabExtensions", $"{id}.xml");

    }

}