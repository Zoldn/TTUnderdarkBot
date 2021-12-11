using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.AI;

namespace TUnderdark.Model
{
    internal abstract class CardAction
    {
        public Board Board { get; private set; }
        public TurnVariant Variant { get; private set; }
        public Player ActivePlayer { get; private set; }
        public Location SpyLocation { get; private set; }
        public CardAction()
        {
            Board = null;
            Variant = null;
            ActivePlayer = null;
            SpyLocation = null;
        }

        public void SetContext(Board board, TurnVariant variant, Player activePlayer, Location spyLocation)
        {
            Board = board;
            Variant = variant;
            ActivePlayer = activePlayer;
            SpyLocation = spyLocation;
        }

        /// <summary>
        /// Является ли действие простым (добавить ману/мечи, которое можно выполнить сразу),
        /// нет таргета для цели (выбрать игрока, локацию, шпиона...)
        /// </summary>
        public virtual bool IsSimpleAction => true;
        /// <summary>
        /// Проверка, является ли действие легальным в текущем состоянии игры
        /// (Например, вернуть шпиона, когда ни одного своего шпиона не выставлено
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLegal => true;

        /// <summary>
        /// Добавить ману
        /// </summary>
        public virtual int Mana => 0;
        /// <summary>
        /// Добавить мечи
        /// </summary>
        public virtual int Swords => 0;
        /// <summary>
        /// Supplant any troops
        /// </summary>
        public virtual int Supplant => 0;
        /// <summary>
        /// Assasinate only White
        /// </summary>
        public virtual int AssasinateWhite => 0;
        /// <summary>
        /// Supplant only White
        /// </summary>
        public virtual int SupplantWhite => 0;
        public virtual int SupplantWhiteAnywhere => 0;
        public virtual int Assasinate => 0;
        public virtual int Deploy => 0;
        public virtual int MoveTroop => 0;
        /// <summary>
        /// Вернуть труп
        /// </summary>
        public virtual int ReturnTroop => 0;

        /// <summary>
        /// At end turn promote another card played this turn
        /// </summary>
        public virtual int PromoteAnotherPlayedCardAtEndTurn => 0;
        /// <summary>
        /// Промоут из сброса
        /// </summary>
        public virtual int PromoteFromDiscard => 0;
        /// <summary>
        /// Place spy
        /// </summary>
        public virtual int PlaceSpy => 0;
        /// <summary>
        /// Вернуть своего шпиона
        /// </summary>
        public virtual int ReturnOwnSpy => 0;
        /// <summary>
        /// Предпочитаемые локации для выставления шпиона (эффекты при выставлении и уборке)
        /// </summary>
        public virtual List<Location> PreferrableLocationsToPlaceSpy => new List<Location>();
        /// <summary>
        /// Предпочитаемые локации для возвращения шпиона
        /// </summary>
        public virtual List<Location> PreferrableLocationsToRemoveSpy => new List<Location>();
        /// <summary>
        /// Засупплантить в локации шпиона
        /// </summary>
        public virtual int SupplantInSpyLocation => 0;
        /// <summary>
        /// Вернуть вражеского шпиона
        /// </summary>
        public virtual int ReturnEnemySpy => 0;
        /// <summary>
        /// Тянуть дополнительные карты
        /// </summary>
        public virtual int DrawCard => 0;
        /// <summary>
        /// Перенести колоду в дискард
        /// </summary>
        public virtual bool IsMoveDeckToDiscard => false;
        /// <summary>
        /// Сожрать карту на рынке
        /// </summary>
        public virtual int DevoureMarket => 0;
        /// <summary>
        /// Дополнительные победные очки
        /// </summary>
        public virtual int BonusVP => 0;
    }
}
