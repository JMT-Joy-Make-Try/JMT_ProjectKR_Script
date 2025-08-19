using JMT.Core;
using JMT.UISystem.Content;
using JMT.UISystem.Deck;
using JMT.UISystem.Shop;
using JMT.UISystem.Tooltip;
using JMT.UISystem.Option;
using UnityEngine;
using JMT.UISystem.ItemPopup;
using JMT.UISystem.Result;
using JMT.UISystem.Message;

namespace JMT.UISystem
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private LogController logCompo;
        [SerializeField] private ShopController repairCompo;
        [SerializeField] private DeckController deckCompo;
        [SerializeField] private TooltipController tooltipCompo;
        [SerializeField] private OptionController optionCompo;
        [SerializeField] private ItemGetController itemPopupCompo;
        [SerializeField] private ResultController resultCompo;
        [SerializeField] private MessageController messageCompo;

        public LogController LogCompo => logCompo;
        public ShopController ShopCompo => repairCompo;
        public DeckController DeckCompo => deckCompo;
        public TooltipController TooltipCompo => tooltipCompo;
        public OptionController OptionCompo => optionCompo;
        public ItemGetController ItemPopupCompo => itemPopupCompo;
        public ResultController ResultCompo => resultCompo;
        public MessageController MessageCompo => messageCompo;
    }
}
