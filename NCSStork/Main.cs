using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using UIExtenderLib;
using UIExtenderLib.Interface;

namespace NCSStork {

    public class Main : MBSubModuleBase {

        private UIExtender _extender;

        protected override void OnSubModuleLoad() {
            base.OnSubModuleLoad();

            _extender = new UIExtender("NCSStork");
            _extender.Register();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            _extender.Verify();
        }

    }

    [UIExtenderLib.Interface.PrefabExtension("MapBar", "descendant::ListPanel[@Id='BottomInfoBar']/Children")]
    public class PrefabExtension : PrefabExtensionInsertPatch {

        public override int Position => PositionLast;
        public override string Name => "NCSStorkButton";

    }

    [UIExtenderLib.Interface.ViewModelMixin]
    public class ViewModelMixin : BaseViewModelMixin<MapInfoVM> {

        private int _childrenAmount;
        private string _childrenTooltip;

        [DataSourceProperty]
        public BasicTooltipViewModel StorkAmountHint => new BasicTooltipViewModel(() => _childrenTooltip);

        [DataSourceProperty] public string ChildrenAmount => "" + _childrenAmount;

        public ViewModelMixin(MapInfoVM vm) : base(vm) { }

        public override void OnRefresh() {
            // TODO this is counting horses not children
            var children =
                MobileParty.MainParty.ItemRoster.Where(i =>
                    i.EquipmentElement.Item.ItemCategory.Id == new MBGUID(671088673));
            var newTooltip = children.Aggregate("Children: ",
                (s, element) => $"{s}\n{element.EquipmentElement.Item.Name}: {element.Amount}");

            if (newTooltip != _childrenTooltip) {
                _childrenAmount = children.Sum(item => item.Amount);
                _childrenTooltip = newTooltip;

                if (_vm.TryGetTarget(out var vm)) {
                    vm.OnPropertyChanged(nameof(ChildrenAmount));
                }
            }
        }

    }

}